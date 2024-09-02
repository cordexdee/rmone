using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;


namespace uGovernIT.Web
{
    public partial class HomeCardProperties : System.Web.UI.UserControl
    {
        public HomeCardPanelSetting homeCardPanelSetting { get; set; }
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = homeCardPanelSetting.Title;
            chkTitle.Checked = homeCardPanelSetting.ShowTitle;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            homeCardPanelSetting.ShowTitle = chkTitle.Checked;
            homeCardPanelSetting.Title = txtTitle.Text;
        }

        public void CopyFromWebpart(HomeCardPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            homeCardPanelSetting = webpart;
        }

        public void CopyFromControl(HomeCardPanelSetting webpart)
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

            homeCardPanelSetting = webpart;
        }
    }
}