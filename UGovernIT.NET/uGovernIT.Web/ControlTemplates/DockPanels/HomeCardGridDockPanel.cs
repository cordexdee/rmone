using DevExpress.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class HomeCardGridDockPanel : DockPanel
    {
        public HomeCardGridPanelSetting homeCardGridPanelSetting { get { return DockSetting as HomeCardGridPanelSetting; } set { DockSetting = value; } }
        CustomFilteredTickets customFilteredTickets;
        protected override void OnInit(EventArgs e)
        {

            this.Styles.Content.CssClass += "AdditionCss";
            this.ClientSideEvents.Init = "function(s,e){  setTimeout(function(){ if(s.mainElement!=null && s.mainElement!=undefined && s.mainElement!=''){s.mainElement.parentElement.style.height='';s.mainElement.style.height='';if(s.mainElement.children[0]!=null && s.mainElement.children[0]!=undefined && s.mainElement.children[0]!=''){s.mainElement.children[0].style.height='';if(s.mainElement.children[0].children[0]!=null && s.mainElement.children[0].children[0]!=undefined && s.mainElement.children[0].children[0]!=''){s.mainElement.children[0].children[0].style.height='';if(s.mainElement.children[0].children[0].children[0]!=undefined){s.mainElement.children[0].children[0].children[0].style.height='';}}}}},1000); }";
            if (homeCardGridPanelSetting != null)
            {
                if (homeCardGridPanelSetting != null && !string.IsNullOrEmpty(homeCardGridPanelSetting.Title))
                    this.HeaderText = $"{homeCardGridPanelSetting.Title} ({this.GetType().Name})"; //homeCardGridPanelSetting.Title;
                else
                    this.HeaderText = this.GetType().Name; //"Task list";

                if (!string.IsNullOrEmpty(homeCardGridPanelSetting.ContentTitle))
                    this.HeaderText += " - " + homeCardGridPanelSetting.ContentTitle;
            }
            if (NeedData)
            {
                customFilteredTickets = (CustomFilteredTickets)Page.LoadControl("~/ControlTemplates/Shared/CustomFilteredTickets.ascx");
                customFilteredTickets.ID = "dashboardSLADockPanel";

                var HomecardView = Page.Master.Master.FindControlRecursive("HomeCardView");
                HomecardView.Visible = true;
                //if (!string.IsNullOrEmpty(ticketDockPanelSetting.ModuleName))
                //    customFilteredTickets.ModuleName = ticketDockPanelSetting.ModuleName;
                //if (ticketDockPanelSetting.PageSize > 0)
                //    customFilteredTickets.PageSize = ticketDockPanelSetting.PageSize;
                //if (!ticketDockPanelSetting.HideNewbutton)
                //    customFilteredTickets.HideNewTicketButton = ticketDockPanelSetting.HideNewbutton;
                //if (!ticketDockPanelSetting.HideModuleDescription)
                //    customFilteredTickets.HideModuleDesciption = ticketDockPanelSetting.HideModuleDescription;
                //if (!ticketDockPanelSetting.HideModuleLogo)
                //    customFilteredTickets.HideModuleDesciption = ticketDockPanelSetting.HideModuleLogo;
                //if (!ticketDockPanelSetting.HideStatusOverProgressBar)
                //    customFilteredTickets.HideStatusOverProgressBar = ticketDockPanelSetting.HideStatusOverProgressBar;
                customFilteredTickets.ModuleName = "CON";

                //satrt
                string ticketModuleName = Convert.ToString(Request["Md"]);
                string status = Convert.ToString(Request["St"]);
                //string UserType = Convert.ToString(Request["UserType"]);
                //UserType = UserType.Replace(" ", string.Empty);
                ApplicationContext context = HttpContext.Current.GetManagerContext();
                TicketManager ticketMgr = new TicketManager(context);
                ModuleViewManager moduleMgr = new ModuleViewManager(context);
                UGITModule homeModule = moduleMgr.LoadByName(ticketModuleName);

                DataTable dthometickets = ticketMgr.GetAllTickets(homeModule);
                if (status == "all")
                {
                    customFilteredTickets.FilteredTable = dthometickets;
                }
                else
                {
                    // mystatus = (my leads & my contacts)
                    if (status == "mystatus" || status == "myprojects" || status == "myactionitems" || status == "myprojectsclosein4weeks")
                    {
                        UserProfileManager umanager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();
                        string moduleTable = moduleMgr.GetModuleTableName(ticketModuleName);
                        string ownerColumn = string.Empty;
                        string userid = context.CurrentUser.Id;
                        var role = umanager.GetUserRoles(userid).Select(x => x.Id).ToList();

                        if (moduleTable == DatabaseObjects.Tables.CRMContact || moduleTable == DatabaseObjects.Tables.CRMCompany || moduleTable == DatabaseObjects.Tables.Opportunity)
                            ownerColumn = DatabaseObjects.Columns.CRMowner;
                        else if (moduleTable == DatabaseObjects.Tables.Lead || moduleTable == DatabaseObjects.Tables.CRMProject)
                            ownerColumn = DatabaseObjects.Columns.Owner;

                        StringBuilder sbQuery = new StringBuilder();

                        if (status == "mystatus" || status == "myprojectsclosein4weeks")
                        {
                            foreach (var item in role)
                            {
                                sbQuery.Append($"{ownerColumn} like '%{item}%' OR ");
                            }
                        }
                        else if (status == "myprojects")
                        {
                            sbQuery.Append($"{DatabaseObjects.Columns.Owner} like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.Estimator} like '%{userid}%' OR ");
                            sbQuery.Append($"ActionUserTypes like '%{userid}%' OR ");
                            sbQuery.Append($"ActionUsers like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{userid}%' OR ");
                            sbQuery.Append($"ProjectExecutive like '%{userid}%' OR ");
                            sbQuery.Append($"ProjectManager like '%{userid}%' OR ");
                            sbQuery.Append($"Superintendent like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.TicketInitiator} like '%{userid}%' OR ");

                            foreach (var item in role)
                            {
                                sbQuery.Append($"{DatabaseObjects.Columns.Owner} like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.Estimator} like '%{item}%' OR ");
                                sbQuery.Append($"ActionUserTypes like '%{item}%' OR ");
                                sbQuery.Append($"ActionUsers like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{item}%' OR ");
                                sbQuery.Append($"ProjectExecutive like '%{item}%' OR ");
                                sbQuery.Append($"ProjectManager like '%{item}%' OR ");
                                sbQuery.Append($"Superintendent like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.TicketInitiator} like '%{item}%' OR ");
                            }
                        }
                        else if (status == "myactionitems")
                        {
                            sbQuery.Append($"ActionUserTypes like '%{userid}%' OR ");
                            sbQuery.Append($"ActionUsers like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{userid}%' OR ");
                            sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{userid}%' OR ");

                            foreach (var item in role)
                            {
                                sbQuery.Append($"ActionUserTypes like '%{item}%' OR ");
                                sbQuery.Append($"ActionUsers like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUserTypes} like '%{item}%' OR ");
                                sbQuery.Append($"{DatabaseObjects.Columns.TicketStageActionUsers} like '%{item}%' OR ");
                            }
                        }

                        string query = !string.IsNullOrEmpty(Convert.ToString(sbQuery)) ? Convert.ToString(sbQuery) : string.Empty;

                        DataRow[] tickets = dthometickets.Select($"{query} {ownerColumn} like '%{userid}%'");
                        if (status == "myprojectsclosein4weeks")
                        {
                            // My Projects Due in 4 Week
                            var dueDate = DateTime.Today.AddDays(28);
                            var currentDate = DateTime.Today;
                            if (tickets.Count() > 0)
                            {
                                dthometickets = tickets.CopyToDataTable();
                                tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} <> 1  AND {DatabaseObjects.Columns.EstimatedConstructionEnd} >= #{currentDate}# AND {DatabaseObjects.Columns.EstimatedConstructionEnd} <= #{dueDate}#");
                            }
                        }

                        if (tickets.Count() > 0)
                            customFilteredTickets.FilteredTable = tickets.CopyToDataTable();
                    }
                    else if (status == "liveprojects")
                    {
                        customFilteredTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} <= 8").CopyToDataTable();
                    }
                    else if (status == "pipeline")
                    {
                        customFilteredTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} < 8").CopyToDataTable();
                    }
                    else if (status == "Projectsreadytostart")
                    {
                        customFilteredTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 0 and {DatabaseObjects.Columns.StageStep} = 7").CopyToDataTable();
                    }
                    else if (status == "closed")
                    {
                        customFilteredTickets.FilteredTable = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1").CopyToDataTable();
                    }
                    else if (status == "closethisweek")
                    {
                        // Total Projects Due in this Week                             
                        DateTime dtMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                        //DateTime dtSaturday = DateTime.Now.StartOfWeek(DayOfWeek.Saturday).AddDays(7);
                        //DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and {DatabaseObjects.Columns.CloseDate} <> '' and  {DatabaseObjects.Columns.CloseDate} >= #{dtSunday}# and {DatabaseObjects.Columns.CloseDate} <= #{dtSaturday}#");
                        DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and  {DatabaseObjects.Columns.CloseDate} >= #{dtMonday}#");

                        if (tickets.Count() > 0)
                            customFilteredTickets.FilteredTable = tickets.CopyToDataTable();
                    }
                    else if (status == "closedthisyear")
                    {
                        // Total Projects Closed this year.                             
                        DateTime now = DateTime.Now;
                        var firstDay = new DateTime(now.Year, 1, 1);
                        var lastDay = firstDay.AddMonths(12).AddDays(-1);

                        DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} = 1 and {DatabaseObjects.Columns.CloseDate} is not null and  {DatabaseObjects.Columns.CloseDate} >= #{firstDay}# and {DatabaseObjects.Columns.CloseDate} <= #{lastDay}#");

                        if (tickets.Count() > 0)
                            customFilteredTickets.FilteredTable = tickets.CopyToDataTable();
                    }
                    else if (status == "closein4weeks")
                    {
                        // Total Projects Due in 4 Week
                        var dueDate = DateTime.Now.AddDays(28);
                        var currentDate = DateTime.Now;
                        DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} <> 1  AND {DatabaseObjects.Columns.EstimatedConstructionEnd} >= #{currentDate}# AND {DatabaseObjects.Columns.EstimatedConstructionEnd} <= #{dueDate}#");

                        if (tickets.Count() > 0)
                            customFilteredTickets.FilteredTable = tickets.CopyToDataTable();
                    }

                    else if (status == "startedthismonth")
                    {
                        DateTime now = DateTime.Now;
                        var firstDay = new DateTime(now.Year, now.Month, 1);
                        var lastDay = firstDay.AddMonths(1).AddDays(-1);

                        DataRow[] tickets = dthometickets.Select($"{DatabaseObjects.Columns.Closed} <> 1  AND {DatabaseObjects.Columns.EstimatedConstructionStart} >= #{firstDay}# AND {DatabaseObjects.Columns.EstimatedConstructionStart} <= #{lastDay}#");

                        if (tickets.Count() > 0)
                            customFilteredTickets.FilteredTable = tickets.CopyToDataTable();
                    }
                }

                customFilteredTickets.HideModuleDesciption = false;
                //end


            }

            if (customFilteredTickets != null)
                this.Controls.Add(customFilteredTickets);
            base.OnInit(e);
        }

    }
}