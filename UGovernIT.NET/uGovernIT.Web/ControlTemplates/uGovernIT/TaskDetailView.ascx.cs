using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using report = uGovernIT.Manager.Reports;

namespace uGovernIT.Web
{
    public partial class TaskDetailView : UserControl
    {
        private UserProfileManager _userProfileManager = null;

        UGITTaskManager TaskManager;

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {

                    _userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());

                }
                return _userProfileManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = GetAllocationData();
            report.UsersDetailTaskListReport userDetailTaskList = new report.UsersDetailTaskListReport(dt);
            userDetailTaskList.Name = "Resource Usage Report";
            RptUserDetailTaskListReport.Report = userDetailTaskList;

        }

        private DataTable GetAllocationData()
        {
            var data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add("TaskAssigned", typeof(string));
            data.Columns.Add("TaskCompleted", typeof(string));
            data.Columns.Add("OnTime", typeof(string));
            data.Columns.Add("CompletedLate", typeof(string));
            data.Columns.Add("Pending", typeof(string));
            data.Columns.Add("Overdue", typeof(string));

            var lstUProfile = UserProfileManager.GetUsersProfile(); //uGITCache.UserProfileCache.GetAllUsers(SPContext.Current.Web);
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());

            var dtTask = TaskManager.GetAllTasksByProjectID(uHelper.getModuleNameByTicketId(Request["TicketPublicId"]), Convert.ToString(Request["TicketPublicId"]));

            int count = 0;
            foreach (UserProfile userProfile in lstUProfile)
            {
                int IntTaskAssigned = 0;
                int IntTaskCompleted = 0;
                int IntOnTime = 0;
                int IntCompletedLate = 0;
                int IntPending = 0;
                int IntOverdue = 0;

                //DataRow newRow  = data.NewRow();

                if (dtTask != null && dtTask.Rows.Count > 0)
                {
                    foreach (DataRow ritem in dtTask.Rows)
                    {
                        string[] userIds = Convert.ToString(ritem[DatabaseObjects.Columns.AssignedTo]).Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                        int pos = Array.IndexOf(userIds, userProfile.Id.ToString());
                        if (pos > -1)
                        {
                            IntTaskAssigned += 1;

                            if (Convert.ToString(ritem[DatabaseObjects.Columns.Status]) == "Completed")
                                IntTaskCompleted += 1;

                            if (UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.CompletionDate])) <= UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.DueDate])) && Convert.ToString(ritem[DatabaseObjects.Columns.Status]) == "Completed")
                                IntOnTime += 1;

                            if (UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.CompletionDate])) >= UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.DueDate])) && Convert.ToString(ritem[DatabaseObjects.Columns.Status]) == "Completed")
                                IntCompletedLate += 1;


                            if (UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.DueDate])) >= DateTime.Now && Convert.ToString(ritem[DatabaseObjects.Columns.Status]) != "Completed")
                                IntPending += 1;

                            if (UGITUtility.StringToDateTime(Convert.ToString(ritem[DatabaseObjects.Columns.DueDate])) <= DateTime.Now && Convert.ToString(ritem[DatabaseObjects.Columns.Status]) != "Completed")
                                IntOverdue += 1;

                        }
                    }

                }


                if (IntTaskAssigned != 0 || IntTaskCompleted != 0 || IntOnTime != 0 || IntCompletedLate != 0 || IntPending != 0 || IntOverdue != 0)
                {
                    count++;
                    DataRow newRow = data.NewRow();
                    newRow[DatabaseObjects.Columns.Id] = userProfile.Id;
                    newRow[DatabaseObjects.Columns.Resource] = userProfile.Name;
                    newRow["TaskAssigned"] = IntTaskAssigned;
                    newRow["TaskCompleted"] = IntTaskCompleted;
                    newRow["OnTime"] = IntOnTime;
                    newRow["CompletedLate"] = IntCompletedLate;
                    newRow["Pending"] = IntPending;
                    newRow["Overdue"] = IntOverdue;
                    newRow["ItemOrder"] = count;

                    data.Rows.Add(newRow);
                }
            }

            return data;

        }

    }
}