
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;

namespace uGovernIT.Report.DxReport
{
    public partial class TicketSummary_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public string SelectedType { get; set; }
        public string SortType { get; set; }
        public string IsModuleSort { get; set; }
        public string StrDateFrom { get; set; }
        public string StrDateTo { get; set; }
        public string Module { get; set; }
        public string ReportFilterURl;
        public string delegateControl ="BuildReport.aspx";

        public ApplicationContext _context = null;

        public ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = System.Web.HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            paramRequired.Add("SelectedType");
            Module = Request["Module"];
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = ApplicationContext.SiteUrl + delegateControl + "?reportName=TicketSummary&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }

            SelectedModule = Request["SelectedModule"];
            SelectedType = Request["SelectedType"];
            SortType = Request["SortType"];
            IsModuleSort = Request["IsModuleSort"];
            StrDateFrom = Request["DateFrom"];
            StrDateTo = Request["DateTo"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            //_context = Session["context"] as ApplicationContext;
            
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            TicketStatus MTicketStatus = TicketStatus.All;

            if (SelectedType.ToLower() == "open")
            {
                MTicketStatus = TicketStatus.Open;
            }
            else if (SelectedType.ToLower() == "closed")
            {
                MTicketStatus = TicketStatus.Closed;
            }
            if (Request["DateFrom"] != null)
            {
                StrDateFrom = Request["DateFrom"];
            }
            if (Request["DateTo"] != null)
            {
                StrDateTo = Request["DateTo"];
            }
           TicketSummaryReportHelper tsrHelper = new TicketSummaryReportHelper(_context, SelectedModule, SortType, IsModuleSort, MTicketStatus, StrDateFrom, StrDateTo);

            var reports = tsrHelper.GetTicketSummaryReport();
            if (reports != null)
            {
                RptOpenTicketsReport.Report = reports;

            }


        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : string.Empty;
            string fileName = string.Format("Open_Ticket_Report_{0}", uHelper.GetCurrentTimestamp());
            string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/upload/{0}.pdf", fileName);
            string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
            if (e.Parameter == "SendMail")
            {
                //string fileName = string.Format("Open_Ticket_Report_{0}", uHelper.GetCurrentTimestamp());
                //string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/IMAGES/ugovernit/upload/{0}.pdf", fileName);
                //string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                RptOpenTicketsReport.Report.ExportToPdf(path);
                e.Result = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, "/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                //string fileName = string.Format("Task_Project_Report_{0}", uHelper.GetCurrentTimestamp());
                //string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/IMAGES/ugovernit/upload/{0}.pdf", fileName);
                //string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptOpenTicketsReport.Report.ExportToPdf(path);
                e.Result = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = uHelper.GetAbsoluteURL(DelegateControlsUrl.uploadDocument + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
       
    }
}
