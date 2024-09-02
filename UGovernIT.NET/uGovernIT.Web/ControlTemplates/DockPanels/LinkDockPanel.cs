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
    
    public class LinkDockPanel:DockPanel
    {
        public LinkPanelSetting linkDockPanelSetting { get { return DockSetting as LinkPanelSetting; } set { DockSetting = value; } }
        LinksUserControl linksUserControl = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=null && s.mainElement.children[0].children[0].children[0]!=undefined && s.mainElement.children[0].children[0].children[0]!=''){s.mainElement.children[0].children[0].children[0].style.height='';}}}}}, 100);}";
            if (linkDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(linkDockPanelSetting.Title))
                {
                    if (Request.QueryString["Control"] != null && Convert.ToString(Request.QueryString["Control"]).EqualsIgnoreCase("pageeditorview"))
                    {
                        this.HeaderText = $"{linkDockPanelSetting.Title} ({this.GetType().Name})";
                    }
                    else
                    {
                        this.HeaderText = linkDockPanelSetting.Title;
                    }
                    
                }
                else
                    this.HeaderText = this.GetType().Name;         
            }
            if (NeedData)
            {
                linksUserControl = (LinksUserControl)Page.LoadControl("~/ControlTemplates/uGovernit/LinksUserControl/LinksUserControl.ascx");
                linksUserControl.ID = "Links";
                linksUserControl.HideControlBorder = linkDockPanelSetting.HideControlBorder;
                linksUserControl.ControlWidth = linkDockPanelSetting.ControlWidth;
                linksUserControl.LinkView = linkDockPanelSetting.LinkView;
            }

            if (linksUserControl != null)
                this.Controls.Add(linksUserControl);
            base.OnInit(e);
        }
    }
}