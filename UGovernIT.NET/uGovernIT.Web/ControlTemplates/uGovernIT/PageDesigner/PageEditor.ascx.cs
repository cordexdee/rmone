using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Linq;
using DevExpress.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Reflection;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.DockPanels;
using System.Xml;
using uGovernIT.Web.Helpers;
using uGovernIT.Manager.Managers;
using System.Threading;
using uGovernIT.Web.ControlTemplates.DockPanels.DockPanelEditControl;
using System.Text.RegularExpressions;
using uGovernIT.DefaultConfig.Data.DefaultData;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class PageEditor : UserControl
    {
        int selectedPageID;
        protected int RowIndex;
        //DataTable list;
        //DataRow selectedPageItem;
        //// SPWeb spWeb;
        //string leftNavZone = "LeftColumn";
        protected string selectedPageUrl;
        protected string pagename;
        //Control abcd;
        PageConfigurationManager objPageConfigurationManager = new PageConfigurationManager(HttpContext.Current.GetManagerContext());
        DashboardPanelViewManager objDashboardPanelViewManager;
        PageConfiguration objPageConfiguration=null;
        public string pageTitle = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
            this.testPanel.Controls.Clear();
            bindSitePages();
            BindMenuTypeDropDwn();

            if (Request["SelectedPage"]!=null)
            {
                selectedPageID = UGITUtility.StringToInt(Request["SelectedPage"]);
                //selectedPageID = UGITUtility.StringToInt(hdnSetting.Get("SelectedPage"));
                objPageConfiguration = objPageConfigurationManager.LoadByID(selectedPageID);
                if (objPageConfiguration == null)
                    return;
     
                selectedPageUrl = UGITUtility.GetAbsoluteURL(string.Format("/"+ objPageConfiguration.RootFolder + "/{0}", objPageConfiguration.Name));
                pagename = objPageConfiguration.Name;
                //grdvPageList.MakeRowVisible(selectedPageID);
                int index = grdvPageList.FindVisibleIndexByKeyValue(selectedPageID);
                RowIndex = index;
                //grdvPageList.ScrollToVisibleIndexOnClient = index;
                grdvPageList.Selection.SetSelection(index, true);
                LoadMasterContent_SelectedPage();
            }
            
            GenerateLayout();
            if (selectedPageID > 0)
            {
               
            }

            if (hdnSetting.Contains("EditWebpartIndex"))
            {
                LoadWebpartProperpties();
            }
        }
        public void GenerateLayout()
        {
            objPageConfiguration = objPageConfigurationManager.LoadByID(selectedPageID);
            if (objPageConfiguration != null)
            {
                List <DockPanelSetting> setting = new List<DockPanelSetting>();
                if (!string.IsNullOrEmpty(objPageConfiguration.ControlInfo))
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(objPageConfiguration.ControlInfo);
                    setting = uHelper.DeSerializeAnObject(document, setting) as List<DockPanelSetting>;
                }
                if (setting != null && setting.Count > 0)
                {
                    foreach (DockPanelSetting controlClassName in setting)
                    {
                        object obj = null; 
                        if (controlClassName != null && !string.IsNullOrEmpty(controlClassName.AssemblyName))
                        {
                            Type type = Type.GetType(controlClassName.AssemblyName);
                            obj = Activator.CreateInstance(type);
                            PropertyInfo propertyInfo = type.GetProperty("Editable");
                            propertyInfo.SetValue(obj, true);
                            PropertyInfo propertyPageID = type.GetProperty("PageID");
                            propertyPageID.SetValue(obj,Convert.ToString(objPageConfiguration.ID)) ;
                            PropertyInfo propertyDockSetting = type.GetProperty("DockSetting");
                            propertyDockSetting.SetValue(obj, controlClassName);                          
                            
                        }
                        if (obj != null)
                        {
                            Control ctrl = obj as Control;
                            this.testPanel.Controls.Add(ctrl);
                        }
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
            txtPageHeading.Text = pagename;
            if (string.IsNullOrEmpty(pagename))
                pageHeaderContainer.Visible = txtPageHeading.Visible = false;
            else
                pageHeaderContainer.Visible = txtPageHeading.Visible = true;

            GetWebpartList();

            base.OnPreRender(e);
        }

        private void bindSitePages()
        {
            List<PageConfiguration> lstPageConfiguration = objPageConfigurationManager.Load().OrderBy(x=>x.Title).ToList();
            if (lstPageConfiguration.Count > 0)
            {
                grdvPageList.DataSource = lstPageConfiguration;
                grdvPageList.DataBind();
            }
        }

        private static string CreateWebPartPage(DataTable list, string pageTitle, int layoutTemplate)
        {
            string value = string.Empty;
            //const string newItemTemplate = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            //                                "<Method ID=\"0,NewWebPage\">" +
            //                                "<SetList Scope=\"Request\">{0}</SetList>" +
            //                                "<SetVar Name=\"Cmd\">NewWebPage</SetVar>" +
            //                                "<SetVar Name=\"ID\">New</SetVar>" +
            //                                "<SetVar Name=\"Type\">WebPartPage</SetVar>" +
            //                                "<SetVar Name=\"WebPartPageTemplate\">{2}</SetVar>" +
            //                                "<SetVar Name=\"Overwrite\">true</SetVar>" +
            //                                "<SetVar Name=\"Title\">{1}</SetVar>" +
            //                                "</Method>";
            //var newItemXml = string.Format(newItemTemplate, list.ID, pageTitle, layoutTemplate);
            //return list.ParentWeb.ProcessBatchData(newItemXml);
            return value;
        }

        protected void btAddNewPage_Click(object sender, EventArgs e)
        {
            PageConfiguration Configuration = new PageConfiguration();
            //SPList pagesLibrary = SPContext.Current.Web.Lists.TryGetList("Site Pages");
            //if (pagesLibrary == null)
            //    return;

            if (txtNewPage.Text.Trim() == string.Empty)
                return;

            string pageTitle = txtNewPage.Text;
            if (pageTitle.EndsWith(".aspx", StringComparison.CurrentCultureIgnoreCase))
                pageTitle = pageTitle.Substring(0, pageTitle.LastIndexOf(".aspx"));
            Configuration.Title = pageTitle;
            Configuration.Name = pageTitle;
            if (pageTitle == "RequestList" || pageTitle == "Request")
                Configuration.RootFolder = "SitePages";
            else
                Configuration.RootFolder = "Pages";
            objPageConfigurationManager.Insert(Configuration);
            // string result = CreateWebPartPage(pagesLibrary, pageTitle, 3);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Added page: {Configuration.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);

            txtNewPage.Text = string.Empty;

            bindSitePages();

            List<PageConfiguration> dataPageConfiguration = grdvPageList.DataSource as List<PageConfiguration>;
            if (dataPageConfiguration == null)
                return;
            Configuration = dataPageConfiguration.AsEnumerable().FirstOrDefault(x => x.Title ==pageTitle);
            // DataRow dataRow = dataPageConfiguration.AsEnumerable().FirstOrDefault(x => x.Field<string>("FileLeafRef") == string.Format("{0}.aspx", pageTitle));
            if (Configuration == null)
                return;

            selectedPageID = UGITUtility.StringToInt(Configuration.ID);

            hdnSetting.Remove("SelectedPage");
            hdnSetting.Set("SelectedPage", selectedPageID);

            grdvPageList.Selection.SelectRowByKey(selectedPageID);

            grdvPageList.MakeRowVisible(selectedPageID);
            int index = grdvPageList.FindVisibleIndexByKeyValue(selectedPageID);
            grdvPageList.ScrollToVisibleIndexOnClient = index;
            Configuration = objPageConfigurationManager.LoadByID(selectedPageID);
            //selectedPageItem = SPListHelper.GetSPListItem(list, selectedPageID);
            selectedPageUrl = UGITUtility.GetAbsoluteURL(string.Format("/SitePages/{0}", Configuration.Name));
            //GenerateLayout();
            LoadMasterContent_SelectedPage();
            Response.Redirect("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview&isudlg=1&pageTitle=Page+Editor&SelectedPage="+Configuration.ID);

        }

        protected void ddlDashboardGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDashboardGroup.SelectedItem != null && ddlDashboardGroup.SelectedIndex >= 0)
            {
                bindgroupdropdown(ddlDashboardGroup.SelectedItem.Text);
            }
            else
            {
                ddlDashboardView.Items.Clear();
                ddlDashboardView.Items.Insert(0, new ListEditItem("Please select", string.Empty));
            }
        }

        private void bindgroupdropdown(string ViewType)
        {
            ddlDashboardView.Items.Clear();
            ddlDashboardView.SelectedIndex = -1;

            //DataTable dashboardViewList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanelView);
            DataTable dashboardViewList = UGITUtility.ToDataTable(objDashboardPanelViewManager.Load());
            if (dashboardViewList != null && dashboardViewList.Rows.Count > 0)
            {
                DataRow[] collection = dashboardViewList.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.UGITViewType, ViewType.ToLower()));
                if (collection.Count() > 0)
                {
                    dashboardViewList = null;
                    dashboardViewList = collection.CopyToDataTable();
                    dashboardViewList.DefaultView.Sort = DatabaseObjects.Columns.Title;
                    foreach (DataRow row in dashboardViewList.DefaultView.ToTable().Rows)
                    {
                        ddlDashboardView.Items.Add(new ListEditItem(Convert.ToString(row[DatabaseObjects.Columns.Title])));
                    }
                    ddlDashboardView.Items.Insert(0, new ListEditItem("Please select", string.Empty));
                }
            }
            //SPContext.Current.Web.Lists[DatabaseObjects.Lists.DashboardPanelView];
            //SPQuery query = new SPQuery();
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            //query.ViewFieldsOnly = true;
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.UGITViewType, ViewType.ToLower());
            //DataTable dashboardViewTable = SPListHelper.GetDataTable(DatabaseObjects.Lists.DashboardPanelView, query);
            //if (dashboardViewTable != null && dashboardViewTable.Rows.Count > 0)
            //{
            //    dashboardViewTable.DefaultView.Sort = DatabaseObjects.Columns.Title;
            //    foreach (DataRow row in dashboardViewTable.DefaultView.ToTable().Rows)
            //    {
            //        ddlDashboardView.Items.Add(new ListEditItem(Convert.ToString(row[DatabaseObjects.Columns.Title])));
            //    }
            //}
            //ddlDashboardView.Items.Insert(0, new ListEditItem("Please select", string.Empty));
        }

        protected void chkShowHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkShowHeader.Checked)
                ddlMenuList.Visible = false;
            else
            {
                ddlMenuList.Visible = true;
                if (ddlMenuList.SelectedIndex == 1)
                    ddlMenuList.SelectedIndex = ddlMenuList.Items.IndexOfValue("Default");
            }
            //ShowHeaderOptions();
            SetMasterContent_SelectedPage();
        }

        private void AddWebpartPage()
        {
            //SPWeb spWeb = SPContext.Current.Web;
            //SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
            //Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);

            //uGovernITTopMenuBar menuWebpart = new uGovernITTopMenuBar();
            //menuWebpart.HideTopMenu = false;
            //menuWebpart.HideFooter = true;
            //menuWebpart.MenuType = "Default";
            //manager.AddWebPart(menuWebpart, "Body", 0);
        }

        protected void btCreateWebpart_Click(object sender, EventArgs e)
        {
            AddWebpartPage();
        }

        private static DataTable GetWebpardsFromPage(DataTable pageList, int pageID, bool includeHeader = false, bool includeLeftNav = false)
        {
            DataTable dt = new DataTable();
            //DataTable webparts = new DataTable();
            //webparts.Columns.Add("Title", typeof(string));
            //webparts.Columns.Add("Type", typeof(string));
            //webparts.Columns.Add("Index", typeof(int));
            //webparts.Columns.Add("Description", typeof(string));
            //webparts.Columns.Add("UniqueID", typeof(string));
            //webparts.Columns.Add("TitleWithType", typeof(string));

            //SPWeb spWeb = SPContext.Current.Web;
            //SPListItem item = SPListHelper.GetSPListItem(pageList, pageID);
            //if (item == null)
            //    return null;
            //Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);
            //foreach (Microsoft.SharePoint.WebPartPages.WebPart webPart in manager.WebParts)
            //{
            //    if (webPart.Hidden)
            //        continue;

            //    if (!includeHeader && webPart is uGovernITTopMenuBar ||
            //        !includeLeftNav && webPart is uGovernITSideBar ||
            //        webPart is TitleBarWebPart)
            //    {
            //        continue;
            //    }

            //    if (webPart.ZoneID != "Body" && webPart.ZoneID != "FullPage")
            //        continue;



            //    DataRow row = webparts.Rows.Add(webPart.Title, webPart.GetType().Name, webPart.ZoneIndex);
            //    row["Description"] = string.Empty;
            //    row["UniqueID"] = webPart.UniqueID;
            //    row["TitleWithType"] = webPart.Title;
            //    if (webPart.Title != webPart.GetType().Name)
            //    {
            //        row["TitleWithType"] = string.Format("{0} ({1})", webPart.Title, webPart.GetType().Name);
            //    }

            //}

            //webparts.DefaultView.Sort = "Index asc";
            //return webparts.DefaultView.ToTable();
            return dt;
        }

        private DataTable GetWebpartDataTable()
        {
            DataTable webparts = new DataTable();
            List<Type> tlist = new List<Type>();
            List<Type> types = typeof(IDockPanel).GetAllDerivedTypes();
            webparts.Columns.Add("Title", typeof(string));
            webparts.Columns.Add("Name", typeof(string));
            types.ForEach(x =>
            {
                if (!x.Name.Equals("DockPanel", StringComparison.InvariantCultureIgnoreCase))
                {
                    DataRow dr = webparts.NewRow();
                    dr[0] = x.FullName;
                    dr[1] = x.Name;
                    webparts.Rows.Add(dr);
                }
            });
            //DataTable data = webpartList.GetItems(query).GetDataTable();

            ////exclude webpart which we don't want to show option to add as content
            //List<string> excludedWebparts = new List<string>();
            //excludedWebparts.Add("uGovernITSideBar");
            //excludedWebparts.Add("uGovernITTopMenuBar");
            //excludedWebparts.Add("uGovernITAdmin");

            //foreach (DataRow webpart in data.Rows)
            //{
            //    if (excludedWebparts.Contains(Convert.ToString(webpart[DatabaseObjects.Columns.Title])))
            //        continue;

            //    webparts.Rows.Add(Convert.ToString(webpart[DatabaseObjects.Columns.Title]));
            //}
            return webparts;
        }

        private void GetWebpartList()
        {
            DataTable webparts = GetWebpartDataTable();
            DataView view = webparts.DefaultView;
            view.Sort = string.Format("{0} asc", DatabaseObjects.Columns.Title);
            DataTable sortdt = view.ToTable(true);
            webparts = sortdt;
            acvWebparts.DataSource = webparts;
            acvWebparts.DataBind();
        }

        protected void btAddWebpartOnPage_Click(object sender, EventArgs e)
        {
            List<DockPanelSetting> settings = new List<DockPanelSetting>();
            if (!string.IsNullOrEmpty(objPageConfiguration.ControlInfo))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(objPageConfiguration.ControlInfo);
                settings = uHelper.DeSerializeAnObject(document, settings) as List<DockPanelSetting>;
                if (settings == null)
                    settings = new List<DockPanelSetting>();
            }            
            if (!hdnSetting.Contains("SelectedWebpartName"))
                return;
            string webpart = hdnSetting.Get("SelectedWebpartName") as string;
            hdnSetting.Remove("SelectedWebpartName");
            object obj = null;
            if (string.IsNullOrWhiteSpace(webpart))
            {
                return;
            }            
            Type type = Type.GetType(webpart);
            obj = Activator.CreateInstance(type);
            DockPanel dock = obj as DockPanel;
            if (type.Name == "TicketDockPanel")
            {
                dock.DockSetting = new TicketDockPanelSetting();
            }
            else if (type.Name == "DashboardDockPanel")
            {
                dock.DockSetting = new DashboardDockPanelSetting();
            }
            else if (type.Name == "ModuleWebPartDockPanel")
            {
                dock.DockSetting = new ModuleWebpartDockPanelSetting();
            }
            else if (type.Name == "ApplicationUpTimeDashBoardDockPanel")
            {
                dock.DockSetting = new Applicationuptimesetting();
            }
            else if (type.Name == "HomeDockPanel")
            {
                dock.DockSetting = new HomeDockPanelSetting();
            }
            else if (type.Name == "RMMDockPanel")
            {
                dock.DockSetting = new RMMDockPanelSetting();
            }
            else if (type.Name == "MessageBoardDockPanel")
            {
                dock.DockSetting = new MessageboardDockPanelSetting();
            }
            else if (type.Name == "DashboardSLADockPanel")
            {
                dock.DockSetting = new DashboardSLADockPanelSetting();
            }
            else if (type.Name == "DashboardReportDockPanel")
            {
                dock.DockSetting = new DashboardReportPanelSetting();
            }
            else if (type.Name == "ServiceDockPanel")
            {
                dock.DockSetting = new ServiceDockPanelSetting();
            }
            if (type.Name == "TicketCountTrendsDocPanel")
            {
                dock.DockSetting = new TicketCountTrendsDocPanelSetting();
            }

            else if (type.Name == "TaskDockPanel")
            {
                dock.DockSetting = new TaskDockPanelSetting();
            }
            else if (type.Name == "LinkDockPanel")
            {
                dock.DockSetting = new LinkPanelSetting();
            }

            dock.DockSetting.AssemblyName = obj.GetType().FullName;
            dock.DockSetting.ControlID = Guid.NewGuid().ToString() + "_DockPanel_";
            settings.Add(dock.DockSetting);            
            objPageConfiguration.ControlInfo = uHelper.SerializeObject(settings).OuterXml;
            objPageConfigurationManager.Update(objPageConfiguration);
            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Update webpart property : {type.Name}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
            Response.Redirect(HttpContext.Current.Request.Url.ToString());
            //GenerateLayout();
        }       
        private void LoadWebpartProperpties()
        {
            webpartPropsHolder.Controls.Clear();
            string val = Convert.ToString(hdnSetting.Get("EditWebpartIndex"));            
            if (!string.IsNullOrEmpty(val))
            {                
                string[] parameterList = val.Split('|');
                objPageConfiguration = objPageConfigurationManager.Get(x => x.ID == Convert.ToInt64(parameterList[1].ToString()));
                if (objPageConfiguration != null && objPageConfiguration.ControlInfo != null)
                {
                    List<DockPanelSetting> setting = new List<DockPanelSetting>();
                    if (!string.IsNullOrEmpty(objPageConfiguration.ControlInfo))
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(objPageConfiguration.ControlInfo);
                        setting = uHelper.DeSerializeAnObject(document, setting) as List<DockPanelSetting>;
                    }
                    if (setting != null && setting.Count > 0)
                    {
                        DockPanelSetting dockPanel = setting.FirstOrDefault(x => x.ControlID == Convert.ToString(parameterList[0]));
                        //object obj = null;
                        if (dockPanel != null && !string.IsNullOrEmpty(dockPanel.AssemblyName))
                        {
                            switch (dockPanel.AssemblyName)
                            {
                                case "uGovernIT.Web.ControlTemplates.DockPanels.TicketDockPanel":
                                    {
                                        FilterTicketsProperties filterTicketsProperties = (FilterTicketsProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/FilterTicketsProperties.ascx");
                                        filterTicketsProperties.ticketDockPanelSetting = dockPanel as TicketDockPanelSetting;
                                        filterTicketsProperties.ticketDockPanelSetting.Name = objPageConfiguration.Name;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.DashboardDockPanel":
                                    {
                                        DashboardProperties filterTicketsProperties = (DashboardProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/DashboardProperties.ascx");
                                        filterTicketsProperties.dashboardDockPanelSetting = dockPanel as DashboardDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.ModuleWebPartDockPanel":
                                    {
                                        ModuleWebpartProperties filterTicketsProperties = (ModuleWebpartProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/ModuleWebpartProperties.ascx");
                                        filterTicketsProperties.moduleDockPanelSetting = dockPanel as ModuleWebpartDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.RMMDockPanel":
                                    {
                                        RMMWebpartProperties filterTicketsProperties = (RMMWebpartProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/RMMWebpartProperties.ascx");
                                        filterTicketsProperties.rmmDockPanelSetting = dockPanel as RMMDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.HomeDockPanel":
                                    {
                                        HomeWebpartProperties filterTicketsProperties = (HomeWebpartProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/HomeWebpartProperties.ascx");
                                        filterTicketsProperties.homeDockPanelSetting = dockPanel as HomeDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.MessageBoardDockPanel":
                                    {
                                        MessageboardProperties filterTicketsProperties = (MessageboardProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/MessageboardProperties.ascx");
                                        filterTicketsProperties.messageDockPanelSetting = dockPanel as MessageboardDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.DashboardSLADockPanel":
                                    {
                                        SLADashboardProperties filterTicketsProperties = (SLADashboardProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/SLADashboardProperties.ascx");
                                        filterTicketsProperties.dashboardSlaDockPanelSetting = dockPanel as DashboardSLADockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.TaskDockPanel":
                                    {
                                      TaskProperties filterTicketsProperties = (TaskProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/TaskProperties.ascx");
                                        filterTicketsProperties.taskDockPanelSetting = dockPanel as TaskDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.DashboardReportDockPanel":
                                    {
                                        DashboardReportProperties filterTicketsProperties = (DashboardReportProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/DashboardReportProperties.ascx");
                                        filterTicketsProperties.dashboardReportPanelSetting = dockPanel as DashboardReportPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.ServiceDockPanel":
                                    {
                                        ServiceWebpartProperties filterTicketsProperties = (ServiceWebpartProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/ServiceWebpartProperties.ascx");
                                        filterTicketsProperties.serviceDockPanelSetting= dockPanel as ServiceDockPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.TicketCountTrendsDocPanel":
                                    {
                                        TicketCountTrendsProperties ticketCountTrendsProperties = (TicketCountTrendsProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/TicketCountTrendsProperties.ascx");
                                        ticketCountTrendsProperties.TicketCountTrendsDocPanelSetting = dockPanel as TicketCountTrendsDocPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            ticketCountTrendsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(ticketCountTrendsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                case "uGovernIT.Web.ControlTemplates.DockPanels.ApplicationUpTimeDashBoardDockPanel":
                                    {
                                        ApplicationUptimeProperties filterTicketsProperties = (ApplicationUptimeProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/ApplicationUptimeProperties.ascx");
                                        filterTicketsProperties.moduleDockPanelSetting = dockPanel as Applicationuptimesetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;

                                case "uGovernIT.Web.ControlTemplates.DockPanels.LinkDockPanel":
                                    {
                                        LinkProperties filterTicketsProperties = (LinkProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/LinkProperties.ascx");
                                        filterTicketsProperties.linkPanelSetting = dockPanel as LinkPanelSetting;
                                        if (editWebpartPropertiesPopup != null)
                                        {
                                            filterTicketsProperties.ID = parameterList[0] + "_props";
                                            webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                            editWebpartPropertiesPopup.ShowOnPageLoad = true;
                                        }
                                    }
                                    break;
                                default:
                                    editWebpartPropertiesPopup.ShowOnPageLoad = false;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void btSaveWebpartProps_Click(object sender, EventArgs e)
        {

            if (!hdnSetting.Contains("EditWebpartIndex"))
                return;         

            if (!string.IsNullOrEmpty(Convert.ToString(hdnSetting.Get("EditWebpartIndex"))))
            {
                string[] parameterList = Convert.ToString(hdnSetting.Get("EditWebpartIndex")).Split('|');
                objPageConfiguration = objPageConfigurationManager.LoadByID(Convert.ToInt64(parameterList[1].ToString()));
                if (objPageConfiguration != null && objPageConfiguration.ControlInfo != null)
                {
                    List<DockPanelSetting> setting = new List<DockPanelSetting>();
                    if (!string.IsNullOrEmpty(objPageConfiguration.ControlInfo))
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(objPageConfiguration.ControlInfo);
                        setting = uHelper.DeSerializeAnObject(document, setting) as List<DockPanelSetting>;
                    }
                    if (setting != null && setting.Count > 0)
                    {
                        DockPanelSetting dockPanel = setting.FirstOrDefault(x => x.ControlID == Convert.ToString(parameterList[0]));
                        //object obj = null;
                        if (dockPanel != null && !string.IsNullOrEmpty(dockPanel.AssemblyName))
                        {
                            string webPart = dockPanel.AssemblyName;
                            if (webPart == null)
                                return;
                            webPart = webPart.Replace("uGovernIT.Web.ControlTemplates.DockPanels.", "").Trim();
                            switch (webPart)
                            {
                                case "TicketDockPanel":
                                    {
                                        FilterTicketsProperties props = webpartPropsHolder.Controls[0] as FilterTicketsProperties;
                                        if (props != null)
                                        {
                                            TicketDockPanelSetting ticketDockPanel = dockPanel as TicketDockPanelSetting;
                                            ticketDockPanel.Name = objPageConfiguration.Name;
                                            props.CopyFromControl(ticketDockPanel);
                                            dockPanel = ticketDockPanel;
                                        }
                                    }
                                    break;
                                case "DashboardDockPanel":
                                    {
                                        DashboardProperties props = webpartPropsHolder.Controls[0] as DashboardProperties;
                                        if (props != null)
                                        {
                                            DashboardDockPanelSetting ticketDockPanel = dockPanel as DashboardDockPanelSetting;
                                            props.CopyFromControl(ticketDockPanel);
                                            dockPanel = ticketDockPanel;
                                        }
                                    }
                                    break;
                                case "ModuleWebPartDockPanel":
                                    {
                                        ModuleWebpartProperties props = webpartPropsHolder.Controls[0] as ModuleWebpartProperties;
                                        if (props != null)
                                        {
                                            ModuleWebpartDockPanelSetting moduleWebpartDockPanel = dockPanel as ModuleWebpartDockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "ApplicationUpTimeDashBoardDockPanel":
                                    {
                                        ApplicationUptimeProperties props = webpartPropsHolder.Controls[0] as ApplicationUptimeProperties;
                                        if (props != null)
                                        {
                                            Applicationuptimesetting moduleWebpartDockPanel = dockPanel as Applicationuptimesetting ;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "MessageBoardDockPanel":
                                    {
                                        MessageboardProperties props = webpartPropsHolder.Controls[0] as MessageboardProperties;
                                        if (props != null)
                                        {
                                            MessageboardDockPanelSetting moduleWebpartDockPanel = dockPanel as MessageboardDockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "HomeDockPanel":
                                    {
                                      HomeWebpartProperties props = webpartPropsHolder.Controls[0] as HomeWebpartProperties;
                                        if (props != null)
                                        {
                                            HomeDockPanelSetting moduleWebpartDockPanel = dockPanel as HomeDockPanelSetting;
                                            moduleWebpartDockPanel.Name = objPageConfiguration.Name;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "RMMDockPanel":
                                    {
                                        RMMWebpartProperties props = webpartPropsHolder.Controls[0] as RMMWebpartProperties;
                                        if (props != null)
                                        {
                                            RMMDockPanelSetting moduleWebpartDockPanel = dockPanel as RMMDockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "DashboardSLADockPanel":
                                    {
                                        SLADashboardProperties props = webpartPropsHolder.Controls[0] as SLADashboardProperties;
                                        if (props != null)
                                        {
                                            DashboardSLADockPanelSetting moduleWebpartDockPanel = dockPanel as DashboardSLADockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "DashboardReportDockPanel":
                                    {
                                        DashboardReportProperties props = webpartPropsHolder.Controls[0] as DashboardReportProperties;
                                        if (props != null)
                                        {
                                            DashboardReportPanelSetting moduleWebpartDockPanel = dockPanel as DashboardReportPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;

                                case "ServiceDockPanel":
                                    {
                                        ServiceWebpartProperties props = webpartPropsHolder.Controls[0] as ServiceWebpartProperties;
                                        if (props != null)
                                        {
                                            ServiceDockPanelSetting moduleWebpartDockPanel = dockPanel as ServiceDockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "TicketCountTrendsDocPanel":
                                    {
                                        TicketCountTrendsProperties props = webpartPropsHolder.Controls[0] as TicketCountTrendsProperties;
                                        if (props != null)
                                        {
                                            TicketCountTrendsDocPanelSetting ticketCountTrendsDockPanelSettings = dockPanel as TicketCountTrendsDocPanelSetting;
                                            props.CopyFromControl(ticketCountTrendsDockPanelSettings);
                                            dockPanel = ticketCountTrendsDockPanelSettings;
                                        }
                                    }
                                    break;


                                case "TaskDockPanel":
                                    {
                                        TaskProperties props = webpartPropsHolder.Controls[0] as TaskProperties;
                                        if (props != null)
                                        {
                                            TaskDockPanelSetting moduleWebpartDockPanel = dockPanel as TaskDockPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                case "LinkDockPanel":
                                    {
                                        LinkProperties props = webpartPropsHolder.Controls[0] as LinkProperties;
                                        if (props != null)
                                        {
                                            LinkPanelSetting moduleWebpartDockPanel = dockPanel as LinkPanelSetting;
                                            props.CopyFromControl(moduleWebpartDockPanel);
                                            dockPanel = moduleWebpartDockPanel;
                                        }
                                    }
                                    break;
                                //case "uGovernITHome":
                                //    {
                                //        HomeWebpartProperties props = webpartPropsHolder.Controls[0] as HomeWebpartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITHome webpartObj = webPart as uGovernITHome;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITDashboardCharts":
                                //    {
                                //        DashboardChartWebpartProperties props = webpartPropsHolder.Controls[0] as DashboardChartWebpartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITDashboardCharts webpartObj = webPart as uGovernITDashboardCharts;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITModuleWebpart":
                                //    {
                                //        ModuleWebpartProperties props = webpartPropsHolder.Controls[0] as ModuleWebpartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITModuleWebpart webpartObj = webPart as uGovernITModuleWebpart;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITRMM":
                                //    {
                                //        RMMWebpartProperties props = webpartPropsHolder.Controls[0] as RMMWebpartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITRMM webpartObj = webPart as uGovernITRMM;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITEDM":
                                //    {
                                //        EDMWebPartProperties props = webpartPropsHolder.Controls[0] as EDMWebPartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITEDM webpartObj = webPart as uGovernITEDM;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITPortfolioGanttView":
                                //    {
                                //        PortfolioGanntViewWebPartProperties props = webpartPropsHolder.Controls[0] as PortfolioGanntViewWebPartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITPortfolioGanttView webpartObj = webPart as uGovernITPortfolioGanttView;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                //case "uGovernITVendorOperationalDashboard":
                                //    {
                                //        VendorOperationalDashboardWebPartProperties props = webpartPropsHolder.Controls[0] as VendorOperationalDashboardWebPartProperties;
                                //        if (props != null)
                                //        {
                                //            uGovernITVendorOperationalDashboard webpartObj = webPart as uGovernITVendorOperationalDashboard;
                                //            props.CopyFromControl(webpartObj);
                                //            manager.SaveChanges(webpartObj);
                                //        }
                                //    }
                                //    break;
                                default:
                                    //{
                                    //    BasicWebpartProperties props = webpartPropsHolder.Controls[0] as BasicWebpartProperties;
                                    //    if (props != null)
                                    //    {
                                    //        props.CopyFromControl(webPart);
                                    //        manager.SaveChanges(webPart);
                                    //    }

                                    //}
                                    break;
                            }
                            editWebpartPropertiesPopup.ShowOnPageLoad = false;
                            objPageConfiguration.ControlInfo = uHelper.SerializeObject(setting).OuterXml;
                            objPageConfigurationManager.Update(objPageConfiguration);
                            Util.Log.ULog.WriteUGITLog(HttpContext.Current.GetManagerContext().CurrentUser.Id, $"Update webpart property : {webPart}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), HttpContext.Current.GetManagerContext().TenantID);
                            Response.Redirect(HttpContext.Current.Request.Url.ToString());
                        }
                    }
                }
            }
        }

        protected void ddlDashboardView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        protected void chkShowFooter_CheckedChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        protected void chkShowSearch_CheckedChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        protected void ddlMenuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        protected void chkShowLeftNav_CheckedChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        private void SetMasterContent_SelectedPage()
        {
            infoMessageBoard.Visible = false;
            PageConfiguration PageConfiguration = objPageConfigurationManager.LoadByID(selectedPageID);
            if (PageConfiguration == null)
                return;
            
            if (PageConfiguration != null)
            {
                string menuType = string.Empty;
                if (ddlMenuList.SelectedItem != null)
                    menuType = Convert.ToString(ddlMenuList.SelectedItem.Value);

                PageConfiguration.HideTopMenu = !chkenablemenu.Checked;
                PageConfiguration.HideHeader = !chkHeader.Checked;
                PageConfiguration.TopMenuName = menuType;
                PageConfiguration.TopMenuType = menuType;
                PageConfiguration.HideSearch = !chkShowSearch.Checked;
                PageConfiguration.HideFooter = !chkShowFooter.Checked;
                PageConfiguration.HideLeftMenu = !chkShowLeftNav.Checked;
                PageConfiguration.Title = !string.IsNullOrEmpty(txtPageTitle.Text) ? txtPageTitle.Text.Trim() : string.Empty;
                
                if (chkShowLeftNav.Checked && rdbenableleftnavigation.SelectedIndex != -1)
                {
                    // Forcefully setting the selected index as 0 to select 'Show Menu' item as we have hide the 'Show Dashboard' item.
                    rdbenableleftnavigation.SelectedIndex = 0;
                    
                    if (rdbenableleftnavigation.SelectedIndex == 0)
                    {
                        string leftsidemenutype = string.Empty;
                        if (ddlleftmenuoptions.SelectedItem == null)
                        {
                            ddlleftmenuoptions.SelectedIndex = ddlleftmenuoptions.Items.IndexOfValue("Default");
                            leftsidemenutype = Convert.ToString(ddlleftmenuoptions.SelectedItem.Value);
                        }
                        else
                        {
                            leftsidemenutype = Convert.ToString(ddlleftmenuoptions.SelectedItem.Value);
                        }

                        PageConfiguration.LeftMenuName = leftsidemenutype;
                        PageConfiguration.LeftMenuType = "Menu";
                    }
                    else if (rdbenableleftnavigation.SelectedIndex == 1)
                    {
                        string leftsideashboradtype = string.Empty;
                        if (ddlDashboardGroup.SelectedItem == null)
                            ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOfValue("Default");
                        if (ddlDashboardGroup.SelectedIndex != -1)
                        {
                            leftsideashboradtype = Convert.ToString(ddlDashboardGroup.SelectedItem.Value);
                            PageConfiguration.LeftMenuName = leftsideashboradtype;
                            if (ddlDashboardView.SelectedItem != null)
                                PageConfiguration.LeftMenuType = Convert.ToString(ddlDashboardView.SelectedItem.Value);
                        }
                    }
                }
                objPageConfigurationManager.Update(PageConfiguration);
            }     
        }

        private void LoadMasterContent_SelectedPage()
        {
            editContentPanel.Visible = true;
            infoMessageBoard.Visible = false;
            ResetToDefault();
            PageConfiguration PageConfiguration = objPageConfigurationManager.LoadByID(selectedPageID);
            if (PageConfiguration != null)
            {
                if (!PageConfiguration.HideLeftMenu)
                {
                    chkShowLeftNav.Checked = true;
                    
                    // Forcefully setting the LeftMenuType as 'Menu' because we have hide the option for 'Show Dashboard'
                    if (string.IsNullOrEmpty(PageConfiguration.LeftMenuType) || PageConfiguration.LeftMenuType != "Menu")
                        PageConfiguration.LeftMenuType = "Menu";

                    if (PageConfiguration.LeftMenuType == "Menu")
                    {
                        rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("0"));
                    }
                    else
                    {
                        rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("1"));
                    }
                    ddlleftmenuoptions.Visible = true;
                    if (string.IsNullOrWhiteSpace(PageConfiguration.LeftMenuName))
                        PageConfiguration.LeftMenuName = "Default";
                    ddlleftmenuoptions.SelectedIndex = ddlleftmenuoptions.Items.IndexOfValue(PageConfiguration.LeftMenuName);
                    if (PageConfiguration.LeftMenuType != "Menu")
                    {
                        ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOfValue(PageConfiguration.LeftMenuName);
                        ddlDashboardGroup_SelectedIndexChanged(ddlDashboardGroup, new EventArgs());
                        ddlDashboardView.SelectedIndex = ddlDashboardView.Items.IndexOfValue(PageConfiguration.LeftMenuType);
                        //ddlDashboardView.Text = PageConfiguration.LeftMenuType;//ddlDashboardView.Items.IndexOfValue(PageConfiguration.LeftMenuType);
                    }
                      
                          
                }
                if (!PageConfiguration.HideTopMenu)
                {
                    chkenablemenu.Checked = true;
                    ShowHeaderOptions();
                    chkShowHeader.Checked = true;
                    ddlMenuList.Visible = true;
                    if (string.IsNullOrWhiteSpace(PageConfiguration.TopMenuName))
                        PageConfiguration.TopMenuName = "Default";
                    ddlMenuList.SelectedIndex = ddlMenuList.Items.IndexOfValue(PageConfiguration.TopMenuName);
                }
                if (!PageConfiguration.HideSearch)
                {
                    chkShowSearch.Checked = true;
                }
                if (!PageConfiguration.HideFooter)
                {
                    chkShowFooter.Checked = true;
                }
                if (!PageConfiguration.HideHeader)
                {
                    chkHeader.Checked = true;
                }
                if (!string.IsNullOrEmpty(PageConfiguration.Title))
                {
                    pageTitle = txtPageTitle.Text = PageConfiguration.Title;
                }
                //  DataTable pageWebParts = GetWebpardsFromPage(list, selectedPageID);
                //  acvPageContent.DataSource = pageWebParts;
                //  acvPageContent.DataBind();

                // SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
                // Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);
                //  List<System.Web.UI.WebControls.WebParts.WebPart> webParts = manager.WebParts.Cast<System.Web.UI.WebControls.WebParts.WebPart>().ToList();

                //  System.Web.UI.WebControls.WebParts.WebPart headerWebPart = webParts.FirstOrDefault(x => manager.GetZoneID(x) == "Body" && x.GetType().Name == "uGovernITTopMenuBar");

                //if (headerWebPart != null)
                //{
                //    uGovernITTopMenuBar topMenu = headerWebPart as uGovernITTopMenuBar;
                //    chkenablemenu.Checked = true;
                //    ShowHeaderOptions();
                //    if (!topMenu.HideTopMenu)
                //    {
                //        chkShowHeader.Checked = true;
                //        ddlMenuList.Visible = true;
                //        if (string.IsNullOrWhiteSpace(topMenu.MenuType))
                //            topMenu.MenuType = "Default";

                //        ddlMenuList.SelectedIndex = ddlMenuList.Items.IndexOfValue(topMenu.MenuType);
                //    }
                //    else
                //    {
                //        ddlMenuList.Visible = false;
                //    }

                //    chkShowSearch.Checked = !topMenu.HideGlobalSearch;
                //    chkShowSetting.Checked = !topMenu.HideSettingMenu;
                //    chkShowFooter.Checked = !topMenu.HideFooter;
                //}

                ///System.Web.UI.WebControls.WebParts.WebPart sideBarWebpart = webParts.FirstOrDefault(x => manager.GetZoneID(x) == leftNavZone && x.GetType().Name == "uGovernITSideBar");
                // System.Web.UI.WebControls.WebParts.WebPart leftChartDashboardWebpart = webParts.FirstOrDefault(x => manager.GetZoneID(x) == leftNavZone && x.GetType().Name == "uGovernITDashboardCharts");
                //  System.Web.UI.WebControls.WebParts.WebPart leftsidetopmenubar = webParts.FirstOrDefault(x => manager.GetZoneID(x) == leftNavZone && x.GetType().Name == "uGovernITTopMenuBar");
                //if (leftsidetopmenubar != null)
                //{
                //    uGovernITTopMenuBar topMenu = leftsidetopmenubar as uGovernITTopMenuBar;
                //    if (topMenu != null)
                //    {
                //        chkShowLeftNav.Checked = true;
                //        rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("0"));
                //        ddlleftmenuoptions.Visible = true;
                //        if (string.IsNullOrWhiteSpace(topMenu.MenuType))
                //            topMenu.MenuType = "Default";

                //        ddlleftmenuoptions.SelectedIndex = ddlleftmenuoptions.Items.IndexOfValue(topMenu.MenuType);
                //        if (sideBarWebpart != null)
                //        {
                //            manager.DeleteWebPart(sideBarWebpart);
                //        }
                //        if (leftChartDashboardWebpart != null)
                //        {
                //            manager.DeleteWebPart(leftChartDashboardWebpart);
                //        }
                //    }
                //}
                //if (sideBarWebpart != null)
                //{
                //    uGovernITSideBar leftNav = sideBarWebpart as uGovernITSideBar;
                //    if (!string.IsNullOrWhiteSpace(leftNav.LeftSideNavigation))
                //    {
                //        chkShowLeftNav.Checked = true;
                //        rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("1"));
                //        ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOfValue("Side Dashboards");
                //        ddlDashboardGroup_SelectedIndexChanged(ddlDashboardGroup, new EventArgs());
                //        ddlDashboardView.SelectedIndex = ddlDashboardView.Items.IndexOfValue(leftNav.LeftSideNavigation);
                //        if (leftsidetopmenubar != null)
                //        {
                //            manager.DeleteWebPart(leftsidetopmenubar);
                //        }

                //    }
                //}

                //if (leftChartDashboardWebpart != null)
                //{
                //    uGovernITDashboardCharts leftNav = leftChartDashboardWebpart as uGovernITDashboardCharts;
                //    if (!string.IsNullOrWhiteSpace(leftNav.Dashboard))
                //    {
                //        var view = DashboardPanelView.Load(leftNav.Dashboard, SPContext.Current.Web);
                //        if (view != null)
                //        {
                //            chkShowLeftNav.Checked = true;
                //            rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("1  "));
                //            ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOfValue(view.ViewType);
                //            ddlDashboardGroup_SelectedIndexChanged(ddlDashboardGroup, new EventArgs());
                //            ddlDashboardView.SelectedIndex = ddlDashboardView.Items.IndexOfValue(leftNav.Dashboard);
                //            if (leftsidetopmenubar != null)
                //            {
                //                manager.DeleteWebPart(leftsidetopmenubar);
                //            }
                //        }
                //    }
                //}
                //if (sideBarWebpart == null && leftChartDashboardWebpart == null && rdbenableleftnavigation.Items.Count > 0)
                //{
                //    rdbenableleftnavigation.SelectedIndex = rdbenableleftnavigation.Items.IndexOf(rdbenableleftnavigation.Items.FindByValue("0"));
                //}
            }
        }

        private void ResetToDefault()
        {
            chkShowFooter.Checked = false;
            chkShowHeader.Checked = false;
            chkHeader.Checked = false;
            ShowHeaderOptions();
            chkShowSearch.Checked = false;
            ddlMenuList.SelectedIndex = -1;
            chkShowLeftNav.Checked = false;
            ddlDashboardView.SelectedIndex = -1;
            chkShowSetting.Checked = false;
            chkenablemenu.Checked = false;
            txtPageTitle.Text = string.Empty;
        }

        private void BindMenuTypeDropDwn()
        {
            MenuNavigationManager objMenuNavigationHelper = new MenuNavigationManager(HttpContext.Current.GetManagerContext());
            List<MenuNavigation> lstMenuNavigation = objMenuNavigationHelper.Load();
            if (lstMenuNavigation.Count > 0)
            {
                List<string> lstMenuName = lstMenuNavigation.Where(x => x.MenuName != null).Select(x => x.MenuName).Distinct().ToList();
                if (lstMenuName.Count == 0 || lstMenuName == null)
                {
                    lstMenuName = new List<string>();
                }

                if (lstMenuName.Count <= 0)
                {
                    List<string> menuNavigationTypes = new List<string>();
                    menuNavigationTypes.Add("Default");
                    ddlMenuList.Items.Add("Default", "Default");
                    ddlleftmenuoptions.Items.Add("Default", "Default"); 
                }

                foreach (string row in lstMenuName)
                {
                    ddlMenuList.Items.Add(row, row);
                    ddlleftmenuoptions.Items.Add(row, row);
                }
            }            
        }

        protected void btEditPage_Click(object sender, EventArgs e)
        {
            if (hdnSetting.Contains("SelectedPage"))
            {
                selectedPageID = UGITUtility.StringToInt(hdnSetting.Get("SelectedPage"));
                if (selectedPageID > 0)
                {
                    Response.Redirect("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview&Width=1229.4&Height=595.8&isudlg=1&pageTitle=Page+Editor&SelectedPage=" + selectedPageID);
                    //LoadMasterContent_SelectedPage();
                }
            }
        }

        protected void acvPageContent_CardDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //string uniqueID = Convert.ToString(e.Keys[0]);

            //SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
            //Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);
            //System.Web.UI.WebControls.WebParts.WebPart webPart = manager.WebParts.Cast<System.Web.UI.WebControls.WebParts.WebPart>().FirstOrDefault(x => x.UniqueID == uniqueID);
            //if (webPart == null)
            //    return;

            //manager.DeleteWebPart(webPart);

            //e.Cancel = true;

            //DataTable pageWebParts = GetWebpardsFromPage(list, selectedPageID);
            //acvPageContent.DataSource = pageWebParts;
            //acvPageContent.DataBind();
        }

        protected void btDeletePage_Click(object sender, EventArgs e)
        {
            if (objPageConfiguration != null)
            {
                objPageConfigurationManager.Delete(objPageConfiguration);
                TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
                List<TabView> tabViews = tabViewManager.Load(x => x.ViewName == objPageConfiguration.Name).ToList();
                tabViewManager.Delete(tabViews);
                hdnSetting.Remove("SelectedPage");
                Response.Redirect("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=pageeditorview&Width=1229.4&Height=595.8&isudlg=1&pageTitle=Page+Editor");
                bindSitePages();
                editContentPanel.Visible = false;
                infoMessageBoard.Visible = true;
            }
        }

        protected void grdvPageList_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            //GridViewContextMenuItem Rename = e.CreateItem("Rename", "Rename");
            //Rename.Image.Url = "/Content/images/duplicate.png";
            //e.Items.Add(Rename);
        }

        protected void grdvPageList_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
        {
            //if (e.Item.Name == "Duplicate")
            //{
            //    DataRow row = grdvPageList.GetDataRow(e.ElementIndex);
            //    if (row != null)
            //    {
            //        SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
            //        Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);
            //    }
            //}
        }

        protected void grdvPageList_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int pageID = UGITUtility.StringToInt(e.Keys[0]);
            PageConfiguration objpage = objPageConfigurationManager.LoadByID(pageID);
            
            if (objpage != null)
            {
                objPageConfigurationManager.Delete(objpage);
                hdnSetting.Remove("SelectedPage");
                bindSitePages();
                editContentPanel.Visible = false;
                infoMessageBoard.Visible = true;
            }
            
            e.Cancel = true;
        }

        private void ShowHeaderOptions()
        {
            panelHeaderOptions.Visible = false;
            if (chkenablemenu.Checked)//chkShowHeader.Checked
            {
                panelHeaderOptions.Visible = true;
                ddlMenuList.Visible = false;
            }
        }

        protected void btSortWebpart_Click(object sender, EventArgs e)
        {
            //string sortSourceWebpartKey = Convert.ToString(hdnSetting.Get("sortSourceWebpartKey"));
            //string sortTargetWebpartKey = Convert.ToString(hdnSetting.Get("sortTargetWebpartKey"));

            //hdnSetting.Remove("sortSourceWebpartKey");
            //hdnSetting.Remove("sortTargetWebpartKey");

            //if (sortSourceWebpartKey != null)
            //    sortSourceWebpartKey = sortSourceWebpartKey.Replace("\n", string.Empty).Replace("\r", string.Empty).Trim();

            //if (sortTargetWebpartKey != null)
            //    sortTargetWebpartKey = sortTargetWebpartKey.Replace("\n", string.Empty).Replace("\r", string.Empty).Trim();

            //if (sortSourceWebpartKey != sortTargetWebpartKey && !string.IsNullOrWhiteSpace(sortSourceWebpartKey))
            //{
            //    SPWeb spWeb = SPContext.Current.Web;
            //    SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
            //    using (Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
            //    {

            //        List<Microsoft.SharePoint.WebPartPages.WebPart> webparts = new List<Microsoft.SharePoint.WebPartPages.WebPart>();
            //        foreach (Microsoft.SharePoint.WebPartPages.WebPart webPart in manager.WebParts)
            //        {
            //            if (webPart is uGovernITTopMenuBar ||
            //                webPart is TitleBarWebPart || (webPart.ZoneID != "Body" && webPart.ZoneID != "FullPage"))
            //            {
            //                continue;
            //            }
            //            webparts.Add(webPart);
            //        }

            //        webparts = webparts.OrderBy(x => x.ZoneIndex).ToList();
            //        Microsoft.SharePoint.WebPartPages.WebPart sourceWebpart = webparts.FirstOrDefault(x => x.UniqueID == sortSourceWebpartKey);
            //        if (sourceWebpart != null)
            //        {
            //            webparts.Remove(sourceWebpart);
            //            Microsoft.SharePoint.WebPartPages.WebPart targetWebpart = webparts.FirstOrDefault(x => x.UniqueID == sortTargetWebpartKey);
            //            if (targetWebpart != null)
            //            {
            //                int targetIndex = webparts.IndexOf(targetWebpart);
            //                webparts.Insert(targetIndex, sourceWebpart);
            //            }
            //            else
            //            {
            //                webparts.Add(sourceWebpart);
            //            }
            //        }

            //        for (int i = 0; i < webparts.Count; i++)
            //        {
            //            manager.MoveWebPart(webparts[i], webparts[i].ZoneID, i + 1);
            //            manager.SaveChanges(webparts[i]);
            //        }

            //    }

            //    LoadMasterContent_SelectedPage();
            //}
        }

        protected void Label1_Load(object sender, EventArgs e)
        {
            //string existingtext = Label1.Text;
            //Label1.Text = pagename != string.Empty ? string.Format("{0} {1}?", existingtext, pagename) : string.Format("{0} {1}?", existingtext, "this page");
        }



        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pagename))
            {
                //// MoveWebPart(spWeb, pagename, "uGovernITSideBar", "Body", 0);
                //SPListItem item = SPListHelper.GetSPListItem(list, selectedPageID);
                //Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager manager = item.File.GetLimitedWebPartManager(PersonalizationScope.Shared);
                //List<System.Web.UI.WebControls.WebParts.WebPart> webParts = manager.WebParts.Cast<System.Web.UI.WebControls.WebParts.WebPart>().ToList();
                //System.Web.UI.WebControls.WebParts.WebPart sideBarWebpart = webParts.FirstOrDefault(x => x.GetType().Name == "uGovernITSideBar");
                //System.Web.UI.WebControls.WebParts.WebPart topmenubarWebpart = webParts.FirstOrDefault(x => x.GetType().Name == "uGovernITTopMenuBar");

                //if (sideBarWebpart == null || topmenubarWebpart == null)
                //    return;

                //string sidebarzone = manager.GetZoneID(sideBarWebpart);
                //string topmenubar = manager.GetZoneID(topmenubarWebpart);

                //manager.MoveWebPart(sideBarWebpart, topmenubar, 0);
                //manager.SaveChanges(sideBarWebpart);
                //manager.MoveWebPart(topmenubarWebpart, sidebarzone, 0);
                //manager.SaveChanges(topmenubarWebpart);
                //spWeb.Update();
            }

        }

        protected void chkenablemenu_CheckedChanged(object sender, EventArgs e)
        {
            panelHeaderOptions.Visible = false;

            if (chkenablemenu.Checked)
            {
                panelHeaderOptions.Visible = true;
            }
            else
            {
                chkShowHeader.Checked = false;
                chkShowSearch.Checked = false;
                chkShowHeader_CheckedChanged(null, null);
            }
            SetMasterContent_SelectedPage();
        }
        protected void chkHeaderSetting_CheckedChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }
        protected void chkShowSetting_CheckedChanged(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }

        protected void rdbenableleftnavigation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbenableleftnavigation.SelectedIndex == 0)
            {
                ddlleftmenuoptions.SelectedIndex = ddlleftmenuoptions.Items.IndexOfValue("Default");
            }
            if (rdbenableleftnavigation.SelectedIndex == 1)
            {
                ddlDashboardGroup.SelectedIndex = -1;
                ddlDashboardView.SelectedIndex = -1;
            }
            SetMasterContent_SelectedPage();
        }

        protected void abb_ClientLayout(object sender, ASPxClientLayoutArgs e)
        {
            objPageConfiguration = objPageConfigurationManager.Get(x => x.ID == selectedPageID);
            if (objPageConfiguration != null)
            {
                if (e.LayoutMode == ClientLayoutMode.Loading) {  e.LayoutData = objPageConfiguration.LayoutInfo; }
                   
                if (e.LayoutMode== ClientLayoutMode.Saving && !string.IsNullOrEmpty(e.LayoutData))
                {
                    objPageConfiguration.LayoutInfo = e.LayoutData;
                    objPageConfigurationManager.Update(objPageConfiguration);                   
                }                
            }
        }

        protected void abb_AfterDock(object source, DockManagerEventArgs e)
        {

        }

        protected void editWebpartPropertiesPopup_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            ASPxPopupControl popControl=source as ASPxPopupControl;
            popControl.ShowOnPageLoad = true;
            //if (!string.IsNullOrEmpty(e.Parameter))
            //{
            //    string[] parameterList = e.Parameter.Split('|');
            //    objPageConfiguration = objPageConfigurationManager.Get(x => x.ID == Convert.ToInt64(parameterList[1].ToString()));
            //    if (objPageConfiguration != null && objPageConfiguration.ControlInfo != null)
            //    {
            //        List<DockPanelSetting> setting = new List<DockPanelSetting>();
            //        if (!string.IsNullOrEmpty(objPageConfiguration.ControlInfo))
            //        {
            //            XmlDocument document = new XmlDocument();
            //            document.LoadXml(objPageConfiguration.ControlInfo);
            //            setting = uHelper.DeSerializeAnObject(document, setting) as List<DockPanelSetting>;
            //        }
            //        if (setting != null && setting.Count > 0)
            //        {
            //            DockPanelSetting dockPanel = setting.FirstOrDefault(x => x.ControlID == Convert.ToString(parameterList[0]));
            //            object obj = null;
            //            if (dockPanel != null && !string.IsNullOrEmpty(dockPanel.AssemblyName))
            //            {
            //                if (dockPanel.AssemblyName== "uGovernIT.Web.ControlTemplates.DockPanels.TicketDockPanel")
            //                {
            //                    FilterTicketsProperties filterTicketsProperties = (FilterTicketsProperties)Page.LoadControl("/ControlTemplates/DockPanels/DockPanelEditControl/FilterTicketsProperties.ascx");
            //                    filterTicketsProperties.ticketDockPanelSetting = dockPanel as TicketDockPanelSetting;
            //                    if (popControl != null)
            //                    {
            //                        filterTicketsProperties.ID = Convert.ToString(parameterList[0]) + "_props";
            //                        hdnSetting.Set("EditWebpartIndex", e.Parameter + "|" + filterTicketsProperties.ID);
            //                        webpartPropsHolder.Controls.Add(filterTicketsProperties);
                                   
            //                        hdnEditWebpartIndex.Value = "EditWebpartIndex|"+ e.Parameter+"|"+ filterTicketsProperties.ID;
            //                        popControl.ShowOnPageLoad = true;
            //                    }
            //                }
            //            }     
            //        }
            //    }
            //}
        }

        protected void btnSaveConfiguration_Click(object sender, EventArgs e)
        {
            SetMasterContent_SelectedPage();
        }
    }
}
