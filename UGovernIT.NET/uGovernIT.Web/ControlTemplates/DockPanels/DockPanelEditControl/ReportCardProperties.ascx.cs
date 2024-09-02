
using System;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web
{
    public partial class ReportCardProperties : UserControl
    {
        public ReportDockPanelsetting reportCardDockPanelSetting { get; set; }
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = reportCardDockPanelSetting.Title;
            chkTitle.Checked = reportCardDockPanelSetting.ShowTitle;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            reportCardDockPanelSetting.ShowTitle = chkTitle.Checked;
            reportCardDockPanelSetting.Title = txtTitle.Text;
        }

        public void CopyFromWebpart(ReportDockPanelsetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            reportCardDockPanelSetting = webpart;
        }

        public void CopyFromControl(ReportDockPanelsetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            //webpart.ShowTitle = chkTitle.Checked;
            //webpart.Title = txtTitle.Text;
            //webpart.ContentTitle = txtTitle.Text;
            //webpart.Name = txtTitle.Text;



            //webpart.FilterView = Convert.ToString(ddlViewFilter.Value);
            //webpart.IncludeOpen = chkIncludeOpenTickets.Checked;
            //webpart.ShowSLAName = chkShowSLAName.Checked;
            //webpart.StringOfSelectedModule = StringOfSelectedModule;
            //webpart.SLAEnableModules = SLAEnableModules;
            //webpart.Module = Module;

            reportCardDockPanelSetting = webpart;
        }

    }
}