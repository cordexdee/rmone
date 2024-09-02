using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using uGovernIT.Manager;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ServiceImport : UserControl
    {
        ServiceExtension serExt;
        //ServiceExtension serviceError;
        ServicesManager objServicesManager;
        ServiceQuestionMappingManager objServiceQuestionMappingManager;
        ServiceSectionManager objServiceSectionManager;
        ServiceQuestionManager objServiceQuestionManager;
        ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleViewManager;
        List<string> errorList = new List<string>();
        DataTable dtServices = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            moduleViewManager = new ModuleViewManager(_context);
            objServicesManager = new ServicesManager(_context);
            objServiceQuestionMappingManager = new ServiceQuestionMappingManager(_context);
            objServiceQuestionManager = new ServiceQuestionManager(_context);
            objServiceSectionManager = new ServiceSectionManager(_context);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btImportFiles_Click(object sender, EventArgs e)
        {
            int numFiles = Request.Files.Count;
            if (numFiles < 1)
            {
                lblErrorMessage.Visible = true;
            }
            else
            {
                List<string> lstFiles = new List<string>();
                for (int i = 0; i < numFiles; i++)
                {
                    HttpPostedFile postedfile = Request.Files[i];

                    if (!string.IsNullOrEmpty(postedfile.FileName))
                    {
                        System.IO.Stream inStream = postedfile.InputStream;
                        byte[] fileData = new byte[postedfile.ContentLength];
                        inStream.Read(fileData, 0, postedfile.ContentLength);
                        string fileName = Path.GetFileName(postedfile.FileName);
                        string folderPath = uHelper.GetUploadFolderPath();
                        bool folderExists = System.IO.Directory.Exists(folderPath);
                        if (!folderExists)
                            System.IO.Directory.CreateDirectory(folderPath);
                        postedfile.SaveAs(System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                        lstFiles.Add(fileName);
                    }
                }
                if (lstFiles != null && lstFiles.Count > 0)
                {
                    trServices.Style.Add("visibility", "visible");
                    gvServices.DataSource = lstFiles;
                    gvServices.DataBind();
                }
                else
                {
                    trServices.Style.Add("visibility", "hidden");
                }
            }
        }

        protected void btnCreateServices_Click(object sender, EventArgs e)
        {
            List<string> lstFiles = new List<string>();
            foreach (GridViewRow row in gvServices.Rows)
            {
                Label lblServiceName = (Label)row.Cells[0].FindControl("lblServiceText");
                string serviceName = lblServiceName.Text;
                lstFiles.Add(serviceName);
            }
            List<Services> lstServices = new List<Services>();
            if (lstFiles != null && lstFiles.Count > 0)
            {
                foreach (string fileName in lstFiles)
                {
                    ServiceExtension service = new ServiceExtension();
                    string text = System.IO.File.ReadAllText(System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                    //Import sharepoint export file
                    bool isSPFile = false;
                    if (!string.IsNullOrWhiteSpace(text) && text.IndexOf("uGovernIT.Helpers") != -1)
                    {
                        isSPFile = true;    
                        var replacements = new Dictionary<string, string> { { "ServiceExtension", "SPServiceExtension" }, { "uGovernIT.Helpers", "uGovernIT.Utility" }, { "Microsoft.SharePoint", "uGovernIT.Web" }
                            ,{ "uGovernIT.Core","uGovernIT.Utility"},{ "uGovernIT.Web","uGovernIT.Utility"}
                            ,{ "ServiceQuestion", "SPServiceQuestion" },{ "ServiceSection", "SPServiceSection" }
                            ,{ "VariableValue","SPVariableValue"},{"QuestionMapVariable","SPQuestionMapVariable" }
                            ,{"ServiceQuestionMapping","SPServiceQuestionMapping" },{"ServiceSectionCondition","SPServiceSectionCondition" }
                            ,{ "ServiceTaskCondition","SPServiceTaskCondition"},{ "SLAConfiguration","SPSLAConfiguration"}
                            ,{"UserLookupValue","SPUserLookupValue" },{ "WhereExpression","SPWhereExpression"}
                            ,{"UserInfo","SPUserInfo" },{ "LookupValueServiceExtension","SPLookupValueServiceExtension"}
                        };
                        //,{"Module","SPModule" },{ "ParentTask", "SParentTask" }
                        //,{ "RequestCategory","SPRequestCategory" }


                        var output = replacements.Aggregate(text, (current, replacement) => Regex.Replace(current,string.Format(@"\b{0}\b",replacement.Key), replacement.Value,RegexOptions.IgnoreCase));
                        text = output;
                       
                    }
                    using (Stream stream = new MemoryStream())
                    {
                        try
                        {
                            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
                            stream.Write(data, 0, data.Length);
                            stream.Position = 0;
                            if (isSPFile)
                            {
                                SPToDotNetImportHelper sPToDotNetImportHelper = new SPToDotNetImportHelper();
                                sPToDotNetImportHelper.ServiceCategories = objServiceCategoryManager.Load().Select(m => new { m.ID, m.Title }).ToDictionary(x=>x.ID,x=>x.Title);
                                sPToDotNetImportHelper.input = text;
                                service = sPToDotNetImportHelper.ImportService(stream);

                               
                            }
                            else
                            {
                                DataContractSerializer deserializer = new DataContractSerializer(typeof(ServiceExtension));
                                service = (ServiceExtension)deserializer.ReadObject(stream);
                            }

                            lstServices.Add(service);
                        }
                        catch (Exception ex)
                        {
                            Util.Log.ULog.WriteException(ex, "Error in Importing Service.");
                        }
                    }
                }
            }
            if (lstServices != null && lstServices.Count > 0)
            {
                foreach (ServiceExtension svc in lstServices)
                {
                    serExt = svc;
                    CreateService();
                }
            }
        }

        protected void btnNewServices_Click(object sender, EventArgs e)
        {
            #region sp to .net
            if (!string.IsNullOrEmpty(hdnserviceExists.Value) && hdnserviceExists.Value == "false")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Services"])))
                {
                    serExt = (ServiceExtension)ServicesManager.DeSerializeService(Convert.ToString(ViewState["Services"]), typeof(ServiceExtension));
                }

                serExt.IsActivated = false;
                SaveService(serExt);

                if (serExt.ID > 0 && serExt.Questions != null && serExt.Questions.Count > 0)
                {
                    List<string> keysColl = new List<string>() { "userquestion", ConfigConstants.ExistingUser, ConfigConstants.MirrorAccessFrom };
                    ReplaceTokenDuringImport(serExt, keysColl);
                }

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            #endregion

            /*if (!string.IsNullOrEmpty(hdnserviceExists.Value) && hdnserviceExists.Value == "false")
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Services"])))
                {
                    services = (ServiceExtension)DeSerializeService(Convert.ToString(ViewState["Services"]), typeof(ServiceExtension));

                }
                if (!string.IsNullOrEmpty(Convert.ToString(ViewState["ServicesError"])))
                {
                    serviceError = (ServiceExtension)DeSerializeService(Convert.ToString(ViewState["ServicesError"]), typeof(ServiceExtension));
                }
                if (ValidateFilesData(services))
                {
                    services.IsActivated = false;
                    SaveService(services);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else if (!string.IsNullOrEmpty(hdnserviceExists.Value) && hdnserviceExists.Value == "true")
                {
                    lblErrors.Visible = true;
                    divError.Visible = true;
                    pnlErrorSection.Visible = true;
                    divButtons.Visible = false;
                }
                else
                {
                    lblErrors.Visible = true;
                    divError.Visible = true;
                    pnlErrorSection.Visible = true;
                    divButtons.Visible = true;
                }


                //uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            */
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        private void CreateService()
        {
            if (ValidateFilesData(serExt))
            {
                serExt.IsActivated = false;
                SaveService(serExt);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else if (!string.IsNullOrEmpty(hdnserviceExists.Value) && hdnserviceExists.Value == "true")
            {
                lblErrors.Visible = true;
                divError.Visible = true;
                pnlErrorSection.Visible = true;
                divButtons.Visible = false;
            }
            else
            {
                lblErrors.Visible = true;
                divError.Visible = true;
                pnlErrorSection.Visible = true;
                divButtons.Visible = true;
            }
        }

        private bool ValidateFilesData(ServiceExtension service)
        {
            string strError = string.Empty;
            if (service != null)
            {
                //avoid null crash
                service.LookupValues = service.LookupValues ?? new List<LookupValueServiceExtension>();
                service.UserInfo = service.UserInfo ?? new List<UserProfile>();
                List<string> errors = new List<string>();
                string serviceType = "Service";
                if (service.ServiceType == Constants.ModuleFeedback)
                    serviceType = "Survey";
                else if (service.ServiceType == Constants.ModuleAgent)
                    serviceType = "Agent";

                #region service validatation
                if (service.ServiceType == Constants.ModuleFeedback && IsSurveyForModuleExists(service))
                {
                    btNewService.Visible = false;
                    btnUpdateService.Visible = true;
                    errors.Add(string.Format("<h4>A survey already exists for module <b>{0}</b>, this will override the existing one.</h4>", service.ModuleNameLookup));
                    hdnSurveyForModuleExist.Value = "true";
                }
                else if (IsServiceExists(service.Title))
                {
                    btnUpdateService.Visible = true;
                    btNewService.Text = "Create Copy";

                    //Code enhancement to provide suffix to the service title if the Service already exists
                    if (!string.IsNullOrEmpty(service.ServiceType) && service.ServiceType == Constants.ModuleAgent)
                    {
                        dtServices = objServicesManager.All(true);
                    }
                    else
                        dtServices = objServicesManager.All();

                    if (service.ServiceType == Constants.ModuleFeedback)
                    {
                        errors.Add("<h4>A survey with this name already exists, please choose how to proceed by clicking on a button below.</h4>");
                    }
                    else
                    {
                        errors.Add("<h4>A service with this name already exists, please choose how to proceed by clicking on a button below.</h4>");
                    }
                }

                strError = string.Empty;
                ValidateServiceCategory(service, ref strError);
                if (!string.IsNullOrWhiteSpace(strError))
                    errors.Add(strError);

                //validate service owner
                strError = string.Empty;
                if (!string.IsNullOrWhiteSpace(service.OwnerUser))
                {
                    List<UserProfile> validUsers = ValidateUser(service, service.OwnerUser, ref strError);
                    if (!string.IsNullOrWhiteSpace(strError))
                    {
                        errors.Add("<h4>Owner: User/group <b>" + strError + "</b> not fount or found multiple.</h4>");
                    }

                    if (validUsers != null && validUsers.Count > 0)
                        service.OwnerUser = string.Join(Constants.Separator6, validUsers.Select(x => x.Id));
                    else if (validUsers != null && validUsers.Count == 0)
                        service.OwnerUser = string.Empty;
                }

                //validate authorized to view
                if (!string.IsNullOrWhiteSpace(service.AuthorizedToView))
                {
                    strError = string.Empty;
                    List<UserProfile> validUsers = ValidateUser(service, service.AuthorizedToView.ToString(), ref strError);
                    if (!string.IsNullOrWhiteSpace(strError))
                    {
                        errors.Add(string.Format("<h4>Authorized To View: User/group <b>{0}<b> are not fount or found multiple.</h4>", strError));
                    }

                    if (validUsers != null && validUsers.Count > 0)
                        service.AuthorizedToView = string.Join(Constants.Separator6, validUsers);
                    if (validUsers != null && validUsers.Count == 0)
                        service.AuthorizedToView = string.Empty;
                }

                if (errors.Count > 0)
                {
                    errorList.Add(string.Format("<h3>{0}:</h3>", serviceType));
                    errorList.AddRange(errors);
                }
                #endregion

                ValidateQuestions(service);
                ValidateServiceTaskFields(service, service.Tasks, service.LookupValues, service.UserInfo);
                ValidateSkipLogicData(service);
                ValidateServiceVariables(service);
                ValidateServiceTaskMapping(service);

                divError.InnerHtml = string.Join("", errorList);

            }

            ViewState["Services"] = ServicesManager.SerializeService(service);

            return errorList.Count > 0 ? false : true;
        }

        private bool ValidateServiceCategory(ServiceExtension service, ref string strError)
        {
            ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(_context);
            bool isValid = true;
            if (!string.IsNullOrEmpty(service.ServiceCategoryType))
            {
                List<ServiceCategory> categories = objServiceCategoryManager.LoadAllCategories();
                ServiceCategory serviceCategory = null;
                //find exact match with name and id first
                if (categories != null && categories.Count > 0 && service.CategoryId > 0)
                {
                    serviceCategory = categories.Where(x => x.CategoryName == service.ServiceCategoryType && x.ID == service.CategoryId).FirstOrDefault();
                }

                //find match with name if exact match not found
                if (serviceCategory == null && categories != null && categories.Count > 0)
                {
                    categories = categories.Where(x => x.CategoryName == service.ServiceCategoryType).ToList();
                    if (categories != null && service.ServiceType != Constants.ModuleFeedback && service.ServiceType != Constants.ModuleAgent)
                    {
                        if (categories.Count == 0)
                        {
                            strError = string.Format("<h4>Category: Category with Name <b>{0}</b> does not exists.</h4>", service.ServiceCategoryType);
                            isValid = false;
                        }
                        else if (categories.Count > 1)
                        {
                            strError = string.Format("<h4>Category: More than two Category with Name <b>{0}</b> exists.</h4>", service.ServiceCategoryType);
                            isValid = false;
                        }
                        else
                        {
                            serviceCategory = categories.FirstOrDefault();
                        }

                        service.ServiceCategoryType = string.Empty;
                        service.CategoryId = 0;
                        if (serviceCategory != null)
                        {
                            service.ServiceCategoryType = serviceCategory.CategoryName;
                            service.CategoryId = serviceCategory.ID;
                        }

                    }
                }
            }

            return isValid;
        }
        private bool ValidateApplications(ref ServiceExtension service, ref string strError, ref ServiceExtension serviceError)
        {
            bool isValid = true;
            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'"); //SPListHelper.GetDataTable(DatabaseObjects.Lists.Applications);

            foreach (ServiceQuestion question in service.Questions.Where(y => y.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess))
            {
                string sfOptions = string.Empty;
                question.QuestionTypePropertiesDicObj.TryGetValue("application", out sfOptions);
                if (sfOptions != null && sfOptions.ToLower() != "all")
                {
                    List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                    List<string> applications = new List<string>();
                    foreach (string item in sfOptionList)
                    {
                        string applicationName = Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        DataRow[] drApps = null;
                        if (applicationName.Contains(';') && dtApplications != null)
                        {
                            drApps = dtApplications.Select(string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.Id, applicationName.Replace("'", "").Split(';')[0], DatabaseObjects.Columns.Title, applicationName.Replace("'", "").Split(';')[1]));
                            applications.Add(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0] + Constants.Separator2 + Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + applicationName.Split(';')[0]);

                            if (drApps == null || drApps.Length == 0)
                            {
                                strError = string.Format("{0} <br/> Application with Name {1} does not exist", strError, applicationName.Split(';')[1]);
                                isValid = false;
                            }
                        }
                        else
                        {
                            if (!strError.Contains("Selected Application(s) does not exist in the current environment"))
                                strError = string.Format("{0} <br/> Selected Application(s) does not exist in the current environment", strError);
                            isValid = false;
                        }
                    }

                    if (applications.Count > 0)
                        question.QuestionTypePropertiesDicObj["application"] = string.Join(Constants.Separator1, applications.ToArray());
                }
            }

            if (!isValid)
                serviceError.Questions = service.Questions;

            return isValid;
        }
        private bool IsValidUser(string FldUser, List<UserProfile> Users, ref string strError, ref ServiceExtension serviceError, string fieldName)
        {
            bool isUserExist = true;
            List<string> spFldUser = UGITUtility.ConvertStringToList(FldUser, Constants.Separator6);
            if (Users != null && Users.Count > 0 && spFldUser != null && spFldUser.Count > 0)
            {
                List<UserProfile> spFldUsers = HttpContext.Current.GetUserManager().GetUserInfosById(FldUser);
                foreach (UserProfile spUser in spFldUsers)
                {
                    if (spUser != null)
                    {
                        //UserInfo userInfo = Users.Where(c => c.ID == spUser.LookupId).FirstOrDefault();
                        UserProfile profile = spUser;
                        if (profile == null)
                        {
                            if (serviceError.UserInfo == null)
                            {
                                serviceError.UserInfo = new List<UserProfile>();
                            }
                            serviceError.UserInfo.Add(profile);
                            isUserExist = false;

                            if (profile != null)
                                strError = string.Format("{0} <br /> User with Name:{1} in field {2} does not exist ", strError, profile.Name, fieldName);
                            else
                                strError = string.Format("{0} <br /> User with ID:{1} in field {2} does not exist", strError, spUser.Id, fieldName);
                        }
                    }
                }
            }
            return isUserExist;
        }

        private void ValidateServiceTaskFields(ServiceExtension service, List<UGITTask> lstServiceTask, List<LookupValueServiceExtension> lookupValuesCollection, List<UserProfile> lstUserInfo)
        {
            string strError = string.Empty;
            if (lstServiceTask != null && lstServiceTask.Count > 0)
            {
                foreach (UGITTask servicetask in lstServiceTask)
                {
                    List<string> errors = new List<string>();
                    strError = string.Empty;
                    //Check Request Category
                    strError = string.Empty;
                    if (!string.IsNullOrWhiteSpace(servicetask.RequestTypeCategory) && !string.IsNullOrWhiteSpace(servicetask.RelatedModule))
                    {
                        string module = servicetask.RelatedModule;
                        UGITModule moduleObj = moduleViewManager.LoadByName(module);

                        ModuleRequestType requestTypeLookup = null;
                        string requestID = servicetask.RequestTypeCategory;
                        LookupValueServiceExtension lookserviceExtention = service.LookupValues.FirstOrDefault(x => (x.ListName == DatabaseObjects.Tables.RequestType || x.ListName == DatabaseObjects.Columns.RequestType) && x.ID == requestID);
                        if (lookserviceExtention != null)
                        {
                            List<string> lookupValueList = lookserviceExtention.Value.Split(new string[] { ">" }, StringSplitOptions.None).ToList();
                            if (lookupValueList != null && lookupValueList.Count == 3)
                            {
                                ModuleRequestType requestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.Category.ToLower() == lookupValueList[0].Trim().ToLower() && x.SubCategory.ToLower() == lookupValueList[1].Trim().ToLower() && x.RequestType.ToLower() == lookupValueList[2].Trim().ToLower());
                                if (requestType != null)
                                    requestTypeLookup = requestType;
                            }

                            if (requestTypeLookup == null)
                            {
                                errors.Add(string.Format("<h4>Request type <b>{0}</b> not found.</h4>", string.Join(" > ", lookupValueList.Where(x => !string.IsNullOrWhiteSpace(x)))));
                            }
                        }
                        else
                        {
                            errors.Add(string.Format("<h4>Request type <b>{0}</b> not found.</h4>", servicetask.RequestTypeCategory));
                        }

                        servicetask.RequestTypeCategory = string.Empty;
                        if (requestTypeLookup != null)
                            servicetask.RequestTypeCategory =UGITUtility.ObjectToString(requestTypeLookup.ID);

                    }

                    strError = string.Empty;
                    if (!string.IsNullOrWhiteSpace(servicetask.AssignedTo))
                    {
                        strError = string.Empty;
                        List<UserProfile> lookups = ValidateUser(service, servicetask.AssignedTo.ToString(), ref strError);


                        if (!string.IsNullOrWhiteSpace(strError))
                        {
                            errors.Add(string.Format("User/group <b>{0}</b> not found or found multiple for assigned to.", strError));
                        }

                        servicetask.AssignedTo = string.Empty;
                        if (lookups.Count > 0)
                        {
                            servicetask.AssignedTo = string.Join(Constants.Separator6, lookups.Select(x => x.Id));
                        }
                    }

                    strError = string.Empty;
                    if (!string.IsNullOrWhiteSpace(servicetask.Approver))
                    {
                        strError = string.Empty;
                        List<UserProfile> lookups = ValidateUser(service, servicetask.Approver.ToString(), ref strError);

                        if (!string.IsNullOrWhiteSpace(strError))
                        {
                            errors.Add(string.Format("User/group <b>{0}</b> not found or found multiple for approver.", strError));
                        }

                        servicetask.Approver = string.Empty;
                        if (lookups.Count > 0)
                        {
                            servicetask.Approver = string.Join(Constants.Separator6, lookups.Select(x => x.Id));
                        }
                    }

                    if (errors.Count > 0)
                    {
                        errorList.Add(string.Format("<h3>{0}: <b>{1}</b></h3>", string.IsNullOrWhiteSpace(servicetask.RelatedModule) ? "Task" : "Ticket", servicetask.Title));
                        errorList.AddRange(errors.Select(x => string.Format("<h4>{0}</h4>", x)));
                    }
                }
            }
        }


        private void ValidateSkipLogicData(ServiceExtension service)
        {
            string strError = string.Empty;
            foreach (ServiceSectionCondition skipCondition in service.SkipSectionCondition)
            {
                List<WhereExpression> lstWhereExpression = skipCondition.Conditions;
                foreach (WhereExpression expression in lstWhereExpression)
                {
                    List<string> errors = new List<string>();

                    ServiceQuestion question = service.Questions.Where(c => c.TokenName == expression.Variable).FirstOrDefault();
                    if (question == null)
                    {
                        errors.Add("Missing question in skip condition!");
                    }
                    else if (question.QuestionType.ToLower() == "userfield")
                    {
                        strError = string.Empty;
                        List<UserProfile> spUser = ValidateUser(service, expression.Value, ref strError);
                        expression.Value = string.Empty;
                        if (spUser != null && spUser.Count > 0)
                            expression.Value = spUser.FirstOrDefault().Id;

                        if (!string.IsNullOrWhiteSpace(strError))
                            errors.Add(string.Format("User/group <b>{0}</b> are not found or found multiple.", strError));
                    }
                    else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                    {
                        strError = string.Empty;
                        List<LookupValueServiceExtension> lookupColl = CheckLookupvalues(expression.Value, service.LookupValues, ref strError, DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title);

                        if (!string.IsNullOrWhiteSpace(strError))
                            errors.Add(strError);

                        expression.Value = string.Empty;
                        if (lookupColl != null && lookupColl.Count > 0)
                            expression.Value = lookupColl.First().ID;
                    }
                    else if (question.QuestionType.ToLower() == "lookup")
                    {
                        strError = string.Empty;
                        if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                        {
                            string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                            string fieldname = question.QuestionTypePropertiesDicObj["lookupfield"];
                            List<LookupValueServiceExtension> lookupColl = CheckLookupvalues(expression.Value, service.LookupValues, ref strError, listName, fieldname);

                            if (!string.IsNullOrWhiteSpace(strError))
                                errors.Add(strError);

                            if (lookupColl != null && lookupColl.Count > 0)
                            {
                                expression.Value = lookupColl.First().ID;
                            }
                            else if (lookupColl == null)
                            {
                                expression.Value = string.Empty;
                            }
                        }
                    }

                    if (errors.Count > 0)
                    {
                        errorList.Add(string.Format("<h3>Question Skip Logic: <b>{0}</b></h3>", skipCondition.Title));
                        errorList.AddRange(errors.Select(x => string.Format("<h4>{0}</h4>", x)));
                    }
                }
            }

            foreach (ServiceTaskCondition skipCondition in service.SkipTaskCondition)
            {
                List<WhereExpression> lstWhereExpression = skipCondition.Conditions;
                foreach (WhereExpression expression in lstWhereExpression)
                {
                    List<string> errors = new List<string>();

                    ServiceQuestion question = service.Questions.Where(c => c.TokenName == expression.Variable).FirstOrDefault();
                    if (question == null)
                    {
                        errors.Add("Missing question in skip condition!");
                    }
                    else if (question.QuestionType.ToLower() == "userfield")
                    {
                        strError = string.Empty;

                        List<UserProfile> spUser = ValidateUser(service, expression.Value, ref strError);
                        expression.Value = string.Empty;
                        if (spUser != null && spUser.Count > 0)
                            expression.Value = spUser.First().Id;

                        if (!string.IsNullOrWhiteSpace(strError))
                            errors.Add(string.Format("User/group <b>{0}</b> are not found or found multiple.", strError));
                    }
                    else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                    {
                        strError = string.Empty;
                        List<LookupValueServiceExtension> lookupColl = CheckLookupvalues(expression.Value, service.LookupValues, ref strError, DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title);

                        if (!string.IsNullOrWhiteSpace(strError))
                            errors.Add(strError);

                        expression.Value = string.Empty;
                        if (lookupColl != null && lookupColl.Count > 0)
                            expression.Value = lookupColl.First().ID;
                    }
                    else if (question.QuestionType.ToLower() == "lookup")
                    {
                        strError = string.Empty;
                        if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                        {
                            string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                            string fieldname = question.QuestionTypePropertiesDicObj["lookupfield"];
                            List<LookupValueServiceExtension> lookupColl = CheckLookupvalues(expression.Value, service.LookupValues, ref strError, listName, fieldname);

                            if (!string.IsNullOrWhiteSpace(strError))
                                errors.Add(strError);

                            expression.Value = string.Empty;
                            if (lookupColl != null && lookupColl.Count > 0)
                                expression.Value = lookupColl.First().ID;
                        }
                    }

                    if (errors.Count > 0)
                    {
                        errorList.Add(string.Format("<h3>Task Skip Logic: <b>{0}</b></h3>", skipCondition.Title));
                        errorList.AddRange(errors.Select(x => string.Format("<h4>{0}</h4>", x)));
                    }
                }
            }
        }
        private List<LookupValueServiceExtension> CheckLookupvalues(string id, List<LookupValueServiceExtension> lookupValuesCollection, ref string strError, string listName, string filedName, string moduleName = "", string moduleField = "")
        {
            List<LookupValueServiceExtension> correctLookups = new List<LookupValueServiceExtension>();
            LookupValueServiceExtension lookUpValue = lookupValuesCollection.FirstOrDefault(c => c.ID == id && c.ListName.ToLower() == listName.ToLower());
            if (lookUpValue != null)
            {
                string query = string.Empty;
                //query.ViewFields = string.Format("<FieldRef Name='{0}' Nullable='True'/>", filedName);
                List<string> exps = new List<string>();
                exps.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));
                exps.Add(string.Format("{0}='{1}'", filedName, lookUpValue.Value));
                if (!string.IsNullOrWhiteSpace(moduleName))
                    exps.Add(string.Format("{0}='{0}'", moduleField, moduleName));
                query = UGITUtility.GenerateWhereQueryWithAndOr(exps, exps.Count - 1, true);

                DataTable spListItemCollection = GetTableDataManager.GetTableData(listName, query);
                if (spListItemCollection == null || spListItemCollection.Rows.Count == 0)
                {
                    strError = string.Format("{0} with Name <b>{1}</b> does not exist", listName, lookUpValue.Value);
                }
                else if (spListItemCollection.Rows.Count > 1)
                {
                    strError = string.Format("{0} with Name <b>{1}</b> having multiple values", listName, lookUpValue.Value);
                }
                else
                {
                    correctLookups.Add(new LookupValueServiceExtension(UGITUtility.ObjectToString(spListItemCollection.Rows[0][DatabaseObjects.Columns.ID]), Convert.ToString(spListItemCollection.Rows[0][filedName]), listName));
                }
            }
            return correctLookups;
        }
        // Check if a (non-deleted) service exists with the same name
        private bool IsServiceExists(string serviceTitle)
        {
            if (!string.IsNullOrEmpty(serviceTitle))
            {
                List<Services> lstServices = objServicesManager.LoadAllServices().Where(x => x.Title.EqualsIgnoreCase(serviceTitle) && !x.Deleted).ToList(); ;
                if (lstServices != null && lstServices.Count > 0)
                    return true;
            }
            return false;
        }

        private bool IsSurveyForModuleExists(ServiceExtension serviceExt)
        {
            bool isExist = false;
            List<Services> allServices = objServicesManager.LoadAllSurveys();
            if (allServices == null || allServices.Count == 0)
                isExist = false;
            if (!string.IsNullOrWhiteSpace(serviceExt.ModuleNameLookup) && allServices.Exists(x => x.ModuleNameLookup == serviceExt.ModuleNameLookup))
            {
                isExist = true;
            }
            return isExist;
        }
        private void SaveService(ServiceExtension serviceExt, bool updateIfExist = false)
        {
            var map = new AutoMapper.MapperConfiguration(config =>
            {
                config.CreateMap<ServiceExtension, Services>();
            }).CreateMapper();

            Services service = map.Map<Services>(serviceExt);
            //Services service = (uGovernIT.Utility.Services)serviceExt;
            Services newService = service;
            Services oldService = null;
            if (hdnSurveyForModuleExist.Value != null && Convert.ToString(hdnSurveyForModuleExist.Value) == "true") //only one survey can exist against one module
            {
                List<Services> allServices = objServicesManager.LoadAllSurveys();
                oldService = allServices.FirstOrDefault(x => x.ModuleNameLookup == serviceExt.ModuleNameLookup);

                //load full service with questions and other parts
                if (oldService != null)
                {
                    oldService = objServicesManager.LoadByServiceID(service.ID, true, true, true);
                }
            }
            else if (updateIfExist)
            {
                oldService = objServicesManager.LoadServiceByTitle(service.Title,true);
            }

            if (oldService != null)
            {
                objServiceQuestionMappingManager.Delete(oldService.QuestionsMapping);
                UGITTaskManager uGITTaskManager = new UGITTaskManager(_context);
                uGITTaskManager.Delete(oldService.Tasks);
                objServiceQuestionManager.Delete(oldService.Questions);
                objServiceSectionManager.Delete(oldService.Sections);
                newService.ID = oldService.ID;
                newService.IsActivated = oldService.IsActivated;
            }
            else // New service with "- Copy" suffix
            {
                if (!string.IsNullOrEmpty(service.ServiceType) && service.ServiceType == Constants.ModuleAgent)
                    dtServices = objServicesManager.All(true); // Get all agents
                else
                    dtServices = objServicesManager.All(); // Get all services

                newService.Title = objServicesManager.GetUpdatedServiceTitle(service.Title, dtServices);
                newService.ID = 0;
            }

            if (!string.IsNullOrWhiteSpace(divError.InnerHtml))
            {
                Dictionary<string, string> customProperties = newService.CustomProperties;
                if (customProperties == null)
                {
                    customProperties = new Dictionary<string, string>();
                }
                if (!customProperties.ContainsKey("serviceerrorshtml"))
                {
                    customProperties.Add("serviceerrorshtml", HttpContext.Current.Server.HtmlEncode(divError.InnerHtml));
                }
                else
                {
                    customProperties["serviceerrorshtml"] = HttpContext.Current.Server.HtmlEncode(divError.InnerHtml);
                }
                newService.CustomProperties = customProperties;
            }
            /*List<string> owners = new List<string>();
            if ((serviceError == null || serviceError.OwnerUser == null) && serviceExt.OwnerUser != null)
            {
                foreach (string userEntity in UGITUtility.ConvertStringToList(serviceExt.OwnerUser, Constants.Separator6))
                {
                    UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.UserName == userEntity);
                    if (user != null)
                    {
                        if (user.isRole)
                        {
                            UserProfile spgrp = user;
                            if (spgrp != null && spgrp.Name == user.Name)
                            {
                                owners.Add(spgrp.Id);
                            }
                        }
                        else
                        {
                            UserProfile spuser = user;// UserProfile.GetUserById(user.ID);
                            if (spuser != null)
                            {
                                owners.Add(spuser.Id);
                            }
                        }
                    }
                }
            }

            //SPFieldUserValueCollection authToView = new SPFieldUserValueCollection();
            List<string> authToView = new List<string>();
            if ((serviceError == null || serviceError.AuthorizedToView == null) && serviceExt.AuthorizedToView != null)
            {
                foreach (var userEntity in UGITUtility.ConvertStringToList(serviceExt.AuthorizedToView, Constants.Separator6))
                {
                    UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.UserName == userEntity);
                    if (user != null)
                    {

                        if (user.isRole)
                        {
                            UserProfile spgrp = user; //UserProfile.GetGroupByName(user.Name);
                            if (spgrp != null && spgrp.Name == user.Name)
                            {
                                authToView.Add(spgrp.Id);
                            }
                        }
                        else
                        {
                            UserProfile spusr = user; //UserProfile.GetUserById(user.ID);
                            if (spusr != null)
                            {
                                authToView.Add(spusr.Id);
                            }
                        }
                    }
                }
            }
            newService.AuthorizedToView = string.Join(Constants.Separator6, authToView);
            newService.OwnerUser = string.Join(Constants.Separator6, owners);
            if (!string.IsNullOrEmpty(serviceError.ServiceCategoryType))
            {
                List<ServiceCategory> allServiceCategories = objServiceCategoryManager.Load(); // ServiceCategory.LoadAllCategories();
                ServiceCategory category = allServiceCategories.FirstOrDefault(x => x.CategoryName == serviceError.ServiceCategoryType);
                if (category == null)
                {
                    category = new ServiceCategory();
                    category.CategoryName = serviceError.ServiceCategoryType;
                    category.Title = serviceError.ServiceCategoryType;
                    objServiceCategoryManager.Insert(category);
                }
                newService.ServiceCategoryType = category.CategoryName;
                newService.CategoryId = category.ID;
            }
            if (serviceError != null)
            {
                Dictionary<string, string> customProperties = newService.CustomProperties;
                if (customProperties == null)
                {
                    customProperties = new Dictionary<string, string>();
                }
                if (!customProperties.ContainsKey("serviceerrors"))
                {
                    customProperties.Add("serviceerrors", Convert.ToString(SerializeService(serviceError)).Replace("=", "~"));
                }
                else
                {
                    customProperties["serviceerrors"] = Convert.ToString(SerializeService(serviceError));
                }
                if (!customProperties.ContainsKey("serviceerrorshtml"))
                {
                    customProperties.Add("serviceerrorshtml", HttpContext.Current.Server.HtmlEncode(divError.InnerHtml));
                }
                else
                {
                    customProperties["serviceerrorshtml"] = HttpContext.Current.Server.HtmlEncode(divError.InnerHtml);
                }
                newService.CustomProperties = customProperties;
            }
            var map = new AutoMapper.MapperConfiguration(config =>
            {
                config.CreateMap<ServiceExtension, Services>();
            }).CreateMapper();

            Services svc = map.Map<Services>(serviceExt);
            */
            //Services svc = map.Map<Services>(serviceExt);

            objServicesManager.Save(newService);
            objServicesManager.SaveSection(newService);
            objServicesManager.SaveQuestions(newService);
            objServicesManager.SaveSectionQuestionOrder(newService);
            objServicesManager.SaveTasks(newService);
            objServicesManager.SaveSkipTasksAndSections(newService);
            UpdateLooupField(newService);
            objServicesManager.SaveMapDeafultValues(newService);
        }

        private void SaveSectionQuestionOrder(Services newService)
        {
            if (newService.Sections != null && newService.Sections.Count > 0)
            {
                //  ServiceSection.SaveOrder(newService.Sections);
            }
            if (newService.Questions != null && newService.Questions.Count > 0)
            {
                //  ServiceQuestion.SaveOrder(newService.Questions);
            }

        }

        private void SaveSection(Services newService)
        {
          
            if (newService.Sections != null && newService.Sections.Count > 0)
            {
                foreach (ServiceSection section in newService.Sections)
                {
                    long id = section.ID;
                    string sectionName = section.SectionName;
                    if (string.IsNullOrWhiteSpace(sectionName))
                        sectionName = section.Title;
                    section.SectionName = sectionName;
                    section.ServiceID = newService.ID;
                    section.ID = 0;
                    section.TenantID = _context.TenantID; // Assigning loggedin users TenantID;
                    objServiceSectionManager.Save(section);
                    // section.SetSectionId();
                    //section.Save();
                    //List<ServiceQuestion> svcQuestionList = newService.Questions.Where(c => c.ServiceSectionID == id && Convert.ToString(c.ServiceSectionName).Trim() == sectionName.Trim()).ToList();
                    List<ServiceQuestion> svcQuestionList = newService.Questions.Where(c => c.ServiceSectionID == id).ToList();
                    foreach (ServiceQuestion svcQuest in svcQuestionList)
                    {
                        svcQuest.ServiceSectionID = section.ID;
                        svcQuest.TenantID = _context.TenantID;
                    }
                    if (newService.SkipSectionCondition != null)
                    {
                        foreach (ServiceSectionCondition sectionCondition in newService.SkipSectionCondition)
                        {
                            List<long> sections = sectionCondition.SkipSectionsID;
                            if (sections != null && sections.Count > 0)
                            {
                                for (int i = 0; i < sections.Count; i++)
                                {
                                    if (sections[i] == id)
                                        sections[i] = section.ID;
                                }
                                sectionCondition.SkipSectionsID = sections;
                            }
                        }
                    }
                }
            }
        }

        private void SaveQuestions(Services newService)
        {
            objServiceQuestionManager = new ServiceQuestionManager(_context);
            if (newService.Questions != null && newService.Questions.Count > 0)
            {
                // SPList applicationList = SPListHelper.GetSPList(DatabaseObjects.Lists.Applications);
                //DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications); //applicationList.Items.GetDataTable();
                foreach (ServiceQuestion question in newService.Questions)
                {
                    int questionId = Convert.ToInt32(question.ID);
                    string questionTitle = question.QuestionTitle;
                    // question.SetQuestionId();
                    question.ID = 0;
                    question.ServiceID = newService.ID;
                    objServiceQuestionManager.Save(question);
                    foreach (ServiceQuestion ques in newService.Questions.Where(x => x.QuestionType.ToLower() == "applicationaccessrequest"))
                    {
                        if (ques.QuestionTypePropertiesDicObj != null && ques.QuestionTypePropertiesDicObj.Count > 0)
                        {
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            string changedKey = string.Empty;
                            KeyValuePair<string, string> existingUser = ques.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "existinguser");
                            if (existingUser.Key != null)
                            {
                                changedKey = "existinguser";

                            }
                            if (!string.IsNullOrWhiteSpace(changedKey))
                            {
                                ques.QuestionTypePropertiesDicObj[changedKey] = Convert.ToString(question.ID);
                            }
                        }
                    }

                    foreach (ServiceQuestionMapping quesMap in newService.QuestionsMapping)
                    {
                        quesMap.ID = 0;
                        //quesMap.SetDefaultId();
                        quesMap.ServiceID = newService.ID;
                        quesMap.TenantID = _context.TenantID;
                        if (quesMap.ServiceQuestionID > 0 && quesMap.ServiceQuestionID == questionId && !string.IsNullOrEmpty(quesMap.ServiceQuestionName) && quesMap.ServiceQuestionName.Trim() == Convert.ToString(questionTitle).Trim())
                        {
                            quesMap.ServiceQuestionID = Convert.ToInt32(question.ID);
                            quesMap.PickValueFrom = Convert.ToString(question.ID);
                        }
                    }
                    if (newService.SkipSectionCondition != null)
                    {
                        foreach (ServiceSectionCondition sectionCondition in newService.SkipSectionCondition)
                        {
                            List<long> questions = sectionCondition.SkipQuestionsID;
                            if (questions != null && questions.Count > 0)
                            {
                                for (int i = 0; i < questions.Count; i++)
                                {
                                    if (questions[i] == questionId)
                                        questions[i] = Convert.ToInt32(question.ID);
                                }
                                sectionCondition.SkipQuestionsID = questions;
                            }
                        }
                    }

                }
            }
        }



        private void SaveTasks(Services newService, ServiceExtension serviceExt)
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(_context);
            Dictionary<long, long> tempdic = new Dictionary<long, long>();
            string TicketId = string.Empty;
            if (newService.Tasks != null && newService.Tasks.Count > 0)
            {
                UGITTask task = null;
                foreach (UGITTask task1 in newService.Tasks)
                {
                    task = task1;
                    long id = task.ID;
                    string title = task.Title;
                    task.TicketId = Convert.ToString(newService.ID);
                    task.TenantID = _context.TenantID;
                    task.ID = 0;
                    UGITTask errorTask = null;
                    //if (serviceError != null && serviceError.Tasks != null && serviceError.Tasks.Count > 0)
                    //{
                    //    errorTask = serviceError.Tasks.FirstOrDefault(c => c.ID == id);
                    //}

                    if (errorTask != null && errorTask.AssignedTo != null)
                    {
                        task.AssignedTo = null;
                    }
                    else if (task.AssignedTo != null && task.AssignedTo != "")
                    {
                        //  SPFieldUserValueCollection userValueCollAssigned = new SPFieldUserValueCollection();
                        List<string> userValueCollAssigned = new List<string>();
                        foreach (var userEntity in UGITUtility.ConvertStringToList(task.AssignedTo, Constants.Separator6))
                        {
                            UserProfile user = serviceExt.UserInfo.FirstOrDefault(c => c.Id == userEntity);
                            if (user == null)
                                continue;
                            if (user.isRole)
                            {
                                UserProfile spgroup = user; //UserProfile.GetGroupByName(user.Name);
                                if (spgroup != null && spgroup.Name == user.Name)
                                {
                                    //SPFieldUserValue userVal = new SPFieldUserValue(SPContext.Current.Web, spgroup.ID, spgroup.Name);
                                    if (spgroup != null)
                                        userValueCollAssigned.Add(spgroup.Id);
                                }
                            }
                            else
                            {
                                UserProfile spuser = user;
                                //SPUser spuser = UserProfile.GetUserById(userEntity.LookupId);
                                if (spuser != null && spuser.Name == user.Name)
                                {
                                    //SPFieldUserValue userVal = new SPFieldUserValue(SPContext.Current.Web, user.ID, user.Name);
                                    //if (userVal != null && userVal.User != null)
                                    //    userValueCollAssigned.Add(userVal);
                                    userValueCollAssigned.Add(spuser.Id);
                                }
                            }
                        }
                        task.AssignedTo = string.Join(Constants.Separator6, userValueCollAssigned);
                    }
                    if (errorTask != null && errorTask.ModuleNameLookup != null)
                    {
                        task.ModuleNameLookup = null;
                    }
                    else if (task.ModuleNameLookup != null)
                    {
                        LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == task.ModuleNameLookup && c.ListName.ToLower() == "config_modules") as LookupValueServiceExtension;
                        if (svcLookup != null)
                        {
                            task.ModuleNameLookup = Convert.ToString(svcLookup.ID);
                            //SPFieldLookupValue lookUpValue = new SPFieldLookupValue(svcLookup.ID, svcLookup.Value);
                            //if (lookUpValue != null)
                            //    task.ModuleName = lookUpValue;
                        }
                    }

                    if (errorTask != null && errorTask.RequestTypeCategory != null)
                    {
                        task.RequestTypeCategory = null;
                    }
                    else if (task.RequestTypeCategory != null && task.RequestTypeCategory != "")
                    {
                        LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == task.RequestTypeCategory && c.ListName.ToLower() == "requestcategory");
                        if (svcLookup != null)
                        {
                            task.RequestTypeCategory = svcLookup.ListName;
                            //SPFieldLookupValue lookUpValue = new SPFieldLookupValue(svcLookup.ID, svcLookup.Value);
                            //if (lookUpValue != null)
                            //    task.RequestCategory = lookUpValue;
                        }
                    }

                    if (task.ParentTaskID > 0)
                    {
                        long parentId = task.ParentTaskID;
                        task.ParentTaskID = tempdic[parentId];
                    }
                    TicketId = Convert.ToString(newService.ID);
                    string moduleName = newService.ModuleNameLookup;
                    objUGITTaskManager.SaveTask(ref task, moduleName, TicketId);
                    // task.Save();
                    tempdic.Add(id, task.ID);
                    foreach (UGITTask svcTask in newService.Tasks)
                    {
                        if (svcTask.Predecessors != null)
                        {
                            foreach (string lookupValue in UGITUtility.ConvertStringToList(svcTask.Predecessors, Constants.Separator6))
                            {
                                LookupValueServiceExtension svcLookup = serviceExt.LookupValues.FirstOrDefault(c => c.ID == lookupValue && c.ListName.ToLower() == "serviceticketrelationships");
                                //if (svcLookup != null && (svcLookup.ID == id && svcLookup.Value == title))
                                //{
                                //    lookupValue = task.ID;
                                //}
                            }
                        }

                    }
                    if (newService.SkipTaskCondition != null)
                    {
                        foreach (ServiceTaskCondition taskCondition in newService.SkipTaskCondition)
                        {
                            List<long> tasks = taskCondition.SkipTasks;
                            if (tasks != null && tasks.Count > 0)
                            {
                                for (int i = 0; i < tasks.Count; i++)
                                {
                                    if (tasks[i] == id)
                                        tasks[i] = task.ID;
                                }
                                taskCondition.SkipTasks = tasks;
                            }
                        }
                    }
                    List<ServiceQuestionMapping> questionMapList = newService.QuestionsMapping.Where(x => x.ServiceTaskID == id).ToList();
                    if (questionMapList != null && questionMapList.Count > 0)
                    {
                        foreach (ServiceQuestionMapping questionMap in questionMapList)
                        {
                            questionMap.ServiceTaskID = task.ID;
                        }
                    }
                }
                List<UGITTask> _svcTask = objUGITTaskManager.LoadByProjectID("SVCConfig", TicketId);
                foreach (UGITTask svcTask in _svcTask)
                {
                    if (!string.IsNullOrEmpty(svcTask.Predecessors))
                    {
                        List<string> lsttask = new List<string>();
                        string oldTaskIDs = svcTask.Predecessors;
                        string[] taskIDs = oldTaskIDs.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < taskIDs.Length; i++)
                        {
                            if (tempdic.ContainsKey(Convert.ToInt64(taskIDs[i])))
                            {
                                lsttask.Add(Convert.ToString(tempdic[Convert.ToInt64(taskIDs[i])]));
                            }
                        }
                        if (lsttask.Count > 0)
                        {
                            UGITTask __svcTask = svcTask;
                            svcTask.Predecessors = UGITUtility.ConvertListToString(lsttask, Constants.Separator6);
                            objUGITTaskManager.SaveTask(ref __svcTask, "SVCConfig", TicketId);
                        }
                    }
                }
            }
        }

        protected void btnUpdateService_Click(object sender, EventArgs e)
        {
            #region sp to .net
            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Services"])))
            {
                serExt = (ServiceExtension)ServicesManager.DeSerializeService(Convert.ToString(ViewState["Services"]), typeof(ServiceExtension));
            }

            serExt.IsActivated = false;
            SaveService(serExt, true);

            if (serExt.ID > 0 && serExt.Questions != null && serExt.Questions.Count > 0)
            {
                List<string> keysColl = new List<string>() { "userquestion", ConfigConstants.ExistingUser, ConfigConstants.MirrorAccessFrom };
                ReplaceTokenDuringImport(serExt, keysColl);
            }

            uHelper.ClosePopUpAndEndResponse(Context, true);
            #endregion
            /* if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Services"])))
             {
                 services = (ServiceExtension)ServicesManager.DeSerializeService(Convert.ToString(ViewState["Services"]), typeof(ServiceExtension));
             }
             if (!string.IsNullOrEmpty(Convert.ToString(ViewState["ServicesError"])))
             {
                 serviceError = (ServiceExtension)DeSerializeService(Convert.ToString(ViewState["ServicesError"]), typeof(ServiceExtension));
             }
             if (ValidateFilesData(services))
             {
                 services.IsActivated = false;
                 SaveService(services);
             }

             if (services.ID > 0 && services.Questions != null && services.Questions.Count > 0)
             {
                 List<string> keysColl = new List<string>() { "userquestion", ConfigConstants.ExistingUser, ConfigConstants.MirrorAccessFrom };
                 ReplaceTokenDuringImport(services, keysColl);
             }

             uHelper.ClosePopUpAndEndResponse(Context, true);
             */
        }

        public void ReplaceTokenDuringImport(ServiceExtension service, List<string> keysColl)
        {
            ServiceQuestion questionItem = null;

            foreach (string loopKey in keysColl)
            {
                service.Questions.Where(x => x.QuestionTypePropertiesDicObj.ContainsKey(loopKey)).ToList().ForEach(y =>
                {
                    Dictionary<string, string> dic = y.QuestionTypePropertiesDicObj;
                    if (dic != null)
                    {
                        string val = dic[loopKey];
                        if (val != null)
                        {
                            ServiceQuestion ques = service.Questions.Where(p => p.TokenName.ToLower() == val).FirstOrDefault();
                            if (ques != null)
                            {
                                questionItem = objServiceQuestionManager.LoadByID(y.ID);//SPListHelper.GetSPListItem(DatabaseObjects.Lists.ServiceQuestions, y.ID);
                                UpdateQuestionDuringImport(questionItem, loopKey, ques);
                            }

                        }
                    }
                });
            }
        }

        private void UpdateQuestionDuringImport(ServiceQuestion questionItem, string loopKey, ServiceQuestion ques)
        {
            if (questionItem != null)
            {
                Dictionary<string, string> innerDic = questionItem.QuestionTypePropertiesDicObj;  //uHelper.GetCustomProperties(Convert.ToString(uHelper.GetSPItemValue(questionItem, DatabaseObjects.Columns.QuestionTypeProperties)), Constants.Separator);
                if (innerDic != null && innerDic.ContainsKey(loopKey))
                {
                    innerDic[loopKey] = Convert.ToString(ques.ID);
                    StringBuilder param = new StringBuilder();
                    foreach (string key in innerDic.Keys)
                    {
                        param.AppendFormat("{0}={1}{2}", key, innerDic[key], Constants.Separator);
                    }

                    questionItem.QuestionTypeProperties = param.ToString();
                    objServiceQuestionManager.Update(questionItem);
                }
            }
        }

        private void SaveSkipTasksAndSections(Services newService)
        {
            if ((newService.SkipTaskCondition != null && newService.SkipTaskCondition.Count > 0) || (newService.SkipSectionCondition != null && newService.SkipSectionCondition.Count > 0))
            {
                objServicesManager.Save(newService);
            }
        }

        private void SaveMapDeafultValues(Services newService)
        {
            if (newService.QuestionsMapping != null && newService.QuestionsMapping.Count > 0)
            {
                foreach (ServiceQuestionMapping serviceQuesMap in newService.QuestionsMapping)
                {
                    
                    objServiceQuestionMappingManager.Save(serviceQuesMap);
                    // serviceQuesMap.Save();
                }
            }
        }
        private void SaveVariables(Services newService)
        {
            if (newService.QMapVariables != null && newService.QMapVariables.Count > 0)
            {

            }
        }
        public static string SerializeService(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static object DeSerializeService(string s, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(s);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }

        void ValidateQuestions(ServiceExtension service)
        {
            foreach (ServiceQuestion question in service.Questions)
            {
                if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                {
                    if (question.QuestionTypePropertiesDicObj.ContainsKey("module") && question.QuestionTypePropertiesDicObj.ContainsKey("requesttypes"))
                    {
                        string module = question.QuestionTypePropertiesDicObj["module"];
                        List<string> requestTypeIDs = UGITUtility.SplitString(question.QuestionTypePropertiesDicObj["requesttypes"], Constants.Separator2).ToList();

                        UGITModule moduleObj = moduleViewManager.LoadByName(module);
                        List<string> invalidRequestTypes = new List<string>();
                        List<string> validRequestTypeIDs = new List<string>();
                        foreach (string reqId in requestTypeIDs)
                        {
                            if (UGITUtility.StringToInt(reqId) == 0)
                                continue;

                            LookupValueServiceExtension lookserviceExtention = service.LookupValues.FirstOrDefault(x => x.ListName == DatabaseObjects.Tables.RequestType && x.ID == reqId);
                            if (lookserviceExtention != null)
                            {
                                List<string> lookupValueList = lookserviceExtention.Value.Split(new string[] { ">" }, StringSplitOptions.None).ToList();
                                if (lookupValueList != null && lookupValueList.Count == 3)
                                {
                                    ModuleRequestType requestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.Category.ToLower() == lookupValueList[0].Trim().ToLower() && x.SubCategory.ToLower() == lookupValueList[1].Trim().ToLower() && x.RequestType.ToLower() == lookupValueList[2].Trim().ToLower());
                                    if (requestType != null)
                                        validRequestTypeIDs.Add(Convert.ToString(requestType.ID));
                                    else
                                        invalidRequestTypes.Add(string.Join(" > ", lookupValueList.Where(x => !string.IsNullOrWhiteSpace(x))));
                                }
                            }
                        }

                        if (invalidRequestTypes.Count > 0)
                        {
                            errorList.Add(string.Format("<h3>Question: {0}</h3>", question.QuestionTitle));
                            errorList.Add("<h4>Following request types are not found.</h4>");
                            errorList.Add(string.Format("<h4><b>{0}</b></h4>", string.Join("; ", invalidRequestTypes)));
                        }

                        question.QuestionTypePropertiesDicObj["requesttypes"] = string.Empty;
                        if (validRequestTypeIDs.Count > 0)
                            question.QuestionTypePropertiesDicObj["requesttypes"] = string.Join(Constants.Separator2, validRequestTypeIDs);
                    }
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    if (question.QuestionTypePropertiesDicObj.ContainsKey("usertype"))
                    {
                        List<string> validUserList = new List<string>();
                        List<string> inValidUserList = new List<string>();
                        string userValue = string.Empty;

                        string key = string.Empty;
                        if (question.QuestionTypePropertiesDicObj.ContainsKey("defaultval"))
                        {
                            userValue = question.QuestionTypePropertiesDicObj["defaultval"];
                            key = "defaultval";
                        }
                        else if (question.QuestionTypePropertiesDicObj.ContainsKey("specificusergroup"))
                        {
                            key = "specificusergroup";
                            userValue = question.QuestionTypePropertiesDicObj["specificusergroup"];
                        }

                        if (string.IsNullOrWhiteSpace(userValue))
                            continue;

                        string[] userValueList = userValue.Split(',');
                        if (userValueList.Count() > 0)
                        {
                            string strError = string.Empty;
                            List<UserProfile> validUsers = ValidateUser(service, userValue, ref strError);
                            if (validUsers != null && validUsers.Count > 0)
                                question.QuestionTypePropertiesDicObj[key] = string.Join(",", validUsers.Select(x => x.Name));
                            if (!string.IsNullOrWhiteSpace(strError))
                            {
                                errorList.Add(string.Format("<h3>Question: {0}</h3>", question.QuestionTitle));
                                errorList.Add(string.Format("<h4>User/group <b>{0}</b> not found or found multiple for Default Value.</h4>", strError));
                            }
                        }
                    }
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess)
                {
                    List<string> errors = new List<string>();
                    DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                    string sfOptions = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("application", out sfOptions);
                    if (sfOptions != null && sfOptions.ToLower() != "all")
                    {
                        List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                        List<string> applications = new List<string>();
                        foreach (string item in sfOptionList)
                        {
                            string applicationName = Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            DataRow[] drApps = null;
                            if (dtApplications != null && dtApplications.Rows.Count > 0)
                            {
                                if (applicationName.Contains(';'))
                                {
                                    drApps = dtApplications.Select(string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.Id, applicationName.Replace("'", "").Split(';')[0], DatabaseObjects.Columns.Title, applicationName.Replace("'", "").Split(';')[1]));
                                }
                                else
                                {
                                    LookupValueServiceExtension expVal = service.LookupValues.FirstOrDefault(x => x.ListName == DatabaseObjects.Tables.Applications && x.ID == applicationName);
                                    if (expVal != null)
                                        drApps = dtApplications.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, expVal.Value));
                                    else
                                        drApps = dtApplications.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, applicationName));
                                }
                            }

                            if (drApps == null || drApps.Length == 0)
                            {
                                errors.Add(string.Format("Application with Name <b>{0}</b> does not exist", applicationName));
                            }
                            else
                            {
                                int appID = UGITUtility.StringToInt(drApps[0][DatabaseObjects.Columns.Id]);
                                applications.Add(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0] + Constants.Separator2 + Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + appID);
                            }
                        }

                        question.QuestionTypePropertiesDicObj["application"] = string.Join(Constants.Separator1, applications.ToArray());

                        if (errors.Count > 0)
                        {
                            errorList.Add(string.Format("<h3>Question: {0}</h3>", question.QuestionTitle));
                            errorList.AddRange(errors.Select(x => string.Format("<h4>{0}</h4>", x)));
                        }
                    }
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                {
                    List<string> errors = new List<string>();
                    //specificuser
                    if (question.QuestionTypePropertiesDicObj.ContainsKey("specificuser"))
                    {
                        List<string> validUserList = new List<string>();
                        List<string> inValidUserList = new List<string>();
                        string userValue = string.Empty;

                        string key = key = "specificuser";
                        userValue = question.QuestionTypePropertiesDicObj["specificuser"];

                        if (!string.IsNullOrWhiteSpace(userValue))
                        {
                            string strError = string.Empty;
                            List<UserProfile> validUsers = ValidateUser(service, userValue, ref strError);
                            if (validUsers != null && validUsers.Count > 0)
                                question.QuestionTypePropertiesDicObj[key] = string.Join(",", validUsers.Select(x => x.Name));
                            if (!string.IsNullOrWhiteSpace(strError))
                            {
                                errors.Add(string.Format("<h4>User/group(s) <b>{0}</b> not found/multiple for Default Value.</h4>", strError));
                            }
                        }
                    }

                    //check asset type (request type)
                    if (question.QuestionTypePropertiesDicObj.ContainsKey("assettype"))
                    {
                        string module = "CMDB";
                        List<string> requestTypeIDs = UGITUtility.SplitString(question.QuestionTypePropertiesDicObj["assettype"], Constants.Separator6).ToList();
                        UGITModule moduleObj = moduleViewManager.LoadByName(module);
                        List<string> invalidRequestTypes = new List<string>();
                        List<string> validRequestTypeIDs = new List<string>();
                        foreach (string reqId in requestTypeIDs)
                        {
                            LookupValueServiceExtension lookserviceExtention = service.LookupValues.FirstOrDefault(x => x.ListName == DatabaseObjects.Tables.RequestType && x.ID == reqId);
                            if (lookserviceExtention != null)
                            {
                                List<string> lookupValueList = lookserviceExtention.Value.Split(new string[] { Constants.Separator9 }, StringSplitOptions.None).ToList();
                                if (lookupValueList != null)
                                {
                                    ModuleRequestType requestType = moduleObj.List_RequestTypes.FirstOrDefault(x => x.Category.ToLower() == lookupValueList[0].ToLower() && x.SubCategory.ToLower() == lookupValueList[1].ToLower() && x.RequestType.ToLower() == lookupValueList[2].ToLower());
                                    if (requestType != null)
                                        validRequestTypeIDs.Add(Convert.ToString(requestType.ID));
                                    else
                                        invalidRequestTypes.Add(string.Join(" > ", lookupValueList.Where(x => !string.IsNullOrWhiteSpace(x))));
                                }
                            }
                        }

                        if (invalidRequestTypes.Count > 0)
                        {
                            errors.Add("<h4 class='sb'>Following Asset types are not found.</h4>");
                            errors.AddRange(invalidRequestTypes.Select(x => string.Format("<h4>{0}</h4>", x)));
                        }

                        question.QuestionTypePropertiesDicObj["assettype"] = string.Empty;
                        if (validRequestTypeIDs.Count > 0)
                            question.QuestionTypePropertiesDicObj["assettype"] = string.Join(Constants.Separator6, validRequestTypeIDs);
                    }

                    if (errors.Count > 0)
                    {
                        errorList.Add(string.Format("<h3>Question: {0}</h3>", question.QuestionTitle));
                        errorList.AddRange(errors);
                    }
                }
            }
        }
        private List<UserProfile> ValidateUser(ServiceExtension service, string spFldUsers, ref string strError)
        {
            List<UserProfile> validUsers = new List<UserProfile>();
            List<string> strErrors = new List<string>();
            if (string.IsNullOrWhiteSpace(spFldUsers))
            {
                return validUsers;
            }

            if (service.UserInfo == null || service.UserInfo.Count == 0)
            {
                strError = "User information not avaible.";
                return validUsers;
            }


            List<string> users = new List<string>();
            string seperator = Constants.Separator;
            if (spFldUsers.IndexOf(seperator) != -1) //contain id;#value
                users = UGITUtility.ConvertStringToList(spFldUsers, seperator);
            else
                users = UGITUtility.ConvertStringToList(spFldUsers, Constants.Separator6);


            List<string> invalidUsers = new List<string>();
            UserProfileManager userProfileManager = new UserProfileManager(_context);
            foreach (string spUser in users)
            {
                UserProfile userInfo = null;
                if (!string.IsNullOrWhiteSpace(spUser))
                {
                    userInfo = service.UserInfo.FirstOrDefault(x => x.Id == spUser);
                    if (userInfo != null)
                    {
                        validUsers.Add(userInfo);
                    }
                    else
                    {
                        invalidUsers.Add(spUser);
                    }
                }
            }

            if (invalidUsers.Count > 0)
            {
                strError = string.Join("; ", invalidUsers);
            }
            return validUsers;
        }
        void ValidateServiceVariables(ServiceExtension service)
        {
            #region Variables
            string strError = string.Empty;
            List<string> errors = new List<string>();
            if (service.QMapVariables == null)
            {
                return;
            }

            //correctly export user type of variables
            List<QuestionMapVariable> mapVariables = service.QMapVariables.Where(x => x.Type == Constants.ServiceQuestionType.USERFIELD).ToList();
            foreach (QuestionMapVariable mVariable in mapVariables)
            {
                strError = string.Empty;
                if (mVariable.DefaultValue != null && mVariable.DefaultValue.IsPickFromConstant &&
                    !string.IsNullOrWhiteSpace(mVariable.DefaultValue.PickFrom))
                {
                    strError = string.Empty;
                    List<UserProfile> uColl = ValidateUser(service, mVariable.DefaultValue.PickFrom, ref strError);
                    if (uColl != null && uColl.Count > 0)
                        mVariable.DefaultValue.PickFrom = string.Join(Constants.Separator6, uColl);

                    if (!string.IsNullOrWhiteSpace(strError))
                    {
                        errors.Add(string.Format("User/group <b>{0}</b> are not found or found multiple for Default Value.", strError));
                    }
                }

                if (mVariable.VariableValues != null)
                {
                    foreach (VariableValue v in mVariable.VariableValues)
                    {
                        if (v.IsPickFromConstant && !string.IsNullOrWhiteSpace(v.PickFrom))
                        {
                            strError = string.Empty;
                            List<UserProfile> uColl = ValidateUser(service, v.PickFrom, ref strError);
                            if (uColl != null && uColl.Count > 0)
                                v.PickFrom = string.Join(Constants.Separator6, uColl);

                            if (!string.IsNullOrWhiteSpace(strError))
                            {
                                errors.Add(string.Format("User/group <b>{0}</b> not found or found multiple for Default Value of one of the condition.", strError));
                            }
                        }

                        if (v.Conditions != null)
                        {
                            foreach (WhereExpression exp in v.Conditions)
                            {
                                ServiceQuestion question = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == exp.Variable.ToLower());
                                if (question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                                {
                                    strError = string.Empty;
                                    List<UserProfile> uColl = ValidateUser(service, exp.Value, ref strError);
                                    exp.Value = string.Empty;
                                    if (uColl != null && uColl.Count > 0)
                                        exp.Value = uColl.First().Id;

                                    if (!string.IsNullOrWhiteSpace(strError))
                                        errors.Add(string.Format("User/group <b>{0}</b> not found/multiple for condition.", strError));
                                }
                            }
                        }
                    }
                }

                if (errors.Count > 0)
                {
                    errorList.Add(string.Format("<h3>Variable: {0}</h3>", mVariable.Title));
                    errorList.AddRange(errors.Select(x => string.Format("<h4>{0}</h4>", x)));
                }
            }

            #endregion
        }
        void ValidateServiceTaskMapping(ServiceExtension serviceExtension)
        {
            if (serviceExtension.QuestionsMapping == null)
                return;

            string strError = string.Empty;
            var mapLookup = serviceExtension.QuestionsMapping.ToLookup(x => x.ServiceTaskID);
            List<string> errors = new List<string>();
            foreach (var map in mapLookup)
            {
                errors = new List<string>();
                List<ServiceQuestionMapping> taskMaps = map.ToList();
                long taskID = map.Key ?? 0;
                string moduleName = string.Empty;
                UGITTask task = null;
                if (taskID > 0)
                {
                    task = serviceExtension.Tasks.FirstOrDefault(x => x.ID == taskID);
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
                    ULog.WriteLog(string.Format("module name found while looking into module"), "ValidateServiceTaskMapping");
                    continue;
                }


                if (moduleName == "_Task") // map is against task
                {
                    ServiceQuestionMapping qmap = taskMaps.FirstOrDefault(x => x.ColumnName == DatabaseObjects.Columns.AssignedTo);
                    if (qmap != null && !string.IsNullOrWhiteSpace(qmap.ColumnName) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                    {
                        strError = string.Empty;
                        List<UserProfile> userLookups = ValidateUser(serviceExtension, qmap.ColumnValue, ref strError);
                        if (userLookups != null && userLookups.Count > 0)
                            qmap.ColumnValue = string.Join(Constants.Separator6, userLookups);
                        if (!string.IsNullOrWhiteSpace(strError))
                        {
                            errors.Add(string.Format("<h4>{0}: User/group <b>{1}</b> are not fount or found multiple.</h4>", qmap.ColumnName, strError));
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
                    if (moduleDataList == null)
                    {
                        ULog.WriteLog(string.Format("Module data list not found"), "ExportServiceQuestionMapping");
                        continue;
                    }

                    foreach (ServiceQuestionMapping qmap in taskMaps)
                    {
                        if (!UGITUtility.IfColumnExists(qmap.ColumnName, moduleDataList))
                            continue;

                        strError = string.Empty;
                        if (qmap.ColumnName.EndsWith("user", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(qmap.ColumnValue) && qmap.ColumnValue.IndexOf(Constants.Separator) != 1)
                        {
                            List<UserProfile> userLookups = ValidateUser(serviceExtension, qmap.ColumnValue, ref strError);
                            if (userLookups != null && userLookups.Count > 0)
                                qmap.ColumnValue = string.Join(Constants.Separator6, userLookups);
                            if (!string.IsNullOrWhiteSpace(strError))
                            {
                                errors.Add(string.Format("<h4>{0} ({2}): User/group <b>{1}</b> are not fount or found multiple.</h4>", qmap.ColumnName, strError, qmap.ColumnValue));
                            }
                        }
                    }
                }

                if (errors.Count > 0)
                {
                    string type = moduleName;
                    if (moduleName == "_Task")
                        type = "Task";

                    string title = serviceExtension.Title;
                    if (task != null)
                        title = task.Title;

                    errorList.Add(string.Format("<h3>Mapping: {0}:{1}</h3>", type, title));
                    errorList.AddRange(errors);
                }
            }
        }
        string RemoveNameSpace(string xmlText)
        {
            List<string> vs = new List<string>() { "Module", "Service", "RequestCategory" };
            XElement xElement = XElement.Parse(xmlText);
            foreach (XElement XE in xElement.DescendantsAndSelf())
            {
                // Stripping the namespace by setting the name of the element to it's localname only
                XE.Name = XE.Name.LocalName;
                if (vs.Any(x => x.EqualsIgnoreCase(XE.Name.LocalName)))
                {

                }
                //if (XE.Name == "ServiceExtension")
                //    continue;
                // replacing all attributes with attributes that are not namespaces and their names are set to only the localname
                //XE.ReplaceAttributes((from xattrib in XE.Attributes().Where(xa =>xa.Name!= "ServiceExtension" && !xa.IsNamespaceDeclaration) select new XAttribute(xattrib.Name.LocalName, xattrib.Value)));
            }

            return xElement.ToString();
        }

        private void UpdateLooupField(Services service)
        {
            //Set Lookup fields
            if (service != null)
            {
                UGITModule moduleObj = null;
                UGITTaskManager uGITTaskManager = null;
                UGITTask uGITTask = null;
                ModuleImpact moduleImpact = null;
                ModuleSeverity moduleSeverity = null;
                service.QuestionsMapping.ForEach(x =>
                {
                    if (x.ColumnName.EndsWith("lookup", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (x.ServiceTaskID > 0)
                        {
                            uGITTaskManager = new UGITTaskManager(_context);
                            uGITTask = uGITTaskManager.LoadByID(x.ServiceTaskID.Value);
                            if (uGITTask != null && !string.IsNullOrWhiteSpace(uGITTask.RelatedModule))
                            {
                                moduleObj = moduleViewManager.LoadByName(uGITTask.RelatedModule);
                                if (moduleObj != null)
                                {
                                    if (x.ColumnName == DatabaseObjects.Columns.TicketImpactLookup)
                                    {
                                        moduleImpact = moduleObj.List_Impacts.FirstOrDefault(y => y.Impact.Equals(x.ColumnValue, StringComparison.InvariantCultureIgnoreCase));
                                        if (moduleImpact != null)
                                            x.ColumnValue = UGITUtility.ObjectToString(moduleImpact.ID);
                                    }
                                    else if (x.ColumnName == DatabaseObjects.Columns.TicketSeverityLookup)
                                    {
                                        moduleSeverity = moduleObj.List_Severities.FirstOrDefault(y => y.Severity.Equals(x.ColumnValue, StringComparison.InvariantCultureIgnoreCase));
                                        if (moduleSeverity != null)
                                            x.ColumnValue = UGITUtility.ObjectToString(moduleSeverity.ID);
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}
