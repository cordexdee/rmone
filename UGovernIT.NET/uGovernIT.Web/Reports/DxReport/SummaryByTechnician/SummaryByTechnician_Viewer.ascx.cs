using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Manager.Reports;
using uGovernIT.Entities;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.DxReport;
using System.Web;
using System.Collections.Specialized;
using uGovernIT.Helpers;

namespace uGovernIT.ControlTemplates.uGovernIT
{
    public partial class SummaryByTechnician_Viewer : UserControl
    {
        public string SelectedModule { get; set; }
        public string IncludeCounts { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool GroupByCategory { get; set; }
        public bool IsModuleSort { get; set; }
        public bool IncludeTechnician { get; set; }
        public string SelectedITManagers { get; set; }
        public string Module { get; set; }
        ApplicationContext _context = System.Web. HttpContext.Current.GetManagerContext();
        SummaryByTechnician_Report report;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        public string ReportFilterURl;
        ModuleViewManager moduleViewManager;

        protected override void OnInit(EventArgs e)
        {
            //_context = Session["context"] as ApplicationContext;
            _context = HttpContext.Current.GetManagerContext();

            moduleViewManager = new ModuleViewManager(_context);
            List<string> paramRequired = new List<string>();
            paramRequired.Add("SelectedModule");
            paramRequired.Add("IncludeCounts");
            paramRequired.Add("DateFrom");
            paramRequired.Add("DateTo");
            Module = Request["Module"];
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) ==false)
            {
                ReportFilterURl = delegateControl + "?reportName=SummaryByTechnician&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
            SelectedModule = Request["SelectedModule"];
            IsModuleSort = UGITUtility.StringToBoolean(Request["IsModuleSort"]);
            IncludeTechnician = UGITUtility.StringToBoolean(Request["IncludeTechnician"]);
            IncludeCounts = Request["IncludeCounts"];
            GroupByCategory = UGITUtility.StringToBoolean(Request["GroupByCategory"]);
            SelectedITManagers = Request["SelectedITManagers"];
            DateFrom = UGITUtility.StringToDateTime(Request["DateFrom"]);
            DateTo = UGITUtility.StringToDateTime(Request["DateTo"]);
            ModuleStatistics moduleStatistics = new ModuleStatistics(_context);
            List<string> status =  UGITUtility.ConvertStringToList(IncludeCounts, ",");
            DataTable data = moduleStatistics.GetTicketsCountByPRP(SelectedModule, GroupByCategory, status, DateFrom, DateTo, IsModuleSort,IncludeTechnician, SelectedITManagers);

            TicketSummaryByPRPEntity entity = new TicketSummaryByPRPEntity();
            string prms = string.Format("{0}='{1}' AND {2}='{3}' AND {4} IN ('{5}') AND {6} = '{7}'", DatabaseObjects.Columns.ShowTicketSummary, 1, 
                DatabaseObjects.Columns.EnableModule, 1, DatabaseObjects.Columns.ModuleName, SelectedModule.Replace(",", "','"), 
                DatabaseObjects.Columns.TenantID, _context.TenantID);
            DataTable dtModules = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, prms, DatabaseObjects.Columns.Title, null);
            if (dtModules.Rows.Count > 0)
            {
                //var modulelist = dtModules.AsEnumerable().Select(r => r[DatabaseObjects.Columns.Title].ToString());
                var modulelist = dtModules.AsEnumerable().Select(r => r[DatabaseObjects.Columns.Title].ToString());
                entity.ModuleName = SelectedModule + ";" + string.Join(",", modulelist);
            }
            else
                entity.ModuleName = SelectedModule;
            entity.IsModuleSort = IsModuleSort;
            entity.IncludeTechnician = IncludeTechnician;
            entity.IncludeCounts = IncludeCounts;    //"Assigned,Closed,OnHold";
            entity.Data = data;
            entity.GroupByCategory =GroupByCategory;
            if (DateFrom != DateTime.MinValue)
                entity.StartDate = UGITUtility.GetDateStringInFormat(DateFrom, false);
            if (DateTo != DateTime.MinValue)
                entity.EndDate = UGITUtility.GetDateStringInFormat(DateTo, false);
              report = new SummaryByTechnician_Report(entity);
            if (RptOpenTicketsReport != null)
                RptOpenTicketsReport.Report = report;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            if (RptOpenTicketsReport != null)
                RptOpenTicketsReport.Report = report;
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : string.Empty;
            string fileName = string.Format("TicketCountByPRP_Report_{0}", uHelper.GetCurrentTimestamp());
            string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/ugovernit/upload/{0}.pdf", fileName);
            string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
            if (e.Parameter == "SendMail")
            {
                RptOpenTicketsReport.Report.ExportToPdf(path);
                e.Result = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, "/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL); 
                //UGITUtility.GetAbsoluteURL("~/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=openTicketReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                RptOpenTicketsReport.Report.ExportToPdf(path);
                e.Result = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
                //e.Result = uHelper.GetAbsoluteURL(DelegateControlsUrl.uploadDocument + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            //if (RptOpenTicketsReport != null)
            //    RptOpenTicketsReport.Report = report;
            //base.OnPreRender(e);
        }
    }
}
