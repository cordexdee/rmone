using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace uGovernIT.Web
{
    public class ServiceExportHelper
    {
        ApplicationContext _spWeb;
        List<string> keyColl = new List<string>() {
                "userquestion",
                "locationuserquestion",
                "departmentuserquestion",
                "dependentlocationquestion",
                "companydivisions",
                "MirrorAccessFrom",
                "ExistingUser",
                "NewUSer",
                "validateagainst"
            };

        public ServiceExportHelper(ApplicationContext spWeb)
        {
            _spWeb = spWeb;
        }

        private void CreateUserLookupList(ref List<UserInfo> lstUserLookUp, PropertyInfo item, object value)
        {
            if (value != null)
            {
                if (item.PropertyType == typeof(SPFieldUserValueCollection))
                {
                    SPFieldUserValueCollection users = (SPFieldUserValueCollection)value;
                    foreach (SPFieldUserValue lookup in users)
                    {
                        if (lookup.LookupId > 0 && !string.IsNullOrEmpty(lookup.LookupValue))
                            InsertUserValue(lookup, lstUserLookUp);
                    }
                }
                else if (item.PropertyType == typeof(SPFieldUserValue))
                {
                    SPFieldUserValue lookup = (SPFieldUserValue)value;
                    if (lookup.LookupId > 0 && !string.IsNullOrEmpty(lookup.LookupValue))
                        InsertUserValue(lookup, lstUserLookUp);
                }
            }
        }

        private void InsertLookupValue(SPFieldLookupValue lookup, ref List<LookupValueServiceExtension> lstLookUp, string listName, string fieldName)
        {
            var itemExist = lstLookUp.FirstOrDefault(c => c.ID == lookup.LookupId && c.Value == lookup.LookupValue);
            if (itemExist == null)
            {
                SPListItem spListItem = SPListHelper.GetSPListItem(listName, lookup.LookupId);
                if (spListItem != null && !string.IsNullOrEmpty(Convert.ToString(spListItem[fieldName])))
                {
                    LookupValueServiceExtension lookupValue = new LookupValueServiceExtension(lookup.LookupId, Convert.ToString(spListItem[fieldName]), listName);
                    lstLookUp.Add(lookupValue);
                }
            }
        }

        private void InsertUserValue(SPFieldUserValue lookup, List<UserInfo> lstUserLookUp)
        {
            var itemExist = lstUserLookUp.FirstOrDefault(c => c.ID == lookup.LookupId && c.Name == lookup.LookupValue);
            if (itemExist == null)
            {
                UserProfile profile = UserProfile.LoadById(lookup.LookupId);
                if (profile != null)
                {
                    UserInfo lookupValue = new UserInfo(profile.ID, profile.Name);
                    lstUserLookUp.Add(lookupValue);
                }
                SPGroup group = UserProfile.GetGroupByID(lookup.LookupId);
                if (group != null)
                {
                    UserInfo lookupValue = new UserInfo(lookup.LookupId, lookup.LookupValue, true);
                    lstUserLookUp.Add(lookupValue);
                }
            }
        }

        private void CreatelookupValuesList(ref List<LookupValueServiceExtension> lstLookUp, PropertyInfo item, object value)
        {
            if (value != null)
            {
                string itemName = item.Name;
                string listName = string.Empty;
                string fieldName = string.Empty;
                bool isUpdatelist = false;
                if (itemName.ToLower() == "requestcategory")
                {
                    listName = DatabaseObjects.Lists.RequestType;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "module")
                {
                    listName = DatabaseObjects.Lists.Modules;
                    fieldName = DatabaseObjects.Columns.ModuleName;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "predecessors")
                {
                    listName = DatabaseObjects.Lists.ServiceTicketRelationships;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "service")
                {
                    listName = DatabaseObjects.Lists.Services;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                if (isUpdatelist)
                {
                    if (item.PropertyType == typeof(SPFieldLookupValueCollection))
                    {
                        try
                        {
                            SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection(Convert.ToString(value));
                            foreach (SPFieldLookupValue lookup in lookups)
                            {
                                if (lookup.LookupId > 0 && !string.IsNullOrEmpty(lookup.LookupValue))
                                    InsertLookupValue(lookup, ref lstLookUp, listName, fieldName);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex, "Error exporting " + itemName);
                        }
                    }
                    else if (item.PropertyType == typeof(SPFieldLookupValue))
                    {
                        try
                        {
                            SPFieldLookupValue lookup = new SPFieldLookupValue(Convert.ToString(value));
                            if (lookup.LookupId > 0 && !string.IsNullOrEmpty(lookup.LookupValue))
                                InsertLookupValue(lookup, ref lstLookUp, listName, fieldName);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex, "Error exporting " + itemName);
                        }

                    }
                }
            }
        }

        private void ReplaceIdWithToken(Services service, List<string> keyColl)
        {
            foreach (string key in keyColl)
            {
                service.Questions.Where(x => x.QuestionTypeProperties.ContainsKey(key)).ToList().ForEach(y =>
                {
                    Dictionary<string, string> dic = y.QuestionTypeProperties;
                    if (dic != null)
                    {
                        string val = dic[key];
                        int valId = 0;
                        int.TryParse(val, out valId);
                        if (valId > 0)
                        {
                            ServiceQuestion ques = service.Questions.Where(z => z.ID == valId).FirstOrDefault();
                            if (ques != null)
                                y.QuestionTypeProperties[key] = ques.TokenName.ToLower();
                        }
                    }
                });
            }
        }

        public ServiceExtension GetServiceExtension(Services service)
        {

            if (service.Questions != null && service.Questions.Count > 0)
                ReplaceIdWithToken(service, keyColl);

            ServiceExtension serviceExtension = new ServiceExtension(service);
            List<LookupValueServiceExtension> lstLookUp = serviceExtension.LookupValues;
            List<UserInfo> lstUserLookUp = serviceExtension.UserInfo;

            #region service properties
            //fix service question and user fields
            service.CustomProperties = null;
            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty("AuthorizedToView"), serviceExtension.AuthorizedToView);
            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty("Owners"), serviceExtension.Owners);
            #endregion

            #region Questions
            DataTable dtApplications = null;
            //fix service question lookup and user fields
            foreach (ServiceQuestion ques in serviceExtension.Questions)
            {
                if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess)
                {
                    if (dtApplications == null)
                        dtApplications = SPListHelper.GetDataTable(DatabaseObjects.Lists.Applications);

                    string sfOptions = string.Empty;
                    ques.QuestionTypeProperties.TryGetValue("application", out sfOptions);
                    if (sfOptions != null && sfOptions.ToLower() != "all")
                    {
                        List<string> sfOptionList = uHelper.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                        List<string> applications = new List<string>();
                        foreach (string item in sfOptionList)
                        {
                            string applicationid = uHelper.SplitString(item, Constants.Separator2, 1);
                            if (applicationid != null)
                                applicationid = uHelper.SplitString(applicationid, "-", 1);
                            if (applicationid != null)
                                applicationid = uHelper.SplitString(applicationid, ";", 0);

                            string applicationName = item;
                            if (dtApplications != null && dtApplications.Rows.Count > 0)
                            {
                                DataRow[] drApps = dtApplications.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, applicationid));
                                if (drApps.Length > 0)
                                {
                                    applicationName = item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0] + Constants.Separator2 + Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + Convert.ToString(drApps[0][DatabaseObjects.Columns.Id]);
                                    applications.Add(applicationName);

                                    string appTitle = Convert.ToString(drApps[0][DatabaseObjects.Columns.Title]);
                                    LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(uHelper.StringToInt(applicationid), appTitle, DatabaseObjects.Lists.Applications);
                                    lookupExtension.AddIn(lstLookUp);
                                }
                            }
                        }
                        ques.QuestionTypeProperties["application"] = string.Join(Constants.Separator1, applications.ToArray());
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                {
                    if (ques.QuestionTypeProperties.ContainsKey("requesttypes") && ques.QuestionTypeProperties.ContainsKey("module"))
                    {
                        string requestTypes = ques.QuestionTypeProperties["requesttypes"];
                        string module = ques.QuestionTypeProperties["module"];
                        if (!string.IsNullOrEmpty(requestTypes) && !string.IsNullOrEmpty(module))
                        {
                            List<string> requestTypeIDs = uHelper.SplitString(requestTypes, "~").ToList();
                            UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, module);
                            List<string> validRequestTypeIDs = new List<string>();

                            foreach (string reqId in requestTypeIDs)
                            {
                                ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID.ToString() == reqId);
                                if (moduleRequestType == null)
                                {
                                    Log.WriteLog("ERROR: Missing request type in with ID of [$" + reqId + "$] for service: " + ques.QuestionTitle);
                                    continue;
                                }

                                validRequestTypeIDs.Add(reqId);

                                string requestType = (moduleRequestType.Category + ">" + moduleRequestType.SubCategory + ">" + moduleRequestType.RequestType);
                                LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(moduleRequestType.ID, requestType, DatabaseObjects.Lists.RequestType);
                                lookupExtension.AddIn(lstLookUp);
                            }
                            ques.QuestionTypeProperties["requesttypes"] = string.Join("~", validRequestTypeIDs);
                        }
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                {
                    if (ques.QuestionTypeProperties.ContainsKey("assettype"))
                    {
                        string requestTypes = ques.QuestionTypeProperties["assettype"];
                        string module = "CMDB";
                        if (!string.IsNullOrEmpty(requestTypes) && !string.IsNullOrEmpty(module))
                        {
                            List<string> requestTypeIDs = uHelper.SplitString(requestTypes, Constants.Separator6).ToList();
                            UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, module);
                            List<string> validRequestTypeIDs = new List<string>();

                            foreach (string reqId in requestTypeIDs)
                            {
                                ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID.ToString() == reqId);
                                if (moduleRequestType == null)
                                {
                                    Log.WriteLog("ERROR: Missing request type in with ID of [$" + reqId + "$] for service: " + ques.QuestionTitle);
                                    continue;
                                }

                                validRequestTypeIDs.Add(reqId);

                                string requestType = (moduleRequestType.Category + Constants.Separator9 + moduleRequestType.SubCategory + Constants.Separator9 + moduleRequestType.RequestType);
                                LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(moduleRequestType.ID, requestType, DatabaseObjects.Lists.RequestType);
                                lookupExtension.AddIn(lstLookUp);
                            }
                            ques.QuestionTypeProperties["assettype"] = string.Join(Constants.Separator6, validRequestTypeIDs);
                        }
                    }

                    if (ques.QuestionTypeProperties.ContainsKey("specificuser"))
                    {
                        string userValue = ques.QuestionTypeProperties["specificuser"];
                        string[] userValueList = userValue.Split(',');
                        if (userValueList.Count() > 0)
                        {
                            List<string> validUsers = new List<string>();
                            foreach (string uValue in userValueList)
                            {
                                UserProfile userProfile = UserProfile.LoadByLoginName(uValue);
                                if (userProfile != null)
                                {
                                    validUsers.Add(userProfile.LoginName);

                                    InsertUserValue(new SPFieldUserValue(_spWeb, userProfile.ID, userProfile.Name), lstUserLookUp);
                                }
                            }

                            ques.QuestionTypeProperties["specificuser"] = string.Join(Constants.Separator6, validUsers);
                        }
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    if (ques.QuestionTypeProperties.ContainsKey("usertype"))
                    {
                        List<string> validUserList = new List<string>();
                        string userValue = string.Empty;
                        string key = string.Empty;
                        if (ques.QuestionTypeProperties.ContainsKey("defaultval"))
                        {
                            userValue = ques.QuestionTypeProperties["defaultval"];
                            key = "defaultval";
                        }
                        else if (ques.QuestionTypeProperties.ContainsKey("specificusergroup"))
                        {
                            key = "specificusergroup";
                            userValue = ques.QuestionTypeProperties["specificusergroup"];
                        }

                        if (string.IsNullOrWhiteSpace(userValue))
                            continue;

                        string[] userValueList = userValue.Split(',');
                        if (userValueList.Count() > 0)
                        {
                            foreach (string uValue in userValueList)
                            {
                                //find first with exact match
                                UserProfile userProfile = UserProfile.LoadByLoginName(uValue);
                                if (userProfile != null)
                                {
                                    validUserList.Add(userProfile.LoginName);

                                    InsertUserValue(new SPFieldUserValue(_spWeb, userProfile.ID, userProfile.Name), lstUserLookUp);
                                }
                            }
                            ques.QuestionTypeProperties["key"] = string.Join(Constants.Separator6, validUserList);
                        }
                    }
                }
            }
            #endregion

            #region Tasks
            //fix task lookup and user fields
            foreach (ServiceTask item in serviceExtension.Tasks)
            {
                if (item.RequestCategory != null && item.RequestCategory.LookupId > 0 && item.Module != null)
                {
                    string module = item.Module.LookupValue;
                    UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, module);

                    int requestID = item.RequestCategory.LookupId;

                    ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID == requestID);
                    if (moduleRequestType != null)
                    {
                        string requestType = (moduleRequestType.Category + ">" + moduleRequestType.SubCategory + ">" + moduleRequestType.RequestType);
                        LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(moduleRequestType.ID, requestType, DatabaseObjects.Lists.RequestType);
                        lookupExtension.AddIn(lstLookUp);
                    }
                }

                List<PropertyInfo> properties = item.GetType().GetProperties().Where(c => c.Name != DatabaseObjects.Columns.RequestCategory && (c.PropertyType == typeof(SPFieldLookupValueCollection) || c.PropertyType == typeof(SPFieldLookupValue))).ToList();
                foreach (PropertyInfo prop in properties)
                {
                    var value = item.GetType().GetProperty(prop.Name).GetValue(item, null);
                    CreatelookupValuesList(ref lstLookUp, prop, value);
                }

                List<PropertyInfo> propertiesUser = item.GetType().GetProperties().Where(c => c.PropertyType == typeof(SPFieldUserValueCollection) || c.PropertyType == typeof(SPFieldUserValue)).ToList();
                foreach (PropertyInfo prop in propertiesUser)
                {
                    var value = item.GetType().GetProperty(prop.Name).GetValue(item, null);
                    CreateUserLookupList(ref lstUserLookUp, prop, value);
                }
            }
            #endregion

            #region Skip conditions

            if (service.SkipSectionCondition != null && service.SkipSectionCondition.Count > 0)
            {
                foreach (ServiceSectionCondition taskCondition in service.SkipSectionCondition)
                {
                    if (taskCondition.Conditions != null && taskCondition.Conditions.Count > 0)
                    {
                        List<WhereExpression> lstWhereExpression = taskCondition.Conditions;
                        foreach (WhereExpression expression in lstWhereExpression)
                        {
                            ServiceQuestion question = service.Questions.Where(c => c.TokenName == expression.Variable).FirstOrDefault();
                            if (question == null)
                            {
                                Log.WriteLog("ERROR: Missing question in skip logic with token [$" + expression.Variable + "$] for service: " + service.Title);
                                continue;
                            }

                            if (question.QuestionType.ToLower() == "userfield")
                            {
                                try
                                {
                                    SPFieldUserValue lookup = new SPFieldUserValue(_spWeb, expression.Value);
                                    if (lookup != null && lookup.User != null && lookup.LookupId > 0)
                                        InsertUserValue(lookup, lstUserLookUp);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                            if (question.QuestionType.ToLower() == "requestCategory")
                            {
                                try
                                {
                                    SPFieldLookupValue lookupColl = new SPFieldLookupValue(expression.Value);
                                    if (lookupColl != null && lookupColl.LookupId > 0)
                                    {
                                        InsertLookupValue(lookupColl, ref lstLookUp, DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }

                            }
                            if (question.QuestionType.ToLower() == "lookup")
                            {
                                try
                                {
                                    SPFieldLookupValue lookupColl = new SPFieldLookupValue(expression.Value);
                                    if (lookupColl != null && lookupColl.LookupId > 0)
                                    {
                                        if (question.QuestionTypeProperties != null && question.QuestionTypeProperties.Count > 0)
                                        {
                                            string listName = question.QuestionTypeProperties["lookuplist"];
                                            string fieldName = question.QuestionTypeProperties["lookupfield"];

                                            InsertLookupValue(lookupColl, ref lstLookUp, listName, fieldName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                        }

                    }
                }
            }

            if (service.SkipTaskCondition != null && service.SkipTaskCondition.Count > 0)
            {
                foreach (ServiceTaskCondition taskCondition in service.SkipTaskCondition)
                {
                    if (taskCondition.Conditions != null && taskCondition.Conditions.Count > 0)
                    {
                        List<WhereExpression> lstWhereExpression = taskCondition.Conditions;
                        foreach (WhereExpression expression in lstWhereExpression)
                        {
                            ServiceQuestion question = service.Questions.Where(c => c.TokenName == expression.Variable).FirstOrDefault();
                            if (question == null)
                            {
                                Log.WriteLog("ERROR: Missing question in skip logic with token [$" + expression.Variable + "$] for service: " + service.Title);
                                continue;
                            }

                            if (question.QuestionType.ToLower() == "userfield")
                            {
                                try
                                {
                                    SPFieldUserValue lookup = new SPFieldUserValue(_spWeb, expression.Value);
                                    if (lookup != null && lookup.User != null && lookup.LookupId > 0)
                                        InsertUserValue(lookup, lstUserLookUp);
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                            if (question.QuestionType.ToLower() == "requestCategory")
                            {
                                try
                                {
                                    SPFieldLookupValue lookupColl = new SPFieldLookupValue(expression.Value);
                                    if (lookupColl != null && lookupColl.LookupId > 0)
                                    {
                                        InsertLookupValue(lookupColl, ref lstLookUp, DatabaseObjects.Lists.RequestType, DatabaseObjects.Columns.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }

                            }
                            if (question.QuestionType.ToLower() == "lookup")
                            {
                                try
                                {
                                    SPFieldLookupValue lookupColl = new SPFieldLookupValue(expression.Value);
                                    if (lookupColl != null && lookupColl.LookupId > 0)
                                    {
                                        if (question.QuestionTypeProperties != null && question.QuestionTypeProperties.Count > 0)
                                        {
                                            string listName = question.QuestionTypeProperties["lookuplist"];
                                            string fieldName = question.QuestionTypeProperties["lookupfield"];
                                            InsertLookupValue(lookupColl, ref lstLookUp, listName, fieldName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Variables

            if (service.QMapVariables != null)
            {
                //correctly export user type of variables
                List<QuestionMapVariable> mapVariables = service.QMapVariables.Where(x => x.Type == Constants.ServiceQuestionType.USERFIELD).ToList();
                foreach (QuestionMapVariable mVariable in mapVariables)
                {
                    if (mVariable.DefaultValue != null && mVariable.DefaultValue.IsPickFromConstant && !string.IsNullOrWhiteSpace(mVariable.DefaultValue.PickFrom))
                    {
                        SPFieldUserValue uValLookup = new SPFieldUserValue(_spWeb, mVariable.DefaultValue.PickFrom);
                        if (uValLookup != null)
                            InsertUserValue(uValLookup, lstUserLookUp);
                    }

                    if (mVariable.VariableValues != null)
                    {
                        foreach (VariableValue v in mVariable.VariableValues)
                        {
                            if (v.IsPickFromConstant && !string.IsNullOrWhiteSpace(v.PickFrom))
                            {
                                SPFieldUserValue uValLookup = new SPFieldUserValue(_spWeb, v.PickFrom);
                                if (uValLookup != null)
                                    InsertUserValue(uValLookup, lstUserLookUp);
                            }

                            if (v.Conditions != null)
                            {
                                foreach (WhereExpression exp in v.Conditions)
                                {
                                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == exp.Variable.ToLower());
                                    if (question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                                    {
                                        UserProfile user = UserProfile.LoadById(uHelper.StringToInt(exp.Value), _spWeb);
                                        if (user != null)
                                        {
                                            InsertUserValue(new SPFieldUserValue(_spWeb, user.ID, user.Name), lstUserLookUp);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion


            #region mapping
            ExportServiceQuestionMapping(serviceExtension);
            #endregion

            serviceExtension.LookupValues = lstLookUp;
            serviceExtension.UserInfo = lstUserLookUp;

            return serviceExtension;
        }

        private void ExportServiceQuestionMapping(ServiceExtension serviceExtension)
        {
            if (serviceExtension.QuestionsMapping == null)
                return;

            var mapLookup = serviceExtension.QuestionsMapping.ToLookup(x => x.ServiceTaskID);
            foreach (var map in mapLookup)
            {
                List<ServiceQuestionMapping> taskMaps = map.ToList();
                int taskID = map.Key;
                string moduleName = string.Empty;
                if (taskID > 0)
                {
                    ServiceTask task = serviceExtension.Tasks.FirstOrDefault(x => x.ID == taskID);
                    if (task != null)
                    {
                        if (task.Module != null)
                            moduleName = task.Module.LookupValue;
                        else
                        {
                            moduleName = "_Task";
                        }
                    }
                }
                else
                    moduleName = "SVC";

                //Not found any mapping
                if (string.IsNullOrWhiteSpace(moduleName))
                {
                    Log.WriteLog(string.Format("module name found while looking into module"), "ExportServiceQuestionMapping");
                    continue;
                }


                if (moduleName == "_Task") // map is against task
                {
                    ServiceQuestionMapping qmap = taskMaps.FirstOrDefault(x => x.ColumnName == DatabaseObjects.Columns.AssignedTo);
                    if (qmap != null && !string.IsNullOrWhiteSpace(qmap.ColumnName) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                    {
                        SPFieldUserValueCollection userLookups = new SPFieldUserValueCollection(_spWeb, Convert.ToString(qmap.ColumnValue));
                        if (userLookups != null)
                        {
                            foreach (SPFieldUserValue uVal in userLookups)
                            {
                                InsertUserValue(uVal, serviceExtension.UserInfo);
                            }
                        }
                    }
                }
                else //mapping is against ticket or svc so export user correctly for it
                {
                    UGITModule module = uGITCache.ModuleConfigCache.GetCachedModule(_spWeb, moduleName);
                    if (module == null)
                    {
                        Log.WriteLog(string.Format("module not found while looking into module from database"), "ExportServiceQuestionMapping");
                        continue;
                    }

                    SPList moduleDataList = SPListHelper.GetSPList(module.ModuleTicketTable, _spWeb);
                    if (moduleDataList == null)
                    {
                        Log.WriteLog(string.Format("Module data list not found"), "ExportServiceQuestionMapping");
                        continue;
                    }

                    foreach (ServiceQuestionMapping qmap in taskMaps)
                    {
                        SPField spfield = SPListHelper.GetSPField(moduleDataList, qmap.ColumnName);
                        if (spfield != null && spfield.Type == SPFieldType.User && !string.IsNullOrWhiteSpace(qmap.ColumnValue) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                        {
                            SPFieldUserValueCollection userLookups = new SPFieldUserValueCollection(_spWeb, Convert.ToString(qmap.ColumnValue));
                            if (userLookups != null)
                            {
                                foreach (SPFieldUserValue uVal in userLookups)
                                {
                                    InsertUserValue(uVal, serviceExtension.UserInfo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
