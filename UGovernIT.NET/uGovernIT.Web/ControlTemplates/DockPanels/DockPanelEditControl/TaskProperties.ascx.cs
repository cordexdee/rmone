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
    public partial class TaskProperties : UserControl
    {


        public TaskDockPanelSetting taskDockPanelSetting { get; set; }
        public ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = taskDockPanelSetting.Title;
            chkTitle.Checked = taskDockPanelSetting.ShowTitle;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            taskDockPanelSetting.ShowTitle = chkTitle.Checked;
            taskDockPanelSetting.Title = txtTitle.Text;
        }

        public void CopyFromWebpart(TaskDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;

            taskDockPanelSetting = webpart;
        }

        public void CopyFromControl(TaskDockPanelSetting webpart)
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

            taskDockPanelSetting = webpart;
        }
    }
}