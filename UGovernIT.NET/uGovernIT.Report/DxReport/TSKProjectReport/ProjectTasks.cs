using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class ProjectTasks : DevExpress.XtraReports.UI.XtraReport
    {
        private TopMarginBand topMarginBand1;
        private DetailBand detailBand1;
        private BottomMarginBand bottomMarginBand1;
        private bool openTaskOnly;
        public ProjectTasks(DataTable datatable, bool openTaskOnly = false)
        {
            this.openTaskOnly = openTaskOnly;
            InitializeComponent();

            if (openTaskOnly)
                xrLabel1.Text = "Project Tasks - Open Tasks Only";
            BindAllTasks(datatable);
        }

        private void BindAllTasks(DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                row["PercentComplete"] = Convert.ToDouble(row["PercentComplete"]) / 100;
                row[DatabaseObjects.Columns.TaskBehaviour] = UGITUtility.GetImageUrlForReport(GetBehaviourIcon(Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour])));
            }
            xrTCTasksNum.DataBindings.Add("Text", null, "ItemOrder");
            xrLblTaskTitle.DataBindings.Add("Text", null, "Title");
            xrTCTasksStatus.DataBindings.Add("Text", null, "Status");
            xrTCTasksPrecentComplete.DataBindings.Add("Text", null, "PercentComplete", "{0:0%}");
            xrTCTasksPredecessor.DataBindings.Add("Text", null, "PredecessorsByorder");
            xrTCTasksStartDate.DataBindings.Add("Text", null, "StartDate", "{0:MMM-dd-yyyy}");
            xrTCTasksDueDate.DataBindings.Add("Text", null, "DueDate", "{0:MMM-dd-yyyy}");
            xrTCTasksAssignedTo.DataBindings.Add("Text", null, "AssignedTo");
            xrLblTaskDuration.DataBindings.Add("Text", null, "UGITDuration");
            xrLblTaskContribution.DataBindings.Add("Text", null, "UGITContribution");
            xrLblUGITLevel.DataBindings.Add("Text", null, "UGITLevel");
            xrLblChildCount.DataBindings.Add("Text", null, "UGITChildCount");
            xrPBicon.DataBindings.Add("ImageUrl", null, DatabaseObjects.Columns.TaskBehaviour);

            datatable.DefaultView.Sort = "ItemOrder ASC";

            Report.DataSource = datatable.DefaultView.ToTable();
        }

        private string GetBehaviourIcon(string behaviour)
        {
            if (string.IsNullOrEmpty(behaviour))
                return string.Empty;

            string fileName = string.Empty;
            switch (behaviour)
            {
                case Constants.TaskType.Milestone:
                    fileName = @"/Report/Content/images/milestone_icon.png";
                    break;
                case Constants.TaskType.Deliverable:
                    fileName = @"/Report/Content/images/document_down.png";
                    break;
                case Constants.TaskType.Receivable:
                    fileName = @"/Report/Content/images/document_up.png";
                    break;
            }

            return fileName;
        }

        private void xrLblTaskTitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            if (xrLblChildCount.Text != string.Empty)
            {
                int count = Convert.ToInt32(xrLblChildCount.Text);
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
                int count = Convert.ToInt32(xrLblUGITLevel.Text);
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
            string value = xrLblTaskDuration.Text + ((!string.IsNullOrEmpty(xrLblTaskDuration.Text) && Convert.ToDouble(xrLblTaskDuration.Text) == 1) ? " Day" : " Days");
            value = value + " (" + xrLblTaskContribution.Text + "%)";
            xrTCTasksDuration.Text = value;
        }

        private void IInitializeComponent()
        {
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Name = "detailBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ProjectTasks
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
