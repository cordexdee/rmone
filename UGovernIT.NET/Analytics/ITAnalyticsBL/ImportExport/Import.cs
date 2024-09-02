using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ITAnalyticsBL.Core;
using ITAnalyticsBL.DB;

using System.IO;
using Microsoft.Office;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.Xml;

namespace ITAnalyticsBL.ImportExport
{
    public class Import
    {
        //// <summary>
        //// Convert excel runs to key, value pair dictionary so that input generator can unterstand the run value 
        //// </summary>
        //// <param name="analytic">analytic object</param>
        //// <param name="importExcelPath">Excel file path</param>
        //// <param name="numberOfInput">number of runs existed in excel file</param>
        //// <returns></returns>
        public static List<Dictionary<string, object>> GetAnalyticInputsValuesFromExcel(Analytic analytic, string importExcelPath, int numberOfInput)
        {
            List<Dictionary<string, object>> inputs = new List<Dictionary<string, object>>();
            bool rowWiseInputs = false;
            if (System.IO.File.Exists(importExcelPath))
            {
                FileInfo fInfo = new FileInfo(importExcelPath);
                if (fInfo.Extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
                {
                    int featureNumber = 0;
                    string firstFeature = string.Empty;
                    int startColumn = 1;

                    ///Get all feature persented in analytic in sequence.
                    List<AnalyticFeature> features = AnalyticFeature.GetAllFeaturesOfAanalytics(analytic);
                   
                    ///Check section is exist in the anaytic or not
                    if (analytic.Sections != null && analytic.Sections.Count > 0)
                    {
                        featureNumber = 3;
                    }
                    else
                    {
                        featureNumber = 2;
                    }

                    ///Get first feature name or question
                    if (features != null && features.Count > 0)
                    {
                        firstFeature = features[0].Question != string.Empty ? features[0].Question : features[0].Name;
                    }

                    Application exlApp = new Application();
                    Workbook wb = exlApp.Workbooks.Open(importExcelPath);
                    Worksheet ws = wb.Worksheets[1];

                    ///Check wheghter runs are row wise of column wise
                    string elxFeatureVal =  ws.Cells[featureNumber, ExportImportHelper.GetExcelColumnName(startColumn)].Value;
                    if (elxFeatureVal == firstFeature)
                    {
                        rowWiseInputs = true;
                    }

                    /// loop till last runs
                    for (int i = 1; i <= numberOfInput; i++)
                    {
                        Dictionary<string, object> input = new Dictionary<string, object>();
                        int inputNumber = featureNumber + i;
                        string inputColumn = ExportImportHelper.GetExcelColumnName(inputNumber);
                        int startFeature = 1;

                        ///loop till last feature
                        for (int j = 0; j < features.Count; j++)
                        {
                            string column = string.Empty;
                            string row = string.Empty;
                            ///Get column and row value based on rowwiseinputs type
                            if (rowWiseInputs)
                            {
                                column = ExportImportHelper.GetExcelColumnName(startFeature);
                                row = inputNumber.ToString();
                                startFeature += 1;
                            }
                            else
                            {
                                column = inputColumn;
                                row = startFeature.ToString();
                                startFeature += 1;
                            }

                            string value = Convert.ToString(ws.Cells[row, column].Value);
                            string featureQuestion = features[j].Question != string.Empty ? features[j].Question : features[j].Name;
                            string key = features[j].Name.Replace(" ", "_");
                            if (!input.ContainsKey(key) && value != null && value.Trim() != string.Empty)
                            {
                                input.Add(key, value);
                            }
                        }
                        inputs.Add(input);
                    }

                    wb.Close();
                }
            }
            return inputs;
        }

        //// <summary>
        //// Convert excel runs to key, value pair dictionary so that input generator can unterstand the run value 
        //// </summary>
        //// <param name="analyticXml">Analytic xml doc</param>
        //// <param name="importExcelPath">Excel file path</param>
        //// <param name="numberOfInput">number of runs existed in excel file</param>
        //// <returns></returns>
        public static List<XmlDocument> GetAnalyticInputXmlsFromExcel(XmlDocument analyticXml, string importExcelPath, int numberOfInput)
        {
            Analytic analytic = Analytic.GetAnalytic(analyticXml);
            List<XmlDocument> inputs = new List<XmlDocument>();
            List<Dictionary<string, object>> inputsForm = Import.GetAnalyticInputsValuesFromExcel(analytic, importExcelPath, numberOfInput);
            foreach (Dictionary<string, object> inputForm in inputsForm)
            {
                inputs.Add(AnalyticInputGenerator.CreateAnalyticInput(analytic, inputForm));
            }
            return inputs;
        }
    }
}
