using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels.DockPanelEditControl
{
    public partial class ServiceWebpartProperties : System.Web.UI.UserControl
    {
        public ServiceDockPanelSetting serviceDockPanelSetting { get; set; }
        DashboardManager dashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());


        protected override void OnInit(EventArgs e)
        {

            var dashboardItems = dashboardManager.Load(x => x.DashboardType == DashboardType.Panel).OrderBy(x => x.Title).ToList();
            ddlPanel.DataTextField = DatabaseObjects.Columns.Title;
            ddlPanel.DataValueField = DatabaseObjects.Columns.ID;
            ddlPanel.DataSource = dashboardItems;
            ddlPanel.DataBind();

            ddlPanel.Items.Insert(0, new ListItem("--Select--", "0"));


            //chkTitle.Checked = homeDockPanelSetting.ShowTitle;
            //txtTitle.Text = homeDockPanelSetting.Title;

            //txtWelcomeTitle.Text = homeDockPanelSetting.WelcomeHeading;
            //txtWelcomeDesc.Text = homeDockPanelSetting.WelcomeDesc;

            //chkWaitingOnMeTab.Checked = !homeDockPanelSetting.HideWaitingOnMeTab;
            //txtWaitingOnMeTab.Text = homeDockPanelSetting.UpdateWaitingOnMeTab; //UpdateWaitingOnMeTab;
            //WaitingOnMeTabName = homeDockPanelSetting.UpdateWaitingOnMeTab;
            //chkMyReqeustTab.Checked = !homeDockPanelSetting.HideMyTickets;
            //txtMyRequestTab.Text = homeDockPanelSetting.UpdateMyTickets;

            //chkEnableNewButton.Checked = homeDockPanelSetting.EnableNewButton;

            //chkMyDepartmentTab.Checked = !homeDockPanelSetting.HideMyDepartmentTickets;
            //txtMyDepartmentTab.Text = homeDockPanelSetting.UpdateMyDepartmentTickets;

            //chkMyDivisionTab.Checked = !homeDockPanelSetting.HideMyDivisionTickets;
            //txtMyDivisionTab.Text = homeDockPanelSetting.UpdateMyDivisionTickets;

            //chkMyProjectsTab.Checked = !homeDockPanelSetting.HideMyProject;
            //txtMyProjectsTab.Text = homeDockPanelSetting.UpdateMyProject;

            //chkMyClosedRequestsTab.Checked = !homeDockPanelSetting.HideMyClosedTickets;
            //txtMyClosedRequestsTab.Text = homeDockPanelSetting.UpdateMyClosedTickets;

            //chkMyTaskTab.Checked = !homeDockPanelSetting.HideMyTasks;
            //txtMyTaskTab.Text = homeDockPanelSetting.UpdateMyTasks;

            //chkDocumentPendingApprovalTab.Checked = !homeDockPanelSetting.HideMyPendingApprovals;
            //txtDocumentPendingApprovalTab.Text = homeDockPanelSetting.UpdateMyPendingApprovals;

            //chkSMSModules.Checked = !homeDockPanelSetting.HideSMSModules;
            //chkGovernanceModules.Checked = !homeDockPanelSetting.HideGovernanceModules;
            chkServiceCatalog.Checked = serviceDockPanelSetting.ShowServiceCatalog;

            //ddlModulePanelOrder.SelectedIndex = ddlModulePanelOrder.Items.IndexOfText(homeDockPanelSetting.ModulePanelOrder.ToString());
            //ddlMyTicketPanelOrder.SelectedIndex = ddlMyTicketPanelOrder.Items.IndexOfText(homeDockPanelSetting.MyTicketPanelOrder.ToString());
            //ddlServiceCatalogOrder.SelectedIndex = ddlServiceCatalogOrder.Items.IndexOfText(homeDockPanelSetting.ServiceCatalogOrder.ToString());

            //txtNoOfTickets.Text = homeDockPanelSetting.NoOfPreviewTickets.ToString();
            chkShowServiceIcons.Checked = serviceDockPanelSetting.ShowIcons;
            rblServiceViewType.SelectedValue = serviceDockPanelSetting.ServiceViewType;

            cmbIconSize.SelectedIndex = cmbIconSize.Items.IndexOfValue(Convert.ToString(serviceDockPanelSetting.IconSize));

            chkPanel.Checked = serviceDockPanelSetting.ShowPanel;

            ddlPanel.SelectedValue = Convert.ToString(serviceDockPanelSetting.PanelId);            

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //homeDockPanelSetting.ShowTitle = chkTitle.Checked;
            //homeDockPanelSetting.Title = txtTitle.Text.Trim();

            //homeDockPanelSetting.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            //homeDockPanelSetting.WelcomeDesc = txtWelcomeDesc.Text.Trim();

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
            //homeDockPanelSetting.HideGovernanceModules = !chkGovernanceModules.Checked;
            serviceDockPanelSetting.ShowServiceCatalog = chkServiceCatalog.Checked;
            serviceDockPanelSetting.ServiceViewType = rblServiceViewType.SelectedValue;

            serviceDockPanelSetting.ShowIcons = chkShowServiceIcons.Checked;
            serviceDockPanelSetting.IsServiceDocPanel = true;

            //homeDockPanelSetting.EnableNewButton = chkEnableNewButton.Checked;

            //homeDockPanelSetting.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());

            //homeDockPanelSetting.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            //homeDockPanelSetting.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            //homeDockPanelSetting.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);
            serviceDockPanelSetting.ShowPanel = chkPanel.Checked;
            string SelectedPanel = ddlPanel.SelectedValue == "" ? "0" : ddlPanel.SelectedValue;
            serviceDockPanelSetting.PanelId = Convert.ToInt64(SelectedPanel);

            if (cmbIconSize.Value != null)
                serviceDockPanelSetting.IconSize = Convert.ToInt32(cmbIconSize.SelectedItem.Value);
           
        }

        public void CopyFromWebpart(ServiceDockPanelSetting webpart)
        {
            //homeDockPanelSetting.ShowTitle = chkTitle.Checked;
            //homeDockPanelSetting.Title = txtTitle.Text.Trim();
            //homeDockPanelSetting.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            //homeDockPanelSetting.WelcomeDesc = txtWelcomeDesc.Text.Trim();
            //homeDockPanelSetting.HideWaitingOnMeTab = !chkWaitingOnMeTab.Checked;
            //homeDockPanelSetting.UpdateWaitingOnMeTab = "";// txtWaitingOnMeTab.Text.Trim();
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
            //homeDockPanelSetting.HideGovernanceModules = !chkGovernanceModules.Checked;
            serviceDockPanelSetting.ShowServiceCatalog = chkServiceCatalog.Checked;
            serviceDockPanelSetting.ServiceViewType = rblServiceViewType.SelectedValue;

            serviceDockPanelSetting.ShowIcons = chkShowServiceIcons.Checked;

            //homeDockPanelSetting.EnableNewButton = chkEnableNewButton.Checked;
            //homeDockPanelSetting.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());
            //homeDockPanelSetting.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            //homeDockPanelSetting.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            //serviceDockPanelSetting.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);
            serviceDockPanelSetting.ShowPanel = chkPanel.Checked;
            serviceDockPanelSetting.PanelId = Convert.ToInt64(ddlPanel.SelectedValue);
            serviceDockPanelSetting.IsServiceDocPanel = true;

            serviceDockPanelSetting.IconSize = webpart.IconSize;

            webpart = serviceDockPanelSetting;
        }

        public void CopyFromControl(ServiceDockPanelSetting webpart)
        {
            //webpart.ShowTitle = chkTitle.Checked;
            //webpart.Title = txtTitle.Text.Trim();
            //webpart.WelcomeHeading = txtWelcomeTitle.Text.Trim();
            //webpart.WelcomeDesc = txtWelcomeDesc.Text.Trim();
            //webpart.HideWaitingOnMeTab = !chkWaitingOnMeTab.Checked;
            //webpart.UpdateWaitingOnMeTab = "";// txtWaitingOnMeTab.Text.Trim();
            //webpart.HideMyDepartmentTickets = !chkMyDepartmentTab.Checked;
            //webpart.UpdateMyDepartmentTickets = txtMyDepartmentTab.Text.Trim();
            //webpart.HideMyDivisionTickets = !chkMyDivisionTab.Checked;
            //webpart.UpdateMyDivisionTickets = txtMyDivisionTab.Text.Trim();
            //webpart.HideMyPendingApprovals = !chkDocumentPendingApprovalTab.Checked;
            //webpart.UpdateMyPendingApprovals = txtDocumentPendingApprovalTab.Text.Trim();
            //webpart.HideMyProject = !chkMyProjectsTab.Checked;
            //webpart.UpdateMyProject = txtMyProjectsTab.Text.Trim();
            //webpart.HideMyTasks = !chkMyTaskTab.Checked;
            //webpart.UpdateMyTasks = txtMyTaskTab.Text.Trim();
            //webpart.HideMyTickets = !chkMyReqeustTab.Checked;
            //webpart.UpdateMyTickets = txtMyRequestTab.Text.Trim();
            //webpart.HideMyClosedTickets = !chkMyClosedRequestsTab.Checked;
            //webpart.UpdateMyClosedTickets = txtMyClosedRequestsTab.Text.Trim();
            //webpart.HideSMSModules = !chkSMSModules.Checked;
            //webpart.HideGovernanceModules = !chkGovernanceModules.Checked;
            webpart.ShowServiceCatalog = chkServiceCatalog.Checked;
            //webpart.EnableNewButton = chkEnableNewButton.Checked;
            //webpart.NoOfPreviewTickets = UGITUtility.StringToInt(txtNoOfTickets.Text.Trim());
            //webpart.ModulePanelOrder = UGITUtility.StringToInt(ddlModulePanelOrder.Value);
            //webpart.MyTicketPanelOrder = UGITUtility.StringToInt(ddlMyTicketPanelOrder.Value);
            //webpart.ServiceCatalogOrder = UGITUtility.StringToInt(ddlServiceCatalogOrder.Value);
            webpart.ShowIcons = chkShowServiceIcons.Checked;
            webpart.ServiceViewType = rblServiceViewType.SelectedValue;

            serviceDockPanelSetting.IsServiceDocPanel = true;

            webpart.ShowPanel = chkPanel.Checked;
            webpart.PanelId = Convert.ToInt64(ddlPanel.SelectedValue);

             webpart.IconSize = serviceDockPanelSetting.IconSize;

            serviceDockPanelSetting = webpart;
        }

    }
}