using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
namespace uGovernIT.DxReport
{
    public class SurveyFeedbackReport_SurveyHelper
    {
        public string Selectedsurvey { get; set; }
        public string Surveyoftype { get; set; }
        public string Survey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        Services service;
        ServicesManager servicesManager;
        SurveyFeedbackManager surveyFeedbackManager;
        public DataTable resultedTable;
        public string FilterExpression { get; set; }
        public SurveyFeedbackReport_SurveyHelper(ApplicationContext context, string selectedSurvey,string fromDate="",string toDate="")
        {
            servicesManager = new ServicesManager(context);
            surveyFeedbackManager = new SurveyFeedbackManager(context);
            Selectedsurvey = selectedSurvey;
            FromDate =UGITUtility.StringToDateTime(fromDate);
            ToDate = UGITUtility.StringToDateTime(toDate);
        }

        public XtraReport GetFeedbackReport()
        {
            XtraReport report;
            SurveyFeedbackReport_Report surveyreportobj = new SurveyFeedbackReport_Report(LoadFeedbackData(), Surveyoftype, Survey);
            report = surveyreportobj;
            return report;
        }

        /// <summary>
        /// Loads feedback table from selected module
        /// </summary>
        /// <returns></returns>
        /// private DataTable LoadFeedbackData(string surveyId)
        public DataTable LoadFeedbackData()
        {
            if (string.IsNullOrEmpty(Selectedsurvey))
                return null;
            try
            {
                service = servicesManager.LoadByID(Convert.ToInt32(Selectedsurvey));
                if (service == null)
                    return null;
                if (service.ModuleId > 0)
                {
                    Survey = service.Title;
                    Surveyoftype = "Module";
                }
                if (service.ModuleId == 0)
                {
                    Surveyoftype = "Generic";
                    Survey = service.Title;
                }

                DataRow[] dataRow = null;
                DataTable table = null;
                dataRow = surveyFeedbackManager.GetDataTable().Select(string.Format("{0}={1}", "ServiceLookUp", Selectedsurvey));
                if (dataRow != null && dataRow.Length > 0)
                {
                    table = dataRow.CopyToDataTable();
                }
                //return null if there is no feedback
                if (table == null)
                {
                    return null;
                }

                //Changes column name of created, userdepartment, userlocation
                if (table.Columns.Contains(DatabaseObjects.Columns.Created))
                    table.Columns[DatabaseObjects.Columns.Created].ColumnName = "Submit Date";

                if (table.Columns.Contains(DatabaseObjects.Columns.Author))
                    table.Columns[DatabaseObjects.Columns.Author].ColumnName = "Submitted By";

                if (table.Columns.Contains(DatabaseObjects.Columns.TotalRating))
                    table.Columns[DatabaseObjects.Columns.TotalRating].ColumnName = "Average Rating";

                DataTable calculatedData = new DataTable();
                List<ServiceQuestion> rQuestions = null;
                if (service.Questions != null)
                    rQuestions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating).ToList();

                ServiceQuestion question = null;
                List<string> deleteColumns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    if(rQuestions!=null)
                        question = rQuestions.FirstOrDefault(x => x.TokenName == column.ColumnName);
                    if (question != null)
                    {

                        column.Caption = question.QuestionTitle;
                        if (!table.Columns.Contains(question.QuestionTitle))
                            column.ColumnName = question.QuestionTitle;
                    }
                    else if ((column.ColumnName.Contains("Rating") || column.ColumnName.Equals("Description")) && !column.ColumnName.Contains("Average Rating")
                        && (!column.ColumnName.Contains(DatabaseObjects.Columns.ModuleName) || !column.ColumnName.Contains(DatabaseObjects.Columns.ModuleNameLookup)) && !column.ColumnName.Contains(DatabaseObjects.Columns.Id)||column.ColumnName.Equals(DatabaseObjects.Columns.TenantID))
                    {
                        deleteColumns.Add(column.ColumnName);
                    }
                }

                    //Remove unwanted columns

                    foreach (string column in deleteColumns)
                    {
                        table.Columns.Remove(table.Columns[column]);
                    }
                
                if (!table.Columns.Contains(DatabaseObjects.Columns.Title))
                    table.Columns.Add(DatabaseObjects.Columns.Title);
                if (!table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                    table.Columns.Add(DatabaseObjects.Columns.TicketPRP);
                if (!table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                    table.Columns.Add(DatabaseObjects.Columns.TicketOwner);
                if (!table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                    table.Columns.Add(DatabaseObjects.Columns.CategoryName);

                if (!table.Columns.Contains(DatabaseObjects.Columns.UserDepartment))
                    table.Columns.Add(DatabaseObjects.Columns.UserDepartment);


                if (!table.Columns.Contains(DatabaseObjects.Columns.UserLocation))
                    table.Columns.Add(DatabaseObjects.Columns.UserLocation);

                if (table.Columns.Contains(DatabaseObjects.Columns.Title) && table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                {
                    table.Columns[DatabaseObjects.Columns.TicketId].SetOrdinal(0);
                    table.Columns[DatabaseObjects.Columns.Title].SetOrdinal(1);
                }

                if (table.Columns.Contains(DatabaseObjects.Columns.TicketId))
                    table.Columns[DatabaseObjects.Columns.TicketId].ColumnName = "Ticket ID";

                if (table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                    table.Columns[DatabaseObjects.Columns.TicketPRP].ColumnName = "PRP";

                if (table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
                    table.Columns[DatabaseObjects.Columns.TicketOwner].ColumnName = "Owner";

                if (table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                    table.Columns[DatabaseObjects.Columns.CategoryName].ColumnName = "Category";

                if (table.Columns.Contains(DatabaseObjects.Columns.UserDepartment))
                    table.Columns[DatabaseObjects.Columns.UserDepartment].ColumnName = "Department";

                if (table.Columns.Contains(DatabaseObjects.Columns.UserLocation))
                    table.Columns[DatabaseObjects.Columns.UserLocation].ColumnName = "Location";

                if (FromDate != DateTime.MinValue && ToDate != DateTime.MinValue &&
                    table != null && table.Rows.Count > 0 && uHelper.IfColumnExists("Submit Date", table))
                {
                    DataRow[] rowColl = table.Select(string.Format("[{0}]>=#{1}# AND [{0}]<=#{2}#", "Submit Date",
                                                     FromDate.Date.ToString("MM/dd/yyyy"), ToDate.Date.AddDays(1).ToString("MM/dd/yyyy")));
                    if (rowColl != null)
                    {
                        if (rowColl.Length == 0)
                            resultedTable = null;
                        else
                            resultedTable = rowColl.CopyToDataTable();
                        table = resultedTable;
                    }
                }

                if (table != null && table.Rows.Count > 0 && !string.IsNullOrEmpty(FilterExpression))
                {
                    DataRow[] filterColl = table.Select(FilterExpression);
                    if (filterColl == null || filterColl.Length == 0)
                        return table.Clone();

                    table = filterColl.CopyToDataTable();

                }

                return table;
            }
            catch (Exception e)
            {
                uGovernIT.Util.Log.ULog.WriteException(e); return null;
            }
            
        }
    }
}
