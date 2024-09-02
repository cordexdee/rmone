using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using uGovernIT.Utility;
using Winnovative.WnvHtmlConvert;

namespace uGovernIT.Helpers
{
    public class ExportReport
    {
        public bool ScriptsEnabled { get; set; }
        public bool ShowHeader { get; set; }
        public bool ShowFooter { get; set; }
        public string HeaderHeading { get; set; }
        public string HeaderSubHeading { get; set; }
        public string FooterHeading { get; set; }
        public int ReportType { get; set; }

        public byte[] GetReport(string url)
        {
            if (ReportType == 1)
            {
                ImgConverter imgConverter = GetImageConverter();
                imgConverter.NavigationTimeout = 3600;
                return imgConverter.GetImageBytesFromUrl(url, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                PdfConverter pdfConverter = GetPdfConverter();
                pdfConverter.NavigationTimeout = 3600;
                return pdfConverter.GetPdfBytesFromUrl(url);
            }
        }

        public byte[] GetReportFromHTML(string html, string baseUrl)
        {
            if (ReportType == 1)
            {
                ImgConverter imgConverter = GetImageConverter();
                imgConverter.NavigationTimeout = 3600;
                return imgConverter.GetImageBytesFromHtmlString(html, System.Drawing.Imaging.ImageFormat.Png, baseUrl);
            }
            else
            {
                PdfConverter pdfConverter = GetPdfConverter();
                return pdfConverter.GetPdfBytesFromHtmlString(html, baseUrl);
            }
        }

        private PdfConverter GetPdfConverter()
        {
            PdfConverter pdfConverter = new PdfConverter();

            pdfConverter.LicenseKey = GetPDFLicenseKey();

            //pdfConverter.AuthenticationOptions.Username = "teklptp028\\Manish Hada";
            //pdfConverter.AuthenticationOptions.Password = "password";

            // set the HTML page width in pixels - the default value is 1024 pixels
            //pdfConverter.PageWidth = int.Parse(textBoxCustomWebPageWidth.Text.Trim());

            //set the PDF page size 
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;

            // set if the generated PDF contains selectable text or an embedded image - default value is true
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = false;

            // Enable Alpha blending for transparent images - default is true
            pdfConverter.AlphaBlendEnabled = true;

            // set the PDF standard used to generate the PDF document - default is Full
            //pdfConverter.PdfStandardSubset = GetPdfStandard("PDF/A");

            // set the PDF document margins
            pdfConverter.PdfDocumentOptions.LeftMargin = 10;
            pdfConverter.PdfDocumentOptions.RightMargin = 10;
            pdfConverter.PdfDocumentOptions.TopMargin = 10;
            pdfConverter.PdfDocumentOptions.BottomMargin = 10;

            // set if the HTTP links are enabled in the generated PDF
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;

            // set if the HTML content is resized if necessary to fit the PDF page width - default is true
            pdfConverter.PdfDocumentOptions.FitWidth = true;

            // set if the PDF page should be automatically resized to the size of the HTML content when FitWidth is false
            //pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;

            // embed the true type fonts in the generated PDF document
            pdfConverter.PdfDocumentOptions.EmbedFonts = true;

            // compress the images in PDF with JPEG to reduce the PDF document size - default is true
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = false;

            // Set compression level of PDF document
            //pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;

            // set if the JavaScript is enabled during conversion 
            //pdfConverter.ScriptsEnabled = true;

            // Set if the converter should try to avoid breaking the images between PDF pages
            pdfConverter.AvoidImageBreak = true;

            // show or hide header and footer
            pdfConverter.PdfDocumentOptions.ShowHeader = ShowHeader;
            pdfConverter.PdfDocumentOptions.ShowFooter = ShowFooter;

            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Landscape;

            StringBuilder headerHTML = new StringBuilder();
            if (string.IsNullOrEmpty(HeaderHeading))
            {
                HeaderHeading = string.Empty;
            }

            headerHTML.AppendFormat("<div style='font-size:20px;text-align:center;'>{0}</div>", HeaderHeading);
            pdfConverter.PdfHeaderOptions.HeaderTextColor = Color.Blue;
            if (!string.IsNullOrEmpty(HeaderSubHeading))
            {
                headerHTML.AppendFormat("<div style='font-size:12px;'>{0}</div>", HeaderSubHeading);
            }

            if (headerHTML != null && !string.IsNullOrEmpty(headerHTML.ToString()))
            {
                HtmlToPdfArea area = new HtmlToPdfArea(headerHTML.ToString(), "");
                pdfConverter.PdfHeaderOptions.AddHtmlToPdfArea(area);
            }

            pdfConverter.PdfHeaderOptions.DrawHeaderLine = true;
            pdfConverter.PdfFooterOptions.FooterText = string.Empty;
            if (!string.IsNullOrEmpty(FooterHeading))
            {
                pdfConverter.PdfFooterOptions.FooterText = FooterHeading;
            }
            pdfConverter.PdfFooterOptions.FooterTextColor = Color.Blue;
            pdfConverter.PdfFooterOptions.DrawFooterLine = true;
            pdfConverter.PdfFooterOptions.PageNumberText = "Page ";
            pdfConverter.PdfFooterOptions.ShowPageNumber = true;

            //pdfConverter.PdfBookmarkOptions.TagNames = new string[] { "h1", "h2" };

            return pdfConverter;
        }

        private ImgConverter GetImageConverter()
        {
            ImgConverter imgConverter = new ImgConverter();

            imgConverter.LicenseKey = GetPDFLicenseKey();

            // set the HTML page width in pixels
            // the default value is 1024 pixels
            //pdfConverter.PageWidth = int.Parse(textBoxCustomWebPageWidth.Text.Trim());

            // set if the generated PDF contains selectable text or an embedded image - default value is true
            // pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = cbGenerateSelectablePdf.Checked;

            //  pdfConverter.PdfBookmarkOptions.TagNames = new string[] { "h1", "h2" };

            return imgConverter;
        }

        private static PdfStandardSubset GetPdfStandard(string standardName)
        {
            switch (standardName)
            {
                case "PDF":
                    return PdfStandardSubset.Full;
                case "PDF/A":
                    return PdfStandardSubset.Pdf_A_1b;
                case "PDF/X":
                    return PdfStandardSubset.Pdf_X_1a;
                case "PDF/SiqQA":
                    return PdfStandardSubset.Pdf_SiqQ_a;
                case "PDF/SiqQB":
                    return PdfStandardSubset.Pdf_SiqQ_b;
                default:
                    return PdfStandardSubset.Full;
            }
        }

        private static string GetPDFLicenseKey()
        {
            // Demo key
            // return "Q2hzY3Jjc2N3bXNjcHJtcnFtenp6eg==";

            // Winnovative PDF Reporting Toolkit for .NET Redistributable License 
            // Includes: HTML to PDF Converter, Excel Library, Web Chart
            return "+NPK2MnL2MnBwcrYwNbI2MvJ1snK1sHBwcE=";
        }

        #region Excel Report
        //public string GenerateExcelReport(string title, string reportSubject, string reportComments, params DataTable[] dataTable)
        //{
        //    string outputFilePath = string.Empty;

        //    // get the Excel workbook format
        //    ExcelWorkbookFormat workbookFormat = (ExcelVersion == 2003) ? ExcelWorkbookFormat.Xls_2003 : ExcelWorkbookFormat.Xlsx_2007;

        //    // create the workbook in the desired format with a single worksheet
        //    ExcelWorkbook workbook = new ExcelWorkbook(workbookFormat);

        //    // set the license key before saving the workbook
        //    //workbook.LicenseKey = "RW51ZXZ0ZXVldGt1ZXZ0a3R3a3x8fHw=";
        //    workbook.LicenseKey = GetPDFLicenseKey();

        //    // set workbook description properties
        //    workbook.DocumentProperties.Subject = "uGovernIT - " + reportSubject;
        //    workbook.DocumentProperties.Comments = "uGovernIT - " + reportComments;
        //    ExcelWorksheet worksheet = null;


        //    #region Add a style used for the data table header
        //    ExcelCellStyle dataHeaderStyle = workbook.Styles.AddStyle("DataHeaderStyle");
        //    dataHeaderStyle.Font.Size = 10;
        //    dataHeaderStyle.Font.Bold = true;
        //    dataHeaderStyle.Alignment.VerticalAlignment = ExcelCellVerticalAlignmentType.Center;
        //    dataHeaderStyle.Alignment.HorizontalAlignment = ExcelCellHorizontalAlignmentType.Left;
        //    dataHeaderStyle.Fill.FillType = ExcelCellFillType.SolidFill;
        //    dataHeaderStyle.Fill.SolidFillOptions.BackColor = Color.LightBlue;
        //    dataHeaderStyle.Borders[ExcelCellBorderIndex.Bottom].LineStyle = ExcelCellLineStyle.Thin;
        //    dataHeaderStyle.Borders[ExcelCellBorderIndex.Top].LineStyle = ExcelCellLineStyle.Thin;
        //    dataHeaderStyle.Borders[ExcelCellBorderIndex.Left].LineStyle = ExcelCellLineStyle.Thin;
        //    dataHeaderStyle.Borders[ExcelCellBorderIndex.Right].LineStyle = ExcelCellLineStyle.Thin;
        //    #endregion

        //    // formating worksheet in alterrows color.
        //    ExcelCellStyle dataAlternateRowStyle = workbook.Styles.AddStyle("DataAlternateRowStyle");
        //    dataAlternateRowStyle.Font.Size = 10;
        //    dataAlternateRowStyle.Font.Bold = false;
        //    dataAlternateRowStyle.Alignment.VerticalAlignment = ExcelCellVerticalAlignmentType.Center;
        //    dataAlternateRowStyle.Alignment.HorizontalAlignment = ExcelCellHorizontalAlignmentType.Left;
        //    dataAlternateRowStyle.Fill.FillType = ExcelCellFillType.SolidFill;
        //    dataAlternateRowStyle.Fill.SolidFillOptions.BackColor = Color.LightGray;


        //    #region Add a style used for the cells in the worksheet title area
        //    ExcelCellStyle titleStyle = workbook.Styles.AddStyle("WorksheetTitleStyle");
        //    // center the text in the title area
        //    titleStyle.Alignment.HorizontalAlignment = ExcelCellHorizontalAlignmentType.Left;
        //    titleStyle.Alignment.VerticalAlignment = ExcelCellVerticalAlignmentType.Center;
        //    // set the title area borders
        //    titleStyle.Borders[ExcelCellBorderIndex.Bottom].Color = Color.Green;
        //    titleStyle.Borders[ExcelCellBorderIndex.Bottom].LineStyle = ExcelCellLineStyle.Medium;
        //    titleStyle.Borders[ExcelCellBorderIndex.Top].Color = Color.Green;
        //    titleStyle.Borders[ExcelCellBorderIndex.Top].LineStyle = ExcelCellLineStyle.Medium;
        //    titleStyle.Borders[ExcelCellBorderIndex.Left].Color = Color.Green;
        //    titleStyle.Borders[ExcelCellBorderIndex.Left].LineStyle = ExcelCellLineStyle.Medium;
        //    titleStyle.Borders[ExcelCellBorderIndex.Right].Color = Color.Green;
        //    titleStyle.Borders[ExcelCellBorderIndex.Right].LineStyle = ExcelCellLineStyle.Medium;
        //    if (workbookFormat == ExcelWorkbookFormat.Xls_2003)
        //    {
        //        // set the solid fill for the title area range with a custom color
        //        titleStyle.Fill.FillType = ExcelCellFillType.SolidFill;
        //        titleStyle.Fill.SolidFillOptions.BackColor = Color.FromArgb(255, 255, 204);
        //    }
        //    else
        //    {
        //        // set the gradient fill for the title area range with a custom color
        //        titleStyle.Fill.FillType = ExcelCellFillType.GradientFill;
        //        titleStyle.Fill.GradientFillOptions.Color1 = Color.FromArgb(255, 255, 204);
        //        titleStyle.Fill.GradientFillOptions.Color2 = Color.White;
        //    }
        //    // set the title area font 
        //    titleStyle.Font.Size = 14;
        //    titleStyle.Font.Bold = true;
        //    titleStyle.Font.UnderlineType = ExcelCellUnderlineType.Single;
        //    #endregion

        //    for (int i = 0; i < dataTable.Length; i++)
        //    {
        //        DataTable table = dataTable[i];
        //        // get the first worksheet in the workbook

        //        if (workbook.Worksheets.Count >= dataTable.Length)
        //        {
        //            worksheet = workbook.Worksheets[i];
        //        }
        //        else
        //        {
        //            workbook.Worksheets.AddWorksheet();
        //            worksheet = workbook.Worksheets[i];
        //        }

        //        worksheet.PageSetup.PaperSize = ExcelPagePaperSize.PaperA4;
        //        worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
        //        worksheet.PageSetup.LeftMargin = 1;
        //        worksheet.PageSetup.RightMargin = 1;
        //        worksheet.PageSetup.TopMargin = 1;
        //        worksheet.PageSetup.BottomMargin = 1;

        //        string cname = GetExcelColumnName(table.Columns.Count);

        //        // set the default worksheet name
        //        worksheet.Name = table.TableName;

        //        if (table.Rows.Count <= 0)
        //            table.Rows.Add("No data");

        //        // load data from DataTable into the worksheet
        //        worksheet.LoadDataTable(table, 5, 1, true);

        //        #region WRITE THE WORKSHEET TOP TITLE

        //        // merge the cells in the range to create the title area 
        //        worksheet["A2:" + cname + "3"].Merge();
        //        // gets the merged range containing the top left cell of the range
        //        ExcelRange titleRange = worksheet["A2"].MergeArea;
        //        // set the text of title area
        //        worksheet["A2"].Text = table.TableName;

        //        // set a row height of 18 points for each row in the range
        //        titleRange.RowHeightInPoints = 18;
        //        // set the worksheet top title style
        //        titleRange.Style = titleStyle;

        //        #endregion

        //        worksheet["A5:" + cname + "5"].Style = dataHeaderStyle;
        //        worksheet["A5:" + cname + "5"].RowHeightInPoints = 20;

        //        // lock the data table header
        //        worksheet["A6"].FreezePanes();

        //        for (int j = 0; j < table.Rows.Count; j++)
        //        {
        //            if ((j % 2) == 0)
        //            {
        //                string rangeReference = string.Format("A{0}:{1}{2}", (6 + j), cname, (6 + j));
        //                worksheet[rangeReference].Style = dataAlternateRowStyle;
        //            }
        //        }

        //        // autofit column width
        //        worksheet.AutofitColumns();
        //    }

        //    // Save the Excel document in local path
        //    try
        //    {
        //        string outFileName = workbookFormat == ExcelWorkbookFormat.Xls_2003 ? title + ".xls" : title + ".xlsx";
        //        string outputPath = UGITUtility.GetTempFolderPath();
        //        outputFilePath = System.IO.Path.Combine(outputPath, outFileName);
        //        SPSecurity.RunWithElevatedPrivileges(delegate ()
        //        {
        //            if (File.Exists(outputFilePath))
        //            {
        //                File.Delete(outputFilePath);
        //            }
        //            workbook.Save(outputFilePath);
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.Log.ULog.WriteException(ex);
        //    }
        //    finally
        //    {
        //        workbook.Close();
        //        workbook = null;
        //        worksheet = null;
        //    }
        //    return outputFilePath;
        //}

        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
        #endregion

    }
}
 