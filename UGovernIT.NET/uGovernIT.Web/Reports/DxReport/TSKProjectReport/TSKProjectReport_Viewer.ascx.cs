
using System;
using System.Data;
using System.Web.UI;
using uGovernIT.Helpers;
using uGovernIT.Manager.Entities;
using uGovernIT.Manager.Reports;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraReports.UI;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.DxReport;

namespace uGovernIT.ControlTemplates.uGovernIT
{
    public partial class TSKProjectReport_Viewer : UserControl
    {
        public int[] TSKIds { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowSummaryGanttChart { get; set; }
        public bool ShowAllTask { get; set; }
        public bool ShowMilestone { get; set; }
        public bool ShowDeliverable { get; set; }
        public bool ShowReceivable { get; set; }
        public bool ShowProjectRoles { get; set; }
        public bool ShowProjectDescription { get; set; }
        public string Module { get; set; }
        private ProjectReportHelper prHelper;
        public string delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
        ApplicationContext _context = System.Web.HttpContext.Current.GetManagerContext();
        public string ReportFilterURl;
        protected override void OnLoad(EventArgs e)
        {
            string eventArgument = Request.Params["__EVENTARGUMENT"];
            if (eventArgument == "saveToWindow=format:xls;" || eventArgument == "saveToDisk=format:xls;"
             || eventArgument == "saveToWindow=format:xlsx;" || eventArgument == "saveToDisk=format:xlsx;"
             || eventArgument == "saveToWindow=format:png;" || eventArgument == "saveToDisk=format:png;")
            {
                //if (prHelper.sourcetable != null && prHelper.sourcetable.Rows.Count == 1)
                //{
                //    RptVwrProjectReport.Report.Bands[BandKind.PageHeader].Visible = false;
                //}
            }
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {
            ShowStatus= UGITUtility.StringToBoolean(Request["Status"]);
            ShowSummaryGanttChart = UGITUtility.StringToBoolean(Request["SGC"]);
            ShowMilestone = UGITUtility.StringToBoolean(Request["SMS"]);
            ShowAllTask = UGITUtility.StringToBoolean(Request["SAT"]);
            ShowDeliverable= UGITUtility.StringToBoolean(Request["SKD"]);
            ShowReceivable = UGITUtility.StringToBoolean(Request["SKR"]);
            ShowProjectRoles = UGITUtility.StringToBoolean(Request["ProjectRoles"]);
            ShowProjectDescription = UGITUtility.StringToBoolean(Request["ProjectDesc"]);
            List<string> paramRequired = new List<string>();
            paramRequired.Add("TSKIds");
            paramRequired.Add("ShowStatus");
            paramRequired.Add("ShowSummaryGanttChart");
            paramRequired.Add("ShowAllTask");
            paramRequired.Add("ShowMilestone");
            paramRequired.Add("ShowDeliverable");
            paramRequired.Add("ShowReceivable");
            paramRequired.Add("ShowProjectRoles");
            paramRequired.Add("ShowProjectDescription");
            Module = Request["Module"];
            if (Request["TSKIds"] != null)
            {
                if (!string.IsNullOrEmpty(Request["TSKIds"]))
                {
                    TSKIds = Array.ConvertAll<string, int>(Request["TSKIds"].Split(','), int.Parse);
                }
            }
            if (ReportHelper.CheckFilterValues(Request.QueryString, paramRequired) == false)
            {
                ReportFilterURl = delegateControl + "?reportName=TSKProjectReport&Filter=Filter&Module=" + Module;
                Response.Redirect(ReportFilterURl);
            }
            //hdnConfiguration.Set("SendEmailUrl", uHelper.GetAbsoluteURL("/_layouts/15/ugovernit/DelegateControl.aspx?control=ticketemail&type=tskReport&localpath=" + path + "&relativepath=" + uploadFileURL));
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);

            //SPListItem pmmItem = SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMTicket, PMMId);
            //  DataRow module = uGITCache.GetModuleDetails("TSK");

            TSKProjectReportEntity prEntity = new TSKProjectReportEntity();
            prEntity.ShowStatus = ShowStatus;
            prEntity.ShowSummaryGanttChart =ShowSummaryGanttChart;
            prEntity.ShowAllTask = ShowAllTask;
            prEntity.ShowMilestone = ShowMilestone;
            prEntity.ShowDeliverable = ShowDeliverable;
            prEntity.ShowReceivable = ShowReceivable;
            prEntity.ShowProjectRoles = ShowProjectRoles;
            prEntity.ShowProjectDescription = ShowProjectDescription;
            prEntity.IsPMM = false;


            prHelper = new ProjectReportHelper();
            prEntity = prHelper.GetTSKProjectsEntity(_context, prEntity, TSKIds);

            if (prEntity.Projects.Rows.Count > 1)
            {
                ASPxSplitter1.Panes[0].Collapsed = false;
                MultiProReport proReport = new MultiProReport(prEntity);
                RptVwrProjectReport.Report = proReport;
            }
            else
            {
                ASPxSplitter1.Panes[0].Collapsed = true;
                ProjectReport proReport = new ProjectReport(prEntity);
                RptVwrProjectReport.Report = proReport;
            }

            pnlReport.Visible = true;

            base.OnInit(e);
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Task_Project_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

               // RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=tskReport&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string fileName = string.Format("Task_Project_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format(ReportHelper.GetReportSubFolder() + "/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

               // RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }
     
    }

}
