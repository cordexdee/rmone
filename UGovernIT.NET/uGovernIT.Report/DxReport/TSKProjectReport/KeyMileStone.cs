using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Text;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class KeyMileStone : DevExpress.XtraReports.UI.XtraReport
    {

       
        public KeyMileStone(DataTable datatable)
        {
            InitializeComponent();
            BindSummaryTasks(datatable);
        }

        private void BindSummaryTasks(DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                //row[DatabaseObjects.Columns.PercentComplete] = Convert.ToDouble(row[DatabaseObjects.Columns.PercentComplete]) / 100;
                row[DatabaseObjects.Columns.TaskBehaviour] = UGITUtility.GetImageUrlForReport( GetImage(Convert.ToString(row[DatabaseObjects.Columns.TaskBehaviour])));
            }

            //xrTCSumTasksNum.DataBindings.Add("Text", null, "ItemOrder");
            xrTCSumTasksNum.Summary.Func = SummaryFunc.RecordNumber;
            xrTCSumTasksNum.Summary.FormatString = "{0:#}";
            xrTCSumTasksNum.Summary.Running = SummaryRunning.Report;
            //xrTCSumTasksTitle.DataBindings.Add("Text", null, "Title");
            //xrRTTitle.DataBindings.Add("Text", null, "Title");
            xrLblTitle.DataBindings.Add("Text", null, "Title");

            xrTCSumTasksDescription.DataBindings.Add("Text", null, DatabaseObjects.Columns.Status);
            xrTCSumTasksPrecentComplete.DataBindings.Add("Text", null, "PercentComplete", "{0:0%}");
            xrTCSumTasksStartDate.DataBindings.Add("Text", null, "StartDate", "{0:MMM-dd-yyyy}");
            xrTCSumTasksDueDate.DataBindings.Add("Text", null, "DueDate", "{0:MMM-dd-yyyy}");
            xrLblUGITLevel.DataBindings.Add("Text", null, "UGITLevel");
            xrLblChildCount.DataBindings.Add("Text", null, "UGITChildCount");
            //xrLblTaskBehavier.DataBindings.Add("Text", null, DatabaseObjects.Columns.TaskBehaviour);
            xrPBicon.DataBindings.Add("ImageUrl", null, DatabaseObjects.Columns.TaskBehaviour);
            //xrPBicon.Padding = 5;

            datatable.DefaultView.Sort = "ItemOrder ASC";
            Report.DataSource = datatable.DefaultView.ToTable(); ;
        }



        private string GetImage(string behaviour)
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

        private void xrTCSumTasksDescription_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTCSumTasksDescription.Text = UGITUtility.StripHTML(xrTCSumTasksDescription.Text,false); //Microsoft.SharePoint.Utilities.SPHttpUtility.HtmlDecode(xrTCSumTasksDescription.Text);
        }

        private void xrLblTitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
          
            XRTableCell tc = label.Parent as XRTableCell;

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

               
                if (tc != null && tc.Controls.Count > 0)
                {
                    XRPictureBox image = tc.Controls[1] as XRPictureBox;
                    if (count > 0)
                    {
                        //float imgaxis = (23 * count);
                        //float nonimgaxis = 20 * count;
                        //float lblxaxis = 45 * count;
                        if (image != null && !string.IsNullOrEmpty(image.ImageUrl))
                        {
                            image.StylePriority.UsePadding = true;
                            this.xrPBicon.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.416665F);
                            image.LocationFloat = new DevExpress.Utils.PointFloat(23F, 3.416665F);
                            label.LocationFloat = new DevExpress.Utils.PointFloat(45F, 0F);
                        }
                        else
                        {
                            label.LocationFloat = new DevExpress.Utils.PointFloat(20F, 0F);
                            image.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.416665F);
                        }
                        //image.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0);
                    }
                    else if (image != null && !string.IsNullOrEmpty(image.ImageUrl))
                    {
                        image.StylePriority.UsePadding = true;
                        this.xrPBicon.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.416665F);
                        image.LocationFloat = new DevExpress.Utils.PointFloat(18F, 3.416665F);
                        label.LocationFloat = new DevExpress.Utils.PointFloat(38F, 0F);
                    }
                    else
                    {
                        label.LocationFloat = new DevExpress.Utils.PointFloat(20F, 0F);
                        image.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.416665F);
                    }
                }
                label.Borders = DevExpress.XtraPrinting.BorderSide.None;
                label.StylePriority.UsePadding = true;

            }
        }
    }
}
