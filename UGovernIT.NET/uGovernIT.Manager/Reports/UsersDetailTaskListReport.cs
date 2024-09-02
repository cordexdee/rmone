using System.Data;

namespace uGovernIT.Manager.Reports
{
    public partial class UsersDetailTaskListReport : DevExpress.XtraReports.UI.XtraReport
    {
        public UsersDetailTaskListReport(DataTable datatable)
        {
            InitializeComponent();
            BindAllTasks(datatable);
        }

        private void BindAllTasks(DataTable datatable)
        {
            //foreach (DataRow row in datatable.Rows)
            //{
            //    row["PercentComplete"] = Convert.ToDouble(row["PercentComplete"]) / 100;
            //   // row[DatabaseObjects.Columns.TaskBehaviour] = GetBehaviourIcon(Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]));
            //}

            xrlblNumber.DataBindings.Add("Text", null, "ItemOrder");
            xrTableCellResource.DataBindings.Add("Text", null, "Resource");
            xrTCTaskAssigne.DataBindings.Add("Text", null, "TaskAssigned");
            xrTCTaskCompleted.DataBindings.Add("Text", null, "TaskCompleted");
            xrTCOnTime.DataBindings.Add("Text", null, "OnTime");
            xrTCCompletedLate.DataBindings.Add("Text", null, "CompletedLate");

            xrTCOverdue.DataBindings.Add("Text", null, "Pending");
            xrTCPending.DataBindings.Add("Text", null, "Overdue");

            //datatable.DefaultView.Sort = "ItemOrder ASC";

            Report.DataSource = datatable.DefaultView.ToTable();
        }

        

        
    }
}
