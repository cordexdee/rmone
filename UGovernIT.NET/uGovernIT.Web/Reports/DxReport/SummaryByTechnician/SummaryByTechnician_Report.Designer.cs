namespace uGovernIT.DxReport
{
    partial class SummaryByTechnician_Report
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
            this.xrPivotGrid = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLblHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.moduleNameHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid});
            this.Detail.HeightF = 145.8333F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPivotGrid
            // 
            this.xrPivotGrid.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivotGrid.Name = "xrPivotGrid";
            this.xrPivotGrid.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid.OptionsView.ShowColumnGrandTotalHeader = false;
            this.xrPivotGrid.OptionsView.ShowColumnGrandTotals = false;
            this.xrPivotGrid.OptionsView.ShowColumnHeaders = false;
            this.xrPivotGrid.OptionsView.ShowColumnTotals = false;
            this.xrPivotGrid.OptionsView.ShowDataHeaders = false;
            this.xrPivotGrid.OptionsView.ShowFilterHeaders = false;
            this.xrPivotGrid.SizeF = new System.Drawing.SizeF(837F, 50F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 50F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLblHeader,
            this.moduleNameHeader});
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLblHeader
            // 
            this.xrLblHeader.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLblHeader.Name = "xrLblHeader";
            this.xrLblHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(56, 2, 0, 0, 100F);
            this.xrLblHeader.SizeF = new System.Drawing.SizeF(837F, 31.33334F);
            this.xrLblHeader.StylePriority.UseFont = false;
            this.xrLblHeader.StylePriority.UsePadding = false;
            this.xrLblHeader.StylePriority.UseTextAlignment = false;
            this.xrLblHeader.Text = "Report By Technician";
            this.xrLblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLblHeader.TextTrimming = System.Drawing.StringTrimming.Word;
            // 
            // moduleNameHeader
            // 
            this.moduleNameHeader.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moduleNameHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 41.33334F);
            this.moduleNameHeader.Name = "moduleNameHeader";
            this.moduleNameHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(56, 2, 0, 0, 100F);
            this.moduleNameHeader.SizeF = new System.Drawing.SizeF(837F, 31.33334F);
            this.moduleNameHeader.StylePriority.UseFont = false;
            this.moduleNameHeader.StylePriority.UsePadding = false;
            this.moduleNameHeader.StylePriority.UseTextAlignment = false;
            this.moduleNameHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.moduleNameHeader.TextTrimming = System.Drawing.StringTrimming.Word;
            // 
            // PageHeader
            // 
            this.PageHeader.Name = "PageHeader";
            // 
            // SummaryByTechnician_Report
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader});
            this.Margins = new System.Drawing.Printing.Margins(5, 8, 0, 50);
            this.Version = "22.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLblHeader;
        private DevExpress.XtraReports.UI.XRLabel moduleNameHeader;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivotGrid;
    }
}
