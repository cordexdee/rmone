using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Entities;
using uGovernIT.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public class WeeklyTeamReport_Scheduler : IReportScheduler
    {
        ModuleStatistics moduleStatistics;
        public WeeklyTeamReport_Scheduler()
        {

        }
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.WeeklyTeamReport);
            formdic.Add(ReportScheduleConstant.Module, "TSR");
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", "-7", Constants.Separator1, "0", Constants.Separator, "Days"));
            formdic.Add(ReportScheduleConstant.Category, "All");
            return formdic;
        }
        public string GetReport(ApplicationContext _context,Dictionary<string, object> formobj, string attachFormat)
        {
            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
            string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
            string category = formobj.ContainsKey(ReportScheduleConstant.Category) ? Convert.ToString(formobj[ReportScheduleConstant.Category]) : string.Empty;

            DateTime DateFrom = new DateTime();
            DateTime DateTo = new DateTime();
            if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
            {
                string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
                Dictionary<string, DateTime> dic = uHelper.GetReportScheduleDates(_context,dateRange);
                if (dic.Count > 0)
                {
                    DateFrom = dic["DateFrom"];
                    DateTo = dic["DateTo"];
                }
            }

            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

            string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;

            DataTable data = new DataTable();
            moduleStatistics = new ModuleStatistics(_context);
            data = moduleStatistics.GetWeeklyTeamPrfCount(moduleName, DateFrom, DateTo, category);
            WeeklyTeamPrfReportEntity entity = new WeeklyTeamPrfReportEntity();
            entity.Data = data;

            if (DateFrom != DateTime.MinValue)
                entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);
            if (DateTo != DateTime.MinValue)
                entity.EndDate = UGITUtility.GetDateStringInFormat(DateTo, false);
            WeeklyTeamReport_Report reportWeekly = new WeeklyTeamReport_Report(entity);
            XtraReport report = reportWeekly;
            return ReportHelper.ExportFiles(report, attachFormat, filePath, title);
        }
    }

}