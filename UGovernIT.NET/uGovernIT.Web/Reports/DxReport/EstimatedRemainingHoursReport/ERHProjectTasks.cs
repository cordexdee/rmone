using System;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;
namespace uGovernIT.DxReport
{
    public partial class ERHProjectTasks : DevExpress.XtraReports.UI.XtraReport
    {
        public ERHProjectTasks(DataTable datatable)
        {
            InitializeComponent();
            BindAllTasks(datatable);
        }

        private void BindAllTasks(DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                row["PercentComplete"] = Convert.ToDouble(row["PercentComplete"]) / 100;
               // row[DatabaseObjects.Columns.TaskBehaviour] = GetBehaviourIcon(Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]));
            }
            xrTCTasksNum.DataBindings.Add("Text", null, "ItemOrder");
            xrLblTaskTitle.DataBindings.Add("Text", null, "Title");
            //xrTCTasksStatus.DataBindings.Add("Text", null, "Status");
            xrTCTasksPrecentComplete.DataBindings.Add("Text", null, "PercentComplete", "{0:0%}");
            xrTCTasksAssignedTo.DataBindings.Add("Text", null, "AssignedTo");
            xrTCTasksStartDate.DataBindings.Add("Text", null, "StartDate", "{0:MMM-dd-yyyy}");
            xrTCTasksDueDate.DataBindings.Add("Text", null, "DueDate", "{0:MMM-dd-yyyy}");
            
            xrTCTasksActualHours.DataBindings.Add("Text", null, "UGITDuration");
            xrTCTasksEstimateHours.DataBindings.Add("Text", null, "UGITContribution");
            xrTCTaskERH.DataBindings.Add("Text", null, "EstimatedRemainingHours");
            
            xrTCERHActualvsEstimate.DataBindings.Add("Text", null, "ActualVariance");
            xrTCTaskProjected.DataBindings.Add("Text", null, "Projected");
            xrTCTaskProjectedvsEstimate.DataBindings.Add("Text", null, "ProjectedEstimate");
            xrTCTaskComment.DataBindings.Add("Text", null, "PMMComment");

            xrLblUGITLevel.DataBindings.Add("Text", null, "UGITLevel");
            xrLblChildCount.DataBindings.Add("Text", null, "UGITChildCount");
            

            datatable.DefaultView.Sort = "ItemOrder ASC";

            Report.DataSource = datatable.DefaultView.ToTable();
        }

        private void xrLblTaskTitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            if (xrLblChildCount.Text != string.Empty)
            {
                int count = UGITUtility.StringToInt(xrLblChildCount.Text);
                if (count > 0)
                {
                    label.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    label.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            if (xrLblUGITLevel.Text != string.Empty)
            {
                int count = UGITUtility.StringToInt(xrLblUGITLevel.Text);
                if (count > 0)
                {
                    label.Padding = new DevExpress.XtraPrinting.PaddingInfo(10 * count, 0, 0, 0);
                }
                else
                {
                    label.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0);
                }
                label.Borders = DevExpress.XtraPrinting.BorderSide.None;
                label.StylePriority.UsePadding = false;
            }
        }

        private void xrTCTasksDuration_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //string value = xrLblTaskDuration.Text + ((!string.IsNullOrEmpty(xrLblTaskDuration.Text) && Convert.ToDouble(xrLblTaskDuration.Text) == 1) ? " Day" : " Days");
            //value = value + " (" + xrLblTaskContribution.Text + "%)";
            //xrTCTasksEstimateHours.Text = value;
        }

    }
}
