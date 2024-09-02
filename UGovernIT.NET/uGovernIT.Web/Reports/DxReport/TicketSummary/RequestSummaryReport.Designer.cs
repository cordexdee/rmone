namespace uGovernIT.DxReport
{
    partial class RequestSummaryReport
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTCRequestTitle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCRequestCount = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLblRequestSummary = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(275F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTCRequestTitle,
            this.xrTCRequestCount});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTCRequestTitle
            // 
            this.xrTCRequestTitle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTCRequestTitle.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTCRequestTitle.Name = "xrTCRequestTitle";
            this.xrTCRequestTitle.StylePriority.UseBorders = false;
            this.xrTCRequestTitle.StylePriority.UseFont = false;
            this.xrTCRequestTitle.StylePriority.UseTextAlignment = false;
            this.xrTCRequestTitle.Text = "xrTCRequestTitle";
            this.xrTCRequestTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCRequestTitle.Weight = 2.1999998446893514D;
            // 
            // xrTCRequestCount
            // 
            this.xrTCRequestCount.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTCRequestCount.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.xrTCRequestCount.Name = "xrTCRequestCount";
            this.xrTCRequestCount.StylePriority.UseBorders = false;
            this.xrTCRequestCount.StylePriority.UseFont = false;
            this.xrTCRequestCount.StylePriority.UseTextAlignment = false;
            this.xrTCRequestCount.Text = "xrTCRequestCount";
            this.xrTCRequestCount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCRequestCount.Weight = 0.5499998529711031D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 1.041667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLblRequestSummary
            // 
            this.xrLblRequestSummary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLblRequestSummary.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLblRequestSummary.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrLblRequestSummary.ForeColor = System.Drawing.Color.Black;
            this.xrLblRequestSummary.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLblRequestSummary.Name = "xrLblRequestSummary";
            this.xrLblRequestSummary.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblRequestSummary.SizeF = new System.Drawing.SizeF(275F, 23F);
            this.xrLblRequestSummary.StylePriority.UseBackColor = false;
            this.xrLblRequestSummary.StylePriority.UseBorders = false;
            this.xrLblRequestSummary.StylePriority.UseFont = false;
            this.xrLblRequestSummary.StylePriority.UseForeColor = false;
            this.xrLblRequestSummary.StylePriority.UseTextAlignment = false;
            this.xrLblRequestSummary.Text = "Ticket Creation Statistics by Age";
            this.xrLblRequestSummary.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLblRequestSummary});
            this.ReportHeader.HeightF = 23F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // RequestSummaryReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(14, 100, 100, 1);
            this.Version = "15.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLblRequestSummary;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTCRequestTitle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCRequestCount;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
    }
}
