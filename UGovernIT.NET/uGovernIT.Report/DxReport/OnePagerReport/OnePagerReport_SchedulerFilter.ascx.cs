﻿using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using DevExpress.Web;
using System.Text;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Report.Intrastructure;

namespace uGovernIT.Report.DxReport
{

    public partial class OnePagerReport_SchedulerFilter : ReportScheduleFilterBase
    {
        #region Global variables
        TicketStatus ticketStatus { get; set; }
        TicketPriority ticketPriority { get; set; }
        bool isSchedule = false;
        bool IsEdit { get; set; }
        long Id { get; set; }
        DataTable dtRequestType;
        public string departmentLabel;
        #endregion
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");
        protected override void OnInit(EventArgs e)
        {
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            if (Request["Schedule"] != null)
            {
                isSchedule = Convert.ToBoolean(Request["Schedule"]);
                IsEdit = Convert.ToBoolean(Request["Edit"]);
            }
            FillPriority();
            Department_Load();
            Functionalarea_Load(Convert.ToString(cbDepartment.Value));
            RequestType_Load();
            FillProjectInitiatives();
            FillProjectClass();
            cblProject_Load();
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
                FillForm();
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //Schedule Report 
        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.OnePagerReport);
            formdic.Add(ReportScheduleConstant.TicketStatus, (rbOpen.Checked ? TicketStatus.Open :
                                        (rbClose.Checked ? TicketStatus.Closed :
                                        (rbAll.Checked ? TicketStatus.All : TicketStatus.Open))));

            formdic.Add(ReportScheduleConstant.TicketPriority, rblistPriority.Value);
            formdic.Add(ReportScheduleConstant.Department, cbDepartment.Value);
            formdic.Add(ReportScheduleConstant.FunctionAreas, cbFunctionalarea.Value);
            formdic.Add(ReportScheduleConstant.Category, cbCategory.Value);
            formdic.Add(ReportScheduleConstant.SubCategory, cbSubCategory.Value);
            formdic.Add(ReportScheduleConstant.RequestType, cbRequesttype.Value);
            if (cbProjectInitiative.Value == null)
                formdic.Add(ReportScheduleConstant.ProjectInitiative, "");
            else
                formdic.Add(ReportScheduleConstant.ProjectInitiative, cbProjectInitiative.Value);
            if (cbProjectClass.Value == null)
                formdic.Add(ReportScheduleConstant.ProjectClass, "");
            else
                formdic.Add(ReportScheduleConstant.ProjectClass, cbProjectClass.Value);
            List<string> ids = new List<string>();
            foreach (ListEditItem item in lstProjectsRHS.Items)
                ids.Add(Convert.ToString(item.Value));
            formdic.Add(ReportScheduleConstant.ids, string.Join(",", ids.ToArray()));
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
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
        protected void TicketPriority_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            switch (rb.ID)
            {
                case "rbHigh":
                    ticketPriority = TicketPriority.High;
                    break;
                case "rbMedium":
                    ticketPriority = TicketPriority.Medium;
                    break;
                case "rbLow":
                    ticketPriority = TicketPriority.Low;
                    break;
                case "rbtpAll":
                    ticketPriority = TicketPriority.All;
                    break;
                default:
                    break;
            }
            FillProjects();
        }

        protected void cbFunctionalarea_Callback(object sender, CallbackEventArgsBase e)
        {
            FillFunctionalArea(e.Parameter);
        }
        private void FillFunctionalArea(string department)
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(_context);

            List<FunctionalArea> listFunctionalArea = null;

            if (Convert.ToInt32(department) > 0)
            {
                listFunctionalArea = functionalAreasManager.Load(x => Convert.ToString(x.DepartmentLookup).Equals(department)).OrderBy(x => x.Title).ToList();
            }
            if (listFunctionalArea != null && listFunctionalArea.Count() > 0)
            {
                cbFunctionalarea.ValueField = DatabaseObjects.Columns.ID;
                cbFunctionalarea.TextField = DatabaseObjects.Columns.Title;
                cbFunctionalarea.DataSource = listFunctionalArea;
                cbFunctionalarea.DataBind();
                cbFunctionalarea.Items.Insert(0, new ListEditItem("--All Functional Areas--", 0));
                cbFunctionalarea.SelectedIndex = 0;
            }
            else
            {
                cbFunctionalarea.Items.Clear();
                cbFunctionalarea.Items.Insert(0, new ListEditItem("--All Functional Areas--", 0));
                cbFunctionalarea.SelectedIndex = 0;
            }
        }
        protected void cbSubCategory_Callback(object sender, CallbackEventArgsBase e)
        {
            FillSubCategory(e.Parameter);
        }
        private void FillSubCategory(string category)
        {
            DataTable dtSubCategory = new DataTable();
            if (!string.IsNullOrEmpty(category) && category != "0")
            {
                var tempdtSubCategory = dtRequestType.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Category) == category &&
                                                                                x.Field<string>(DatabaseObjects.Columns.SubCategory) != string.Empty)
                                                                                .CopyToDataTable();

                dtSubCategory = tempdtSubCategory.DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory);
            }
            else
            {
                dtSubCategory = dtRequestType.DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory);
            }

            cbSubCategory.DataSource = dtSubCategory;
            cbSubCategory.DataBind();
            cbSubCategory.Items.Insert(0, new ListEditItem("--All Sub Categories--", "0"));
            cbSubCategory.SelectedIndex = 0;
        }

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            FillProjects();
        }

        private void FillProjects()
        {
            List<int> projects = new List<int>();
            string spQuery = string.Empty;

            List<string> QueryList = new List<string>();

            ///Priority
            if (Convert.ToInt32(rblistPriority.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                        DatabaseObjects.Columns.TicketPriorityLookup, rblistPriority.Value));
            }

            ///Department
            if (Convert.ToInt32(cbDepartment.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                        DatabaseObjects.Columns.TicketBeneficiaries, cbDepartment.Value));
            }

            ///Functional area
            if (Convert.ToInt32(cbFunctionalarea.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                       DatabaseObjects.Columns.FunctionalAreaLookup, cbFunctionalarea.Value));
            }

            ///Request Type
            if (Convert.ToInt32(cbRequesttype.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                      DatabaseObjects.Columns.TicketRequestTypeLookup, cbRequesttype.Value));
            }

            ///Project Initiatives
            if (Convert.ToInt32(cbProjectInitiative.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                       DatabaseObjects.Columns.ProjectInitiativeLookup, cbProjectInitiative.Value));
            }

            ///Project Class
            if (Convert.ToInt32(cbProjectClass.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                      DatabaseObjects.Columns.ProjectClassLookup, cbProjectClass.Value));
            }
            string faquery = string.Empty;
            if (QueryList.Count > 0)
            {
                faquery = "(" + string.Join(" And ", QueryList) + ")";
            }
            //  string faquery = "(" + string.Join(" And ", QueryList) + ")"; //uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, true);
            QueryList = new List<string>();

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

            spQuery = "(" + string.Join(" And ", QueryList) + ")"; 
            lstProjectsLHS.Items.Clear();

            var spListItemColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, spQuery).Select();
            if (spListItemColl != null && spListItemColl.Count() > 0)
            {
                DataTable dt = spListItemColl.CopyToDataTable();
                dt = uHelper.SortCollection(dt, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in dt.Rows)
                {
                    if (lstProjectsRHS.Items.FindByValue(Convert.ToString(dr[DatabaseObjects.Columns.ID])) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                    }
                }
            }
            DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);
        }

        private void DisableEnableButtonControl(bool addButton, bool addAllButton, bool removeButton, bool removeAllButton, UGITControlType controltype)
        {
            btnMoveAllItemsToLeftProj.ClientEnabled = removeAllButton;
            btnMoveSelectedItemsToLeftProj.ClientEnabled = removeButton;
            btnMoveAllItemsToRightProj.ClientEnabled = addAllButton;
            btnMoveSelectedItemsToLeftProj.ClientEnabled = addButton;
        }

        protected void lstProjectsLHS_Callback(object sender, CallbackEventArgsBase e)
        {
            FillProjects();
        }
        protected void btnMoveSelectedItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
        }
        protected void btnMoveAllItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.Projects);
        }
        protected void btnMoveSelectedItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsRHS, lstProjectsLHS);
            List<ListEditItem> items = lstProjectsLHS.Items.Cast<ListEditItem>().OrderBy(x => x.Text).ToList();
            lstProjectsLHS.Items.Clear();
            foreach (ListEditItem item in items)
            {
                lstProjectsLHS.Items.Add(item);
            }
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
        }
        protected void btnMoveAllItemsToLeftProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsRHS, lstProjectsLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.Projects);
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
        private void SwapDropDownValue(DropDownList ddl1, DropDownList ddl2, int remSum)
        {
            int tempValue = Math.Abs( 10 - remSum - UGITUtility.StringToInt(ddl2.SelectedValue));
            ddl2.SelectedValue = ddl1.SelectedValue;
            ddl1.SelectedValue = tempValue.ToString();
        }

        protected void cbRequesttype_Callback(object sender, CallbackEventArgsBase e)
        {
            FillRequestType(e.Parameter);
        }

        private void FillRequestType(string parameter)
        {
            string[] value = parameter.Split('|');
            DataTable dtrequest = null;
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(value[0]) && value[0] != "0")
            {
                sb.AppendFormat("{0}='{1}' ", DatabaseObjects.Columns.Category, value[0]);
            }

            if (!string.IsNullOrEmpty(value[1]) && value[1] != "0")
            {
                sb.AppendFormat("And {0}='{1}' ", DatabaseObjects.Columns.SubCategory, value[1]);
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                var dataRows = dtRequestType.Select(sb.ToString());
                if (dataRows != null && dataRows.Count() > 0)
                {
                    dtrequest = dataRows.CopyToDataTable();
                }
            }
            else
            {
                dtrequest = dtRequestType.Copy();
            }
            cbRequesttype.Items.Clear();
            cbRequesttype.DataSource = dtrequest;
            cbRequesttype.DataBind();
            cbRequesttype.Items.Insert(0, new ListEditItem("--All Project Types--", "0"));
            cbRequesttype.SelectedIndex = 0;
        }

        private void FillForm()
        {
            if (Id > 0)
            {
                Dictionary<string, object> formdic = LoadFilter(Id);
                if (formdic!=null && formdic.Count > 0)
                {
                    if (formdic.ContainsKey(ReportScheduleConstant.FunctionAreas) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.FunctionAreas]).Trim()))
                    {
                        int functionalarea = Convert.ToInt32(formdic[ReportScheduleConstant.FunctionAreas]);
                        Functionalarea_Load(Convert.ToString(formdic[ReportScheduleConstant.Department]));
                        cbFunctionalarea.SelectedIndex = cbFunctionalarea.Items.IndexOfValue(functionalarea);
                    }
                    if (formdic.ContainsKey(ReportScheduleConstant.Department) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.Department]).Trim()))
                    {
                        int department = Convert.ToInt32(formdic[ReportScheduleConstant.Department]);
                        cbDepartment.SelectedIndex = cbDepartment.Items.IndexOfValue(department);
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.TicketPriority) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.TicketPriority]).Trim()))
                    {
                        int ticketpriority = Convert.ToInt32(formdic[ReportScheduleConstant.TicketPriority]);
                        //rblistPriority.SelectedIndex = rblistPriority.Items.IndexOfValue(ticketpriority);
                        rblistPriority.Items.FindByValue(Convert.ToString(ticketpriority)).Selected = true;
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.Category) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.Category]).Trim()))
                    {
                        string category = Convert.ToString(formdic[ReportScheduleConstant.Category]);
                        cbCategory.SelectedIndex = cbCategory.Items.IndexOfValue(category);
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.SubCategory) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.SubCategory]).Trim()))
                    {
                        string subcategory = Convert.ToString(formdic[ReportScheduleConstant.SubCategory]);
                        cbSubCategory.SelectedIndex = cbSubCategory.Items.IndexOfValue(subcategory);
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.RequestType) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.RequestType]).Trim()))
                    {
                        int requesttype = Convert.ToInt32(formdic[ReportScheduleConstant.RequestType]);
                        cbRequesttype.SelectedIndex = cbRequesttype.Items.IndexOfValue(requesttype);
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.ProjectInitiative) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ProjectInitiative]).Trim()))
                    {
                        int projectinitiative = Convert.ToInt32(formdic[ReportScheduleConstant.ProjectInitiative]);
                        cbProjectInitiative.SelectedIndex = cbProjectInitiative.Items.IndexOfValue(projectinitiative);
                    }

                    if (formdic.ContainsKey(ReportScheduleConstant.ProjectClass) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ProjectClass]).Trim()))
                    {
                        int projectclass = Convert.ToInt32(formdic[ReportScheduleConstant.ProjectClass]);
                        cbProjectClass.SelectedIndex = cbProjectClass.Items.IndexOfValue(projectclass);
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
                    int[] projects = Array.ConvertAll<string, int>(Convert.ToString(formdic[ReportScheduleConstant.ids]).Split(','), int.Parse);
                    FillProjects();
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
        }
        private void FillPriority()
        {
            PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(_context);
            List<ModulePrioirty> listTicketPriority = prioirtyViewManager.Load(x => x.ModuleNameLookup.Equals("PMM")).OrderBy(x => x.Title).ToList();
            //DataTable dtpriority = uHelper.GetTicketPriorityByModuleName("PMM");
            if (listTicketPriority != null && listTicketPriority.Count > 0)
            {
                //dtpriority =uHelper.SortCollection(dtpriority, DatabaseObjects.Columns.Title);
                rblistPriority.ValueField = DatabaseObjects.Columns.ID;
                rblistPriority.TextField = DatabaseObjects.Columns.Title;
                rblistPriority.DataSource = listTicketPriority;
                rblistPriority.DataBind();
                rblistPriority.Items.Insert(0, new ListEditItem("All", 0));
                rblistPriority.SelectedIndex = 0;
            }
        }
        private void Department_Load()
        {
            DepartmentManager departmentManager = new DepartmentManager(_context);
            List<Department> lstDepartment = departmentManager.Load().OrderBy(x => x.Title).ToList();
            // DataTable dtDepartment = SPListHelper.GetDataTable(DatabaseObjects.Lists.Department);
            if (lstDepartment != null && lstDepartment.Count > 0)
            {
                cbDepartment.ValueField = DatabaseObjects.Columns.ID;
                cbDepartment.TextField = DatabaseObjects.Columns.Title;
                cbDepartment.DataSource = lstDepartment;
                cbDepartment.DataBind();
                cbDepartment.Items.Insert(0, new ListEditItem("--All Departments--", 0));
                cbDepartment.SelectedIndex = 0;
            }
        }
        private void RequestType_Load()
        {
            RequestTypeManager requestTypeManager = new RequestTypeManager(_context);
            DataRow[] dataRowRequetType = null;
            dtRequestType = requestTypeManager.GetDataTable();
            if (dtRequestType != null && dtRequestType.Rows.Count > 0)
            {
                dataRowRequetType = dtRequestType.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, "PMM"));
                if (dataRowRequetType != null && dataRowRequetType.Length > 0)
                    dtRequestType = dataRowRequetType.CopyToDataTable();
            }
            DataTable dtCategory = dtRequestType.DefaultView.ToTable(true, DatabaseObjects.Columns.Category);

            DataTable dtSubCategory = new DataTable();
            if (!string.IsNullOrEmpty(Convert.ToString(cbCategory.Value)))
            {
                string categoryValue = Convert.ToString(cbCategory.Value);
                var tempdtSubCategory = dtRequestType.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Category) == categoryValue).CopyToDataTable();
                dtSubCategory = tempdtSubCategory.DefaultView.ToTable(true, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.SubCategory);
            }
            else
            {
                dtSubCategory = dtRequestType.DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(cbCategory.Value)) && !string.IsNullOrEmpty(Convert.ToString(cbSubCategory.Value)))
            {
                string categoryValue = Convert.ToString(cbCategory.Value);
                string subcategoryValue = Convert.ToString(cbSubCategory.Value);
                var tempdtSubCategory = dtRequestType.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Category) == categoryValue &&
                                                                                x.Field<string>(DatabaseObjects.Columns.SubCategory) == subcategoryValue)
                                                                                .CopyToDataTable();
                dtRequestType = tempdtSubCategory.Copy();
            }

            if (dtCategory != null && dtCategory.Rows.Count > 0)
            {
                dtCategory = uHelper.SortCollection(dtCategory, DatabaseObjects.Columns.Category);
                cbCategory.Items.Clear();
                cbCategory.ValueField = DatabaseObjects.Columns.Category;
                cbCategory.TextField = DatabaseObjects.Columns.Category;
                cbCategory.DataSource = dtCategory;
                cbCategory.DataBind();
                cbCategory.Items.Insert(0, new ListEditItem("--All Categories--", "0"));
                cbCategory.SelectedIndex = 0;
            }

            if (dtSubCategory != null && dtSubCategory.Rows.Count > 0)
            {
                dtSubCategory = uHelper.SortCollection(dtSubCategory, DatabaseObjects.Columns.SubCategory);
                cbSubCategory.Items.Clear();
                cbSubCategory.ValueField = DatabaseObjects.Columns.SubCategory;
                cbSubCategory.TextField = DatabaseObjects.Columns.SubCategory;
                cbSubCategory.DataSource = dtSubCategory;
                cbSubCategory.DataBind();
                cbSubCategory.Items.Insert(0, new ListEditItem("--All Sub Categories--", "0"));
                cbSubCategory.SelectedIndex = 0;
            }

            if (dtRequestType != null && dtRequestType.Rows.Count > 0)
            {
                dtRequestType = uHelper.SortCollection(dtRequestType, DatabaseObjects.Columns.TicketRequestType);
                cbRequesttype.Items.Clear();
                cbRequesttype.ValueField = DatabaseObjects.Columns.Id;
                cbRequesttype.TextField = DatabaseObjects.Columns.TicketRequestType;
                cbRequesttype.DataSource = dtRequestType;
                cbRequesttype.DataBind();
                cbRequesttype.Items.Insert(0, new ListEditItem("--All Project Types--", 0));
                cbRequesttype.SelectedIndex = 0;
            }
        }
        private void Functionalarea_Load(string department)
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(_context);
            List<FunctionalArea> listFunctionalArea = null;
            if (Convert.ToInt32(department) > 0)
            {
                listFunctionalArea = functionalAreasManager.Load(x => Convert.ToString(x.DepartmentLookup).Equals(department)).OrderBy(x => x.Title).ToList();

            }
            if (listFunctionalArea != null && listFunctionalArea.Count() > 0)
            {
                cbFunctionalarea.ValueField = DatabaseObjects.Columns.ID;
                cbFunctionalarea.TextField = DatabaseObjects.Columns.Title;
                cbFunctionalarea.DataSource = listFunctionalArea;
                cbFunctionalarea.DataBind();
                cbFunctionalarea.Items.Insert(0, new ListEditItem("--All Functional Areas--", 0));
                cbFunctionalarea.SelectedIndex = 0;
            }
            else
            {
                cbFunctionalarea.Items.Clear();
            }
        }
        private void FillProjectInitiatives()
        {
            ProjectInitiativeViewManager projectInitiativeViewManager = new ProjectInitiativeViewManager(_context);
            List<ProjectInitiative> lstProjectInitiative = projectInitiativeViewManager.Load().OrderBy(x => x.Title).ToList();
            if (lstProjectInitiative != null && lstProjectInitiative.Count > 0)
            {
                cbProjectInitiative.ValueField = DatabaseObjects.Columns.ID;
                cbProjectInitiative.TextField = DatabaseObjects.Columns.Title;
                cbProjectInitiative.DataSource = lstProjectInitiative;
                cbProjectInitiative.DataBind();
                cbProjectInitiative.Items.Insert(0, new ListEditItem("--All Project Initiatives--", 0));
                cbProjectInitiative.SelectedIndex = 0;
            }
        }
        private void FillProjectClass()
        {
            ProjectClassViewManager projectClassViewManager = new ProjectClassViewManager(_context);
            List<ProjectClass> lstProjectClass = projectClassViewManager.Load().OrderBy(x => x.Title).ToList();
            //DataTable dtprojectclass = SPListHelper.GetDataTable(DatabaseObjects.Lists.ProjectClass);
            if (lstProjectClass != null && lstProjectClass.Count > 0)
            {

                cbProjectClass.ValueField = DatabaseObjects.Columns.ID;
                cbProjectClass.TextField = DatabaseObjects.Columns.Title;
                cbProjectClass.DataSource = lstProjectClass;
                cbProjectClass.DataBind();
                cbProjectClass.Items.Insert(0, new ListEditItem("--All Project Classes--", 0));
                cbProjectClass.SelectedIndex = 0;
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

            DataTable _DataTable = new DataTable();
            DataRow[] dataRow = null;
            dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, openTicketQuery).Select();
            lstProjectsRHS.Items.Clear();
            if (dataRow != null && dataRow.Count() > 0)
            {
                _DataTable = dataRow.CopyToDataTable();
                _DataTable = uHelper.SortCollection(_DataTable, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTable.Rows)
                {
                    lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                }
            }
        }
    }
}