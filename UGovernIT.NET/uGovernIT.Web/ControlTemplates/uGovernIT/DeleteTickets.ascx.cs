using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility.Entities;
using System.Reflection;
using uGovernIT.Helpers;
using uGovernIT.DMS.Amazon;
using uGovernIT.DAL.Infratructure;
using uGovernIT.Utility.Entities.Common;

/*
 * 
 * Delete Ticket functionality details
 * ====================================
 * 
 * a) Common for all modules
 *   1) ModuleWorkflowHistory(Recycle) -> ModuleWorkflowHistoryArchive
 *   2) ModuleUserStatistics(Delete)
 *   3) TicketRelationship(Recycle)
 *   4) TicketEmails (Recycle)
 *   5) Escalations in ScheduleActions (Delete)
 *   6) DashboardSummary (Delete)
 *   7) TicketWorkflowSLASummary (Delete)
 * 
 * b) On SVC  
 *   1) SVCRequest(Recycle) -> SVCRequest Archive
 * 
 * c) On TSK
 *   1) TSK Projects(Recycle) -> TSK Projects Archive 
 *   2) TSK Task ( Recycle)
 *   3) Allocation entries are  directly deleted as we do in other cases
 * 
 * d) On NPR
 *   1) NPR Request(Recycle) -> NPR Request Archive
 *   2) NPRBudget (Recycle)
 *   3) NPRMonthlyBudget (Recycle)
 *   4) NPR Resource (Recycle)
 *   5) NPR Task (Recycle)
 * 
 * e) On PMM
 *   1) PMM Projects(Recycle) -> PMM Projects Archive
 *   2) PMMBudget(Recycle)
 *   3) PMMBudgetHistory(Recycle)
 *   4) PMMMonthlyBudget(Recycle)
 *   5) PMMMonthlyBudgetHistroy(Recycle)
 *   6) PMM Comments(Recycle)
 *   7) PMM Comments Histroy(Recycle)
 *   8) PMM Task(Recycle) 
 *   9) PMM Task Histroy(Recycle)
 *  10) PMM Budget Actuals (Recycle)
 *  11) PMM Issues Recycle(Recycle)
 *  12) PMM Issues History(Recycle)
 *  13) PMM Monitors State(Recycle)
 *  14) PMM Monitors State History(Recycle)
 *  15) PMM Risks(Recycle)
 *  16) PMM Events(Recycle)
 *  17) Project Release(Recycle)
 *  18) Portal Info(Recycle)
 *  19) PMM Sprint Task(Recycle)
 *  20) PMM Sprint(Recycle)
 *  21) PMM Sprint Summary(Recycle)
 *  22) PMM Baseline(Recycle)
 *  23) Documents Portal
 *  
 * f) VFM: 
 *   1) VendorSOWInvoiceDetails (Recycle)
 *   
 * g) VPM:
 *   1) VendorSLAPerformance (Recycle)
 *   
 */

namespace uGovernIT.Web
{
    public partial class DeleteTickets : UserControl
    {
        public string Module { get; set; }
        public List<string> selectedTicketIds { get; set; }
        public List<string> ExcludedTickets { get; set; }
        public bool IsFilteredTableExist { get; set; }
        public DataTable FilteredTable { get; set; }

        ListPicker customListPicker;
        DataTable dtTickets = null;
        List<string> TicketIds = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        TicketManager ObjTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        DMSManagerService repositoryService = new DMSManagerService(HttpContext.Current.GetManagerContext());
        protected void Page_Init(object sender, EventArgs e)
        {
            customListPicker = ((ListPicker)lstPicker);
            
            customListPicker.EnableModuleDropdown = true;
            customListPicker.IsMultiSelect = true;
            customListPicker.Module = Module;
            customListPicker.FilteredTable = FilteredTable;
            customListPicker.IsDeleteTicket = true;
            if (hdnTicketIds.Contains("IsNextClick") && hdnTicketIds.Contains("SelectedTicketIds") && Convert.ToString(hdnTicketIds.Get("IsNextClick")) == "true" && !string.IsNullOrEmpty(Convert.ToString(hdnTicketIds.Get("SelectedTicketIds"))))
            {
                CreateControls();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnShowTicketInfo_Click(object sender, EventArgs e)
        {
            // CreateControls();
            divDetails.Visible = true;
            divList.Visible = false;
        }

        private void CreateControls()
        {
            object[] arrayTickets = (object[])hdnTicketIds.Get("SelectedTicketIds");
            TicketIds = new List<string>();
            foreach (object item in arrayTickets)
            {
                TicketIds.Add(Convert.ToString(item));
            }
            if (TicketIds != null && TicketIds.Count > 0)
            {
                if (dtTickets == null)
                {
                    dtTickets = new DataTable();
                    dtTickets.Columns.Add(new DataColumn(DatabaseObjects.Columns.ParentTicketId));
                    dtTickets.Columns.Add(new DataColumn(DatabaseObjects.Columns.ChildTicketId));
                    pnlTree.Controls.Clear();
                }
                int cnt = 1;
                foreach (string ticketId in TicketIds)
                {
                    string[] ticketInfo = ticketId.Split('-');
                    string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                   DataRow ticketItem = Ticket.GetCurrentTicket(context,moduleName, ticketId);
                    if (ticketItem != null)
                    {
                        DataRow dr = dtTickets.NewRow();
                        dr[DatabaseObjects.Columns.ParentTicketId] = ticketId;
                        dr[DatabaseObjects.Columns.ChildTicketId] = string.Empty;
                        dtTickets.Rows.Add(dr);
                        TreeView treeView = new TreeView();
                        treeView.EnableViewState = false;
                        treeView.ShowCheckBoxes = TreeNodeTypes.All;
                        TreeNode parentNode = new TreeNode(string.Format("{0}: {1}", Convert.ToString(ticketId), Convert.ToString(ticketItem[DatabaseObjects.Columns.Title])), ticketId);
                        parentNode.Checked = true;
                        GetChildTickets(ticketId, parentNode);
                        treeView.Nodes.Add(parentNode);
                        Panel pnl = new Panel();
                        if (cnt % 2 != 0)
                        {
                            pnl.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F2F2F2");
                            pnl.Style.Add(HtmlTextWriterStyle.BorderColor, "black");
                            pnl.Style.Add(HtmlTextWriterStyle.BorderCollapse, "collapse");
                        }
                        pnl.Controls.Add(treeView);
                        pnl.Style.Add("display", "block");
                        pnlTree.Controls.Add(pnl);
                        treeView.ExpandAll();
                        treeView.Attributes.Add("CustomType", "treeViewControl");
                        cnt++;
                    }


                }
                divDetails.Visible = true;
                divList.Visible = false;
            }
        }

        private void RetrieveParentNode(TreeNode treeNode, bool isChecked)
        {
            treeNode.Parent.Checked = isChecked;
        }

        private void checkchild(TreeNode treeNode, bool isChecked)
        {
            foreach (TreeNode node in treeNode.ChildNodes)
            {
                node.Checked = isChecked;
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            divDetails.Visible = false;
            divList.Visible = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string history = string.Empty;
            List<string> lstDeletedTickets = new System.Collections.Generic.List<string>();
            foreach (Control control in pnlTree.Controls)
            {
                if (control.GetType() == typeof(Panel) && control.Controls.Count > 0 && control.Controls[0].GetType() == typeof(TreeView))
                {
                    TreeView treeView = (TreeView)control.Controls[0];
                    TreeNodeCollection collection = treeView.CheckedNodes;
                    if (collection != null && collection.Count > 0)
                    {
                        foreach (TreeNode node in collection)
                        {
                            lstDeletedTickets.Add(node.Value);
                        }
                    }
                }
            }

            UGITModule module = null;
            ModuleUserStatisticsManager moduleUserStatisticsManager = new ModuleUserStatisticsManager(context);
            ModuleWorkflowHistoryManager moduleWorkflowHistoryManager = new ModuleWorkflowHistoryManager(context);
            ModuleWorkflowHistoryArchiveManager moduleWorkflowHistoryArchiveManager = new ModuleWorkflowHistoryArchiveManager(context);
            DashboardSummaryManager dashboardSummaryManager = new DashboardSummaryManager(context);
            WorkflowSLASummaryManager workflowSLASummaryManager = new WorkflowSLASummaryManager(context);
            EmailsManager emailsManager = new EmailsManager(context);

            foreach (string ticketID in lstDeletedTickets)
            {
                bool tasksDeleted = false;

                string moduleName = uHelper.getModuleNameByTicketId(ticketID);
                module = ObjModuleViewManager.GetByName(moduleName);
                DataRow spItem = ObjTicketManager.GetByTicketID(module,ticketID);
                if (spItem != null)
                {
                    // Copying in ModuleWorkflowHistoryArchive list and deleteing from ModuleWorkflowHistory list
                    string query = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketID);


                    List<ModuleWorkflowHistory> moduleWorkFlowItemColl = moduleWorkflowHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory).Select(query);
                    if (moduleWorkFlowItemColl != null && moduleWorkFlowItemColl.Count() > 0)
                    {
                        int cnt = moduleWorkFlowItemColl.Count();
                        foreach (ModuleWorkflowHistory moduleWorkItem in moduleWorkFlowItemColl)
                        {

                            ModuleWorkflowHistoryArchive moduleWorkFlowHistoryArchiveItem = new ModuleWorkflowHistoryArchive();
                            Type typeB = moduleWorkFlowHistoryArchiveItem.GetType();
                            foreach (PropertyInfo property in moduleWorkItem.GetType().GetProperties())
                            {
                                if (!property.CanRead || (property.GetIndexParameters().Length > 0))
                                    continue;

                                PropertyInfo other = typeB.GetProperty(property.Name);
                                if ((other != null) && (other.CanWrite))
                                    other.SetValue(moduleWorkFlowHistoryArchiveItem, property.GetValue(moduleWorkItem, null), null);
                            }
                            moduleWorkflowHistoryManager.Delete(moduleWorkItem);
                            moduleWorkflowHistoryArchiveManager.Insert(moduleWorkFlowHistoryArchiveItem);
                            //CopySpItem(moduleWorkItem, moduleWorkFlowHistoryArchiveItem);
                        }
                        history = string.Format("Recycled {0} items from  WorkflowHistory List.", cnt);
                    }
                    // Deleting from ModuleUserStatistics list

                    List<ModuleUserStatistic> moduleUserStatsColl = moduleUserStatisticsManager.Load(x => x.TicketId == ticketID).ToList();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics).Select(query);
                    if (moduleUserStatsColl != null && moduleUserStatsColl.Count() > 0)
                    {
                        history = string.Format(" {0} Deleted following items from  ModuleUserStatistics List:", history);
                        foreach (ModuleUserStatistic moduleUserStats in  moduleUserStatsColl)
                        {                            
                            history = string.Format("{0} UserRole {1}- TicketUser {2},", history, moduleUserStats.UserRole, moduleUserStats.UserName);
                            moduleUserStatisticsManager.Delete(moduleUserStats);
                        }
                        history = history.Substring(0, history.Length - 1);
                        history = string.Format("{0}.", history);
                    }
                    // Deleting from TicketRelationship list
                    string queryTicketRelation = string.Format("{0}='{1}' or {2}='{3}'", DatabaseObjects.Columns.ParentTicketId, ticketID, DatabaseObjects.Columns.ChildTicketId, ticketID);
                    TicketRelationManager ticketRelationManager = new TicketRelationManager(context);
                   List<TicketRelation> ticketRelationshipColl = ticketRelationManager.Load(x => x.ParentTicketID == ticketID || x.ChildTicketID == ticketID).ToList();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelationship).Select(queryTicketRelation);
                    if (ticketRelationshipColl != null && ticketRelationshipColl.Count() > 0)
                    {
                        history = string.Format(" {0} Recycled following items from  TicketRelationship List:", history);
                        foreach (TicketRelation trelation in ticketRelationshipColl)
                        {
                            history = string.Format("{0} ParentTicketId {1}- ChildTicketId {2},", history, trelation.ParentTicketID, trelation.ChildTicketID);
                            ticketRelationManager.Delete(trelation);
                            tasksDeleted = true;
                        }
                        history = history.Substring(0, history.Length - 1);
                        history = string.Format("{0}.", history);
                    }

                    // Deleting from TicketEmails list
                    List<Email> ticketEmailsColl = emailsManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketEmails).Select(query);
                    if (ticketEmailsColl != null && ticketEmailsColl.Count() > 0)
                    {
                        history = string.Format(" {0} Recycled {1} items from  TicketEmails List:", history, ticketEmailsColl.Count());
                        foreach (Email item in ticketEmailsColl)
                        {                            
                            emailsManager.Delete(item);
                            //item.Recycle();
                        }
                        history = history.Substring(0, history.Length - 1);
                        history = string.Format("{0}.", history);
                    }

                    // Deleting from DashboardSummary list
                    List<DashboardSummary> dashboardSummaryColl = dashboardSummaryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardSummary).Select(query);
                    if (dashboardSummaryColl != null && dashboardSummaryColl.Count() > 0)
                    {
                        foreach (DashboardSummary dsummary in dashboardSummaryColl)
                        {
                            dashboardSummaryManager.Delete(dsummary);
                        }
                    }

                    // Deleting from TicketWorkflowSLASummary list
                    List<WorkflowSLASummary> ticketSummaryColl = workflowSLASummaryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketWorkflowSLASummary).Select(query);
                    if (ticketSummaryColl != null && ticketSummaryColl.Count() > 0)
                    {
                        foreach(WorkflowSLASummary wfSummary in  ticketSummaryColl)
                        {
                            workflowSLASummaryManager.Delete(wfSummary);
                        }
                    }

                    // Delete any pending escalations for this ticket
                    EscalationProcess escalationProcess = new EscalationProcess(context);
                    escalationProcess.DeleteEscalation(spItem);
                    if (moduleName == ModuleNames.NPR)
                    {
                        // NPR Budget
                        //string qryNPRBudget = string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketNPRIdLookup, spItem[DatabaseObjects.Columns.Id]);
                        ModuleBudgetManager moduleBudgetManager = new ModuleBudgetManager(context);
                        //DataRow[] nprBudgetCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRBudget).Select(qryNPRBudget);
                        List<ModuleBudget> nprBudgetCollection = moduleBudgetManager.Load(x => x.TicketId == ticketID);
                        if (nprBudgetCollection != null)
                        {
                            history = string.Format(" {0} Deleted following items from  NPR Budget List:", history);
                            foreach ( ModuleBudget nprBudgetItem in nprBudgetCollection)
                            {
                               
                                history = string.Format("{0} Budget Details {1},", history, nprBudgetItem.BudgetItem);
                                moduleBudgetManager.Delete(nprBudgetItem);
                            }
                        }

                        // NPR Budget
                       //string qryNPRBudgetMonthly =  string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketNPRIdLookup, spItem[DatabaseObjects.Columns.Id]);
                        ModuleMonthlyBudgetManager moduleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);
                        List<ModuleMonthlyBudget> nprBudgetMonthlyCollection = moduleMonthlyBudgetManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRMonthlyBudget).Select(qryNPRBudgetMonthly);
                        if (nprBudgetMonthlyCollection != null)
                        {
                            history = string.Format(" {0} Deleted following items from  NPR Budget Monthly List:", history);
                            foreach (ModuleMonthlyBudget nprBudgetMonthlyItem in  nprBudgetMonthlyCollection)
                            {
                                history = string.Format("{0} Monthly Budget Details {1},", history, nprBudgetMonthlyItem.BudgetType);
                                // nprBudgetMonthlyItem.Recycle();
                                moduleMonthlyBudgetManager.Delete(nprBudgetMonthlyItem);
                            }
                        }


                        // NPR Resources
                        //string qryNPRResources =  string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketNPRIdLookup, spItem[DatabaseObjects.Columns.Id]);
                        NPRResourcesManager nPRResourcesManager = new NPRResourcesManager(context);
                        List<NPRResource> nprNPRResourcesCollection = nPRResourcesManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRResources).Select(qryNPRResources);
                        if (nprNPRResourcesCollection != null)
                        {
                            history = string.Format(" {0} Deleted following items from  NPR Resources List:", history);
                            foreach (NPRResource nprNPRResourcesItem in nprNPRResourcesCollection)
                            {
                                history = string.Format("{0} Resources Details {1},", history, nprNPRResourcesItem.BudgetTypeChoice);
                                //nprNPRResourcesItem.Recycle();
                                nPRResourcesManager.Delete(nprNPRResourcesItem);
                            }
                        }

                        // NPR Tasks
                       // string qryNPRTasks =  string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketNPRIdLookup, spItem[DatabaseObjects.Columns.Id]);
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> nprNPRTaskCollection = taskManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.NPRTasks).Select(qryNPRTasks);
                        if (nprNPRTaskCollection != null)
                        {
                            history = string.Format(" {0} Deleted following items from  NPR Tasks List:", history);
                            foreach (UGITTask nprNPRTaskItem in nprNPRTaskCollection)
                            {
                                history = string.Format("{0} Task Details {1},", history, nprNPRTaskItem.Title);
                                //nprNPRTaskItem.Recycle();
                                taskManager.Delete(nprNPRTaskItem);
                                tasksDeleted = true;
                            }
                        }
                        //RMMSummaryHelper.DeleteAllocationsByTasks(SPContext.Current.Web, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.TSK)
                    {
                        // TSK Tasks
                        string qryTSKTasks = string.Format("{0}='{1}'", DatabaseObjects.Columns.TSKIDLookup, spItem[DatabaseObjects.Columns.Id]);
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> nprTSKTaskCollection = taskManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKTasks).Select(qryTSKTasks);
                        if (nprTSKTaskCollection != null)
                        {
                            history = string.Format(" {0} Deleted following items from  NPR Tasks List:", history);
                            foreach (UGITTask nprTSKTaskItem in nprTSKTaskCollection)
                            {
                                history = string.Format("{0} Task Details {1},", history, nprTSKTaskItem.Title);
                                //nprTSKTaskItem.Recycle();
                                taskManager.Delete(nprTSKTaskItem);
                                tasksDeleted = true;
                            }
                        }

                        //RMMSummaryHelper.DeleteAllocationsByTasks(SPContext.Current.Web, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.PMM)
                    {
                        // PMM Budget
                        string pmmChildListQuery =  string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.TicketPMMIdLookup, spItem[DatabaseObjects.Columns.Id]);
                        ModuleBudgetManager moduleBudgetManager= new ModuleBudgetManager(context);
                        List<ModuleBudget> pmmBudgetCollection = moduleBudgetManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMBudget).Select(pmmChildListQuery);
                        if (pmmBudgetCollection != null && pmmBudgetCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from PMM Budget List:", history);
                            foreach (ModuleBudget pmmBudgetItem in pmmBudgetCollection)
                            {
                               history = string.Format("{0} Budget Details {1},", history, pmmBudgetItem.BudgetItem);
                                //pmmBudgetItem.Recycle();
                                moduleBudgetManager.Delete(pmmBudgetItem);
                            }
                        }

                        // PMM Budget History Commented By Munna,After approval of Manish Sir
                        //DataRow[] pmmBudgetHistoryCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMBudgetHistory).Select(pmmChildListQuery);
                        //if (pmmBudgetHistoryCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from PMM Budget List:", history);
                        //    while (pmmBudgetHistoryCollection.Count() > 0)
                        //    {
                        //        DataRow pmmBudgetIHistorytem = pmmBudgetHistoryCollection[0];
                        //        history = string.Format("{0} Budget Details {1},", history, pmmBudgetIHistorytem[DatabaseObjects.Columns.BudgetItem]);
                        //        //pmmBudgetIHistorytem.Recycle();
                        //    }
                        //}

                        //ModuleBudgetHistory
                        ModuleBudgetHistoryManager moduleBudgetHistoryManager = new ModuleBudgetHistoryManager(context);
                        List<ModuleBudgetHistory> moduleBudgetHistories = moduleBudgetHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetHistory).Select(pmmChildListQuery);
                        if (moduleBudgetHistories != null && moduleBudgetHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ModuleBudgetHistory:", history);
                            foreach (ModuleBudgetHistory moduleBudgetHistory in moduleBudgetHistories)
                            {
                                history = string.Format("{0} ModuleBudgetHistory Details {1},", history, moduleBudgetHistory.BudgetItem);
                                moduleBudgetHistoryManager.Delete(moduleBudgetHistory);
                            }
                        }

                        //ModuleBudgetActualsHistory
                        ModuleBudgetActualsHistoryManager moduleBudgetActualsHistoryManager = new ModuleBudgetActualsHistoryManager(context);
                        List<ModuleBudgetsActualHistory> moduleBudgetsActualHistories = moduleBudgetActualsHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudgetHistory).Select(pmmChildListQuery);
                        if (moduleBudgetsActualHistories != null && moduleBudgetsActualHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ModuleBudgetActualsHistory:", history);
                            foreach (ModuleBudgetsActualHistory moduleBudgetsActualHistory in moduleBudgetsActualHistories)
                            {
                                history = string.Format("{0} ModuleBudgetActualsHistory Details {1},", history, moduleBudgetsActualHistory.Title);
                                moduleBudgetActualsHistoryManager.Delete(moduleBudgetsActualHistory);
                            }
                        }

                        // PMM Budget Monthly
                        ModuleMonthlyBudgetManager moduleMonthlyBudgetManager = new ModuleMonthlyBudgetManager(context);

                        List<ModuleMonthlyBudget> pmmBudgetMonthlyCollection = moduleMonthlyBudgetManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMMonthlyBudget).Select(pmmChildListQuery);
                        if (pmmBudgetMonthlyCollection != null && pmmBudgetMonthlyCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Budget Monthly List:", history);
                            foreach (ModuleMonthlyBudget pmmBudgetMonthlyItem in pmmBudgetMonthlyCollection)
                            {
                                history = string.Format("{0} Monthly Budget Details {1},", history, pmmBudgetMonthlyItem.BudgetType);
                                //pmmBudgetMonthlyItem.Recycle();
                                moduleMonthlyBudgetManager.Delete(pmmBudgetMonthlyItem);
                            }
                        }

                        //ModuleMonthlyBudgetHistory
                        ModuleMonthlyBudgetHistoryManager moduleMonthlyBudgetHistoryManager = new ModuleMonthlyBudgetHistoryManager(context);
                        List<ModuleMonthlyBudgetHistory> moduleMonthlyBudgetHistories = moduleMonthlyBudgetHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonthlyBudgetHistory).Select(pmmChildListQuery);
                        if (moduleMonthlyBudgetHistories != null && moduleMonthlyBudgetHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ModuleMonthlyBudgetHistory:", history);
                            foreach (ModuleMonthlyBudgetHistory moduleMonthlyBudgetHistory in moduleMonthlyBudgetHistories)
                            {
                                history = string.Format("{0} ModuleMonthlyBudgetHistory Details {1},", history, moduleMonthlyBudgetHistory.TicketId);
                                moduleMonthlyBudgetHistoryManager.Delete(moduleMonthlyBudgetHistory);
                            }
                        }

                        //ModuleTaskHistory
                        ModuleTaskHistoryManager moduleTaskHistoryManager = new ModuleTaskHistoryManager(context);
                        List<ModuleTasksHistory> moduleTasksHistories = moduleTaskHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTaskHistory).Select(pmmChildListQuery);
                        if (moduleTasksHistories != null && moduleTasksHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ModuleTasksHistory:", history);
                            foreach (ModuleTasksHistory moduleTasksHistory in moduleTasksHistories)
                            {
                                history = string.Format("{0} ModuleTasksHistory Details {1},", history, moduleTasksHistory.Title);
                                moduleTaskHistoryManager.Delete(moduleTasksHistory);
                            }
                        }

                        //ProjectStageHistory
                        ProjectStageHistoryManager projectStageHistoryManager = new ProjectStageHistoryManager(context);
                        List<ProjectStageHistory> projectStageHistories = projectStageHistoryManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleTaskHistory).Select(pmmChildListQuery);
                        if (projectStageHistories != null && projectStageHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ProjectStageHistory:", history);
                            foreach (ProjectStageHistory projectStageHistory in projectStageHistories)
                            {
                                history = string.Format("{0} ProjectStageHistory Details {1},", history, projectStageHistory.Title);
                                projectStageHistoryManager.Delete(projectStageHistory);
                            }
                        }

                        // PMM Budget Monthly History Added by mudassir 16 march 2020
                        //DataRow[] pmmBudgetMonthlyHistoryCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMMonthlyBudgetHistory).Select(pmmChildListQuery);
                        //if (pmmBudgetMonthlyHistoryCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from  PMM Budget Monthly History List:", history);
                        //    while (pmmBudgetMonthlyHistoryCollection.Count() > 0)
                        //    {
                        //        DataRow pmmBudgetMonthlyHistoryItem = pmmBudgetMonthlyHistoryCollection[0];
                        //        history = string.Format("{0} Monthly Budget History Details {1},", history, pmmBudgetMonthlyHistoryItem[DatabaseObjects.Columns.BudgetType]);
                        //        // pmmBudgetMonthlyHistoryItem.Recycle();
                        //    }
                        //}
                        //End
                        // PMM Budgets Actual
                        BudgetActualsManager budgetActualsManager = new BudgetActualsManager(context);
                        List<BudgetActual> pmmBudgetActualsCollection = budgetActualsManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMBudgetActuals).Select(pmmChildListQuery);
                        if (pmmBudgetActualsCollection != null && pmmBudgetActualsCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from Budgets Actual:", history);
                            foreach (BudgetActual pmmBudgetActualsItem in pmmBudgetActualsCollection)
                            {
                              
                                history = string.Format("{0} Budgets Actual Details {1},", history, pmmBudgetActualsItem.ModuleBudgetLookup);
                                // pmmBudgetActualsItem.Recycle();
                                budgetActualsManager.Delete(pmmBudgetActualsItem);
                            }
                        }

                        // PMM Tasks
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                       List<UGITTask> pmmTaskCollection = taskManager.Load(x => x.TicketId == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMTasks).Select(pmmChildListQuery);
                        if (pmmTaskCollection != null && pmmTaskCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Tasks List:", history);
                            foreach ( UGITTask pmmTaskItem in pmmTaskCollection)
                            {
                                history = string.Format("{0} Task Details {1},", history, pmmTaskItem.Title);
                               
                                taskManager.Delete(pmmTaskItem);
                                tasksDeleted = true;
                            }
                        }

                        // PMM Tasks History 
                        //DataRow[] pmmTaskHistoryCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMTasksHistory).Select(pmmChildListQuery);
                        //if (pmmTaskHistoryCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from  PMM Tasks History:", history);
                        //    while (pmmTaskHistoryCollection.Count() > 0)
                        //    {
                        //        DataRow pmmTaskHistoryItem = pmmTaskHistoryCollection[0];
                        //        history = string.Format("{0} Task History Details {1},", history, pmmTaskHistoryItem[DatabaseObjects.Columns.Title]);
                        //        //pmmTaskHistoryItem.Recycle();
                        //    }
                        //}
                        
                        // PMM Comments
                        PMMCommentManager pMMCommentManager = new PMMCommentManager(context);
                        List<PMMComments> pmmCommentsCollection = pMMCommentManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.ID]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments).Select(pmmChildListQuery);
                        if (pmmCommentsCollection != null && pmmCommentsCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Comments:", history);
                            foreach ( PMMComments pmmCommentsItem in pmmCommentsCollection)
                            {
                                history = string.Format("{0} Comments Details {1},", history, pmmCommentsItem.Title);
                                pMMCommentManager.Delete(pmmCommentsItem);
                            }
                        }

                        // PMM Comments Histroy
                        PMMCommentHistoryManager pMMCommentHistoryManager = new PMMCommentHistoryManager(context);
                        List<PMMCommentHistory> pMMCommentHistories = pMMCommentHistoryManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.ID]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMComments).Select(pmmChildListQuery);
                        if (pMMCommentHistories != null && pMMCommentHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM CommentHistory:", history);
                            foreach (PMMCommentHistory pMMCommentHistory in pMMCommentHistories)
                            {
                                history = string.Format("{0} CommentHistory Details {1},", history, pMMCommentHistory.Title);
                                pMMCommentHistoryManager.Delete(pMMCommentHistory);
                            }
                        }

                        // PMM Issues
                        //All the records from ModuleTasks, including Issues have already been deleted for this ticket.
                        //PMMIssueManager pMMIssueManager = new PMMIssueManager(context);
                        //List<PMMIssues> pmmIssuesCollection = pMMIssueManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMIssues).Select(pmmChildListQuery);
                        List<UGITTask> lstTasks = taskManager.LoadByProjectID(spItem[DatabaseObjects.Columns.Id].ToString());
                        List<UGITTask> pmmIssuesCollection = lstTasks.Where(x => x.SubTaskType.ToLower() == "issues").ToList();
                        if (pmmIssuesCollection != null && pmmIssuesCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Issues:", history);
                            foreach (UGITTask pmmIssuesItem in  pmmIssuesCollection)
                            {
                                history = string.Format("{0} Issues Details {1},", history, pmmIssuesItem.Title);
                                //pmmIssuesItem.Recycle();
                                taskManager.Delete(pmmIssuesItem);
                            }
                        }

                        // PMM Issues History
                        //DataRow[] pmmIssuesHistoryCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMIssuesHistory).Select(pmmChildListQuery);
                        //if (pmmIssuesHistoryCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from  PMM Issues History:", history);
                        //    while (pmmIssuesHistoryCollection.Count() > 0)
                        //    {
                        //        DataRow pmmIssuesHistoryItem = pmmIssuesHistoryCollection[0];
                        //        history = string.Format("{0} Issues History Details {1},", history, pmmIssuesHistoryItem[DatabaseObjects.Columns.Title]);
                        //        //pmmIssuesHistoryItem.Recycle();
                        //    }
                        //}

                        // PMM Monitors
                        ProjectMonitorStateManager projectMonitorStateManager = new ProjectMonitorStateManager(context);
                        List<ProjectMonitorState> pmmMonitorsCollection = projectMonitorStateManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState).Select(pmmChildListQuery);
                        if (pmmMonitorsCollection != null && pmmMonitorsCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Monitors:", history);
                            foreach ( ProjectMonitorState pmmMonitorsItem in pmmMonitorsCollection)
                            {
                                history = string.Format("{0} Monitors Details {1},", history, pmmMonitorsItem.Title);
                                projectMonitorStateManager.Delete(pmmMonitorsItem);
                            }
                        }

                        // PMM Monitors History 
                        //DataRow[] pmmMonitorsHistoryCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorStateHistory).Select(pmmChildListQuery);
                        //if (pmmMonitorsHistoryCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from  PMM Monitors History:", history);
                        //    while (pmmMonitorsHistoryCollection.Count() > 0)
                        //    {
                        //        DataRow pmmMonitorsHistoryItem = pmmMonitorsHistoryCollection[0];
                        //        history = string.Format("{0} Monitors History Details {1},", history, pmmMonitorsHistoryItem[DatabaseObjects.Columns.Title]);
                        //        //pmmMonitorsHistoryItem.Recycle();
                        //    }
                        //}

                        ProjectMonitorStateHistoryManager projectMonitorStateHistoryManager = new ProjectMonitorStateHistoryManager(context);
                        List<ProjectMonitorStateHistory> projectMonitorStateHistories = projectMonitorStateHistoryManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState).Select(pmmChildListQuery);
                        if (projectMonitorStateHistories != null && projectMonitorStateHistories.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM ProjectMonitorStateHistory:", history);
                            foreach (ProjectMonitorStateHistory projectMonitorStateHistory in projectMonitorStateHistories)
                            {
                                history = string.Format("{0} Monitors Details {1},", history, projectMonitorStateHistory.Title);
                                projectMonitorStateHistoryManager.Delete(projectMonitorStateHistory);
                            }
                        }


                        // PMM Risks 
                        //All the records from ModuleTasks, including Risks have already been deleted for this ticket.
                        //PMMRiskManager pMMRiskManager = new PMMRiskManager(context);
                        //List<PMMRisks> pmmRisksCollection = pMMRiskManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMRisks).Select(pmmChildListQuery);
                        List<UGITTask> pmmRisksCollection = lstTasks.Where(x => x.SubTaskType.ToLower() == "risk").ToList();

                        if (pmmRisksCollection != null && pmmRisksCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Risks:", history);
                            foreach (UGITTask pmmRisksCollectionItem in pmmRisksCollection)
                            {
                                history = string.Format("{0} Task Details {1},", history, pmmRisksCollectionItem.Title);
                                //pmmRisksCollectionItem.Recycle();
                                taskManager.Delete(pmmRisksCollectionItem);
                            }
                        }

                        //PMM Events.
                        PMMEventManager pMMEventManager = new PMMEventManager(context);
                        List<PMMEvents> pmmEventsCollection = pMMEventManager.Load(x => x.PMMIdLookup == Convert.ToString(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMEvents).Select(pmmChildListQuery);
                        if (pmmEventsCollection != null && pmmEventsCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  PMM Events:", history);
                            foreach (PMMEvents pmmEventsCollectionItem in  pmmEventsCollection)
                            {
                                history = string.Format("{0} Events Details {1},", history, pmmEventsCollectionItem.Title);
                                // pmmEventsCollectionItem.Recycle();
                                pMMEventManager.Delete(pmmEventsCollectionItem);
                            }
                        }

                        //PMM Baseline.
                        //DataRow[] pmmBaselineCollection = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMBaselineDetail).Select( pmmChildListQuery);
                        //if (pmmBaselineCollection != null)
                        //{
                        //    history = string.Format(" {0} Deleted following items from  PMM Baseline:", history);
                        //    while (pmmBaselineCollection.Count() > 0)
                        //    {
                        //        DataRow pmmBaselineCollectionItem = pmmBaselineCollection[0];
                        //        history = string.Format("{0} Baseline Details {1},", history, pmmBaselineCollectionItem[DatabaseObjects.Columns.Title]);
                        //        //pmmBaselineCollectionItem.Recycle();
                        //    }
                        //}

                        BaseLineDetailsManager baselineMrg = new BaseLineDetailsManager(context);
                        List<BaseLineDetails> baselineCollection = baselineMrg.Load(x => x.TicketID == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.BaseLineDetails).Select( pmmChildListQuery);
                        if (baselineCollection != null && baselineCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from BaseLineDetails:", history);
                            foreach (BaseLineDetails baseLine in baselineCollection)
                            {
                                history = string.Format("{0} Sprint Summary Details {1},", history, baseLine.Title);
                                //pmmSprintSummaryCollectionItem.Recycle();
                                baselineMrg.Delete(baseLine);
                            }
                        }


                        //PMM Sprint Summary.
                        SprintSummaryManager sprintSummaryManager = new SprintSummaryManager(context);
                        List<SprintSummary> pmmSprintSummaryCollection = sprintSummaryManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintSummary).Select( pmmChildListQuery);
                        if (pmmSprintSummaryCollection != null && pmmSprintSummaryCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from Sprint Summary:", history);
                            foreach ( SprintSummary pmmSprintSummaryCollectionItem in  pmmSprintSummaryCollection)
                            {
                                history = string.Format("{0} Sprint Summary Details {1},", history, pmmSprintSummaryCollectionItem.Title);
                                //pmmSprintSummaryCollectionItem.Recycle();
                                sprintSummaryManager.Delete(pmmSprintSummaryCollectionItem);
                            }
                        }

                        //PMM Sprint Task.
                        SprintTaskManager sprintTaskManager = new SprintTaskManager(context);
                        List<SprintTasks> pmmSprintTaskCollection = sprintTaskManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintTasks).Select(pmmChildListQuery);
                        if (pmmSprintTaskCollection != null && pmmSprintTaskCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from Sprint Task:", history);
                            foreach (SprintTasks pmmSprintTaskCollectionItem in pmmSprintTaskCollection)
                            {
                                history = string.Format("{0} Sprint Task Details {1},", history, pmmSprintTaskCollectionItem.Title);
                                sprintTaskManager.Delete(pmmSprintTaskCollectionItem);
                                tasksDeleted = true;
                            }
                        }

                        //PMM Sprint.
                        SprintManager sprintManager = new SprintManager(context);
                        List<Sprint> pmmSprintCollection = sprintManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.SprintSummary).Select( pmmChildListQuery);
                        if (pmmSprintCollection != null && pmmSprintCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from Sprint:", history);
                            foreach ( Sprint pmmSprintCollectionItem in pmmSprintCollection)
                            {
                                history = string.Format("{0} Sprint Details {1},", history, pmmSprintCollectionItem.Title);
                                sprintManager.Delete(pmmSprintCollectionItem);
                            }
                        }
                                                

                        //PMM Release.
                        PMMReleaseManager pMMReleaseManager = new PMMReleaseManager(context);
                        List<ProjectReleases> pmmReleaseCollection = pMMReleaseManager.Load(x => x.PMMIdLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases).Select( pmmChildListQuery);
                        if (pmmReleaseCollection != null && pmmReleaseCollection.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from Release:", history);
                            foreach (ProjectReleases pmmReleaseCollectionItem in pmmReleaseCollection)
                            {
                                history = string.Format("{0} Release Details {1},", history, pmmReleaseCollectionItem.Title);
                                pMMReleaseManager.Delete(pmmReleaseCollectionItem);
                            }
                        }

                        //TicketHours
                        TicketHoursManager ticketHoursManager = new TicketHoursManager(context);
                        List<ActualHour> actualHours  = ticketHoursManager.Load(x => x.TicketID == ticketID);// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases).Select( pmmChildListQuery);
                        if (actualHours != null && actualHours.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from TicketHours:", history);
                            foreach (ActualHour item in actualHours)
                            {
                                history = string.Format("{0} TicketHours Details {1},", history, item.TicketID);
                                ticketHoursManager.Delete(item);
                            }
                        }

                        //PMMHistory
                        DataTable dtPmmHistory = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMHistory, $"{DatabaseObjects.Columns.TicketId} = '{ticketID}' and {DatabaseObjects.Columns.TicketId} = '{context.TenantID}'", "ID,TicketId,Title", null);
                        if (dtPmmHistory != null && dtPmmHistory.Rows.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from PMMHistory:", history);
                            foreach (DataRow row in dtPmmHistory.Rows)
                            {
                                history = string.Format("{0} PMMHistory Details {1},", history, row[DatabaseObjects.Columns.Title]);
                                GetTableDataManager.ExecuteQuery($"Delete from {DatabaseObjects.Tables.PMMHistory} where {DatabaseObjects.Columns.ID} = {row[DatabaseObjects.Columns.ID]}");
                            }
                        }
                        RMMSummaryHelper.DeleteAllocationsByTasks(context, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.VFM)
                    {
                        // Delete from VendorSOWInvoiceDetails
                        string invoicequery = string.Format("{0}='{1}'", DatabaseObjects.Columns.SOWInvoiceLookup, spItem["ID"]);
                        VendorSOWInvoiceDetailManager vendorSOWInvoiceDetailManager = new VendorSOWInvoiceDetailManager(context);
                        List<VendorSOWInvoiceDetail> invoiceColl = vendorSOWInvoiceDetailManager.Load(x => x.SOWInvoiceLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.VendorSOWInvoiceDetail).Select( invoicequery);
                        if (invoiceColl != null)
                        {
                            foreach (VendorSOWInvoiceDetail invoiceItem in invoiceColl)
                            {
                                //invoiceItem.Recycle();
                                vendorSOWInvoiceDetailManager.Delete(invoiceItem);

                            }
                        }
                    }
                    else if (moduleName == ModuleNames.VPM)
                    {
                        // Delete from VendorSLAPerformance
                        string vpmQuery = string.Format("{0}={1}", DatabaseObjects.Columns.VendorVPMLookup, spItem["ID"]);
                        //vpmQuery.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
                        DataRow[] vpmColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.VendorSLAPerformance, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select( vpmQuery);
                        if (vpmColl != null)
                        {
                            while (vpmColl.Count() > 0)
                            {
                                DataRow vpmItem = vpmColl[0];
                               // vpmItem.Recycle();
                            }
                        }
                    }
                    else if (moduleName == ModuleNames.CMDB)
                    {
                        //if (UGITUtility.IfColumnExists(spItem, DatabaseObjects.Columns.ReplacementAsset_SNLookup))
                        //    spItem[DatabaseObjects.Columns.ReplacementAsset_SNLookup] = DBNull.Value;

                        ApplicationServersManager appServersManager = new ApplicationServersManager(context);
                        List<ApplicationServer> applicationServers = appServersManager.Load(x => x.AssetsTitleLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));
                        if (applicationServers.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from ApplicationServers:", history);
                            foreach (ApplicationServer applicationServer in applicationServers)
                            {
                                history = string.Format("{0} Sprint Details {1},", history, applicationServer.Title);
                                appServersManager.Delete(applicationServer);
                            }
                        }
                    }
                    else if (moduleName == ModuleNames.SVC)
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> svcTaskCollection = taskManager.Load(x => x.TicketId == ticketID);
                        UGITModule uModule = null;

                        if (svcTaskCollection != null)
                        {
                            history = string.Format("{0}Deleted Following items from SVC Tasks List:",history);
                            foreach (UGITTask SvcItem in svcTaskCollection)
                            {
                                if (SvcItem.RelatedTicketID != null && SvcItem.Behaviour == Constants.TaskType.Ticket)
                                {
                                    DataRow dr = Ticket.GetCurrentTicket(context, SvcItem.RelatedModule, SvcItem.RelatedTicketID);
                                    if (dr != null)
                                    {   
                                        //dr.RowState = DataRowState.Added;                                        
                                        uModule = ObjModuleViewManager.GetByName(SvcItem.RelatedModule);
                                        //dr.SetAdded();
                                        //ObjTicketManager.SaveArchive(uModule, dr);
                                        //dr.SetModified();
                                        ObjTicketManager.Delete(uModule, dr); 
                                    }
                                }                                

                                history = string.Format("{0} Task Details {1},",history,SvcItem.Title);
                                taskManager.Delete(SvcItem);
                               
                            }
                        }
                    }
                    else if (moduleName == ModuleNames.APP)
                    {
                        //ApplicationModule.
                        ApplicationModuleManager applicationModuleManager = new ApplicationModuleManager(context);
                        List<ApplicationModule> applicationModules = applicationModuleManager.Load(x => x.APPTitleLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectReleases).Select( pmmChildListQuery);
                        if (applicationModules != null && applicationModules.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from ApplicationModule:", history);
                            foreach (ApplicationModule applicationModule in applicationModules)
                            {
                                history = string.Format("{0} ApplicationModule Details {1},", history, applicationModule.Title);
                                applicationModuleManager.Delete(applicationModule);
                            }
                        }

                        //ApplicationPassword.
                        ApplicationPasswordManager applicationPasswordManager = new ApplicationPasswordManager(context);
                        List<ApplicationPasswordEntity> applicationPasswordEntities = applicationPasswordManager.Load(x => x.APPTitleLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationPassword).Select( pmmChildListQuery);
                        if (applicationPasswordEntities != null && applicationPasswordEntities.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from ApplicationPassword:", history);
                            foreach (ApplicationPasswordEntity applicationPasswordEntity in applicationPasswordEntities)
                            {
                                history = string.Format("{0} ApplicationPassword Details {1},", history, applicationPasswordEntity.APPPasswordTitle);
                                applicationPasswordManager.Delete(applicationPasswordEntity);
                            }
                        }

                        //ApplicationRole.
                        ApplicationRoleManager applicationRoleManager = new ApplicationRoleManager(context);
                        List<ApplicationRole> applicationRoles = applicationRoleManager.Load(x => x.APPTitleLookup == Convert.ToInt64(spItem[DatabaseObjects.Columns.Id]));// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ApplicationPassword).Select( pmmChildListQuery);
                        if (applicationRoles != null && applicationRoles.Count > 0)
                        {
                            history = string.Format(" {0} Deleted following items from ApplicationRole:", history);
                            foreach (ApplicationRole applicationRole in applicationRoles)
                            {
                                history = string.Format("{0} ApplicationRole Details {1},", history, applicationRole.Title);
                                applicationRoleManager.Delete(applicationRole);
                            }
                        }
                    }
                    else if(moduleName == ModuleNames.COM)
                    {                        
                        RelatedCompanyManager relatedCompanyManager = new RelatedCompanyManager(context);
                        List<RelatedCompany> relatedCompanies = relatedCompanyManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (relatedCompanies != null && relatedCompanies.Count() > 0)
                        {
                            UGITModule moduleObj = ObjModuleViewManager.GetByName(ModuleNames.COM);
                            ModuleFormLayout formLayout = moduleObj.List_FormLayout.FirstOrDefault(x => x.FieldDisplayName == "Related Companies");
                            ModuleFormTab formTab = moduleObj.List_FormTab.FirstOrDefault(x => x.TabId == formLayout.TabId);

                            history = string.Format("{0} Deleted following items from {1} List:", history, formTab.TabName);

                            foreach (var item in relatedCompanies)
                            {
                                history = string.Format("{0} {2} Details {1},", history, item.Title, formTab.TabName);
                                relatedCompanyManager.Delete(item);
                            }
                        }         
                    }
                    else if (moduleName == ModuleNames.CON)
                    {
                        CRMActivitiesManager crmActivitiesManager = new CRMActivitiesManager(context);
                        List<CRMActivities> crmActivities = crmActivitiesManager.Load(x => x.ContactLookup == Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]));
                        if (crmActivities != null && crmActivities.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  CRMActivities List:", history);
                            foreach (var item in crmActivities)
                            {
                                history = string.Format("{0} CRMActivities Details {1},", history, item.Title);
                                crmActivitiesManager.Delete(item);
                            }
                        }
                    }
                    else if (moduleName == ModuleNames.LEM)
                    {                     
                        ModuleStageConstraintsManager moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);
                        List<ModuleStageConstraints> constraints = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (constraints != null && constraints.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  Module Stage Constraints List:", history);
                            foreach (var item in constraints)
                            {
                                history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                                moduleStageConstraintsManager.Delete(item);
                            }
                        }

                        // LEM Tasks
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> tasks = taskManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");                       
                        if (tasks != null && tasks.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  LEM Tasks List:", history);
                            foreach (var item in tasks)
                            {
                                history = string.Format("{0} Task Details {1},", history, item.Title);
                                taskManager.Delete(item);
                            }                            
                        }
                        RMMSummaryHelper.DeleteAllocationsByTasks(context, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.OPM)
                    {
                        ModuleStageConstraintsManager moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);
                        List<ModuleStageConstraints> constraints = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (constraints != null && constraints.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  Module Stage Constraints List:", history);
                            foreach (var item in constraints)
                            {
                                history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                                moduleStageConstraintsManager.Delete(item);
                            }
                        }

                        //ProjectEstimatedAllocationManager ProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                        //List<ProjectEstimatedAllocation> allocations = ProjectAllocationManager.Load($"TicketID = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        //if (allocations != null && allocations.Count() > 0)
                        //{
                        //    history = string.Format(" {0} Deleted following items from OPM Allocation List:", history);
                        //    foreach (var item in allocations)
                        //    {
                        //        history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                        //        ProjectAllocationManager.Delete(item);
                        //    }
                        //}

                        // OPM Tasks
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> tasks = taskManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (tasks != null && tasks.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  OPM Tasks List:", history);
                            foreach (var item in tasks)
                            {
                                history = string.Format("{0} Task Details {1},", history, item.Title);
                                taskManager.Delete(item);
                            }
                        }
                        RMMSummaryHelper.DeleteAllocationsByTasks(context, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.CPR)
                    {
                        ModuleStageConstraintsManager moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);
                        List<ModuleStageConstraints> constraints = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (constraints != null && constraints.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  Module Stage Constraints List:", history);
                            foreach (var item in constraints)
                            {
                                history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                                moduleStageConstraintsManager.Delete(item);
                            }
                        }

                        //ProjectEstimatedAllocationManager ProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                        //List<ProjectEstimatedAllocation> allocations = ProjectAllocationManager.Load($"TicketID = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        //if (allocations != null && allocations.Count() > 0)
                        //{
                        //    history = string.Format(" {0} Deleted following items from CPR Allocation List:", history);
                        //    foreach (var item in allocations)
                        //    {
                        //        history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                        //        ProjectAllocationManager.Delete(item);
                        //    }
                        //}                                               

                        // CPR Tasks
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> tasks = taskManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (tasks != null && tasks.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  CPR Tasks List:", history);
                            foreach (var item in tasks)
                            {
                                history = string.Format("{0} Task Details {1},", history, item.Title);
                                taskManager.Delete(item);
                            }
                        }
                        RMMSummaryHelper.DeleteAllocationsByTasks(context, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }
                    else if (moduleName == ModuleNames.CNS)
                    {
                        ModuleStageConstraintsManager moduleStageConstraintsManager = new ModuleStageConstraintsManager(context);
                        List<ModuleStageConstraints> constraints = moduleStageConstraintsManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (constraints != null && constraints.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  Module Stage Constraints List:", history);
                            foreach (var item in constraints)
                            {
                                history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                                moduleStageConstraintsManager.Delete(item);
                            }
                        }

                        // ProjectAllocationManager = new ProjectEstimatedAllocationManager(context);
                        //List<ProjectEstimatedAllocation> allocations = ProjectAllocationManager.Load($"TicketID = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        //if (allocations != null && allocations.Count() > 0)
                        //{ProjectEstimatedAllocationManager
                        //    history = string.Format(" {0} Deleted following items from CNS Allocation List:", history);
                        //    foreach (var item in allocations)
                        //    {
                        //        history = string.Format("{0} Module Stage Constraints Details {1},", history, item.Title);
                        //        ProjectAllocationManager.Delete(item);
                        //    }
                        //}

                        // CNS Tasks
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> tasks = taskManager.Load($"{DatabaseObjects.Columns.TicketId} = '{spItem[DatabaseObjects.Columns.TicketId]}'");
                        if (tasks != null && tasks.Count() > 0)
                        {
                            history = string.Format(" {0} Deleted following items from  CNS Tasks List:", history);
                            foreach (var item in tasks)
                            {
                                history = string.Format("{0} Task Details {1},", history, item.Title);
                                taskManager.Delete(item);
                            }
                        }
                        
                        RMMSummaryHelper.DeleteAllocationsByTasks(context, Convert.ToString(spItem[DatabaseObjects.Columns.TicketId]), true);
                    }

                    DeleteDocuments(ticketID);

                    // Copying in Ticket archive list and deleting from ticket list
                    string listName = string.Format("{0}_Archive", module.ModuleTable);
                    //DataTable spList1 = GetTableDataManager.GetTableData(listName);
                    DataTable spList = GetTableDataManager.GetTableStructure(listName);                    
                    if (spList != null)
                    {
                        DataRow archiveListItem = spList.NewRow();
                        spList.Rows.Add(archiveListItem);
                        history = string.Format(" {0} Deleted Item => Id: {1}, Title: {2} from List: {3}.", history, spItem[DatabaseObjects.Columns.TicketId], spItem[DatabaseObjects.Columns.Title], spItem.Table.TableName);
                        archiveListItem[DatabaseObjects.Columns.History] = string.Format("{0} {1}", spItem[DatabaseObjects.Columns.History], history);
                        Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Deleted Item => Id: {spItem[DatabaseObjects.Columns.TicketId]}, Title: {spItem[DatabaseObjects.Columns.Title]}, from Table: {module.ModuleTable}.", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Module), context.TenantID, moduleName, ticketID);
                        CopySpItem(spItem, archiveListItem);
                        //spItem.Recycle();
                    }

                    if (TicketIds.Contains(Convert.ToString(ticketID)))
                    {
                        TicketIds.Remove(Convert.ToString(ticketID));
                    }

                    //Log.AuditTrail(SPContext.Current.Web.CurrentUser, "deleted item " + ticketID);
                }

                if (tasksDeleted)
                {
                    //TaskCache.CleanProjectTasksCache(moduleName, ticketID);
                }
                //uGITCache.ModuleDataCache.DeleteTicketFromCache(moduleID, ticketID);
            }

            moduleUserStatisticsManager.RefreshCache();

            if (TicketIds != null && TicketIds.Count > 0)
            {
                Object[] arrObj = new Object[TicketIds.Count];
                int cnt = 0;
                foreach (string item in TicketIds)
                {
                    arrObj[cnt] = item;
                    cnt++;
                }
                hdnTicketIds.Set("SelectedTicketIds", arrObj);
            }
            else
            {
                hdnTicketIds.Set("SelectedTicketIds", null);
            }

           // uGITCache.ReloadModuleUserStatistics();

            divDetails.Visible = false;
            divList.Visible = false;

            divconfirmMsg.Visible = true;
        }

        private void DeleteDocuments(string ticketId)
        {
            if (!string.IsNullOrEmpty(ticketId))
            {
                ticketId = ticketId.Replace("-", "_");

                var directory = repositoryService.GetUserRepoDirectory(context.CurrentUser.Id, ticketId);
                if (directory != null)
                    repositoryService.DeleteDirectoryRecursive(directory.DirectoryId);
            }
        }

        private void GetChildTickets(string ticketId, TreeNode parentNode)
        {
            string query= string.Format("{0}='{1}'", DatabaseObjects.Columns.ParentTicketId, ticketId);
            DataRow[] spColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TicketRelation, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select(query);
            if (spColl != null && spColl.Count() > 0)
            {
                foreach (DataRow item in spColl)
                {
                    if (TicketIds != null && TicketIds.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId])) && !TicketIds.Contains(Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId])))
                    {
                        DataRow dr = dtTickets.NewRow();
                        dr[DatabaseObjects.Columns.ParentTicketId] = ticketId;
                        dr[DatabaseObjects.Columns.ChildTicketId] = Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]);
                        dtTickets.Rows.Add(dr);
                        string[] ticketInfo = Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]).Split('-');
                        DataRow ticketItem = Ticket.GetCurrentTicket(context, ticketInfo[0], Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]));
                        if (ticketItem != null)
                        {
                            TreeNode node = new TreeNode(string.Format("{0}: {1}", Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]), Convert.ToString(ticketItem[DatabaseObjects.Columns.Title])), Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]));
                            parentNode.ChildNodes.Add(node);
                            node.Checked = true;
                            GetChildTickets(Convert.ToString(item[DatabaseObjects.Columns.ChildTicketId]), node);
                        }
                    }
                }
            }
        }

        private void CopySpItem(DataRow sourceItem, DataRow destinationItem)
        {
            foreach (DataColumn field in sourceItem.Table.Columns)
            {
                if (field.ColumnName != "Attachments" && field.ColumnName != "History")
                {
                    if (UGITUtility.IsSPItemExist(sourceItem, field.ColumnName) && UGITUtility.IfColumnExists(field.ColumnName, destinationItem.Table))
                    {
                        destinationItem[field.ColumnName] = sourceItem[field.ColumnName];
                    }
                }                
            }
            UGITModule module = ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(Convert.ToString(sourceItem[DatabaseObjects.Columns.TicketId])));
            
            ObjTicketManager.SaveArchive(module, destinationItem);
            ObjTicketManager.Delete(module, sourceItem);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
    }
}
