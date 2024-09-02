using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using uGovernIT.Manager;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DxReport
{
    public class TicketSummary_Scheduler : IReportScheduler
    {
        public TicketSummary_Scheduler()
        {

        }
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.TicketSummary);
            formdic.Add(ReportScheduleConstant.Module, "All");
            formdic.Add(ReportScheduleConstant.SortByModule, false);
            formdic.Add(ReportScheduleConstant.TicketStatus, "All");
            formdic.Add(ReportScheduleConstant.SortBy, "oldesttonewest");
            formdic.Add(ReportScheduleConstant.FromDate, "");
            formdic.Add(ReportScheduleConstant.ToDate, "");
            return formdic;
        }
        /// <summary>
        /// Gets the ticket summary report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        public string GetReport(ApplicationContext _context, Dictionary<string, object> formobj, string attachFormat)
        {
            string tempPath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
            string module = Convert.ToString(formobj[ReportScheduleConstant.Module]);
            string sortbyModule = Convert.ToString(formobj[ReportScheduleConstant.SortByModule]);
            TicketStatus tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formobj[ReportScheduleConstant.TicketStatus]));
            string sortby = Convert.ToString(formobj[ReportScheduleConstant.SortBy]);
            string fromdate = string.Empty, todate = string.Empty;

            if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
            {
                string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
                Dictionary<string, DateTime> dic = uHelper.GetReportScheduleDates(_context, dateRange);
                if (dic.Count > 0)
                {
                    fromdate = UGITUtility.ObjectToString(dic["DateFrom"]);
                    todate = UGITUtility.ObjectToString(dic["DateTo"]);
                }
            }
            //if (!string.IsNullOrEmpty(Convert.ToString(formobj[ReportScheduleConstant.FromDate])))
            //{
            //    fromdate = Convert.ToString(formobj[ReportScheduleConstant.FromDate]);
            //}
            //if (!string.IsNullOrEmpty(Convert.ToString(formobj[ReportScheduleConstant.ToDate])))
            //{
            //    todate = Convert.ToString(formobj[ReportScheduleConstant.ToDate]);
            //}
            //UserProfile currentUser = _context.CurrentUser;
            TicketSummaryReportHelper tsrHelper = new TicketSummaryReportHelper(_context, module, sortby, sortbyModule, tstatus, fromdate, todate);
            XtraReport report = tsrHelper.GetTicketSummaryReport();
            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);
            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
            return ReportHelper.ExportFiles(report, attachFormat, tempPath, title);
        }
    }
}