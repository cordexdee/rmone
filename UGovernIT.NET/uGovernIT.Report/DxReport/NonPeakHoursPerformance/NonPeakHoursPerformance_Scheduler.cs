using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Manager.Report.Entities;
using uGovernIT.Manager.Reports;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;
namespace uGovernIT.Report.DxReport
{
    public class NonPeakHoursPerformance_Scheduler : IReportScheduler
    {
        ModuleStatistics moduleStatistics;
        public NonPeakHoursPerformance_Scheduler()
        {

        }
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.NonPeakHoursPerformance);
            formdic.Add(ReportScheduleConstant.Module, "TSR");
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", "-7", Constants.Separator1, "0", Constants.Separator, "Days"));
            formdic.Add(ReportScheduleConstant.NonPeakHourWindow, "");
            formdic.Add(ReportScheduleConstant.WorkingWindowStartTime, "");
            formdic.Add(ReportScheduleConstant.WorkingWindowEndTime, "");
            formdic.Add(ReportScheduleConstant.Category, "All");
            return formdic;

        }
        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
              moduleStatistics = new ModuleStatistics(applicationContext);
                string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
                string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
               DateTime DateFrom = new DateTime();
                if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
                {
                    string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
                    Dictionary<string, DateTime> dic = uHelper.GetReportScheduleDates(applicationContext,dateRange);
                    if (dic.Count > 0)
                        DateFrom = dic["DateFrom"];
                }
                string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;

                int nonPeakHourWindow = formobj.ContainsKey(ReportScheduleConstant.NonPeakHourWindow) ? UGITUtility.StringToInt(formobj[ReportScheduleConstant.NonPeakHourWindow]) : 0;
                if (nonPeakHourWindow == 0)
                    nonPeakHourWindow = UGITUtility.StringToInt(applicationContext.ConfigManager.GetValue("NonPeakHourWindow"));

                string workingWindowStartTime = formobj.ContainsKey(ReportScheduleConstant.WorkingWindowStartTime) ? Convert.ToString(formobj[ReportScheduleConstant.WorkingWindowStartTime]) : string.Empty;
                string workingWindowEndTime = formobj.ContainsKey(ReportScheduleConstant.WorkingWindowEndTime) ? Convert.ToString(formobj[ReportScheduleConstant.WorkingWindowEndTime]) : string.Empty;


                if (string.IsNullOrEmpty(workingWindowStartTime))
                    workingWindowStartTime = Convert.ToString(applicationContext.ConfigManager.GetValue("WorkdayStartTime"));


                if (string.IsNullOrEmpty(workingWindowEndTime))
                    workingWindowEndTime = Convert.ToString(applicationContext.ConfigManager.GetValue("WorkdayEndTime"));


            DataTable data = moduleStatistics.GetNonPeakHoursCount(moduleName, DateFrom, nonPeakHourWindow, workingWindowStartTime, workingWindowEndTime);
            HelpDeskPrfReportEntity entity = new HelpDeskPrfReportEntity();
            entity.Data = data;

            if (DateFrom != DateTime.MinValue)
                entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);

            NonPeakHoursPerformance_Report report = new NonPeakHoursPerformance_Report(entity);
            XtraReport reportNonPeakHours = report;
            return ReportHelper.ExportFiles(reportNonPeakHours, attachFormat, filePath, title);
            }
        }
    }