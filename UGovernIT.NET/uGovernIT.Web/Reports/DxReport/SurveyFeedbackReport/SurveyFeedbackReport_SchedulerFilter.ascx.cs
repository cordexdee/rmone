using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.DxReport
{
    public partial class SurveyFeedbackReport_SchedulerFilter : ReportScheduleFilterBase
    {
        #region Properties
        public string type;
        protected bool isDataBind;
        public string selectedsurvey;
        public Dictionary<string, object> ReportScheduleDict { get; set; }
        #endregion
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        ServicesManager servicesManager;
        public long Id { get; set; }
        protected void Page_Init(object sender, EventArgs e)
        {
            servicesManager = new ServicesManager(applicationContext);
            dtFromtDatesurvey.Date = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            dtToDatesurvey.Date = DateTime.Now;
            spneditfrom.Text = "-7";
            spneditto.Text = "0";
            if (Request["ID"] != null)
            {
                Id = UGITUtility.StringToLong(Request["ID"]);
                scheduleID = Id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
               // ReportScheduleDict = uHelper.ReportScheduleDict;
              //  if (ReportScheduleDict != null && ReportScheduleDict.Count != 0 && ReportScheduleDict.ContainsKey(ReportScheduleConstant.Report) && Convert.ToString(ReportScheduleDict[ReportScheduleConstant.Report]) == "SurveyFeedbackReport")
                    FillForm();
            }
        }

        protected void ddlSurveyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSurveyType.SelectedItem.Text != "ALL")
                type = ddlSurveyType.SelectedItem.Text;

            //ClearDateFilter();
            LoadFeedbackSurvey(ddlSurveyType.SelectedItem.Text);
        }

        protected void ddlSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedsurvey = ddlSurvey.SelectedIndex > 0 ? ddlSurvey.SelectedItem.Text : string.Empty;
        }
        protected void ClearDateFilter()
        {
            //dtDateFrom.ClearSelection();
            //dtDateTo.ClearSelection();

        }
        private void LoadFeedbackSurvey(string selectedType)
        {
            DataTable surveydt = null;
            DataRow[] dataRow = null;
            //SPQuery query = new SPQuery();
            //List<string> fields = new List<string>();
            //fields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title));
            //fields.Add(string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Id));
            //fields.Add(string.Format("<FieldRef Name='{0}' LookupId='TRUE' />", DatabaseObjects.Columns.ModuleNameLookup));

            //query.ViewFields = string.Join("", fields.ToArray());
            //query.ViewFieldsOnly = true;
            //query.Query = string.Format("<Where><Eq><FieldRef Name='{1}'/><Value Type='Lookup'>{2}</Value></Eq></Where><OrderBy><FieldRef Name='{0}'/></OrderBy>", DatabaseObjects.Columns.Title, DatabaseObjects.Columns.ServiceCategoryNameLookup, Constants.ModuleFeedback);

            //surveydt = SPListHelper.GetDataTable(DatabaseObjects.Lists.Services, query);

            dataRow = servicesManager.GetDataTable().Select(string.Format("{0} like '%{1}%'", DatabaseObjects.Columns.ServiceType, Constants.ModuleFeedbackNoTilt));
            if (dataRow != null && dataRow.Length > 0)
            {
                surveydt = dataRow.CopyToDataTable();
                surveydt.DefaultView.Sort = DatabaseObjects.Columns.Title+" Asc";
                surveydt.Columns.Add(DatabaseObjects.Columns.ModuleName);
                //surveydt.Columns.Add(DatabaseObjects.Columns.ModuleId);

                Dictionary<string, string> dictemp = new Dictionary<string, string>();
                foreach (DataRow item in surveydt.Rows)
                {
                    string moduleLookup = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ModuleNameLookup));
                    if (!string.IsNullOrEmpty(moduleLookup))
                    {
                        item[DatabaseObjects.Columns.ModuleName] = moduleLookup;
                    }
                    else
                    {
                        item[DatabaseObjects.Columns.ModuleName] = "Generic";
                    }
                    //SPFieldLookupValue moduleLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(item, DatabaseObjects.Columns.ModuleNameLookup)));
                    //if (moduleLookup.LookupId > 0)
                    //{
                    //    item[DatabaseObjects.Columns.ModuleId] = moduleLookup.LookupId;
                    //    if (moduleLookup.LookupId == 0)
                    //        item[DatabaseObjects.Columns.ModuleName] = "Generic";
                    //    else
                    //        item[DatabaseObjects.Columns.ModuleName] = moduleLookup.LookupValue;
                    //}
                }
                string expresstion = string.Empty;
                DataRow[] dtrow;

                if (selectedType == "Generic")
                    dtrow = surveydt.Select(string.Format("ModuleName = 'Generic'"));
                else if (selectedType == "Module")
                    dtrow = surveydt.Select(string.Format("ModuleName <> 'Generic'"));
                else
                    dtrow = surveydt.Select();

                ddlSurvey.Items.Clear();

                if (dtrow != null && dtrow.Length > 0)
                {
                    ddlSurvey.DataSource = dtrow.CopyToDataTable();
                    ddlSurvey.TextField = "Title";
                    ddlSurvey.ValueField = "ID";
                    ddlSurvey.DataBind();
                }

                ddlSurvey.Items.Insert(0, new ListEditItem("--Select--", "0"));
                ddlSurvey.SelectedIndex = 0;
                if (ddlSurvey.Items.Count == 2 && !isDataBind)
                {
                    isDataBind = true;
                    ddlSurvey.SelectedIndex = ddlSurvey.Items.IndexOf(ddlSurvey.Items.FindByValue(ddlSurvey.Items[1].Value));
                    ddlSurvey_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> formdic = new Dictionary<string, object>();
            formdic.Add(ReportScheduleConstant.Report, TypeOfReport.SurveyFeedbackReport);
            formdic.Add(ReportScheduleConstant.Type, ddlSurveyType.Value == null ? "ALL" : ddlSurveyType.SelectedItem.Text);
            formdic.Add(ReportScheduleConstant.Module, ddlSurvey.Value == null ? "" : Convert.ToString(ddlSurvey.SelectedItem.Value));
            formdic.Add(ReportScheduleConstant.Survey, ddlSurvey.Value == null ? "ALL" : ddlSurvey.SelectedItem.Text);
            formdic.Add(ReportScheduleConstant.DateRange, string.Format("{0}{1}{2}{3}{4}",spneditfrom.Text,Constants.Separator1,spneditto.Text,Constants.Separator, ddlDateUnitsFrom.SelectedItem.Text));
            Filterproperties = formdic;
            SaveFilters();
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        private void FillForm()
        {
            Dictionary<string, object> formdic = LoadFilter(scheduleID);
            if (formdic == null || formdic.Count == 0)
            {
                SurveyFeedbackReport_Scheduler scheduler = new SurveyFeedbackReport_Scheduler();
                formdic = scheduler.GetDefaultData();
            }
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
                        spneditfrom.Text = arrDates[0];
                        spneditto.Text = arrDates[1];
                    }
                }
            }

            if (formdic.ContainsKey(ReportScheduleConstant.Type))
                ddlSurveyType.SelectedIndex = ddlSurveyType.Items.IndexOf(ddlSurveyType.Items.FindByValue(formdic["Type"]));

            if (ddlSurveyType.SelectedIndex != -1)
            {
                type = ddlSurveyType.SelectedItem.Text;
                LoadFeedbackSurvey(ddlSurveyType.SelectedItem.Text);
            }

            if (formdic.ContainsKey(ReportScheduleConstant.Module))
                ddlSurvey.SelectedIndex = ddlSurvey.Items.IndexOf(ddlSurvey.Items.FindByValue(formdic["Module"]));

            
            if (formdic.ContainsKey(ReportScheduleConstant.Survey))
                ddlSurvey.SelectedIndex = ddlSurvey.Items.IndexOf(ddlSurvey.Items.FindByText(Convert.ToString(formdic["Survey"])));
        }
    }
}
