namespace uGovernIT.Report.DxReport
{
    partial class AccompolishmentReport
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
            this.xrTDAccomplishment = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTCAccNum = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCTitle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCCompletedOn = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCNote = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTHeaderAccomplishment = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrEvenControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrOddControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTDAccomplishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTHeaderAccomplishment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTDAccomplishment});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTDAccomplishment
            // 
            this.xrTDAccomplishment.BackColor = System.Drawing.Color.Empty;
            this.xrTDAccomplishment.BorderColor = System.Drawing.Color.Black;
            this.xrTDAccomplishment.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTDAccomplishment.Dpi = 100F;
            this.xrTDAccomplishment.EvenStyleName = "xrEvenControlStyle";
            this.xrTDAccomplishment.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTDAccomplishment.Name = "xrTDAccomplishment";
            this.xrTDAccomplishment.OddStyleName = "xrOddControlStyle";
            this.xrTDAccomplishment.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTDAccomplishment.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTDAccomplishment.SizeF = new System.Drawing.SizeF(1083F, 25F);
            this.xrTDAccomplishment.StylePriority.UseBorderColor = false;
            this.xrTDAccomplishment.StylePriority.UseBorders = false;
            this.xrTDAccomplishment.StylePriority.UsePadding = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTCAccNum,
            this.xrTCTitle,
            this.xrTCCompletedOn,
            this.xrTCNote});
            this.xrTableRow2.Dpi = 100F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTCAccNum
            // 
            this.xrTCAccNum.Dpi = 100F;
            this.xrTCAccNum.Name = "xrTCAccNum";
            this.xrTCAccNum.StylePriority.UseTextAlignment = false;
            this.xrTCAccNum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTCAccNum.Weight = 0.075443249885056574D;
            // 
            // xrTCTitle
            // 
            this.xrTCTitle.Dpi = 100F;
            this.xrTCTitle.Name = "xrTCTitle";
            this.xrTCTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCTitle.StylePriority.UsePadding = false;
            this.xrTCTitle.StylePriority.UseTextAlignment = false;
            this.xrTCTitle.Text = "Title";
            this.xrTCTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCTitle.Weight = 0.90811322467291766D;
            // 
            // xrTCCompletedOn
            // 
            this.xrTCCompletedOn.Dpi = 100F;
            this.xrTCCompletedOn.Name = "xrTCCompletedOn";
            this.xrTCCompletedOn.StylePriority.UseTextAlignment = false;
            this.xrTCCompletedOn.Text = "xrTCCompletedOn";
            this.xrTCCompletedOn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCCompletedOn.Weight = 0.33530333104689419D;
            // 
            // xrTCNote
            // 
            this.xrTCNote.Dpi = 100F;
            this.xrTCNote.Name = "xrTCNote";
            this.xrTCNote.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCNote.StylePriority.UsePadding = false;
            this.xrTCNote.StylePriority.UseTextAlignment = false;
            this.xrTCNote.Text = "ProjectNotes";
            this.xrTCNote.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCNote.Weight = 1.707252744403601D;
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
            this.xrTHeaderAccomplishment});
            this.PageHeader.Dpi = 100F;
            this.PageHeader.HeightF = 40F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTHeaderAccomplishment
            // 
            this.xrTHeaderAccomplishment.BackColor = System.Drawing.Color.LightGray;
            this.xrTHeaderAccomplishment.BorderColor = System.Drawing.Color.Black;
            this.xrTHeaderAccomplishment.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTHeaderAccomplishment.Dpi = 100F;
            this.xrTHeaderAccomplishment.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTHeaderAccomplishment.LocationFloat = new DevExpress.Utils.PointFloat(0F, 15F);
            this.xrTHeaderAccomplishment.Name = "xrTHeaderAccomplishment";
            this.xrTHeaderAccomplishment.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTHeaderAccomplishment.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTHeaderAccomplishment.SizeF = new System.Drawing.SizeF(1083F, 25F);
            this.xrTHeaderAccomplishment.StylePriority.UseBackColor = false;
            this.xrTHeaderAccomplishment.StylePriority.UseBorderColor = false;
            this.xrTHeaderAccomplishment.StylePriority.UseBorders = false;
            this.xrTHeaderAccomplishment.StylePriority.UseFont = false;
            this.xrTHeaderAccomplishment.StylePriority.UsePadding = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell3});
            this.xrTableRow1.Dpi = 100F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 100F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Weight = 0.075247965640521039D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 100F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Accomplishments";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell2.Weight = 0.90576258359910522D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 100F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Completed On";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.33443539964309971D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 100F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Notes";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 1.7028335382464048D;
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
            // AccompolishmentReport
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
            ((System.ComponentModel.ISupportInitialize)(this.xrTDAccomplishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTHeaderAccomplishment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTHeaderAccomplishment;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrTDAccomplishment;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTCAccNum;
        private DevExpress.XtraReports.UI.XRTableCell xrTCTitle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCNote;
        private DevExpress.XtraReports.UI.XRControlStyle xrEvenControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle xrOddControlStyle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCCompletedOn;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
    }
}
