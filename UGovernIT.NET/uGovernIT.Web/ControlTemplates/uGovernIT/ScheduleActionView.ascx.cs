using System;
using System.Collections.Generic;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Data;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class ScheduleActionView :UserControl
    {
        public int requestId { get; set; }       
        public bool IsArchive { get; set; }
        public bool IsModuleWebpart { get; set; }
        DataRow dr=null;
        string tableName = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            ScheduleActionsManager scheduleActionsManager = new ScheduleActionsManager(context);
            List<SchedulerAction> scheduleActions = scheduleActionsManager.Load();
            //UGITUtility.ToDataTable<ScheduleAction>(scheduleActions);

            ScheduleActionsArchiveManager scheduleActionsArchiveManager = new ScheduleActionsArchiveManager(context);
            List<SchedulerActionArchive> scheduleActionsArchive = scheduleActionsArchiveManager.Load();
            DataTable dtScheduleActionsArchive = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SchedulerActionArchives);

            if (Request["IsModuleWebpart"] != null)
            {
                IsModuleWebpart = UGITUtility.StringToBoolean(Request["IsModuleWebpart"]);// lnkDelete.Visible = false;
            }
            if (Request["IsArchive"] != null)
            {
                IsArchive = UGITUtility.StringToBoolean(Request["IsArchive"]);
            }
            if (requestId > 0)
            {
                if (IsModuleWebpart)
                {
                    lnkDelete.Visible = true;
                }
                if (IsArchive)
                {
                    tableName = DatabaseObjects.Tables.SchedulerActionArchives;
                    dr = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SchedulerActionArchives, " Id=" + requestId).Select().First();      // spListitem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.ScheduleActionsArchive, requestId);
                }
                else
                {
                    tableName = DatabaseObjects.Tables.SchedulerActions;
                    dr = GetTableDataManager.GetTableData(DatabaseObjects.Tables.SchedulerActions, " Id=" + requestId).Select().First();  //spListitem = SPListHelper.GetSPListItem(DatabaseObjects.Tables.ScheduleActions, requestId);
                }
                FillForm();
            }
            base.OnInit(e);
        }

        private void FillForm()
        {
            lblScheduleTitle.Text = Convert.ToString(dr[DatabaseObjects.Columns.Title]);
            lblStartTime.Text = String.Format("{0:MMM-dd-yyyy HH:mm tt}", Convert.ToDateTime(dr[DatabaseObjects.Columns.StartTime]));
            lblScheduleEmailTo.Text = Convert.ToString(dr[DatabaseObjects.Columns.EmailIDTo]);
            lblScheduleEmailCC.Text = Convert.ToString(dr[DatabaseObjects.Columns.EmailIDCC]);
            lblScheduleSubject.Text = Convert.ToString(dr[DatabaseObjects.Columns.MailSubject]);
            txtScheduleEmailBody.Text = Convert.ToString(dr[DatabaseObjects.Columns.EmailBody]);
            lblScheduleActionType.Text = Convert.ToString(dr[DatabaseObjects.Columns.ActionType]);
            lblTicketId.Text = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);
            recurrTable.Visible = chkScheduleRecurring.Checked = UGITUtility.StringToBoolean(dr[DatabaseObjects.Columns.Recurring]);
            if (chkScheduleRecurring.Checked)
            {
                lblScheduleRecurrInterval.Text = Convert.ToString(dr[DatabaseObjects.Columns.RecurringInterval]);
                double escalationMinutes = Convert.ToDouble(dr[DatabaseObjects.Columns.RecurringInterval]);
                int workingHoursInADay = uHelper.GetWorkingHoursInADay(context);
                //it would be 24hrs in case of reminder
                if (lblScheduleActionType.Text == ScheduleActionType.Reminder.ToString())
                    workingHoursInADay = 24;
                if (escalationMinutes % (workingHoursInADay * 60) == 0)
                {
                    lblScheduleRecurrInterval.Text = string.Format("{0} {1}", string.Format("{0:0.##}", escalationMinutes / (workingHoursInADay * 60)), Constants.SLAConstants.Days);
                }
                else if (escalationMinutes % 60 == 0)
                {
                    lblScheduleRecurrInterval.Text = string.Format("{0} {1}", string.Format("{0:0.##}", escalationMinutes / 60), Constants.SLAConstants.Hours);
                }
                else
                {
                    lblScheduleRecurrInterval.Text = string.Format("{0} {1}", string.Format("{0:0.##}", escalationMinutes), Constants.SLAConstants.Minutes);
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dr[DatabaseObjects.Columns.RecurringEndDate])))
                {
                    lblRecurrEndDate.Text = String.Format("{0:MMM-dd-yyyy HH:mm tt}", Convert.ToDateTime(dr[DatabaseObjects.Columns.RecurringEndDate]));
                }
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (dr != null)
            {
                GetTableDataManager.delete<int>(tableName, DatabaseObjects.Columns.ID,Convert.ToString(dr["ID"]));
                dr.Delete();
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}