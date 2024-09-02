using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ProjectLifeCycleStage : UserControl
    {
        public int Id { get; set; }
        public string ProjectPublicId { get; set; }
        public string ModuleName = "PMM";
        public string changedFields;
        string alreadySavedProjectLifeCycle = string.Empty;
        private DataRow _SPListItem;
        List<LifeCycle> lifeCycles;
        List<UGITTask> taskList;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        TaskTemplateManager taskTemplateHelper = null;
        UGITTaskManager taskHelper = null;
        ResourceAllocationManager resourceAllocation = null;
        Ticket ticket = null;
        UserProfile currentUser;

        protected override void OnInit(EventArgs e)
        {
            taskHelper = new UGITTaskManager(context);
            ticket = new Ticket(context, ModuleName);
            taskTemplateHelper = new TaskTemplateManager(context);
            resourceAllocation = new ResourceAllocationManager(context);
            currentUser = HttpContext.Current.CurrentUser();

            _SPListItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.ID} = {Id} and {DatabaseObjects.Columns.TenantID} = '{context.TenantID}'").Select()[0];

            long LookupId = Convert.ToInt64(_SPListItem[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
            LifeCycle selectedLifeCycle = lifeCycles.FirstOrDefault(x => x.ID == LookupId);
            if (selectedLifeCycle != null)
            {
                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/ControlTemplates/Shared/LifeCycleGUI.ascx");
                ctr2.ModuleLifeCycle = selectedLifeCycle;
                pLifeCycleStageGUI.Controls.Add(ctr2);
            }
            taskList = taskHelper.LoadByProjectID(ModuleName, Convert.ToString(Id));
            //Fill Project details
            Fill();
            if (taskList != null && taskList.Count > 0)//check in project plan that tasks are >0 then checkbox will be unchecked 
                chkResetProjectPlantoDefault.Checked = false;
            else
                chkResetProjectPlantoDefault.Checked = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void Fill()
        {
            string LookupId = Convert.ToString(_SPListItem[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
            alreadySavedProjectLifeCycle = LookupId;
            ddlLifeCycleModel.SelectedValue = LookupId;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<LifeCycle> lifecycleitem = lifeCycles.Where(l => l.ID == Convert.ToInt32(ddlLifeCycleModel.SelectedValue)).ToList();
            LifeCycle stagelifecycleitem = lifecycleitem.SingleOrDefault();

            if (ddlLifeCycleModel.SelectedValue == alreadySavedProjectLifeCycle && !chkResetProjectPlantoDefault.Checked)
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
                return;//return if user just saving as it is without any modification
            }

            //Delete all task against current project
            if (chkResetProjectPlantoDefault.Checked)
            {
                if (taskList != null && taskList.Count > 0)
                {
                    taskHelper.DeleteTasks(ModuleName, taskList);
                    Util.Log.ULog.WriteLog($"User {currentUser.Name} DELETED ALL tasks for project {ProjectPublicId}");

                    taskList = taskHelper.LoadByProjectID(ModuleName, Convert.ToString(Id));
                    taskHelper.CalculateProjectStartEndDate(ModuleName, taskList, _SPListItem);

                    ticket.CommitChanges(_SPListItem);
                    //TaskCache.ReloadProjectTasks(ModuleName, ProjectPublicId); // cache is not implemented so not using right now
                }
                //Create default tasks again
                if (taskList != null && taskList.Count == 0)
                {
                    LifeCycleManager objLifeCycleHelper = new LifeCycleManager(context);
                    LifeCycle createlifeCycle = objLifeCycleHelper.LoadProjectLifeCycleByName(ddlLifeCycleModel.SelectedItem.Text);

                    if (createlifeCycle != null)
                    {
                        taskList = taskTemplateHelper.GenerateDefaultTasks(createlifeCycle, (_SPListItem[DatabaseObjects.Columns.TicketActualStartDate] == null || _SPListItem[DatabaseObjects.Columns.TicketActualStartDate] == DBNull.Value) ? DateTime.Now : Convert.ToDateTime(_SPListItem[DatabaseObjects.Columns.TicketActualStartDate]));
                    }
                    foreach (UGITTask tk in taskList)
                    {
                        tk.ModuleNameLookup = "PMM";
                        tk.ProjectLookup = Convert.ToInt64(_SPListItem[DatabaseObjects.Columns.ID]); //new SPFieldLookupValue(_SPListItem.ID, Convert.ToString(_SPListItem[DatabaseObjects.Columns.TicketId]));
                        tk.Predecessors = null;
                        tk.ParentTaskID = 0;
                    }
                }

                if (taskList != null && taskList.Count > 0)
                {
                    //Imports tasks against project
                    taskList = taskHelper.ImportTasks("PMM", taskList, false, Convert.ToString(_SPListItem[DatabaseObjects.Columns.TicketId]));

                    //set fields like start and enddate and other schedule related field in project
                    taskHelper.CalculateProjectStartEndDate("PMM", taskList, _SPListItem);
                    ticket.CommitChanges(_SPListItem);

                    //Update task cache for imported project.
                    //TaskCache.ReloadProjectTasks("PMM", Convert.ToString(uHelper.GetSPItemValue(_SPListItem, DatabaseObjects.Columns.TicketId))); // cache is not implemented so not using right now

                    //User Allocation in RMM in TaskImport...                    
                    resourceAllocation.UpdateProjectPlannedAllocationByUser(taskList, "PMM", Convert.ToString(_SPListItem[DatabaseObjects.Columns.TicketId]), true);
                }
            }

            if (ddlLifeCycleModel.SelectedValue != alreadySavedProjectLifeCycle)
            {
                LifeCycleStage ticketCurrentStage = ticket.GetTicketCurrentStage(_SPListItem);
                LifeCycle ticketCurrentLifeCycle = ticket.GetTicketLifeCycle(_SPListItem);
                //Update Cache
                long LookupValue = Convert.ToInt64(_SPListItem[DatabaseObjects.Columns.ProjectLifeCycleLookup]);
                string message = string.Format("Changed project lifecycle from [{0}] to [{1}]", LookupValue, ddlLifeCycleModel.SelectedItem.Text);
                changedFields += message;
                uHelper.CreateHistory(currentUser, changedFields, _SPListItem,context);


                LifeCycleStage stage = stagelifecycleitem.Stages.FirstOrDefault(x => x.StageStep == ticketCurrentStage.StageStep);
                if (stage == null)
                {
                    //This happens when user changing large stages cycle into small stages cycle
                    if (ticketCurrentLifeCycle.Stages.LastOrDefault().StageStep == ticketCurrentStage.StageStep)
                    {
                        //if current stage is last stage then set new stage as last stage
                        stage = stagelifecycleitem.Stages.LastOrDefault();
                    }
                    else
                    {
                        //otherwise set stage one step before to close stage.
                        stage = ticketCurrentLifeCycle.Stages.FirstOrDefault(x => x.StageStep == stagelifecycleitem.Stages.LastOrDefault().StageStep - 1);
                    }
                }

                _SPListItem[DatabaseObjects.Columns.TicketStatus] = stage.Name;
                _SPListItem[DatabaseObjects.Columns.StageStep] = stage.StageStep;
                _SPListItem[DatabaseObjects.Columns.ProjectLifeCycleLookup] = ddlLifeCycleModel.SelectedValue;

                ticket.CommitChanges(_SPListItem);

                Util.Log.ULog.WriteLog($"User {currentUser.Name} {message} for project {ProjectPublicId}");
            }

            List<UGITTask> filteredTasks = taskList.Where(a => a.StageStep > stagelifecycleitem.Stages.Count).ToList();
            foreach (UGITTask task in filteredTasks)
            {
                task.StageStep = 0;
                task.IsMileStone = false;
                task.Changes = true;
            }

            taskHelper.SaveTasks(ref taskList);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void ddlLifeCycleModel_Init(object sender, EventArgs e)
        {
            Ticket TicketRequest = new Ticket(context, ModuleName);
            lifeCycles = TicketRequest.Module.List_LifeCycles;
            foreach (LifeCycle cycle in lifeCycles)
            {
                ddlLifeCycleModel.Items.Add(new ListItem(cycle.Name, cycle.ID.ToString()));
            }
        }

        protected void ddlLifeCycleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LifeCycle selectedLifeCycle = lifeCycles.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(ddlLifeCycleModel.SelectedValue));
            if (selectedLifeCycle != null)
            {
                LifeCycleGUI ctr2 = (LifeCycleGUI)Page.LoadControl("~/ControlTemplates/Shared/LifeCycleGUI.ascx");
                ctr2.ModuleLifeCycle = selectedLifeCycle;
                pLifeCycleStageGUI.Controls.Clear();
                pLifeCycleStageGUI.Controls.Add(ctr2);
            }
        }
    }
}