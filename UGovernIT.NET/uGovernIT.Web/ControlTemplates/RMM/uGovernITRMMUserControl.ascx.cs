using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Cache;

namespace uGovernIT.Web
{
    public partial class uGovernITRMMUserControl : UserControl
    {
        DataTable tabs = new DataTable();
        public bool HideAllocationTab { get; set; }
        public bool HideActualTab { get; set; }
        public bool HideResourceTab { get; set; }
        public bool HideResourcePlanningTab { get; set; }
        public bool HideResourceAvailabilityTab { get; set; }
        public bool HideAllocationTimelineTab { get; set; }
        public bool HideProjectComplexityTab { get; set; }
        public bool HideCapacityReportTab { get; set; }
        public bool HideBillingAndMarginsTab { get; set; }
        public bool HideExecutiveKPITab { get; set; }
        public bool HideResourceUtilizationIndexTab { get; set; }
        //public bool HideFinancialViewTab { get; set; }
        public bool HideManageAllocationTemplatesTab { get; set; }
        public bool HideBenchTab { get; set; }

        public int ResourceAllocationOrder { get; set; }
        public int ActualOrder { get; set; }
        public int ResourceOrder { get; set; }
        public int ResourcePlaningOrder { get; set; }
        public int ResourceAvailabilityOrder { get; set; }
        public int AllocationTimelineOrder { get; set; }
        public int ProjectComplexityOrder { get; set; }
        public int CapacityReportOrder { get; set; }
        public int BillingAndMarginOrder { get; set; }
        public int ExecutiveKPIOrder { get; set; }
        public int ResourceUtilizationIndexOrder { get; set; }
        //public int FinancialViewOrder { get; set; }
        public int ManageAllocationTemplatesTabOrder { get; set; }
        public int BenchTabOrder { get; set; }

        protected string RmmResourceReportUrl = string.Empty;
        protected string resourceByPersonReportUrl = string.Empty;
        protected string resourceEstimatedRemainingHoursUrl = string.Empty;
        protected string resourceUrl = string.Empty;
        public string customQueryUrl = string.Empty;
        bool isResourceAdmin = false;
        bool isSuperAdmin;
        public string BenchReportUrl { get; set; }
        

        protected string skillReportUrl = string.Empty;

        protected string resourceSkillUrl = string.Empty;
        public string RUIurl = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public ConfigurationVariableManager ObjConfigurationVariableManager = null;

        UserProfileManager objUserProfileManager = HttpContext.Current.GetUserManager();
        AllocationManager objAllocationManager = null;
        ModuleViewManager objModuleViewManager = null;
        protected override void OnInit(EventArgs e)
        {
            ObjConfigurationVariableManager = new ConfigurationVariableManager(context);
            objAllocationManager = new AllocationManager(context);
            objModuleViewManager = new ModuleViewManager(context);

            //Checks current user is in super admin group or not
            isSuperAdmin = objUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser());
            BenchReportUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=benchreport");
            RUIurl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=resourceutilizationindex");
            customQueryUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=reportresult");
            resourceSkillUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=resourceskillreport");
            Guid allocationFrameID = Guid.NewGuid();
            string url = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=CustomResourceAllocation", allocationFrameID));
            LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url, allocationFrameID));
            tabPanelconainerAllocation.Controls.Add(lCtr);

            Guid actualFrameID = Guid.NewGuid();
            string url1 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=customresourcetimeSheet", actualFrameID));
            LiteralControl lCtr1 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' minheight='250px' frameborder='0' id='{1}'></iframe>", url1, actualFrameID));
            tabPanelconainerActual.Controls.Add(lCtr1);

            Guid userFrameID = Guid.NewGuid();
            string url2 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?isdlg=1&isudlg=1&frameObjId={0}&control=customgroupsandusersinfo", userFrameID));
            LiteralControl lCtr2 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' minheight='800px' frameborder='0' id='{1}'></iframe>", url2, userFrameID));
            tabPanelconainerUsers.Controls.Add(lCtr2);

            Guid resourcePlanningFrameID = Guid.NewGuid();
            string url3 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=skillreport&isdlg=1&isudlg=1", resourcePlanningFrameID));
            LiteralControl lCtr3 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url3, resourcePlanningFrameID));
            tabPanelresourcePlanning.Controls.Add(lCtr3);

            Guid resourceAvailabilityID = Guid.NewGuid();
            string url4 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=resourceavailability&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", resourceAvailabilityID));
            LiteralControl lCtr4 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url4, resourceAvailabilityID));
            tabPanelresourceavailability.Controls.Add(lCtr4);

            //new tab Resource Allocation
            Guid allocationTimelineFrameID = Guid.NewGuid();
            string url5 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?frameObjId={0}&control=resourceallocationgrid&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", allocationTimelineFrameID));
            LiteralControl lCtr5 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' style='min-width:1180px' height='100' frameborder='0' id='{1}'></iframe>", url5, allocationTimelineFrameID));
            tabPanelAllocationTimeline.Controls.Add(lCtr5); 

            //new tab Resource Project Complexity
            Guid projectComplexityFrameID = Guid.NewGuid();
            string url6 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?frameObjId={0}&control=ResourceProjectComplexity&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", projectComplexityFrameID));
            LiteralControl lCtr6 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' style='min-width:1180px' height='100' frameborder='0' id='{1}'></iframe>", url6, projectComplexityFrameID));
            tabPanelProjectComplexity.Controls.Add(lCtr6);

            Guid capcityReportID = Guid.NewGuid();
            string url7 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=capcityreport&isdlg=1&isudlg=1", capcityReportID));
            LiteralControl lCtr7 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url7, capcityReportID));
            tabPanelCapacityReport.Controls.Add(lCtr7);

            Guid billingandmarginReportID = Guid.NewGuid();
            string url8 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=BillingAndMarginsReport", billingandmarginReportID));
            LiteralControl lCtr8 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url8, billingandmarginReportID));
            tabPanelBillingAndMargins.Controls.Add(lCtr8);

            Guid executiveKpiReportID = Guid.NewGuid();
            string url9 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=executivekpi", executiveKpiReportID));
            LiteralControl lCtr9 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url9, executiveKpiReportID));
            tabPanelExecutiveKpi.Controls.Add(lCtr9);

            Guid resourceutilizationindexReportID = Guid.NewGuid();
            string url10 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=resourceutilizationindex", resourceutilizationindexReportID));
            LiteralControl lCtr10 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url10, resourceutilizationindexReportID));
            tabPanelResourceUtilizationIndex.Controls.Add(lCtr10);

            Guid manageAllocationTemplatesReportID = Guid.NewGuid();
            string url12 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=manageallocationtemplates", manageAllocationTemplatesReportID));
            LiteralControl lCtr12 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url12, manageAllocationTemplatesReportID));
            tabPanelManageAllocationTemplates.Controls.Add(lCtr12);

            Guid benchReportID = Guid.NewGuid();
            string url13 = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=bench", benchReportID));
            LiteralControl lCtr13 = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' scrolling='no' frameurl='{0}' width='100%' height='100' frameborder='0' id='{1}'></iframe>", url13, benchReportID));
            tabPanelBench.Controls.Add(lCtr13);

            // Get the reports absolute url.
            RmmResourceReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/RMMResourceReport.aspx"));
            resourceEstimatedRemainingHoursUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?control=resourceerh"));
            resourceUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/delegatecontrol.aspx?control=ResourceReport"));


            base.OnInit(e);

            //Generate the url for "Resource Report -2.
            resourceByPersonReportUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/ResourceByPersonReport.aspx"));
            isResourceAdmin = objUserProfileManager.IsUGITSuperAdmin(HttpContext.Current.CurrentUser()) || objUserProfileManager.IsResourceAdmin(HttpContext.Current.CurrentUser()) || objUserProfileManager.IsAdmin(HttpContext.Current.CurrentUser());
           

            LoadDdlResourceManager();
            LoadFunctionalArea();
            LoaddllResourceUser();
            UserProfile currentuser = context.CurrentUser;
            ddlDepartmentControl.SetValues(UGITUtility.ObjectToString(currentuser.Department));
            if (!IsPostBack)
                GenerateLevel1Category();
        }

        private void LoadFunctionalArea()
        {
            //Bind Work Functional Area
            DataTable dtFunArea = GetTableDataManager.GetTableData(DatabaseObjects.Tables.FunctionalAreas, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND {DatabaseObjects.Columns.Deleted} = 0");
            if (dtFunArea != null && dtFunArea.Rows.Count > 0)
            {
                dtFunArea = dtFunArea.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Id });
                dtFunArea.DefaultView.Sort = DatabaseObjects.Columns.Title + " ASC";
                ddlFunctionalArea.DataSource = dtFunArea;
                ddlFunctionalArea.DataTextField = DatabaseObjects.Columns.Title;
                ddlFunctionalArea.DataValueField = DatabaseObjects.Columns.Title;
                ddlFunctionalArea.DataBind();
                ddlFunctionalArea.Items.Insert(0, "All");
            }
        }

        protected void LoadDdlResourceManager()
        {
            if (isResourceAdmin)
            {
                // Get cached monthly summary table
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("@TenantID", context.TenantID);
                //Fetch distinct managers sorted by their names.
                DataTable table = GetTableDataManager.GetData("distinctmanageruser", values);
                ddlResourceManager.Items.Clear();
                if (table != null && table.Rows.Count > 0)
                {
                    ddlResourceManager.DataSource = table;
                    ddlResourceManager.DataValueField = "ManagerLookup";
                    ddlResourceManager.DataTextField = "ManagerLookup$";
                    ddlResourceManager.DataBind();
                }                
                ddlResourceManager.Items.Insert(0, "All");
            }
            else
            {
                UserProfile currentUser = objUserProfileManager.LoadById(HttpContext.Current.CurrentUser().Id);
                if (currentUser != null && currentUser.IsManager)
                {
                    List<UserProfile> userProfiles = objUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id);
                    userProfiles = userProfiles.Where(x => x.IsManager).ToList();
                    //foreach (UserProfile uProfile in userProfiles)
                    //{
                    //    ddlResourceManager.Items.Add(new ListItem(uProfile.Name, uProfile.Name));
                    //}
                    ddlResourceManager.DataSource = userProfiles;
                    ddlResourceManager.DataValueField = "Name";
                    ddlResourceManager.DataTextField = "Name";
                    ddlResourceManager.DataBind();
                }
                else
                {
                    dvResourceReport.Visible = false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Constants.PageTitle.Rmm;
            CreateTabs();
            SetMetInfoForIE();


            //Get param from querystring and deside which tab will be shown
            int formulaID = 0;
            if (Request["fID"] != null)
            {
                int.TryParse(Request["fID"], out formulaID);
            }
            int expressionID = 0;
            if (Request["eID"] != null)
            {
                int.TryParse(Request["eID"], out expressionID);
            }

            if (UGITUtility.GetCookie(Request, "RMMTabId") != null)
            {
                hTabNumber.Value = UGITUtility.GetCookieValue(Request, "RMMTabId");
            }
            else if (formulaID > 0 && expressionID > 0 && !IsPostBack)
            {
                //commented becuase its hardcode index value.
                // hTabNumber.Value = "3";
            }

           
            //SPListItem rmmItem = SPListHelper.LoadModuleListItem("RMM")
            UGITModule rmmItem = objModuleViewManager.GetByName("RMM");
            if (rmmItem != null)
            {
                if (rmmItem.ModuleDescription!= null)
                {
                    moduleDescription.Text = rmmItem.ModuleDescription.ToString();
                    cModuleImgPanel.Visible = false ;
                }
                //Commented By Munna
                if (rmmItem.Attachments.Split(',').ToList().Count > 0)
                {
                    moduleLogo.ImageUrl = rmmItem.Attachments.Split(',').ToList()[0];
                }
                else
                {
                    cModuleImgPanel.Visible = true;
                    moduleLogo.ImageUrl = "/Content/images/uGovernIT/" + rmmItem.ModuleName.ToString() + "_32x32.png";
                    moduleLogo.CssClass = "rmmModule-homeImg";
                }
            }

            
        }

        private void CreateTabs()
        {
            if (tbcDetailTabs.Tabs.Count == 0)
            {
                // Set default ordering if not present
                if (ResourceAllocationOrder == 0)
                    ResourceAllocationOrder = 1;
                if (ActualOrder == 0)
                    ActualOrder = 2;
                if (ResourceOrder == 0)
                    ResourceOrder = 3;
                if (ResourcePlaningOrder == 0)
                    ResourcePlaningOrder = 4;
                if (ResourceAvailabilityOrder == 0)
                    ResourceAvailabilityOrder = 5;
                if (AllocationTimelineOrder == 0)
                    AllocationTimelineOrder = 6;
                if (ProjectComplexityOrder == 0)
                    ProjectComplexityOrder = 7;
                if (CapacityReportOrder == 0)
                    CapacityReportOrder = 8;
                if (BillingAndMarginOrder == 0)
                    BillingAndMarginOrder = 9;
                if (ExecutiveKPIOrder == 0)
                    ExecutiveKPIOrder = 10;
                if (ResourceUtilizationIndexOrder == 0)
                    ResourceUtilizationIndexOrder = 11;
                //if (FinancialViewOrder == 0)
                //    FinancialViewOrder = 11;
                if (ManageAllocationTemplatesTabOrder == 0)
                    ManageAllocationTemplatesTabOrder = 12;

                if(BenchTabOrder == 0)
                    BenchTabOrder = 13;

                var dictionaryDisplayOrder = new Dictionary<string, int>(5);
                dictionaryDisplayOrder.Add("Resources", ResourceOrder);
                dictionaryDisplayOrder.Add("ResourceAllocation", ResourceAllocationOrder);
                dictionaryDisplayOrder.Add("Actuals", ActualOrder);                
                dictionaryDisplayOrder.Add("ResourcePlanning", ResourcePlaningOrder);
                dictionaryDisplayOrder.Add("ResourceAvailability", ResourceAvailabilityOrder);
                dictionaryDisplayOrder.Add("AllocationTimeline", AllocationTimelineOrder);
                dictionaryDisplayOrder.Add("ProjectComplexity", ProjectComplexityOrder);
                dictionaryDisplayOrder.Add("CapacityReport", CapacityReportOrder);
                dictionaryDisplayOrder.Add("BillingAndMargins", BillingAndMarginOrder);
                dictionaryDisplayOrder.Add("ExecutiveKpi", ExecutiveKPIOrder);
                dictionaryDisplayOrder.Add("ResourceUtilizationIndex", ResourceUtilizationIndexOrder);
                dictionaryDisplayOrder.Add("ManageAllocationTemplates", ManageAllocationTemplatesTabOrder);
                dictionaryDisplayOrder.Add("Bench", BenchTabOrder);
                //dictionaryDisplayOrder.Add("FinancialView", FinancialViewOrder);

                // Order by values.
                // ... Use LINQ to specify sorting by value.
                //var items = from pair in dictionaryDisplayOrder
                //            orderby pair.Value ascending
                //            select pair;
                var items = from pair in dictionaryDisplayOrder
                            orderby pair.Value ascending
                            select pair;
                // Display results.
                foreach (KeyValuePair<string, int> pair in items)
                {
                    if (!HideResourceTab && pair.Key == "Resources")
                    {
                        DevExpress.Web.Tab tab3 = new DevExpress.Web.Tab("Resources", pair.Key);
                        tab3.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab3);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }

                    }

                    if (!HideAllocationTab && pair.Key == "ResourceAllocation")
                    {
                        DevExpress.Web.Tab tab1 = new DevExpress.Web.Tab("Resource Allocation", pair.Key);
                        tab1.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab1);

                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }

                    }

                    if (!HideActualTab && pair.Key == "Actuals")
                    {
                        DevExpress.Web.Tab tab2 = new DevExpress.Web.Tab("Timesheet", pair.Key);
                        tab2.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab2);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }

                    }
                
                    if (!HideResourcePlanningTab && pair.Key == "ResourcePlanning")
                    {
                        DevExpress.Web.Tab tab4 = new DevExpress.Web.Tab("Resource Planning", pair.Key);
                        tab4.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab4);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if(!HideResourceAvailabilityTab && pair.Key == "ResourceAvailability")
                    {
                        DevExpress.Web.Tab tab5 = new DevExpress.Web.Tab("Resource Utilization", pair.Key);
                        tab5.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab5);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if (!HideAllocationTimelineTab && pair.Key == "AllocationTimeline")
                    {
                        DevExpress.Web.Tab tab6 = new DevExpress.Web.Tab("Administrator View", pair.Key);
                        tab6.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab6);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if(!HideProjectComplexityTab && pair.Key == "ProjectComplexity")
                    {
                        DevExpress.Web.Tab tab7 = new DevExpress.Web.Tab("Project Complexity", pair.Key);
                        tab7.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab7);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if (!HideCapacityReportTab && pair.Key == "CapacityReport")
                    {
                        DevExpress.Web.Tab tab8 = new DevExpress.Web.Tab("Capacity Planning", pair.Key);
                        tab8.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab8);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if(!HideBillingAndMarginsTab && pair.Key == "BillingAndMargins")
                    {
                        DevExpress.Web.Tab tab9 = new DevExpress.Web.Tab("Billing And Margins", pair.Key);
                        tab9.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab9);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if (!HideExecutiveKPITab && pair.Key == "ExecutiveKpi")
                    {
                        DevExpress.Web.Tab tab10 = new DevExpress.Web.Tab("Executive KPI", pair.Key);
                        tab10.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab10);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if (!HideResourceUtilizationIndexTab && pair.Key == "ResourceUtilizationIndex")
                    {
                        DevExpress.Web.Tab tab10 = new DevExpress.Web.Tab("RUI", pair.Key);
                        tab10.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab10);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if (!HideManageAllocationTemplatesTab && pair.Key == "ManageAllocationTemplates")
                    {
                        DevExpress.Web.Tab tab10 = new DevExpress.Web.Tab("Manage Allocation Templates", pair.Key);
                        tab10.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab10);
                        if (string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }

                    if(!HideBenchTab && pair.Key == "Bench")
                    {
                        DevExpress.Web.Tab tab11 = new DevExpress.Web.Tab("Bench", pair.Key);
                        tab11.ToolTip = pair.Key;
                        tbcDetailTabs.Tabs.Add(tab11);
                        if(string.IsNullOrEmpty(hTabNumber.Value))
                        {
                            hTabNumber.Value = pair.Key.Replace(" ", "");
                        }
                    }
                    //if (!HideFinancialViewTab && pair.Key == "FinancialView")
                    //{
                    //    DevExpress.Web.Tab tab10 = new DevExpress.Web.Tab("Financial View", pair.Key);
                    //    tab10.ToolTip = pair.Key;
                    //    tbcDetailTabs.Tabs.Add(tab10);
                    //    if (string.IsNullOrEmpty(hTabNumber.Value))
                    //    {
                    //        hTabNumber.Value = pair.Key.Replace(" ", "");
                    //    }
                    //}
                }

            }
        }

        protected void TabRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                RepeaterItem rItem = e.Item;
                DataRowView rowView = (DataRowView)rItem.DataItem;
                LinkButton tabLink = (LinkButton)rItem.FindControl("tabLink");
                tabLink.Text = rowView["TabName"].ToString();
                tabLink.ToolTip = rowView["TabToolTip"].ToString();
                tabLink.Attributes.Add("href", "javascript:Void(0)");
            }
        }

        protected void CategoryLevel1_Load(object sender, EventArgs e)
        {
            //if (categoryLevel1.Items.Count <= 0)
            //{
            //    SPList categoryList = SPListHelper.LoadRequestCategoriesList();
            //    SPQuery query = new SPQuery();
            //    query.Query = string.Format("<Where><Eq><FieldRef Name='{1}' /><Value Type='Text'>{0}</Value></Eq></Where>", "Level1",DatabaseObjects.Columns.RequestCategoryType);
            //    SPListItemCollection collection = categoryList.GetItems(query);
            //    if (collection != null && collection.Count > 0)
            //    {
            //        categoryLevel1.DataSource = collection.GetDataTable();
            //        categoryLevel1.DataTextField = DatabaseObjects.Columns.Title;
            //        categoryLevel1.DataValueField = DatabaseObjects.Columns.Id;
            //        categoryLevel1.DataBind();
            //        categoryLevel1.Items.Add(new ListItem("Project", "-1"));
            //    }
            //}
        }

        private void GenerateLevel1Category()
        {
            //Get the allocation type
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            DataTable AllocationTypeTable = AllocationTypeManager.LoadLevel1(context); //In SP: AllocationType.LoadLevel1();
            if (AllocationTypeTable != null && AllocationTypeTable.Rows.Count > 0)
            {
                DataTable table = AllocationTypeTable.Clone();
                DataColumn LevelValue = new DataColumn("LevelValue");
                table.Columns.Add(LevelValue);

                foreach (DataRow row in AllocationTypeTable.Rows)
                {
                    string Level1 = Convert.ToString(row["LevelTitle"]);
                    if (!string.IsNullOrEmpty(Level1))
                    {

                        DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules,$"{string.Format("{0}='{1}' and {2}='{3}' ", DatabaseObjects.Columns.TenantID, context.TenantID, DatabaseObjects.Columns.Title, Level1)}");
                        //dtModules= moduleManager.GetDataTable($"Title = '{Level1}' ");
                        //DataRow[] drModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules).Select(string.Format("{0}='{1}'",DatabaseObjects.Columns.Title, Level1));
                        if (dtModules != null && dtModules.Rows.Count > 0)
                        {
                            ListItem item = new ListItem(Level1, Convert.ToString(dtModules.Rows[0][DatabaseObjects.Columns.ModuleName]));
                            item.Selected = true;
                            chkBoxCategoryList.Items.Add(item);
                            item.Selected = true;
                        }
                        else
                        {
                            ListItem item = new ListItem(Level1, Level1);
                            item.Selected = true;
                            chkBoxCategoryList.Items.Add(item);
                            item.Selected = true;
                        }
                        //ConfigurationVariableManager ObjConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                        ////if (Level1.Trim() == ConfigurationVariable.RMMLevel1PMMProjects)
                        //if (Level1.Trim() == ObjConfigurationVariableManager.GetValue("RMMLevel1PMMProjects"))
                        //{
                        //    ListItem item = new ListItem(ObjConfigurationVariableManager.GetValue("RMMLevel1PMMProjects"), Constants.RMMLevel1PMMProjectsType);
                        //    item.Selected = true;
                        //    chkBoxCategoryList.Items.Add(item);
                        //    item.Selected = true;
                        //}
                        ////else if (Level1.Trim() == ConfigurationVariable.RMMLevel1TSKProjects)
                        //else if (Level1.Trim() == ObjConfigurationVariableManager.GetValue("RMMLevel1TSKProjects"))
                        //{
                        //    ListItem item = new ListItem(ObjConfigurationVariableManager.GetValue("RMMLevel1TSKProjects"), Constants.RMMLevel1TSKProjectsType);
                        //    item.Selected = true;
                        //    chkBoxCategoryList.Items.Add(item);
                        //    item.Selected = true;
                        //}
                        ////else if (Level1.Trim() == ConfigurationVariable.RMMLevel1NPRProjects)
                        //else if (Level1.Trim() == ObjConfigurationVariableManager.GetValue("RMMLevel1NPRProjects"))
                        //{
                        //    ListItem item = new ListItem(ObjConfigurationVariableManager.GetValue("RMMLevel1NPRProjects"), Constants.RMMLevel1NPRProjectsType);
                        //    item.Selected = true;
                        //    chkBoxCategoryList.Items.Add(item);
                        //    item.Selected = true;
                        //}
                        //else
                        //{
                        //    ListItem item = new ListItem(Level1, Level1);
                        //    item.Selected = true;
                        //    chkBoxCategoryList.Items.Add(item);
                        //    item.Selected = true;
                        //}
                    }
                }

            }
        }

        protected void ddlReportView_Load(object sender, EventArgs e)
        {
            if (ddlReportView.Items.Count <= 3)
            {
                DataTable dashboardPanels = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                if (dashboardPanels != null)
                {
                    dashboardPanels = dashboardPanels.Copy();
                    dashboardPanels.Columns.Add("ModuleNameTemp");
                    dashboardPanels.Columns["ModuleNameTemp"].Expression = string.Format("'{0}'+{1}+'{0}'", Constants.Separator, DatabaseObjects.Columns.DashboardModuleMultiLookup);
                    DataRow[] tempRows = null;
                    tempRows = dashboardPanels.Select(string.Format("{0} like '%{2}{1}{2}%'", "ModuleNameTemp", "RMM", Constants.Separator));

                    if (tempRows.Length > 0)
                    {
                        foreach (DataRow q in tempRows)
                        {
                            ddlReportView.Items.Add(new ListItem(Convert.ToString(q[DatabaseObjects.Columns.Title]), string.Format("query_{0}", q[DatabaseObjects.Columns.Id])));
                        }
                    }
                }
            }
        }

        private void SetMetInfoForIE()
        {
            foreach (Control ctl in Page.Header.Controls)
            {
                if (ctl is System.Web.UI.HtmlControls.HtmlMeta)
                {
                    System.Web.UI.HtmlControls.HtmlMeta meta = (System.Web.UI.HtmlControls.HtmlMeta)ctl;
                    if (meta != null && meta.HttpEquiv == "X-UA-Compatible")
                    {
                        meta.Content = "IE=10";
                        break;
                    }
                }
            }

        }

        private void LoaddllResourceUser()
        {
            List<UserProfile> userCollection = objUserProfileManager.GetUsersProfile();
            
            if (ddlUserResource.Items.Count <= 0)
            {

                if (isSuperAdmin)
                {
                    
                    //userCollection = objUserProfileManager.GetUsersProfile();      // objUserProfileManager.GetAllSiteUsers(); returns no data
                    if (userCollection != null)
                    {
                        ddlUserResource.DataValueField = "Id";
                        ddlUserResource.DataTextField = "Name";
                        ddlUserResource.DataBind();
                        foreach (UserProfile user in userCollection)
                        {
                            ddlUserResource.Items.Add(new ListItem(user.Name, user.Id.ToString()));
                        }

                    }
                }
                else
                {

                    userCollection = userCollection.Where(x => x.ManagerID == HttpContext.Current.CurrentUser().Id && x.Enabled == true).ToList();
                    //userCollection = objUserProfileManager.GetUserByManager(HttpContext.Current.CurrentUser().Id);//LoadUserWorkingUnder(SPContext.Current.Web.CurrentUser.ID, ref userCollection);
                    userCollection = userCollection.OrderBy(x => x.Name).ToList();
                    if (userCollection != null)
                    {
                        foreach (UserProfile user in userCollection)
                        {
                            ddlUserResource.Items.Add(new ListItem(user.Name, user.Id.ToString()));
                        }
                        ddlUserResource.DataValueField = "Id";
                        ddlUserResource.DataTextField = "Name";
                        ddlUserResource.DataBind();
                    }
                }
            }
        }
    }
}
