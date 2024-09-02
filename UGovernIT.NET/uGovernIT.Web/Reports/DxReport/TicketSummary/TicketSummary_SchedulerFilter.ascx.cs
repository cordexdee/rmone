
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
namespace uGovernIT.DxReport
{
    public partial class TicketSummary_SchedulerFilter : ReportScheduleFilterBase
    {
        public bool IsEdit { get; set; }
        public long Id { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        ModuleViewManager viewManager;
        protected override void OnInit(EventArgs e)
        {
            if (Request["Edit"] != null)
            {
                IsEdit = Convert.ToBoolean(Request["Edit"]);
            }
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
            FillModules();
            FillForm();
            base.OnInit(e);
        }

        private void FillForm()
        {
            Dictionary<string, object> formdic = LoadFilter(scheduleID);
            if (formdic == null || formdic.Count == 0)
            {
                TicketSummary_Scheduler scheduler = new TicketSummary_Scheduler();
                formdic = scheduler.GetDefaultData();
            }
            if (formdic != null && formdic.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(formdic[ReportScheduleConstant.Report]) == TypeOfReport.TicketSummary.ToString())
            {
                ddlModules.SelectedValue = Convert.ToString(formdic[ReportScheduleConstant.Module]);
                chkSortByModule.Checked = Convert.ToBoolean(formdic[ReportScheduleConstant.SortByModule]);
                rdbtnLstFilterCriteria.SelectedValue = Convert.ToString(formdic[ReportScheduleConstant.TicketStatus]);
                rdSortCriteria.SelectedValue = Convert.ToString(formdic[ReportScheduleConstant.SortBy]);
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
                //if (!string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.FromDate])))
                //{
                //    dtFromTicketSummary.Date = Convert.ToDateTime(formdic[ReportScheduleConstant.FromDate]);
                //}
                //if (!string.IsNullOrEmpty(Convert.ToString(formdic[ReportScheduleConstant.ToDate])))
                //{
                //    dtToTicketSummary.Date = Convert.ToDateTime(formdic[ReportScheduleConstant.ToDate]);
                //}
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void FillModules()
        {
            viewManager = new ModuleViewManager(_context);
            DataTable dataTable = new DataTable();
            DataRow[] smsModules = viewManager.GetDataTable().Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleType, ModuleType.SMS));

            if (smsModules != null)
            {
                dataTable = smsModules.CopyToDataTable();
                ddlModules.DataSource = dataTable;
                ddlModules.DataTextField = DatabaseObjects.Columns.Title;
                ddlModules.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModules.DataBind();
                ddlModules.Items.Insert(0, new ListItem("All", "All"));
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.TicketSummary);
            formdic.Add(ReportScheduleConstant.Module, ddlModules.SelectedValue);
            formdic.Add(ReportScheduleConstant.SortByModule, chkSortByModule.Checked);
            formdic.Add(ReportScheduleConstant.TicketStatus, rdbtnLstFilterCriteria.SelectedValue);
            formdic.Add(ReportScheduleConstant.SortBy, rdSortCriteria.SelectedValue);
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}", txtValueFrom.Text, Constants.Separator1, txtValueTo.Text, Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            //formdic.Add(ReportScheduleConstant.FromDate, dtFromTicketSummary == null ? "" : dtFromTicketSummary.Date.ToString());
            //formdic.Add(ReportScheduleConstant.ToDate, dtToTicketSummary == null ? "" : dtToTicketSummary.Date.ToString());
            //uHelper.ReportScheduleDict = formdic;
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

    }
}
