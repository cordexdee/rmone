
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager.Reports;
using System.Linq;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.DxReport
{
    public partial class TSKProjectReport_SchedulerFilter : ReportScheduleFilterBase
    {
        TicketStatus ticketStatus { get; set; }
        bool isSchedule = false;
        bool IsEdit = false;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        FunctionalAreasManager functionalAreasManager;
        RequestTypeManager requestTypeManager;
        ProjectClassViewManager projectClassViewManager;
        public long Id { get; set; }
        protected override void OnInit(EventArgs e)
        {
            if (Request["Schedule"] != null)
            {
                isSchedule = Convert.ToBoolean(Request["Schedule"]);
                IsEdit = Convert.ToBoolean(Request["Edit"]);
            }

            lstProjectClassLHS_Load();
            lstFunctionAreaLHS_Load();
            cblProject_Load();

            chkStatus.Checked = true;
            chkKeyReceivables.Checked = false;
            chkKeyDeliverables.Checked = false;
            chkShowMilestone.Checked = true;
            chkShowAllTasks.Checked = false;
            chkGanttChart.Checked = true;

            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
                FillForm();
            }
            base.OnInit(e);
        }

        private void FillForm()
        {
            Dictionary<string, object> formdic = uHelper.ReportScheduleDict;
            if (formdic.Count > 0 && formdic.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(formdic[ReportScheduleConstant.Report]) == TypeOfReport.TSKProjectReport.ToString())
            {
                int[] functionAreas = Array.ConvertAll<string, int>(Convert.ToString(formdic[ReportScheduleConstant.FunctionAreas]).Split(','), int.Parse);
                foreach (int id in functionAreas)
                {
                    ListEditItem item = lstFunctionAreaLHS.Items.FindByValue(id);
                    if (item != null)
                    {
                        lstFunctionAreaLHS.Items.RemoveAt(item.Index);
                        lstFunctionAreaRHS.Items.Add(item);
                    }
                }

                if (formdic.ContainsKey(ReportScheduleConstant.ProjectType))
                {
                    int[] projectType = Array.ConvertAll<string, int>(Convert.ToString(formdic[ReportScheduleConstant.ProjectType]).Split(','), int.Parse);
                    foreach (int id in projectType)
                    {
                        ListEditItem item = lstProjTypeLHS.Items.FindByValue(id);
                        if (item != null)
                        {
                            lstProjTypeLHS.Items.RemoveAt(item.Index);
                            lstProjTypeRHS.Items.Add(item);
                        }
                    }
                }

                TicketStatus ticketStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formdic[ReportScheduleConstant.TicketStatus]));
                switch (ticketStatus)
                {
                    case TicketStatus.All:
                        rbAll.Checked = true;
                        break;
                    case TicketStatus.Open:
                        rbOpen.Checked = true;
                        break;
                    case TicketStatus.Closed:
                        rbClose.Checked = true;
                        break;
                    default:
                        break;
                }

                chkStatus.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowStatus]);
                chkProjectRoles.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowProjectRoles]);
                chkProjectDescription.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowProjectDescription]);
                chkKeyReceivables.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowReceivable]);
                chkKeyDeliverables.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowDeliverable]);
                chkShowMilestone.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowMilestone]);
                chkShowAllTasks.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowAllTask]);
                chkGanttChart.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowSummaryGanttChart]);

                int[] projects = Array.ConvertAll<string, int>(Convert.ToString(formdic[ReportScheduleConstant.ids]).Split(','), int.Parse);

                cblProject_Load();
                foreach (int id in projects)
                {
                    ListEditItem item = lstProjectsLHS.Items.FindByValue(id);
                    if (item != null)
                    {
                        lstProjectsLHS.Items.RemoveAt(item.Index);
                        lstProjectsRHS.Items.Add(item);
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            lblMsg.Text = "";
            base.OnLoad(e);
        }

        protected void cblProject_Load()
        {
            lstProjectsLHS.Items.Clear();
            string openTicketQuery = string.Empty;
            
            if (rbOpen.Checked)
            {
                openTicketQuery = string.Format("({0} <> 'True' OR {0} is null) AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbClose.Checked)
            {
                openTicketQuery = string.Format("{0}='True' AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbAll.Checked)
            {
                openTicketQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID);
            }

            DataRow[] dataRow = null;
            DataTable _DataTable = null;
            dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, openTicketQuery).Select();
            if (dataRow != null && dataRow.Length > 0)
            {
                _DataTable = dataRow.CopyToDataTable();
                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in _DataTable.Rows)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                    }
                }
            }
        }
        private void DisableEnableButtonControl(bool addButton, bool addAllButton, bool removeButton, bool removeAllButton, UGITControlType controltype)
        {
            switch (controltype)
            {
                case UGITControlType.ProjectType:
                    btnMoveAllItemsToLeftProjType.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftProjType.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightProjType.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftProjType.ClientEnabled = addButton;
                    break;
                case UGITControlType.FunctionArea:
                    btnMoveAllItemsToLeftFuncArea.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftFuncArea.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightFuncArea.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftFuncArea.ClientEnabled = addButton;
                    break;
                case UGITControlType.Projects:
                    btnMoveAllItemsToLeftProj.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftProj.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightProj.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftProj.ClientEnabled = addButton;
                    break;
                default:
                    break;
            }

        }

        private void MoveSelectedItem(ASPxListBox lstFrom, ASPxListBox lstTo)
        {
            if (lstFrom.SelectedItems.Count > 0)
            {
                while (lstFrom.SelectedItems.Count > 0)
                {
                    ListEditItem item = lstFrom.SelectedItems[0];
                    lstTo.Items.Add(item.Text, item.Value);
                    lstFrom.Items.RemoveAt(item.Index);
                }
            }

        }

        private void MoveAllItem(ASPxListBox lstFrom, ASPxListBox lstTo)
        {
            if (lstFrom.Items.Count > 0)
            {
                while (lstFrom.Items.Count > 0)
                {
                    ListEditItem item = lstFrom.Items[0];
                    lstTo.Items.Add(item.Text, item.Value);
                    lstFrom.Items.RemoveAt(item.Index);
                }
            }

        }

        private void FillProjects()
        {
            List<string> projectType = new List<string>();
            List<string> functionAreas = new List<string>();
            List<int> projects = new List<int>();

            foreach (ListEditItem item in lstProjTypeRHS.Items)
            {
                projectType.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
            {
                functionAreas.Add(Convert.ToString(item.Value));
            }

            string spQuery = string.Empty;
            List<string> QueryList = new List<string>();
            foreach (var item in projectType)
            {
                QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.ProjectClassLookup, item));
            }
            string pcquery = string.Empty;
            if (QueryList.Count > 0)
            {
                pcquery = "(" + string.Join(" OR ", QueryList) + ")";
            }
            //uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false);

            QueryList = new List<string>();
            foreach (var item in functionAreas)
            {
                QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.FunctionalAreaLookup, item));
            }
            string faquery = string.Empty;
            if (QueryList.Count > 0)
            {
                faquery = "(" + string.Join(" OR ", QueryList) + ")";
            }
            // string faquery = uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false);
            QueryList = new List<string>();

            if (!string.IsNullOrEmpty(pcquery))
                QueryList.Add(pcquery);
            if (!string.IsNullOrEmpty(faquery))
                QueryList.Add(faquery);

            if (rbOpen.Checked)
            {
                QueryList.Add(string.Format("({0} <> 'True' OR {0} is null) AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID));
            }
            else if (rbClose.Checked)
            {
                QueryList.Add(string.Format("{0}='True' AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID));
            }
            else if (rbAll.Checked)
            {
                QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));
            }

            if (QueryList.Count > 0)
            {
                spQuery = string.Join(" And ", QueryList);
            }
            
            lstProjectsLHS.Items.Clear();
            DataRow[] spListItemColl = null;
            spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, spQuery).Select();
            if (spListItemColl != null && spListItemColl.Length > 0)
            {
                DataTable dt = spListItemColl.CopyToDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    if (lstProjectsRHS.Items.FindByValue(Convert.ToString(dr[DatabaseObjects.Columns.Id])) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.Id]));
                    }
                }
            }

            DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);
        }

        protected void lstProjectClassLHS_Load()
        {
            lstProjTypeLHS.Items.Clear();
            projectClassViewManager = new ProjectClassViewManager(_context);
            List<ProjectClass> listProjectClass = projectClassViewManager.Load(x => !x.Deleted);
            if (listProjectClass != null && listProjectClass.Count > 0)
            {
                lstProjTypeLHS.DataSource = listProjectClass;
                lstProjTypeLHS.TextField = DatabaseObjects.Columns.Title;
                lstProjTypeLHS.ValueField = DatabaseObjects.Columns.ID;
                lstProjTypeLHS.DataBind();
            }
        }

        protected void lstProjectType_Load()
        {
            lstProjTypeLHS.Items.Clear();
            requestTypeManager = new RequestTypeManager(_context);
            List<ModuleRequestType> lstModuleRequestType = requestTypeManager.Load(x => x.ModuleNameLookup == "TSK" && !x.Deleted);
            if (lstModuleRequestType != null && lstModuleRequestType.Count > 0)
            {
                lstProjTypeLHS.DataSource = lstModuleRequestType;
                lstProjTypeLHS.TextField = DatabaseObjects.Columns.Title;
                lstProjTypeLHS.ValueField = DatabaseObjects.Columns.ID;
                lstProjTypeLHS.DataBind();
            }
        }

        protected void lstFunctionAreaLHS_Load()
        {
            lstFunctionAreaLHS.Items.Clear();
            functionalAreasManager = new FunctionalAreasManager(_context);
            List<FunctionalArea> functionalArea = functionalAreasManager.Load(x => x.Deleted == false).ToList();
            if (functionalArea != null && functionalArea.Count > 0)
            {
                lstFunctionAreaLHS.DataSource = functionalArea;
                lstFunctionAreaLHS.TextField = DatabaseObjects.Columns.Title;
                lstFunctionAreaLHS.ValueField = DatabaseObjects.Columns.ID;
                lstFunctionAreaLHS.DataBind();
            }
        }

        protected void btnMoveAllItemsToRightProjType_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjTypeLHS, lstProjTypeRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.ProjectType);
            FillProjects();
        }

        protected void btnMoveSelectedItemsToLeftProjType_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjTypeRHS, lstProjTypeLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.ProjectType);
            FillProjects();
        }

        protected void btnMoveSelectedItemsToRightProjType_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjTypeLHS, lstProjTypeRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.ProjectType);
            FillProjects();
        }

        protected void btnMoveAllItemsToLeftProjType_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjTypeRHS, lstProjTypeLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.ProjectType);
            FillProjects();
        }

        protected void btnMoveSelectedItemsToRightFuncArea_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstFunctionAreaLHS, lstFunctionAreaRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.FunctionArea);
            FillProjects();
        }

        protected void btnMoveAllItemsToRightFuncArea_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstFunctionAreaLHS, lstFunctionAreaRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.FunctionArea);
            FillProjects();
        }

        protected void btnMoveSelectedItemsToLeftFuncArea_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstFunctionAreaRHS, lstFunctionAreaLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.FunctionArea);
            FillProjects();
        }

        protected void btnMoveAllItemsToLeftFuncArea_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstFunctionAreaRHS, lstFunctionAreaLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.FunctionArea);
            FillProjects();
        }

        protected void btnMoveSelectedItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
        }

        protected void btnMoveSelectedItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsRHS, lstProjectsLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
        }

        protected void btnMoveAllItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsRHS, lstProjectsLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.Projects);
        }

        protected void btnMoveAllItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.Projects);
        }

        protected void rbOpen_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            switch (rb.ID)
            {
                case "rbOpen":
                    ticketStatus = TicketStatus.Open;
                    break;
                case "rbClose":
                    ticketStatus = TicketStatus.Closed;
                    break;
                case "rbAll":
                    ticketStatus = TicketStatus.All;
                    break;
                default:
                    break;
            }
            FillProjects();

        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.TSKProjectReport);
            formdic.Add(ReportScheduleConstant.TicketStatus, (rbOpen.Checked ? TicketStatus.Open :
                                        (rbClose.Checked ? TicketStatus.Closed :
                                        (rbAll.Checked ? TicketStatus.All : TicketStatus.Open))));

            List<string> ids = new List<string>();
            foreach (ListEditItem item in lstProjectsRHS.Items)
                ids.Add(Convert.ToString(item.Value));
            formdic.Add(ReportScheduleConstant.ids, string.Join(",", ids.ToArray()));

            ids = new List<string>();
            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
                ids.Add(Convert.ToString(item.Value));
            formdic.Add(ReportScheduleConstant.FunctionAreas, string.Join(",", ids.ToArray()));

            ids = new List<string>();
            foreach (ListEditItem item in lstProjTypeRHS.Items)
                ids.Add(Convert.ToString(item.Value));
            formdic.Add(ReportScheduleConstant.ProjectType, string.Join(",", ids.ToArray()));

            formdic.Add(ReportScheduleConstant.ShowStatus, chkStatus.Checked);
            formdic.Add(ReportScheduleConstant.ShowAllTask, chkShowAllTasks.Checked);
            formdic.Add(ReportScheduleConstant.ShowMilestone, chkShowMilestone.Checked);
            formdic.Add(ReportScheduleConstant.ShowDeliverable, chkKeyDeliverables.Checked);
            formdic.Add(ReportScheduleConstant.ShowReceivable, chkKeyReceivables.Checked);
            formdic.Add(ReportScheduleConstant.ShowProjectDescription, chkProjectDescription.Checked);
            formdic.Add(ReportScheduleConstant.ShowProjectRoles, chkProjectRoles.Checked);
            formdic.Add(ReportScheduleConstant.ShowSummaryGanttChart, chkGanttChart.Checked);
            Filterproperties = formdic;
            uHelper.ReportScheduleDict = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
