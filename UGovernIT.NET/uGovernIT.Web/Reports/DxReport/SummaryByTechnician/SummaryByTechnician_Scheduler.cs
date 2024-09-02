using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Entities;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.DxReport
{
    public class SummaryByTechnician_Scheduler : IReportScheduler
    {
        ModuleViewManager objModuleViewManager;
        public SummaryByTechnician_Scheduler()
        {

        }
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.SummaryByTechnician);
            formdic.Add(ReportScheduleConstant.Module, "All");
            formdic.Add(ReportScheduleConstant.GroupByCategory, false);
            formdic.Add(ReportScheduleConstant.IncludeORP, false);
            formdic.Add(ReportScheduleConstant.IncludeCounts, "All");
            formdic.Add(ReportScheduleConstant.FromDate, "");
            formdic.Add(ReportScheduleConstant.ToDate, "");
            formdic.Add(ReportScheduleConstant.ITManagers, "All");
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", "-7", Constants.Separator1, "0", Constants.Separator, "Days"));
            formdic.Add(ReportScheduleConstant.SortByModule, true);
            return formdic;
        }
        /// <summary>
        /// Gets the ticket summary by technician report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        public string GetReport(ApplicationContext _context, Dictionary<string, object> formobj, string attachFormat)
        {
            try
            {
                objModuleViewManager = new ModuleViewManager(_context);
                ModuleStatistics moduleStatistics = new ModuleStatistics(_context);
                string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
                string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();
                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

                string moduleName = formobj.ContainsKey(ReportScheduleConstant.Module) ? Convert.ToString(formobj[ReportScheduleConstant.Module]) : string.Empty;
                if (moduleName.ToLower() == "all")
                {
                    DataTable dtModules = objModuleViewManager.GetDataTable();
                    if (dtModules != null && dtModules.Rows.Count != 0)
                    {
                        var modules = dtModules.AsEnumerable()
                                          .Where(x => !x.IsNull(DatabaseObjects.Columns.ShowTicketSummary)
                                                    && x.Field<string>(DatabaseObjects.Columns.ShowTicketSummary).Equals("True")
                                                    && !x.IsNull(DatabaseObjects.Columns.EnableModule)
                                                    && x.Field<string>(DatabaseObjects.Columns.EnableModule).Equals("True"));

                        moduleName = string.Join(",", modules.Select(x => x.Field<string>(DatabaseObjects.Columns.ModuleName)).ToList());
                    }
                }

                bool groupByCategory = formobj.ContainsKey(ReportScheduleConstant.GroupByCategory) ? UGITUtility.StringToBoolean(formobj[ReportScheduleConstant.GroupByCategory]) : false;
                bool includeORP = formobj.ContainsKey(ReportScheduleConstant.IncludeORP) ? UGITUtility.StringToBoolean(formobj[ReportScheduleConstant.IncludeORP]) : false;
                bool sortByModule = formobj.ContainsKey(ReportScheduleConstant.SortByModule) ? UGITUtility.StringToBoolean(formobj[ReportScheduleConstant.SortByModule]) : false;
                string includeCounts = formobj.ContainsKey(ReportScheduleConstant.IncludeCounts) ? Convert.ToString(formobj[ReportScheduleConstant.IncludeCounts]) : string.Empty;
                if (includeCounts.ToLower() == "all")
                {
                    includeCounts = string.Empty;
                    string[] moduleNames = moduleName.Split(',');
                    foreach (string strmodule in moduleNames)
                    {
                        UGITModule module = objModuleViewManager.LoadByName(strmodule);
                        if (module != null)
                        {
                            LifeCycle lifeCycle = module.List_LifeCycles.FirstOrDefault();
                            if (lifeCycle != null)
                            {
                                List<LifeCycleStage> stages = lifeCycle.Stages.Where(x => x.StageTypeChoice == StageType.Assigned.ToString() || x.StageTypeChoice == StageType.Closed.ToString()).ToList();
                                foreach (string item in stages.Select(x => x.Name))
                                {
                                    if (!includeCounts.Contains(item))
                                    {
                                        if (!string.IsNullOrEmpty(includeCounts))
                                            includeCounts += "," + item;
                                        else
                                            includeCounts = item;
                                    }
                                }

                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(includeCounts))
                        includeCounts += ",OnHold";
                    else
                        includeCounts += "OnHold";
                }

                string itManagers = formobj.ContainsKey(ReportScheduleConstant.ITManagers) ? Convert.ToString(formobj[ReportScheduleConstant.ITManagers]) : string.Empty;
                DateTime DateFrom = new DateTime();
                DateTime DateTo = new DateTime();
                if (formobj.ContainsKey(ReportScheduleConstant.DateRange))
                {
                    string dateRange = Convert.ToString(formobj[ReportScheduleConstant.DateRange]);
                    Dictionary<string, DateTime> dic = uHelper.GetReportScheduleDates(_context, dateRange);
                    if (dic.Count > 0)
                    {
                        DateFrom = dic["DateFrom"];
                        DateTo = dic["DateTo"];
                    }
                }
                DataTable data = moduleStatistics.GetTicketsCountByPRP(moduleName, groupByCategory, UGITUtility.ConvertStringToList(includeCounts, ","), DateFrom, DateTo, sortByModule, includeORP, itManagers);
                TicketSummaryByPRPEntity entity = new TicketSummaryByPRPEntity();
                entity.Data = data;
                entity.ModuleName = moduleName;
                entity.GroupByCategory = groupByCategory;
                if (DateFrom != DateTime.MinValue)
                    entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);
                if (DateTo != DateTime.MinValue)
                    entity.EndDate = UGITUtility.GetDateStringInFormat(DateTo, false);
                SummaryByTechnician_Report reports = new SummaryByTechnician_Report(entity);
                XtraReport report = reports;
                return ReportHelper.ExportFiles(report, attachFormat, filePath, title);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex.ToString());
            }
            return null;
        }

    }
}