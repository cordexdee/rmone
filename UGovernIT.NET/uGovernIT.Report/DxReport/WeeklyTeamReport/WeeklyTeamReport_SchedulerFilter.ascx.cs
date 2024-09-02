
using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Report.Intrastructure;
using System.Linq;

namespace uGovernIT.Report.DxReport
{
    public partial class WeeklyTeamReport_SchedulerFilter : ReportScheduleFilterBase
    {
        public string PopID { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        ModuleViewManager moduleViewManager;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        public Dictionary<string,object> ReportScheduleDict { get; set; }
        public WeeklyTeamReport_SchedulerFilter()
        {
            PopID = "aspxWeeklyPrfReport";
            Type = "runreportsdsd";

        }
        public string ModuleName = "TSR";
        public string delegateControl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx");
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
            //dtFromtDate.Date = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            // dtToDate.Date = DateTime.Now;
            txtValueFrom.Text = "-7";
            txtValueTo.Text = "0";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            lnkSubmit.Visible = false;
            spanNoOfDays.Visible = true;
                lnkSubmit.Visible = true;
            if(!IsPostBack)
            {
                FillForm();
              
            }
           
        }
        private void FillForm()
        {
            Dictionary<string, object> formdic = LoadFilter(scheduleID);
            if (formdic == null || formdic.Count == 0)
            {
                TicketSummary_Scheduler scheduler = new TicketSummary_Scheduler();
                formdic = scheduler.GetDefaultData();
            }
            if (formdic != null && formdic.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(formdic[ReportScheduleConstant.Report]) == TypeOfReport.WeeklyTeamReport.ToString())
            {
                glCategories.Text = Convert.ToString(formdic[ReportScheduleConstant.Category]);
                if (formdic.ContainsKey(ReportScheduleConstant.DateRange))
                {
                    string dateRange = Convert.ToString(formdic[ReportScheduleConstant.DateRange]);
                    string[] arrDateRange = dateRange.Split(new string[] { Constants.Separator }, StringSplitOptions.None);
                    if (arrDateRange.Length > 1)
                    {
                        ddlDateUnitsFrom.SelectedIndex = ddlDateUnitsFrom.Items.IndexOf(ddlDateUnitsFrom.Items.FindByTextWithTrim(arrDateRange[1]));
                        string[] arrDates = arrDateRange[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.None);
                        if (arrDates.Length > 1)
                        {
                            txtValueFrom.Text = arrDates[0];
                            txtValueTo.Text = arrDates[1];
                        }
                    }
                }
            }
        }
        protected void glCategories_Init(object sender, EventArgs e)
        {
            FillCategories();
            glCategories.DataBind();
        }
       
        private void FillCategories()
        {
            moduleViewManager = new ModuleViewManager(_context);
            UGITModule module = moduleViewManager.LoadByName(ModuleName); //uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, ModuleName);
            if (module == null)
                return;
            List<ModuleRequestType> requestType = module.List_RequestTypes;
            if (requestType != null && requestType.Count > 0)
            {
                string[] requestTypeCategories = requestType.Where(x => !x.Deleted).Select(x => x.Category).OrderBy(x => x).Distinct().ToArray();
                if (requestTypeCategories.Length > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(DatabaseObjects.Columns.Category);
                    foreach (string item in requestTypeCategories)
                    {
                        DataRow dr = dt.NewRow();
                        dr[DatabaseObjects.Columns.Category] = item;
                        dt.Rows.Add(dr);
                    }
                    glCategories.DataSource = dt;
                }
            }
        }

        protected void cbpnlMain_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.WeeklyTeamReport);
            formdic.Add(ReportScheduleConstant.Module, ModuleName);
            formdic.Add(ReportScheduleConstant.Category, glCategories.Text);
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", txtValueFrom.Text, Constants.Separator1, txtValueTo.Text, Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            Filterproperties = formdic;
            SaveFilters();
            // uHelper.ReportScheduleDict = formdic;
            uHelper.ClosePopUpAndEndResponse(Context,false);
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.WeeklyTeamReport);
            formdic.Add(ReportScheduleConstant.Module, ModuleName);
            formdic.Add(ReportScheduleConstant.Category, glCategories.Text);
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", txtValueFrom.Text, Constants.Separator1, txtValueTo.Text, Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            Filterproperties = formdic;
            SaveFilters();
            // uHelper.ReportScheduleDict = formdic;
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
