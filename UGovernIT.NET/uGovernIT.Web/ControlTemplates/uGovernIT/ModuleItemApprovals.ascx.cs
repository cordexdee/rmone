using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web.UI.HtmlControls;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ModuleItemApprovals : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        //public string ModuleItemPublicID { get; set; }
        //public int CurrentWorkFlowStep { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public int ModuleID { get; set; }
        public string ModuleItemPublicID { get; set; }
        public int CurrentWorkFlowStep { get; set; }
        protected override void OnPreRender(EventArgs e)
        {
            DataTable approvals = GetItemApprovals();
            rApprovals.DataSource = approvals;
            rApprovals.DataBind();
            base.OnPreRender(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    DataTable approvals = GetItemApprovals();
        //    rApprovals.DataSource = approvals;
        //    rApprovals.DataBind();
        //    base.OnPreRender(e);
        //}


        //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
        protected DataTable GetItemApprovals()
        {
            UserProfileManager userProfileManager = new UserProfileManager(context);
            string assignedUserName = "";
            DataTable approvalsTableWithDetail = new DataTable();
            approvalsTableWithDetail.Columns.Add("Action");
            approvalsTableWithDetail.Columns.Add("Description");
            approvalsTableWithDetail.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(int));
            approvalsTableWithDetail.Columns.Add("DisplayStageStep", typeof(int));
            approvalsTableWithDetail.Columns.Add(DatabaseObjects.Columns.StartDate, typeof(DateTime));
            approvalsTableWithDetail.Columns.Add(DatabaseObjects.Columns.EndDate, typeof(DateTime));
            approvalsTableWithDetail.Columns.Add(DatabaseObjects.Columns.TicketUser);
            DataColumn columnLtst = new DataColumn("Latest", typeof(bool));
            columnLtst.DefaultValue = false;
            approvalsTableWithDetail.Columns.Add(columnLtst);
            DataColumn columnParent = new DataColumn("isParent", typeof(bool));
            columnParent.DefaultValue = false;
            approvalsTableWithDetail.Columns.Add(columnParent);
            approvalsTableWithDetail.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            approvalsTableWithDetail.Columns.Add("EndDateTime", typeof(String));

            if (string.IsNullOrEmpty(ModuleName) || ModuleItemPublicID == null || ModuleItemPublicID.Trim() == string.Empty)
                return approvalsTableWithDetail;

            Ticket ticket = new Ticket(context, ModuleName);
            DataRow currentTicket = Ticket.GetCurrentTicket(context, ticket.Module.ModuleName, ModuleItemPublicID);
            LifeCycle ticketLifeCycle = ticket.GetTicketLifeCycle(currentTicket);

            //List<LifeCycleStage> lifeCycleStages = ticket.GetActiveLifeCycleStages(currentTicket);

            if (ticketLifeCycle == null)
                return approvalsTableWithDetail;

            ModuleWorkflowHistoryManager workflowManager = new ModuleWorkflowHistoryManager(context);
            List<ModuleWorkflowHistory> moduleWorkflowHistoryItems = workflowManager.Load(x => x.ModuleNameLookup == ModuleName && x.TicketId == ModuleItemPublicID).OrderBy(x => x.Created).ToList();

            DataTable moduleWorkflowItemsTable = UGITUtility.ToDataTable<ModuleWorkflowHistory>(moduleWorkflowHistoryItems);


            if (moduleWorkflowItemsTable == null || moduleWorkflowItemsTable.Rows.Count == 0)
                return approvalsTableWithDetail;

            // Get TicketEvents entries
            DataTable TicketEventsData = new DataTable();
            TicketEventManager TEM = new TicketEventManager(context);
            List<TicketEvents> lstTicketEventsItems = new List<TicketEvents>();
            if (ticket.Module.ModuleName == "SVC")
            {
                lstTicketEventsItems = TEM.Load(x => x.Ticketid.Trim() == ModuleItemPublicID.Trim() && x.TenantID== context.TenantID).ToList();

            }
            else
            {
                lstTicketEventsItems = TEM.Load(x => x.Ticketid == ModuleItemPublicID && x.TenantID == context.TenantID && (x.TicketEventType.ToLower() == "created" || x.TicketEventType.ToLower() == "assigned" || x.TicketEventType.ToLower() == "hold")).ToList();
            }
            DataTable ticketEventData = UGITUtility.ToDataTable(lstTicketEventsItems);


            // Process ModuleWorkflowHistory entries
            foreach (DataRow historyEntry in moduleWorkflowItemsTable.Rows)
            {
                try
                {
                    int stageStep = UGITUtility.StringToInt(historyEntry[DatabaseObjects.Columns.StageStep]);
                    if (stageStep <= 0 || stageStep >= CurrentWorkFlowStep)
                        continue;

                    DateTime stageEndDate = UGITUtility.StringToDateTime(historyEntry[DatabaseObjects.Columns.StageEndDate]);
                    if (stageEndDate == DateTime.MinValue)
                        continue;

                    // IF we have SVC ticket events to show then start from there instead of from workflow history for 1st entry
                    if (stageStep == 1 && ticketEventData != null && ticketEventData.Rows.Count > 0 && ticketEventData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketEventType, Constants.TicketEventType.Created)).Length > 0)
                        continue;

                    LifeCycleStage stage = ticketLifeCycle.Stages.FirstOrDefault(x => x.StageStep == stageStep);
                    if (stage == null)
                        continue; // Skipped stage

                    int displayStep = ticketLifeCycle.Stages.IndexOf(stage) + 1;
                    DataRow nRow = approvalsTableWithDetail.NewRow();
                    nRow["Action"] = stage.Action;
                    nRow["isParent"] = true;
                    nRow[DatabaseObjects.Columns.TicketUser] = string.Empty;
                    nRow[DatabaseObjects.Columns.EndDate] = stageEndDate;
                    nRow["EndDateTime"] = stageEndDate.ToString("MMM-d-yyyy hh:mm:ss tt");
                    string user = Convert.ToString(historyEntry[DatabaseObjects.Columns.StageClosedByName]);
                    if (!string.IsNullOrEmpty(user))
                    {
                        nRow[DatabaseObjects.Columns.TicketUser] = user;
                        nRow["Description"] = string.Format("{0} at {1}", user, stageEndDate.ToString("MMM-d-yyyy hh:mm:ss tt"));
                    }
                    else
                        nRow["Description"] = string.Format("at {0}", stageEndDate.ToString("MMM-d-yyyy hh:mm:ss tt"));

                    nRow[DatabaseObjects.Columns.StageStep] = stage.StageStep;
                    nRow["DisplayStageStep"] = displayStep;
                    nRow[DatabaseObjects.Columns.Title] = ModuleItemPublicID;
                    approvalsTableWithDetail.Rows.Add(nRow);
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }

            }

            // Mark latest ocurrence of each stage
            if (approvalsTableWithDetail != null && approvalsTableWithDetail.Rows.Count > 0)
            {
                var lookups = approvalsTableWithDetail.AsEnumerable().ToLookup(x => x.Field<int>(DatabaseObjects.Columns.StageStep));
                foreach (var item in lookups)
                {
                    item.Last()["Latest"] = true;
                }
            }


            // Process TicketEvents entries
            if (ticketEventData != null && ticketEventData.Rows.Count > 0)
            {
                //remove first entry of assigned, we don't need to show it here
                DataRow[] assignedEvents = ticketEventData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketEventType, "Assigned")).OrderBy(x => x.Field<DateTime>(DatabaseObjects.Columns.TicketEventTime)).ToArray();
                if (assignedEvents.Length > 0)
                    ticketEventData.Rows.Remove(assignedEvents[0]);

                foreach (DataRow eventRow in ticketEventData.Rows)
                {
                    DataRow nRow = approvalsTableWithDetail.NewRow();
                    string eventType = Convert.ToString(eventRow[DatabaseObjects.Columns.TicketEventType]);
                    if (eventType == "Assigned" && !string.IsNullOrWhiteSpace(Convert.ToString(eventRow[DatabaseObjects.Columns.AffectedUsers])))
                    {
                        {
                                assignedUserName = userProfileManager.GetUserNameById(Convert.ToString(eventRow[DatabaseObjects.Columns.AffectedUsers]));
                        }
                        nRow["Action"] = string.Format("Assigned: {0}", assignedUserName);
                    }
                    else
                        nRow["Action"] = eventType;

                    nRow[DatabaseObjects.Columns.TicketUser] = eventRow[DatabaseObjects.Columns.TicketEventBy];
                    nRow[DatabaseObjects.Columns.EndDate] = eventRow[DatabaseObjects.Columns.TicketEventTime];
                    nRow["EndDateTime"] = UGITUtility.StringToDateTime(eventRow[DatabaseObjects.Columns.TicketEventTime]).ToString("MMM-d-yyyy hh:mm:ss tt");
                    nRow[DatabaseObjects.Columns.StageStep] = eventRow[DatabaseObjects.Columns.StageStep];

                    nRow[DatabaseObjects.Columns.Title] = ModuleItemPublicID;

                    if (eventType == Constants.TicketEventType.Created)
                    {
                        nRow["DisplayStageStep"] = 1;
                        nRow["isParent"] = true;
                    }
                    else
                        nRow["isParent"] = false;

                    nRow["Latest"] = true;
                    DateTime eventDate = UGITUtility.StringToDateTime(eventRow[DatabaseObjects.Columns.TicketEventTime]);

                    string desc = Convert.ToString(eventRow[DatabaseObjects.Columns.EventReason]);
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(eventRow[DatabaseObjects.Columns.SubTaskTitle])))
                    {
                        desc = Convert.ToString(eventRow[DatabaseObjects.Columns.SubTaskTitle]);
                        nRow[DatabaseObjects.Columns.Title] = desc;
                    }

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(eventRow[DatabaseObjects.Columns.SubTaskId])) && Convert.ToString(eventRow[DatabaseObjects.Columns.SubTaskId]).Contains("-"))
                    {
                        desc = string.Format("{0}: {1}", eventRow[DatabaseObjects.Columns.SubTaskId], desc); // Prefix sub-ticket titles with TicketId
                        nRow[DatabaseObjects.Columns.Title] = string.Format("{0}: {1}", eventRow[DatabaseObjects.Columns.SubTaskId], Convert.ToString(eventRow[DatabaseObjects.Columns.SubTaskTitle]));
                    }


                    approvalsTableWithDetail.Rows.Add(nRow);
                    
                }
            }

            if (approvalsTableWithDetail != null)
            {
                approvalsTableWithDetail.DefaultView.Sort = string.Format("{0}  asc", DatabaseObjects.Columns.EndDate);
                approvalsTableWithDetail = approvalsTableWithDetail.DefaultView.ToTable();
            }
            //int i = 1;
            //if (ModuleItemPublicID.StartsWith("SVC"))
            //{
            //    //do nothing
            //}
            //else
            //{
            //    foreach (DataRow eventRow in approvalsTableWithDetail.Rows)
            //    {
            //        eventRow["DisplayStageStep"] = i;
            //        i++;
            //    }
            //}
            return approvalsTableWithDetail;
        }


        protected void rApprovals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (uHelper.getModuleNameByTicketId(ModuleItemPublicID) != "SVC")
                {
                    HtmlTableCell trTitle = (HtmlTableCell)e.Item.FindControl("trTitle");
                    trTitle.Visible = false;
                }
            }
        }
        //
    }
}