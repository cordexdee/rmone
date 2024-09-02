namespace uGovernIT.Manager.Reports
{
    partial class UsersDetailTaskListReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTTasksDetails = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrlblNumber = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellResource = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCTaskAssigne = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCTaskCompleted = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCOnTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCCompletedLate = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCPending = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCOverdue = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrEvenControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrOddControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTTasksDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTTasksDetails});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 25F;
            this.Detail.KeepTogether = true;
            this.Detail.KeepTogetherWithDetailReports = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTTasksDetails
            // 
            this.xrTTasksDetails.BackColor = System.Drawing.Color.Transparent;
            this.xrTTasksDetails.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.xrTTasksDetails.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTTasksDetails.Dpi = 100F;
            this.xrTTasksDetails.EvenStyleName = "xrEvenControlStyle";
            this.xrTTasksDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTTasksDetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTTasksDetails.Name = "xrTTasksDetails";
            this.xrTTasksDetails.OddStyleName = "xrOddControlStyle";
            this.xrTTasksDetails.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTTasksDetails.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow25});
            this.xrTTasksDetails.SizeF = new System.Drawing.SizeF(815.25F, 25F);
            this.xrTTasksDetails.StylePriority.UseBorderColor = false;
            this.xrTTasksDetails.StylePriority.UseBorders = false;
            this.xrTTasksDetails.StylePriority.UseFont = false;
            this.xrTTasksDetails.StylePriority.UsePadding = false;
            this.xrTTasksDetails.StylePriority.UseTextAlignment = false;
            this.xrTTasksDetails.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrlblNumber,
            this.xrTableCellResource,
            this.xrTCTaskAssigne,
            this.xrTCTaskCompleted,
            this.xrTCOnTime,
            this.xrTCCompletedLate,
            this.xrTCPending,
            this.xrTCOverdue});
            this.xrTableRow25.Dpi = 100F;
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1D;
            // 
            // xrlblNumber
            // 
            this.xrlblNumber.Dpi = 100F;
            this.xrlblNumber.Name = "xrlblNumber";
            this.xrlblNumber.Weight = 0.055394172474599754D;
            // 
            // xrTableCellResource
            // 
            this.xrTableCellResource.Dpi = 100F;
            this.xrTableCellResource.Name = "xrTableCellResource";
            this.xrTableCellResource.StylePriority.UseTextAlignment = false;
            this.xrTableCellResource.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCellResource.Weight = 0.40622394491807173D;
            // 
            // xrTCTaskAssigne
            // 
            this.xrTCTaskAssigne.Dpi = 100F;
            this.xrTCTaskAssigne.Name = "xrTCTaskAssigne";
            this.xrTCTaskAssigne.StylePriority.UseTextAlignment = false;
            this.xrTCTaskAssigne.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCTaskAssigne.Weight = 0.19387960948020652D;
            // 
            // xrTCTaskCompleted
            // 
            this.xrTCTaskCompleted.Dpi = 100F;
            this.xrTCTaskCompleted.Name = "xrTCTaskCompleted";
            this.xrTCTaskCompleted.StylePriority.UseTextAlignment = false;
            this.xrTCTaskCompleted.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCTaskCompleted.Weight = 0.22157670097486634D;
            // 
            // xrTCOnTime
            // 
            this.xrTCOnTime.Dpi = 100F;
            this.xrTCOnTime.Name = "xrTCOnTime";
            this.xrTCOnTime.StylePriority.UseTextAlignment = false;
            this.xrTCOnTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCOnTime.Weight = 0.1292530697678817D;
            // 
            // xrTCCompletedLate
            // 
            this.xrTCCompletedLate.Dpi = 100F;
            this.xrTCCompletedLate.Name = "xrTCCompletedLate";
            this.xrTCCompletedLate.StylePriority.UseTextAlignment = false;
            this.xrTCCompletedLate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCCompletedLate.Weight = 0.22157670044338207D;
            // 
            // xrTCPending
            // 
            this.xrTCPending.Dpi = 100F;
            this.xrTCPending.Name = "xrTCPending";
            this.xrTCPending.StylePriority.UseTextAlignment = false;
            this.xrTCPending.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCPending.Weight = 0.13848543139705677D;
            // 
            // xrTCOverdue
            // 
            this.xrTCOverdue.Dpi = 100F;
            this.xrTCOverdue.Name = "xrTCOverdue";
            this.xrTCOverdue.StylePriority.UseTextAlignment = false;
            this.xrTCOverdue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCOverdue.Weight = 0.13894706519941147D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 5F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 5F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8,
            this.xrLabel1});
            this.ReportHeader.Dpi = 100F;
            this.ReportHeader.HeightF = 63.99993F;
            this.ReportHeader.KeepTogether = true;
            this.ReportHeader.LockedInUserDesigner = true;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable8
            // 
            this.xrTable8.BackColor = System.Drawing.Color.LightGray;
            this.xrTable8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.xrTable8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable8.Dpi = 100F;
            this.xrTable8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(0.2500034F, 38.99993F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow23});
            this.xrTable8.SizeF = new System.Drawing.SizeF(815F, 25F);
            this.xrTable8.StylePriority.UseBackColor = false;
            this.xrTable8.StylePriority.UseBorderColor = false;
            this.xrTable8.StylePriority.UseBorders = false;
            this.xrTable8.StylePriority.UseFont = false;
            this.xrTable8.StylePriority.UsePadding = false;
            this.xrTable8.StylePriority.UseTextAlignment = false;
            this.xrTable8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23,
            this.xrTableCell33,
            this.xrTableCell1,
            this.xrTableCell42,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell47,
            this.xrTableCell3});
            this.xrTableRow23.Dpi = 100F;
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 100F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "#";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell23.Weight = 0.055389554029617247D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Dpi = 100F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 5, 0, 0, 100F);
            this.xrTableCell33.StylePriority.UsePadding = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "Resource";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            this.xrTableCell33.Weight = 0.40619005158436033D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 100F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Tasks Assigned";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.19386344179001708D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Dpi = 100F;
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseTextAlignment = false;
            this.xrTableCell42.Text = "Tasks Completed";
            this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell42.Weight = 0.22155822348186377D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Dpi = 100F;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.Text = "On Time";
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell40.Weight = 0.12924229313538249D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 100F;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "Completed Late";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell41.Weight = 0.22155821146662938D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Dpi = 100F;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.Text = "Pending";
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell47.Weight = 0.13847388385101739D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 100F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Overdue";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.13847389623000056D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(299.4024F, 10F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(245.5976F, 23F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorderColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Resource Usage";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrEvenControlStyle
            // 
            this.xrEvenControlStyle.BackColor = System.Drawing.Color.White;
            this.xrEvenControlStyle.Name = "xrEvenControlStyle";
            this.xrEvenControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrOddControlStyle
            // 
            this.xrOddControlStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.xrOddControlStyle.Name = "xrOddControlStyle";
            this.xrOddControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // UsersDetailTaskListReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(5, 5, 5, 5);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrEvenControlStyle,
            this.xrOddControlStyle});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTTasksDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable8;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell33;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell42;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell40;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell41;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell47;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTable xrTTasksDetails;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow25;
        private DevExpress.XtraReports.UI.XRTableCell xrlblNumber;
        private DevExpress.XtraReports.UI.XRTableCell xrTCTaskCompleted;
        private DevExpress.XtraReports.UI.XRTableCell xrTCOnTime;
        private DevExpress.XtraReports.UI.XRTableCell xrTCCompletedLate;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPending;
        private DevExpress.XtraReports.UI.XRTableCell xrTCOverdue;
        private DevExpress.XtraReports.UI.XRControlStyle xrEvenControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle xrOddControlStyle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCTaskAssigne;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCellResource;
    }
}
