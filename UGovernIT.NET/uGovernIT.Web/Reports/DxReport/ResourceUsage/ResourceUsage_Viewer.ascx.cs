using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using DevExpress.XtraReports.Web;
using uGovernIT.Utility;
using report = uGovernIT.DxReport;


namespace uGovernIT.DxReport.ResourceUsage
{
    public partial class ResourceUsage_Viewer : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        UGITTaskManager objUGITTaskManager;
        UserProfileManager userManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            objUGITTaskManager = new UGITTaskManager(context);
            userManager = new UserProfileManager(context);
            DataTable dt = GetAllocationData();
            report.ResourceUsage_Report resourceUsageTaskList = new report.ResourceUsage_Report(dt);
            resourceUsageTaskList.Name = "Resource Usage Report";
            RptResourceUsageReport.Report = resourceUsageTaskList;
        }

        private DataTable GetAllocationData()
        {
            DataTable data = new DataTable();
            data.Columns.Add(DatabaseObjects.Columns.Id, typeof(string));
            data.Columns.Add(DatabaseObjects.Columns.ItemOrder, typeof(int));
            data.Columns.Add(DatabaseObjects.Columns.Resource, typeof(string));
            data.Columns.Add("TaskAssigned", typeof(string));
            data.Columns.Add("TaskCompleted", typeof(string));
            data.Columns.Add("OnTime", typeof(string));
            data.Columns.Add("CompletedLate", typeof(string));
            data.Columns.Add("Pending", typeof(string));
            data.Columns.Add("Overdue", typeof(string));

            List<UserProfile> lstUProfile = userManager.GetUsersProfile();

            DataTable dtTask = objUGITTaskManager.GetAllTasksByProjectID(uHelper.getModuleNameByTicketId(Request["TicketPublicId"]), Request["TicketPublicId"]);

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