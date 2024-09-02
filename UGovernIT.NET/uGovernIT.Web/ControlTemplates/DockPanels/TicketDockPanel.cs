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
    
    public class TicketDockPanel:DockPanel
    {
        public TicketDockPanelSetting ticketDockPanelSetting { get { return DockSetting as TicketDockPanelSetting; } set { DockSetting = value; } }
        CustomFilteredTickets control = null;
        protected override void OnInit(EventArgs e)
        {
            this.ClientSideEvents.Init = "function(s,e){ }";
            
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
                    this.HeaderText = this.GetType().Name; //"Ticket";

                if (!string.IsNullOrEmpty(ticketDockPanelSetting.ModuleName))
                this.HeaderText += " - " + ticketDockPanelSetting.ModuleName;
            }

          
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = (CustomFilteredTickets)page.LoadControl("~/ControlTemplates/shared/CustomFilteredTickets.ascx");
            control.ID = "customFilter";
            if (ticketDockPanelSetting != null)
            {
                if (!string.IsNullOrEmpty(ticketDockPanelSetting.ModuleName))
                    control.ModuleName = ticketDockPanelSetting.ModuleName;
                if (ticketDockPanelSetting.PageSize > 0)
                    control.PageSize = ticketDockPanelSetting.PageSize;
                if (!ticketDockPanelSetting.HideNewbutton)
                    control.HideNewTicketButton = ticketDockPanelSetting.HideNewbutton;
                //if (!ticketDockPanelSetting.HideModuleDescription)
                    control.HideModuleDesciption = ticketDockPanelSetting.HideModuleDescription;
                //if (!ticketDockPanelSetting.HideModuleLogo)
                    control.HideModuleDesciption = ticketDockPanelSetting.HideModuleLogo;
                if (!ticketDockPanelSetting.HideStatusOverProgressBar)
                    control.HideStatusOverProgressBar = ticketDockPanelSetting.HideStatusOverProgressBar;
                control.ShowBandedrows = ticketDockPanelSetting?.ShowBandedRows ?? false;
                control.ShowCompactView = ticketDockPanelSetting?.ShowCompactRows ?? false;
                if (!string.IsNullOrEmpty(ticketDockPanelSetting.Name))
                    control.ViewName = ticketDockPanelSetting.Name;
            }

            return control;
        }
    }
}