
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
    public partial class TaskSummary_Filter : ReportScheduleFilterBase
    {
        public string ModuleName { get; set; }
        TicketStatus ticketStatus { get; set; }
        bool IsEdit = false;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        FunctionalAreasManager functionalAreasManager;
        RequestTypeManager requestTypeManager;
        ProjectClassViewManager projectClassViewManager;
        public long Id { get; set; }
        protected override void OnInit(EventArgs e)
        {
            if (Request["Module"] != null)
            {
                ModuleName = Request["Module"];
            }

            if (ModuleName == "PMM")
            {
                lstProjectType_Load();
                lblProjectClass.Text = "Project Type";
            }
            else if (ModuleName == "TSK")
            {
                lstProjectClassLHS_Load();
                lblProjectClass.Text = "Class";
                lblHeader.Text = "Select Task List(s):";
                lblProjects.Text = "Task Lists";
            }
            else
            {
                liBuild.Visible = false;
                lstProjectType_Load();
                lblProjectClass.Text = "Project Type";
            }

            lstFunctionAreaLHS_Load();
            cblProject_Load();
            if (Request["Edit"] != null)
            {
                IsEdit = Convert.ToBoolean(Request["Edit"]);
            }
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
            base.OnInit(e);
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
                openTicketQuery = string.Format("And {0} <> '{1}' ", DatabaseObjects.Columns.TicketClosed, "true");
            }
            else if (rbClose.Checked)
            {
                openTicketQuery = string.Format("And {0}='{1}'", DatabaseObjects.Columns.TicketClosed,"true");
            }
            else if (rbAll.Checked)
            {
                openTicketQuery = "";
            }
            
            DataTable _DataTable = null;
            if (ModuleName == "PMM")
            _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"TenantID='{_context.TenantID}'" + openTicketQuery);
            else if (ModuleName == "TSK")
                _DataTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"TenantID='{_context.TenantID}'" + openTicketQuery);

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
            List<string> projectClass = new List<string>();
            List<string> projectFunction = new List<string>();
            List<string> projects = new List<string>();

            foreach (ListEditItem item in lstProjTypeRHS.Items)
            {
                projectClass.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
            {
                projectFunction.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstProjectsRHS.Items)
            {
                projects.Add(Convert.ToString(item.Value));
            }

            string PMMids = (projects != null && projects.Count > 0) ? string.Join(",", projects.ToArray()) : "0";
            if (rbOpen.Checked)
            {
                ticketStatus = TicketStatus.Open;
            }
            else if (rbClose.Checked)
            {
                ticketStatus = TicketStatus.Closed;
            }
            else if (rbAll.Checked)
            {
                ticketStatus = TicketStatus.All;
            }
            string url = string.Format("~/Reports/DxReport/TaskSummary/taskSummary.aspx?isdlg=1&moduleName={0}", ModuleName);
            url = url + string.Format("&Ids={0}", PMMids);
            url = url + string.Format("&ProjectStatus={0}", ticketStatus.ToString());

            Response.Redirect(UGITUtility.GetAbsoluteURL(url));
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
            List<string> projectClass = new List<string>();
            List<string> functionAreas = new List<string>();
            List<int> projects = new List<int>();
            foreach (ListEditItem item in lstProjTypeRHS.Items)
            {
                projectClass.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
            {
                functionAreas.Add(Convert.ToString(item.Value));
            }

            string spQuery = string.Empty;

            List<string> QueryList = new List<string>();
            foreach (var item in projectClass)
            {
                if (ModuleName == "PMM")
                {
                    QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketRequestTypeLookup, item));
                }
                else if (ModuleName == "TSK")
                {
                    QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.ProjectClassLookup, item));
                }
            }
            string pcquery = string.Empty;
            if (QueryList.Count > 0)
            {
                pcquery = string.Format("({0})", UGITUtility.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false));
            }

            QueryList = new List<string>();
            foreach (var item in functionAreas)
            {
                QueryList.Add(string.Format("{0}= '{1}'", DatabaseObjects.Columns.FunctionalAreaLookup, item));
            }
            string faquery = string.Empty;
            if (QueryList.Count > 0)
            {
                faquery = string.Format("({0})", UGITUtility.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false));
            }

            QueryList = new List<string>();
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
            DataTable spListItemColl = null;

            if (ModuleName == "PMM")
            {
                spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects,spQuery);
            }
            else if (ModuleName == "TSK")
            {
                spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects,spQuery);
            }

            lstProjectsRHS.Items.Clear();
            lstProjectsLHS.TextField = DatabaseObjects.Columns.Title;
            lstProjectsLHS.ValueField = DatabaseObjects.Columns.ID;
            lstProjectsLHS.DataSource = spListItemColl;
            lstProjectsLHS.DataBind();
            //Disable action buttons
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
            //List<ModuleRequestType> lstModuleRequestType = requestTypeManager.Load(x => x.ModuleNameLookup == "PMM" && !x.IsDeleted.HasValue);
            List<ModuleRequestType> lstModuleRequestType = requestTypeManager.Load(x => x.ModuleNameLookup == "PMM" && x.Deleted == false);
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
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }

}
