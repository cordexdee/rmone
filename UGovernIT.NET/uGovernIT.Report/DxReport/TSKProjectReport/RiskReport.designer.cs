namespace uGovernIT.Report.DxReport
{
    partial class RiskReport
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
            this.xrTCRiskNum = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCRiskTitle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCProbability = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCImpact = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCMitigationPlan = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCContingencyPlan = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrEvenControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrOddControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrAssignedTo = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.xrTCRiskNum,
            this.xrTCRiskTitle,
            this.xrTCProbability,
            this.xrTCImpact,
            this.xrAssignedTo,
            this.xrTCMitigationPlan,
            this.xrTCContingencyPlan});
            this.xrTableRow18.Dpi = 100F;
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTCRiskNum
            // 
            this.xrTCRiskNum.Dpi = 100F;
            this.xrTCRiskNum.Name = "xrTCRiskNum";
            this.xrTCRiskNum.StylePriority.UseTextAlignment = false;
            this.xrTCRiskNum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTCRiskNum.Weight = 0.075443249885056574D;
            // 
            // xrTCRiskTitle
            // 
            this.xrTCRiskTitle.Dpi = 100F;
            this.xrTCRiskTitle.Name = "xrTCRiskTitle";
            this.xrTCRiskTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCRiskTitle.StylePriority.UsePadding = false;
            this.xrTCRiskTitle.StylePriority.UseTextAlignment = false;
            this.xrTCRiskTitle.Text = "Title";
            this.xrTCRiskTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCRiskTitle.Weight = 0.70146068393306216D;
            // 
            // xrTCProbability
            // 
            this.xrTCProbability.Dpi = 100F;
            this.xrTCProbability.Name = "xrTCProbability";
            this.xrTCProbability.StylePriority.UseTextAlignment = false;
            this.xrTCProbability.Text = "Probability";
            this.xrTCProbability.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCProbability.Weight = 0.2770910042496777D;
            // 
            // xrTCImpact
            // 
            this.xrTCImpact.Dpi = 100F;
            this.xrTCImpact.Name = "xrTCImpact";
            this.xrTCImpact.StylePriority.UseTextAlignment = false;
            this.xrTCImpact.Text = "Impact";
            this.xrTCImpact.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCImpact.Weight = 0.21014681990567416D;
            // 
            // xrTCMitigationPlan
            // 
            this.xrTCMitigationPlan.Dpi = 100F;
            this.xrTCMitigationPlan.Name = "xrTCMitigationPlan";
            this.xrTCMitigationPlan.Text = "Mitigation Plan";
            this.xrTCMitigationPlan.Weight = 0.61262556935078893D;
            // 
            // xrTCContingencyPlan
            // 
            this.xrTCContingencyPlan.Dpi = 100F;
            this.xrTCContingencyPlan.Name = "xrTCContingencyPlan";
            this.xrTCContingencyPlan.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCContingencyPlan.StylePriority.UsePadding = false;
            this.xrTCContingencyPlan.StylePriority.UseTextAlignment = false;
            this.xrTCContingencyPlan.Text = "Contingency Plan";
            this.xrTCContingencyPlan.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCContingencyPlan.Weight = 0.87516472723475447D;
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
            this.xrTableCell43,
            this.xrTableCell1,
            this.xrTableCell3,
            this.xrTableCell2,
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
            this.xrTableCell21.ForeColor = System.Drawing.Color.Red;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseForeColor = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Risks";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell21.Weight = 0.69964351521319412D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 100F;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseTextAlignment = false;
            this.xrTableCell43.Text = "Probability";
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell43.Weight = 0.27637375492553762D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 100F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Impact";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.2096028549952092D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 100F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Mitigation Plan";
            this.xrTableCell2.Weight = 0.61103980426814752D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 100F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Contingency Plan";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell22.Weight = 0.87289951053962189D;
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
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 100F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Assigned To";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.27347078462096741D;
            // 
            // xrAssignedTo
            // 
            this.xrAssignedTo.Dpi = 100F;
            this.xrAssignedTo.Name = "xrAssignedTo";
            this.xrAssignedTo.StylePriority.UseTextAlignment = false;
            this.xrAssignedTo.Text = "Assigned To";
            this.xrAssignedTo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrAssignedTo.Weight = 0.27418049880472295D;
            // 
            // RiskReport
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
        private DevExpress.XtraReports.UI.XRTableCell xrTCRiskNum;
        private DevExpress.XtraReports.UI.XRTableCell xrTCRiskTitle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCProbability;
        private DevExpress.XtraReports.UI.XRTableCell xrTCContingencyPlan;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell21;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell43;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell22;
        private DevExpress.XtraReports.UI.XRControlStyle xrEvenControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle xrOddControlStyle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCImpact;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTCMitigationPlan;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrAssignedTo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
    }
}
