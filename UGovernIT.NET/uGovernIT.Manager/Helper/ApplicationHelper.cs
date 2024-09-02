using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;

namespace uGovernIT.Manager
{
    public  class ApplicationHelper
    {
        ConfigurationVariableManager objConfigurationVariableHelper;
        UserProfileManager UserManager;
        ApplicationContext context = null;
        ModuleViewManager ObjModuleViewManager = null;
        RequestTypeManager objRequestTypeManager;
        public ApplicationHelper(ApplicationContext _context)
        {
            context = _context;
            UserManager = _context.UserManager;
            objConfigurationVariableHelper = new ConfigurationVariableManager(_context);
            ObjModuleViewManager = new ModuleViewManager(_context);
            objRequestTypeManager = new RequestTypeManager(_context);

        }
        public void SyncToRequestType(ApplicationContext context, DataRow item)
        {

            // if (properties.ListId == uGITCache.GetListID(DatabaseObjects.Lists.Applications, properties.Web))
            // {
            // SPListItem item = properties.ListItem;
            if (UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.SyncToRequestType]) == false)
                return; // Don't need to sync this application

            string appTitle = Convert.ToString(item[DatabaseObjects.Columns.Title]);

            //  bool isAllowUnsafe = properties.Web.AllowUnsafeUpdates;
            // SPWeb thisWeb = properties.Web;
            //  thisWeb.AllowUnsafeUpdates = true;

            DataTable dtModules = ObjModuleViewManager.LoadAllModules();
            foreach (DataRow dr in dtModules.Rows)
            {
                if (UGITUtility.StringToBoolean(dr[DatabaseObjects.Columns.SyncAppsToRequestType]) == false)
                    continue; // Don't need to sync to this uGovernIT module

                string moduleName = Convert.ToString(dr[DatabaseObjects.Columns.ModuleName]);
                string moduleId = Convert.ToString(dr[DatabaseObjects.Columns.Id]);

                ULog.WriteLog("Syncing application [" + appTitle + "] to module " + moduleName);
                if (UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.SyncAtModuleLevel]) == false)
                    SyncApplToRequestType(item, moduleName, moduleId); // Sync at Application level
                else
                    SyncApplModuleToRequestType(item, moduleName, moduleId); // Sync at App Module level

                //uGITCache.ModuleConfigCache.ReloadModule(properties.Web, moduleName);
                //reqTypeListUpdated = true;
            }
            //thisWeb.AllowUnsafeUpdates = isAllowUnsafe;
            // }

            //if (reqTypeListUpdated)
            // uGITCache.RefreshList(DatabaseObjects.Lists.RequestType, properties.Web);
            // }

            //base.ItemUpdated(properties);
        }
        private void SyncApplToRequestType(DataRow appItem, string moduleName, string moduleId)
        {
            ModuleRequestType reqTypeItem = new ModuleRequestType();
            // Try to get Request Type item using APP Ticket Id
            ModuleRequestType requestTypeList = objRequestTypeManager.Get(x=> x.ModuleNameLookup.Equals(moduleName) && x.AppReferenceInfo.Equals(Convert.ToString(appItem[DatabaseObjects.Columns.TicketId])));
            if (requestTypeList!=null)
                reqTypeItem = requestTypeList;
            else
            {
                ModuleRequestType itemsByTitle = objRequestTypeManager.Get(x => x.ModuleNameLookup.Equals(moduleName) && x.Title.Equals(Convert.ToString(appItem[DatabaseObjects.Columns.Title])));
                if (itemsByTitle!=null)
                    reqTypeItem = itemsByTitle;
                else
                    reqTypeItem = new ModuleRequestType(); // Still not found, so create new item
            }

            if (reqTypeItem != null)
                SaveApplicationToRequestType(appItem, reqTypeItem, moduleName, moduleId);
        }
        private void SaveApplicationToRequestType(DataRow appItem, ModuleRequestType reqTypeItem, string moduleName, string ModuleId)
        {
            reqTypeItem.AppReferenceInfo = Convert.ToString(appItem[DatabaseObjects.Columns.TicketId]);

            if (moduleName == "TSR")
            {
                string TSRAppCategory = objConfigurationVariableHelper.GetValue(ConfigConstants.TSRAppCategory);
                if (string.IsNullOrWhiteSpace(TSRAppCategory))
                    TSRAppCategory = "Application Support";

                reqTypeItem.Category = TSRAppCategory;
                reqTypeItem.SubCategory = Convert.ToString(appItem[DatabaseObjects.Columns.CategoryNameChoice]);
            }
            else // ACR or INC
            {
                reqTypeItem.Category = Convert.ToString(appItem[DatabaseObjects.Columns.CategoryNameChoice]);
                reqTypeItem.SubCategory = Convert.ToString(appItem[DatabaseObjects.Columns.SubCategory]);
            }

            reqTypeItem.RequestType = Convert.ToString(appItem[DatabaseObjects.Columns.Title]); // App Name -> Request Type
            reqTypeItem.Title = string.Format("{0} > {1} > {2}",
                                                                  reqTypeItem.Category,
                                                                  reqTypeItem.SubCategory,
                                                                  reqTypeItem.RequestType);

            if (reqTypeItem.ID == 0)
            {
                reqTypeItem.WorkflowType = "Full";
            }

            //List<string> ownerCollection = UGITUtility.ConvertStringToList(Convert.ToString(appItem[DatabaseObjects.Columns.TicketOwner]),Constants.Separator6); //new SPFieldUserValueCollection(thisWeb, Convert.ToString(appItem[DatabaseObjects.Columns.TicketOwner]));
            reqTypeItem.Owner = Convert.ToString(appItem[DatabaseObjects.Columns.TicketOwner]);

            reqTypeItem.FunctionalAreaLookup = Convert.ToInt64(appItem[DatabaseObjects.Columns.FunctionalAreaLookup]);

            //SPFieldUserValueCollection prpGroup = new SPFieldUserValueCollection(thisWeb, Convert.ToString(appItem[DatabaseObjects.Columns.SupportedBy]));
            reqTypeItem.PRPGroup = Convert.ToString(appItem[DatabaseObjects.Columns.SupportedBy]);

            // Copy description if not empty
            //if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.UGITDescription])))
            //    reqTypeItem[DatabaseObjects.Columns.RequestTypeDescription] = appItem[DatabaseObjects.Columns.UGITDescription];            

            reqTypeItem.ModuleNameLookup = moduleName;
            //SPFieldLookupValue appLookUp = new SPFieldLookupValue(Convert.ToString(appItem[DatabaseObjects.Columns.Id]));
            reqTypeItem.APPTitleLookup = Convert.ToInt32(appItem[DatabaseObjects.Columns.ID]);
            reqTypeItem.ApplicationModulesLookup = null;
            reqTypeItem.IssueTypeOptions = Convert.ToString(appItem[DatabaseObjects.Columns.IssueTypeOptions]);
            reqTypeItem.Deleted = false;

            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, appItem.Table))
                reqTypeItem.SLADisabled = UGITUtility.StringToBoolean(appItem[DatabaseObjects.Columns.SLADisabled]);
            if (reqTypeItem.ID > 0)
                objRequestTypeManager.Update(reqTypeItem);
            else
                objRequestTypeManager.Insert(reqTypeItem);
            // reqTypeItem.Update();
        }
        private void SyncApplModuleToRequestType(DataRow appItem, string moduleName, string ModuleId)
        {
            bool enableAppModuleRoles = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.EnableAppModuleRoles);
            string TSRAppCategory = objConfigurationVariableHelper.GetValue(ConfigConstants.TSRAppCategory);
            if (string.IsNullOrWhiteSpace(TSRAppCategory))
                TSRAppCategory = "Application Support";
            DataTable requestTypeList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            int AppId = UGITUtility.StringToInt(appItem[DatabaseObjects.Columns.ID]);
            DataTable applModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationModules, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
            DataRow[] spColl = null;
            if (applModules != null && applModules.Rows.Count > 0)
                spColl = applModules.Select().AsEnumerable().Where(x =>Convert.ToInt32(x.Field<object>(DatabaseObjects.Columns.APPTitleLookup)) == AppId).ToArray();
            if (spColl != null && spColl.Count() > 0)
            {
                // SPQuery queryTitle = new SPQuery();
                //queryTitle.Query = "<Where><And><Eq><FieldRef Name=\"ModuleNameLookup\" /><Value Type=\"Text\">" + moduleName + "</Value></Eq><Eq><FieldRef Name=\"AppReferenceInfo\" /><Value Type=\"Text\">" + appItem[DatabaseObjects.Columns.TicketId] + "</Value></Eq></And></Where>";
                List<ModuleRequestType> itemsByTitle = objRequestTypeManager.Load(x => x.ModuleNameLookup.Equals(moduleName) && x.AppReferenceInfo.Equals(Convert.ToString(appItem[DatabaseObjects.Columns.TicketId]))); // requestTypeList.Select().AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == moduleName && x.Field<string>(DatabaseObjects.Columns.AppReferenceInfo) == Convert.ToString(appItem[DatabaseObjects.Columns.TicketId])).ToArray();
                foreach (DataRow item in spColl)
                {
                    ModuleRequestType spItem = null;
                    if (itemsByTitle.Count() > 0)
                    {
                        foreach (ModuleRequestType requestitem in itemsByTitle)
                        {
                            int appModuleLookUpId = Convert.ToInt32(requestitem.ApplicationModulesLookup);
                            if (appModuleLookUpId > 0 && appModuleLookUpId == UGITUtility.StringToInt(item[DatabaseObjects.Columns.ID]))
                            {
                                spItem = requestitem;
                                break;
                            }
                        }
                    }

                    if (spItem == null)
                        spItem = new ModuleRequestType(); //requestTypeList.NewRow();

                    spItem.AppReferenceInfo = Convert.ToString(appItem[DatabaseObjects.Columns.TicketId]);

                    if (moduleName == "TSR")
                    {
                        spItem.Category = TSRAppCategory;
                        spItem.SubCategory = Convert.ToString(appItem[DatabaseObjects.Columns.Title]);
                    }
                    else
                    {
                        spItem.Category = Convert.ToString(appItem[DatabaseObjects.Columns.CategoryNameChoice]);
                        spItem.SubCategory = Convert.ToString(appItem[DatabaseObjects.Columns.Title]);
                    }

                    spItem.RequestType = Convert.ToString(item[DatabaseObjects.Columns.Title]); // App Module Name -> Request Type
                    spItem.Title = string.Format("{0} > {1} > {2}",
                                                                          spItem.Category,
                                                                          spItem.SubCategory,
                                                                          spItem.RequestType);

                    if (UGITUtility.StringToInt(spItem.ID) == 0)
                    {
                        spItem.WorkflowType = "Full";
                    }

                    // Functional Area ALWAYS comes from Parent Application
                    spItem.FunctionalAreaLookup = Convert.ToInt64(appItem[DatabaseObjects.Columns.FunctionalAreaLookup]);

                    // Copy Owner from module if enabled and non-null value, else from parent application
                    string ownerCollection = null;
                    if (enableAppModuleRoles)
                        ownerCollection = Convert.ToString(item[DatabaseObjects.Columns.TicketOwner]);
                    if (ownerCollection == null)
                        ownerCollection = Convert.ToString(appItem[DatabaseObjects.Columns.TicketOwner]);
                    spItem.Owner = ownerCollection;

                    // Copy PRP Group from module if enabled and non-null value, else from parent application
                    string prpGroup = null;
                    if (enableAppModuleRoles)
                        prpGroup = Convert.ToString(item[DatabaseObjects.Columns.SupportedBy]);
                    if (prpGroup == null)
                        prpGroup = Convert.ToString(appItem[DatabaseObjects.Columns.SupportedBy]);
                    spItem.PRPGroup = prpGroup;

                    // Copy description from APP Module if not empty
                    //if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.UGITDescription])))
                    //    spItem[DatabaseObjects.Columns.RequestTypeDescription] = item[DatabaseObjects.Columns.UGITDescription];

                    spItem.ModuleNameLookup = moduleName;
                    //SPFieldLookupValue appModuleLookUp = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.Id]));
                    spItem.ApplicationModulesLookup = uHelper.getModuleNameByTicketId(Convert.ToString(appItem[DatabaseObjects.Columns.ID]));// Convert.ToString(item[DatabaseObjects.Columns.ModuleName]);
                    spItem.APPTitleLookup = null;

                    string issueTypeOptions = Convert.ToString(appItem[DatabaseObjects.Columns.IssueTypeOptions]);
                    spItem.IssueTypeOptions = string.Empty;
                    if (!string.IsNullOrWhiteSpace(issueTypeOptions))
                    {
                        spItem.IssueTypeOptions = issueTypeOptions.Replace("\n", Constants.Separator);
                    }
                    spItem.Deleted = false;

                    if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.SLADisabled, appItem.Table))
                        spItem.SLADisabled = UGITUtility.StringToBoolean(appItem[DatabaseObjects.Columns.SLADisabled]);
                    if (spItem.ID > 0)
                        objRequestTypeManager.Update(spItem);
                    else
                        objRequestTypeManager.Insert(spItem);
                    //spItem.Update();
                }
            }

        }
    }
}
