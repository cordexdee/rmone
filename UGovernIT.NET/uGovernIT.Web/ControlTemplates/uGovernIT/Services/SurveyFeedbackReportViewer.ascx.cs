
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Linq;
using uGovernIT.Manager.Reports;
using System.Text;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class SurveyFeedbackReportViewer : UserControl
    {
        public string TicketId { get; set; }
        public string Filteredids { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string Selectedsurvey { get; set; }
        public string Surveyoftype { get; set; }
        public string Survey { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        public string stageName { get; set; }
        public string FilterExp { get; set; }
        //private DataTable resultedTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Selectedsurvey))
                return;
            //SurveyFeedbackReport surveyreportobj = new SurveyFeedbackReport(LoadFeedbackData(), Surveyoftype, Survey);
            //Test obj = new Test(LoadFeedbackData());
            //RptVwrProjectReport.Report = surveyreportobj;
        }

        /// <summary>
        /// Loads feedback table from selected module
        /// </summary>
        /// <returns></returns>
        /// private DataTable LoadFeedbackData(string surveyId)
        private DataTable LoadFeedbackData()
        {
            if (string.IsNullOrEmpty(Selectedsurvey))
                return null;

            ServicesManager servicesManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            Services service = servicesManager.LoadSurveybySurvey(Convert.ToInt32(Selectedsurvey));
            if (service == null)
            {
                return null;
            }
            
            
            //if (!string.IsNullOrEmpty(Filteredids))
            //{
            //    List<string> lstids = Filteredids.Split(',').ToList();
            //    StringBuilder strcominequery = new StringBuilder();
            //    strcominequery.Append(string.Format("<In><FieldRef Name='{0}' />", DatabaseObjects.Columns.Id));
            //    strcominequery.Append("<Values>");
            //    lstids.ForEach(x => strcominequery.Append(string.Format("<Value Type='Number'>{0}</Value>", Convert.ToInt32(x))));
            //    strcominequery.Append("</Values>");
            //    strcominequery.Append("</In>");
            //    List<string> expression = new List<string>();
            //    expression.Add(strcominequery.ToString());
            //    expression.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.ServiceTitleLookup, Selectedsurvey));

            //    sQuery.Query = string.Format("<Where>{0}</Where>", uHelper.GenerateWhereQueryWithAndOr(expression, true));

            //}
            //else
            //    sQuery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='TRUE'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ServiceTitleLookup, Selectedsurvey);

            //DataTable table = SPListHelper.GetDataTable(DatabaseObjects.Lists.SurveyFeedback, sQuery);

            //return null if there is no feedback
            //if (table == null)
            //{
            //    return null;
            //}

            ////Changes column name of created, userdepartment, userlocation
            //if (table.Columns.Contains(DatabaseObjects.Columns.Created))
            //    table.Columns[DatabaseObjects.Columns.Created].ColumnName = "Submit Date";

            //if (table.Columns.Contains(DatabaseObjects.Columns.Author))
            //    table.Columns[DatabaseObjects.Columns.Author].ColumnName = "Submitted By";

            //if (table.Columns.Contains(DatabaseObjects.Columns.TotalRating))
            //{
            //    table.Columns[DatabaseObjects.Columns.TotalRating].ColumnName = "Average Rating";
            //}

            //DataTable calculatedData = new DataTable();

            //List<ServiceQuestion> rQuestions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating).ToList();
            //ServiceQuestion question = null;
            //List<string> deleteColumns = new List<string>();
            //foreach (DataColumn column in table.Columns)
            //{
            //    question = rQuestions.FirstOrDefault(x => x.TokenName == column.ColumnName);
            //    if (question != null)
            //    {
            //        column.Caption = question.QuestionTitle;
            //        if (!table.Columns.Contains(question.QuestionTitle))
            //            column.ColumnName = question.QuestionTitle;
            //    }
            //    else if (column.ColumnName.Contains("Rating") && !column.ColumnName.Contains("Average Rating")
            //        && !column.ColumnName.Contains(DatabaseObjects.Columns.ModuleName) && !column.ColumnName.Contains(DatabaseObjects.Columns.Id))
            //    {
            //        deleteColumns.Add(column.ColumnName);
            //    }
            //}

            ////Remove unwanted columns
            //foreach (string column in deleteColumns)
            //{
            //    table.Columns.Remove(table.Columns[column]);
            //}

            //if (!table.Columns.Contains(DatabaseObjects.Columns.Title))
            //    table.Columns.Add(DatabaseObjects.Columns.Title);
            //if (!table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
            //{
            //    table.Columns.Add(DatabaseObjects.Columns.TicketPRP);

            //}
            //if (!table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
            //    table.Columns.Add(DatabaseObjects.Columns.TicketOwner);
            //if (!table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
            //    table.Columns.Add(DatabaseObjects.Columns.CategoryName);

            //if (table.Columns.Contains(DatabaseObjects.Columns.Title) && table.Columns.Contains(DatabaseObjects.Columns.TicketId))
            //{
            //    table.Columns[DatabaseObjects.Columns.TicketId].SetOrdinal(0);
            //    table.Columns[DatabaseObjects.Columns.Title].SetOrdinal(1);
            //}

            //if (table.Columns.Contains(DatabaseObjects.Columns.TicketId))
            //    table.Columns[DatabaseObjects.Columns.TicketId].ColumnName = "Ticket ID";

            //if (table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
            //    table.Columns[DatabaseObjects.Columns.TicketPRP].ColumnName = "PRP";

            //if (table.Columns.Contains(DatabaseObjects.Columns.TicketOwner))
            //    table.Columns[DatabaseObjects.Columns.TicketOwner].ColumnName = "Owner";

            //if (table.Columns.Contains(DatabaseObjects.Columns.CategoryName))
            //    table.Columns[DatabaseObjects.Columns.CategoryName].ColumnName = "Category";

            //if (table.Columns.Contains(DatabaseObjects.Columns.SubCategory))
            //    table.Columns[DatabaseObjects.Columns.SubCategory].ColumnName = "Sub-Category";

            //if (table.Columns.Contains(DatabaseObjects.Columns.TicketRequestType))
            //    table.Columns[DatabaseObjects.Columns.TicketRequestType].ColumnName = "Request Type";

            //if (table.Columns.Contains(DatabaseObjects.Columns.SLADisabled))
            //    table.Columns[DatabaseObjects.Columns.SLADisabled].ColumnName = "SLA Disabled";

            //resultedTable = table;

            return new DataTable();
        }
       
        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                //string fileName = string.Format("Survey_Feedback_Report_{0}_{1}", Surveyoftype, uHelper.GetCurrentTimestamp());
                //string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                //string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                //RptVwrProjectReport.Report.ExportToPdf(path);

                //e.Result = UGITUtility.GetAbsoluteURL("_layouts/15/ugovernit/DelegateControl.aspx?control=ticketemail&type=surveyfeedbackreport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                //string typeparam = "multiprojectreport";
                //string fileName = string.Format("Survey_Feedback_Report_{0}", uHelper.GetCurrentTimestamp());
                //string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                //string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                //RptVwrProjectReport.Report.ExportToPdf(path);
                //e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&type=" + typeparam + "&localpath=" + path + "&relativepath=" + uploadFileURL + "&DocName=" + DocName + "&folderid=" + FolderGuid);
            }

        }
    }
}
