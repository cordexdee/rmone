using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Manager;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using uGovernIT.Web.Helpers;
using uGovernIT.Utility.Entities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.DefaultConfig;
using uGovernIT.DataTransfer;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;

namespace uGovernIT.Web.Controllers
{
    public class ApplicationRegistrationRequestController : Controller
    {
        private ApplicationContext _applicationContext;
        private TenantManager _tenantManager;
        private UserProfileManager _userProfileManager;
        private UserProfileManager _userManager;
        private TenantRegistrationManager _tenantRegistrationManager;
        private TenantRegistration _tenantRegistration;
        private TenantRegistrationData _tenantRegistrationData;
        private TenantValidation _tenantValidation;
        public UserProfileManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserProfileManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRegistrationRequestController()
        {
            _applicationContext = ApplicationContext.Create();
            _tenantManager = new TenantManager(_applicationContext);
            _tenantRegistrationManager = new TenantRegistrationManager(_applicationContext);
            _tenantRegistration = new TenantRegistration();
            _tenantRegistrationData = new TenantRegistrationData();
            _tenantValidation = new TenantValidation(_applicationContext);

            var defaultTenant = _tenantManager.GetTenantList().SingleOrDefault(y => y.AccountID.Equals(ConfigHelper.DefaultTenant, StringComparison.InvariantCultureIgnoreCase));

            _applicationContext.TenantID = Convert.ToString(defaultTenant.TenantID);
            _applicationContext.TenantAccountId = Convert.ToString(defaultTenant.AccountID);

            ViewBag.AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public ActionResult Index()
        {
            SetViewBagData();

            return View();
        }

        public ActionResult Tenant()
        {
            SetViewBagData();

            return View();
        }

        public void SetViewBagData()
        {
            try
            {
                string ID = Convert.ToString(Request.QueryString["tid"]);

                if (!string.IsNullOrEmpty(ID))
                {
                    string data = QueryString.Decode(ID);
                    string[] arr = data.Split('&');

                    ViewBag.TicketID = Convert.ToString(arr[0]);
                    ViewBag.Email = Convert.ToString(arr[1]);
                }
                
                var serviceManager = new ServicesManager(_applicationContext);
                TenantOnBoardingHelper tenantOnBoardingHelper = new TenantOnBoardingHelper(_applicationContext);
                var listNewTenantServiceTitle = tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles();
                var services = serviceManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
                ViewBag.ServiceID = serviceManager.LoadAllServices("services").Where(x => listNewTenantServiceTitle.Any(y => y.EqualsIgnoreCase(x.Title)) && x.Deleted == false).Select(x => x.ID).FirstOrDefault();
                // ViewBag.ServiceID = serviceManager.LoadAllServices("services").Where(x => x.Title.Equals(ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].ToString())).Select(x => x.ID).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        public ActionResult RegistrationStatus(ApplicationRegistrationRequest requestTicketStatus)
        {
            var updateResult = new UpdateResult();            

            try
            {
                var serviceInput = new ServiceInput();
                var moduleViewManager = new ModuleViewManager(_applicationContext);
                //var serviceManager = new ServicesManager(_applicationContext);

                if (string.IsNullOrEmpty(requestTicketStatus.EmailID) || string.IsNullOrEmpty(requestTicketStatus.TicketID))
                {
                    updateResult.message = "Please enter valid Email Id and Ticket Id";
                    return Json(updateResult);
                }

                //List<Services> services = serviceManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
                var moduleName = "SVC";
                var module = moduleViewManager.LoadByName(moduleName);

                //var tableData = GetTableDataManager.GetTableData(module.ModuleTable, DatabaseObjects.Columns.TicketId + " = '" + requestTicketStatus.TicketID.Trim() + "' and " + DatabaseObjects.Columns.Title + " = '" + ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].ToString() + "'");
                var tableData = GetTableDataManager.GetTableData(module.ModuleTable, $"{DatabaseObjects.Columns.TicketId}='{requestTicketStatus.TicketID.Trim()}'");  //Above line commented as it is creating problem when 'Registration of new tenant' sevice has custom mapping in Service & Catalog Agents.

                if (tableData.Rows.Count > 0)
                {
                    var item = tableData.Rows[0];
                    string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.UserQuestionSummary));

                    if (!string.IsNullOrEmpty(questionInputs))
                    {
                        var doc = new XmlDocument();
                        Dictionary<string, string> queAns = new Dictionary<string, string>();

                        doc.LoadXml(questionInputs.Trim());

                        serviceInput = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                        serviceInput.ServiceSections.ForEach(x => x.Questions.ForEach(y => { queAns.Add(y.Token, y.Value); }));

                        if (requestTicketStatus.EmailID.Trim().Equals(queAns["email"], StringComparison.CurrentCultureIgnoreCase))// && requestTicketStatus.TicketID.Equals(queAns["companyname"],StringComparison.CurrentCultureIgnoreCase))
                        {
                            string ticketId = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.TicketId));
                            var taskManager = new UGITTaskManager(_applicationContext);
                            var subtask = taskManager.LoadByProjectID(ticketId);
                            var status = new Dictionary<string, string>();

                            subtask.ForEach(x => { status.Add(Regex.Replace(x.Title, @"\s+", ""), Regex.Replace(x.Status.ToLower(), @"\s+", "_")); });

                            var jsonStatus = Newtonsoft.Json.JsonConvert.SerializeObject(status);

                            updateResult.data = jsonStatus;
                            updateResult.success = true;
                        }
                    }
                }

                updateResult.message = updateResult.success == false ? updateResult.message = "Ticket ID or Email ID is invalid" : "";
            }
            catch (Exception ex)
            {
                updateResult.success = false;
                updateResult.message = "Error to fetch status";

                Util.Log.ULog.WriteException(ex);
            }

            return Json(updateResult);
        }

        public ActionResult UserVerfication(string id, string acc, string di)
        {
            try
            {
                string errMessage = string.Empty;
                var signinManager = HttpContext.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                //string identify = "";
                string Enabled = "";
                acc = acc.Replace(" ", "+");
                id = id.Replace(" ", "+");
                di = di.Replace(" ", "+");

                string userId = uGovernITCrypto.Decrypt(id, "ugitpass");
                string accountId = uGovernITCrypto.Decrypt(acc, "ugitpass");
                string pd = uGovernITCrypto.Decrypt(di, "ugitpass");
                string userName = string.Empty;
                String TenantID = String.Empty;
                if (!string.IsNullOrEmpty(userId))
                {
                    _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                    //UserProfile user = _userProfileManager.LoadById(userId);

                    DataTable tenantData = GetTableDataManager.GetTenantDataUsingQueries($"select * from {DatabaseObjects.Tables.Tenant} where {DatabaseObjects.Columns.Deleted} = '1' and TenantID = '{accountId}' ");

                    if (tenantData != null && tenantData.Rows.Count > 0)
                    {
                        ViewBag.Info = Constants.UserAccountMessage.InactiveUser;
                        return View("TenantLogin");
                    }

                    string Query = $"Select * from AspNetUsers  where Id = '{userId}' and TenantID = '{accountId}'";
                    DataTable userData = GetTableDataManager.ExecuteQuery(Query);

                    if (userData != null && userData.Rows.Count > 0)
                    {
                        Enabled = userData.Rows[0]["Enabled"].ToString();
                        userName = userData.Rows[0][DatabaseObjects.Columns.UserName].ToString();
                        TenantID = userData.Rows[0][DatabaseObjects.Columns.TenantID].ToString();
                    }
                    

                    if (Enabled == "False")
                    {
                        Query = $"Update AspNetUsers Set Enabled = 1 where Id = '{userId}' and TenantID = '{accountId}';";
                        var success = GetTableDataManager.ExecuteNonQuery(Query);

                        //identify = "true";
                        ViewBag.identifyact = "activated";
                        ViewBag.identify = "";
                    }
                    
                    if(TenantID != null)
                    {
                        _applicationContext = ApplicationContext.CreateContext(TenantID);
                        UserProfile user = new UserProfile();
                        UserProfileManager userProfileManager = new UserProfileManager(_applicationContext);
                        UserProfile userProfile = userProfileManager.LoadById(userId);
                        if (userProfile != null)
                        {
                            _userProfileManager.UpdateIntoCache(userProfile);
                            signinManager.SignInAsync(userProfile, true, true);
                        }
                    }
                    string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
                    Response.Redirect(siteUrl);



                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

            return View(ViewBag);
        }

        public ActionResult TenantLogin(string id, string acc, string di)
        {
            try
            {
                string errMessage = string.Empty;
                var signinManager = HttpContext.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                acc = acc.Replace(" ", "+");
                id = id.Replace(" ", "+");
                di = di.Replace(" ", "+");

                string tenantId = string.Empty;                
                string accountIdForMail = uGovernITCrypto.Decrypt(id, "ugitpass");
                string userNameForMail = uGovernITCrypto.Decrypt(acc, "ugitpass");
                string passwordForMail = uGovernITCrypto.Decrypt(di, "ugitpass");
                
                var tenantData = GetTableDataManager.IsExist($"select * from {DatabaseObjects.Tables.Tenant} where  AccountId = '{accountIdForMail}'",true);

                if (tenantData == false)
                {
                        errMessage = Constants.UserAccountMessage.IsTenantDeleted;
                        ViewBag.Info = errMessage;

                        Util.Log.ULog.WriteLog($"errMessage: {errMessage}");

                        return View();
                }
                if (tenantData == true)
                {
                    var tenantDataFetchTenantId = GetTableDataManager.GetTenantDataUsingQueries($"select * from {DatabaseObjects.Tables.Tenant} where  AccountId = '{accountIdForMail}'");
                    if (tenantDataFetchTenantId!= null)
                    {
                        tenantId = tenantDataFetchTenantId.Rows[0]["TenantId"].ToString();
                        _applicationContext = ApplicationContext.CreateContext(tenantId);
                        UserProfile user = new UserProfile();
                        //user = _applicationContext.CurrentUser;
                        UserProfileManager userProfileManager1 = new UserProfileManager(_applicationContext);
                        user = userProfileManager1.GetUserByUserName(userNameForMail);

                        signinManager.SignInAsync(user, true, true);
                        string landingPageUrl = new LandingPagesManager(_applicationContext).LandingPagesfinal(user, "frommail");
                        //string siteUrl = landingPageUrl.Contains(ConfigurationManager.AppSettings["SiteUrl"]) ? landingPageUrl : ConfigurationManager.AppSettings["SiteUrl"] + landingPageUrl;
                        string siteUrl = landingPageUrl;
                        Response.Redirect(siteUrl);
                    }
                    
                }

                //-----In future use the below method as it check status of user as well--but we need to over load this method----//
                //var result = signinManager.PasswordSignIn(userNameForMail.Trim(), passwordForMail, true, shouldLockout: false);
                //if (result == SignInStatus.Success)
                //{
                //}
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }

            return new EmptyResult();
        }
        
        public ActionResult TenantCreation(string id)
        {
            var result = new UpdateResult();
            var defaultConfigManager = new DefaultConfigManager();
            string accountId = string.Empty;
            string errorMesg = string.Empty;
            bool companyExist = false;
            bool emailExist = false;

            try
            {
                _tenantRegistration = _tenantRegistrationManager.LoadByID(Convert.ToInt64(id));
                _tenantRegistrationData = Newtonsoft.Json.JsonConvert.DeserializeObject<TenantRegistrationData>(_tenantRegistration.TenantRegistrationData);
                _userProfileManager = System.Web.HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                if (_tenantRegistration != null && _tenantRegistration.Deleted)
                {
                    errorMesg = QueryString.Encode("Tenant already created.");
                }
                else if(_tenantValidation.IsNewTenantCreationLimitExceded())
                {
                    errorMesg = QueryString.Encode("New Tenant creation exceeded daily limit. Please try again tomorrow or contact Help Desk.");
                }
                else if (_tenantRegistrationData != null)
                {
                    var tenantOnBoardingHelper = new TenantOnBoardingHelper(_applicationContext);
                    companyExist = tenantOnBoardingHelper.IsCompanyExist(_tenantRegistrationData.Company);
                    emailExist = tenantOnBoardingHelper.IsEmailExist(_tenantRegistrationData.Email);

                    if (!companyExist && !emailExist)
                    {
                        if(_tenantRegistrationData != null && _tenantRegistrationData.TenantCreationStarted == false)
                        {
                            _tenantRegistrationData.TenantCreationStarted = true;
                            _tenantRegistration.TenantRegistrationData = Newtonsoft.Json.JsonConvert.SerializeObject(_tenantRegistrationData);
                            _tenantRegistrationManager.Update(_tenantRegistration);
                        }
                        else if(_tenantRegistrationData != null && _tenantRegistrationData.TenantCreationStarted == true)
                        {
                            errorMesg = QueryString.Encode("New Tenant creation process already started.");
                            Response.Redirect("/ControlTemplates/RegistrationSuccessm.aspx?ve=true&cm=" + $"{companyExist.ToString().ToLower()}&em={emailExist.ToString().ToLower()}&msg={errorMesg}&tn={_tenantRegistrationData.Company}", true);
                        }

                        accountId = TenantOnBoardingHelper.RemoveSpaceandSpecialchar(_tenantRegistrationData.Company);
                        AccountInfo accountInfo = defaultConfigManager.GetAdminAccountInfo(accountId, _tenantRegistrationData.Email, _userProfileManager.GeneratePassword());

                        Tenant model = new Tenant
                        {
                            AccountID = accountId,
                            TenantName = accountId, //_tenantRegistrationData.Company,
                            TenantUrl = _tenantRegistrationData.Url,
                            Email = _tenantRegistrationData.Email,
                            SelfRegisteredTenant = true,
                            Name = _tenantRegistrationData.Name,
                            Title = _tenantRegistrationData.Title,
                            Contact = _tenantRegistrationData.Contact
                        };

                        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(_applicationContext);
                        //List<string> modules = ObjModuleViewManager.LoadAllModule().Where(x => x.EnableModule == true).Select(x => x.ModuleName).OrderBy(x => x).ToList();
                        ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(_applicationContext);
                        string ModelSite = ObjConfigVariable.GetValue(ConfigConstants.ModelSite);
                        if (!string.IsNullOrEmpty(ModelSite))
                        {
                            //string selectedConfig = "Location,Departments,FunctionalAreas,Roles,Modules,BudgetsCategory,ConfigurationVariables,DefaultEntries,WikiCategories,StageExitCriteriaTemplates,DashboardAndQueries,FactTables,QuickTickets,MessageBoard,UserSkills,ServiceCatalogAndAgents,Environment,SubLocation,ProjectLifecycles,ProjectInitiative,ProjectClass,ProjectStandards,GlobalRoles,LandingPages,JobTitle,EmployeeTypes,GovernanceConfiguration,ModuleMonitors,ModuleMonitorOptions,ProjectComplexity,MailTokenColumnName,GenericStatus,LinkViews,SurveyFeedback,Phrases,Widgets,ChecklistTemplates,LeadCriteria,RankingCriteria,Studio,State";
                            string selectedConfig = "FunctionalAreas,Modules,BudgetsCategory,CRMRelationshipTypeLookup,ConfigurationVariables,DefaultEntries,WikiCategories,StageExitCriteriaTemplates,DashboardAndQueries,Attachments,FactTables,QuickTickets,AssetVendors,AssetModels,MessageBoard,UserSkills,ServiceCatalogAndAgents,ACRTypes,DRQRapidTypes,DRQSystemAreas,Environment,SubLocation,ProjectLifecycles,ProjectInitiative,ProjectClass,ProjectStandards,LandingPages,EmployeeTypes,GovernanceConfiguration,ModuleMonitors,ModuleMonitorOptions,ProjectComplexity,MailTokenColumnName,GenericStatus,LinkViews,TenantScheduler,SurveyFeedback,Phrases,Widgets,ChecklistTemplates,LeadCriteria,RankingCriteria,State";

                            //string selectedModules = string.Join(Constants.Separator6, modules);  //"TSR,ACR,RCA,PRS,DRQ,INC,BTS,APP,TSK,SVC,WIKI,NPR,PMM,CMDB,HLP,CMT,COM,CON,LEM,OPM,CPR,CNS";

                            string configPath = Path.Combine(uHelper.GetDataMigrationTemplate());
                            FileInfo configFileInfo = new FileInfo(configPath);
                            string json = configFileInfo.OpenText().ReadToEnd();
                            //JObject jo = JObject.Parse(json);
                            JToken jToken = JToken.Parse(json);

                            List<string> lstModulesList = jToken["modules"].Select(x => x).Select(x => (string)x["name"]).Distinct().ToList();
                            string selectedModules = string.Join(Constants.Separator6, lstModulesList);

                            DMTenant sourceTenant = new DMTenant();
                            sourceTenant.tenantid = ModelSite;
                            sourceTenant.tenantname = ModelSite;
                            sourceTenant.dbconnection = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]);
                            sourceTenant.commondbconnection = Convert.ToString(ConfigurationManager.ConnectionStrings["tenantcnn"]);

                            DMTenant targetTenant = new DMTenant();
                            targetTenant.tenantid = accountInfo.AccountID;
                            targetTenant.tenantname = model.Name;
                            targetTenant.dbconnection = Convert.ToString(ConfigurationManager.ConnectionStrings["cnn"]); 
                            targetTenant.commondbconnection = Convert.ToString(ConfigurationManager.ConnectionStrings["tenantcnn"]);
                            targetTenant.username = accountInfo.UserName;
                            targetTenant.password = accountInfo.Password;
                            targetTenant.userEmail = model.Email;
                            targetTenant.TenantEmail = model.Email;
                            targetTenant.contact = model.Contact;
                            targetTenant.title = model.Title;
                            targetTenant.SelfRegisteredTenant = model.SelfRegisteredTenant;
                            targetTenant.url = model.TenantUrl;

                            Thread createTenantThread = new Thread(delegate ()
                            {
                                ImportManager manager = new ImportManager(selectedConfig, selectedModules, sourceTenant, targetTenant);
                                manager.Excute("dtd", false);

                                _tenantRegistration.Deleted = true;
                                _tenantRegistrationManager.Update(_tenantRegistration);

                                var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(accountInfo.Email, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                                var saResponse = new EmailHelper(_applicationContext).SendEmailToSuperAdmin(accountInfo);
                            });

                            createTenantThread.Start();
                        }
                        else
                        {
                            defaultConfigManager.CreateTenantCommon(model, accountInfo, _applicationContext);

                            _tenantRegistration.Deleted = true;
                            _tenantRegistrationManager.Update(_tenantRegistration);
                        }
                    }
                }

                Response.Redirect("/ControlTemplates/RegistrationSuccessm.aspx?ve=true&cm=" + $"{companyExist.ToString().ToLower()}&em={emailExist.ToString().ToLower()}&msg={errorMesg}&tn={_tenantRegistrationData.Company}");
            }
            catch (Exception ex)
            {
                result.message = ex.Message;

                Util.Log.ULog.WriteException(ex);
            }

            return Json(result);
        }
    }
}