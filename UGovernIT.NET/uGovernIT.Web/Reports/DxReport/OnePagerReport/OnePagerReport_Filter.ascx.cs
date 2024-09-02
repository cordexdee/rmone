using System;
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
using uGovernIT.Manager.Managers;

namespace uGovernIT.DxReport
{

    public partial class OnePagerReport_Filter : UserControl
    {
        #region Global variables
        TicketStatus ticketStatus { get; set; }
        TicketPriority ticketPriority { get; set; }
        bool isSchedule = false;
        bool IsEdit { get; set; }
        DataTable dtRequestType;
        DataTable _DataTable;
        UGITModule module = null;
        public string departmentLabel;
        #endregion
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        TicketManager ticketManager = null;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        protected override void OnInit(EventArgs e)
        {
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            module = moduleManager.GetByName(ModuleNames.PMM);
            if (Request["Schedule"] != null)
            {
                isSchedule = Convert.ToBoolean(Request["Schedule"]);
                IsEdit = Convert.ToBoolean(Request["Edit"]);
            }
          //  dtRequestType = uHelper.GetRequestTypeByModuleName("PMM");

            FillPriority();
            Department_Load();
            Functionalarea_Load();
            RequestType_Load();
            FillProjectInitiatives();
            FillProjectClass();
            cblProject_Load();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_DataTable == null)
                _DataTable = ticketManager.GetOpenTickets(module);
            if (!IsPostBack && Request["alltickets"] != null)
            {
                string selectExpress = Request["alltickets"];
                DataRow[] filterColl = _DataTable.Select(string.Format("TicketId IN ({0})", string.Join(",", selectExpress.Split(',').Select(x => string.Format("\'{0}\'", x)).ToList())));
                if (filterColl != null && filterColl.Length > 0)
                {
                    foreach (DataRow row in filterColl)
                    {
                        int index = lstProjectsLHS.Items.IndexOfValue(UGITUtility.StringToInt(row[DatabaseObjects.Columns.ID]));
                        if (index > 0)
                        {
                            ListEditItem item = lstProjectsLHS.Items.ElementAt(index);
                            if (item == null)
                                continue;

                            lstProjectsLHS.Items.Remove(item);
                            lstProjectsRHS.Items.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]), Convert.ToString(row[DatabaseObjects.Columns.Id]));
                        }

                    }
                    DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);
                }
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        //Build Report 
        protected void lnkBuild_Click(object sender, EventArgs e)
        {
            if (lstProjectsRHS.Items.Count == 0)
            {
                lblMsg.Text = "Please select at least one Project.";
                return;
            }
            DateTime from, to;
            List<string> projectClass = new List<string>();
            List<string> projectFunction = new List<string>();
            List<string> projects = new List<string>();
            if (dtcfrom.Date != null)
            {
                from = dtcfrom.Date;
            }
            if (dtcto.Date != null)
            {
                to = dtcto.Date;
            }


            foreach (ListEditItem item in lstProjectsRHS.Items)
            {
                projects.Add(Convert.ToString(item.Value));
            }

            string PMMids = (projects != null && projects.Count > 0) ? string.Join(",", projects.ToArray()) : "0";
            int projectYear = DateTime.Now.Year;

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

            Dictionary<int, string> dcSortOrder = new Dictionary<int, string>();
            List<int> tempOrder = new List<int>();
            dcSortOrder.Add(UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue), DatabaseObjects.Columns.TicketPriorityLookup);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue), DatabaseObjects.Columns.TicketStatus);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProjectRank.SelectedValue), DatabaseObjects.Columns.ProjectRank);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue), DatabaseObjects.Columns.Title);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue), DatabaseObjects.Columns.TicketDesiredCompletionDate);
            List<string> order = dcSortOrder.OrderBy(x => x.Key).Select(x => x.Value).ToList();

            string url = delegateControl + "?reportName=OnePagerReport&isdlg=1";
            url = url + string.Format("&PMMIds={0}", PMMids);
            url = url + string.Format("&TicketStatus={0}", ticketStatus);
            url = url + string.Format("&projectYear={0}", projectYear);
            url = url + string.Format("&SortOrder={0}", string.Join(",", order.ToArray()));
            Response.Redirect(url);
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
            if (rbOpen.Checked)
            {
                _DataTable = ticketManager.GetOpenTickets(module);
                //openTicketQuery = string.Format("({0} <> 'True' OR {0} is null) AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbClose.Checked)
            {
                _DataTable = ticketManager.GetClosedTickets(module);
                //openTicketQuery = string.Format("{0}='True' AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbAll.Checked)
            {
                _DataTable = ticketManager.GetAllTickets(module);
                //openTicketQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID);
            }

            ///Priority
            if (Convert.ToInt32(rblistPriority.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                        DatabaseObjects.Columns.TicketPriorityLookup, rblistPriority.Value));
            }

            ///Department
            if (Convert.ToInt32(cbDepartment.Value) > 0)
            {
                QueryList.Add(string.Format("{0}='{1}'",
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
           
            //QueryList = new List<string>();

            //if (!string.IsNullOrEmpty(faquery))
            //    QueryList.Add(faquery);

            //if (rbOpen.Checked)
            //{
            //    QueryList.Add(string.Format("({0} <> 'True' OR {0} is null) AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID));
            //}
            //else if (rbClose.Checked)
            //{
            //    QueryList.Add(string.Format("{0}='True' AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID));
            //}
            //else if (rbAll.Checked)
            //{
            //    QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));
            //}

            //if (QueryList.Count > 0)
            //    spQuery = "(" + string.Join(" And ", QueryList) + ")"; 

            lstProjectsLHS.Items.Clear();

            var spListItemColl = _DataTable.Select(faquery); //GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, spQuery).Select();
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
            if (lstFrom.ID == "lstProjectsLHS")
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
            else
                lstFrom.Items.Clear();
        }
        private void SwapDropDownValue(DropDownList ddl1, DropDownList ddl2, int remSum)
        {
            int tempValue = Math.Abs( 15 - remSum - UGITUtility.StringToInt(ddl2.SelectedValue) );
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
        private void FillPriority()
        {
            PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(_context);
            List<ModulePrioirty> listTicketPriority = prioirtyViewManager.Load(x => x.ModuleNameLookup.Equals("PMM")).OrderBy(x => x.Title).ToList();
            //DataTable dtpriority = uHelper.GetTicketPriorityByModuleName("PMM");
            if (listTicketPriority != null && listTicketPriority.Count > 0)
            {
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
        private void Functionalarea_Load()
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(_context);
            List<FunctionalArea> listFunctionalArea = null;
            if (Convert.ToInt32(cbDepartment.Value) > 0)
            {
                listFunctionalArea = functionalAreasManager.Load(x => Convert.ToString(x.DepartmentLookup).Equals(cbDepartment.Value)).OrderBy(x => x.Title).ToList();

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
                _DataTable = ticketManager.GetOpenTickets(module);
                //openTicketQuery = string.Format("({0} <> 'True' OR {0} is null) AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbClose.Checked)
            {
                _DataTable = ticketManager.GetClosedTickets(module);
                //openTicketQuery = string.Format("{0}='True' AND {1}='{2}'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, _context.TenantID);
            }
            else if (rbAll.Checked)
            {
                _DataTable = ticketManager.GetAllTickets(module);
                //openTicketQuery = string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID);
            }

            //DataTable _DataTable = new DataTable();
            //DataRow[] dataRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, openTicketQuery).Select();
            lstProjectsRHS.Items.Clear();
            if (_DataTable != null && _DataTable.Rows.Count > 0)
            {
                _DataTable = uHelper.SortCollection(_DataTable, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTable.Rows)
                {
                    lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                }
            }
        }

        protected void ddlProjectSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrioritySortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProjectSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectSortOrder, remSum);
            }

        }

        protected void ddlPrioritySortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProgressSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlTargetDateSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }
            else if (ddlProjectRank.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectRank, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue));
            }
        }

        protected void ddlProjectRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectRank, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectRank, remSum);
            }
        }

        protected void ddlProgressSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProgressSortOrder, remSum);
            }
        }

        protected void ddlTargetDateSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlTargetDateSortOrder, remSum);
            }
        }
    }
}
