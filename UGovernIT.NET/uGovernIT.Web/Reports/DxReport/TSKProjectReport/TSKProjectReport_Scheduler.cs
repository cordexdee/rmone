
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using uGovernIT.Manager;
using uGovernIT.Manager.Reports;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Helpers;
using uGovernIT.DxReport;

namespace uGovernIT.Web
{
    public class TSKProjectReport_Scheduler : IReportScheduler
    {
        public TSKProjectReport_Scheduler()
        {

        }
        public Dictionary<string, object> GetDefaultData()
        {
            throw new NotImplementedException();
        }

        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
                string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
                string fileName = string.Empty;
                TSKProjectReportEntity proEntity = new TSKProjectReportEntity();
                proEntity.ShowStatus = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowStatus]);
                proEntity.ShowProjectRoles = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectRoles]);
                proEntity.ShowProjectDescription = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectDescription]);
                proEntity.ShowReceivable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowReceivable]);
                proEntity.ShowDeliverable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowDeliverable]);
                proEntity.ShowMilestone = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMilestone]);
                proEntity.ShowAllTask = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAllTask]);
                proEntity.ShowSummaryGanttChart = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowSummaryGanttChart]);

                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);
                string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

                int[] PMMids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.ids]).Split(','), int.Parse);
            ProjectReportHelper prHelper = new ProjectReportHelper();
            proEntity = prHelper.GetTSKProjectsEntity(applicationContext,proEntity, PMMids);

            MultiProReport multiproject = new MultiProReport(proEntity);

            return ReportHelper.ExportFiles(multiproject, attachFormat, filePath, title);
            }
        }
    }