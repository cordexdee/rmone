
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
namespace uGovernIT.DxReport
{
    public partial class NonPeakHoursPerformance_Filter : UserControl
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> ReportScheduleDict { get; set; }
        protected string ModuleName = "TSR";
        public string delegateControl;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public NonPeakHoursPerformance_Filter()
        {
            PopID = "aspxHelpDeskPerformance";
            Type = "runreport";
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            delegateControl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/BuildReport.aspx");
            string nonPeakHourWindow = context.ConfigManager.GetValue("NonPeakHourWindow");
            if (string.IsNullOrWhiteSpace(nonPeakHourWindow))
                nonPeakHourWindow = "2";
            txtNPHWindow.Text = nonPeakHourWindow;
            dtHelpDeskPerformance.Date=DateTime.Now;
            dtWorkingHoursStart.Value = Convert.ToDateTime(context.ConfigManager.GetValue("WorkdayStartTime"));
            dtWorkingHoursEnd.Value = Convert.ToDateTime(context.ConfigManager.GetValue("WorkdayEndTime"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context,false);
        }
    }
}
