using System;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using uGovernIT.Manager.Reports;
using uGovernIT.Manager.Report.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Report.Helpers;

namespace uGovernIT.Report.DxReport
{
    public partial class NonPeakHoursPerformance_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public string workingWindowStartTime { get; set; }
        public string workingWindowEndTime { get; set; }
        public DateTime DateFrom { get; set; }
        public int NonPeakHourWindow { get; set; }
        ModuleStatistics moduleStatistics;
        public string Module { get; set; }
        public string delegateControl = UGITUtility.GetAbsoluteURL("BuildReport.aspx");
        ApplicationContext _context = System.Web.HttpContext.Current.GetManagerContext();
        public string ReportFilterURl;
        protected override void OnInit(EventArgs e)
        {
            //_context = Session["context"] as ApplicationContext;

            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            Module = Request["Module"];
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                //ReportFilterURl = _context.ReportUrl + delegateControl+ "?reportName=NonPeakHoursPerformance&Filter=Filter&Module=" + Module;
                ReportFilterURl = delegateControl + "?reportName=NonPeakHoursPerformance&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
            SelectedModule = Request["SelectedModule"];
            workingWindowStartTime = Request["workingWindowStartTime"];
            workingWindowEndTime = Request["workingWindowEndTime"];
            DateFrom = UGITUtility.StringToDateTime(Request["Date"]);
            NonPeakHourWindow = UGITUtility.StringToInt(Request["nonPeakHrWindow"]);
            moduleStatistics = new ModuleStatistics(_context);
            DataTable data = moduleStatistics.GetNonPeakHoursCount(SelectedModule, DateFrom,NonPeakHourWindow, workingWindowStartTime, workingWindowEndTime);
            HelpDeskPrfReportEntity entity = new HelpDeskPrfReportEntity();
            entity.Data = data;
            if (DateFrom != DateTime.MinValue)
                entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);
            NonPeakHoursPerformance_Report report = new NonPeakHoursPerformance_Report(entity);
            RptHelpDeskPerformanceReport.Report = report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
        }
        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("WeeklyTeamPerformance_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetTempFolderPathNew(), fileName));
                RptHelpDeskPerformanceReport.Report.ExportToPdf(path);

                e.Result = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string fileName = string.Format("WeeklyTeamPerformance_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetTempFolderPathNew(), fileName));

                RptHelpDeskPerformanceReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = uHelper.GetAbsoluteURL(DelegateControlsUrl.uploadDocument + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
    }
}
