
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
using uGovernIT.Report.Intrastructure;

namespace uGovernIT.Report.DxReport
{
    public partial class TaskSummary_SchedulerFilter : ReportScheduleFilterBase
    {
        public string ModuleName { get; set; }
        TicketStatus ticketStatus { get; set; }
        bool IsEdit = false;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        FunctionalAreasManager functionalAreasManager;
        RequestTypeManager requestTypeManager;
        ProjectClassViewManager projectClassViewManager;
        ModuleViewManager moduleViewManager;
        public long Id { get; set; }
        protected override void OnInit(EventArgs e)
        {
            FillModule();

            if (Request["Module"] != null)
            {
                ModuleName = Request["Module"];
            }
            if (string.IsNullOrEmpty(ModuleName))
            {
                trmodule.Visible = true;
                ModuleName = ddlmodule.SelectedValue;
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
                ModuleName = ddlmodule.SelectedValue;
                lnkSubmit.Visible = true;
                lnkBuild.Visible = false;
                trmodule.Visible = true;
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
            FillForm();
            base.OnInit(e);
        }

        private void FillModule()
        {
            moduleViewManager = new ModuleViewManager(_context);
            ddlmodule.Items.Clear();
            ddlmodule.Items.Add(new ListItem("None"));
            UGITModule uGITModule = moduleViewManager.GetByName("PMM");
            ddlmodule.Items.Add(new ListItem(uGITModule.Title, uGITModule.ModuleName));
            uGITModule = moduleViewManager.GetByName("TSK");
            ddlmodule.Items.Add(new ListItem(uGITModule.Title, uGITModule.ModuleName));
        }

        protected override void OnLoad(EventArgs e)
        {

            lblMsg.Text = "";
            if (ddlmodule.SelectedIndex > 0)
            {
                ModuleName = ddlmodule.SelectedValue;
            }
            base.OnLoad(e);
        }

        private void FillForm()
        {
            if (Id > 0)
            {
                Dictionary<string, object> formdic = LoadFilter(Id);
                if (formdic != null && formdic.Count > 0)
                {
                    ddlmodule.SelectedValue = Convert.ToString(formdic[ReportScheduleConstant.Module]);
                    ddlmodule_SelectedIndexChanged(ddlmodule, new EventArgs());

                    TicketStatus tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formdic[ReportScheduleConstant.TicketStatus]));
                    switch (tstatus)
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
                            rbOpen.Checked = true;
                            break;
                    }

                    string[] intArray = Convert.ToString(formdic[ReportScheduleConstant.ProjectType]).Split(',');
                    AddSelectedRecordsToRHS(lstProjTypeLHS, lstProjTypeRHS, intArray, UGITControlType.ProjectType);

                    intArray = Convert.ToString(formdic[ReportScheduleConstant.FunctionAreas]).Split(',');
                    AddSelectedRecordsToRHS(lstFunctionAreaLHS, lstFunctionAreaRHS, intArray, UGITControlType.FunctionArea);

                    intArray = Convert.ToString(formdic[ReportScheduleConstant.Projects]).Split(',');
                    AddSelectedRecordsToRHS(lstProjectsLHS, lstProjectsRHS, intArray, UGITControlType.Projects);
                }
            }
        }

        void AddSelectedRecordsToRHS(ASPxListBox lstFrom, ASPxListBox lstTo, string[] intArray, UGITControlType controlType)
        {
            for (int i = 0; i < intArray.Length; i++)
            {
                ListEditItem item = lstFrom.Items.FindByValue(intArray[i]);
                if (item != null)
                {
                    lstTo.Items.Add(item.Text, item.Value);
                    lstFrom.Items.RemoveAt(item.Index);
                }
            }

            DisableEnableButtonControl(false, (lstFrom.Items.Count > 0), false, (lstTo.Items.Count > 0), controlType);
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

            DataTable _DataTable = null;
            DataRow[] dataRow = null;
            if (ModuleName == "PMM")
                dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, openTicketQuery).Select();
            else if (ModuleName == "TSK")
                dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, openTicketQuery).Select();

            if (dataRow != null && dataRow.Length > 0)
            {
                _DataTable = dataRow.CopyToDataTable();
                if (_DataTable != null)
                {
                    foreach (DataRow dr in _DataTable.Rows)
                    {
                        if (lstProjectsRHS.Items.FindByValue(Convert.ToString(dr[DatabaseObjects.Columns.ID])) == null)
                        {
                            lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                        }
                    }
                }
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
            string url = string.Format("/_layouts/15/ugovernit/taskSummary.aspx?isdlg=1&moduleName={0}", ModuleName);
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
            string pcquery = "(" + string.Join(" OR ", QueryList) + ")"; //uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false);

            QueryList = new List<string>();
            foreach (var item in functionAreas)
            {
                QueryList.Add(string.Format("{0}= '{1}'", DatabaseObjects.Columns.FunctionalAreaLookup, item));
            }

            string faquery = string.Empty;
            if(QueryList.Count() > 0)
                faquery = "(" + string.Join(" OR ", QueryList) + ")"; //uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, false);
            
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

            spQuery = string.Join(" And ", QueryList);
            lstProjectsLHS.Items.Clear();
            DataRow[] spListItemColl = null;

            if (ModuleName == "PMM")
            {
                spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, spQuery).Select();
            }
            else if (ModuleName == "TSK")
            {
                spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.TSKProjects, spQuery).Select();
            }

            lstProjectsRHS.Items.Clear();
            if (spListItemColl != null && spListItemColl.Length > 0)
            {
                DataTable dt = spListItemColl.CopyToDataTable();
                foreach (DataRow dr in dt.Rows)
                {
                    if (lstProjectsRHS.Items.FindByValue(dr[DatabaseObjects.Columns.ID]) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
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
            List<ModuleRequestType> lstModuleRequestType = requestTypeManager.Load(x => x.ModuleNameLookup == "PMM" && !x.Deleted);
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
            //FillProjects();
        }

        protected void btnMoveSelectedItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsRHS, lstProjectsLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
            //FillProjects();
        }

        protected void btnMoveAllItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsRHS, lstProjectsLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.Projects);
            //FillProjects();
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

        protected void ddlmodule_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstProjectsRHS.Items.Clear();
            lstFunctionAreaRHS.Items.Clear();
            lstProjTypeRHS.Items.Clear();


            ModuleName = ddlmodule.SelectedValue;
            if (ModuleName == "PMM")
            {
                lstProjectType_Load();
                lblProjectClass.Text = "Project Type";
            }
            else if (ModuleName == "TSK")
            {
                lstProjectClassLHS_Load();
                lblProjectClass.Text = "Project Class";
            }

            lstFunctionAreaLHS_Load();
            cblProject_Load();
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.TaskSummary);
            formdic.Add(ReportScheduleConstant.Module, ddlmodule.SelectedValue);
            formdic.Add(ReportScheduleConstant.TicketStatus, (rbOpen.Checked ? TicketStatus.Open :
                                                             (rbClose.Checked ? TicketStatus.Closed :
                                                             (rbAll.Checked ? TicketStatus.All : TicketStatus.Open))));

            List<string> projectTypes = new List<string>();
            foreach (ListEditItem item in lstProjTypeRHS.Items)
                projectTypes.Add(Convert.ToString(item.Value));

            List<string> functionAreas = new List<string>();
            foreach (ListEditItem item in lstFunctionAreaRHS.Items)
                functionAreas.Add(Convert.ToString(item.Value));

            List<string> projects = new List<string>();
            foreach (ListEditItem item in lstProjectsRHS.Items)
                projects.Add(Convert.ToString(item.Value));

            formdic.Add(ReportScheduleConstant.ProjectType, string.Join(",", projectTypes.ToArray()));
            formdic.Add(ReportScheduleConstant.FunctionAreas, string.Join(",", functionAreas.ToArray()));
            formdic.Add(ReportScheduleConstant.Projects, string.Join(",", projects.ToArray()));
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }

}
