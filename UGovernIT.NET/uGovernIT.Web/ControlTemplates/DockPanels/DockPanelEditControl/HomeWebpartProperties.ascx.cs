using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.DockPanels;

namespace uGovernIT.Web
{
    public partial class HomeWebpartProperties : UserControl
    {
        public string WaitingOnMeTabName { get; set; }
        // ConfigurationVariable.GetValue("WaitingOnMeTabName", "Waiting On Me");
        public HomeDockPanelSetting homeDockPanelSetting { get; set; }
        protected override void OnInit(EventArgs e)
        {
            chkTitle.Checked = homeDockPanelSetting.ShowTitle;
            txtTitle.Text = homeDockPanelSetting.Title;

            txtWelcomeTitle.Text = homeDockPanelSetting.WelcomeHeading;
            txtWelcomeDesc.Text = homeDockPanelSetting.WelcomeDesc;
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            string Viewname = string.Empty;
            if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(homeDockPanelSetting.Name)))
                Viewname = homeDockPanelSetting.Name;
            List<TabView> tabView = tabViewManager.Load(z => z.ModuleNameLookup == string.Empty && z.ViewName == Viewname);
            if (tabView.Count == 0)
            {
                tabView = tabViewManager.Load(z => z.ViewName == "Home");
                foreach (var item in tabView)
                {
                    item.ID = 0;
                }
            }
            if (tabView != null && tabView.Count > 0)
            {
                repeaterTabView.DataSource = tabView;
                repeaterTabView.DataBind();
            }

            chkEnableNewButton.Checked = homeDockPanelSetting.EnableNewButton;
            

            //chkSMSModules.Checked = !homeDockPanelSetting.HideSMSModules;
            //chkGovernanceModules.Checked = !homeDockPanelSetting.HideGovernanceModules;
            chkServiceCatalog.Checked = homeDockPanelSetting.ShowServiceCatalog;
            //chkCRMModules.Checked = !homeDockPanelSetting.HideCRMModules;

            ddlModulePanelOrder.SelectedIndex = ddlModulePanelOrder.Items.IndexOfText(homeDockPanelSetting.ModulePanelOrder.ToString());
            ddlMyTicketPanelOrder.SelectedIndex = ddlMyTicketPanelOrder.Items.IndexOfText(homeDockPanelSetting.MyTicketPanelOrder.ToString());
            ddlServiceCatalogOrder.SelectedIndex = ddlServiceCatalogOrder.Items.IndexOfText(homeDockPanelSetting.ServiceCatalogOrder.ToString());

            txtNoOfTickets.Text = homeDockPanelSetting.NoOfPreviewTickets.ToString();
            chkShowServiceIcons.Checked = homeDockPanelSetting.ShowIcons;
            chkShowBandedRows.Checked = homeDockPanelSetting.ShowBandedRows;
            chkShowCompactRows.Checked = homeDockPanelSetting.ShowCompactRows;
            rblServiceViewType.SelectedValue = homeDockPanelSetting.ServiceViewType;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            homeDockPanelSetting.ShowTitle = chkTitle.Checked;
            homeDockPanelSetting.Title = txtTitle.Text.Trim();

            homeDockPanelSetting.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            homeDockPanelSetting.WelcomeDesc = txtWelcomeDesc.Text.Trim();

            //homeDockPanelSetting.HideWaitingOnMeTab = !chkWaitingOnMeTab.Checked;
            //homeDockPanelSetting.UpdateWaitingOnMeTab = "";//WaitingOnMeTabName; // txtWaitingOnMeTab.Text.Trim();

            //homeDockPanelSetting.HideMyDepartmentTickets = !chkMyDepartmentTab.Checked;
            //homeDockPanelSetting.UpdateMyDepartmentTickets = txtMyDepartmentTab.Text.Trim();

            //homeDockPanelSetting.HideMyDivisionTickets = !chkMyDivisionTab.Checked;
            //homeDockPanelSetting.UpdateMyDivisionTickets = txtMyDivisionTab.Text.Trim();

            //homeDockPanelSetting.HideMyPendingApprovals = !chkDocumentPendingApprovalTab.Checked;
            //homeDockPanelSetting.UpdateMyPendingApprovals = txtDocumentPendingApprovalTab.Text.Trim();

            //homeDockPanelSetting.HideMyProject = !chkMyProjectsTab.Checked;
            //homeDockPanelSetting.UpdateMyProject = txtMyProjectsTab.Text.Trim();

            //homeDockPanelSetting.HideMyTasks = !chkMyTaskTab.Checked;
            //homeDockPanelSetting.UpdateMyTasks = txtMyTaskTab.Text.Trim();

            //homeDockPanelSetting.HideMyTickets = !chkMyReqeustTab.Checked;
            //homeDockPanelSetting.UpdateMyTickets = txtMyRequestTab.Text.Trim();

            //homeDockPanelSetting.HideMyClosedTickets = !chkMyClosedRequestsTab.Checked;
            //homeDockPanelSetting.UpdateMyClosedTickets = txtMyClosedRequestsTab.Text.Trim();


            //homeDockPanelSetting.HideSMSModules = !chkSMSModules.Checked;
            //homeDockPanelSetting.HideCRMModules = !chkCRMModules.Checked;
            //homeDockPanelSetting.HideGovernanceModules = !chkGovernanceModules.Checked;
            //homeDockPanelSetting.ShowServiceCatalog = chkServiceCatalog.Checked;

            homeDockPanelSetting.EnableNewButton = chkEnableNewButton.Checked;

            homeDockPanelSetting.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());

            homeDockPanelSetting.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            homeDockPanelSetting.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            homeDockPanelSetting.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);

        }

        public void CopyFromWebpart(HomeDockPanelSetting webpart)
        {
            homeDockPanelSetting.ShowTitle = chkTitle.Checked;
            homeDockPanelSetting.Title = txtTitle.Text.Trim();
            homeDockPanelSetting.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            homeDockPanelSetting.WelcomeDesc = txtWelcomeDesc.Text.Trim();
            //Tab related properties configured from Config_TabView table, Add/ Edit and view will be based on this table
            //table contains record for each tab with view name like 'Home','TSR' etc.
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            TabView tabView = null;
            foreach (RepeaterItem i in repeaterTabView.Items)
            {
                HiddenField hiddenID = (HiddenField)i.FindControl("idField");
                if (hiddenID != null)
                {
                    tabView = tabViewManager.LoadByID(Convert.ToInt64(hiddenID.Value));
                }
                if (tabView != null)
                {
                    TextBox txtDisplay = (TextBox)i.FindControl("txtDisplayName");
                    ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                    if (checkBox != null)
                        tabView.ShowTab = checkBox.Checked;
                    if (txtDisplay != null)
                        tabView.TabDisplayName = Convert.ToString(txtDisplay.Text);
                    tabViewManager.Update(tabView);
                    tabView = null;
                }
            }
            //homeDockPanelSetting.HideSMSModules = !chkSMSModules.Checked;
            //homeDockPanelSetting.HideCRMModules = !chkCRMModules.Checked;
            //homeDockPanelSetting.HideGovernanceModules = !chkGovernanceModules.Checked;
            homeDockPanelSetting.ShowServiceCatalog = chkServiceCatalog.Checked;
            homeDockPanelSetting.EnableNewButton = chkEnableNewButton.Checked;
            homeDockPanelSetting.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());
            homeDockPanelSetting.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            homeDockPanelSetting.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            homeDockPanelSetting.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);
            webpart = homeDockPanelSetting;
        }

        public void CopyFromControl(HomeDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text.Trim();
            webpart.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            webpart.WelcomeDesc = txtWelcomeDesc.Text.Trim();
            //Tab related properties configured from Config_TabView table, Add/ Edit and view will be based on this table
            //table contains record for each tab with view name like 'Home','TSR' etc.
            TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
            foreach (RepeaterItem i in repeaterTabView.Items)
            {
                HiddenField hiddenID = (HiddenField)i.FindControl("idField");
                if (hiddenID != null && Convert.ToInt64(hiddenID.Value) > 0)
                {
                    TabView tabView = tabViewManager.LoadByID(Convert.ToInt64(hiddenID.Value));
                    if (tabView != null)
                    {
                        ASPxTextBox txtDisplay = (ASPxTextBox)i.FindControl("txtDisplayName");
                        ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                        if (checkBox != null)
                            tabView.ShowTab = checkBox.Checked;
                        if (txtDisplay != null)
                            tabView.TabDisplayName = Convert.ToString(txtDisplay.Text);
                        tabView.ViewName = webpart.Name;
                        tabView.ModuleNameLookup = string.Empty;
                        tabViewManager.Update(tabView);
                    }
                }
                else
                {
                    TabView tab = new TabView();
                    ASPxTextBox txtDisplay = (ASPxTextBox)i.FindControl("txtDisplayName");
                    ASPxCheckBox checkBox = (ASPxCheckBox)i.FindControl("chkTitle");
                    HiddenField hdnTabOrderId = (HiddenField)i.FindControl("hdnTabOrder");
                    HiddenField hdnTablableName = (HiddenField)i.FindControl("hdnTablabelName");

                    if (checkBox != null)
                        tab.ShowTab = checkBox.Checked;
                    if (txtDisplay != null)
                        tab.TabDisplayName = Convert.ToString(txtDisplay.Text);
                    tab.ViewName = webpart.Name;
                    tab.ModuleNameLookup = string.Empty;
                    tab.TabOrder = Convert.ToInt32(hdnTabOrderId.Value);
                    tab.TabName = txtDisplay.ClientInstanceName;
                    tab.ColumnViewName = "MyHomeTab";
                    tab.TablabelName = hdnTablableName.Value;
                    tabViewManager.Insert(tab);
                }
            }
            //webpart.HideSMSModules = !chkSMSModules.Checked;
            //webpart.HideCRMModules = !chkCRMModules.Checked;
            //webpart.HideGovernanceModules = !chkGovernanceModules.Checked;
            webpart.ShowServiceCatalog = chkServiceCatalog.Checked;
            webpart.EnableNewButton = chkEnableNewButton.Checked;
            webpart.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());
            webpart.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            webpart.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            webpart.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);
            webpart.ShowIcons = chkShowServiceIcons.Checked;
            webpart.ShowBandedRows = chkShowBandedRows.Checked;
            webpart.ShowCompactRows = chkShowCompactRows.Checked;
            webpart.ServiceViewType = rblServiceViewType.SelectedValue;
            homeDockPanelSetting = webpart;
        }

        protected void control_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                TabViewManager tabViewManager = new TabViewManager(HttpContext.Current.GetManagerContext());
                List<TabView> tabView = tabViewManager.Load(z => z.ViewName == "Home");
                if (tabView != null)
                {
                    repeaterTabView.DataSource = tabView;
                    repeaterTabView.DataBind();
                }
            }
        }
    }
}
