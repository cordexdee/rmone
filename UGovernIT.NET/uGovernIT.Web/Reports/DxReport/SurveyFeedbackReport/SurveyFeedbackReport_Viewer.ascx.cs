
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using uGovernIT.Manager;
using System.Text;
using uGovernIT.Utility;
using System.Web;
namespace uGovernIT.DxReport
{
    public partial class SurveyFeedbackReport_Viewer : UserControl
    {
        public string TicketId { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string Selectedsurvey { get; set; }
        public string Surveyoftype { get; set; }
        public string Survey { get; set; }
        public string DocName { get; set; }
        public string FolderGuid { get; set; }
        public string SelectFolder { get; set; }
        public string PathValue { get; set; }
        public string stageName { get; set; }

        ServicesManager servicesManager;
        SurveyFeedbackManager surveyFeedbackManager;
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        public string FilterExp { get; set; }
        protected override void OnInit(EventArgs e)
        {

            servicesManager = new ServicesManager(applicationContext);
            surveyFeedbackManager = new SurveyFeedbackManager(applicationContext);
            Fromdate = string.Empty;
            if (!string.IsNullOrEmpty(Request["from"]))
                Fromdate = Request["from"];
            Todate = string.Empty;
            if (!string.IsNullOrEmpty(Request["to"]))
                Todate = Request["to"];
            Selectedsurvey = string.Empty;
            if (!string.IsNullOrEmpty(Request["selectedsurvey"]))
                Selectedsurvey = Request["selectedsurvey"];
            Surveyoftype = string.Empty;
            if (!string.IsNullOrEmpty(Request["type"]))
                Surveyoftype = Request["type"];
            Survey = string.Empty;
            if (!string.IsNullOrEmpty(Request["survey"]))
                Survey = Request["survey"];
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //SPDelta 155(Start:-Survey complete functionality)
            hdnconfiguration.Set("RequestUrl", Request.Url.AbsolutePath.Replace("/Report", ""));
            //SPDelta 155(End:-Survey complete functionality)
            if (string.IsNullOrEmpty(Selectedsurvey))
                return;
            SurveyFeedbackReport_SurveyHelper helper = new SurveyFeedbackReport_SurveyHelper(applicationContext,Selectedsurvey,fromDate:Fromdate,toDate:Todate);
            helper.FilterExpression = FilterExp;
            SurveyFeedbackReport_Report surveyreportobj = new SurveyFeedbackReport_Report(helper.LoadFeedbackData(), Surveyoftype, Survey);
            //Test obj = new Test(LoadFeedbackData());
            RptVwrProjectReport.Report = surveyreportobj;
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                string fileName = string.Format("Survey_Feedback_Report_{0}_{1}", Surveyoftype, uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));
                RptVwrProjectReport.Report.ExportToPdf(path);

                e.Result = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=ticketemail&type=surveyfeedbackreport&localpath=" + path + "&relativepath=" + uploadFileURL);
               
            }
            else if (e.Parameter == "SaveToDoc")
            {
                string typeparam = "multiprojectreport";
                string fileName = string.Format("Survey_Feedback_Report_{0}", uHelper.GetCurrentTimestamp());
                string uploadFileURL = string.Format("/Content/images/uploadedfiles/{0}.pdf", fileName);
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                RptVwrProjectReport.Report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&type=" + typeparam + "&localpath=" + path + "&relativepath=" + uploadFileURL + "&DocName=" + DocName + "&folderid=" + FolderGuid);
            }

        }
    }
}
