
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.DxReport
{
    public partial class ProjectStatusReport_Filter : UserControl
    {
        TicketStatus ticketStatus { get; set; }
        TicketPriority ticketPriority { get; set; }
        bool isSchedule = false;
        bool IsEdit { get; set; }
        DataTable dtRequestType;
        public string departmentLabel;
        protected DataTable _DataTable = null;
        TicketManager ticketManager = null;
        ModuleViewManager moduleManager = null;
        UGITModule module = null;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        protected override void OnInit(EventArgs e)
        {
            if (Request["individual"] != null)
            {
                projectside.Visible = false;
                tblprostatusreport.Width = "500px";
            }
            else
                tblprostatusreport.Width = "1300px";

            ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            module = moduleManager.GetByName(ModuleNames.PMM);
            departmentLabel = uHelper.GetDepartmentLabelName(DepartmentLevel.Department);
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


            if (!SetProjectReportDefaults())
            {
                chkStatus.Checked = true;
                chkProjectRoles.Checked = true;
                chkProjectDescription.Checked = true;
                chkBudgetSummary.Checked = true;
                chkPlannedvsActualByCategory.Checked = false;
                chkAccomplishments.Checked = true;
                chkPlan.Checked = true;
                chkIssues.Checked = true;
                chkDecisionLog.Checked = true;
                chkKeyReceivables.Checked = false;
                chkKeyDeliverables.Checked = false;
                chkPlannedvsActualByBudgetItem.Checked = false;
                chkShowMilestone.Checked = true;
                chkShowAllTasks.Checked = false;
                chkPlannedvsActualByMonth.Checked = false;
                chkResourceAllocation.Checked = true;
                chkMilestone.Checked = true;
                chkTrafficLight.Checked = true;
                chkAllMilestone.Checked = false;
            }
            chkCalculate.Text = "Calculate Expected Spend: ";
            if (IsEdit)
            {
              //  FillForm();
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(_DataTable == null)
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

        private bool SetProjectReportDefaults()
        {
            ProjectStatusReport_Scheduler obj = new ProjectStatusReport_Scheduler(_context);
            Dictionary<string, object> dic = obj.GetDefaultData();
            if (dic == null || dic.Count == 0)
                return false;
            foreach (string key in dic.Keys) 
            {
                Control control = FindControl(key);
                if (control == null)
                    continue;
                CheckBox chk = control as CheckBox;
                if (chk != null) 
                    chk.Checked = Convert.ToBoolean(dic[key]);
            }
            return true;
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

        //private void FillForm()
        //{
        //    Dictionary<string, object> formdic = uHelper.ReportScheduleDict;
        //    if (formdic.Count > 0 && formdic.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(formdic[ReportScheduleConstant.Report]) == "ProjectReport")
        //    {
        //        if (formdic.ContainsKey(ReportScheduleConstant.FunctionAreas) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.FunctionAreas]).Trim()))
        //        {
        //            int functionalarea = Convert.ToInt32(formdic[ReportScheduleConstant.FunctionAreas]);
        //            cbFunctionalarea.SelectedIndex = cbFunctionalarea.Items.IndexOfValue(functionalarea);
        //        }
        //        if (formdic.ContainsKey(ReportScheduleConstant.Department) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.Department]).Trim()))
        //        {
        //            int department = Convert.ToInt32(formdic[ReportScheduleConstant.Department]);
        //            cbDepartment.SelectedIndex = cbDepartment.Items.IndexOfValue(department);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.TicketPriority) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.TicketPriority]).Trim()))
        //        {
        //            int ticketpriority = Convert.ToInt32(formdic[ReportScheduleConstant.TicketPriority]);
        //            rblistPriority.SelectedIndex = rblistPriority.Items.IndexOfValue(ticketpriority);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.Category) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.Category]).Trim()))
        //        {
        //            string category = Convert.ToString(formdic[ReportScheduleConstant.Category]);
        //            cbCategory.SelectedIndex = cbCategory.Items.IndexOfValue(category);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.SubCategory) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.SubCategory]).Trim()))
        //        {
        //            string subcategory = Convert.ToString(formdic[ReportScheduleConstant.SubCategory]);
        //            cbSubCategory.SelectedIndex = cbSubCategory.Items.IndexOfValue(subcategory);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.RequestType) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.RequestType]).Trim()))
        //        {
        //            int requesttype = Convert.ToInt32(formdic[ReportScheduleConstant.RequestType]);
        //            cbRequesttype.SelectedIndex = cbRequesttype.Items.IndexOfValue(requesttype);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.ProjectInitiative) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ProjectInitiative]).Trim()))
        //        {
        //            int projectinitiative = Convert.ToInt32(formdic[ReportScheduleConstant.ProjectInitiative]);
        //            cbProjectInitiative.SelectedIndex = cbProjectInitiative.Items.IndexOfValue(projectinitiative);
        //        }

        //        if (formdic.ContainsKey(ReportScheduleConstant.ProjectClass) && !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ProjectClass]).Trim()))
        //        {
        //            int projectclass = Convert.ToInt32(formdic[ReportScheduleConstant.ProjectClass]);
        //            cbProjectClass.SelectedIndex = cbProjectClass.Items.IndexOfValue(projectclass);
        //        }

        //        TicketStatus ticketStatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formdic[ReportScheduleConstant.TicketStatus]));
        //        switch (ticketStatus)
        //        {
        //            case TicketStatus.All:
        //                rbAll.Checked = true;
        //                break;
        //            case TicketStatus.Open:
        //                rbOpen.Checked = true;
        //                break;
        //            case TicketStatus.Closed:
        //                rbClose.Checked = true;
        //                break;
        //            default:
        //                break;
        //        }


        //        chkStatus.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowStatus]);
        //        chkProjectRoles.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowProjectRoles]);
        //        chkProjectDescription.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowProjectDescription]);
        //        chkBudgetSummary.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowBudgetSummary]);
        //        chkPlannedvsActualByCategory.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowPlannedvsActualByCategory]);
        //        chkAccomplishments.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowAccomplishment]);
        //        chkPlan.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowPlan]);
        //        chkIssues.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowIssues]);
        //        chkDecisionLog.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowDecisionLog]);
        //        chkKeyReceivables.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowReceivable]);
        //        chkKeyDeliverables.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowDeliverable]);
        //        chkPlannedvsActualByBudgetItem.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowPlannedvsActualByBudgetItem]);
        //        chkShowMilestone.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowMilestone]);
        //        chkShowAllTasks.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowAllTask]);
        //        chkPlannedvsActualByMonth.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowPlannedvsActualByMonth]);
        //        chkResourceAllocation.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowResourceAllocation]);
        //        chkMilestone.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.ShowSummaryGanttChart]);
        //        chkCalculate.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.CalculateExpected]);
        //        if (!string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ids])))
        //        {
        //            int[] projects = Array.ConvertAll<string, int>(Convert.ToString(formdic[ReportScheduleConstant.ids]).Split(','), int.Parse);
        //            FillProjects();
        //            foreach (int id in projects)
        //            {
        //                ListEditItem item = lstProjectsLHS.Items.FindByValue(id);
        //                if (item != null)
        //                {
        //                    lstProjectsLHS.Items.RemoveAt(item.Index);
        //                    lstProjectsRHS.Items.Add(item);
        //                }
        //            }
        //        }
        //    }
        //}

        protected override void OnLoad(EventArgs e)
        {
            lblMsg.Text = "";

            base.OnLoad(e);
        }

        protected void cblProject_Load()
        {
            lstProjectsLHS.Items.Clear();

            string openTicketQuery = string.Empty;
            List<string> ticketQuery = new List<string>();
            ticketQuery.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));
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

            lstProjectsRHS.Items.Clear();
            if (_DataTable != null && _DataTable.Rows.Count > 0)
            {
                _DataTable = uHelper.SortCollection(_DataTable, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTable.Rows)
                {
                    lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                }

                if (!IsPostBack && Request["alltickets"] != null)
                {
                    string selectExpress = Request["alltickets"];
                    DataRow[] filterColl = _DataTable.Select(string.Format("TicketId IN ({0})", string.Join(",", selectExpress.Split(',').Select(x => string.Format("\'{0}\'", x)).ToList())));
                    if (filterColl != null && filterColl.Length > 0)
                    {
                        foreach (DataRow row in filterColl)
                        {
                            ListEditItem item = lstProjectsLHS.Items.FindByValue(row[DatabaseObjects.Columns.Id]);
                            if (item == null)
                                continue;

                            lstProjectsLHS.Items.Remove(item);
                            lstProjectsRHS.Items.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]), Convert.ToString(row[DatabaseObjects.Columns.Id]));
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

           // string url = "/_layouts/15/uGovernIT/delegatecontrol.aspx?isdlg=1&control=projectreportviewer";
            string url = delegateControl + "?reportName=ProjectStatusReport&";
            url = url + string.Format("&PMMIds={0}", PMMids);
            url = url + string.Format("&projectYear={0}", projectYear);
            url = url + string.Format("&Acc={0}", chkAccomplishments.Checked);
            url = url + string.Format("&Plan={0}", chkPlan.Checked);
            url = url + string.Format("&Status={0}", chkStatus.Checked);
            url = url + string.Format("&Risk={0}", chkRisks.Checked);
            url = url + string.Format("&Issues={0}", chkIssues.Checked);
            url = url + string.Format("&DecisionLog={0}", chkDecisionLog.Checked);
            url = url + string.Format("&SGC={0}", chkMilestone.Checked);
            url = url + string.Format("&SAT={0}", chkShowAllTasks.Checked);
            url = url + string.Format("&SMS={0}", chkShowMilestone.Checked);
            url = url + string.Format("&SKD={0}", chkKeyDeliverables.Checked);
            url = url + string.Format("&SKR={0}", chkKeyReceivables.Checked);
            url = url + string.Format("&CalcExpected={0}", chkCalculate.Checked);
            url = url + string.Format("&ProjectDesc={0}", chkProjectDescription.Checked);
            url = url + string.Format("&BudgetSummary={0}", chkBudgetSummary.Checked);
            url = url + string.Format("&PlannedvsActualByCategory={0}", chkPlannedvsActualByCategory.Checked);
            url = url + string.Format("&PlannedvsActualByBudgetItem={0}", chkPlannedvsActualByBudgetItem.Checked);
            url = url + string.Format("&PlannedvsActualByMonth={0}", chkPlannedvsActualByMonth.Checked);
            url = url + string.Format("&ProjectRoles={0}", chkProjectRoles.Checked);
            url = url + string.Format("&ResourceAllocation={0}", chkResourceAllocation.Checked);
            url = url + string.Format("&ProjectStatus={0}", ticketStatus.ToString());
            url = url + string.Format("&SortOrder={0}", string.Join(",", order.ToArray()));
            url = url + string.Format("&Trafficlight={0}", chkTrafficLight.Checked);
            url = url + string.Format("&AllMilestone={0}", chkAllMilestone.Checked);
            url = url + string.Format("&SOTO={0}", chkShowOpenTaskOnly.Checked);
            Response.Redirect(url);
        }

        private void DisableEnableButtonControl(bool addButton, bool addAllButton, bool removeButton, bool removeAllButton, UGITControlType controltype)
        {
            btnMoveAllItemsToLeftProj.ClientEnabled = removeAllButton;
            btnMoveSelectedItemsToLeftProj.ClientEnabled = removeButton;
            btnMoveAllItemsToRightProj.ClientEnabled = addAllButton;
            btnMoveSelectedItemsToLeftProj.ClientEnabled = addButton;
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
            if(lstFrom.ID == "lstProjectsLHS")
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

        private void FillProjects()
        {
            string spQuery = string.Empty;

            List<string> QueryList = new List<string>();
            //QueryList.Add(string.Format("{0}='{1}'", DatabaseObjects.Columns.TenantID, _context.TenantID));
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

            QueryList = new List<string>();

            if (!string.IsNullOrEmpty(faquery))
                QueryList.Add(faquery);

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
            else if (rbAll.Checked)
            {
                _DataTable = ticketManager.GetAllTickets(module);
            }
            if (QueryList.Count > 0)
            {
                spQuery = $" {string.Join(" And ", QueryList)}";
            }
            lstProjectsLHS.Items.Clear();
            //lstProjectsRHS.Items.Clear();
            var filterRows = _DataTable.Select(spQuery);
            DataTable _DataTableFilter = filterRows.Any() ? filterRows.CopyToDataTable() : null;

            if (_DataTableFilter != null && _DataTableFilter.Rows.Count > 0)
            {
                _DataTableFilter = uHelper.SortCollection(_DataTableFilter, DatabaseObjects.Columns.Title);
                foreach (DataRow dr in _DataTableFilter.Rows)
                {
                    if (lstProjectsRHS.Items.FindByValue(Convert.ToString(dr[DatabaseObjects.Columns.ID])) == null)
                    {
                        lstProjectsLHS.Items.Add(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.ID]));
                    }
                }
            }
            DisableEnableButtonControl(false, (lstProjectsLHS.Items.Count > 0), false, (lstProjectsRHS.Items.Count > 0), UGITControlType.Projects);
        }

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

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.ProjectReport);
            formdic.Add(ReportScheduleConstant.TicketStatus, (rbOpen.Checked ? TicketStatus.Open :
                                        (rbClose.Checked ? TicketStatus.Closed :
                                        (rbAll.Checked ? TicketStatus.All : TicketStatus.Open))));

            formdic.Add(ReportScheduleConstant.TicketPriority, rblistPriority.Value);
            formdic.Add(ReportScheduleConstant.Department, cbDepartment.Value);
            formdic.Add(ReportScheduleConstant.FunctionAreas, cbFunctionalarea.Value);
            formdic.Add(ReportScheduleConstant.Category, cbCategory.Value);
            formdic.Add(ReportScheduleConstant.SubCategory, cbSubCategory.Value);
            formdic.Add(ReportScheduleConstant.RequestType, cbRequesttype.Value);
            formdic.Add(ReportScheduleConstant.ProjectInitiative, cbProjectInitiative.Value);
            formdic.Add(ReportScheduleConstant.ProjectClass, cbProjectClass.Value);

            List<string> ids = new List<string>();
            foreach (ListEditItem item in lstProjectsRHS.Items)
                ids.Add(Convert.ToString(item.Value));
            formdic.Add(ReportScheduleConstant.ids, string.Join(",", ids.ToArray()));

            formdic.Add(ReportScheduleConstant.ShowAccomplishment, chkAccomplishments.Checked);
            formdic.Add(ReportScheduleConstant.ShowPlan, chkPlan.Checked);
            formdic.Add(ReportScheduleConstant.ShowIssues, chkIssues.Checked);
            formdic.Add(ReportScheduleConstant.ShowDecisionLog, chkDecisionLog.Checked);
            formdic.Add(ReportScheduleConstant.ShowStatus, chkStatus.Checked);
            formdic.Add(ReportScheduleConstant.ShowSummaryGanttChart, chkMilestone.Checked);
            formdic.Add(ReportScheduleConstant.ShowAllTask, chkShowAllTasks.Checked);
            formdic.Add(ReportScheduleConstant.ShowMilestone, chkShowMilestone.Checked);
            formdic.Add(ReportScheduleConstant.ShowDeliverable, chkKeyDeliverables.Checked);
            formdic.Add(ReportScheduleConstant.ShowReceivable, chkKeyReceivables.Checked);
            formdic.Add(ReportScheduleConstant.CalculateExpected, chkCalculate.Checked);
            formdic.Add(ReportScheduleConstant.ShowProjectDescription, chkProjectDescription.Checked);
            formdic.Add(ReportScheduleConstant.ShowBudgetSummary, chkBudgetSummary.Checked);
            formdic.Add(ReportScheduleConstant.ShowPlannedvsActualByCategory, chkPlannedvsActualByCategory.Checked);
            formdic.Add(ReportScheduleConstant.ShowPlannedvsActualByBudgetItem, chkPlannedvsActualByBudgetItem.Checked);
            formdic.Add(ReportScheduleConstant.ShowPlannedvsActualByMonth, chkPlannedvsActualByMonth.Checked);
            formdic.Add(ReportScheduleConstant.ShowProjectRoles, chkProjectRoles.Checked);
            formdic.Add(ReportScheduleConstant.ShowResourceAllocation, chkResourceAllocation.Checked);
            formdic.Add(ReportScheduleConstant.ShowMonitorState, true);

            uHelper.ReportScheduleDict = formdic;

            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
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

        //protected void btRefresh_Click(object sender, EventArgs e)
        //{
        //    FillProjects();
        //}

        protected void lstProjectsLHS_Callback(object sender, CallbackEventArgsBase e)
        {
            FillProjects();
        }

        protected void cbFunctionalarea_Callback(object sender, CallbackEventArgsBase e)
        {
            FillFunctionalArea(e.Parameter);
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
        private void SwapDropDownValue(DropDownList ddl1, DropDownList ddl2, int remSum)
        {
            int tempValue = Math.Abs( 15 - remSum - UGITUtility.StringToInt(ddl2.SelectedValue));
            ddl2.SelectedValue = ddl1.SelectedValue;
            ddl1.SelectedValue = tempValue.ToString();
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

        protected void ddlProjectSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrioritySortOrder.SelectedValue == ddlProjectSortOrder.SelectedValue)
            {
                int remSum = UGITUtility.StringToInt(ddlTargetDateSortOrder.SelectedValue) + UGITUtility.StringToInt(ddlProjectRank.SelectedValue) + UGITUtility.StringToInt(ddlProgressSortOrder.SelectedValue); ;
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
    }
}
