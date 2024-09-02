
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
namespace uGovernIT.Report.DxReport
{
    public partial class TSKProjectReport_Filter : UserControl
    {
        TicketStatus ticketStatus { get; set; }
        bool isSchedule = false;
        bool IsEdit = false;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        FunctionalAreasManager functionalAreasManager;
        RequestTypeManager requestTypeManager;
        ProjectClassViewManager projectClassViewManager;
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");
        public string ReportFilterURl;
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
            if (IsEdit)
            {
                FillForm();
            }

            base.OnInit(e);
        }

        private void FillForm()
        {
            Dictionary<string, object> formdic = uHelper.ReportScheduleDict;
            if (formdic.Count > 0)
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
                openTicketQuery = string.Format("{0} <> 'True' OR {0} is null", DatabaseObjects.Columns.TicketClosed);
            }
            else if (rbClose.Checked)
            {
                openTicketQuery = string.Format("{0}='TRUE'", DatabaseObjects.Columns.TicketClosed);
            }
            else if (rbAll.Checked)
            {
                openTicketQuery = "";
            }

            // openTicketQuery.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/><FieldRef Name='{2}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title);
            //   openTicketQuery.ViewFieldsOnly = true;
            //DataRow[] dataRow = null;
            DataTable _DataTable = null;
            if (!string.IsNullOrEmpty(openTicketQuery))
            {
                _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"TenantID='{_context.TenantID}' and " + openTicketQuery);
            }
            else
            {
                _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, $"TenantID='{_context.TenantID}'");
            }
            lstProjectsRHS.Items.Clear();
            if (_DataTable != null && _DataTable.Rows.Count > 0)
            {
                lstProjectsLHS.TextField = DatabaseObjects.Columns.Title;
                lstProjectsLHS.ValueField = DatabaseObjects.Columns.ID;
                lstProjectsLHS.DataSource = _DataTable;
                lstProjectsLHS.DataBind();
            }
        }

        protected void lnkBuild_Click(object sender, EventArgs e)
        {
            if (lstProjectsRHS.Items.Count == 0)
            {
                lblMsg.Text = "Please select at least one Project.";
                return;
            }
            List<string> projectType = new List<string>();
            List<string> projectFunction = new List<string>();
            List<string> tasks = new List<string>();

            foreach (ListEditItem item in lstProjTypeRHS.Items)
            {
                projectType.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
            {
                projectFunction.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstProjectsRHS.Items)
            {
                tasks.Add(Convert.ToString(item.Value));
            }

            string tskids = (tasks != null && tasks.Count > 0) ? string.Join(",", tasks.ToArray()) : "0";
            int projectYear = DateTime.Now.Year;
            string url = delegateControl + "?reportName=TSKProjectReport";


            //string url = "/_layouts/15/ugovernit/delegatecontrol.aspx?control=tskprojectreport";
            url = url + string.Format("&TSKIds={0}", tskids);
            url = url + string.Format("&projectYear={0}", projectYear);
            url = url + string.Format("&Status={0}", chkStatus.Checked);
            url = url + string.Format("&SGC={0}", chkGanttChart.Checked);
            url = url + string.Format("&SAT={0}", chkShowAllTasks.Checked);
            url = url + string.Format("&SMS={0}", chkShowMilestone.Checked);
            url = url + string.Format("&SKD={0}", chkKeyDeliverables.Checked);
            url = url + string.Format("&SKR={0}", chkKeyReceivables.Checked);
            url = url + string.Format("&ProjectRoles={0}", chkProjectRoles.Checked);
            url = url + string.Format("&ProjectDesc={0}", chkProjectDescription.Checked);
            Response.Redirect((url));
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
            if(QueryList.Count>0)
                pcquery=string.Format("({0})",  UGITUtility.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false));
            QueryList = new List<string>();
            foreach (var item in functionAreas)
            {
                QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.FunctionalAreaLookup, item));
            }

            string faquery = string.Empty;
            if(QueryList.Count>0) 
               faquery= UGITUtility.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false);

            if (!string.IsNullOrEmpty(pcquery))
                QueryList.Add(pcquery);
            if (!string.IsNullOrEmpty(faquery))
                QueryList.Add(faquery);

            if (rbOpen.Checked)
            {
                QueryList.Add(string.Format("{0}<>'True'", DatabaseObjects.Columns.TicketClosed));
            }
            else if (rbClose.Checked)
            {
                QueryList.Add(string.Format("{0}='True'", DatabaseObjects.Columns.TicketClosed));
            }

            QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));

            spQuery = UGITUtility.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, true);

            lstProjectsLHS.Items.Clear();
            DataTable _DataTable = null;
            _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects,  spQuery);
            if (_DataTable != null && _DataTable.Rows.Count > 0)
            {
                lstProjectsLHS.TextField = DatabaseObjects.Columns.Title;
                lstProjectsLHS.ValueField = DatabaseObjects.Columns.ID;
                lstProjectsLHS.DataSource = _DataTable;
                lstProjectsLHS.DataBind();
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
            MoveAllItem(lstProjTypeLHS, lstProjTypeRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.ProjectType);
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

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);

        }
    }
}