namespace uGovernIT.DxReport
{
    partial class IssuesReport
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
            this.xrTIssues = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTCIssuesNum = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCIssues = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCPriority = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCICreatedOn = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCIDueDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCIStatus = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTCIssueAssignedTo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrResolution = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrEvenControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrOddControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.xrTIssues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTIssues});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTIssues
            // 
            this.xrTIssues.BackColor = System.Drawing.Color.Empty;
            this.xrTIssues.BorderColor = System.Drawing.Color.Black;
            this.xrTIssues.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTIssues.EvenStyleName = "xrEvenControlStyle";
            this.xrTIssues.ForeColor = System.Drawing.Color.Black;
            this.xrTIssues.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTIssues.Name = "xrTIssues";
            this.xrTIssues.OddStyleName = "xrOddControlStyle";
            this.xrTIssues.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTIssues.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow20});
            this.xrTIssues.SizeF = new System.Drawing.SizeF(1082.996F, 25F);
            this.xrTIssues.StylePriority.UseBorderColor = false;
            this.xrTIssues.StylePriority.UseBorders = false;
            this.xrTIssues.StylePriority.UseForeColor = false;
            this.xrTIssues.StylePriority.UsePadding = false;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTCIssuesNum,
            this.xrTCIssues,
            this.xrTCPriority,
            this.xrTCICreatedOn,
            this.xrTCIDueDate,
            this.xrTCIStatus,
            this.xrTCIssueAssignedTo,
            this.xrResolution});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1D;
            // 
            // xrTCIssuesNum
            // 
            this.xrTCIssuesNum.Name = "xrTCIssuesNum";
            this.xrTCIssuesNum.StylePriority.UseTextAlignment = false;
            this.xrTCIssuesNum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTCIssuesNum.Weight = 0.07544325524765283D;
            // 
            // xrTCIssues
            // 
            this.xrTCIssues.Name = "xrTCIssues";
            this.xrTCIssues.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrTCIssues.StylePriority.UsePadding = false;
            this.xrTCIssues.StylePriority.UseTextAlignment = false;
            this.xrTCIssues.Text = "Issues";
            this.xrTCIssues.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTCIssues.Weight = 0.908113307767756D;
            // 
            // xrTCPriority
            // 
            this.xrTCPriority.Name = "xrTCPriority";
            this.xrTCPriority.StylePriority.UseTextAlignment = false;
            this.xrTCPriority.Text = "Priority";
            this.xrTCPriority.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCPriority.Weight = 0.22120841752918069D;
            // 
            // xrTCICreatedOn
            // 
            this.xrTCICreatedOn.Name = "xrTCICreatedOn";
            this.xrTCICreatedOn.StylePriority.UseTextAlignment = false;
            this.xrTCICreatedOn.Text = "Created On";
            this.xrTCICreatedOn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCICreatedOn.Weight = 0.29164415560204876D;
            // 
            // xrTCIDueDate
            // 
            this.xrTCIDueDate.Multiline = true;
            this.xrTCIDueDate.Name = "xrTCIDueDate";
            this.xrTCIDueDate.StylePriority.UseTextAlignment = false;
            this.xrTCIDueDate.Text = "Due Date";
            this.xrTCIDueDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCIDueDate.Weight = 0.29164415560204876D;
            // 
            // xrTCIStatus
            // 
            this.xrTCIStatus.Name = "xrTCIStatus";
            this.xrTCIStatus.StylePriority.UseTextAlignment = false;
            this.xrTCIStatus.Text = "Status";
            this.xrTCIStatus.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCIStatus.Weight = 0.28582288819719237D;
            // 
            // xrTCIssueAssignedTo
            // 
            this.xrTCIssueAssignedTo.Name = "xrTCIssueAssignedTo";
            this.xrTCIssueAssignedTo.StylePriority.UseTextAlignment = false;
            this.xrTCIssueAssignedTo.Text = "AssignedTo";
            this.xrTCIssueAssignedTo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTCIssueAssignedTo.Weight = 0.29106183600276936D;
            // 
            // xrResolution
            // 
            this.xrResolution.Name = "xrResolution";
            this.xrResolution.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 2, 2, 100F);
            this.xrResolution.StylePriority.UsePadding = false;
            this.xrResolution.StylePriority.UseTextAlignment = false;
            this.xrResolution.Text = "Resolution";
            this.xrResolution.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrResolution.Weight = 0.66116211967968219D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 8.333333F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 10.41667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.PageHeader.HeightF = 41.66667F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable2
            // 
            this.xrTable2.BackColor = System.Drawing.Color.LightGray;
            this.xrTable2.BorderColor = System.Drawing.Color.Black;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.ForeColor = System.Drawing.Color.Black;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.0004882813F, 16.66667F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow19});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1082.996F, 25F);
            this.xrTable2.StylePriority.UseBackColor = false;
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseForeColor = false;
            this.xrTable2.StylePriority.UsePadding = false;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell25,
            this.xrTableCell1,
            this.xrTableCell44,
            this.xrTableCell2,
            this.xrTableCell46,
            this.xrTableCell28,
            this.xrTableCell3});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Weight = 0.075247970962416D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.ForeColor = System.Drawing.Color.Red;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseForeColor = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "Issues";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 0.90576258085002459D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Priority";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.22063445088329087D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseTextAlignment = false;
            this.xrTableCell44.Text = "Date Identified";
            this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell44.Weight = 0.29088923326258886D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Due Date";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.29088923326258886D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.Text = "Status";
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell46.Weight = 0.2850830344640104D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Assigned To";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.29030841341414093D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Resolution";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 0.65945043838279882D;
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
            // IssuesReport
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
            this.Version = "18.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTIssues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTIssues;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow20;
        private DevExpress.XtraReports.UI.XRTableCell xrTCIssuesNum;
        private DevExpress.XtraReports.UI.XRTableCell xrTCIssues;
        private DevExpress.XtraReports.UI.XRTableCell xrTCICreatedOn;
        private DevExpress.XtraReports.UI.XRTableCell xrTCIStatus;
        private DevExpress.XtraReports.UI.XRTableCell xrTCIssueAssignedTo;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow19;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell25;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell44;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell46;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRControlStyle xrEvenControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle xrOddControlStyle;
        private DevExpress.XtraReports.UI.XRTableCell xrTCPriority;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrResolution;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTCIDueDate;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
    }
}
