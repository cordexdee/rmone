using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    
    public class MessageBoardDockPanel:DockPanel
    {
        public MessageboardDockPanelSetting ticketDockPanelSetting { get { return DockSetting as MessageboardDockPanelSetting; } set { DockSetting = value; } }
        uGovernITMessageBoardUserControl control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=null && s.mainElement.children[0].children[0].children[0]!=undefined && s.mainElement.children[0].children[0].children[0]!=''){s.mainElement.children[0].children[0].children[0].style.height='';}}}}}, 100);}";
            if (ticketDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(ticketDockPanelSetting.Title))
                {
                    if (Request.QueryString["Control"] != null && Convert.ToString(Request.QueryString["Control"]).EqualsIgnoreCase("pageeditorview"))
                    {
                        this.HeaderText = $"{ticketDockPanelSetting.Title} ({this.GetType().Name})"; //ticketDockPanelSetting.Title;
                    }
                    else
                    {
                        this.HeaderText = ticketDockPanelSetting.Title;
                    }                     
                }
                else
                    this.HeaderText = this.GetType().Name; //"MessageBoard";         
            }
            
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (uGovernITMessageBoardUserControl)page.LoadControl("~/ControlTemplates/uGovernit/uGovernITMessageBoard/uGovernITMessageBoardUserControl.ascx");
            control.ID = "messageboard";

            return control;
        }
    }
}