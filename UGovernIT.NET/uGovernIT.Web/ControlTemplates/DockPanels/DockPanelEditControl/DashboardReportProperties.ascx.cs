using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web
{
    public partial class DashboardReportProperties : UserControl
    {
        public DashboardReportPanelSetting dashboardReportPanelSetting { get; set; }

        protected override void OnInit(EventArgs e)
        {
            chkTitle.Checked = dashboardReportPanelSetting.ShowTitle;
            txtTitle.Text = dashboardReportPanelSetting.Title;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dashboardReportPanelSetting.ShowTitle = chkTitle.Checked;
            dashboardReportPanelSetting.Title = txtTitle.Text;
        }

        public void CopyFromWebpart(DashboardReportPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            dashboardReportPanelSetting = webpart;
        }

        public void CopyFromControl(DashboardReportPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            dashboardReportPanelSetting = webpart;
        }
    }
}