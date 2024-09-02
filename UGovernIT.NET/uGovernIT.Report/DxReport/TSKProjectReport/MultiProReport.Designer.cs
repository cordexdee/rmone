namespace uGovernIT.Report.DxReport
{
    partial class MultiProReport
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
            this.xrlblTicket = new DevExpress.XtraReports.UI.XRLabel();
            this.xrProjectSubReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlblTicket,
            this.xrProjectSubReport});
            this.Detail.HeightF = 101.0417F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrlblTicket
            // 
            this.xrlblTicket.LocationFloat = new DevExpress.Utils.PointFloat(1000F, 0F);
            this.xrlblTicket.Name = "xrlblTicket";
            this.xrlblTicket.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblTicket.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrlblTicket.Text = "TicketId";
            this.xrlblTicket.Visible = false;
            // 
            // xrProjectSubReport
            // 
            this.xrProjectSubReport.LocationFloat = new DevExpress.Utils.PointFloat(10F, 0F);
            this.xrProjectSubReport.Name = "xrProjectSubReport";
            this.xrProjectSubReport.SizeF = new System.Drawing.SizeF(1090F, 101.0417F);
            this.xrProjectSubReport.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrProjectSubReport_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 19F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 11F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // MultiProReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Bookmark = "Projects Report";
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 19, 11);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Version = "12.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRSubreport xrProjectSubReport;
        private DevExpress.XtraReports.UI.XRLabel xrlblTicket;
    }
}
