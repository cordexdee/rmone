using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using uGovernIT.Helpers;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Report.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Report.Helpers;

namespace uGovernIT.Report.DxReport
{
    public partial class WeeklyTeamReport_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string SelectedCategories { get; set; }
        public string Module { get; set; }
        ModuleStatistics moduleStatistics;
        public string delegateControl = "BuildReport.aspx";
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public string ReportFilterURl;
        protected override void OnInit(EventArgs e)
        {
            //_context = Session["context"] as ApplicationContext;
            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            Module = Request["Module"];
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = _context.SiteUrl + delegateControl + "?reportName=WeeklyTeamReport&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
            SelectedModule = Request["SelectedModule"];
            DateFrom = UGITUtility.StringToDateTime(Request["DateFrom"]);
            DateTo = UGITUtility.StringToDateTime(Request["DateTo"]);
            SelectedCategories = Request["Categories"];
            moduleStatistics = new ModuleStatistics(_context);
            DataTable data = moduleStatistics.GetWeeklyTeamPrfCount(SelectedModule, DateFrom, DateTo,SelectedCategories);
            WeeklyTeamPrfReportEntity entity = new WeeklyTeamPrfReportEntity();
            entity.Data = data;
         
            if (DateFrom != DateTime.MinValue)
                entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);
            if (DateTo != DateTime.MinValue)
                entity.EndDate = UGITUtility.GetDateStringInFormat(DateTo, false);
            WeeklyTeamReport_Report report = new WeeklyTeamReport_Report(entity);
           //WeeklyTeamPerformanceReport report = new WeeklyTeamPerformanceReport(entity);
            RptWeeklyTeamPerformanceReport.Report = report;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
        }
        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
           
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("WeeklyTeamPerformance_Report_{0}", UGITUtility.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content//images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetTempFolderPathNew(), fileName));
                RptWeeklyTeamPerformanceReport.Report.ExportToPdf(path);

                e.Result =UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string fileName = string.Format("WeeklyTeamPerformance_Report_{0}", UGITUtility.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetTempFolderPathNew(), fileName));

                RptWeeklyTeamPerformanceReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = uHelper.GetAbsoluteURL(DelegateControlsUrl.uploadDocument + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
    }
}
