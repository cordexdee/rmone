using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class TaskDockPanel : DockPanel
    {
        public TaskDockPanelSetting taskDockPanelSetting { get { return DockSetting as TaskDockPanelSetting; } set { DockSetting = value; } }
        Control control;
        //UserService UserService;
        protected override void OnInit(EventArgs e)
        {

            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            if (taskDockPanelSetting != null)
            {
                if (taskDockPanelSetting != null && !string.IsNullOrEmpty(taskDockPanelSetting.Title))
                    this.HeaderText = taskDockPanelSetting.Title;
                else
                    this.HeaderText = "Task list";
                if (!string.IsNullOrEmpty(taskDockPanelSetting.ContentTitle))
                    this.HeaderText = taskDockPanelSetting.ContentTitle;
            }

            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            if (page.Request.QueryString["tkt"] != null)
            {
                if (page.Request.QueryString["tkt"].ToString() == "tkt")
                {
                    control = (TicketTaskControl)page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/TicketTaskControl.ascx");
                    control.ID = "dashboardSLADockPanel";
                }
                else if (page.Request.QueryString["tkt"].ToString() == "morit")
                {
                    UserMoreThenJustIT userMoreThenJustIT = (UserMoreThenJustIT)page.LoadControl("~/ControlTemplates/uGovernIT/UserMoreThenJustIT.ascx");
                    userMoreThenJustIT.ID = "dashboardSLADockPanel";
                    //taskDockPanelSetting.Title = "More Than Just IT";
                    userMoreThenJustIT.PageTitleForLandingPage = "More Than Just IT";

                    control = userMoreThenJustIT;
                }
                else if (page.Request.QueryString["tkt"].ToString() == "newt")
                {
                    control = (UserService)page.LoadControl("~/ControlTemplates/uGovernIT/UserService.ascx");
                    control.ID = "dashboardSLADockPanel";
                }
            }
            else
            {

                control = (TaskUserControl)page.LoadControl("~/ControlTemplates/uGovernIT/dashboard/TaskUserControl.ascx");
                control.ID = "dashboardSLADockPanel";
            }
            return control;
        }
    }
}