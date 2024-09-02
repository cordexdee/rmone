
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
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using System.Web;
namespace uGovernIT.DxReport
{
    public partial class ProjectReport_Filter : UserControl
    {
        TicketStatus ticketStatus { get; set; }
        public string ModuleName { get; set; }
        bool IsEdit { get; set; }
        DataTable dtRequestType;
        public string departmentLabel { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        protected DataTable _DataTable = null;
        ModuleViewManager moduleManager = null;
        TicketManager ticketManager = null;
        UGITModule module = null;
        protected override void OnInit(EventArgs e)
        {
            moduleManager = new ModuleViewManager(_context);
            ticketManager = new TicketManager(_context);
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
            //lstPriority_Load();
            //lstProgress_Load();

            ModuleName = UGITUtility.ObjectToString(Request["Module"]);
            module = moduleManager.LoadByName(ModuleName, true);

            FillPriority();
            FillDepartment();
            FillFunctionalArea(Convert.ToString(cbDepartment.Value));
            RequestType_Load();
            FillProjectInitiatives();
            FillProjectClass();

            cblProject_Load();
            chkProjectName.Checked = true;
            chkPriority.Checked = true;
            chkStatus.Checked = true;
            chkDescription.Checked = true;
            chkTargetDate.Checked = true;
            chkProjectManager.Checked = true;
            chkProgress.Checked = true;
            chkProjectType.Checked = true;
            chkPercentComp.Checked = true;

            chkProStatus.Checked = true;
            chkPlainText.Checked = true;
            chkLatestOnly.Checked = true;

            chkMonitors.Checked = true;
            chkRisk.Checked = true;
            chkAccomplishments.Checked = true;
            chkPlan.Checked = true;
            chkIssues.Checked = true;
            chkTrafficLight.Checked = true;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            lblMsg.Text = "";
            //BTS-21-000594: Error fixing and code optimization
            //ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            if (_DataTable == null || _DataTable.Rows.Count == 0)
            {
                //UGITModule module = moduleManager.LoadByName(ModuleName, true);
                _DataTable = ticketManager.GetOpenTickets(module);
            }
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
                            lstProjectsRHS.Items.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]), row[DatabaseObjects.Columns.ID]);

                        }
                    }
                    DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);

                }
            }
            base.OnLoad(e);
        }

        //protected void lstPriority_Load()
        //{
            
        //    lstPriorityLHS.Items.Clear();
        //    SPList spList = SPListHelper.GetSPList(DatabaseObjects.Tables.TicketPriority);
        //    DataTable dt = spList.Items.GetDataTable();
        //    if (dt != null)
        //    {
        //        DataRow[] dr = dt.Select(string.Format("{0}='{1}' AND ({2}=0 OR {2} IS NULL)", DatabaseObjects.Columns.ModuleNameLookup, "PMM", DatabaseObjects.Columns.IsDeleted));
        //        if (dr != null && dr.Length > 0)
        //        {
        //            foreach (DataRow row in dr)
        //            {
        //                lstPriorityLHS.Items.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]));
        //            }
        //        }
        //    }
        //}

        //protected void lstProgress_Load()
        //{
        //    lstProgressLHS.Items.Clear();
        //    SPList spList = SPListHelper.GetSPList(DatabaseObjects.Lists.ProjectLifeCycleStages);
        //    DataTable dt = spList.Items.GetDataTable();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        //DataRow[] dr = dt.Select(string.Format("{0}='{1}' AND ({2}=0 OR {2} IS NULL)", DatabaseObjects.Columns.ModuleNameLookup, "PMM", DatabaseObjects.Columns.IsDeleted));
        //        //if (dr != null && dr.Length > 0)
        //        //{                 
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            lstProgressLHS.Items.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]));
        //        }
        //        //}
        //    }
        //}

        private void FillPriority()
        {
            PrioirtyViewManager prioirtyViewManager = new PrioirtyViewManager(_context);
            List<ModulePrioirty> listTicketPriority = prioirtyViewManager.Load(x=> x.ModuleNameLookup.Equals(ModuleName)).OrderBy(x=> x.Title).ToList();
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

        private void RequestType_Load()
        {
            RequestTypeManager requestTypeManager = new RequestTypeManager(_context);
            DataRow[] dataRowRequetType = null;
            dtRequestType = requestTypeManager.GetDataTable();
            if (dtRequestType != null && dtRequestType.Rows.Count > 0) {
                dataRowRequetType = dtRequestType.Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.ModuleNameLookup, ModuleName));
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
                                                                                x.Field<string>(DatabaseObjects.Columns.SubCategory) == subcategoryValue);
                if (tempdtSubCategory != null)                                                 
                    dtRequestType = tempdtSubCategory.CopyToDataTable();
            }

            if (dtCategory != null && dtCategory.Rows.Count > 0)
            {
                dtCategory =uHelper.SortCollection(dtCategory,DatabaseObjects.Columns.Category);
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
                dtSubCategory =uHelper.SortCollection(dtSubCategory,DatabaseObjects.Columns.SubCategory);
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
                dtRequestType = uHelper.SortCollection(dtRequestType,DatabaseObjects.Columns.TicketRequestType);
                cbRequesttype.Items.Clear();
                cbRequesttype.ValueField = DatabaseObjects.Columns.Id;
                cbRequesttype.TextField = DatabaseObjects.Columns.TicketRequestType;
                cbRequesttype.DataSource = dtRequestType;
                cbRequesttype.DataBind();
                cbRequesttype.Items.Insert(0, new ListEditItem("--All Project Types--", 0));
                cbRequesttype.SelectedIndex = 0;
            }
        }

        private void FillDepartment()
        {
            DepartmentManager departmentManager = new DepartmentManager(_context);
            List<Department> lstDepartment = departmentManager.Load().OrderBy(x=> x.Title).ToList();
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

        private void FillSubCategory(string category)
        {
            DataTable dtSubCategory = new DataTable();
            if (!string.IsNullOrEmpty(category) && category != "0")
            {
                var tempdtSubCategory = dtRequestType.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.Category) == category &&
                                                                                x.Field<string>(DatabaseObjects.Columns.SubCategory) != string.Empty);
                                                                                

                dtSubCategory = tempdtSubCategory.Any() ? tempdtSubCategory.CopyToDataTable().DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory): null;
            }
            else
            {
                dtSubCategory = dtRequestType.DefaultView.ToTable(true, DatabaseObjects.Columns.SubCategory);
            }

            cbSubCategory.DataSource = dtSubCategory;
            cbSubCategory.DataBind();
            if (dtSubCategory != null)
            {
                cbSubCategory.Items.Insert(0, new ListEditItem("--All Sub Categories--", "0"));
                cbSubCategory.SelectedIndex = 0;
            }
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

        private void FillProjectInitiatives()
        {
            ProjectInitiativeViewManager projectInitiativeViewManager = new ProjectInitiativeViewManager(_context);
            List<ProjectInitiative> lstProjectInitiative = projectInitiativeViewManager.Load().OrderBy(x=> x.Title).ToList();
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
            List<ProjectClass> lstProjectClass = projectClassViewManager.Load().OrderBy(x=>x.Title).ToList();
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

        private void FillFunctionalArea(string department)
        {
            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(_context);

            List<FunctionalArea> listFunctionalArea = null;

            if (Convert.ToInt32(department) > 0)
            {
                listFunctionalArea = functionalAreasManager.Load(x => Convert.ToString(x.DepartmentLookup).Equals(department)).OrderBy(x=> x.Title).ToList();
            }
            if (listFunctionalArea != null && listFunctionalArea.Count > 0)
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

        protected void rbOpen_CheckedChanged(object sender, EventArgs e)
        {
            FillProject();
        }

        #region Unused Code
        protected void btnMoveSelectedItemsToRightPriority_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstPriorityLHS, lstPriorityRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Priority);
            FillProject();
        }

        protected void btnMoveAllItemsToRightPriority_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstPriorityLHS, lstPriorityRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.Priority);
            FillProject();
        }

        protected void btnMoveSelectedItemsToLeftPriority_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstPriorityRHS, lstPriorityLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Priority);
           FillProject();
        }

        protected void btnMoveAllItemsToLeftPriority_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstPriorityRHS, lstPriorityLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.Priority);
            FillProject();
        }


        protected void btnMoveSelectedItemsToRightProgress_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProgressLHS, lstProgressRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Progress);
           FillProject();
        }

        protected void btnMoveAllItemsToRightProgress_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProgressLHS, lstProgressRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.Progress);
           FillProject();
        }

        protected void btnMoveSelectedItemsToLeftProgress_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProgressRHS, lstProgressLHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Progress);
           FillProject();
        }

        protected void btnMoveAllItemsToLeftProgress_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProgressRHS, lstProgressLHS);
            DisableEnableButtonControl(false, true, false, false, UGITControlType.Progress);
           FillProject();
        }
        #endregion

        protected void btnMoveSelectedItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveSelectedItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, true, false, true, UGITControlType.Projects);
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

        protected void btnMoveAllItemsToRightProj_Click(object sender, EventArgs e)
        {
            MoveAllItem(lstProjectsLHS, lstProjectsRHS);
            DisableEnableButtonControl(false, false, false, true, UGITControlType.Projects);
        }

        private void DisableEnableButtonControl(bool addButton, bool addAllButton, bool removeButton, bool removeAllButton, UGITControlType controltype)
        {
            switch (controltype)
            {
                case UGITControlType.Priority:
                    btnMoveAllItemsToLeftPriority.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftPriority.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightPriority.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftPriority.ClientEnabled = addButton;
                    break;
                case UGITControlType.Progress:
                    btnMoveAllItemsToLeftProgress.ClientEnabled = removeAllButton;
                    btnMoveSelectedItemsToLeftProgress.ClientEnabled = removeButton;
                    btnMoveAllItemsToRightProgress.ClientEnabled = addAllButton;
                    btnMoveSelectedItemsToLeftProgress.ClientEnabled = addButton;
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


        protected void lnkBuild_Click(object sender, EventArgs e)
        {
            if (lstProjectsRHS.Items.Count == 0)
            {
                lblMsg.Text = "Please select at least one Project.";
                return;
            }
            List<string> sortOrder = new List<string>();
            List<string> pmmIds = new List<string>();
            foreach (ListEditItem item in lstProjectsRHS.Items)
            {
                pmmIds.Add(Convert.ToString(item.Value));
            }

            Dictionary<int, string> dcSortOrder = new Dictionary<int, string>();
            List<int> tempOrder = new List<int>();

            dcSortOrder.Add(UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue), DatabaseObjects.Columns.TicketPriorityLookup);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue), DatabaseObjects.Columns.TicketStatus);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProjectRank.SelectedValue), DatabaseObjects.Columns.ProjectRank);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue), DatabaseObjects.Columns.Title);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue), DatabaseObjects.Columns.TicketDesiredCompletionDate);
            dcSortOrder.Add(UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue), DatabaseObjects.Columns.ProjectInitiativeLookup);
            List<string> order = dcSortOrder.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            string selectedProjectStatus = "open";
            if (rbClose.Checked)
                selectedProjectStatus = "closed";
            else if (rbAll.Checked)
                selectedProjectStatus = "all";

            string url = delegateControl + "?reportName=ProjectReport&isudlg=1";
            url = url + string.Format("&Module={0}", ModuleName);
            url = url + string.Format("&SelectedProjectStatus={0}", selectedProjectStatus);
            url = url + string.Format("&ProjectName={0}", chkProjectName.Checked);
            url = url + string.Format("&Priority={0}", chkPriority.Checked);
            url = url + string.Format("&Status={0}", chkStatus.Checked);
            url = url + string.Format("&Description={0}", chkDescription.Checked);
            url = url + string.Format("&TargetDate={0}", chkTargetDate.Checked);
            url = url + string.Format("&ProjectManager={0}", chkProjectManager.Checked);
            url = url + string.Format("&Progress={0}", chkProgress.Checked);
            url = url + string.Format("&ProjectType={0}", chkProjectType.Checked);
            url = url + string.Format("&PercentageComp={0}", chkPercentComp.Checked);
            url = url + string.Format("&LatestOnly={0}", chkLatestOnly.Checked);
            url = url + string.Format("&PlainText={0}", chkPlainText.Checked);
            url = url + string.Format("&Acc={0}", chkAccomplishments.Checked);
            url = url + string.Format("&Plan={0}", chkPlan.Checked);
            url = url + string.Format("&ProStatus={0}", chkProStatus.Checked);
            url = url + string.Format("&Issues={0}", chkIssues.Checked);
            url = url + string.Format("&Risk={0}", chkRisk.Checked);
            url = url + string.Format("&Monitors={0}", chkMonitors.Checked);
            url = url + string.Format("&ProjectStatus={0}", ticketStatus.ToString());
            url = url + string.Format("&AccDesc={0}", chkAccDesc.Checked);
            url = url + string.Format("&PlanDesc={0}", chkPlanDesc.Checked);
            url = url + string.Format("&IssDesc={0}", chkIssuesDesc.Checked);
            url = url + string.Format("&RiskDesc={0}", chkRiskDesc.Checked);
            url = url + string.Format("&PMMIds={0}", string.Join(",", pmmIds.ToArray()));
            url = url + string.Format("&SortOrder={0}", string.Join(",", order.ToArray()));
            url = url + string.Format("&Trafficlight={0}", chkTrafficLight.Checked);
            Response.Redirect(url);
        }

        protected void cblProject_Load()
        {
            lstProjectsLHS.Items.Clear();

            //BTS-21-000594: Fetch cache data as per the user's selection.
            if (rbOpen.Checked)
            {
                _DataTable = ticketManager.GetOpenTickets(module);
            }
            else if (rbClose.Checked)
            {
                _DataTable = ticketManager.GetClosedTickets(module);
            }
            else //if (rbAll.Checked)
            {
                _DataTable = ticketManager.GetAllTickets(module);
            }

            DataTable _DataTableFilter = _DataTable;

            lstProjectsRHS.Items.Clear();
            
            if (_DataTableFilter != null && _DataTableFilter.Rows.Count > 0)
            {
                _DataTableFilter = uHelper.SortCollection(_DataTableFilter, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTableFilter.Rows)
                {
                    lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), dr[DatabaseObjects.Columns.ID]);
                }
            }

        }

        private void FillProjects()
        {
            List<string> priorityList = new List<string>();
            List<string> progressList = new List<string>();
            //List<int, Sr> sortOrder = new List<string>();

            foreach (ListEditItem item in lstPriorityRHS.Items)
            {
                priorityList.Add(Convert.ToString(item.Value));
            }

            foreach (ListEditItem item in lstProgressRHS.Items)
            {
                progressList.Add(Convert.ToString(item.Value));
            }


            DataTable dtPMM = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{_context.TenantID}'");

            string status = string.Format("'{0}'", string.Join("','", progressList.ToArray()));
            string priority = string.Format("'{0}'", string.Join("','", priorityList.ToArray()));

            List<string> pmmIds = new List<string>();
            List<string> conditions = new List<string>();

            if (rbOpen.Checked)
            {
                conditions.Add(string.Format("{0} <> '{1}'", DatabaseObjects.Columns.TicketClosed, "True"));
            }
            else if (rbClose.Checked)
            {
                conditions.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketClosed, "True"));
            }


            if (progressList.Count > 0)
            {
                conditions.Add("{0} In ({1})");
            }

            if (priorityList.Count > 0)
            {
                conditions.Add("{2} In ({3})");
            }



            DataRow[] drs = dtPMM.Select(string.Format(string.Join(" And ", conditions.ToArray()), DatabaseObjects.Columns.TicketStatus, status, DatabaseObjects.Columns.TicketPriorityLookup, priority));

            lstProjectsLHS.Items.Clear();

            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    if (lstProjectsRHS.Items.FindByValue(dr[DatabaseObjects.Columns.ID]) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), dr[DatabaseObjects.Columns.ID]);
                    }
                }

                for (int i = 0; i < lstProjectsRHS.Items.Count; i++)
                {
                    int value = UGITUtility.StringToInt(lstProjectsRHS.Items[i].Value);
                    DataRow dr = drs.FirstOrDefault(x => x.Field<int>(DatabaseObjects.Columns.ID) == value);
                    if (dr == null)
                    {
                        lstProjectsRHS.Items.Remove(lstProjectsRHS.Items[i]);
                        i--;
                    }
                }
            }
            else
            {
                lstProjectsRHS.Items.Clear();
            }


            DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);
        }

        private void FillProject()
        {
            List<int> projects = new List<int>();
            string spQuery = string.Empty;

            List<string> QueryList = new List<string>();
            //UGITModule module = moduleManager.LoadByName(ModuleName, true);

            ///Priority
            if (Convert.ToInt32(rblistPriority.Value) > 0)
            {
                QueryList.Add(string.Format("{0}={1}",
                        DatabaseObjects.Columns.TicketPriorityLookup, rblistPriority.Value));
            }

            ///Department
            if (Convert.ToInt32(cbDepartment.Value) > 0)
            {
                //QueryList.Add(string.Format("{0}={1}",
                QueryList.Add(string.Format("{0} like '%{1}%'",
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
            string faquery = string.Empty; //uHelper.GenerateWhereQueryWithAndOr(QueryList, QueryList.Count - 1, true);
            if (QueryList.Count > 0)
            {
                faquery = "(" + string.Join(" AND ", QueryList) + ")";
            }
       
            QueryList = new List<string>();

            if (!string.IsNullOrEmpty(faquery))
                QueryList.Add(faquery);
            //BTS-21-000594: Fetch cache data as per the user's selection.
            if (rbOpen.Checked)
            {
                if (_DataTable == null)
                {
                    _DataTable = ticketManager.GetOpenTickets(module);
                }
                //QueryList.Add(string.Format("{0} <> '{1}'", DatabaseObjects.Columns.TicketClosed,"True"));
            }
            else if (rbClose.Checked)
            {
                _DataTable = ticketManager.GetClosedTickets(module);
                //QueryList.Add(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketClosed, "True"));
            }
            else if(rbAll.Checked)
            {
                _DataTable = ticketManager.GetAllTickets(module);
            }
            if (QueryList.Count > 0)
            {
                spQuery = $" {string.Join(" And ", QueryList)}";
            }
            lstProjectsLHS.Items.Clear();
            
            var filterRows = _DataTable.Select(spQuery);
            DataTable _DataTableFilter = filterRows.Any()? filterRows.CopyToDataTable(): null;

            lstProjectsRHS.Items.Clear();

            if (_DataTableFilter != null && _DataTableFilter.Rows.Count > 0)
            {
                //lstProjectsLHS.DataSource = _DataTable;
                _DataTableFilter = uHelper.SortCollection(_DataTableFilter, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTableFilter.Rows)
                {
                    if (lstProjectsRHS.Items.FindByValue(Convert.ToString(dr[DatabaseObjects.Columns.ID])) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), dr[DatabaseObjects.Columns.ID]);
                    }
                }
            }
            DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void SwapDropDownValue(DropDownList ddl1, DropDownList ddl2, int remSum)
        {
            int tempValue = Math.Abs( 21 - remSum - UGITUtility.StringToInt(ddl2.SelectedValue));
            ddl2.SelectedValue = ddl1.SelectedValue;
            ddl1.SelectedValue = tempValue.ToString();
        }

        protected void ddlPrioritySortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue));
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProgressSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue));
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlTargetDateSortOrder, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue));
            }
            else if (ddlProjectRank.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlProjectRank, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue));
            }
            else if (ddlBusinessInitiative.SelectedValue == ddlPrioritySortOrder.SelectedValue)
            {
                SwapDropDownValue(ddlBusinessInitiative, ddlPrioritySortOrder, UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue));
            }

        }

        protected void ddlProgressSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProgressSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProgressSortOrder, remSum);
            }
            else if (ddlBusinessInitiative.SelectedValue == ddlProgressSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlBusinessInitiative, ddlProgressSortOrder, remSum);
            }
        }

        protected void ddlProjectSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrioritySortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlProjectSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectSortOrder, remSum);
            }
            else if (ddlBusinessInitiative.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlBusinessInitiative, ddlProjectSortOrder, remSum);
            }
        }

        protected void ddlTargetDateSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectRank, ddlTargetDateSortOrder, remSum);
            }
            else if (ddlBusinessInitiative.SelectedValue == ddlTargetDateSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlBusinessInitiative, ddlTargetDateSortOrder, remSum);
            }
        }

        protected void cbprequesttype_Callback(object sender, CallbackEventArgsBase e)
        {
            RequestType_Load();
        }

        protected void cbSubCategory_Callback(object sender, CallbackEventArgsBase e)
        {
            FillSubCategory(e.Parameter);
        }

        protected void cbRequesttype_Callback(object sender, CallbackEventArgsBase e)
        {
            FillRequestType(e.Parameter);
        }

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            FillProject();
        }

        protected void lstProjectsLHS_Callback(object sender, CallbackEventArgsBase e)
        {
            FillProject();
        }

        protected void cbFunctionalarea_Callback(object sender, CallbackEventArgsBase e)
        {
            FillFunctionalArea(e.Parameter);
        }

        protected void ddlBusinessInitiative_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlBusinessInitiative.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlBusinessInitiative, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlBusinessInitiative.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlBusinessInitiative, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlBusinessInitiative.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlBusinessInitiative, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlBusinessInitiative.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlBusinessInitiative, remSum);
            }
            else if (ddlProjectRank.SelectedValue == ddlBusinessInitiative.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlBusinessInitiative, remSum);
            }
        }

        protected void ddlProjectRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProjectSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlProgressSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlProgressSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlPrioritySortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlPrioritySortOrder, ddlProjectRank, remSum);
            }
            else if (ddlTargetDateSortOrder.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlBusinessInitiative.SelectedValue);
                SwapDropDownValue(ddlTargetDateSortOrder, ddlProjectRank, remSum);
            }
            else if (ddlBusinessInitiative.SelectedValue == ddlProjectRank.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlPrioritySortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue);
                SwapDropDownValue(ddlBusinessInitiative, ddlProjectRank, remSum);
            }
        }
    }
}
