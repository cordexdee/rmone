using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class PhaseChanges : UserControl
    {
        public int PMMID { get; set; }
        protected DataRow pmmItem = null;
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }
        protected string currentModulePagePath;

        Ticket ticketRequest = null;
        LifeCycle projectLifeCycle = null;
        LifeCycleStage currentLifeCycleStage = null;

        UserProfileManager UserManager;
        ModuleViewManager ModuleManager;
        UserProfile User;
        UGITModule Module;

        protected string ModuleName = "PMM";

        protected long closeStageId = 0;
        ApplicationContext context;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            UserManager = new UserProfileManager(context);
            ModuleManager = new ModuleViewManager(context);
            User = context.CurrentUser;

            TicketManager ticketManager = new TicketManager(context);
            ticketRequest = new Ticket(context, "PMM");

            Module = ModuleManager.LoadByName(ModuleName);

            pmmItem = ticketManager.GetByID(Module, PMMID); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMProjects, PMMID);

            projectLifeCycle = ticketRequest.GetTicketLifeCycle(pmmItem);

            LifeCycleStage closeStage = ticketRequest.GetTicketCloseStage(pmmItem);

            if (closeStage != null)

                closeStageId = closeStage.ID;

            currentLifeCycleStage = ticketRequest.GetTicketCurrentStage(pmmItem, projectLifeCycle);

            /// code moved from pre render event
            if (IsReadOnly)
            {
                //gridRisks.Enabled = false;
                //LinkButton2.Visible = false;
            }
            //else if (IsShowBaseline)
            //{
            //   // ddlProjectStatus.Visible = false;
            //}
            else
            {
                string historyUrlPath = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/projectmanagement.aspx?control=projectAllStatusSummary");

                if (projectLifeCycle != null && projectLifeCycle.Stages.Count > 0)
                {
                    //if at least one task attached to milestone then don't show manual stage move
                    UGITTaskManager taskManager = new UGITTaskManager(context);
                    List<UGITTask> projectTasks = taskManager.LoadByTemplateID(PMMID); //UGITTaskHelper.LoadByProjectID(SPContext.Current.Web, "PMM", PMMID);
                    if (!projectTasks.Exists(x => x.IsMileStone))
                    {
                        phasePanel.Visible = true;
                        //Fill project stage dropdown
                        if (ddlProjectStatus.Items.Count <= 0)
                        {
                            foreach (LifeCycleStage stage in projectLifeCycle.Stages)
                            {

                                ListItem item = new ListItem(Convert.ToString(stage.StageStep) + " - " + stage.Name, Convert.ToString(stage.ID));
                                if (stage.StageStep == currentLifeCycleStage.StageStep)
                                {
                                    item.Selected = true;
                                    ddlProjectStatus.Enabled = false;
                                    btChangeProjectStage.Enabled = false;
                                }

                                if (IsShowBaseline && item.Selected)
                                {
                                    liProjectstatus.Visible = IsShowBaseline;
                                    liProjectstatus.Text = Convert.ToString(item);
                                    ddlProjectStatus.Visible = !IsShowBaseline;
                                    btChangeProjectStage.Visible = !IsShowBaseline;

                                }
                                ddlProjectStatus.Items.Add(item);
                            }
                        }
                    }
                }

            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            
        }

        protected void BtChangeProjectStage_Click(object sender, EventArgs e)
        {
            bool isStatusChanged = false;
            LifeCycleStage newStage = null;
            if (currentLifeCycleStage.ID.ToString() != ddlProjectStatus.SelectedValue)
            {
                if (projectLifeCycle.ID == 0)
                {
                    pmmItem[DatabaseObjects.Columns.ModuleStepLookup] = ddlProjectStatus.SelectedValue;
                }

                newStage = projectLifeCycle.Stages.FirstOrDefault(x => x.ID.ToString() == ddlProjectStatus.SelectedValue);
                pmmItem[DatabaseObjects.Columns.StageStep] = newStage.StageStep;
                pmmItem[DatabaseObjects.Columns.TicketStatus] = newStage.Name;
                pmmItem[DatabaseObjects.Columns.CurrentStageStartDate] = DateTime.Now;

                // If moving to last stage, set Close Date in ticket
                LifeCycleStage closeStage = ticketRequest.GetTicketCloseStage(projectLifeCycle);
                if (closeStage.StageStep == newStage.StageStep)
                {
                    if (hdnConfirmCloseTasksAction != null && !string.IsNullOrWhiteSpace(Convert.ToString(hdnConfirmCloseTasksAction.Value)))
                    {
                        bool closePMMTasks = UGITUtility.StringToBoolean(Convert.ToString(hdnConfirmCloseTasksAction.Value));
                        ticketRequest.ClosePMMTicket(pmmItem, string.Empty, closePMMTasks);
                    }
                    else
                        ticketRequest.ClosePMMTicket(pmmItem, string.Empty);
                }
                else
                {
                    pmmItem[DatabaseObjects.Columns.TicketClosed] = 0;
                    pmmItem[DatabaseObjects.Columns.TicketCloseDate] = DBNull.Value;
                    uHelper.CreateHistory(User, string.Format("Project Stage changed from {0} to {1}", currentLifeCycleStage.Name, newStage.Name), pmmItem, false,context);
                }

                //HiddenField refreshPage = this.Page.FindControlRecursive("refreshPage") as HiddenField;
                //refreshPage.Value = "true";
                refreshPhase.Value = "true";
                
                isStatusChanged = true;
            }

            int actualPhaseCompletion = 0;
            if (UGITUtility.IsSPItemExist(pmmItem, DatabaseObjects.Columns.ProjectPhasePctComplete))
            {
                int.TryParse(Convert.ToString(pmmItem[DatabaseObjects.Columns.ProjectPhasePctComplete]), out actualPhaseCompletion);
            }

            ticketRequest.CommitChanges(pmmItem);

            if (isStatusChanged)
            {
                //TaskCache.ReloadProjectTasks(moduleName, pmmPublicId);
                LifeCycleStage projectStage = ticketRequest.GetTicketCurrentStage(pmmItem);

                //// Create baseline: Note CreateBaseline generates new version using pmmItem.Update()
                //PMMBaseline baseline = new PMMBaseline(pmmItem.ID, DateTime.Now);
                //baseline.BaselineComment = projectStage.Name;
                //baseline.CreateBaseline(pmmItem);
            }
            string cookieValue = UGITUtility.GetCookieValue(Request, ModuleName + "-TicketSelectedTabConst");
            if (!string.IsNullOrEmpty(cookieValue))
            {
                UGITUtility.CreateCookie(Response, ModuleName + "-TicketSelectedTab", cookieValue);
                UGITUtility.DeleteCookie(Request, Response, ModuleName + "-TicketSelectedTabConst");
            }

            confirmCloseTasks.ShowOnPageLoad = false;
        }
    }
}