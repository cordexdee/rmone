using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;
using System.Linq;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public partial class DashboardProperties : UserControl
    {
        public DashboardDockPanelSetting dashboardDockPanelSetting { get; set; }
        protected override void OnInit(EventArgs e)
        {
            ddlDashboardView.ClientEnabled = true;
            ddlDashboardView.ClientInstanceName = "ddlDashboardView";
            ddlDashboardView.Enabled = true;
            txtTitle.Text = dashboardDockPanelSetting.Title;
            chkTitle.Checked = dashboardDockPanelSetting.ShowTitle;
            ddlDashboardView.Value = dashboardDockPanelSetting.Name;
            if (!string.IsNullOrWhiteSpace(dashboardDockPanelSetting.DashboardGroup))
            {
                //ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOf(ddlDashboardGroup.Items.FindByValue(dashboardDockPanelSetting.ViewName));
                ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOf(ddlDashboardGroup.Items.FindByValue(dashboardDockPanelSetting.DashboardGroup));
                BindViewName(dashboardDockPanelSetting.DashboardGroup);                
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(dashboardDockPanelSetting.DashboardGroup))
            {
                //ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOf(ddlDashboardGroup.Items.FindByValue(dashboardDockPanelSetting.ViewName));
                if (!Page.IsPostBack)
                {
                    ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOf(ddlDashboardGroup.Items.FindByValue(dashboardDockPanelSetting.DashboardGroup));
                    BindViewName(dashboardDockPanelSetting.DashboardGroup); 
                }
            }
            dashboardDockPanelSetting.Title = txtTitle.Text;
            dashboardDockPanelSetting.ShowTitle = chkTitle.Checked;
            dashboardDockPanelSetting.Name = Convert.ToString(ddlDashboardView.Value);
            dashboardDockPanelSetting.ViewName = Convert.ToString(ddlDashboardView.Value);
            dashboardDockPanelSetting.DashboardGroup = Convert.ToString(ddlDashboardGroup.Value);
        }
        private void bindGroup()
        {          
            ddlDashboardGroup.Items.Clear();
            ddlDashboardGroup.Items.Add(new ListEditItem("Select View Type", ""));
            ddlDashboardGroup.Items.Add(new ListEditItem("Indivisible Dashboards", "Indivisible Dashboards"));
            ddlDashboardGroup.Items.Add(new ListEditItem("Super Dashboards", "Super Dashboards"));
            ddlDashboardGroup.Items.Add(new ListEditItem("Side Dashboards", "Side Dashboards"));
            ddlDashboardGroup.Items.Add(new ListEditItem("Common Dashboards", "Common Dashboards"));
            if (!string.IsNullOrWhiteSpace(dashboardDockPanelSetting.DashboardGroup))
                ddlDashboardGroup.SelectedIndex = ddlDashboardGroup.Items.IndexOf(ddlDashboardGroup.Items.FindByValue(dashboardDockPanelSetting.Name));
        }
        public void BindViewName(string dashboardGroup)
        {
            DashboardPanelViewManager dashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
            List<DashboardPanelView> modules = dashboardPanelViewManager.Load(x => x.ViewType == dashboardGroup);
            ddlDashboardView.Items.Clear();

            if (modules.Count > 0)
            {
                ddlDashboardView.Text = "Select View";
                ddlDashboardView.TextField = "ViewName";
                ddlDashboardView.ValueField = "ViewName";
                ddlDashboardView.DataSource = modules.OrderBy(x=>x.Title);
                ddlDashboardView.DataBind();
                if (!string.IsNullOrWhiteSpace(dashboardDockPanelSetting.ViewName))
                    ddlDashboardView.SelectedIndex = ddlDashboardView.Items.IndexOf(ddlDashboardView.Items.FindByValue(dashboardDockPanelSetting.ViewName));
            }

        }

        public void CopyFromWebpart(DashboardDockPanelSetting webpart)
        {
            webpart.Title = txtTitle.Text;
            webpart.ShowTitle = chkTitle.Checked;
            webpart.DashboardGroup = Convert.ToString(ddlDashboardGroup.Value);
            webpart.ViewName = Convert.ToString(ddlDashboardView.Value);
            dashboardDockPanelSetting = webpart;
        }
        public void CopyFromControl(DashboardDockPanelSetting webpart)
        {
            webpart.Title = txtTitle.Text;
            webpart.ShowTitle = chkTitle.Checked;
            webpart.DashboardGroup = Convert.ToString(ddlDashboardGroup.Value);
            webpart.ViewName = Convert.ToString(ddlDashboardView.Value);
            dashboardDockPanelSetting = webpart;
        }

        protected void ddlGroudp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ASPxComboBox comboBox = sender as ASPxComboBox;
            string selectedValue = Convert.ToString(comboBox.Value);
            if (!string.IsNullOrEmpty(selectedValue))
                BindViewName(selectedValue);
        }

        protected void ddlDashboardView_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                BindViewName(e.Parameter);
        }

        protected void ddlDashboardGroup_Init(object sender, EventArgs e)
        {
            bindGroup();
        }
    }
}
