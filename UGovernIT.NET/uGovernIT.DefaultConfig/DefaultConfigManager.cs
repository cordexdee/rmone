using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.DAL;
using uGovernIT.Manager.Core;
using uGovernIT.DefaultConfig.Data.DefaultData;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
using System.Threading;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Util.Cache;

namespace uGovernIT.DefaultConfig
{
    public class GlobalVar
    {
        public static string TenantID { get; set; }
    }




    public class DefaultConfigManager
    {
        public void CreateTenantCommon(Tenant model, AccountInfo accountInfo, ApplicationContext _applicationContext)
        {
            ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(_applicationContext);
            var resultTenantCreation = new UpdateResult();
            try
            {
                Thread createTenantThread = new Thread(delegate ()
                {
                    resultTenantCreation = CreateTenantInfo(model, "all", accountInfo);

                    if (resultTenantCreation.success)
                    {
                        CacheHelper<object>.Delete(string.Format("Available_Tenants"));
                        string forwardMailAddress = ObjConfigVariable.GetValue(ConfigConstants.ForwardMailAddress);

                        if (!string.IsNullOrEmpty(forwardMailAddress))
                        {
                            var lstforwardMailAddress = forwardMailAddress.Split(',');
                            foreach (string strMail in lstforwardMailAddress)
                            {
                                var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(strMail, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                            }
                        }
                        else
                        {
                            var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(accountInfo.Email, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                        }

                    }
                });

                createTenantThread.Start();

            }
            catch (Exception exception)
            {
                ULog.WriteException(exception);
            }
        }


        // for creating tenant using super admin
        public UpdateResult CreateTenantInfo(Tenant tenantInfo, string moduleNames, AccountInfo accountInfo)
        {
            var result = new UpdateResult();

            try
            {
                moduleNames = moduleNames.Trim().ToLower(); // consider as "all"(input) 
                moduleNames = moduleNames == "all" ? string.Empty : moduleNames;

                //Find and call script class using reflection
                var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                                where t.GetInterfaces().Contains(typeof(IModule))
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                select Activator.CreateInstance(t) as IModule;

                if (!string.IsNullOrWhiteSpace(moduleNames))
                {
                    string[] modules = moduleNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower().Trim()).ToArray();
                    instances = instances.Where(x => modules.Contains(x.ModuleName.ToLower())).ToList();
                }

                // Adding new tenant's in common database
                var tenant = InsertConfigTenant(tenantInfo);
                if (tenant != null)
                {
                   
                    var applicationContext = ApplicationContext.Create();

                    GlobalVar.TenantID = applicationContext.TenantID = tenant.TenantID.ToString();
                    accountInfo.Name = tenantInfo.Name; //added to set tenant name as defaukt user name 

                    foreach (var instance in instances)
                    {
                        LoadModuleConfiguration(instance, applicationContext);
                    }

                    LoadCommanConfiguration(applicationContext, tenant.AccountID, tenant.TenantName, accountInfo, false);

                    ImportServiceHelper importServiceHelper = new ImportServiceHelper(applicationContext);
                    importServiceHelper.LoadService();

                    LoadRemainingDefaultData(applicationContext);

                    result.success = true;
                }
                else
                {
                    result.message = "Account ID is already exists. Please enter different Account ID";
                }
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                ULog.WriteException(ex);
            }

            return result;
        }


        public static Tenant InsertConfigTenant(Tenant tenantInfo)
        {
            return CreateTenant(tenantInfo.AccountID, tenantInfo.TenantName, tenantInfo.TenantUrl, tenantInfo.Email, tenantInfo.IsOffice365Subscription, tenantInfo.SelfRegisteredTenant, tenantInfo.Name, tenantInfo.Contact, tenantInfo.Title);
        }

        public static Tenant InsertConfigTenant(string accountId)
        {
            return CreateTenant(accountId, accountId, "", "", false);
        }

        public AccountInfo GetAdminAccountInfo(string accountId, string email, string password = null)
        {
            return ConfigData.GetAdminAccountInfo(accountId, email, password);
        }

        private static Tenant CreateTenant(string accountId, string tenantName, string tenantUrl, string email, bool office365Subscription, bool? selfRegisteredTenant = null, string name = null, string contact = null, string title = null)
        {
            Guid _guid = Guid.NewGuid();

            var tenant = new Tenant()
            {
                TenantID = _guid,
                TenantName = !string.IsNullOrEmpty(tenantName) ? tenantName : accountId,
                AccountID = accountId,
                Country = "India",
                TenantUrl = !string.IsNullOrEmpty(tenantUrl) ? tenantUrl : string.Empty,
                DBName = string.Empty,
                DBServer = string.Empty,
                Deleted = false,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                CreatedByUser = _guid.ToString(),
                ModifiedByUser = _guid.ToString(),
                IsOffice365Subscription = office365Subscription,
                Email = email,
                SelfRegisteredTenant = selfRegisteredTenant,
                Name = !string.IsNullOrEmpty(name) ? name : accountId, //name,
                Contact = contact,
                Title = title
            };

            // verify InsertItem if points to common db
            var status = uGITDAL.InsertItemComman(tenant);
            if (status == 0)
            {
                return null;
            }

            List<Tenant> tenants = (List<Tenant>)Util.Cache.CacheHelper<object>.Get(string.Format("Available_Tenants"));
            tenants.Add(tenant);
            CacheHelper<object>.AddOrUpdate("Available_Tenants", tenants);

            return tenant;
        }

        public static void LoadModuleConfiguration(IModule module, ApplicationContext context)
        {
            if (module is IDashboard)
                return;

            var moduleData = module.Module;

            var moduleViewManager = new ModuleViewManager(context);
            moduleViewManager.Insert(moduleData);

            var moduleFormTabManager = new ModuleFormTabManager(context);
            moduleFormTabManager.InsertItems(module.GetFormTabs());

            //  uGITDAL.InsertItem(module.GetFormTabs());
            var moduleSeverities = new List<ModuleSeverity>();
            var priorities = new List<ModulePrioirty>();
            var impacts = new List<ModuleImpact>();
            var rPriorities = new List<ModulePriorityMap>();

            module.GetPriorityMapping(ref impacts, ref moduleSeverities, ref priorities, ref rPriorities);

            var severityManager = new SeverityManager(context);
            var prioirtyViewManager = new PrioirtyViewManager(context);
            var impactManager = new ImpactManager(context);
            var requestPriorityManager = new RequestPriorityManager(context);

            impactManager.InsertItems(impacts);
            severityManager.InsertItems(moduleSeverities);
            prioirtyViewManager.InsertItems(priorities);

            rPriorities = RequestPriorities(impacts, moduleSeverities, priorities, rPriorities);
            requestPriorityManager.InsertItems(rPriorities);

            var lifeCycleManager = new LifeCycleStageManager(context);
            lifeCycleManager.InsertItems(module.GetLifeCycleStage());

            var moduleColumnManager = new ModuleColumnManager(context);
            moduleColumnManager.InsertItems(module.GetModuleColumns());

            var moduleDefaultValueManager = new ModuleDefaultValueManager(context);
            moduleDefaultValueManager.InsertItems(module.GetModuleDefaultValue());

            var moduleUserTypeManager = new ModuleUserTypeManager(context);
            moduleUserTypeManager.InsertItems(module.GetModuleUserType());

            var moduleFormLayoutManager = new ModuleFormLayoutManager(context);
            moduleFormLayoutManager.InsertItems(module.GetModuleFormLayout());

            var requestRoleWriteAccessManager = new RequestRoleWriteAccessManager(context);
            requestRoleWriteAccessManager.InsertItems(module.GetModuleRoleWriteAccess());

            var requestTypeManager = new RequestTypeManager(context);
            requestTypeManager.InsertItems(module.GetModuleRequestType());

            var taskEmailViewManager = new TaskEmailViewManager(context);
            taskEmailViewManager.InsertItems(module.GetModuleTaskEmail());

            var aCRTypeManager = new ACRTypeManager(context);
            aCRTypeManager.InsertItems(module.GetACRTypes());

            var drqRapidTypesManager = new DrqRapidTypesManager(context);
            drqRapidTypesManager.InsertItems(module.GetDRQRapidTypes());

            var drqSyetemAreaViewManager = new DrqSyetemAreaViewManager(context);
            drqSyetemAreaViewManager.InsertItems(module.GetDRQSystemAreas());

            var tabViewManager = new TabViewManager(context);
            tabViewManager.InsertItems(module.UpdateTabView());

            var defaultD = new ConfigData(context);
            if (module.ModuleName == "PMM")
                defaultD.InsertProjectLifeCycles(context);
            defaultD.GetWikiContents(context);
            NavigationUrl(context);
        }

        public static void LoadCommanConfiguration(ApplicationContext applicationContext, string accountId, string tenantName, AccountInfo accountInfo, bool createSuperAdmin)
        {
            try
            {
                var defaultD = new ConfigData(applicationContext);

                LandingPages landingPage = new LandingPages();
                LandingPagesManager ObjLandingPagesManager = new LandingPagesManager(applicationContext);
                //landingPage.Title = "Admin";
                //landingPage.Name = "Admin";
                //landingPage.Description = "Admin";
                //landingPage.LandingPage = "/Pages/userhomepage";
                List<LandingPages> lstLandingPage = defaultD.listOfLandingPages();
                ObjLandingPagesManager.InsertItems(lstLandingPage);
                landingPage = ObjLandingPagesManager.GetRoleByName("Admin");

                applicationContext.TenantAccountId = tenantName;// for showing name in  page header comapny name so 
                                                                // create user and user roles and global Role
                defaultD.GetGlobalRoles(applicationContext);
                defaultD.UpdateRoles(applicationContext, accountId, createSuperAdmin);
                defaultD.UpdateUsers(applicationContext, accountId, accountInfo, createSuperAdmin);

                List<PageConfiguration> pages = defaultD.GetPages();
                var pageConfigurationManager = new PageConfigurationManager(applicationContext);
                pageConfigurationManager.InsertItems(pages);
                //defaultD.UpdateTenantScheduler(applicationContext);
                //defaultD.GetHelpCards(applicationContext);

                // add common configuration data 
                defaultD.UpdateMenuNavigations(applicationContext);
                defaultD.UpdateTabView(applicationContext);
                defaultD.UpdateFieldConfigData(applicationContext);
                defaultD.GetUGITModule(applicationContext);
                defaultD.GetModuleStageType(applicationContext);
                defaultD.GetConfigurationVariable(applicationContext);
                defaultD.GetStates(applicationContext);
                defaultD.GetCRMRelationshipTypes(applicationContext);
                defaultD.GetBudgetCategories(applicationContext);
                defaultD.GetLocation(applicationContext);

                List<Company> companyList = defaultD.GetCompany(applicationContext);
                List<Department> departmentList = defaultD.GetDepartment(applicationContext, companyList);

                defaultD.GetFunctionalAreas(applicationContext, departmentList);
                defaultD.GetMailTokenColumnName(applicationContext);
                defaultD.GetModuleColumns(applicationContext);
                defaultD.GetMessageBoard(applicationContext);

                List<ClientAdminCategory> ClientAdminCategoryList = defaultD.GetClientAdminCategory(applicationContext);
                defaultD.GetClientAdminConfigurationLists(applicationContext, ClientAdminCategoryList);

                defaultD.GetGenericTicketStatus(applicationContext);
                List<ModuleMonitor> ModuleMonitorList = defaultD.GetModuleMonitors(applicationContext);
                defaultD.GetModuleMonitorOptions(applicationContext, ModuleMonitorList);
                defaultD.GetProjectSimilarityConfig(applicationContext);
                defaultD.GetLeadCriteria(applicationContext);
                defaultD.GetProjectComplexity(applicationContext);
                //defaultD.UpdateReportConfigData(applicationContext);
                // defaultD.GetPhrases(applicationContext);
                defaultD.GetServiceCategory(applicationContext);
                defaultD.GetVendorType(applicationContext);
                defaultD.GetAssetVendor(applicationContext);
                defaultD.GetAssetModel(applicationContext);
               // defaultD.UpdateTenantScheduler(applicationContext);
                //defaultD.ExecuteStoredProcedures(applicationContext);
            }
            catch(Exception ex)
            {
                ULog.WriteException(ex);
            }
            
        }

        public static void LoadRemainingDefaultData(ApplicationContext applicationContext)
        {
            var defaultD = new ConfigData(applicationContext);
            defaultD.GetPhrases(applicationContext);
            defaultD.GetWidgets(applicationContext);
            defaultD.GetHelpCards(applicationContext);
            defaultD.ExecuteStoredProcedures(applicationContext);
            defaultD.UpdateTenantScheduler(applicationContext);
        }

        public static void NavigationUrl(ApplicationContext applicationContext)
        {
            WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(applicationContext);
            RequestTypeManager requestTypeManager = new RequestTypeManager(applicationContext);

            ModuleRequestType acrModuleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and ModuleNameLookup='wiki' and { DatabaseObjects.Columns.TicketRequestType }='Application Change Request'");

            //ACR
            if (acrModuleRequestType != null)
            {
                WikiArticles acrwikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.TicketRequestTypeLookup}={acrModuleRequestType.ID}");
                var acrQueryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={acrwikiArticle.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='acr' ";
                uGITDAL.ExecuteNonQuery(acrQueryString);
            }
            //TSR
            ModuleRequestType tsrModuleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}'and ModuleNameLookup='wiki'  and { DatabaseObjects.Columns.TicketRequestType }='Technical Service Request'");
            if (tsrModuleRequestType != null)
            {
                WikiArticles tsrwikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.TicketRequestTypeLookup}={tsrModuleRequestType.ID}");
                var tsrQueryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={tsrwikiArticle.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='tsr' ";
                uGITDAL.ExecuteNonQuery(tsrQueryString);
            }
            //DRQ

            //WikiArticles drqWikiContents = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.ModuleName}='drq'");
            ModuleRequestType drqModuleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and ModuleNameLookup='wiki'  and { DatabaseObjects.Columns.TicketRequestType }='Change Management'");
            if (drqModuleRequestType != null)
            {
                WikiArticles drqWikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.TicketRequestTypeLookup}={drqModuleRequestType.ID}");
                var drqQueryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={drqWikiArticle.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='drq' ";
                uGITDAL.ExecuteNonQuery(drqQueryString);
            }

            //BTS

            ModuleRequestType btsModuleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and ModuleNameLookup='wiki'  and { DatabaseObjects.Columns.TicketRequestType }='Bug Tracking'");
            if (btsModuleRequestType != null)
            {
                WikiArticles btsWikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.TicketRequestTypeLookup}={btsModuleRequestType.ID}");

                var btsQueryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={btsWikiArticle.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='bts' ";

                uGITDAL.ExecuteNonQuery(btsQueryString);
            }
            //INC

            ModuleRequestType incModuleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and ModuleNameLookup='wiki'  and { DatabaseObjects.Columns.TicketRequestType }='Outage Incidents'");
            if (incModuleRequestType != null)
            {
                WikiArticles incWikiArticle = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.TicketRequestTypeLookup}={incModuleRequestType.ID}");

                var incQueryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={incWikiArticle.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='inc' ";

                uGITDAL.ExecuteNonQuery(incQueryString);
            }
            //WikiArticles serviceWikiContents = wikiArticlesManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{applicationContext.TenantID}' and {DatabaseObjects.Columns.ModuleName}='svc'");
            //var queryString = $"Update Config_Modules set NavigationUrl='/Layouts/uGovernIT/delegatecontrol.aspx?control=wikiDetails&isHelp=true&ticketId={serviceWikiContents.TicketId}' where TenantID='{applicationContext.TenantID}' and ModuleName='svc' ";
            //uGITDAL.ExecuteNonQuery(acrQueryString);

        }

        public static List<ModulePriorityMap> RequestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> severities, List<ModulePrioirty> priorities, List<ModulePriorityMap> requestPriorities)
        {
            var modulePriorityMaps = new List<ModulePriorityMap>();

            if (requestPriorities != null && requestPriorities.Count > 0)
            {
                foreach (ModulePriorityMap mr in requestPriorities)
                {
                    if (impacts.Count > mr.ImpactLookup)
                    {
                        ModuleImpact impact = impacts[Convert.ToInt32(mr.ImpactLookup)];
                        if (impact != null)
                            mr.ImpactLookup = impact.ID;
                    }

                    if (severities.Count > mr.SeverityLookup)
                    {
                        ModuleSeverity severity = severities[Convert.ToInt32(mr.SeverityLookup)];
                        if (severity != null)
                            mr.SeverityLookup = severity.ID;
                    }

                    if (priorities.Count > mr.PriorityLookup)
                    {
                        ModulePrioirty priority = priorities[Convert.ToInt32(mr.PriorityLookup)];
                        if (priority != null)
                            mr.PriorityLookup = priority.ID;
                    }

                    if (mr.ImpactLookup > 0 && mr.SeverityLookup > 0 && mr.PriorityLookup > 0)
                    {
                        modulePriorityMaps.Add(mr);
                    }
                }
            }

            return modulePriorityMaps;
        }
    }
}
