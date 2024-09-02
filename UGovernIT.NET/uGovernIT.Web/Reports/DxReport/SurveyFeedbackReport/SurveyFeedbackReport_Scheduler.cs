using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public class SurveyFeedbackReport_Scheduler : IReportScheduler
    {
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.SurveyFeedbackReport);
            formdic.Add(ReportScheduleConstant.Type, "ALL");
            formdic.Add(ReportScheduleConstant.Module, "");
            formdic.Add(ReportScheduleConstant.Survey, "");
            formdic.Add(ReportScheduleConstant.FromDate, "");
            formdic.Add(ReportScheduleConstant.ToDate, "");
            return formdic;
        }
        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
            string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
            string module = Convert.ToString(formobj[ReportScheduleConstant.Module]);
            string type = Convert.ToString(formobj[ReportScheduleConstant.Type]);
            string survey = Convert.ToString(formobj[ReportScheduleConstant.Survey]);
            DateTime DateFrom = new DateTime();
            DateTime DateTo = new DateTime();
            if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
            {
                string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
                Dictionary<string, DateTime> dic = uHelper.GetReportScheduleDates(applicationContext,dateRange);
                if (dic.Count > 0)
                {
                    DateFrom = dic["DateFrom"];
                    DateTo = dic["DateTo"];
                }
            }
            string fromdate = DateFrom.ToString(), todate = DateTo.ToString();
            //Get report for schedule survey feedback
            SurveyFeedbackReport_SurveyHelper helper = new SurveyFeedbackReport_SurveyHelper(applicationContext,module, fromDate: fromdate, toDate: todate);
            XtraReport report = helper.GetFeedbackReport();
            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);
            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
            return ReportHelper.ExportFiles(report, attachFormat, filePath, title);
        }
    }
}