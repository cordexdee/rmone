using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.Helpers
{
    public class ServiceExportHelper
    {
        ApplicationContext _spWeb;
        ModuleViewManager moduleViewManager;
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
            moduleViewManager = new ModuleViewManager(_spWeb);
        }

        private void CreateUserLookupList(ref List<UserProfile> lstUserLookUp, PropertyInfo item, object value)
        {
            if (value != null)
            {
                if (item.PropertyType == typeof(string))
                {
                    List<UserProfile> users =_spWeb.UserManager.GetUserInfosById(Convert.ToString(value));
                    foreach (UserProfile lookup in users)
                    {
                        if (lookup != null)
                            InsertUserValue(lookup.Id, ref lstUserLookUp);
                    }
                }
            }
        }

        private void InsertLookupValue(string lookup, ref List<LookupValueServiceExtension> lstLookUp, string listName, string fieldName)
        {
            var itemExist = lstLookUp.FirstOrDefault(c => c.ID == lookup);
            if (itemExist == null)
            {
                LookupValueServiceExtension lookupValue = new LookupValueServiceExtension(lookup, fieldName, listName);
                lstLookUp.Add(lookupValue);
            }
        }

        private void InsertUserValue(string lookup,ref List<UserProfile> lstUserLookUp)
        {
            var itemExist = lstUserLookUp.FirstOrDefault(c => c.Id!=null && c.Id == lookup);
            if (itemExist == null)
            {
                UserProfile profile =_spWeb.UserManager.LoadById(lookup);
                if (profile != null)
                {
                    lstUserLookUp.Add(profile);
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
                    listName = DatabaseObjects.Tables.RequestType;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "module")
                {
                    listName = DatabaseObjects.Tables.Modules;
                    fieldName = DatabaseObjects.Columns.ModuleName;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "predecessors")
                {
                    listName = DatabaseObjects.Tables.ModuleTasks;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "service")
                {
                    listName = DatabaseObjects.Tables.Services;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                if (isUpdatelist)
                {
                    if (item.PropertyType == typeof(string))
                    {
                        try
                        {
                            List<string> lookups = UGITUtility.ConvertStringToList(Convert.ToString(value), Constants.Separator6);
                            foreach (string lookup in lookups)
                            {
                                if (!string.IsNullOrEmpty(lookup))
                                    InsertLookupValue(lookup, ref lstLookUp, listName, fieldName);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error exporting " + itemName);
                        }
                    }
                }
            }
        }

        private void ReplaceIdWithToken(Services service, List<string> keyColl)
        {
            foreach (string key in keyColl)
            {
                service.Questions.Where(x => x.QuestionTypePropertiesDicObj.ContainsKey(key)).ToList().ForEach(y =>
                {
                    Dictionary<string, string> dic = y.QuestionTypePropertiesDicObj;
                    if (dic != null)
                    {
                        string val = dic[key];
                        int valId = 0;
                        int.TryParse(val, out valId);
                        if (valId > 0)
                        {
                            ServiceQuestion ques = service.Questions.Where(z => z.ID == valId).FirstOrDefault();
                            if (ques != null)
                                y.QuestionTypePropertiesDicObj[key] = ques.TokenName.ToLower();
                        }
                    }
                });
            }
        }

        public ServiceExtension GetServiceExtension(Services service,DataTable dtApplication)
        {

            if (service.Questions != null && service.Questions.Count > 0)
                ReplaceIdWithToken(service, keyColl);

            ServiceExtension serviceExtension = new ServiceExtension(service);
            List<LookupValueServiceExtension> lstLookUp = serviceExtension.LookupValues??new List<LookupValueServiceExtension>();
            List<UserProfile> lstUserLookUp = serviceExtension.UserInfo??new List<UserProfile>();
            #region service properties
            //fix service question and user fields
            service.CustomProperties = null;
            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty(DatabaseObjects.Columns.AuthorizedToView), serviceExtension.AuthorizedToView);
            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty(DatabaseObjects.Columns.Owner), serviceExtension.OwnerUser);
            #endregion

            #region Questions
            DataTable dtApplications = dtApplication;
            //fix service question lookup and user fields
            foreach (ServiceQuestion ques in serviceExtension.Questions)
            {
                if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess)
                {
                    if (dtApplications == null)
                        dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{_spWeb.TenantID}'");

                    string sfOptions = string.Empty;
                    ques.QuestionTypePropertiesDicObj.TryGetValue("application", out sfOptions);
                    if (sfOptions != null && sfOptions.ToLower() != "all")
                    {
                        List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                        List<string> applications = new List<string>();
                        foreach (string item in sfOptionList)
                        {
                            string applicationid = UGITUtility.SplitString(item, Constants.Separator2, 1);
                            if (applicationid != null)
                                applicationid = UGITUtility.SplitString(applicationid, "-", 1);
                            if (applicationid != null)
                                applicationid = UGITUtility.SplitString(applicationid, ";", 0);

                            string applicationName = item;
                            if (dtApplications != null && dtApplications.Rows.Count > 0)
                            {
                                DataRow[] drApps = dtApplications.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, applicationid));
                                if (drApps.Length > 0)
                                {
                                    applicationName = item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0] + Constants.Separator2 + Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + Convert.ToString(drApps[0][DatabaseObjects.Columns.Id]);
                                    applications.Add(applicationName);

                                    string appTitle = Convert.ToString(drApps[0][DatabaseObjects.Columns.Title]);
                                    LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(applicationid, appTitle, DatabaseObjects.Tables.Applications);
                                    lookupExtension.AddIn(lstLookUp);
                                }
                            }
                        }
                        ques.QuestionTypePropertiesDicObj["application"] = string.Join(Constants.Separator1, applications.ToArray());
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                {
                    if (ques.QuestionTypePropertiesDicObj.ContainsKey("requesttypes") && ques.QuestionTypePropertiesDicObj.ContainsKey("module"))
                    {
                        string requestTypes = ques.QuestionTypePropertiesDicObj["requesttypes"];
                        string module = ques.QuestionTypePropertiesDicObj["module"];
                        if (!string.IsNullOrEmpty(requestTypes) && !string.IsNullOrEmpty(module))
                        {
                            List<string> requestTypeIDs = UGITUtility.SplitString(requestTypes, "~").ToList();
                            UGITModule moduleObj = moduleViewManager.LoadByName(module);
                            List<string> validRequestTypeIDs = new List<string>();

                            foreach (string reqId in requestTypeIDs)
                            {
                                ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID.ToString() == reqId);
                                if (moduleRequestType == null)
                                {

                                    ULog.WriteLog("ERROR: Missing request type in with ID of [$" + reqId + "$] for service: " + ques.QuestionTitle);
                                    continue;
                                }

                                validRequestTypeIDs.Add(reqId);

                                string requestType = (moduleRequestType.Category + ">" + moduleRequestType.SubCategory + ">" + moduleRequestType.RequestType);
                                LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(UGITUtility.ObjectToString(moduleRequestType.ID), requestType, DatabaseObjects.Tables.RequestType);
                                lookupExtension.AddIn(lstLookUp);
                            }
                            ques.QuestionTypePropertiesDicObj["requesttypes"] = string.Join("~", validRequestTypeIDs);
                        }
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                {
                    if (ques.QuestionTypePropertiesDicObj.ContainsKey("assettype"))
                    {
                        string requestTypes = ques.QuestionTypePropertiesDicObj["assettype"];
                        string module = "CMDB";
                        if (!string.IsNullOrEmpty(requestTypes) && !string.IsNullOrEmpty(module))
                        {
                            List<string> requestTypeIDs = UGITUtility.SplitString(requestTypes, Constants.Separator6).ToList();
                            UGITModule moduleObj = moduleViewManager.LoadByName(module);
                            List<string> validRequestTypeIDs = new List<string>();

                            foreach (string reqId in requestTypeIDs)
                            {
                                ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID.ToString() == reqId);
                                if (moduleRequestType == null)
                                {
                                    ULog.WriteLog("ERROR: Missing request type in with ID of [$" + reqId + "$] for service: " + ques.QuestionTitle);
                                    continue;
                                }

                                validRequestTypeIDs.Add(reqId);

                                string requestType = (moduleRequestType.Category + Constants.Separator9 + moduleRequestType.SubCategory + Constants.Separator9 + moduleRequestType.RequestType);
                                LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(UGITUtility.ObjectToString(moduleRequestType.ID), requestType, DatabaseObjects.Tables.RequestType);
                                lookupExtension.AddIn(lstLookUp);
                            }
                            ques.QuestionTypePropertiesDicObj["assettype"] = string.Join(Constants.Separator6, validRequestTypeIDs);
                        }
                    }

                    if (ques.QuestionTypePropertiesDicObj.ContainsKey("specificuser"))
                    {
                        string userValue = ques.QuestionTypePropertiesDicObj["specificuser"];
                        string[] userValueList = userValue.Split(',');
                        if (userValueList.Count() > 0)
                        {
                            List<string> validUsers = new List<string>();
                            foreach (string uValue in userValueList)
                            {
                                UserProfile userProfile =_spWeb.UserManager.GetUserById(uValue);
                                if (userProfile != null)
                                {
                                    validUsers.Add(userProfile.Id);

                                    InsertUserValue(userProfile.Id,ref lstUserLookUp);
                                }
                            }

                            ques.QuestionTypePropertiesDicObj["specificuser"] = string.Join(Constants.Separator6, validUsers);
                        }
                    }
                }
                else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    if (ques.QuestionTypePropertiesDicObj.ContainsKey("usertype"))
                    {
                        List<string> validUserList = new List<string>();
                        string userValue = string.Empty;
                        string key = string.Empty;
                        if (ques.QuestionTypePropertiesDicObj.ContainsKey("defaultval"))
                        {
                            userValue = ques.QuestionTypePropertiesDicObj["defaultval"];
                            key = "defaultval";
                        }
                        else if (ques.QuestionTypePropertiesDicObj.ContainsKey("specificusergroup"))
                        {
                            key = "specificusergroup";
                            userValue = ques.QuestionTypePropertiesDicObj["specificusergroup"];
                        }

                        if (string.IsNullOrWhiteSpace(userValue))
                            continue;

                        string[] userValueList = userValue.Split(',');
                        if (userValueList.Count() > 0)
                        {
                            foreach (string uValue in userValueList)
                            {
                                //find first with exact match
                                UserProfile userProfile =_spWeb.UserManager.GetUserById(uValue);
                                if (userProfile != null)
                                {
                                    validUserList.Add(userProfile.Id);

                                    InsertUserValue(userProfile.Id,ref lstUserLookUp);
                                }
                            }
                            ques.QuestionTypePropertiesDicObj["key"] = string.Join(Constants.Separator6, validUserList);
                        }
                    }
                }
            }
            #endregion

            #region Tasks
            //fix task lookup and user fields
            foreach (UGITTask item in serviceExtension.Tasks)
            {
                if (!string.IsNullOrEmpty(item.RequestTypeCategory) && !string.IsNullOrWhiteSpace(item.RelatedModule))
                {
                    string module = item.RelatedModule;
                    UGITModule moduleObj = moduleViewManager.LoadByName(module);

                    int requestID =UGITUtility.StringToInt(item.RequestTypeCategory);

                    ModuleRequestType moduleRequestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.ID == requestID);
                    if (moduleRequestType != null)
                    {
                        string requestType = (moduleRequestType.Category + ">" + moduleRequestType.SubCategory + ">" + moduleRequestType.RequestType);
                        LookupValueServiceExtension lookupExtension = new LookupValueServiceExtension(UGITUtility.ObjectToString(moduleRequestType.ID), requestType, DatabaseObjects.Tables.RequestType);
                        lookupExtension.AddIn(lstLookUp);
                    }
                }

                List<PropertyInfo> properties = item.GetType().GetProperties().Where(c => c.Name != DatabaseObjects.Columns.TicketRequestTypeCategory && c.Name.EndsWith("lookup",StringComparison.InvariantCultureIgnoreCase)).ToList();
                foreach (PropertyInfo prop in properties)
                {
                    var value = item.GetType().GetProperty(prop.Name).GetValue(item, null);
                    CreatelookupValuesList(ref lstLookUp, prop, value);
                }

                List<PropertyInfo> propertiesUser = item.GetType().GetProperties().Where(c => c.Name.EndsWith("user", StringComparison.InvariantCultureIgnoreCase)).ToList();
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
                                ULog.WriteLog("ERROR: Missing question in skip logic with token [$" + expression.Variable + "$] for service: " + service.Title);
                                continue;
                            }

                            if (question.QuestionType.ToLower() == "userfield")
                            {
                                try
                                {
                                    string lookup =  expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookup))
                                        InsertUserValue(lookup, ref lstUserLookUp);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                            if (question.QuestionType.ToLower() == "requestCategory")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookupColl))
                                    {
                                        InsertLookupValue(lookupColl, ref lstLookUp, DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }

                            }
                            if (question.QuestionType.ToLower() == "lookup")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookupColl))
                                    {
                                        if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                                        {
                                            string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                                            string fieldName = question.QuestionTypePropertiesDicObj["lookupfield"];

                                            InsertLookupValue(lookupColl, ref lstLookUp, listName, fieldName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
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
                                ULog.WriteLog("ERROR: Missing question in skip logic with token [$" + expression.Variable + "$] for service: " + service.Title);
                                continue;
                            }

                            if (question.QuestionType.ToLower() == "userfield")
                            {
                                try
                                {
                                    string lookup =  expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookup))
                                        InsertUserValue(lookup,ref lstUserLookUp);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                            if (question.QuestionType.ToLower() == "requestCategory")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookupColl))
                                    {
                                        InsertLookupValue(lookupColl, ref lstLookUp, DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }

                            }
                            if (question.QuestionType.ToLower() == "lookup")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (!string.IsNullOrWhiteSpace(lookupColl))
                                    {
                                        if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                                        {
                                            string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                                            string fieldName = question.QuestionTypePropertiesDicObj["lookupfield"];
                                            InsertLookupValue(lookupColl, ref lstLookUp, listName, fieldName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
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
                        string uValLookup =  mVariable.DefaultValue.PickFrom;
                        if (!string.IsNullOrWhiteSpace(uValLookup))
                            InsertUserValue(uValLookup,ref lstUserLookUp);
                    }

                    if (mVariable.VariableValues != null)
                    {
                        foreach (VariableValue v in mVariable.VariableValues)
                        {
                            if (v.IsPickFromConstant && !string.IsNullOrWhiteSpace(v.PickFrom))
                            {
                                string uValLookup = v.PickFrom;
                                if (!string.IsNullOrWhiteSpace(uValLookup))
                                    InsertUserValue(uValLookup,ref lstUserLookUp);
                            }

                            if (v.Conditions != null)
                            {
                                foreach (WhereExpression exp in v.Conditions)
                                {
                                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == exp.Variable.ToLower());
                                    if (question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                                    {
                                        if (!string.IsNullOrWhiteSpace(exp.Value))
                                        {
                                            InsertUserValue(exp.Value,ref lstUserLookUp);
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
                int taskID = UGITUtility.StringToInt(map.Key ?? 0);
                string moduleName = string.Empty;
                if (taskID > 0)
                {
                    UGITTask task = serviceExtension.Tasks.FirstOrDefault(x => x.ID == taskID);
                    if (task != null)
                    {
                        if (!string.IsNullOrWhiteSpace(task.RelatedModule))
                            moduleName = task.RelatedModule;
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
                    ULog.WriteLog(string.Format("module name found while looking into module"), "ExportServiceQuestionMapping");
                    continue;
                }


                if (moduleName == "_Task") // map is against task
                {
                    ServiceQuestionMapping qmap = taskMaps.FirstOrDefault(x => x.ColumnName == DatabaseObjects.Columns.AssignedTo);
                    if (qmap != null && !string.IsNullOrWhiteSpace(qmap.ColumnName) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                    {
                        List<string> userLookups =UGITUtility.ConvertStringToList(qmap.ColumnValue,Constants.Separator);
                        if (userLookups != null)
                        {
                            foreach (string uVal in userLookups)
                            {
                                List<UserProfile> userProfiles = serviceExtension.UserInfo??new List<UserProfile>();
                                InsertUserValue(uVal,ref userProfiles);
                            }
                        }
                    }
                }
                else //mapping is against ticket or svc so export user correctly for it
                {
                    UGITModule module = moduleViewManager.LoadByName(moduleName);
                    if (module == null)
                    {
                        ULog.WriteLog(string.Format("module not found while looking into module from database"), "ExportServiceQuestionMapping");
                        continue;
                    }

                    DataTable moduleDataList = GetTableDataManager.GetTableStructure(module.ModuleTable);
                    //SPList moduleDataList = SPListHelper.GetSPList(module.ModuleTicketTable, _spWeb);
                    if (moduleDataList == null)
                    {
                        ULog.WriteLog(string.Format("Module data list not found"), "ExportServiceQuestionMapping");
                        continue;
                    }

                    foreach (ServiceQuestionMapping qmap in taskMaps)
                    {
                        if (moduleDataList.Columns.IndexOf(qmap.ColumnName) == -1)
                            continue;

                        DataColumn spfield = moduleDataList.Columns[qmap.ColumnName];
                        if (spfield != null && qmap.ColumnName.EndsWith("user",StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(qmap.ColumnValue) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                        {
                            List<string> userLookups = UGITUtility.ConvertStringToList(qmap.ColumnValue, Constants.Separator);
                            if (userLookups != null)
                            {
                                foreach (string uVal in userLookups)
                                {
                                    List<UserProfile> userProfiles = serviceExtension.UserInfo??new List<UserProfile>();
                                    InsertUserValue(uVal, ref userProfiles);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
