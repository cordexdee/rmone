using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Drawing.Printing;
using DevExpress.XtraPrinting;
using uGovernIT.Utility;

namespace uGovernIT.Manager.Reports
{
    public partial class ApplicationAccessReport : DevExpress.XtraReports.UI.XtraReport
    {
        bool IgnoreAdditionalSetting { get; set; }
        public ApplicationAccessReport(DataTable dtModuleAccess, string ApplicationId,bool ignoreAdditionalSetting)
        {
            InitializeComponent();
            IgnoreAdditionalSetting = ignoreAdditionalSetting;
            CreateDynamicTables(dtModuleAccess);
            Report.DataSource = dtModuleAccess;
            if (dtModuleAccess != null && dtModuleAccess.Rows.Count > 0)
            {
                xrLblMessage.Visible = false;
                xrTable2.Visible = true;
                GroupHeaderModules.Visible = true;
                Detail.Visible = true;
                xrPanel2.Visible = true;
            }
            else
            {
                xrLblMessage.Visible = true;
                xrLblMessage.Text = "No Data Exists.";
                xrTable2.Visible = false;
                GroupHeaderModules.Visible = false;
                Detail.Visible = false;
                xrPanel2.Visible = false;
            }

            xrTCUser.DataBindings.Add("Text", null, "Assignees");
            xrLblHeader.Text = "Application Permission Report for " + ApplicationId;


        }

        private void CreateDynamicTables(DataTable dt)
        {
            XRTable tabledetails = new XRTable();
            XRTable tableheader = new XRTable();
            XRTableRow tr = new XRTableRow();
            XRTableRow trh = new XRTableRow();
            trh.TextAlignment = TextAlignment.MiddleCenter;
            this.PaperKind = PaperKind.Custom;
            this.PageSize = new Size(1300, 850);
            float width = 150;
            int cColCount = 0;
            float firstColWidth = 202.25F;

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.ToLower() != "modules" && col.ColumnName.ToLower() != "assignees")
                {
                    XRTableCell tch = new XRTableCell();
                    tch.Text = col.ColumnName;
                    if (!IgnoreAdditionalSetting)
                        tch.WidthF = width;
                    trh.Cells.Add(tch);

                    XRTableCell tc = new XRTableCell();
                    tc.Name = "xrtc" + col.ColumnName;

                    XRPictureBox pb = new XRPictureBox();
                    pb.DataBindings.Add("Text", null, col.ColumnName);
                    pb.Borders = BorderSide.None;
                  
                    pb.BeforePrint += new PrintEventHandler(tc_BeforePrint);
                    pb.LocationF = new PointF(15, 5);

                    float widthCell = tc.WidthF - (tc.WidthF / 2) - 40;
                    pb.SizeF = new System.Drawing.SizeF(widthCell+7, 18F);
                    pb.Sizing = ImageSizeMode.AutoSize;
                    tc.Controls.Add(pb);
                    tr.Cells.Add(tc);
                }
            }

            tabledetails.Rows.Add(tr);
            tabledetails.AdjustSize();
            #region Set width
            if (!IgnoreAdditionalSetting)
            {
                xrLblHeader.WidthF = cColCount * width + firstColWidth;
                xrPanel2.WidthF = cColCount * width + firstColWidth;
                xrTCPermissionDetails.WidthF = cColCount * width;
                xrTCPermissionHeader.WidthF = cColCount * width;
                xrLblModules.WidthF = cColCount * width + firstColWidth;
            }

            #endregion
            tabledetails.HeightF = xrTCPermissionDetails.HeightF;
           
            tabledetails.WidthF = xrTCPermissionDetails.WidthF;

            xrTCPermissionDetails.Controls.Add(tabledetails);

            tableheader.Rows.Add(trh);
            tableheader.WidthF = xrTCPermissionHeader.WidthF;
            xrTCPermissionHeader.Controls.Add(tableheader);

            GroupHeaderModules.GroupFields.Add(new GroupField("Modules"));
            xrLblModules.DataBindings.Add("Text", null, "Modules");
            
        }

      
        void tc_BeforePrint(object sender, PrintEventArgs e)
        {
            XRPictureBox pb = (XRPictureBox)sender;
           
            if (pb.Text.ToLower() == "true")
            {
                pb.ImageUrl =UGITUtility.GetImageUrlForReport("/Report/content/images/ugovernit/message_good.png");
            }
            else if (pb.Text.ToLower() == "false")
            {
                pb.ImageUrl = UGITUtility.GetImageUrlForReport("/Report/content/images/ugovernit/Crossicon.jpg");
            }
        }
    }
}
