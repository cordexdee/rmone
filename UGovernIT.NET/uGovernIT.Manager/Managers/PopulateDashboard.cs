using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using uGovernIT.Utility;
using System.Data;
using DevExpress.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;
using uGovernIT.Utility.Entities;
using System.Collections;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Text;
using uGovernIT.DAL;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using System.Reflection;

namespace uGovernIT.Manager
{
    public class PopulateDashboard
    {
        ApplicationContext _context;
        private Hashtable DashboardSummaryTicketIDs;
        ModuleWorkflowHistoryManager historyManager;
        public PopulateDashboard(ApplicationContext context)
        {
            DashboardSummaryTicketIDs = new Hashtable();
            _context = context;
            historyManager = new ModuleWorkflowHistoryManager(context);

        }
        public void PopulateDashboardItem(UGITModule module, DataRow readItem)
        {
            if (module == null || readItem == null || !UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketId))
            {
                ULog.WriteLog("ERROR: module or list item not found in PopulateDashboardItem");
                return;
            }

            //Get module name and id
            string moduleId = Convert.ToString(module.ID);
            string moduleName = module.ModuleName;
            DataTable statusMappingTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketStatusMapping, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            DataTable genericTicketStatusTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.GenericTicketStatus, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            DataRow item = null;
            DashboardSummary objDashboardSummary = new DashboardSummary();
            DashboardSummaryManager objDashboardSummaryManager = new DashboardSummaryManager(_context);
            List<LifeCycleStage> objLifeCycleStages = new List<LifeCycleStage>();
            // Edit or add item in DashboardSummary list
            string ticketID = Convert.ToString(readItem[DatabaseObjects.Columns.TicketId]);
            //ULog.WriteLog("PopulateDashboardItem for " + ticketID);

            if (DashboardSummaryTicketIDs != null)
            {
                if (DashboardSummaryTicketIDs.Contains(ticketID))
                {
                    int DashboardSummaryID = int.Parse(Convert.ToString(DashboardSummaryTicketIDs[ticketID]));
                }
            }
            else
            {
                DataTable dtdashboard = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                if (dtdashboard != null && dtdashboard.Rows.Count > 0)
                {
                    DataRow[] dashboardCollection = null;
                    dashboardCollection = dtdashboard.Select(string.Format("{1}='{0}'", Convert.ToString(readItem[DatabaseObjects.Columns.TicketId]), DatabaseObjects.Columns.TicketId));
                    if (dashboardCollection.Count() > 0)
                    {
                        int id = UGITUtility.StringToInt(dashboardCollection[0][DatabaseObjects.Columns.ID]);
                        objDashboardSummary = objDashboardSummaryManager.LoadByID(id);
                    }
                }

            }

            //if item doesn't exist in dashboard summary list then create that item
            if (objDashboardSummary == null)
                objDashboardSummary = new DashboardSummary(); //dashboardList.NewRow();

            // Fill ticket Id
            objDashboardSummary.TicketId = ticketID;

            // Fill title of ticket
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.Title))
                objDashboardSummary.Title = Convert.ToString(readItem[DatabaseObjects.Columns.Title]);

            // Fill Creation date
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketCreationDate))
                objDashboardSummary.CreationDate = Convert.ToDateTime(readItem[DatabaseObjects.Columns.TicketCreationDate]);
            else
                objDashboardSummary.CreationDate = Convert.ToDateTime(readItem[DatabaseObjects.Columns.Created]);

            // Fill request type
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketRequestTypeLookup))
                objDashboardSummary.RequestTypeLookup = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);

            // Fill modulesteplookup
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.ModuleStepLookup))
                objDashboardSummary.ModuleStepLookup = Convert.ToString(readItem[DatabaseObjects.Columns.ModuleStepLookup]);

            // Fill StageStep
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.StageStep))
                objDashboardSummary.StageStep = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.StageStep]);

            // Fill ticket status
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketStatus))
                objDashboardSummary.Status = Convert.ToString(readItem[DatabaseObjects.Columns.TicketStatus]);

            // Fill Initiator
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketInitiator))
                objDashboardSummary.Initiator = Convert.ToString(readItem[DatabaseObjects.Columns.TicketInitiator]);

            // Fill Requestor
            objDashboardSummary.Requestor = null;
            objDashboardSummary.RequestorDepartment = null;
            objDashboardSummary.RequestorCompany = null;
            objDashboardSummary.RequestorDivision = null;
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketRequestor))
            {
                objDashboardSummary.Requestor = Convert.ToString(readItem[DatabaseObjects.Columns.TicketRequestor]);
                UserProfile userProfile = _context.UserManager.GetUserById(Convert.ToString(readItem[DatabaseObjects.Columns.TicketRequestor]));
                if (userProfile != null && userProfile.DepartmentId > 0)
                {
                    item[DatabaseObjects.Columns.RequestorDepartment] = userProfile.Department;
                    DepartmentManager objDepartmentManager = new DepartmentManager(_context);

                    Department dpt = objDepartmentManager.LoadByID(userProfile.DepartmentId);

                    if (dpt != null && dpt.CompanyIdLookup.HasValue)
                        item[DatabaseObjects.Columns.RequestorCompany] = dpt.CompanyIdLookup.Value;
                    if (dpt != null && dpt.DivisionIdLookup.HasValue)
                        item[DatabaseObjects.Columns.RequestorDivision] = dpt.DivisionLookup.Value;
                }
            }
            else if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.DepartmentLookup))
            {
                objDashboardSummary.RequestorDepartment = UGITUtility.SplitString(readItem[DatabaseObjects.Columns.DepartmentLookup], Constants.Separator, 1);
                if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.CompanyTitleLookup))
                    objDashboardSummary.RequestorCompany = UGITUtility.SplitString(readItem[DatabaseObjects.Columns.CompanyTitleLookup], Constants.Separator, 1);
                if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.DivisionLookup))
                    objDashboardSummary.RequestorDivision = UGITUtility.SplitString(readItem[DatabaseObjects.Columns.DivisionLookup], Constants.Separator, 1);
            }

            // Fill ticket owner
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketOwner))
                objDashboardSummary.Owner = Convert.ToString(readItem[DatabaseObjects.Columns.TicketOwner]);

            // Fill PRP Group
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.PRPGroup))
                objDashboardSummary.PRPGroup = Convert.ToString(readItem[DatabaseObjects.Columns.PRPGroup]);

            // Fill ticket PRP
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketPRP))
                objDashboardSummary.PRP = Convert.ToString(readItem[DatabaseObjects.Columns.TicketPRP]);

            // Fill ticket ORP
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketORP))
                objDashboardSummary.ORP = Convert.ToString(readItem[DatabaseObjects.Columns.TicketORP]);

            // Fill actual hours
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketActualHours))
                objDashboardSummary.ActualHours = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketActualHours]);
            else
                objDashboardSummary.ActualHours = 0;

            // Fill ticket initiator-resolved
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketInitiatorResolved))
                objDashboardSummary.InitiatorResolved = Convert.ToString(readItem[DatabaseObjects.Columns.TicketInitiatorResolved]);
            else
                objDashboardSummary.InitiatorResolved = "No";

            // Fill OnHold Ticket
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketOnHold))
                objDashboardSummary.OnHold = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketOnHold]);
            else
                objDashboardSummary.OnHold = 0;
            bool ticketOnHold = objDashboardSummary.OnHold == 1 ? true : false;
            // If ticket on hold, Fill Ticket OnHold Start Date
            if (ticketOnHold && UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketOnHoldStartDate))
                objDashboardSummary.OnHoldStartDate = UGITUtility.StringToDateTime(readItem[DatabaseObjects.Columns.TicketOnHoldStartDate]);
            else
                objDashboardSummary.OnHoldStartDate = null;

            // Fill Ticket Total Hold Duration need discussion
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketOnHoldTillDate))
                objDashboardSummary.OnHoldTillDate = Convert.ToDateTime(readItem[DatabaseObjects.Columns.TicketOnHoldTillDate]);
            else
                objDashboardSummary.OnHoldTillDate = null;

            // Fill Ticket Total Hold Duration need discussion
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketTotalHoldDuration))
                objDashboardSummary.TotalHoldDuration = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketTotalHoldDuration]);
            else
                objDashboardSummary.TotalHoldDuration = 0;

            // Fill Ticket Age for closed ticket
            if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketAge, readItem.Table))
                objDashboardSummary.Age = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketAge]);

            // Fill ticket Source
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.RequestSource))
                objDashboardSummary.RequestSourceChoice = Convert.ToString(readItem[DatabaseObjects.Columns.RequestSource]);

            // Fill ticket category
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketRequestTypeCategory))
                objDashboardSummary.Category = Convert.ToString(readItem[DatabaseObjects.Columns.TicketRequestTypeCategory]);

            // Fill workflow type
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketRequestTypeWorkflow))
                objDashboardSummary.WorkflowType = Convert.ToString(readItem[DatabaseObjects.Columns.TicketRequestTypeWorkflow]);

            // Fill action users (waiting on)
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketStageActionUsers))
                objDashboardSummary.StageActionUsers = Convert.ToString(UGITUtility.GetSPItemValue(readItem, DatabaseObjects.Columns.TicketStageActionUsers));

            // Fill functional area
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.FunctionalAreaLookup))
                objDashboardSummary.FunctionalAreaLookup = UGITUtility.StringToInt(UGITUtility.GetSPItemValue(readItem, DatabaseObjects.Columns.FunctionalAreaLookup));

            // Fill ticket Sub category
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketRequestTypeSubCategory))
                objDashboardSummary.SubCategory = Convert.ToString(readItem[DatabaseObjects.Columns.TicketRequestTypeSubCategory]);

            Guid newGuid = new Guid();

            // Fill ticket ResolvedBy, ClosedBy, AssignedBy, ApprovedBy
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketResolvedBy) && Convert.ToString(readItem[DatabaseObjects.Columns.TicketResolvedBy]) != Convert.ToString(newGuid))
                objDashboardSummary.ResolvedBy=Convert.ToString(readItem[DatabaseObjects.Columns.TicketResolvedBy]);

            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketClosedBy) && Convert.ToString(readItem[DatabaseObjects.Columns.TicketClosedBy]) != Convert.ToString(newGuid))
                objDashboardSummary.ClosedBy=Convert.ToString(readItem[DatabaseObjects.Columns.TicketClosedBy]);

            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketAssignedBy) && Convert.ToString(readItem[DatabaseObjects.Columns.TicketClosedBy]) != Convert.ToString(newGuid))
                objDashboardSummary.AssignedBy = Convert.ToString(readItem[DatabaseObjects.Columns.TicketAssignedBy]);

            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.ApprovedBy))
                objDashboardSummary.ApprovedByUser = Convert.ToString(readItem[DatabaseObjects.Columns.ApprovedBy]);
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.ApprovalDate))
                objDashboardSummary.ApprovalDate = Convert.ToDateTime(readItem[DatabaseObjects.Columns.ApprovalDate]);
            objDashboardSummary.SLADisabled = false;
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.SLADisabled))
                objDashboardSummary.SLADisabled =UGITUtility.StringToBoolean(readItem[DatabaseObjects.Columns.SLADisabled]);

            // Fill location
            objDashboardSummary.LocationLookup = null;
            objDashboardSummary.State = null;
            objDashboardSummary.Country = null;
            objDashboardSummary.Region = null;
            DataTable locationTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Location, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.LocationLookup))
            {
                objDashboardSummary.LocationLookup = UGITUtility.StringToInt((readItem[DatabaseObjects.Columns.LocationLookup]));
                long lookup = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.LocationLookup]); //uHelper.GetSPItemValue(readItem, DatabaseObjects.Columns.LocationLookup)));
                if (lookup > 0 && locationTable != null)
                {
                    DataRow location = locationTable.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ID) == lookup);
                    if (location != null)
                    {
                        objDashboardSummary.Country = Convert.ToString(UGITUtility.GetSPItemValue(location, DatabaseObjects.Columns.UGITCountry));
                        objDashboardSummary.State = Convert.ToString(UGITUtility.GetSPItemValue(location, DatabaseObjects.Columns.UGITState));
                        objDashboardSummary.Region = Convert.ToString(UGITUtility.GetSPItemValue(location, DatabaseObjects.Columns.UGITRegion));
                    }
                }
            }

            // Fill Resolution Type
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketResolutionType))
                objDashboardSummary.ResolutionTypeChoice = Convert.ToString(readItem[DatabaseObjects.Columns.TicketResolutionType]);

            // Fill module id
            objDashboardSummary.ModuleNameLookup = moduleName;

            //Fill generic status
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.ModuleStepLookup) && module.List_LifeCycles[0].Stages != null && statusMappingTable != null && genericTicketStatusTable != null)
            {
                string itemStage = string.Join(",", UGITUtility.SplitString(Convert.ToString(readItem[DatabaseObjects.Columns.ModuleStepLookup]), Constants.Separator));
                if (!string.IsNullOrWhiteSpace(itemStage))
                    itemStage = itemStage.Replace("'", "''");
                StoreBase<LifeCycleStage> store1 = new StoreBase<LifeCycleStage>(_context);

                objLifeCycleStages = store1.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));
                DataRow[] moduleStages = UGITUtility.ToDataTable<LifeCycleStage>(objLifeCycleStages).Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, itemStage));//moduleStagesTable.Select(string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.StageTitle, itemStage));
                if (moduleStages.Length > 0)
                {
                    string Id = Convert.ToString(moduleStages[0][DatabaseObjects.Columns.ID]).Replace("'", "''");
                    DataRow[] stageMappings = stageMappings = statusMappingTable.Select(string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.StageTitleLookup, Id));
                    if (stageMappings.Length > 0)
                    {
                        string genericStatusId = Convert.ToString(stageMappings[0][DatabaseObjects.Columns.GenericStatusLookup]);
                        DataRow[] genericTicketStatus = genericTicketStatusTable.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ID, genericStatusId));
                        if (genericTicketStatus.Length > 0)
                            objDashboardSummary.GenericStatusLookup = UGITUtility.StringToInt(genericTicketStatus[0][DatabaseObjects.Columns.Id]);
                    }
                }
            }

            // Fill priority 
            string priority = string.Empty;
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.TicketPriorityLookup))
            {
                objDashboardSummary.PriorityLookup = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.TicketPriorityLookup]);
                //priority = UGITUtility.SplitString(readItem[DatabaseObjects.Columns.TicketPriorityLookup], Constants.Separator,1);
                priority = string.Join(",", UGITUtility.SplitString(Convert.ToString(readItem[DatabaseObjects.Columns.TicketPriorityLookup]), Constants.Separator));
            }

            List<ModuleWorkflowHistory> historyList = historyManager.Load(x => x.TicketId == Convert.ToString(readItem[DatabaseObjects.Columns.TicketId])).OrderBy(x => x.StageStep).OrderBy(x => x.StageStartDate).ToList();

            // Fill SLA Met
            if (!UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.SLADisabled) || !UGITUtility.StringToBoolean(readItem[DatabaseObjects.Columns.SLADisabled]))
                SetSLAMet(objDashboardSummary, readItem, historyList, Convert.ToString(readItem[DatabaseObjects.Columns.TicketId]), priority, moduleName);

            //SetSLAMet(objDashboardSummary, readItem, historyCollection, Convert.ToString(readItem[DatabaseObjects.Columns.TicketId]), priority, moduleName);

            // Fill Stage Dates
            if (module.List_LifeCycles[0].Stages != null)
            {
                // Reset all dates to null first

                foreach (LifeCycleStage dr in objLifeCycleStages)
                {
                    string stageType = Convert.ToString(dr.StageTypeChoice);
                    PropertyInfo prop = objDashboardSummary.GetType().GetProperty(stageType + "Date");
                    if (prop != null)
                    {
                        prop.SetValue(objDashboardSummary, DateTime.MinValue);
                    }
                }

                if (historyList != null)
                {
                    int currentStageStep = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.StageStep]);
                    foreach (LifeCycleStage dr in objLifeCycleStages)
                    {
                        string stageType = Convert.ToString(dr.StageTypeChoice);
                        if (objDashboardSummary.GetType().GetProperty(stageType + "Date") == null)
                            continue;

                        if (stageType == StageType.Initiated.ToString() && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCreationDate, readItem.Table))
                        {
                            objDashboardSummary.GetType().GetProperty(stageType + "Date").SetValue(objDashboardSummary,UGITUtility.StringToDateTime(readItem[DatabaseObjects.Columns.TicketCreationDate]));
                        }
                        else if (stageType == StageType.Closed.ToString() && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, readItem.Table) && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCloseDate, readItem.Table)
                            && UGITUtility.StringToBoolean(readItem[DatabaseObjects.Columns.TicketClosed]))
                        {
                            objDashboardSummary.GetType().GetProperty(stageType + "Date").SetValue(objDashboardSummary,UGITUtility.StringToDateTime(readItem[DatabaseObjects.Columns.TicketCloseDate]));
                        }
                        else
                        {
                            string stageTypeTitle = Convert.ToString(dr.StageTitle);
                            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketCreationDate, readItem.Table))
                                //item[stageType + "Date"] = UGITUtility.StringToDateTime(readItem[DatabaseObjects.Columns.TicketCreationDate]);
                                objDashboardSummary.GetType().GetProperty(stageType + "Date").SetValue(objDashboardSummary, UGITUtility.StringToDateTime(readItem[DatabaseObjects.Columns.TicketCreationDate]));

                            LifeCycleStage stageTypeStage = module.List_LifeCycles[0].Stages.Where(x => x.StageTitle.Equals(stageTypeTitle) && x.ModuleNameLookup.Equals(module.ModuleName)).FirstOrDefault();

                            if (stageTypeStage != null)
                            {
                                int endStageStep = UGITUtility.StringToInt(stageTypeStage.StageStep);
                                ModuleWorkflowHistory hRow = historyList.LastOrDefault(x => x.StageStep == endStageStep && x.StageStep <= currentStageStep); 
                                if (hRow == null)
                                {
                                    ModuleWorkflowHistory hItem = historyList.FirstOrDefault(x => x.StageStep > endStageStep && x.StageStep <= currentStageStep);
                                    if (hItem != null)
                                        hRow = historyList.OrderBy(x => x.StageStep).LastOrDefault();
                                }
                                if (hRow != null)
                                    objDashboardSummary.GetType().GetProperty(stageType + "Date").SetValue(objDashboardSummary, hRow.StageStartDate);
                            }
                        }
                    }
                }
            }

            // Fill Ticket Closed flag
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketClosed, readItem.Table))
                objDashboardSummary.Closed = UGITUtility.StringToBoolean(readItem[DatabaseObjects.Columns.TicketClosed]);
            else
                objDashboardSummary.Closed = false;

            // Fill Ticket Service and Service Category.
            if (UGITUtility.IsSPItemExist(readItem, DatabaseObjects.Columns.ServiceTitleLookup))
            {
                ServicesManager objServicesManager = new ServicesManager(_context);
                Services objServices = objServicesManager.LoadByID(UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.ServiceTitleLookup]));
                // SPFieldLookupValue serviceVal = new SPFieldLookupValue(Convert.ToString(readItem[DatabaseObjects.Columns.ServiceTitleLookup]));
                objDashboardSummary.ServiceName = objServices.Title;
                //SPListItem serviceItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.Services, serviceVal.LookupId, spWeb);
                if (objServices != null)
                {
                    //SPFieldLookupValue serviceCategoryVal = new SPFieldLookupValue(Convert.ToString(serviceItem[DatabaseObjects.Columns.ServiceCategoryNameLookup]));
                    objDashboardSummary.ServiceCategoryName = objServices.ServiceCategoryType; //serviceCategoryVal.LookupValue;
                }
            }

            /// Fill Ticket Reopen count 
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ReopenCount, readItem.Table))
            {
                objDashboardSummary.ReopenCount = UGITUtility.StringToInt(readItem[DatabaseObjects.Columns.ReopenCount] != DBNull.Value ? readItem[DatabaseObjects.Columns.ReopenCount] : "0");
            }

            try
            {
                if (objDashboardSummary.ID > 0)
                {
                    objDashboardSummaryManager.Update(objDashboardSummary);
                }
                else
                {
                    if (objDashboardSummary.GenericStatusLookup == 0)
                        objDashboardSummary.GenericStatusLookup = null;
                    objDashboardSummaryManager.Insert(objDashboardSummary);
                }
            }           
            catch (Exception ex)
            {
                // Ignore known exception due to event receiver getting fired multiple times
                ULog.WriteException(ex);
            }

            return;
        }
        public void SetSLAMet(DashboardSummary dashboardSummaryItem, DataRow ticketItem, List<ModuleWorkflowHistory> historyCollection, string ticketId, string priority, string moduleName)
        {
            WorkflowSLASummaryManager objWorkflowSLASummaryManager = new WorkflowSLASummaryManager(_context);
            WorkflowSLASummary updateTktWrkFlowSummaryItem = null;
            List<LifeCycleStage> objLifeCycleStages = new List<LifeCycleStage>();
            StoreBase<LifeCycleStage> store1 = new StoreBase<LifeCycleStage>(_context);
            objLifeCycleStages = store1.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName));
            DataRow[] moduleStages = UGITUtility.ToDataTable<LifeCycleStage>(objLifeCycleStages).Select();
            DataTable moduleStageData = new DataTable();

            // no stages found
            if (moduleStages.Length <= 0)
                return;

            DataTable slaSummaryList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketWorkflowSLASummary, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            int currentStageStep = UGITUtility.StringToInt(ticketItem[DatabaseObjects.Columns.StageStep]);


            string status = string.Empty;
            if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketStatus, ticketItem.Table) && !string.IsNullOrWhiteSpace(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketStatus])))
                status = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketStatus]).ToLower();

            if (UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketClosed])) //&& UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketRejected, ticketItem.Table) && UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketRejected]))
            {
                //sla summary is not required for rejected and cancelled ticket.
                //so find all the sla summary againt ticket and delete it also set sla field N/A in Dashboard summary
                DataRow[] collection = null;
                collection = slaSummaryList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketItem[DatabaseObjects.Columns.TicketId]));
                if (collection != null && collection.Count() > 0)
                {      
                    DataRow item = null;
                    //GetTableDataManager.delete<int>(DatabaseObjects.Tables.TicketWorkflowSLASummary, DatabaseObjects.Columns.ID, Convert.ToString(item[DatabaseObjects.Columns.ID]));

                    int i = 0;
                    while (i < collection.Count())
                    {
                        item = collection[i];
                        updateTktWrkFlowSummaryItem = objWorkflowSLASummaryManager.LoadByID(Convert.ToInt64(item[DatabaseObjects.Columns.ID]));
                        updateTktWrkFlowSummaryItem.Closed = true;
                        objWorkflowSLASummaryManager.Update(updateTktWrkFlowSummaryItem);
                        i++;
                    }
                }

                dashboardSummaryItem.AssignmentSLAMet = "N/A";
                dashboardSummaryItem.RequestorContactSLAMet = "N/A";
                dashboardSummaryItem.ResolutionSLAMet = "N/A";
                dashboardSummaryItem.CloseSLAMet = "N/A";
                dashboardSummaryItem.OtherSLAMet = "N/A";
                return;
            }

            moduleStageData = moduleStages.CopyToDataTable();
            DataTable slaRuleTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SLARule, $" {DatabaseObjects.Columns.Deleted} = 'False' and {DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");
            //Return when SLA Rule table is null
            DataTable moduleSLAData = slaRuleTable.Clone();
            if (!slaRuleTable.Columns.Contains(DatabaseObjects.Columns.Use24x7Calendar))
                slaRuleTable.Columns.Add(DatabaseObjects.Columns.Use24x7Calendar, typeof(bool));

            // Skip if SLA rule not for this module and priority
            if (slaRuleTable.Rows.Count > 0)
            {
                DataRow[] selectedRules = slaRuleTable.Select(string.Format("{0} = '{1}' And {2} = {3}", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.TicketPriorityLookup, UGITUtility.StringToInt(priority)));
                if (selectedRules.Length > 0)
                    moduleSLAData = selectedRules.CopyToDataTable();
            }

            #region set use 24*7 for priority based SLA as well
            if (moduleSLAData.Rows.Count > 0 && uHelper.IsTicket24x7Enabled(_context, ticketItem))
            {
                int workingHrsInDay = uHelper.GetWorkingHoursInADay(_context, true);
                foreach (DataRow row in moduleSLAData.Rows)
                {
                    row[DatabaseObjects.Columns.Use24x7Calendar] = true;

                    //SLAHours is store based on working hours in days, eg 8 hours, so we need to convert hours into days and convert it into minutes based on 24 hours
                    double slaHours = UGITUtility.StringToDouble(row[DatabaseObjects.Columns.SLAHours]);
                    if (slaHours % workingHrsInDay == 0)
                        row[DatabaseObjects.Columns.SLAHours] = slaHours / workingHrsInDay * 24;
                }
            }
            #endregion

            bool ticketOnHold = UGITUtility.StringToBoolean(ticketItem[DatabaseObjects.Columns.TicketOnHold]);
            DateTime holdStartDate = DateTime.MinValue;
            DateTime holdTillDate = DateTime.MinValue;
            if (ticketOnHold && UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketOnHoldStartDate))
                holdStartDate = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketOnHoldStartDate]);
            if (ticketOnHold && UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketOnHoldTillDate))
                holdTillDate = UGITUtility.StringToDateTime(ticketItem[DatabaseObjects.Columns.TicketOnHoldTillDate]);

            moduleSLAData = OverrideWithRequestTypeSLA(moduleName, ticketItem, moduleSLAData, moduleStages);
            List<string> slaMetList = new List<string>();
            foreach (DataRow ruleItem in moduleSLAData.Rows)
            {
                // SLA Rule keeps start stage & end stage, ModuleWorkflow history keeps stage prior to end stage
                // So need to go back one from start & end stages
                string startStageName = string.Empty;
                int startStageStep = UGITUtility.StringToInt(ruleItem[DatabaseObjects.Columns.StartStageStep]);
                DataRow[] moduleStepStart = moduleStageData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleStep, startStageStep));
                if (moduleStepStart.Length > 0)
                {
                    startStageStep = UGITUtility.StringToInt(moduleStepStart[0][DatabaseObjects.Columns.ModuleStep]);
                    startStageName = Convert.ToString(moduleStepStart[0][DatabaseObjects.Columns.StageTitle]);
                }

                //int boundaryEndStageStep = 0;
                string endStageName = string.Empty;
                int endStageStep = UGITUtility.StringToInt(ruleItem[DatabaseObjects.Columns.EndStageStep]);
                DataRow[] moduleStepsEnd = moduleStageData.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ModuleStep, endStageStep));
                if (moduleStepsEnd.Length > 0)
                {
                    //Get the end stage from sla rule and move one step before to calculate duration spent. boundary condition [startstge + endstage)
                    endStageStep = UGITUtility.StringToInt(moduleStepsEnd[0][DatabaseObjects.Columns.ModuleStep]);
                    endStageName = Convert.ToString(moduleStepsEnd[0][DatabaseObjects.Columns.StageTitle]);
                    //boundaryEndStageStep = endStageStep;
                    //if (boundaryEndStageStep > startStageStep)
                    //    boundaryEndStageStep = boundaryEndStageStep - 1;
                }


                double duration = 0;
                double onholdDuration = 0;
                DateTime stageStartDate = DateTime.MinValue;
                DateTime stageEndDate = DateTime.MinValue;

                if (historyCollection != null && historyCollection.Count > 0)
                {

                    duration = historyCollection.Where(x=>x.StageStep >= startStageStep && x.StageStep < endStageStep).Sum(x => x.Duration);  
                    onholdDuration = historyCollection.Where(x => x.StageStep >= startStageStep && x.StageStep < endStageStep).Sum(x => x.OnHoldDuration); 
                    duration = duration - onholdDuration;
                    if (duration < 0)
                        duration = 0;

                    // To see if an SLA is met, use start date of the earliest entry and the end date of the latest entry!
                    ModuleWorkflowHistory startStageHRow = historyCollection.FirstOrDefault(x => x.StageStep == startStageStep);
                    //in skip stage we don't create entry in history so
                    //pick next stage if current start stage not found
                    if (startStageHRow == null)
                        startStageHRow = historyCollection.FirstOrDefault(x => x.StageStep > startStageStep);

                    ModuleWorkflowHistory endStageHRow = historyCollection.LastOrDefault(x => x.StageStep == endStageStep && x.StageStep <= currentStageStep);
                    if (endStageHRow == null)
                    {
                        var hItem = historyCollection.FirstOrDefault(x => x.StageStep > endStageStep && x.StageStep <= currentStageStep);
                        if (hItem != null)
                            endStageHRow = historyCollection.OrderBy(x => x.StageStep).LastOrDefault(); 
                    }
                    if (startStageHRow != null)
                        stageStartDate = startStageHRow.StageStartDate;
                    if (endStageHRow != null)
                        stageEndDate = endStageHRow.StageStartDate;
                }

                double slaHours = 0;
                slaHours = Convert.ToDouble(ruleItem[DatabaseObjects.Columns.SLAHours]);
                double slaMinutes = slaHours * 60;

                #region SLA Summary
                // DataRow updateTktWrkFlowSummaryItem = null
                //WorkflowSLASummary updateTktWrkFlowSummaryItem = null;
                StringBuilder queryExps = new StringBuilder();
                queryExps.AppendFormat(string.Format("{0}='{1}' And ", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
                queryExps.AppendFormat(string.Format("{0}='{1}' And ", DatabaseObjects.Columns.TicketId, dashboardSummaryItem.TicketId));
                if (UGITUtility.StringToInt(ruleItem[DatabaseObjects.Columns.ID]) > 0)
                    queryExps.AppendFormat(string.Format("{0}={1}", DatabaseObjects.Columns.RuleNameLookup, ruleItem[DatabaseObjects.Columns.ID]));
                else
                    queryExps.AppendFormat(string.Format("{0}='{1}'", DatabaseObjects.Columns.SLACategory, ruleItem[DatabaseObjects.Columns.SLACategory]));
                DataRow[] slaSummaryColl = slaSummaryList.Select(string.Format(queryExps.ToString()));
                if (slaSummaryColl.Count() > 0)
                {
                    int id = UGITUtility.StringToInt(slaSummaryColl[0][DatabaseObjects.Columns.ID]);

                    updateTktWrkFlowSummaryItem = objWorkflowSLASummaryManager.LoadByID(id);
                }
                else
                {
                    updateTktWrkFlowSummaryItem = new WorkflowSLASummary();
                    updateTktWrkFlowSummaryItem.ModuleNameLookup = Convert.ToString(moduleName);
                    updateTktWrkFlowSummaryItem.TicketId = Convert.ToString(dashboardSummaryItem.TicketId);
                }

                updateTktWrkFlowSummaryItem.Title = Convert.ToString(ticketItem[DatabaseObjects.Columns.Title]);
                
                if (UGITUtility.StringToInt(ruleItem[DatabaseObjects.Columns.ID]) > 0)
                    updateTktWrkFlowSummaryItem.RuleNameLookup = UGITUtility.StringToInt(ruleItem[DatabaseObjects.Columns.ID]);
                else
                    updateTktWrkFlowSummaryItem.RuleNameLookup = null;
                
                updateTktWrkFlowSummaryItem.StartStageName = startStageName;
                updateTktWrkFlowSummaryItem.StartStageStep = startStageStep;
                updateTktWrkFlowSummaryItem.EndStageName = endStageName;
                updateTktWrkFlowSummaryItem.EndStageStep = endStageStep;
                updateTktWrkFlowSummaryItem.SLARuleName = Convert.ToString(ruleItem[DatabaseObjects.Columns.Title]);
                updateTktWrkFlowSummaryItem.SLACategoryChoice = Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]);
                updateTktWrkFlowSummaryItem.Closed = UGITUtility.StringToBoolean((ticketItem[DatabaseObjects.Columns.TicketClosed]));
                updateTktWrkFlowSummaryItem.StageStartDate = null;
                updateTktWrkFlowSummaryItem.DueDate = null;
                
                if (stageStartDate != DateTime.MinValue)
                {
                    updateTktWrkFlowSummaryItem.StageStartDate = stageStartDate;
                    DateTime ugitDueDate = DateTime.MinValue;

                    // Use 24x7 Datetime if Use24x7Calendar is true
                    if (UGITUtility.StringToBoolean(ruleItem[DatabaseObjects.Columns.Use24x7Calendar]))
                        ugitDueDate = stageStartDate.AddMinutes(slaMinutes);
                    else
                        ugitDueDate = uHelper.GetWorkingEndDate(_context, stageStartDate, slaMinutes);

                    if (ugitDueDate != DateTime.MinValue)
                        updateTktWrkFlowSummaryItem.DueDate = ugitDueDate;
                }

                updateTktWrkFlowSummaryItem.TargetTime = UGITUtility.StringToInt(slaMinutes);

                // For tickets on hold, Update duration of hold ticket based on ticket hold start date if stage end date is NOT reached
                if (ticketOnHold && stageStartDate != DateTime.MinValue && stageEndDate == DateTime.MinValue && holdStartDate != DateTime.MinValue)
                {
                    if (UGITUtility.StringToBoolean(ruleItem[DatabaseObjects.Columns.Use24x7Calendar]))
                        duration = holdStartDate.Subtract(stageStartDate).TotalMinutes - onholdDuration;
                    else
                        duration = uHelper.GetWorkingMinutesBetweenDates(_context, stageStartDate, holdStartDate) - onholdDuration;
                }

                if (stageEndDate != DateTime.MinValue)
                {
                    updateTktWrkFlowSummaryItem.StageEndDate = stageEndDate;
                    updateTktWrkFlowSummaryItem.ActualTime = UGITUtility.StringToInt(duration);
                }
                else
                {
                    updateTktWrkFlowSummaryItem.StageEndDate = null;
                    updateTktWrkFlowSummaryItem.ActualTime = 0;
                }

                updateTktWrkFlowSummaryItem.OnHold = ticketOnHold ? 1 : 0;
                updateTktWrkFlowSummaryItem.TotalHoldDuration = onholdDuration;

                // Update Service lookup if created from service
                if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.ServiceTitleLookup))
                    updateTktWrkFlowSummaryItem.ServiceLookup = UGITUtility.StringToLong(ticketItem[DatabaseObjects.Columns.ServiceTitleLookup]);

                //Set Use24x7Calendar flag
                updateTktWrkFlowSummaryItem.Use24x7Calendar = UGITUtility.StringToBoolean(ruleItem[DatabaseObjects.Columns.Use24x7Calendar]);

                if (updateTktWrkFlowSummaryItem.ID > 0)
                    objWorkflowSLASummaryManager.Update(updateTktWrkFlowSummaryItem);
                else
                    objWorkflowSLASummaryManager.Insert(updateTktWrkFlowSummaryItem);
                //updateTktWrkFlowSummaryItem.Update();

                #endregion

                if (stageEndDate == DateTime.MinValue)
                {
                    if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Assignment")
                    {
                        dashboardSummaryItem.AssignmentSLAMet = "N/A";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Requestor Contact")
                    {
                        dashboardSummaryItem.RequestorContactSLAMet = "N/A";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Resolution")
                    {
                        dashboardSummaryItem.ResolutionSLAMet = "N/A";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Close")
                    {
                        dashboardSummaryItem.CloseSLAMet = "N/A";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Other")
                    {
                        dashboardSummaryItem.OtherSLAMet = "N/A";
                    }
                }
                else if (duration <= slaMinutes)
                {
                    if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Assignment")
                    {
                        dashboardSummaryItem.AssignmentSLAMet = "Yes";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Requestor Contact")
                    {
                        dashboardSummaryItem.RequestorContactSLAMet = "Yes";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Resolution")
                    {
                        dashboardSummaryItem.ResolutionSLAMet = "Yes";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Close")
                    {
                        dashboardSummaryItem.CloseSLAMet = "Yes";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Other")
                    {
                        dashboardSummaryItem.OtherSLAMet = "Yes";
                    }

                    slaMetList.Add(Convert.ToString(ruleItem[DatabaseObjects.Columns.Title]));
                }
                else if (duration > slaMinutes)
                {
                    if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Assignment")
                    {
                        dashboardSummaryItem.AssignmentSLAMet = "No";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Requestor Contact")
                    {
                        dashboardSummaryItem.RequestorContactSLAMet = "No";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Resolution")
                    {
                        dashboardSummaryItem.ResolutionSLAMet = "No";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Close")
                    {
                        dashboardSummaryItem.CloseSLAMet = "No";
                    }
                    else if (Convert.ToString(ruleItem[DatabaseObjects.Columns.SLACategory]) == "Other")
                    {
                        dashboardSummaryItem.OtherSLAMet = "No";
                    }
                }

                if (Convert.ToString(dashboardSummaryItem.AssignmentSLAMet) == "No" ||
                    Convert.ToString(dashboardSummaryItem.RequestorContactSLAMet) == "No" ||
                    Convert.ToString(dashboardSummaryItem.ResolutionSLAMet) == "No" ||
                    Convert.ToString(dashboardSummaryItem.CloseSLAMet) == "No" ||
                    Convert.ToString(dashboardSummaryItem.OtherSLAMet) == "No"
                    )
                {
                    dashboardSummaryItem.ALLSLAsMet = "No";
                }
                else
                {
                    dashboardSummaryItem.ALLSLAsMet = "Yes";
                }
            }

            dashboardSummaryItem.SLAMet = string.Join(",", slaMetList);
        }
        private DataTable OverrideWithRequestTypeSLA(string moduleName,DataRow ticketItem, DataTable moduleSLARules, DataRow[] moduleStages)
        {
            ServicesManager objServicesManager = new ServicesManager(_context);
            DataTable ticketSLARules = moduleSLARules;
            if (ticketSLARules == null)
                ticketSLARules = GetSLARuleSchema();

            if (!ticketSLARules.Columns.Contains(DatabaseObjects.Columns.Use24x7Calendar))
                ticketSLARules.Columns.Add(DatabaseObjects.Columns.Use24x7Calendar, typeof(bool));

            DataRow[] selectedRules = ticketSLARules.Select();

            #region override sla from svc for svc ticket only
            if (moduleName == "SVC")
            {
                // ResolutionSLA in minute
                double resolutionSLA = 0;
                bool startResFromAssigned = false;
                Services service = objServicesManager.LoadByID(Convert.ToInt64(ticketItem[DatabaseObjects.Columns.ServiceTitleLookup]));
                
                if (service != null && !service.SLADisabled)
                {
                    resolutionSLA = service.ResolutionSLA;
                    startResFromAssigned = service.StartResolutionSLAFromAssigned;
                }

                if (resolutionSLA > 0)
                {
                    //get initialed, assigned, resolved and close stages from ticket stage
                    DataRow initiatedStageRow = null;
                    //start sla from assigned stage if StartResolutionSLAFromAssigned is enable otherwise from initial stage
                    if (startResFromAssigned)
                        initiatedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Assigned");

                    if (initiatedStageRow == null)
                        initiatedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Initiated");

                    DataRow resolvedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Resolved");
                    if (resolvedStageRow == null)
                        resolvedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Closed");

                    if (initiatedStageRow != null && resolvedStageRow != null)
                    {
                        DataRow[] sSLAs = ticketSLARules.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.SLACategory) != "Resolution").ToArray();
                        if (sSLAs.Length > 0)
                            ticketSLARules = sSLAs.CopyToDataTable();
                        else if (selectedRules.Length > 0)
                            ticketSLARules = GetSLARuleSchema();

                        DataRow nSLA = ticketSLARules.NewRow();
                        nSLA[DatabaseObjects.Columns.Title] = "Resolution";
                        nSLA[DatabaseObjects.Columns.StageTitleLookup] = UGITUtility.StringToInt(initiatedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.EndStageTitleLookup] = UGITUtility.StringToInt(resolvedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.StartStageStep] = UGITUtility.StringToInt(initiatedStageRow[DatabaseObjects.Columns.ModuleStep]);
                        nSLA[DatabaseObjects.Columns.EndStageStep] = UGITUtility.StringToInt(resolvedStageRow[DatabaseObjects.Columns.ModuleStep]);
                        nSLA[DatabaseObjects.Columns.SLAHours] = resolutionSLA / 60;
                        nSLA[DatabaseObjects.Columns.SLACategory] = "Resolution";
                        nSLA[DatabaseObjects.Columns.Use24x7Calendar] = service != null ? service.Use24x7Calendar : false;
                        ticketSLARules.Rows.Add(nSLA);
                    }
                }
            }
            #endregion
            //return as it is, where request type lookup is not exist or null
            if (!UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketRequestTypeLookup))
                return ticketSLARules;

            //Request type SLA iterations
            //SPFieldLookupValue ticketReqTypeLookupValue = new SPFieldLookupValue(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]));
            // if (ticketReqTypeLookupValue != null)
            //{
            //if (ticketReqTypeLookupValue.LookupId == 0)
            // return ticketSLARules;

            #region override sla from request type
            else if (UGITUtility.IsSPItemExist(ticketItem, DatabaseObjects.Columns.TicketRequestTypeLookup))
            {
                int requestTypeID = UGITUtility.StringToInt(ticketItem[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                if (requestTypeID > 0)
                {

                    //Get ticket location which should be requestor location or user manually set location
                    //  SPFieldLookupValue locationLookup = new SPFieldLookupValue(Convert.ToString(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.LocationLookup)));
                    int ticketLocationID = UGITUtility.StringToInt(UGITUtility.GetSPItemValue(ticketItem, DatabaseObjects.Columns.LocationLookup));
                    bool use24x7Calendar = false;
                    DataRow requestTypeItem = null;
                    double assignmentSLA = 0;
                    double resolutionSLA = 0;
                    double closeSLA = 0;
                    double requestorContactSLA = 0;

                    DateTime startStageDateTime = DateTime.Now;
                    DateTime ticketCreationTime = DateTime.UtcNow;

                    //  DataRow[] requestTypeLocationColl = null;
                    //if (locationLookup != null)
                    //{
                    //    // Get RequestTypeByLocation entry for this location if it exists
                    //    SPQuery query = new SPQuery();
                    //    query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>",
                    //                                DatabaseObjects.Columns.TicketRequestTypeLookup, requestTypeID, DatabaseObjects.Columns.LocationLookup, locationLookup.LookupId);
                    //    requestTypeLocationColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.RequestTypeByLocation, query, spWeb);
                    //}
                    string condition = DatabaseObjects.Columns.TicketRequestTypeLookup + "=" + requestTypeID + " and " + DatabaseObjects.Columns.LocationLookup + "=" + ticketLocationID + " and " + DatabaseObjects.Columns.TenantID + "='" + _context.TenantID + "'";
                    DataTable requestTypeLocationColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestTypeByLocation, condition);
                    if (requestTypeLocationColl != null && requestTypeLocationColl.Rows.Count > 0)
                    {
                        // Found RequestTypeByLocation entry for this location, use values from here
                        DataRow requestTypeByLocation = requestTypeLocationColl.Rows[0];
                        use24x7Calendar = UGITUtility.StringToBoolean(requestTypeByLocation[DatabaseObjects.Columns.Use24x7Calendar]);
                        requestorContactSLA = (UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.RequestorContactSLA])) / 60;
                        assignmentSLA = (UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.AssignmentSLA])) / 60;
                        resolutionSLA = (UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.ResolutionSLA])) / 60;
                        closeSLA = (UGITUtility.StringToDouble(requestTypeByLocation[DatabaseObjects.Columns.CloseSLA])) / 60;
                    }
                    else
                    {
                        // else use values from main request type entry
                        requestTypeItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestType, $"{DatabaseObjects.Columns.ID}={requestTypeID} and {DatabaseObjects.Columns.TenantID}='{_context.TenantID}'").Select().First();
                        if (requestTypeItem != null)
                        {
                            requestorContactSLA = (UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.RequestorContactSLA])) / 60;
                            assignmentSLA = (UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.AssignmentSLA])) / 60;
                            resolutionSLA = (UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.ResolutionSLA])) / 60;
                            closeSLA = (UGITUtility.StringToDouble(requestTypeItem[DatabaseObjects.Columns.CloseSLA])) / 60;
                        }
                    }

                    DataRow initiatedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Initiated");
                    DataRow assignedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Assigned");
                    DataRow resolvedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Resolved");
                    DataRow closedStageRow = moduleStages.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.StageTypeChoice) == "Closed");
                    if (resolvedStageRow == null && closedStageRow != null)
                        resolvedStageRow = closedStageRow;
                    else if (resolvedStageRow == null && closedStageRow == null)
                        resolvedStageRow = assignedStageRow;
                    else if (resolvedStageRow != null && closedStageRow == null)
                        closedStageRow = resolvedStageRow;

                    /* This is wrong, RequestorContactSLA needs to be handled differently!
                    if (requestorContactSLA > 0)
                    {
                        DataRow[] sSLAs = selectedRules.Where(x => x.Field<string>(DatabaseObjects.Columns.SLACategory) != "Requestor Contact").ToArray();
                        if (sSLAs.Length > 0)
                            ticketSLARules = sSLAs.CopyToDataTable();
                        else if (selectedRules.Length > 0)
                            ticketSLARules = GetSLARuleSchema();

                        DataRow nSLA = ticketSLARules.NewRow();
                        nSLA[DatabaseObjects.Columns.Title] = "Requestor Contact";
                        nSLA[DatabaseObjects.Columns.StageTitleLookup] = Convert.ToString(initiatedStageRow[DatabaseObjects.Columns.Title]);
                        nSLA[DatabaseObjects.Columns.EndStageTitleLookup] = Convert.ToString(resolvedStageRow[DatabaseObjects.Columns.Title]);
                        nSLA[DatabaseObjects.Columns.SLAHours] = requestorContactSLA;
                        nSLA[DatabaseObjects.Columns.SLACategory] = "Requestor Contact";
                        ticketSLARules.Rows.Add(nSLA);
                    }
                    */

                    if (assignmentSLA > 0)
                    {
                        DataRow[] sSLAs = ticketSLARules.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.SLACategory) != "Assignment").ToArray();
                        if (sSLAs.Length > 0)
                            ticketSLARules = sSLAs.CopyToDataTable();
                        else if (selectedRules.Length > 0)
                            ticketSLARules = GetSLARuleSchema();

                        DataRow nSLA = ticketSLARules.NewRow();
                        nSLA[DatabaseObjects.Columns.Title] = "Assignment";
                        nSLA[DatabaseObjects.Columns.StageTitleLookup] = Convert.ToString(initiatedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.EndStageTitleLookup] = Convert.ToString(assignedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.StartStageStep] = UGITUtility.StringToInt(initiatedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.EndStageStep] = UGITUtility.StringToInt(assignedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.SLAHours] = assignmentSLA;
                        nSLA[DatabaseObjects.Columns.SLACategory] = "Assignment";
                        nSLA[DatabaseObjects.Columns.Use24x7Calendar] = use24x7Calendar;
                        ticketSLARules.Rows.Add(nSLA);

                    }
                    if (resolutionSLA > 0)
                    {
                        DataRow[] sSLAs = ticketSLARules.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.SLACategory) != "Resolution").ToArray();
                        if (sSLAs.Length > 0)
                            ticketSLARules = sSLAs.CopyToDataTable();
                        else if (selectedRules.Length > 0)
                            ticketSLARules = GetSLARuleSchema();

                        DataRow nSLA = ticketSLARules.NewRow();
                        nSLA[DatabaseObjects.Columns.Title] = "Resolution";
                        nSLA[DatabaseObjects.Columns.StageTitleLookup] = Convert.ToString(initiatedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.EndStageTitleLookup] = Convert.ToString(resolvedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.StartStageStep] = UGITUtility.StringToInt(initiatedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.EndStageStep] = UGITUtility.StringToInt(resolvedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.SLAHours] = resolutionSLA;
                        nSLA[DatabaseObjects.Columns.SLACategory] = "Resolution";
                        nSLA[DatabaseObjects.Columns.Use24x7Calendar] = use24x7Calendar;
                        ticketSLARules.Rows.Add(nSLA);

                    }
                    if (closeSLA > 0)
                    {
                        DataRow[] sSLAs = ticketSLARules.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.SLACategory) != "Close").ToArray();
                        if (sSLAs.Length > 0)
                            ticketSLARules = sSLAs.CopyToDataTable();
                        else if (selectedRules.Length > 0)
                            ticketSLARules = GetSLARuleSchema();

                        DataRow nSLA = ticketSLARules.NewRow();
                        nSLA[DatabaseObjects.Columns.Title] = "Close";

                        nSLA[DatabaseObjects.Columns.StageTitleLookup] = Convert.ToString(initiatedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.EndStageTitleLookup] = Convert.ToString(closedStageRow[DatabaseObjects.Columns.ID]);
                        nSLA[DatabaseObjects.Columns.StartStageStep] = UGITUtility.StringToInt(initiatedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.EndStageStep] = UGITUtility.StringToInt(closedStageRow[DatabaseObjects.Columns.StageStep]);
                        nSLA[DatabaseObjects.Columns.SLAHours] = closeSLA;
                        nSLA[DatabaseObjects.Columns.SLACategory] = "Close";
                        nSLA[DatabaseObjects.Columns.Use24x7Calendar] = use24x7Calendar;
                        ticketSLARules.Rows.Add(nSLA);
                    }
                }
            }
            #endregion
            return ticketSLARules;
        }
        private DataTable GetSLARuleSchema()
        {
            DataTable table = new DataTable();
            table.Columns.Add(DatabaseObjects.Columns.ID, typeof(int));
            table.Columns.Add(DatabaseObjects.Columns.Title);
            table.Columns.Add(DatabaseObjects.Columns.SLACategory);
            table.Columns.Add(DatabaseObjects.Columns.SLAHours, typeof(double));
            table.Columns.Add(DatabaseObjects.Columns.SLATarget, typeof(double));
            table.Columns.Add(DatabaseObjects.Columns.EndStageTitleLookup);
            table.Columns.Add(DatabaseObjects.Columns.StageTitleLookup);
            table.Columns.Add(DatabaseObjects.Columns.StartStageStep, typeof(int));
            table.Columns.Add(DatabaseObjects.Columns.EndStageStep, typeof(int));
            table.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup);
            table.Columns.Add(DatabaseObjects.Columns.Use24x7Calendar, typeof(bool));
            return table;
        }
    }
}