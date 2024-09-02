
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Report.Intrastructure;

namespace uGovernIT.Report.DxReport
{
    public partial class NonPeakHoursPerformance_SchedulerFilter : ReportScheduleFilterBase
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public Dictionary<string, object> ReportScheduleDict { get; set; }
        protected string ModuleName = "TSR";
        protected string delegateControl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx");
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public NonPeakHoursPerformance_SchedulerFilter()
        {
            PopID = "aspxHelpDeskPerformance";
            Type = "runreport";
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            string nonPeakHourWindow = context.ConfigManager.GetValue("NonPeakHourWindow");
            if (string.IsNullOrWhiteSpace(nonPeakHourWindow))
                nonPeakHourWindow = "2";
            txtNPHWindow.Text = nonPeakHourWindow;
            //dtHelpDeskPerformance.SetDate(DateTime.Now);
            dtWorkingHoursStart.Value = UGITUtility.StringToDateTime(context.ConfigManager.GetValue("WorkdayStartTime"));
            dtWorkingHoursEnd.Value = UGITUtility.StringToDateTime(context.ConfigManager.GetValue("WorkdayEndTime"));
            txtValueFrom.Text = "-7";
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            spanNoOfDays.Visible = true;
            if (!IsPostBack)
            {
                    FillForm();
            }
        }
        private void FillForm()
        {
            string nonPeakHourWindow = context.ConfigManager.GetValue("NonPeakHourWindow");
            if (string.IsNullOrWhiteSpace(nonPeakHourWindow))
                nonPeakHourWindow = "2";

            Dictionary<string, object> formdic = LoadFilter(scheduleID);
            if (formdic == null || formdic.Count == 0)
            {
                NonPeakHoursPerformance_Scheduler scheduler = new NonPeakHoursPerformance_Scheduler();
                formdic = scheduler.GetDefaultData();
            }
            if (formdic.ContainsKey(ReportScheduleConstant.DateRange))
            {
                string dateRange = Convert.ToString(formdic[ReportScheduleConstant.DateRange]);
                string[] arrDateRange = dateRange.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                if (arrDateRange.Length> 1)
                {
                    ddlDateUnitsFrom.SelectedIndex = ddlDateUnitsFrom.Items.IndexOf(ddlDateUnitsFrom.Items.FindByTextWithTrim(arrDateRange[1]));
                    string[] arrDates = arrDateRange[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
                    if (arrDates.Length > 0)
                    {
                        txtValueFrom.Text = arrDates[0];
                    }
                }
            }
            txtNPHWindow.Text =!string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.NonPeakHourWindow]))? Convert.ToString(formdic[ReportScheduleConstant.NonPeakHourWindow]):nonPeakHourWindow;
            dtWorkingHoursStart.Text =!string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.WorkingWindowStartTime]))? Convert.ToString(formdic[ReportScheduleConstant.WorkingWindowStartTime]): Convert.ToString(context.ConfigManager.GetValue("WorkdayStartTime"));
            dtWorkingHoursEnd.Text= !string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.WorkingWindowEndTime])) ? Convert.ToString(formdic[ReportScheduleConstant.WorkingWindowEndTime]) : Convert.ToString(context.ConfigManager.GetValue("WorkdayEndTime"));
        }
        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.NonPeakHoursPerformance);
            formdic.Add(ReportScheduleConstant.Module, ModuleName);
            formdic.Add(ReportScheduleConstant.NonPeakHourWindow, txtNPHWindow.Text);
            formdic.Add(ReportScheduleConstant.WorkingWindowStartTime, dtWorkingHoursStart.Text);
            formdic.Add(ReportScheduleConstant.WorkingWindowEndTime, dtWorkingHoursEnd.Text);
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", txtValueFrom.Text, Constants.Separator1, "0", Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

    }
}
