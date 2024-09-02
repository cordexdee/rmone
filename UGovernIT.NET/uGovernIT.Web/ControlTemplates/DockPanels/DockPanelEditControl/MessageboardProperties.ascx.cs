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
    public partial class MessageboardProperties : UserControl
    {
        public MessageboardDockPanelSetting messageDockPanelSetting { get; set; } 
        protected override void OnInit(EventArgs e)
        {
            txtTitle.Text = messageDockPanelSetting.Title;
            //txtName.Text = messageDockPanelSetting.Name;         
            chkTitle.Checked = messageDockPanelSetting.ShowTitle;

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            messageDockPanelSetting.ShowTitle = chkTitle.Checked;
            messageDockPanelSetting.Title = txtTitle.Text;
           // messageDockPanelSetting.Name = txtName.Text;         
          
        }

        
        public void CopyFromWebpart(MessageboardDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
           // webpart.Name = txtName.Text;
                
            messageDockPanelSetting = webpart;
        }
        public void CopyFromControl(MessageboardDockPanelSetting webpart)
        {
            webpart.ShowTitle = chkTitle.Checked;
            webpart.Title = txtTitle.Text;
            //webpart.Name = txtName.Text;           
        
            messageDockPanelSetting = webpart;
        }

    }
}
