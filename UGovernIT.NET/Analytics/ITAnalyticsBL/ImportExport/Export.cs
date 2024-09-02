using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ITAnalyticsUtility;
using ITAnalyticsBL.Core;
using ITAnalyticsBL.BL;
using System.IO;




namespace ITAnalyticsBL.ImportExport
{
    public class Export
    {
        /// <summary>
        /// Export analytic xml to excel sheet
        /// </summary>
        /// <param name="analyticXml">analytic xml</param>
        /// <param name="fileExtention">file extention required(xsl or xslx (by default its 'xlsx'))</param>
        /// <param name="columnWise">Column wise question required or not (by default its false)</param>
        /// <param name="headerRequired">Header required or not (by default its true)</param>
        /// <returns></returns>
        public static string ExportAnalyticIntoExcel(XmlDocument analyticXml, string fileExtention = "xlsx", bool columnWise = false, bool headerRequired = true)
        {
            string filePath = string.Empty;
            if (analyticXml != null)
            {
                // Get Analytic Title
                string analyticTitle = analyticXml.DocumentElement.GetAttribute(XmlTagLibrary.Title);
                if (analyticTitle != null)
                {
                    analyticTitle = analyticTitle.Replace(" ", "_") + Guid.NewGuid().ToString();
                }

                //create a copy of export xls template
                string extention = "xls";
                if (fileExtention != null && fileExtention.Equals("xlsx", StringComparison.CurrentCultureIgnoreCase))
                {
                    extention = "xlsx";
                }
                string excelPath = string.Format("{0}{1}.{2}", System.Web.HttpContext.Current.Server.MapPath("/tempchart/"), analyticTitle, extention);

                //Check copied excel file is exist of not
                //if it not exist then call exportanaylticforinput again to run process again
                if (System.IO.File.Exists(excelPath))
                {
                    FileInfo excelFile = new FileInfo(excelPath);
                    excelFile.Delete();
                    ExportAnalyticIntoExcel(analyticXml, fileExtention, columnWise);
                }

                //Convert analytic xml into  object
                Analytic analytic = Analytic.GetAnalytic(analyticXml);


                Application exlApp = new Application();
                if (exlApp == null)
                {
                    return "Enable to create excel application obj.";
                }

                Workbook wb = exlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = wb.Worksheets.Add();
                ws.Name = "Analytic";

                int rowNumber = 1;
                if (headerRequired)
                {
                    CreateExcelHeader(ws, columnWise, analytic);
                }

                // Create analytic template from xmldocumention
                if (analytic != null)
                {
                    if (analytic.Sections != null && analytic.Sections.Count > 0)
                    {
                        CreateSectionsRow(ws, analytic.Sections, rowNumber, columnWise, headerRequired);
                    }
                    else
                    {
                        int lastColumnUsed = 0;
                        if (headerRequired)
                        {
                            lastColumnUsed = 1;
                        }
                        CreateSubSectionsRow(ws, analytic.SubSections, rowNumber, lastColumnUsed, columnWise, headerRequired);
                    }
                }

                ws.SaveAs(excelPath, Type.Missing);
                wb.Close();
                filePath = excelPath;
            }


            return filePath;
        }

        /// <summary>
        /// Create Section Cells
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="analyticSections"></param>
        /// <param name="rowNumber"></param>
        /// <param name="rowWise"></param>
        private static void CreateSectionsRow(Worksheet ws, List<AnalyticSection> analyticSections, int rowNumber, bool columnWise, bool headerRequired)
        {
            if (rowNumber <= 0)
            {
                rowNumber = 1;
            }
            int lastColumn = 0;

            // if header required then leave one block for header(only before first section)
            if (headerRequired)
            {
                lastColumn = 1;
            }

            if (analyticSections != null && analyticSections.Count > 0)
            {
                int lastUsedColumn = 0;
                foreach (AnalyticSection section in analyticSections)
                {
                    int tolalFeatureInSection = section.SubSections.Where(x => x.Features != null).Sum(x => x.Features.Count);

                    if (tolalFeatureInSection > 0)
                    {

                        Range aRange = null;
                        if (columnWise)
                        {
                            string column = ExportImportHelper.GetExcelColumnName(rowNumber);
                            int startRow = lastColumn + 1;
                            int endRow = lastColumn + tolalFeatureInSection;
                            aRange = ws.get_Range(string.Format("{0}{1}", column, startRow), string.Format("{0}{1}", column, endRow));
                            aRange.ColumnWidth = 30;
                        }
                        else
                        {
                            string startColumn = ExportImportHelper.GetExcelColumnName(lastColumn + 1);
                            string endColumn = ExportImportHelper.GetExcelColumnName(lastColumn + tolalFeatureInSection);
                            aRange = ws.get_Range(string.Format("{0}{1}", startColumn, rowNumber), string.Format("{0}{1}", endColumn, rowNumber));
                           
                        }

                        aRange.Merge();
                        aRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        aRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        aRange.set_Value(Type.Missing, section.Title);
                        aRange.Style.WrapText = true;

                        lastColumn = lastColumn + tolalFeatureInSection;
                        // if header required then leave one block for header (only before first subsection)
                        if (analyticSections.IndexOf(section) == 0 && headerRequired)
                        {
                            lastUsedColumn = 1;
                        }
                        lastUsedColumn = CreateSubSectionsRow(ws, section.SubSections, rowNumber + 1, lastUsedColumn, columnWise, headerRequired);
                    }
                }
            }
        }

        /// <summary>
        /// Create Sub Section Cellls
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="analyticSubSections"></param>
        /// <param name="rowNumber"></param>
        /// <param name="lastUsedColumn"></param>
        /// <param name="rowWise"></param>
        /// <returns></returns>
        private static int CreateSubSectionsRow(Worksheet ws, List<AnalyticSubSection> analyticSubSections, int rowNumber, int lastUsedColumn, bool rowWise, bool headerRequired)
        {
            if (rowNumber <= 0)
            {
                rowNumber = 1;
            }
            int lastColumn = lastUsedColumn;

            if (analyticSubSections != null && analyticSubSections.Count > 0)
            {
                int lastFeatureColumn = lastUsedColumn + 1;
                foreach (AnalyticSubSection subSection in analyticSubSections)
                {
                    int tolalFeatureInSection = subSection.Features != null ? subSection.Features.Count : 0;

                    if (tolalFeatureInSection > 0)
                    {
                        string sRange = string.Empty;
                        Range aRange = null;
                        if (rowWise)
                        {
                            string column = ExportImportHelper.GetExcelColumnName(rowNumber);
                            int startRow = lastColumn + 1;
                            int endRow = lastColumn + tolalFeatureInSection;
                            aRange = ws.get_Range(string.Format("{0}{1}", column, startRow), string.Format("{0}{1}", column, endRow));
                            aRange.ColumnWidth = 30;
                        }
                        else
                        {
                            string startColumn = ExportImportHelper.GetExcelColumnName(lastColumn + 1);
                            string endColumn = ExportImportHelper.GetExcelColumnName(lastColumn + tolalFeatureInSection);
                            aRange = ws.get_Range(string.Format("{0}{1}", startColumn, rowNumber), string.Format("{0}{1}", endColumn, rowNumber));
                        }

                       
                        aRange.Merge();
                        aRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        aRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        aRange.set_Value(Type.Missing, subSection.Title);
                        aRange.Style.WrapText = true;


                        lastColumn = lastColumn + tolalFeatureInSection;

                        // if header required then leave one block for header (only before first feature)
                        if (analyticSubSections.IndexOf(subSection) == 0 && headerRequired)
                        {
                            lastUsedColumn = 1;
                        }
                        lastFeatureColumn = CreateFeaturesRow(ws, subSection.Features, rowNumber + 1, lastFeatureColumn, rowWise);
                    }
                }
            }

            return lastColumn;
        }

        /// <summary>
        /// Create Feature Cells
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="analyticFeatures"></param>
        /// <param name="rowNumber"></param>
        /// <param name="lastUsedColumn"></param>
        /// <param name="rowWise"></param>
        /// <returns></returns>
        private static int CreateFeaturesRow(Worksheet ws, List<AnalyticFeature> analyticFeatures, int rowNumber, int lastUsedColumn, bool rowWise)
        {
            if (rowNumber <= 0)
            {
                rowNumber = 1;
            }
            int lastColumn = lastUsedColumn;

            if (analyticFeatures != null && analyticFeatures.Count > 0)
            {
                foreach (AnalyticFeature feature in analyticFeatures)
                {
                    string column = string.Empty;
                    string columnValue = string.Empty;
                    string rowValue = string.Empty;
                    if (rowWise)
                    {
                        string startColumn = ExportImportHelper.GetExcelColumnName(rowNumber);
                        columnValue = startColumn;
                        rowValue = lastColumn.ToString();
                        ws.Cells[rowValue, columnValue].ColumnWidth = 60;
                    }
                    else
                    {
                        string startColumn = ExportImportHelper.GetExcelColumnName(lastColumn);
                        columnValue = startColumn;
                        rowValue = rowNumber.ToString();
                    }

                    ws.Cells[rowValue, columnValue].Value = feature.Question != string.Empty ? feature.Question : feature.Name;
                    ws.Cells[rowValue, columnValue].Style.WrapText = true;
                    ws.Cells[rowValue, columnValue].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    ws.Cells[rowValue, columnValue].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    string commentVal = GetFeatureComment(feature);
                    if (commentVal != null && commentVal.Trim() != string.Empty)
                    {
                        ws.Cells[rowValue, columnValue].AddComment(commentVal);
                    }

                    lastColumn += 1;

                }
            }

            return lastColumn;
        }

        /// <summary>
        /// Get Feature comment which hint the answers
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        private static string GetFeatureComment(AnalyticFeature feature)
        {
            string commentVal = string.Empty;
            if (feature is AnalyticBooleanVariable)
            {
                commentVal = "Please enter value in \"true\" or \"false\"";
            }
            else if (feature is AnalyticLinquisticVariable)
            {
                AnalyticLinquisticVariable obj = (AnalyticLinquisticVariable)feature;
                if (obj.VariableRangeLabels != null)
                {
                    for (int i = 0; i < obj.VariableRangeLabels.Count; i++)
                    {
                        if (i != 0)
                        {
                            commentVal += ", ";
                        }
                        commentVal += obj.VariableRangeLabels[i].Label;
                    }
                    commentVal = "Please select value from {" + commentVal + "}";
                }
            }
            else if (feature is AnalyticLinquisticFuzzyVariable)
            {
               commentVal = "Please enter value in digits";
            }
            else if (feature is AnalyticNumericVariable)
            {
               commentVal = "Please enter value in digits";
            }

            return commentVal;
        }

        private static void CreateExcelHeader(Worksheet ws, bool useColumnWiseHeader, Analytic analytic)
        {
            //Check whether section exist or not show we can decide whether section will be included or not
            bool sectionExist = false;
            if (analytic.Sections != null && analytic.Sections.Count > 0)
            {
                sectionExist = true;
            }

            //Decide which type of header required (column wise or row wise)
            if (useColumnWiseHeader)
            {
                ws.Cells[Type.Missing, 1].EntireRow.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#D1EAFD");
                ws.Cells[Type.Missing, 1].EntireRow.Font.Color = System.Drawing.ColorTranslator.FromHtml("#000000");
                ws.Cells[Type.Missing, 1].EntireRow.Font.Bold = true;
                ws.Cells[Type.Missing, 1].EntireRow.VerticalAlignment =  Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                ws.Cells[Type.Missing, 1].EntireRow.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                if (sectionExist)
                {
                    ws.Cells[1, 1].Value = "Section";
                    ws.Cells[1, 2].Value = "Sub Section";
                    ws.Cells[1, 3].Value = "Feature";
                }
                else
                {
                    ws.Cells[1, 1].Value = "Sub Section";
                    ws.Cells[1, 2].Value = "Feature";
                }
            }
            else
            {
                ws.Cells[1, Type.Missing].EntireColumn.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#D1EAFD");
                ws.Cells[1, Type.Missing].EntireColumn.Font.Color = System.Drawing.ColorTranslator.FromHtml("#000000");
                ws.Cells[1, Type.Missing].EntireColumn.Font.Bold = true;
                ws.Cells[1, Type.Missing].EntireColumn.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                ws.Cells[1, Type.Missing].EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                if (sectionExist)
                {
                    ws.Cells[1, 1].Value = "Section";
                    ws.Cells[2, 1].Value = "Sub Section";
                    ws.Cells[3, 1].Value = "Feature";
                }
                else
                {
                    ws.Cells[1, 1].Value = "Sub Section";
                    ws.Cells[2, 1].Value = "Feature";
                }
            }
        }
    }
}
