using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class ApplicationUptimeProperties : UserControl
    {
        public Applicationuptimesetting moduleDockPanelSetting { get; set; }
        public string DataFilterUrl { get; set; }
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = moduleDockPanelSetting.Title;
            //txtName.Text = moduleDockPanelSetting.Name;         
            if (moduleDockPanelSetting.ModuleName != null)
                ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(moduleDockPanelSetting.ModuleName));
            chkTitle.Checked = moduleDockPanelSetting.ShowTitle;

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            moduleDockPanelSetting.ShowTitle = chkTitle.Checked;
            moduleDockPanelSetting.Title = txtTitle.Text;
            //moduleDockPanelSetting.Name = txtName.Text;         
            moduleDockPanelSetting.ModuleName = Convert.ToString(ddlModule.Value);
        }

        protected void ddlModule_Init(object sender, EventArgs e)
        {
            ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> modules = moduleManager.Load();
            ddlModule.Items.Clear();

            if (modules.Count > 0)
            {
                ddlModule.TextField = "Title";
                ddlModule.ValueField = "ModuleName";
                ddlModule.DataSource = modules;
                ddlModule.DataBind();
                //if (moduleDockPanelSetting.ModuleName != null)
                //    ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(moduleDockPanelSetting.ModuleName)));
            }
        }
        public void CopyFromWebpart(Applicationuptimesetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            //webpart.Name = txtName.Text;
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            moduleDockPanelSetting = webpart;
        }
        public void CopyFromControl(Applicationuptimesetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            // webpart.Name = txtName.Text;           
            webpart.ModuleName = Convert.ToString(ddlModule.Value);
            moduleDockPanelSetting = webpart;
        }
    }
}