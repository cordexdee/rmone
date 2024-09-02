using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web;
using System.Net.Mime;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class TaskSummary_Report : DevExpress.XtraReports.UI.XtraReport
    {
        public TaskSummary_Report(DataTable dataSource)
        {
            InitializeComponent();
            BindAllTasks(dataSource);
        }

        private void BindAllTasks(DataTable datatable)
        {
            if (datatable == null)
                return;

            foreach (DataRow row in datatable.Rows)
            {
                row["PercentComplete"] = Convert.ToDouble(row["PercentComplete"]) / 100;
                //row[DatabaseObjects.Columns.TaskBehaviour] = GetBehaviourIcon(Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour]));
            }
            xrTCItemOrder.DataBindings.Add("Text", null, DatabaseObjects.Columns.ItemOrder);
            xrLblTaskTitle.DataBindings.Add("Text", null, DatabaseObjects.Columns.Title);
            xrTCStatus.DataBindings.Add("Text", null, DatabaseObjects.Columns.Status);
            xrTCPrecentComplete.DataBindings.Add("Text", null, DatabaseObjects.Columns.PercentComplete, uGITFormatConstants.PrecentFormat);
            xrTCPred.DataBindings.Add("Text", null, DatabaseObjects.Columns.PredecessorsByOrder);
            xrTCStartDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.StartDate, uGITFormatConstants.DateFormat);
            xrTCDueDate.DataBindings.Add("Text", null, DatabaseObjects.Columns.DueDate, uGITFormatConstants.DateFormat);
            xrTCAssignTo.DataBindings.Add("Text", null, DatabaseObjects.Columns.AssignedTo);
            xrLblUGITLevel.DataBindings.Add("Text", null, DatabaseObjects.Columns.UGITLevel);
            xrLblChildCount.DataBindings.Add("Text", null, DatabaseObjects.Columns.UGITChildCount);
            xrPBicon.DataBindings.Add("ImageUrl", null, DatabaseObjects.Columns.BehaviourIcon);

            GroupHeaderProject.GroupFields.Add(new GroupField("ProjectID"));
            xrLblGroupHeader.Text = "[ProjectID]   [TitleWithPctComplete]";

            datatable.DefaultView.Sort = "ItemOrder ASC";

            Report.DataSource = datatable.DefaultView.ToTable();
        }

        //private string GetBehaviourIcon(string behaviour)
        //{
        //    if (string.IsNullOrEmpty(behaviour))
        //        return string.Empty;

        //    string fileName = string.Empty;
        //    switch (behaviour)
        //    {
        //        case Constants.TaskType.Milestone:
        //            fileName = @"/_layouts/15/images/uGovernIT/milestone_icon.png";
        //            break;
        //        case Constants.TaskType.Deliverable:
        //            fileName = @"/_layouts/15/images/uGovernIT/document_down.png";
        //            break;
        //        case Constants.TaskType.Receivable:
        //            fileName = @"/_layouts/15/images/uGovernIT/document_up.png";
        //            break;
        //    }

        //    return fileName;
        //}

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
    }
}
