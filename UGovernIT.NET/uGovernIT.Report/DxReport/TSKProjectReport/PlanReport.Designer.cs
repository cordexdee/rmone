namespace uGovernIT.Report.DxReport
{
    partial class PlanReport
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
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTCPlanNum = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCPlanTitle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCPlannedDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCPlanProjectsNotes = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrEvenControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrOddControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable5
            // 
            this.xrTable5.BackColor = System.Drawing.Color.Empty;
            this.xrTable5.BorderColor = System.Drawing.Color.Black;
            this.xrTable5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable5.Dpi = 100F;
            this.xrTable5.EvenStyleName = "xrEvenControlStyle";
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.OddStyleName = "xrOddControlStyle";
            this.xrTable5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow18});
            this.xrTable5.SizeF = new System.Drawing.SizeF(1083F, 25F);
            this.xrTable5.StylePriority.UseBorderColor = false;
            this.xrTable5.StylePriority.UseBorders = false;
            this.xrTable5.StylePriority.UsePadding = false;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTCPlanNum,
            this.xrTCPlanTitle,
            this.xrTCPlannedDate,
            this.xrTCPlanProjectsNotes});
            this.xrTableRow18.Dpi = 100F;
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTCPlanNum
            // 
            this.xrTCPlanNum.Dpi = 100F;
            this.xrTCPlanNum.Name = "xrTCPlanNum";
            this.xrTCPlanNum.StylePriority.UseTextAlignment = false;
            this.xrTCPlanNum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTCPlanNum.Weight = 0.075443249885056574D;
            // 
            // xrTCPlanTitle
            // 
            this.xrTCPlanTitle.Dpi = 100F;
            this.xrTCPlanTitle.Name = "xrTCPlanTitle";
            this.xrTCPlanTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCPlanTitle.StylePriority.UsePadding = false;
            this.xrTCPlanTitle.StylePriority.UseTextAlignment = false;
            this.xrTCPlanTitle.Text = "Title";
            this.xrTCPlanTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCPlanTitle.Weight = 0.90811322467291766D;
            // 
            // xrTCPlannedDate
            // 
            this.xrTCPlannedDate.Dpi = 100F;
            this.xrTCPlannedDate.Name = "xrTCPlannedDate";
            this.xrTCPlannedDate.StylePriority.UseTextAlignment = false;
            this.xrTCPlannedDate.Text = "xrTCPlannedDate";
            this.xrTCPlannedDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCPlannedDate.Weight = 0.33530333104689414D;
            this.xrTCPlannedDate.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTCPlannedDate_BeforePrint);
            // 
            // xrTCPlanProjectsNotes
            // 
            this.xrTCPlanProjectsNotes.Dpi = 100F;
            this.xrTCPlanProjectsNotes.Name = "xrTCPlanProjectsNotes";
            this.xrTCPlanProjectsNotes.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCPlanProjectsNotes.StylePriority.UsePadding = false;
            this.xrTCPlanProjectsNotes.StylePriority.UseTextAlignment = false;
            this.xrTCPlanProjectsNotes.Text = "ProjectNotes";
            this.xrTCPlanProjectsNotes.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCPlanProjectsNotes.Weight = 1.7072527444036021D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 8.333333F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 10.41667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.PageHeader.Dpi = 100F;
            this.PageHeader.HeightF = 42.70833F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable4
            // 
            this.xrTable4.BackColor = System.Drawing.Color.LightGray;
            this.xrTable4.BorderColor = System.Drawing.Color.Black;
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.Dpi = 100F;
            this.xrTable4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0.0004882813F, 17.70833F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1083F, 25F);
            this.xrTable4.StylePriority.UseBackColor = false;
            this.xrTable4.StylePriority.UseBorderColor = false;
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UsePadding = false;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell21,
            this.xrTableCell1,
            this.xrTableCell22});
            this.xrTableRow17.Dpi = 100F;
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 100F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Weight = 0.075247965640521039D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Dpi = 100F;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Immediate Plans";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell21.Weight = 0.90576258359910522D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 100F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Planned Date";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.33443539964309987D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 100F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Notes";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell22.Weight = 1.7028321774252775D;
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
            // PlanReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader});
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(8, 9, 8, 10);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrEvenControlStyle,
            this.xrOddControlStyle});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable5;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow18;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPlanNum;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPlanTitle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPlanProjectsNotes;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell21;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell22;
        private DevExpress.XtraReports.UI.XRControlStyle xrEvenControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle xrOddControlStyle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPlannedDate;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
    }
}
