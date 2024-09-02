using DevExpress.Web;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Core;
using System.Collections.Generic;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CustomProjectTasks : UserControl
    {
        public string ProjectTaskDescription { get; set; }

        Tab tasklistTab = null;
        Tab scrumTab = null;
        public UGITModule currentUGITModule = null;
        DataRow currentTicket;
        TicketManager TicketManagerObj = null;
        ModuleViewManager ModuleManagerObj = null;
        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        
        protected override void OnInit(EventArgs e)
        {
            TicketManagerObj = new TicketManager(AppContext);
            ModuleManagerObj = new ModuleViewManager(AppContext);
            tasklistTab = projecttaskTab.Tabs.FindByName("tasklist");
            scrumTab = projecttaskTab.Tabs.FindByName("scrum");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            moduleDescription.Text = ProjectTaskDescription;

            tasklistTab.Visible = true;
            scrumTab.Visible = true;

            if (!IsPostBack)
            {
                BindModule();
                BindProjects();

                string moduleVal = UGITUtility.GetCookieValue(Request, "Taskmodule");
                if (!string.IsNullOrEmpty(moduleVal) && cmbModuleList.Items.Count > 0)
                    cmbModuleList.SelectedIndex = cmbModuleList.Items.IndexOf(cmbModuleList.Items.FindByValue(moduleVal));
                string projectId = UGITUtility.GetCookieValue(Request, "TaskProjectId");
                if (!string.IsNullOrEmpty(projectId) && cmbProject.Items.Count > 0)
                    cmbProject.SelectedIndex = cmbProject.Items.IndexOf(cmbProject.Items.FindByValue(projectId));

                BindProjectDetails();
            }
            else
                BindProjects();
        }

        private void BindModule()
        {
            List<UGITModule> lstModule = ModuleManagerObj.LoadAllModule();
            if (lstModule.Count > 0)
            {
                List<UGITModule> moduleRows = lstModule.Where(x=>x.EnableModule == true && x.ShowTasksInProjectTasks == true).ToList();  //dtModule.Select(string.Format("{0}='1' AND {1}='1'", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.ShowTasksInProjectTasks));
                if (moduleRows != null && moduleRows.Count == 0)
                {
                    moduleRows = lstModule.Where(x => x.ModuleName == ModuleNames.PMM).ToList();
                    if (moduleRows.Count == 0)
                        return;
                }

                //string[] defaultView = new string[] { DatabaseObjects.Columns.ModuleName, DatabaseObjects.Columns.Id, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.ShowTasksInProjectTasks };
                //dtModule.DefaultView.Sort = DatabaseObjects.Columns.Title;
                //dtModule = dtModule.DefaultView.ToTable(true, defaultView);

                cmbModuleList.DataSource = moduleRows;
                cmbModuleList.DataBind();

                if (cmbModuleList.SelectedIndex < 0)
                    cmbModuleList.SelectedIndex = 0;
            }
        }

        private void BindProjects()
        {
            cmbProject.Items.Clear();
            if (!string.IsNullOrEmpty(Convert.ToString(cmbModuleList.Value)))
            {
                UGITModule currentUGITModule = ModuleManagerObj.GetByName(UGITUtility.ObjectToString(cmbModuleList.Value));
                DataTable dtProject = TicketManagerObj.GetOpenTickets(currentUGITModule);

                if (dtProject != null && dtProject.Rows.Count > 0)
                {
                    UGITModule uGITModule = currentUGITModule;

                    if (dtProject != null && dtProject.Columns.Contains(DatabaseObjects.Columns.IsPrivate) && !AppContext.UserManager.IsUGITSuperAdmin(AppContext.CurrentUser))
                    {
                        DataTable dt = null;
                        EnumerableRowCollection<DataRow> dc = dtProject.AsEnumerable().Where(x => x.Field<bool?>(DatabaseObjects.Columns.IsPrivate) != null && x.Field<bool?>(DatabaseObjects.Columns.IsPrivate) == true);
                        if (dc != null && dc.Count() > 0)
                            dt = dc.CopyToDataTable();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                string ticketId = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
                                string ticketModuleName = uGITModule.ModuleName;
                                if (string.IsNullOrEmpty(ticketModuleName))
                                    ticketModuleName = uHelper.getModuleNameByTicketId(ticketId);

                                if (string.IsNullOrEmpty(ticketModuleName))
                                    continue; // Should never happen!!

                                //if (uGITModule == null || ticketModuleName != uGITModule.ModuleName)
                                //    uGITModule = uGITCache.ModuleConfigCache.GetCachedModule(spWeb, ticketModuleName);

                                if (uGITModule != null)
                                {
                                    List<string> userTypeColumnColl = uGITModule.List_ModuleUserTypes.Select(x => x.FieldName).ToList();
                                    userTypeColumnColl.Add(DatabaseObjects.Columns.IsPrivate);
                                    currentTicket = TicketManagerObj.GetByTicketID(uGITModule, ticketId);  // Ticket.getCurrentTicket(ticketModuleName, ticketId, viewFields: userTypeColumnColl, viewFieldsOnly: true);
                                    if (currentTicket != null && !AppContext.UserManager.IsUserPresentInModuleUserTypes(AppContext.CurrentUser, currentTicket, uGITModule.ModuleName))
                                    {
                                        // Hide if ticket marked private (and user is not named user)
                                        if (uHelper.IfColumnExists(DatabaseObjects.Columns.IsPrivate, currentTicket.Table) && UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsPrivate]))
                                        {
                                            dtProject.AsEnumerable().FirstOrDefault(s => s.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId).Delete();
                                            dtProject.AcceptChanges(); // Need this otherwise get crash in global search
                                        }
                                    }
                                }
                                else
                                {
                                    // Should never get here!!!
                                    currentTicket = TicketManagerObj.GetByTicketID(uGITModule, ticketId);  // Ticket.getCurrentTicket(ticketModuleName, ticketId);
                                    if (currentTicket != null && !AppContext.UserManager.IsUserPresentInModuleUserTypes(AppContext.CurrentUser, currentTicket, ticketModuleName))
                                    {
                                        // Hide if ticket marked private (and user is not named user)
                                        if (uHelper.IfColumnExists(DatabaseObjects.Columns.IsPrivate, currentTicket.Table) && UGITUtility.StringToBoolean(currentTicket[DatabaseObjects.Columns.IsPrivate]))
                                        {
                                            dtProject.AsEnumerable().FirstOrDefault(s => s.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId).Delete();
                                            dtProject.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    dtProject.DefaultView.Sort = DatabaseObjects.Columns.Title;
                    cmbProject.DataSource = dtProject.DefaultView;
                    cmbProject.DataBind();

                    if (cmbProject.SelectedIndex < 0)
                        cmbProject.SelectedIndex = 0;
                }
            }
        }

        private void BindProjectDetails()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(cmbProject.Value)))
            {
                UGITModule UGITModule = ModuleManagerObj.GetByName(UGITUtility.ObjectToString(cmbModuleList.Value));
                DataRow ticketItem = TicketManagerObj.GetByTicketID(UGITModule, Convert.ToString(cmbProject.Value)); // Ticket.getCurrentTicket(uHelper.getModuleNameByTicketId(Convert.ToString(cmbProject.Value)), Convert.ToString(cmbProject.Value), null, false);
                aProjectTitle.InnerText = UGITUtility.TruncateWithEllipsis(Convert.ToString(ticketItem[DatabaseObjects.Columns.Title]), 50);
                lblProjectStatus.Text = Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketStatus]);

                //lblProjectManager.Text = uHelper.GetLookupValue(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketOwner]));
                //lblProjectDescription.Text = uHelper.TruncateWithEllipsis(Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketDescription]), 225);

                //DataRow moduleDetail = uGITCache.GetModuleDetails(uHelper.getModuleNameByTicketId(Convert.ToString(cmbProject.Value)));
                string viewUrl = string.Empty;
                string func = string.Empty;

                if (UGITModule != null)
                {
                    viewUrl = string.Empty;
                    if (!string.IsNullOrEmpty(UGITModule.StaticModulePagePath))
                    {
                        viewUrl = UGITUtility.GetAbsoluteURL(UGITModule.StaticModulePagePath);
                    }

                    if (!string.IsNullOrEmpty(viewUrl))
                    {
                        string width = "90";
                        string height = "90";

                        func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketItem[DatabaseObjects.Columns.TicketId]), aProjectTitle.InnerText, Server.UrlEncode(Request.Url.AbsolutePath), width, height);
                    }

                    aProjectTitle.Attributes.Add("onClick", func);
                }

                if (uHelper.IfColumnExists(DatabaseObjects.Columns.ScrumLifeCycle, ticketItem.Table) && UGITUtility.StringToBoolean(Convert.ToString(ticketItem[DatabaseObjects.Columns.ScrumLifeCycle])))
                    scrumTab.Visible = true;
                else
                    scrumTab.Visible = false;

                if (tasklistTab.Visible)
                {
                    Guid newFrameId = Guid.NewGuid();
                    //string url = UGITUtility.GetAbsoluteURL(string.Format("/_layouts/15/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=TasksList",
                    //                                                    ticketItem[DatabaseObjects.Columns.TicketId], newFrameId, uHelper.getModuleNameByTicketId(Convert.ToString(cmbProject.Value))));
                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&ticketID={0}&module={2}&frameObjId={1}&control=NewProjectTask",
                                                                                        Convert.ToString(ticketItem[DatabaseObjects.Columns.TicketId]), newFrameId, UGITModule.ModuleName));

                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe src='{0}' onload='FrameLoadOnDemand(this)' scrolling='no' frameurl='{0}' width='100%' height='100%' frameborder='0' id='{1}'></iframe>", url, newFrameId));

                    panelTaskList.Controls.Add(lCtr);
                }

                if (scrumTab.Visible)
                {
                    Guid newFrameId = Guid.NewGuid();
                    string url = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/ProjectManagement.aspx?isdlg=1&isudlg=1&Module={1}&PublicTicketID={0}&control=PMMSprints&frameObjId={2}&projectTaskView=1",
                                                                        ticketItem[DatabaseObjects.Columns.TicketId], uHelper.getModuleNameByTicketId(Convert.ToString(cmbProject.Value)), newFrameId));
                    LiteralControl lCtr = new LiteralControl(string.Format("<iframe src='{0}' onload='FrameLoadOnDemand(this)' scrolling='no' frameurl='{0}' width='100%' height='100%' frameborder='0' id='{1}'></iframe>", url, newFrameId));
                    panelSprint.Controls.Add(lCtr);
                }
            }
        }

        protected void cmbModuleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.DeleteCookie(Request, Response, "Taskmodule");
            UGITUtility.DeleteCookie(Request, Response, "TaskProjectId");
            UGITUtility.CreateCookie(Response, "Taskmodule", Convert.ToString(cmbModuleList.Value));
            BindProjects();
            BindProjectDetails();
        }

        protected void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.DeleteCookie(Request, Response, "TaskProjectId");
            UGITUtility.CreateCookie(Response, "TaskProjectId", Convert.ToString(cmbProject.Value));
            BindProjectDetails();
        }
    }
}
