using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class uGovernITHomeUserControl : UserControl
    {
        #region Properties

        public string WelcomeHeading { get; set; }
        public string WelcomeDesc { get; set; }
        public bool HideWaitingOnMeTab { get; set; }
        public bool HideMyTickets { get; set; }
        public bool HideMyTasks { get; set; }
        public bool HideSMSModules { get; set; }
        public bool HideGovernanceModules { get; set; }
        public int NoOfPreviewTickets { get; set; }
        public bool ShowGuideMePanel { get; set; }
        public string GuideMeTitle { get; set; }
        public string GuideMeDescription { get; set; }
        public bool ShowServiceCatalog { get; set; }
        public bool HideMyDepartment { get; set; }
        public bool HideMyProject { get; set; }
        public bool HideMyPendingApprovals { get; set; }
        public int MyTicketPanelOrder { get; set; }
        public bool EnableNewButton { get; set; }
        public int ServiceCatalogOrder { get; set; }
        public int ModulePanelOrder { get; set; }
        public string UpdateWaitingOnMeTab { get; set; }
        public string UpdateMyTickets { get; set; }
        public string UpdateMyTasks { get; set; }
        public string UpdateMyPendingApprovals { get; set; }
        public string UpdateMyDepartmentTickets { get; set; }
        public string UpdateMyProject { get; set; }
        public bool HideMyClosedTickets { get; set; }
        public string UpdateMyClosedTickets { get; set; }
        public int autoRefreshFrequency { get; set; }
        public bool ShowServiceIcons { get; set; }
        public string ServiceCatalogViewMode { get; set; }
        public bool IsServiceDocPanel { get; set; }

        public bool ShowPanel { get; set; }
        public long PanelId { get; set; }
        public int IconSize { get; set; }
        public bool ShowCompactRows { get; set; }
        public bool ShowBandedRows { get; set; }
        public string ViewName { get; set; }
        #endregion

        #region variables

        //Guideme default title and description
        string guidmeTitle = "Guide Me!";
        string guidmeDescription = "Start here if you are not sure which module you need.";

        protected string viewTicketsPath = string.Empty;

        //DataRow[] moduleCollection = null;
        //DataTable userTypeList = null;
        List<ModuleStatisticResponse> moduleStats = new List<ModuleStatisticResponse>();
        DataTable moduleTable = new DataTable();

        protected string calendarURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx");
        private List<string> authorizedModulesArray = new List<string>();

        protected string ticketURL = string.Empty;
        protected string sourceURL;
        protected string editTaskFormUrl = string.Empty;
        protected string ajaxHelperPage = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager objConfigurationVariableHelper = HttpContext.Current.GetManagerContext().ConfigManager;
        UserProfile user;
        public string ajaxPageURL = string.Empty;
        public string paramData = string.Empty;
        protected string newServiceURL;

        Tab waitingonmeLinkTab = null;
        Tab myrequestsLinkTab = null;
        Tab myclosedrequestsLinkTab = null;
        Tab mytaskLinkTab = null;
        Tab departmentTicketLinkTab = null;
        Tab myprojectLinkTab = null;
        Tab documentpendingapproveLinkTab = null;
        Tab divisionTicketLinkTab = null;
        #endregion

        protected override void OnInit(EventArgs e)
        {

            if (IsServiceDocPanel)
            {
                cMyTicketPanel.Visible = false;
                cMyTicketPanel.Style.Add("display", "none");
                cModulePanel.Visible = false;
                cModulePanel.Style.Add("display", "none");
            }

            //tab related change..
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/phraseSearchlobalTicketCreate/");
            user = Context.CurrentUser();

            TabViewManager tabViewManager = new TabViewManager(context);
            List<TabView> tabViewrows = tabViewManager.GetTabsByViewName(!string.IsNullOrEmpty(ViewName)?ViewName:"Home");
            foreach (TabView item in tabViewrows)
            {
                if (item.ShowTab)
                {
                    Tab newtab = new Tab(Convert.ToString(item.TabDisplayName), Convert.ToString(item.TabName));
                    filterTabHome.Tabs.Add(newtab);
                }
            }
            

            if (NoOfPreviewTickets <= 0)
                NoOfPreviewTickets = 10;

            tabRepeater.DataSource = tabViewrows;
            tabRepeater.DataBind();

            // Panels Reordering.
            HtmlTableRow myticketPanel = itemsTable.Rows[2];
            HtmlTableRow servicePanel = itemsTable.Rows[3];
            HtmlTableRow modulesPanel = itemsTable.Rows[4];

            // Set default ordering if not present
            if (MyTicketPanelOrder == 0)
                MyTicketPanelOrder = 1;
            if (ServiceCatalogOrder == 0)
                ServiceCatalogOrder = 2;
            if (ModulePanelOrder == 0)
                ModulePanelOrder = 3;

            myTicketPanel.Value = MyTicketPanelOrder.ToString();
            myServiceCatalog.Value = ServiceCatalogOrder.ToString();
            myModulePanel.Value = ModulePanelOrder.ToString();

            try
            {
                Dictionary<int, HtmlTableRow> val = new Dictionary<int, HtmlTableRow>();
                if (IsPostBack)
                {
                    val.Add(Convert.ToInt16(Request[myTicketPanel.UniqueID]), myticketPanel);
                    val.Add(Convert.ToInt16(Request[myServiceCatalog.UniqueID]), servicePanel);
                    val.Add(Convert.ToInt16(Request[myModulePanel.UniqueID]), modulesPanel);
                }
                else
                {
                    val.Add(MyTicketPanelOrder, myticketPanel);
                    val.Add(ServiceCatalogOrder, servicePanel);
                    val.Add(ModulePanelOrder, modulesPanel);
                }

                itemsTable.Rows.RemoveAt(4);
                itemsTable.Rows.RemoveAt(3);
                itemsTable.Rows.RemoveAt(2);

                List<int> panelOrders = val.Keys.OrderBy(x => x).ToList();
                foreach (int panelOrder in panelOrders)
                {
                    itemsTable.Rows.Add(val[panelOrder]);
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }


            //string currentUserName = thisWeb.CurrentUser.Name;
            //int currentUserID = thisWeb.CurrentUser.ID;

            // Redirect on the landing home page according to the user role.
            //string landingHomePage = UGITUtility.GetHomeUrlByUser();
            //if (!string.IsNullOrEmpty(landingHomePage) && !Request.Url.AbsolutePath.ToLower().Contains(landingHomePage.ToLower())) // Check whether we are already on landing page to prevent looping
            //{
            //    string newURL = uHelper.ConcatURLs( UGITUtility.GetAbsoluteURL(landingHomePage), Request.Url.Query);
            //    Response.Redirect(newURL);
            //}

            //moduleCollection = SPListHelper.LoadModuleListCollectionByType(ModuleType.All);
            //userTypeList = SPListHelper.GetSPList(DatabaseObjects.Lists.ModuleUserTypes);

            string moduleType = string.Empty;
            if (!HideSMSModules && HideGovernanceModules)
            {
                moduleType = ModuleType.SMS.ToString();
            }
            else if (!HideGovernanceModules && HideSMSModules)
            {
                moduleType = ModuleType.Governance.ToString();
            }

            // Get list of authorized modules
            //moduleTable = moduleCollection.CopyToDataTable();
            //DataRow[] authorizedModules = new DataRow[0];
            //var authorizedModuleRows = from row in moduleTable.AsEnumerable()
            //                           where (uHelper.IsUserAuthorizedToViewModule(user, row))
            //                           select row;
            //if (authorizedModuleRows.Count() > 0)
            //    authorizedModules = authorizedModuleRows.CopyToDataTable().Select();
            //if (authorizedModules.Length > 0)
            //    authorizedModulesArray = authorizedModules.Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToList();

            // Select module which we need to show SMS or Governance or all
            DataRow[] selectedModules = new DataRow[0];
            if (!HideSMSModules || !HideGovernanceModules)
            {
                //if (moduleType != string.Empty)
                //    selectedModules = authorizedModuleRows.CopyToDataTable().Select(string.Format("{0} = '{1}' and {2} =1", DatabaseObjects.Columns.ModuleType, moduleType, DatabaseObjects.Columns.EnableModule));
                //else
                //    selectedModules = authorizedModules.CopyToDataTable().Select(string.Format("{0} =1", DatabaseObjects.Columns.EnableModule));
            }
            else
            {
                cModulePanel.Visible = false;
            }

            if (selectedModules.Length > 0)
            {
                moduleTable = selectedModules.CopyToDataTable();
                moduleListRepeater.Load += new EventHandler(ModuleListRepeater_Load);
                moduleListRepeater.ItemDataBound += new RepeaterItemEventHandler(ModuleListRepeater_ItemDataBound);
            }
            else
            {
                moduleTable = new DataTable();
            }

            // Get filtered tickets page url
            viewTicketsPath = UGITUtility.GetAbsoluteURL(objConfigurationVariableHelper.GetValue("FilterTicketsPageUrl"));

            // Show/Hide welcome heading and set value
            if (!string.IsNullOrEmpty(WelcomeHeading))
            {
                welcomeHeading.Text = WelcomeHeading.Trim();
            }
            else
            {
                cWelcomeHeadingPanel.Visible = false;
            }

            // Show/Hide welcome description and set value
            if (!string.IsNullOrEmpty(WelcomeDesc))
            {
                welcomeDesciption.Text = WelcomeDesc.Trim();
            }
            else
            {
                cWelcomeDescPanel.Visible = false;
            }
            if (ShowGuideMePanel)
            {
                // Get guide me  title
                if (!string.IsNullOrEmpty(GuideMeTitle))
                {
                    guidmeTitle = GuideMeTitle;
                }
                //Get guide me description
                if (!string.IsNullOrEmpty(GuideMeDescription))
                {
                    guidmeDescription = GuideMeDescription;
                }
            }
            //ShowServiceCatalog = true;
            //if (ShowServiceCatalog)
            //{
            //    cServiceCatalog.Visible = true;
            //    ServiceCatalog serviceCatalog = (ServiceCatalog)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceCatalog.ascx");
            //    panelServiceCatalog.Controls.Add(serviceCatalog);
            //}
            //else
            //{
            //    cServiceCatalog.Visible = false;
            //}
            if (Request["ModuleName"] != null)
            {
                Ticket moduleRequest = new Ticket(context, Request["ModuleName"]);
                if (moduleRequest != null && moduleRequest.Module != null)
                {
                    ticketURL = UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath);
                }
            }
            sourceURL = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit&");
            newServiceURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx");

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Constants.PageTitle.Home;
            //ajaxHelperPage = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ajaxHelperPage);
            if (HideWaitingOnMeTab && HideMyTickets && HideMyTasks && HideMyDepartment && HideMyProject && HideMyClosedTickets)
                cMyTicketPanel.Visible = false;

            // If tabID hidden variable contains tab name then select it, else gets selected tab from cookie
            string mytab = UGITUtility.GetCookieValue(Request, "mytab");
            if (!string.IsNullOrEmpty(tabID.Value))
                mytab = tabID.Value;
            Dictionary<string, int> stat = null;
            //BTS-23-001038: This method executes multiple times for service catalog etc controls added on the page. 
            //To improve the load speed, this check is added so that the LoadAllCount and other time consuming methods do not execute multiple times.
            if (!IsServiceDocPanel)
            {
                ModuleStatistics mod = new ModuleStatistics(context);
                List<string> tabs = new List<string>();
                foreach (Tab item in filterTabHome.Tabs)
                {
                    tabs.Add(item.Name);
                }
                stat = mod.LoadAllCount(tabs, user.Id);

            
                //ddlFilterHome.Items.Insert(0, new ListEditItem("--All--", string.Empty));
                foreach (KeyValuePair<string, int> pair in stat)
                {
                    int myCount = pair.Value;
                    string tabName = pair.Key;
                    if (myCount > 0)
                    {
                        Tab tab = filterTabHome.Tabs.FindByName(pair.Key) as Tab;
                        ddlFilterHome.Items.Add(string.Format("{0} ({1})", tab.Text, myCount), pair.Key);
                        tab.Text = string.Format("{0} ({1})", tab.Text, myCount);

                    }
                    else
                    {
                    
                        Tab tab = filterTabHome.Tabs.FindByName(pair.Key) as Tab;
                        tab.Visible = false;
                    
                    }
                }
                if (string.IsNullOrEmpty(mytab) && ddlFilterHome.Items.Count > 0)
                {
                    mytab = Convert.ToString(ddlFilterHome.Items[0].Value);

                }

                if (ddlFilterHome.Items.Count > 0)
                {
                    ddlFilterHome.Visible = true;

                }

                else
                {
                    ddlFilterHome.Visible = false;

                }
           
                ddlFilterHome.SelectedIndex = ddlFilterHome.Items.IndexOfValue(mytab);

                if (ddlFilterHome.Items.Count > 0 && ddlFilterHome.SelectedIndex == -1)
                    ddlFilterHome.SelectedIndex = 0;

                if (!HideSMSModules || !HideGovernanceModules)
                {
                    ModuleStatisticRequest sRequest = new ModuleStatisticRequest();
                    sRequest.ModuleName = string.Empty;
                    sRequest.ModuleType = ModuleType.All;
                    if (!HideSMSModules && HideGovernanceModules)
                        sRequest.ModuleType = ModuleType.SMS;
                    else if (HideSMSModules && !HideGovernanceModules)
                        sRequest.ModuleType = ModuleType.Governance;

                    //sRequest.UserID = SPContext.Current.Web.CurrentUser.ID;
                    //sRequest.Tabs.Add(CustomFilterTab.MyOpenTickets);
                    //sRequest.Tabs.Add(CustomFilterTab.OpenTickets);
                    //sRequest.SPWebObj = SPContext.Current.Web;
                    //moduleStats = ModuleStatistics.LoadAll(sRequest);
                }

                waitingonmeLinkTab = filterTabHome.Tabs.FindByName("waitingonme");
                myrequestsLinkTab = filterTabHome.Tabs.FindByName("myopentickets");
                mytaskLinkTab = filterTabHome.Tabs.FindByName("mytask");
                departmentTicketLinkTab = filterTabHome.Tabs.FindByName("mydepartmentticket");
                myprojectLinkTab = filterTabHome.Tabs.FindByName("myproject");
                documentpendingapproveLinkTab = filterTabHome.Tabs.FindByName("documentpendingapprove");
                myclosedrequestsLinkTab = filterTabHome.Tabs.FindByName("myclosedtickets");
                divisionTicketLinkTab = filterTabHome.Tabs.FindByName("mydivision");
                //if (!string.IsNullOrEmpty(UpdateWaitingOnMeTab) && waitingonmeLinkTab != null)
                //    waitingonmeLinkTab.Text = UpdateWaitingOnMeTab;
                //if (!string.IsNullOrEmpty(UpdateMyTickets) && myrequestsLinkTab != null)
                //    myrequestsLinkTab.Text = UpdateMyTickets;
                //if (!string.IsNullOrEmpty(UpdateMyTasks) && mytaskLinkTab != null)
                //    mytaskLinkTab.Text = UpdateMyTasks;
                //if (!string.IsNullOrEmpty(UpdateMyPendingApprovals) && documentpendingapproveLinkTab != null)
                //    documentpendingapproveLinkTab.Text = UpdateMyPendingApprovals;
                //if (!string.IsNullOrEmpty(UpdateMyDepartmentTickets) && departmentTicketLinkTab != null)
                //    departmentTicketLinkTab.Text = UpdateMyDepartmentTickets;
                //if (!string.IsNullOrEmpty(UpdateMyProject) && myprojectLinkTab != null)
                //    myprojectLinkTab.Text = UpdateMyProject;
                if (!string.IsNullOrEmpty(UpdateMyClosedTickets) && myclosedrequestsLinkTab != null)
                    myclosedrequestsLinkTab.Text = UpdateMyClosedTickets;


                //hide unhide tabs based on dock panel properties
                if (HideWaitingOnMeTab && ddlFilterHome.Items.IndexOfValue("waitingonme") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("waitingonme"));
                if (HideMyTickets && ddlFilterHome.Items.IndexOfValue("myopentickets") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("myopentickets"));
                if (HideMyTasks && ddlFilterHome.Items.IndexOfValue("mytask") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("mytask"));
                if (HideMyPendingApprovals && ddlFilterHome.Items.IndexOfValue("documentpendingapprove") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("documentpendingapprove"));
                if (HideMyDepartment && ddlFilterHome.Items.IndexOfValue("mydepartmentticket") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("mydepartmentticket"));
                if (HideMyProject && ddlFilterHome.Items.IndexOfValue("myproject") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("myproject"));
                if (HideMyClosedTickets && ddlFilterHome.Items.IndexOfValue("myclosedtickets") >= 0)
                    ddlFilterHome.Items.RemoveAt(ddlFilterHome.Items.IndexOfValue("myclosedtickets"));

                // Display the Help Text link.
                ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(context);
                autoRefreshFrequency = UGITUtility.StringToInt(objConfigurationVariableHelper.GetValue("AutoRefreshFrequency"));            
            }
            tabID.Value = mytab;

            //ShowServiceCatalog = true;
            if (ShowServiceCatalog)
            {
                cServiceCatalog.Visible = true;

                ServiceCatalog serviceCatalog = (ServiceCatalog)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceCatalog.ascx");
                serviceCatalog.ShowServiceIcons = this.ShowServiceIcons;
                serviceCatalog.ServiceCatalogViewMode = this.ServiceCatalogViewMode;
                serviceCatalog.IconSize = this.IconSize;
                panelServiceCatalog.Controls.Add(serviceCatalog);
            }
            else
            {
                cServiceCatalog.Visible = false;
            }

            if (objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.DisableCustomFilterTab))
            {
                ddlFilterHome.ClientVisible = true;
                filterTabHome.ClientVisible = false;
            }
            else
            {
                ddlFilterHome.ClientVisible = false;
                filterTabHome.ClientVisible = true;
            }
        }

        protected void ModuleListRepeater_Load(object sender, EventArgs e)
        {
            moduleTable.Columns.Add("TotalTickets", typeof(string));
            moduleTable.Columns.Add("MyTickets", typeof(string));

            DataTable modules = moduleTable; //moduleCollection.GetDataTable();

            if (modules != null && modules.Rows.Count > 0)
            {
                //DataView moduleView = modules.DefaultView;
                //moduleView.RowFilter = string.Format("{0} <> 'SVC'", DatabaseObjects.Columns.ModuleName);
                //moduleView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                //modules = moduleView.ToTable();
            }

            for (int i = 0; i < modules.Rows.Count; i++)
            {
                //SPListItem module = SPListHelper.GetItemByID(moduleCollection, Convert.ToInt32(modules.Rows[i][DatabaseObjects.Columns.Id]));
                //if (module != null && Convert.ToString(module[DatabaseObjects.Columns.ModuleName]) != "SVC")
                //{
                //    if (module.Attachments.Count > 0)
                //    {
                //        modules.Rows[i]["Attachments"] = module.Attachments.UrlPrefix + module.Attachments[0];
                //    }
                //    else
                //    {
                //        string defaultPath = "/Content/images/uGovernIT/" + modules.Rows[i][DatabaseObjects.Columns.ModuleName].ToString() + "_32x32.png";
                //        modules.Rows[i]["Attachments"] = defaultPath;
                //    }

                //    if (moduleStats != null)
                //    {
                //        ModuleStatisticResponse mStat = moduleStats.FirstOrDefault(x => x.ModuleName == Convert.ToString(module[DatabaseObjects.Columns.ModuleName]));
                //        if (mStat != null && mStat.TabCounts != null)
                //        {
                //            int totalTickets = 0;
                //            int mytickets = 0;
                //            mStat.TabCounts.TryGetValue(CustomFilterTab.OpenTickets, out totalTickets);
                //            mStat.TabCounts.TryGetValue(CustomFilterTab.MyOpenTickets, out mytickets);
                //            modules.Rows[i]["TotalTickets"] = totalTickets;
                //            modules.Rows[i]["MyTickets"] = mytickets;
                //        }
                //    }
                //}
            }

            moduleListRepeater.DataSource = modules;
            moduleListRepeater.DataBind();
        }

        protected void ModuleListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DataRowView rowView = (DataRowView)e.Item.DataItem;
                //RepeaterItem rItem = e.Item;
                //string moduleName = rowView[DatabaseObjects.Columns.ModuleName].ToString();
                ////ModuleTheme mTheme = ModuleTheme.LoadByThemeColor(moduleName, rowView[DatabaseObjects.Columns.ThemeColor].ToString());

                //HtmlGenericControl contentDiv = (HtmlGenericControl)rItem.FindControl("contentDetailDiv");
                //LinkButton openTicketLink = (LinkButton)rItem.FindControl("openTicketLink");
                //LinkButton myTicketLink = (LinkButton)rItem.FindControl("myTicketLink");

                //openTicketLink.CssClass = "followedhyperlinkdarkest";
                //myTicketLink.CssClass = "followedhyperlinkdarkest";
                ////contentDiv.Attributes.Add("class", mTheme.MHomePanelBgClass + " mcontentdiv");

                //openTicketLink.Text = string.Format("Open Tickets ({0})", rowView["TotalTickets"]);
                //myTicketLink.Text = string.Format("My Tickets ({0})", rowView["MyTickets"]);
                //openTicketLink.Attributes.Add("href", "javascript:");
                //myTicketLink.Attributes.Add("href", "javascript:");

                //string title = string.Empty;
                //if (moduleName == "INC")
                //    title = string.Format("{0}s", uHelper.moduleTypeName(moduleName));
                //else
                //    title = string.Format("{0} {1}s", moduleName, uHelper.moduleTypeName(moduleName));

                //openTicketLink.OnClientClick = string.Format("event.cancelBubble = true;UgitOpenPopupDialog('{0}', 'Module={1}&UserType=All&Status=Open&showalldetail=true&showFilterTabs=false', 'Open {2}', 90, 90, 0)", viewTicketsPath, moduleName, title);
                //myTicketLink.OnClientClick = string.Format("event.cancelBubble = true;UgitOpenPopupDialog('{0}', 'Module={1}&UserType=my&Status=Open&showalldetail=true&showFilterTabs=false', 'My {2}', 90, 90, 0)", viewTicketsPath, moduleName, title);


                ////Check the login user if is in the Initiator group then show the "NEW" button to create new tickets.
                //#region "NewButtonHandler"
                //Panel newbtnPanel = (Panel)rItem.FindControl("btnNewPanel");
                //if (!UserProfile.CheckUserIsInGroup(moduleName, DatabaseObjects.Columns.TicketInitiator, SPContext.Current.Web.CurrentUser.ID))
                //{
                //    newbtnPanel.Visible = false;
                //}
                //else
                //{
                //    DataTable ModulUserTypesTable = uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleUserTypes);

                //    string query = string.Format("{0}='{1}' And  {2}= '{3}' ", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.ColumnName, DatabaseObjects.Columns.TicketInitiator);
                //    DataRow[] moduleUserTypes = ModulUserTypesTable.Select(query);
                //    UserProfile user = UserProfile.LoadById(Convert.ToInt32(SPContext.Current.Web.CurrentUser.ID));
                //    if (moduleUserTypes.Length > 0)
                //    {
                //        if (!UserProfile.CheckUserISManager(moduleUserTypes[0], user))
                //        {
                //            newbtnPanel.Visible = false;
                //        }
                //    }
                //}

                //#endregion

                //Panel btnQuickTicket = (Panel)rItem.FindControl("btnQuickTicket");
                //if (moduleName != string.Empty && Convert.ToString(uGITCache.GetModuleDetails(moduleName)["EnableQuickTicket"]) == "1")
                //    btnQuickTicket.Visible = true;
                //else
                //    btnQuickTicket.Visible = false;
            }
        }

        protected void PopupControl_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            //if (!string.IsNullOrEmpty(e.Parameter))
            //    BindTemplateGrid(e.Parameter);
        }

        private void BindTemplateGrid(string moduleName)
        {
            //if (!PopupControl.JSProperties.ContainsKey("cpTicketUrl"))
            //    PopupControl.JSProperties.Add("cpTicketUrl", false);

            //if (moduleTable != null && moduleTable.Rows.Count > 0)
            //{
            //    DataRow[] dr = moduleTable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleName, moduleName));
            //    if (dr != null && dr.Length > 0)
            //        PopupControl.JSProperties["cpTicketUrl"] =  UGITUtility.GetAbsoluteURL(Convert.ToString(dr[0][DatabaseObjects.Columns.ModuleRelativePagePath]));
            //}

            //DataTable FieldTable = new DataTable();
            //DataTable dtResult = SPListHelper.GetDataTable(DatabaseObjects.Lists.TicketTemplates, SPContext.Current.Web);
            //if (dtResult != null && dtResult.Rows.Count > 0)
            //{
            //    DataRow[] drRows = dtResult.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, moduleName));
            //    if (drRows != null && drRows.Length > 0)
            //    {
            //        FieldTable = drRows.CopyToDataTable();
            //    }
            //    else
            //    {
            //        FieldTable = null;
            //    }
            //}

            //gridTemplate.DataSource = FieldTable;
            //gridTemplate.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (filterTabHome.Tabs.GetVisibleTabCount() == 0)
            {
                cMyTicketPanel.Visible = true;
                Tab tab = filterTabHome.Tabs.FindByName(FilterTab.myopentickets);
                if (tab != null)
                    tab.Visible = true;
            }
            base.OnPreRender(e);
        }

        protected void tabRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TabView dataView = e.Item.DataItem as TabView;
            string tabName = Convert.ToString(dataView.TabName);
            Guid allocationFrameID = Guid.NewGuid();
            string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={0}&control={1}&NoOfPreviewTickets={2}&source={3}&enablenewbutton={4}&IsServiceDocPanel={5}&ShowBandedRows={6}&ShowCompactRows={7}"
                    , allocationFrameID, tabName, NoOfPreviewTickets, Uri.EscapeDataString(Request.Url.AbsolutePath), EnableNewButton, IsServiceDocPanel, ShowBandedRows, ShowCompactRows));
            LiteralControl lCtr = new LiteralControl(string.Format("<iframe onload='callAfterFrameLoad(this)' style='margin-left:2px;background-color:#f6f7fb !important' marginwidth='0' marginheight='0' scrolling='no' frame-url='{0}' width='100%' height='100' frameborder='0' id='{1}' ></iframe>", url, allocationFrameID));
            ((Panel)e.Item.FindControl("tabPanel")).Controls.Add(lCtr);
        }


        protected void DashboardPreview_PreRender(object sender, EventArgs e)
        {
            if (ShowPanel)
            {
                searchPanel.Attributes.Add("class", "searchPannelWrap");
                homecontent.Attributes.Add("class", "homecontent homecontent-fromServices");
                DashboardManager dManager = new DashboardManager(context);
                long dashboardID = PanelId; //2529;
                Dashboard uDashboard = null;
                if (dashboardID > 0)
                {
                    uDashboard = dManager.LoadPanelById(dashboardID, true);
                }
                //return if dashboard not exist
                if (uDashboard == null)
                {
                    return;
                }

                CustomDashboardPanel panel = (CustomDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomDashboardPanel.ascx");
                panel.dashboard = uDashboard;
                panel.UseAjax = true;
                panel.IsServiceDocPanel = true;
                dashboardPreview.Controls.Add(panel);

            }
            else
            {
                dashboardPreview.Visible = false;
                searchPanel.Attributes.Add("class", "searchPannelWrap col-md-12");
            }

        }

    }
}
