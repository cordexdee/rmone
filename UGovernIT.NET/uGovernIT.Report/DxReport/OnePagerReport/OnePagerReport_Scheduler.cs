using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public class OnePagerReport_Scheduler : IReportScheduler
    {
        public Dictionary<string, object> GetDefaultData()
        {
            throw new NotImplementedException();
        }

        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
            string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
            string fileName = string.Empty;
            ProjectCompactReportEntity proEntity = new ProjectCompactReportEntity();
            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);
            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

            int[] PMMids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.ids]).Split(','), int.Parse);
            OnePagerReportHelper prHelper = new OnePagerReportHelper(applicationContext);
            proEntity = prHelper.GetOnePagerReportEntity();
            prHelper.PMMIds = PMMids;
            string reportFileName= string.Empty;
            if (proEntity.ProjectDetails.Rows.Count > 1)
            {
                MultiViewPagerReport report = new MultiViewPagerReport(proEntity);
                reportFileName = ReportHelper.ExportFiles(report, attachFormat, filePath, title);
            }
            else
            {
                ProjectCompactReport report = new ProjectCompactReport(proEntity);
                reportFileName = ReportHelper.ExportFiles(report, attachFormat, filePath, title);
            }

            return reportFileName;
        }
    }
}