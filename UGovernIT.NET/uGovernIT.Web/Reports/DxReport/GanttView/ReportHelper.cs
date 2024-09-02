using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Drawing;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using System.Collections.ObjectModel;
using System.Collections;
using System.Net.Mime;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraPrinting.NativeBricks;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{

    public delegate void CustomizeColumnsCollectionEventHandler(object source, ColumnsCreationEventArgs e);
    public delegate void CustomizeColumnEventHandler(object source, ControlCustomizationEventArgs e);

    public class ReportGenerationHelper
    {
        public string companyLogo = string.Empty;
        public string homePageUrl = string.Empty;
        XRControlStyle xrOddControlStyle = new XRControlStyle();
        XRControlStyle xrEvenControlStyle = new XRControlStyle();
        XtraReport report;
        const int initialGroupOffset = 0;
        const int subGroupOffset = 10;
        const int bandHeight = 20;

        string Title = string.Empty;
        float Fontsize = 0.0F;
        const bool shouldRepeatGroupHeadersOnEveryPage = false;
        Hashtable detailsInfo = new Hashtable();
        DataTable dataSource = new DataTable();
        public string FileFormat { get; set; }
        public string[] GroupDescriptionFields { get; set; }
        private ReportQueryFormat queryFormat { get; set; }
        public event CustomizeColumnsCollectionEventHandler CustomizeColumnsCollection = delegate { };
        public event CustomizeColumnEventHandler CustomizeColumn = delegate { };

        public XtraReport GenerateReport(ASPxGridView aspxGV, DataTable dt, string title)
        {
            return GenerateReport(aspxGV, dt, title, 8F, "pdf");
        }

        public XtraReport GenerateReport(ASPxGridView aspxGV, DataTable dt, string title, float fontSize)
        {
            return GenerateReport(aspxGV, dt, title, fontSize, "pdf");
        }

        public XtraReport GenerateReport(ASPxGridView aspxGV, DataTable dt, string title, float fontSize, string fileFormat)
        {
            return GenerateReport(aspxGV, dt, title, fontSize, fileFormat, null, null);
        }

        private System.Drawing.Printing.PaperKind GetReportPageSize(int rowCount)
        {
            System.Drawing.Printing.PaperKind pKind = System.Drawing.Printing.PaperKind.Letter;
            if (rowCount <= 15)
            {
                pKind = System.Drawing.Printing.PaperKind.Letter;
            }
            else if (rowCount <= 25)
            {
                pKind = System.Drawing.Printing.PaperKind.A3;
            }
            else if (rowCount <= 35)
            {
                pKind = System.Drawing.Printing.PaperKind.A2;
            }
            else
            {
                pKind = System.Drawing.Printing.PaperKind.Custom;
            }
            return pKind;
        }

        public XtraReport GenerateReport(ASPxGridView aspxGV, DataTable dt, string title, float fontSize, string fileFormat, string[] groupDescFields, ReportQueryFormat qryFormat)
        {
            this.queryFormat = qryFormat;
            GroupDescriptionFields = groupDescFields;
            FileFormat = fileFormat;
            Title = title;
            Fontsize = fontSize;
            report = new XtraReport();
            report.Landscape = true;
            report.PaperKind = GetReportPageSize(aspxGV.AllColumns.Count);

            // Change to TextExportMode.Text if you don't need number formatting
            report.ExportOptions.Xls.TextExportMode = TextExportMode.Text;
            report.ExportOptions.Xlsx.TextExportMode = TextExportMode.Text;
            report.ExportOptions.Xlsx.ExportHyperlinks = true;
            report.ExportOptions.Xls.ExportHyperlinks = true;
            if (report.PaperKind == System.Drawing.Printing.PaperKind.Custom)
            {
                report.PageSize = new Size(75 * aspxGV.AllColumns.Count, 1160);
            }

            report.Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            xrOddControlStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            xrOddControlStyle.Name = "xrOddControlStyle";
            xrOddControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);

            xrEvenControlStyle.BackColor = System.Drawing.Color.White;
            xrEvenControlStyle.Name = "xrEvenControlStyle";
            xrEvenControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);

            if (dt != null)
                dataSource = dt.Copy();

            InitDataSource(dt);
            InitDetailsAndPageHeaderAndFooter(aspxGV, qryFormat);
            InitSortings(aspxGV);
            InitGroupHeaders(aspxGV);
            InitFilters(aspxGV);
            InitTotalSummaries(aspxGV);

            return report;
        }

        void InitTotalSummaries(ASPxGridView aspxGV)
        {
            if (aspxGV.TotalSummary.Count > 0)
            {
                report.Bands.Add(new ReportFooterBand() { HeightF = bandHeight + 2 });
                XRPanel panel = new XRPanel()
                {
                    Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom,
                    WidthF = report.PageWidth - (report.Margins.Left + report.Margins.Right),
                    HeightF = bandHeight + 2
                };

                report.Bands[BandKind.ReportFooter].Controls.Add(panel);
                foreach (ASPxSummaryItem item in aspxGV.TotalSummary)
                {
                    GridViewColumn col = aspxGV.Columns[item.ShowInColumn == string.Empty ? item.FieldName : item.ShowInColumn];
                    if (col != null)
                    {
                        if (detailsInfo.Contains(col))
                        {
                            XRLabel label = new XRLabel();
                            label.Borders = BorderSide.None;
                            label.LocationF = ((XRTableCell)detailsInfo[col]).LocationF;
                            label.TextAlignment = TextAlignment.MiddleCenter;
                            label.SizeF = ((XRTableCell)detailsInfo[col]).SizeF;
                            label.DataBindings.Add("Text", null, ((GridViewDataColumn)col).FieldName);
                            label.Summary = new XRSummary() { Running = SummaryRunning.Report };

                            string formatString = ((GridViewDataColumn)col).PropertiesEdit.DisplayFormatString;
                            if (formatString.StartsWith("{0:"))
                                label.Summary.FormatString = item.SummaryType.ToString() + "=" + formatString;
                            else
                                label.Summary.FormatString = item.SummaryType.ToString() + "={0:" + formatString + "}";
                            label.Summary.Func = GetSummaryFunc(item.SummaryType);
                            panel.Controls.Add(label);
                        }
                    }
                }
            }
            else
            {
                XRLine line = new XRLine()
                {
                    LocationF = new PointF(0, 0),
                    WidthF = report.PageWidth - (report.Margins.Left + report.Margins.Right),
                    HeightF = 2
                };
                report.Bands.Add(new ReportFooterBand() { HeightF = 2 });
                report.Bands[BandKind.ReportFooter].Controls.Add(line);
            }
        }

        void InitDataSource(DataTable dt)
        {
            report.DataSource = dt;
        }
        void InitGroupHeaders(ASPxGridView aspxGV)
        {
            ReadOnlyCollection<GridViewDataColumn> groupedColumns = aspxGV.GetGroupedColumns();

            int j = 1;
            for (int i = groupedColumns.Count - 1; i >= 0; i--)
            {
                GridViewDataColumn groupedColumn = groupedColumns[i];
                GroupHeaderBand gb = new GroupHeaderBand();
                GroupFooterBand gfb = new GroupFooterBand();
                gfb.HeightF = bandHeight;
                XRPanel pnl = new XRPanel()
                {
                    BackColor = Color.Beige,
                    SizeF = new SizeF(report.PageWidth - (report.Margins.Left + report.Margins.Right), bandHeight), /*Borders = BorderSide.Bottom,*/
                    Borders = BorderSide.Left | BorderSide.Right
                };
                ///for footer Summary
                if (aspxGV.GroupSummary.Count > 0)
                {
                    foreach (ASPxSummaryItem item in aspxGV.GroupSummary)
                    {
                        GridViewColumn col = aspxGV.Columns[item.ShowInColumn == string.Empty ? item.FieldName : item.ShowInColumn];
                        if (col != null)
                        {
                            if (detailsInfo.Contains(col))
                            {
                                XRLabel label = new XRLabel() { Borders = BorderSide.None };
                                label.LocationF = ((XRTableCell)detailsInfo[col]).LocationF;
                                label.TextAlignment = TextAlignment.MiddleCenter;
                                label.SizeF = ((XRTableCell)detailsInfo[col]).SizeF;
                                //label.Text = item.SummaryType.ToString() + " [" + ((GridViewDataColumn)col).FieldName + "]";
                                label.DataBindings.Add("Text", null, ((GridViewDataColumn)col).FieldName);
                                label.Summary = new XRSummary() { Running = SummaryRunning.Group };
                                string formatString = ((GridViewDataColumn)col).PropertiesEdit.DisplayFormatString;
                                if (formatString.StartsWith("{0:"))
                                    label.Summary.FormatString = item.SummaryType.ToString() + "=" + formatString;
                                else
                                    label.Summary.FormatString = item.SummaryType.ToString() + "={0:" + formatString + "}";
                                label.Summary.Func = GetSummaryFunc(item.SummaryType);
                                pnl.Controls.Add(label);
                            }
                        }
                    }
                }

                gb.Height = bandHeight;
                XRLabel l = new XRLabel()
                {
                    Padding = new PaddingInfo(5, 0, 0, 5),
                    Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
                };
                string groupField = string.Empty;
                if (GroupDescriptionFields != null)
                {
                    groupField = "[" + string.Join("][", GroupDescriptionFields) + "]";
                }
                if (aspxGV.Settings.GroupFormat == "{1}")
                {
                    if (!string.IsNullOrEmpty(groupField))
                    {
                        l.Text = "[" + groupedColumn.FieldName + "]   " + groupField;
                    }
                    else
                    {
                        l.Text = "[" + groupedColumn.FieldName + "]";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(groupField))
                    {
                        l.Text = groupedColumn.Caption + ": [" + groupedColumn.FieldName + "]   " + groupField;
                    }
                    else
                    {
                        l.Text = groupedColumn.Caption + ": [" + groupedColumn.FieldName + "]";
                    }
                }
                l.LocationF = new PointF(initialGroupOffset + i * 10, 0);
                l.BackColor = Color.Beige;
                l.TextAlignment = TextAlignment.MiddleLeft;
                l.Borders = BorderSide.None;
                XRTable headerTable = (XRTable)report.Bands[BandKind.PageHeader].FindControl("headerTable", true);
                //if (aspxGV.GroupSummary.Count > 0)
                //{
                //    l.SizeF = new SizeF(headerTable.Rows[0].Cells[0].WidthF, bandHeight);
                //}
                //else
                //{
                l.SizeF = new SizeF(headerTable.WidthF, bandHeight);
                //}
                //pnl.Controls.Add(l);
                gb.Controls.Add(l);
                gfb.Controls.Add(pnl);
                gb.RepeatEveryPage = shouldRepeatGroupHeadersOnEveryPage;
                GroupField gf = new GroupField(groupedColumn.FieldName, groupedColumn.SortOrder == ColumnSortOrder.Ascending ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending);
                gb.GroupFields.Add(gf);
                report.Bands.Add(gb);
                report.Bands.Add(gfb);
                j++;
            }
        }
        void InitSortings(ASPxGridView aspxGV)
        {
            List<GridViewDataColumn> columns = GetVisibleDataColumns(aspxGV);
            ReadOnlyCollection<GridViewDataColumn> groupedColumns = aspxGV.GetGroupedColumns();
            for (int i = 0; i < columns.Count; i++)
            {
                if (!groupedColumns.Contains(columns[i]))
                {
                    if (columns[i].SortOrder != ColumnSortOrder.None)
                        ((DetailBand)report.Bands[BandKind.Detail]).SortFields.Add(new GroupField(columns[i].FieldName, columns[i].SortOrder == ColumnSortOrder.Ascending ? XRColumnSortOrder.Ascending : XRColumnSortOrder.Descending));
                }
            }
        }
        void InitFilters(ASPxGridView aspxGV)
        {
            report.FilterString = aspxGV.FilterExpression;
        }
        void InitDetailsAndPageHeaderAndFooter(ASPxGridView aspxGV, ReportQueryFormat qryFormat)
        {

            ReadOnlyCollection<GridViewDataColumn> groupedColumns = aspxGV.GetGroupedColumns();
            report.Margins.Left = 20; report.Margins.Right = 20;
            report.Margins.Top = 20; report.Margins.Bottom = 20;

            int pagewidth = (report.PageWidth - (report.Margins.Left + report.Margins.Right));// -groupedColumns.Count * subGroupOffset;
            List<ColumnInfo> columns = GetColumnsInfo(aspxGV, pagewidth);
            CustomizeColumnsCollection(report, new ColumnsCreationEventArgs(pagewidth) { ColumnsInfo = columns });
            report.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] { this.xrEvenControlStyle, this.xrOddControlStyle });

            report.Bands.Add(new DetailBand() { HeightF = bandHeight });
            report.Bands.Add(new PageHeaderBand() { HeightF = bandHeight + 60 });
            report.Bands.Add(new PageFooterBand() { HeightF = bandHeight });

            XRTable headerTable = new XRTable() { Name = "headerTable" };
            headerTable.TextAlignment = TextAlignment.MiddleJustify;
            XRTableRow row = new XRTableRow();
            XRTableRow row1 = new XRTableRow();
            //List<XRLabel> lbList = new List<XRLabel>();
            row.Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            XRTable detailTable = new XRTable();
            detailTable.TextAlignment = TextAlignment.MiddleJustify;
            XRTableRow row2 = new XRTableRow();

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].IsVisible)
                {


                    if (!groupedColumns.Contains(columns[i].GridViewColumn))
                    {

                        XRTableCell cell = new XRTableCell();
                        cell.Padding = new PaddingInfo(5, 0, 0, 5);
                        if (columns.Count - 1 != i)
                        {
                            cell.Borders = BorderSide.Bottom | BorderSide.Right;
                        }
                        else
                        {
                            cell.Borders = BorderSide.Bottom;
                        }

                        cell.BorderWidth = 1;
                        cell.BorderColor = Color.FromArgb(217, 218, 224);

                        cell.Width = columns[i].ColumnWidth;
                        cell.Text = columns[i].GridViewColumn.Caption;
                        row.Cells.Add(cell);

                        if (columns[i].GridViewColumn.ParentBand != null)
                        {
                            XRTableCell lb = new XRTableCell();
                            lb.Text = columns[i].GridViewColumn.ParentBand.Caption;
                            lb.Name = columns[i].GridViewColumn.ParentBand.Caption;
                            //lb.LocationF = new PointF(cell.LocationF.X, 1);
                            lb.WidthF = cell.WidthF * columns[i].GridViewColumn.ParentBand.Columns.Count;
                            lb.Borders = BorderSide.None;
                            lb.Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            lb.TextAlignment = TextAlignment.MiddleCenter;
                            //if (!row1.Cells.Contains(lb))    lb.WidthF = cell.WidthF
                            if (row1.Cells[columns[i].GridViewColumn.ParentBand.Caption] == null)
                            {
                                row1.Cells.Add(lb);
                            }

                        }
                        else
                        {
                            XRTableCell lb = new XRTableCell();
                            lb.Borders = BorderSide.None;
                            lb.Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            lb.WidthF = cell.WidthF;
                            lb.Name = columns[i].GridViewColumn.FieldName;
                            row1.Cells.Add(lb);

                        }

                        XRTableCell cell2 = new XRTableCell();
                        cell2.Name = columns[i].GridViewColumn.FieldName;// "tableCell_" + i;
                        cell2.Padding = new PaddingInfo(5, 0, 0, 5);
                        cell2.Width = columns[i].ColumnWidth;
                        if (columns.Count - 1 != i)
                        {
                            cell2.Borders = BorderSide.Right;
                        }

                        cell2.BorderWidth = 1;
                        cell2.BorderColor = Color.FromArgb(217, 218, 224);
                        cell2.BeforePrint += cell2_BeforePrint;
                        ControlCustomizationEventArgs cc = new ControlCustomizationEventArgs() { FieldName = columns[i].FieldName, IsModified = false, Owner = cell2 };
                        CustomizeColumn(report, cc);


                        if (cc.IsModified == false)
                        {
                            //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                            // if (columns[i].GridViewColumn.PropertiesEdit.DisplayFormatString == "picture" && FileFormat == "pdf")
                            if (columns[i].GridViewColumn.PropertiesEdit.DisplayFormatString == "picture" && (FileFormat == "pdf" || FileFormat == "xls"))
                                //
                            {
                                XRPictureBox pb = new XRPictureBox();
                                pb.DataBindings.Add("ImageUrl", null, columns[i].FieldName);
                                pb.LocationF = new PointF(10, 10);
                                pb.SizeF = new SizeF(24, 24);
                                pb.Borders = BorderSide.None;
                                pb.Name = "pictureBox_" + cell.Name;
                                cell2.Controls.Add(pb);
                            }
                            //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                            //else if (columns[i].GridViewColumn.PropertiesEdit.DisplayFormatString == "picture" && FileFormat == "xls")
                            //{
                            //    cell2.DataBindings.Add("Text", null, columns[i].FieldName);
                            //}
                            //
                            else if (columns[i].GridViewColumn.PropertiesEdit.DisplayFormatString == "Yes;No")
                            {
                                cell2.DataBindings.Add("Text", null, columns[i].FieldName);
                                cell2.Tag = "Yes;No";
                            }
                            else
                            {
                                cell2.DataBindings.Add("Text", null, columns[i].FieldName, columns[i].GridViewColumn.PropertiesEdit.DisplayFormatString);
                            }
                        }


                        detailsInfo.Add(columns[i].GridViewColumn, cell2);

                        if (columns[i].GridViewColumn.CellStyle.HorizontalAlign == HorizontalAlign.Left)
                        {
                            cell.TextAlignment = TextAlignment.MiddleLeft;
                            cell2.TextAlignment = TextAlignment.MiddleLeft;
                        }
                        else if (columns[i].GridViewColumn.CellStyle.HorizontalAlign == HorizontalAlign.Right)
                        {
                            cell.TextAlignment = TextAlignment.MiddleRight;
                            cell2.TextAlignment = TextAlignment.MiddleRight;
                        }
                        else if (columns[i].GridViewColumn.CellStyle.HorizontalAlign == HorizontalAlign.Center)
                        {
                            cell.TextAlignment = TextAlignment.MiddleCenter;
                            cell2.TextAlignment = TextAlignment.MiddleCenter;
                        }
                        else if (columns[i].GridViewColumn.CellStyle.HorizontalAlign == HorizontalAlign.Justify)
                        {
                            cell.TextAlignment = TextAlignment.MiddleJustify;
                            cell2.TextAlignment = TextAlignment.MiddleJustify;
                        }

                        row2.Cells.Add(cell2);
                    }
                }
            }

            // Show two header row if grid contains bandcolumns.
            List<GridViewColumn> gvBandColumn = aspxGV.AllColumns.Where(m => m as GridViewBandColumn != null).ToList();
            if (gvBandColumn.Count > 0)
            {
                headerTable.Rows.AddRange(new XRControl[] { row1, row });
            }
            else
            {
                headerTable.Rows.Add(row);
            }



            headerTable.Width = pagewidth - 2;
            headerTable.LocationF = new PointF(1, 1);
            headerTable.BackColor = Color.LightGray;

            detailTable.Rows.Add(row2);
            detailTable.Width = pagewidth - 2;
            detailTable.LocationF = new PointF(1, 0);
            detailTable.BackColor = Color.White;
            detailTable.StylePriority.UseBackColor = true;
            detailTable.StylePriority.UseBorderColor = false;
            detailTable.StylePriority.UseBorders = false;
            detailTable.StylePriority.UseFont = false;
            detailTable.StylePriority.UsePadding = true;
            detailTable.StylePriority.UseTextAlignment = false;
            detailTable.OddStyleName = "xrOddControlStyle";
            detailTable.EvenStyleName = "xrEvenControlStyle";

            XRPanel pnlheader = new XRPanel() { HeightF = bandHeight + 1, LocationF = new PointF(0, 59), Borders = BorderSide.Left | BorderSide.Right | BorderSide.Top, WidthF = pagewidth };
            //pnlheader.Controls.AddRange(lbList.ToArray());
            pnlheader.Controls.Add(headerTable);
            XRPanel pnldetails = new XRPanel() { HeightF = bandHeight, Borders = BorderSide.Left | BorderSide.Right, WidthF = pagewidth };
            pnldetails.Controls.Add(detailTable);

            XRLabel label = new XRLabel();
            label.Text = qryFormat != null && !string.IsNullOrEmpty(qryFormat.Header) ? qryFormat.Header : Title;
            label.LocationF = new PointF(pagewidth / 3, 10);
            label.WidthF = pagewidth / 3;
            label.TextAlignment = TextAlignment.MiddleCenter;
            label.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            XRLabel labelAdditionalInfo = new XRLabel();
            labelAdditionalInfo.Text = qryFormat != null && !string.IsNullOrEmpty(qryFormat.AdditionalInfo) ? qryFormat.AdditionalInfo : string.Empty;
            labelAdditionalInfo.LocationF = new PointF(pagewidth - (pagewidth / 3), 10);
            labelAdditionalInfo.WidthF = pagewidth / 3;
            labelAdditionalInfo.TextAlignment = TextAlignment.MiddleRight;
            XRTable xrTBLegend = new XRTable() { Name = "Legend" };
            if (qryFormat != null && qryFormat.Legend != null)
            {
                xrTBLegend.LocationF = new PointF(pagewidth - (pagewidth / 3), 10);
                //xrTBLegend.WidthF = pagewidth / 3;
                xrTBLegend.TextAlignment = TextAlignment.MiddleLeft;


              
                XRTableRow rowLegend = new XRTableRow();

                //List<XRLabel> lbList = new List<XRLabel>();
                rowLegend.Font = new System.Drawing.Font("Verdana", Fontsize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                foreach (string str in qryFormat.Legend.Keys.ToList())
                {
                    XRTableCell xrText = new XRTableCell();
                  
                    xrText.Text = string.Format("  {0}",str);
                    XRTableCell xrColor = new XRTableCell();
                    xrColor.WidthF = 25.0F;
                    xrColor.BackColor = System.Drawing.ColorTranslator.FromHtml(qryFormat.Legend[str]);
                    rowLegend.Cells.Add(xrColor);
                    rowLegend.Cells.Add(xrText);
                }

                xrTBLegend.Rows.Add(rowLegend);
            }

            ///Footer Control Add
            XRPageInfo pageinfo = new XRPageInfo();
            pageinfo.Format = "Page {0} of {1}";
            pageinfo.TextAlignment = TextAlignment.MiddleCenter;
            pageinfo.LocationF = new PointF(0, 10);
            pageinfo.WidthF = pagewidth;
            if (qryFormat != null && (!string.IsNullOrEmpty(qryFormat.Footer) || !string.IsNullOrEmpty(qryFormat.AdditionalFooterInfo) || qryFormat.ShowDateInFooter))
            {
                pageinfo.TextAlignment = TextAlignment.MiddleRight;
                pageinfo.LocationF = new PointF((pagewidth - pagewidth / 3), 0);
                pageinfo.WidthF = pagewidth / 3;
            }

            XRLabel footerLLabel = new XRLabel();
            footerLLabel.Text = qryFormat != null && !string.IsNullOrEmpty(qryFormat.Footer) ? qryFormat.Footer : string.Empty;
            footerLLabel.LocationF = new PointF(0, 10);
            footerLLabel.WidthF = pagewidth / 3;
            footerLLabel.TextAlignment = TextAlignment.MiddleLeft;



            XRLabel footerAdditionalInfo = new XRLabel();
            string additionalFooterInfo = string.Empty;
            if (qryFormat != null)
            {
                if (qryFormat.ShowDateInFooter)
                {
                    additionalFooterInfo = string.Format("{0}  {1}", DateTime.Now.ToString("MMM-dd-yyyy"), qryFormat.AdditionalFooterInfo);
                }
                else
                {
                    additionalFooterInfo = qryFormat.AdditionalFooterInfo;
                }
            }
            footerAdditionalInfo.Text = additionalFooterInfo;
            footerAdditionalInfo.LocationF = new PointF(pagewidth / 3, 10);
            footerAdditionalInfo.WidthF = pagewidth / 3;
            footerAdditionalInfo.TextAlignment = TextAlignment.MiddleCenter;



            XRPictureBox logo = new XRPictureBox();
            logo.ImageUrl = companyLogo;
            logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            logo.ImageAlignment = ImageAlignment.MiddleLeft;
            logo.WidthF = pagewidth / 3;
            logo.HeightF = 55;


            if (qryFormat != null && (!string.IsNullOrEmpty(qryFormat.Footer) || !string.IsNullOrEmpty(additionalFooterInfo)))
            {
                report.Bands[BandKind.PageFooter].Controls.AddRange(new XRControl[] { footerLLabel, footerAdditionalInfo, pageinfo });
            }
            else
            {
                report.Bands[BandKind.PageFooter].Controls.AddRange(new XRControl[] { pageinfo });
            }


            if (qryFormat != null)
            {
                if (qryFormat.Legend != null && qryFormat.Legend.Count > 0)
                {
                    report.Bands[BandKind.PageHeader].Controls.AddRange(new XRControl[] { pnlheader, label, logo, xrTBLegend });

                }
                else
                {
                    report.Bands[BandKind.PageHeader].Controls.AddRange(new XRControl[] { pnlheader, label, logo, labelAdditionalInfo });
                }
            }
            else
            {
                report.Bands[BandKind.PageHeader].Controls.AddRange(new XRControl[] { pnlheader, label });
            }

            report.Bands[BandKind.Detail].Controls.Add(pnldetails);
        }

        void cell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            if (cell.Name == "Ticket Id")
            {
                //  XRLabel xr = new XRLabel();
                //  xr.Text = cell.Text;
                string moduleName = string.Empty;
                string ticketID = cell.Text;
                if (!string.IsNullOrEmpty(ticketID) && ticketID.Contains('-'))
                {
                    moduleName = ticketID.Split('-')[0];
                }
                if (!string.IsNullOrEmpty(homePageUrl))
                {
                    cell.NavigateUrl = homePageUrl + "?TicketId=" + ticketID + "&ModuleName=" + moduleName;
                    cell.Target = "_blank";
                }
            }

            if (this.queryFormat != null && this.queryFormat.Legend != null)
            {
                if (!string.IsNullOrEmpty(cell.Text) && cell.Text.Contains(Constants.Separator))
                {
                    string[] vals = cell.Text.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries);
                    if (vals.Length > 1)
                    {
                        cell.Text = vals[1];
                        string colorCode = "#FFFFFF";
                        colorCode = this.queryFormat.Legend[vals[0]];
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml(colorCode);
                    }
                }
                else
                {
                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }

            }

            if (cell.Text.Contains("<br>"))
            {
                cell.Text = cell.Text.Replace("<br>", Environment.NewLine);
                cell.Multiline = true;
            }

            if (cell.Text.Contains("<br/>"))
            {
                cell.Text = cell.Text.Replace("<br/>", Environment.NewLine);
                cell.Multiline = true;
            }



            if (cell.Tag.ToString() == "Yes;No")
            {
                cell.Text = (cell.Text == "1" ? "Yes" : "No");
            }
            if (cell.Text == "Green" || cell.Text == "Yellow" || cell.Text == "Red" || cell.Text.Contains("█"))
            {
                cell.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                switch (cell.Text.Trim())
                {
                    case "Green":
                        cell.ForeColor = Color.Green;
                        break;
                    case "Yellow":
                        cell.ForeColor = Color.Yellow;
                        break;
                    case "Red":
                        cell.ForeColor = Color.Red;
                        break;
                    case "████████████████":
                        cell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        cell.WordWrap = false;
                        cell.Padding = new PaddingInfo(0, 0, 0, 0);
                        //cell.Text = "███████████████";
                        cell.ForeColor = Color.Gray;

                        break;

                    case "███████":
                        cell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        cell.WordWrap = false;
                        cell.Padding = new PaddingInfo(0, 0, 0, 0);
                        cell.TextAlignment = TextAlignment.MiddleRight;
                        cell.ForeColor = Color.Gray;

                        break;

                    case "██████":
                        cell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        cell.WordWrap = false;
                        cell.Padding = new PaddingInfo(0, 0, 0, 0);
                        cell.TextAlignment = TextAlignment.MiddleLeft;
                        cell.ForeColor = Color.Gray;

                        break;
                    default:
                        break;
                }
            }
        }

        private List<ColumnInfo> GetColumnsInfo(ASPxGridView aspxGV, int pagewidth)
        {
            List<ColumnInfo> columns = new List<ColumnInfo>();
            List<GridViewDataColumn> visibleColumns = GetVisibleDataColumns(aspxGV);
            //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
            int width = 0;
            //
            foreach (GridViewDataColumn dataColumn in visibleColumns)
            {
                //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                width = UGITUtility.StringToInt(dataColumn.Width.Value);
                if (width <= 0)
                    width = ((int)pagewidth / visibleColumns.Count);

                if (dataColumn is GridViewDataDateColumn)
                    width = 50;
                else if (dataColumn is GridViewDataSpinEditColumn)
                    width = 40;
                else if (dataColumn.FieldName == "Project ID" || dataColumn.FieldName == DatabaseObjects.Columns.ProjectID || dataColumn.FieldName == DatabaseObjects.Columns.TicketId || dataColumn.FieldName == "Ticket Id")
                    width = 60;
                //




                ColumnInfo column = new ColumnInfo(dataColumn) { ColumnCaption = string.IsNullOrEmpty(dataColumn.Caption) ? dataColumn.FieldName : dataColumn.Caption, ColumnWidth = ((int)pagewidth / visibleColumns.Count), FieldName = dataColumn.FieldName, IsVisible = true };
                columns.Add(column);
            }
            return columns;
        }

        List<GridViewDataColumn> GetVisibleDataColumns(ASPxGridView aspxGV)
        {
            List<GridViewDataColumn> columns = new List<GridViewDataColumn>();
            foreach (GridViewColumn column in aspxGV.VisibleColumns)
            {
                if (column is GridViewDataColumn)
                {
                    if (column.ParentBand != null)
                    {
                        ((GridViewDataColumn)column).FieldName = column.Name + "-" + column.ParentBand.Name;
                    }
                    columns.Add(column as GridViewDataColumn);
                }
                else if (column is GridViewBandColumn)
                {

                }
            }
            return columns;
        }

        private SummaryFunc GetSummaryFunc(SummaryItemType summaryItemType)
        {

            switch (summaryItemType)
            {
                case SummaryItemType.Average:
                    return SummaryFunc.Avg;
                case SummaryItemType.Count:
                    return SummaryFunc.Count;
                case SummaryItemType.Max:
                    return SummaryFunc.Max;
                case SummaryItemType.Min:
                    return SummaryFunc.Min;
                case SummaryItemType.Sum:
                    return SummaryFunc.Sum;
                default:
                    return SummaryFunc.Custom;
            }
        }
        public void WriteXlsToResponse(HttpResponse Response, string fileName, string type)
        {
            report.CreateDocument(true);
            using (MemoryStream ms = new MemoryStream())
            {
                report.ExportToXls(ms);
                ms.Seek(0, SeekOrigin.Begin);
                WriteResponse(Response, ms.ToArray(), type, fileName, "xls");
            }
        }
        public static void WriteXlsToResponse(HttpResponse Response, string fileName, string type, XtraReport report)
        {
            report.CreateDocument(true);
            using (MemoryStream ms = new MemoryStream())
            {
                report.ExportToXls(ms);
                ms.Seek(0, SeekOrigin.Begin);
                WriteResponse(Response, ms.ToArray(), type, fileName, "xls");
            }
        }
        public void WritePdfToResponse(HttpResponse Response, string fileName, string type)
        {
            report.CreateDocument(false);
            using (MemoryStream ms = new MemoryStream())
            {
                report.ExportToPdf(ms);
                ms.Seek(0, SeekOrigin.Begin);
                WriteResponse(Response, ms.ToArray(), type, fileName);
            }
        }
        public static void WritePdfToResponse(HttpResponse Response, string fileName, string type, XtraReport report)
        {
            report.CreateDocument(false);
            using (MemoryStream ms = new MemoryStream())
            {
                report.ExportToPdf(ms);
                ms.Seek(0, SeekOrigin.Begin);
                WriteResponse(Response, ms.ToArray(), type, fileName);
            }
        }

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type, string fileName)
        {
            WriteResponse(response, filearray, type, fileName, "pdf");
        }

        public static void WriteResponse(HttpResponse response, byte[] filearray, string type, string fileName, string fileType)
        {
            response.ClearContent();
            response.Buffer = true;
            response.Cache.SetCacheability(HttpCacheability.Private);
            switch (fileType)
            {
                case "xls":
                    response.ContentType = "application/vnd.ms-excel";
                    break;
                case "pdf":
                    response.ContentType = "application/pdf";
                    break;
                default:
                    response.ContentType = "application/pdf";
                    break;
            }

            ContentDisposition contentDisposition = new ContentDisposition();
            contentDisposition.FileName = fileName;
            contentDisposition.DispositionType = type;
            response.AddHeader("Content-Disposition", contentDisposition.ToString());
            response.BinaryWrite(filearray);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            try
            {
                response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }

        }
    }
    public class ControlCustomizationEventArgs : EventArgs
    {
        XRControl owner;

        public XRControl Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        bool isModified;

        public bool IsModified
        {
            get { return isModified; }
            set { isModified = value; }
        }
        string fieldName;

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

    }
    public class ColumnsCreationEventArgs : EventArgs
    {
        int pageWidth;
        public int PageWidth
        {
            get { return pageWidth; }
        }
        public ColumnsCreationEventArgs(int pageWidth)
        {
            this.pageWidth = pageWidth;
        }
        List<ColumnInfo> columnsInfo;

        public List<ColumnInfo> ColumnsInfo
        {
            get { return columnsInfo; }
            set { columnsInfo = value; }
        }
    }
    public class ColumnInfo
    {
        public ColumnInfo(GridViewDataColumn gridViewColumn)
        {
            this.gridViewColumn = gridViewColumn;
        }
        GridViewDataColumn gridViewColumn;

        public GridViewDataColumn GridViewColumn
        {
            get { return gridViewColumn; }
        }


        string columnCaption;
        public string ColumnCaption
        {
            get { return columnCaption; }
            set { columnCaption = value; }
        }
        string fieldName;

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        int columnWidth;

        public int ColumnWidth
        {
            get { return columnWidth; }
            set { columnWidth = value; }
        }
        bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
    }

    public class DynamicReportBuilderHelperGantView
    {
        const int ReportOffset = 10;
        List<DataSourceDefinitionGantView> dsd;
        TSKProjectReportEntity prEntity;
        NPRProjectReportEntity nprprEntity;
        public void GenerateReport(XtraReport r, TSKProjectReportEntity prEntity)
        {
            this.prEntity = prEntity;
            r.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            r.Margins = new System.Drawing.Printing.Margins(3, 6, 4, 33);
            r.Landscape = true;
            r.PaperKind = System.Drawing.Printing.PaperKind.A3;
            r.Version = "12.2";
            r.DataSource = prEntity.Projects;
            dsd = GenerateDataSourceDefinition(prEntity);
            InitBands(r);
            InitDetailsBasedOnXRTableCell(r, dsd);
        }

        public void NPRGenerateReport(XtraReport r, NPRProjectReportEntity prEntity)
        {
            this.nprprEntity = prEntity;
            r.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            r.Margins = new System.Drawing.Printing.Margins(3, 6, 4, 33);
            r.Landscape = true;
            r.PaperKind = System.Drawing.Printing.PaperKind.A3;
            r.Version = "12.2";
            r.DataSource = prEntity.Projects;
            dsd = GenerateDataSourceDefinition(prEntity);
            InitBands(r);
            InitDetailsBasedOnXRTableCell(r, dsd);
        }
        void cell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string ticketId = ((XRLabel)sender).Text;
            if (string.IsNullOrEmpty(ticketId))
            {
                return;
            }

            foreach (XRTableCell xrTC in ((XRTableRow)((XRLabel)sender).Parent.Parent).Cells)
            {
                if (xrTC.Name == "ShowAccomplishment")
                {
                    //Accomplishment Data
                    if (prEntity.Accomplishment != null && prEntity.Accomplishment.Rows.Count > 0)
                    {
                        var rows = prEntity.Accomplishment.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId), DatabaseObjects.Columns.AccomplishmentDate + " DESC");
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            DataTable dtAccomplishment = rows.CopyToDataTable();
                            xrTC.Multiline = true;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                DateTime dtAccompDate;

                                if (DateTime.TryParse(Convert.ToString(row[DatabaseObjects.Columns.AccomplishmentDate]), out dtAccompDate))
                                {
                                    sb.AppendLine(string.Format("<b>{0}:</b> {1}", string.Format("{0:MMM-dd-yyyy}", Convert.ToDateTime(row[DatabaseObjects.Columns.AccomplishmentDate])),
                                                                        Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("{0}", Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }

                                if (prEntity.ShowAccomplishmentDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.ProjectNote])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                        //((XRRichText)xrTC.Controls[0]).SizeF = xrTC.SizeF;
                    }

                }
                else if (xrTC.Name == "ShowPlan")
                {
                    ///Show Immediate Planned Data
                    if (prEntity.ImmediatePlans != null && prEntity.ImmediatePlans.Rows.Count > 0)
                    {
                        xrTC.Multiline = true;
                        var rows = prEntity.ImmediatePlans.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId), DatabaseObjects.Columns.UGITEndDate);
                        if (rows != null && rows.Count() > 0)
                        {

                            DataTable dtPlannedItem = rows.CopyToDataTable();
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                DateTime dtEndDate;

                                if (DateTime.TryParse(Convert.ToString(row[DatabaseObjects.Columns.UGITEndDate]), out dtEndDate))
                                {
                                    sb.AppendLine(string.Format("<b>{0}:</b> {1}", string.Format("{0:MMM-dd-yyyy}", dtEndDate),
                                                    Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                else
                                {
                                    sb.AppendLine(string.Format("{0}", Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                }
                                if (prEntity.ShowPlanDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.ProjectNote])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowIssues")
                {
                    xrTC.Multiline = true;
                    ///Show Issues
                    if (prEntity.Issues != null && prEntity.Issues.Rows.Count > 0)
                    {

                        var rows = prEntity.Issues.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId), DatabaseObjects.Columns.ItemOrder);
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                sb.AppendLine(string.Format("<b>{0}.</b> {1}", ++i, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                if (prEntity.ShowIssuesDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.Body])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowRisk")
                {
                    xrTC.Multiline = true;
                    ///Show Issues
                    if (prEntity.Risks != null && prEntity.Risks.Rows.Count > 0)
                    {

                        var rows = prEntity.Risks.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));
                        if (rows != null && rows.Count() > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            sb.AppendLine("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/>");
                            foreach (DataRow row in rows)
                            {
                                sb.AppendLine(string.Format("<b>{0}.</b> {1}", ++i, Convert.ToString(row[DatabaseObjects.Columns.Title])));
                                if (prEntity.ShowRiskDesc)
                                {
                                    sb.AppendLine(string.Format("<br/>{0}", Convert.ToString(row[DatabaseObjects.Columns.UGITDescription])));
                                }
                                sb.AppendLine("<br/><br/>");
                            }
                            sb.AppendLine("</span>");
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                        else
                        {
                            ((XRRichText)xrTC.Controls[0]).Html = string.Empty;
                        }
                    }
                }
                else if (xrTC.Name == "ShowProStatus")
                {
                    if (xrTC.Controls.Count > 0)
                    {
                        if (prEntity.ShowLatestOnly)
                        {
                            DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                            ((XRRichText)xrTC.Controls[0]).Html = Convert.ToString(dr[DatabaseObjects.Columns.ProjectSummaryNote]);
                        }
                        else
                        {
                            var rows = prEntity.ExecutiveHistory.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                            StringBuilder sb = new StringBuilder();
                            if (rows != null && rows.Count() > 0)
                            {
                                int i = 0;
                                foreach (DataRow row in rows)
                                {
                                    sb.AppendLine(string.Format(" {0}. {1}", ++i, Convert.ToString(row["Data"])));
                                    sb.AppendLine();
                                }
                            }
                            ((XRRichText)xrTC.Controls[0]).Html = sb.ToString();
                        }
                    }
                    else
                    {
                        if (prEntity.ShowLatestOnly)
                        {
                            DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                            xrTC.Text = GetPlainTextFromHtml(Convert.ToString(dr[DatabaseObjects.Columns.ProjectSummaryNote]));
                        }
                        else
                        {
                            xrTC.Multiline = true;
                            var rows = prEntity.ExecutiveHistory.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, ticketId));
                            StringBuilder sb = new StringBuilder();
                            if (rows != null && rows.Count() > 0)
                            {
                                int i = 0;
                                foreach (DataRow row in rows)
                                {
                                    sb.AppendLine(string.Format("{0}. {1}", ++i, Convert.ToString(row["Data"])));
                                    sb.AppendLine();
                                }
                            }
                            xrTC.Text = GetPlainTextFromHtml(sb.ToString());
                        }
                    }
                }
                else if (xrTC.Name == "Iss")
                {
                    ///Show Monitors
                    if (prEntity.MonitorState != null && prEntity.MonitorState.Rows.Count > 0)
                    {
                        var rows = prEntity.MonitorState.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketPMMIdLookup, ticketId));

                        if (rows != null && rows.Count() > 0)
                        {


                            XRTableCell ctrTC = xrTC;
                            while (ctrTC != null && Convert.ToString(ctrTC.Tag) == "Monitors")
                            {
                                if (ctrTC.Name == "Iss")
                                {
                                    var criticalIssue = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == "Critical Issues");
                                    if (criticalIssue != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]);
                                        else
                                        {
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Green.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Yellow.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(criticalIssue[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Red.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        // ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = string.Empty;
                                    }
                                }
                                else if (ctrTC.Name == "Sco")
                                {
                                    var scope = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == "Within Scope");
                                    if (scope != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]);
                                        else
                                        {
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Green.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Yellow.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(scope[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Red.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        //ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = string.Empty;
                                    }
                                }
                                else if (ctrTC.Name == "$$$")
                                {
                                    var budget = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == "On Budget");
                                    if (budget != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]);
                                        else
                                        {
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Green.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Yellow.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(budget[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Red.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        // ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = string.Empty;
                                    }
                                }
                                else if (ctrTC.Name == "Tme")
                                {
                                    var time = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == "On Time");
                                    if (time != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]);
                                        else
                                        {
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Green.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Yellow.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(time[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Red.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        //ctrTC.Controls[0].Visible = false;
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = string.Empty;
                                    }
                                }
                                else if (ctrTC.Name == "Rsk")
                                {
                                    var risk = rows.FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == "Risk Level");
                                    if (risk != null)
                                    {
                                        if (prEntity.ShowTrafficlight)
                                            ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]);
                                        else
                                        {
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Green.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "G";
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Yellow.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "Y";
                                            if (Convert.ToString(risk[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup]).EndsWith("LED_Red.png"))
                                                ((XRLabel)ctrTC.Controls[1]).Text = "R";
                                        }
                                    }
                                    else
                                    {
                                        ((XRPictureBox)ctrTC.Controls[0]).ImageUrl = string.Empty;
                                    }
                                }

                                ctrTC = ctrTC.NextCell;
                            }
                        }
                    }
                }
                else if (xrTC.Name == "ShowPercentComplete")
                {
                    DataRow dr = prEntity.Projects.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.TicketId) == ticketId);
                    if (dr != null)
                    {
                        double pct = 0;
                        double.TryParse(Convert.ToString(dr[DatabaseObjects.Columns.TicketPctComplete]), out pct);
                        pct = Math.Round(pct * 100, 1, MidpointRounding.AwayFromZero); // Round to nearest 0.1
                        xrTC.Text = string.Format("{0}%", pct);
                    }
                }
            }
        }

        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

        private void InitDetailsBasedOnXRTableCell(XtraReport rep, List<DataSourceDefinitionGantView> dsd)
        {
            int colCount = dsd.Count;
            int totalf = 0;
            for (int i = 0; i < dsd.Count; i++)
                totalf += dsd[i].Factor;
            int fWidth = (rep.PageWidth - ReportOffset - (rep.Margins.Left + rep.Margins.Right)) / totalf;
            int incShift = 0;
            List<XRTableCell> headers = new List<XRTableCell>();
            List<XRTableCell> details = new List<XRTableCell>();
            for (int i = 0; i < colCount; i++)
            {
                if (dsd[i].Fieldname == "ShowMonitorState")
                {
                    int width = (fWidth * dsd[i].Factor) / 5;
                    float padding = (width - 20) / 2;

                    XRTableCell xrHeaderTableCellIss = new XRTableCell();
                    xrHeaderTableCellIss.Text = "Iss";
                    xrHeaderTableCellIss.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellIss.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellSco = new XRTableCell();
                    xrHeaderTableCellSco.Text = "Sco";
                    xrHeaderTableCellSco.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellSco.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellBug = new XRTableCell();
                    xrHeaderTableCellBug.Text = "$$$";
                    xrHeaderTableCellBug.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellBug.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellTme = new XRTableCell();
                    xrHeaderTableCellTme.Text = "Tme";
                    xrHeaderTableCellTme.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellTme.SizeF = new SizeF(width, 20);

                    XRTableCell xrHeaderTableCellRsk = new XRTableCell();
                    xrHeaderTableCellRsk.Text = "Rsk";
                    xrHeaderTableCellRsk.TextAlignment = dsd[i].TextAlignment;
                    xrHeaderTableCellRsk.SizeF = new SizeF(width, 20);
                    headers.AddRange(new XRTableCell[] { xrHeaderTableCellIss, xrHeaderTableCellSco, xrHeaderTableCellBug, xrHeaderTableCellTme, xrHeaderTableCellRsk });


                    XRTableCell xrTableCellIss = new XRTableCell();
                    xrTableCellIss.Name = "Iss";
                    xrTableCellIss.TextAlignment = dsd[i].TextAlignment;
                    xrTableCellIss.SizeF = new SizeF(width, 20);
                    xrTableCellIss.Padding = new PaddingInfo(0, 0, 0, 0);
                    XRPictureBox xrPB1 = new XRPictureBox();
                    xrPB1.SizeF = new SizeF(20, 20);
                    xrPB1.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    //xrPB1.TextAlignment = dsd[i].TextAlignment;
                    xrPB1.StylePriority.UseBorders = false;
                    xrPB1.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    XRLabel xrLabeliis = new XRLabel();
                    xrLabeliis.Borders = BorderSide.None;
                    xrLabeliis.TextAlignment = TextAlignment.MiddleCenter;
                    //xrLabeliis.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                    xrLabeliis.SizeF = new SizeF(20, 20);
                    xrLabeliis.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    xrTableCellIss.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB1, xrLabeliis });
                    xrTableCellIss.Tag = "Monitors";


                    XRTableCell xrTableCellSco = new XRTableCell();
                    xrTableCellSco.Name = "Sco";
                    xrTableCellSco.TextAlignment = dsd[i].TextAlignment;
                    xrTableCellSco.SizeF = new SizeF(width, 20);
                    XRPictureBox xrPB2 = new XRPictureBox();
                    xrPB2.SizeF = new SizeF(20, 20);
                    xrPB2.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    // xrPB2.TextAlignment = dsd[i].TextAlignment;
                    xrPB2.StylePriority.UseBorders = false;
                    xrPB2.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    XRLabel xrLabelSco = new XRLabel();
                    xrLabelSco.Borders = BorderSide.None;
                    xrLabelSco.TextAlignment = TextAlignment.MiddleCenter;
                    //xrLabelSco.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                    xrLabelSco.SizeF = new SizeF(20, 20);
                    xrLabelSco.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    xrTableCellSco.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB2, xrLabelSco });
                    xrTableCellSco.Padding = new PaddingInfo(0, 0, 0, 0);
                    xrTableCellSco.Tag = "Monitors";

                    XRTableCell xrTableCellBug = new XRTableCell();
                    xrTableCellBug.Name = "$$$";
                    xrTableCellBug.TextAlignment = dsd[i].TextAlignment;
                    xrTableCellBug.SizeF = new SizeF(width, 20);
                    XRPictureBox xrPB3 = new XRPictureBox();
                    xrPB3.SizeF = new SizeF(20, 20);
                    xrPB3.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    //xrPB3.TextAlignment = dsd[i].TextAlignment;
                    xrPB3.StylePriority.UseBorders = false;
                    xrPB3.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    XRLabel xrLabelDolar = new XRLabel();
                    xrLabelDolar.Borders = BorderSide.None;
                    xrLabelDolar.TextAlignment = TextAlignment.MiddleCenter;
                    //xrLabelDolar.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                    xrLabelDolar.SizeF = new SizeF(20, 20);
                    xrLabelDolar.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    xrTableCellBug.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB3, xrLabelDolar });
                    xrTableCellBug.Padding = new PaddingInfo(0, 0, 0, 0);
                    xrTableCellBug.Tag = "Monitors";

                    XRTableCell xrTableCellTme = new XRTableCell();
                    xrTableCellTme.Name = "Tme";
                    xrTableCellTme.TextAlignment = dsd[i].TextAlignment;

                    xrTableCellTme.SizeF = new SizeF(width, 20);
                    XRPictureBox xrPB4 = new XRPictureBox();
                    xrPB4.SizeF = new SizeF(20, 20);
                    xrPB4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    //xrPB4.TextAlignment = dsd[i].TextAlignment;
                    xrPB4.StylePriority.UseBorders = false;
                    xrPB4.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    XRLabel xrLabelTme = new XRLabel();
                    xrLabelTme.Borders = BorderSide.None;
                    xrLabelTme.TextAlignment = TextAlignment.MiddleCenter;
                    //xrLabelTme.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                    xrLabelTme.SizeF = new SizeF(20, 20);
                    xrLabelTme.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    xrTableCellTme.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB4, xrLabelTme });
                    xrTableCellTme.Padding = new PaddingInfo(0, 0, 0, 0);
                    xrTableCellTme.Tag = "Monitors";

                    XRTableCell xrTableCellRsk = new XRTableCell();
                    xrTableCellRsk.Name = "Rsk";
                    xrTableCellRsk.TextAlignment = dsd[i].TextAlignment;
                    xrTableCellRsk.SizeF = new SizeF(width, 20);
                    xrTableCellRsk.Tag = "Monitors";
                    XRPictureBox xrPB5 = new XRPictureBox();
                    xrPB5.SizeF = new SizeF(20, 20);
                    xrPB5.Borders = DevExpress.XtraPrinting.BorderSide.None;

                    //xrPB5.TextAlignment = dsd[i].TextAlignment;
                    xrPB5.StylePriority.UseBorders = false;
                    xrPB5.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    XRLabel xrLabelRsk = new XRLabel();
                    xrLabelRsk.Borders = BorderSide.None;
                    xrLabelRsk.TextAlignment = TextAlignment.MiddleCenter;
                    //xrLabelRsk.Font = new Font(new FontFamily("verdana"), 8.23f, FontStyle.Bold);
                    xrLabelRsk.SizeF = new SizeF(20, 20);
                    xrLabelRsk.LocationFloat = new DevExpress.Utils.PointFloat(padding, 2F);

                    xrTableCellRsk.Padding = new PaddingInfo(0, 0, 0, 0);
                    xrTableCellRsk.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrPB5, xrLabelRsk });

                    details.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { xrTableCellIss, xrTableCellSco, xrTableCellBug, xrTableCellTme, xrTableCellRsk });
                }
                else
                {
                    XRTableCell xrHeaderTableCell = CreateHeaderTableCell(dsd[i], fWidth, incShift);
                    XRTableCell xrDetailTableCell = CreateDetailTableCell(dsd[i], fWidth, incShift);
                    incShift += fWidth * dsd[i].Factor;
                    headers.Add(xrHeaderTableCell);
                    details.Add(xrDetailTableCell);
                }


            }
            ((XRTableRow)rep.Bands[BandKind.PageHeader].Controls[0].Controls[0]).Cells.AddRange(headers.ToArray());
            ((XRTableRow)rep.Bands[BandKind.Detail].Controls[0].Controls[0]).Cells.AddRange(details.ToArray());

        }

        private static XRLabel CreateLabel(DataSourceDefinitionGantView dsd, int fWidth, int incShift)
        {
            XRLabel labeld = new XRLabel();
            labeld.Location = new Point(incShift, 0);
            labeld.Size = new Size(fWidth * dsd.Factor, 20);
            return labeld;
        }

        private XRTableCell CreateHeaderTableCell(DataSourceDefinitionGantView dsd, int fWidth, int incShift)
        {
            XRTableCell xrTableCell = new XRTableCell();
            xrTableCell.Name = string.Format("xrTC{0}", dsd.Fieldname);
            xrTableCell.Text = dsd.CaptionName;

            if (dsd.Fieldname == "TicketId")
            {
                xrTableCell.Visible = false;
            }
            xrTableCell.TextAlignment = dsd.TextAlignment;
            xrTableCell.SizeF = new SizeF(fWidth * dsd.Factor, 20);
            return xrTableCell;

        }

        private XRTableCell CreateDetailTableCell(DataSourceDefinitionGantView dsd, int fWidth, int incShift)
        {
            XRTableCell xrTableCell = new XRTableCell();
            xrTableCell.Name = dsd.Fieldname;

            if (dsd.Fieldname == "ShowProStatus")
            {
                if (prEntity.ShowPlainText)
                {
                    //xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.ProjectSummaryNote);
                }
                else
                {
                    XRRichText richText = new XRRichText();
                    richText.StylePriority.UseBorders = false;
                    richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                    //richText.Padding = new PaddingInfo(2, 2, 5, 0);
                    richText.DataBindings.Add("Html", null, DatabaseObjects.Columns.ProjectSummaryNote);
                    xrTableCell.Name = dsd.Fieldname;
                    richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                    xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
                    xrTableCell.Tag = "status";
                }
            }
            else if (dsd.Fieldname == "ShowProjectName" || dsd.Fieldname == DatabaseObjects.Columns.Title)
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.Left;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.Html = string.Format("<span style='font-family: Verdana,Arial,sans-serif;font-size: 8pt;'><br/><b>[{0}]:</b> [{1}]<br/></span>", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.Title);

                XRLabel xrLblTicketId = new XRLabel();
                xrLblTicketId.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketId);
                xrLblTicketId.BeforePrint += cell2_BeforePrint;
                xrLblTicketId.SizeF = new SizeF(0, 0);
                xrLblTicketId.Visible = false;
                xrTableCell.Name = "TicketId";
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrLblTicketId, richText });

            }
            else if (dsd.Fieldname == "ShowPriority")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketPriorityLookup);
            }
            else if (dsd.Fieldname == "ShowStatus")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketStatus);
            }
            else if (dsd.Fieldname == "ShowDescription")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDescription);
            }
            else if (dsd.Fieldname == "ShowTargetDate")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDesiredCompletionDate, "{0:MMM-dd-yyyy}");
            }
            else if (dsd.Fieldname == "ShowProjectManagers")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketProjectManager);
            }
            else if (dsd.Fieldname == "ShowProgress")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketStatus);
            }
            else if (dsd.Fieldname == "ShowProjectType")
            {
                xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketRequestTypeLookup);
            }
            else if (dsd.Fieldname == "ShowPercentComplete")
            {
                // xrTableCell.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketPctComplete);
            }
            else if (dsd.Fieldname == "ShowAccomplishment")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowPlan")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowIssues")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }
            else if (dsd.Fieldname == "ShowRisk")
            {
                XRRichText richText = new XRRichText();
                richText.StylePriority.UseBorders = false;
                richText.Borders = DevExpress.XtraPrinting.BorderSide.None;
                richText.SizeF = new SizeF(fWidth * dsd.Factor, 20);
                richText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                xrTableCell.Name = dsd.Fieldname;
                xrTableCell.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { richText });
            }

            else
            {
                xrTableCell.DataBindings.Add("Text", null, dsd.Fieldname);
                xrTableCell.Name = dsd.Fieldname;
            }

            xrTableCell.TextAlignment = dsd.TextAlignment;
            xrTableCell.SizeF = new SizeF(fWidth * dsd.Factor, 20);
            return xrTableCell;
        }

        private static XRRichText CreateRichText(DataSourceDefinitionGantView dsd, int fWidth, int incShift)
        {
            XRRichText richText = new XRRichText();
            richText.Location = new Point(incShift, 0);
            richText.Size = new Size(fWidth * dsd.Factor, 20);
            return richText;
        }

        private List<DataSourceDefinitionGantView> GenerateDataSourceDefinition(object myComplexObject)
        {
            List<DataSourceDefinitionGantView> dsdl = new List<DataSourceDefinitionGantView>();

            if (myComplexObject is NPRProjectReportEntity)
            {
                NPRProjectReportEntity nprEntity = myComplexObject as NPRProjectReportEntity;

                foreach (Reportable field in nprEntity.Fields)
                {
                    DataSourceDefinitionGantView dsd = new DataSourceDefinitionGantView();
                    dsd.CaptionName = string.IsNullOrWhiteSpace(field.AlternateName) ? field.FName : field.AlternateName;
                    dsd.Fieldname = field.FName;
                    dsd.Factor = field.LenFactor == 0 ? 1 : field.LenFactor;
                    dsd.TextAlignment = field.TextAlignment;
                    dsdl.Add(dsd);
                }
            }
            else
            {
                PropertyInfo[] pi = myComplexObject.GetType().GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    Reportable[] r = pi[i].GetCustomAttributes(typeof(Reportable), false) as Reportable[];
                    if (r.Length > 0)
                    {
                        if (Convert.ToBoolean(pi[i].GetValue(myComplexObject, pi[i].GetIndexParameters())) || pi[i].Name == DatabaseObjects.Columns.TicketId)
                        {
                            DataSourceDefinitionGantView dsd = new DataSourceDefinitionGantView();
                            dsd.CaptionName = r[0].AlternateName == null ? pi[i].Name : r[0].AlternateName;
                            dsd.Fieldname = pi[i].Name;
                            dsd.Factor = r[0].LenFactor == 0 ? 1 : r[0].LenFactor;
                            dsd.TextAlignment = r[0].TextAlignment;
                            dsdl.Add(dsd);
                        }
                    }
                }
            }

            return dsdl;
        }

        public void InitBands(XtraReport rep)
        {
            // Create bands
            DetailBand detail = new DetailBand();
            detail.HeightF = 25F;
            detail.Name = "Detail";
            detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;

            XRTableRow xrTableRow1 = new XRTableRow();
            xrTableRow1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xrTableRow1.Name = "xrTableRow1";
            xrTableRow1.StylePriority.UseFont = false;
            //xrTableRow1.Weight = 1D;

            XRTableRow xrTableRow2 = new XRTableRow();
            xrTableRow2.Name = "xrTableRow2";
            xrTableRow2.Tag = "details";
            //xrTableRow2.Weight = 1D;

            XRTable xrTblProHeader = new XRTable();
            xrTblProHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            xrTblProHeader.BorderColor = System.Drawing.Color.Gray;
            xrTblProHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            xrTblProHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            xrTblProHeader.Name = "xrTblProHeader";
            xrTblProHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrTblProHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            xrTableRow1});
            xrTblProHeader.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 25F);
            xrTblProHeader.StylePriority.UseBackColor = false;
            xrTblProHeader.StylePriority.UseBorderColor = false;
            xrTblProHeader.StylePriority.UseBorders = false;
            xrTblProHeader.StylePriority.UsePadding = false;

            XRTable xrTblProjectDetails = new XRTable();
            xrTblProjectDetails.BorderColor = System.Drawing.Color.Gray;
            xrTblProjectDetails.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            xrTblProjectDetails.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            xrTblProjectDetails.Name = "xrTblProjectDetails";
            xrTblProjectDetails.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrTblProjectDetails.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            xrTableRow2});
            xrTblProjectDetails.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 25F);
            xrTblProjectDetails.StylePriority.UseBorderColor = false;
            xrTblProjectDetails.StylePriority.UseBorders = false;
            xrTblProjectDetails.StylePriority.UsePadding = false;

            PageHeaderBand pageHeader = new PageHeaderBand();
            pageHeader.KeepTogether = true;
            ReportFooterBand reportFooter = new ReportFooterBand();
            reportFooter.KeepTogether = true;
            detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            xrTblProjectDetails});

            pageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { xrTblProHeader
            });
            pageHeader.HeightF = 25F;
            pageHeader.Name = "PageHeader";

            detail.Height = 20;
            reportFooter.Height = 380;
            pageHeader.Height = 20;

            XRLabel xrLabel1 = new XRLabel();
            xrLabel1.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 21.95834F);
            xrLabel1.Name = "xrLabel1";
            xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            xrLabel1.SizeF = new System.Drawing.SizeF(rep.PageWidth - ReportOffset, 41.75F);
            xrLabel1.StylePriority.UseFont = false;
            xrLabel1.StylePriority.UseTextAlignment = false;
            xrLabel1.Text = "Project Summary Report";
            xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;

            ReportHeaderBand ReportHeader = new ReportHeaderBand();
            ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            xrLabel1});
            ReportHeader.HeightF = 97.91666F;
            ReportHeader.Name = "ReportHeader";

            // Place the bands onto a report
            rep.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { ReportHeader, detail, pageHeader });
        }
    }

    public class DataSourceDefinitionGantView
    {
        string fieldname;

        public string Fieldname
        {
            get { return fieldname; }
            set { fieldname = value; }
        }
        string captionName;

        public string CaptionName
        {
            get { return captionName; }
            set { captionName = value; }
        }
        int factor;

        public int Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        private TextAlignment textAlignment;
        public TextAlignment TextAlignment
        {
            get { return this.textAlignment; }
            set { this.textAlignment = value; }
        }
    }
}