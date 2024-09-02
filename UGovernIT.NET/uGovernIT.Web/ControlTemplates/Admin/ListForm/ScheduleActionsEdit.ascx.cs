using DevExpress.Web;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using System.Text.RegularExpressions;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ScheduleActionsEdit : UserControl
    {
        public int requestId { get; set; }
        SchedulerAction spListitem;
        public FormMode FormMode { get; set; }
        QueryParameters _QueryParameter = null;
        HtmlEditorControl _HtmlEditorControl = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ScheduleActionsManager scheduleActionsManager;
        DashboardManager dashboardManager;
        //ReportConfigData reportConfigData;
        ReportConfigManager reportConfigManager;
        TicketTemplateManager TemplateManager = new TicketTemplateManager(HttpContext.Current.GetManagerContext());

        protected override void OnInit(EventArgs e)
        {
            reportConfigManager = new ReportConfigManager(context);
            scheduleActionsManager = new ScheduleActionsManager(context);
            hdnConfiguration.Set("NonPeakHoursPerformance", string.Format("{0}SchedulerReport.aspx?reportName=NonPeakHoursPerformance&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("ProjectReport", string.Format("{0}SchedulerReport.aspx?reportName=ProjectReport&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("TSKProjectReport", string.Format("{0}SchedulerReport.aspx?reportName=TSKProjectReport&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("SummaryByTechnician", string.Format("{0}SchedulerReport.aspx?reportName=SummaryByTechnician&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("SurveyFeedbackReport", string.Format("{0}SchedulerReport.aspx?reportName=SurveyFeedbackReport&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("TaskSummary", string.Format("{0}SchedulerReport.aspx?reportName=TaskSummary&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("TicketSummary", string.Format("{0}SchedulerReport.aspx?reportName=TicketSummary&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));
            hdnConfiguration.Set("WeeklyTeamReport", string.Format("{0}SchedulerReport.aspx?reportName=WeeklyTeamReport&Edit={1}&ID={2}", context.SiteUrl, requestId > 0 ? "true" : "false", requestId > 0 ? requestId : 0));

            hdnConfiguration.Set("GanttView", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ganttview&Edit=" + (requestId > 0 ? "true" : "false")));
            hdnConfiguration.Set("QueryPreview", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=querywizardpreview"));
            hdnConfiguration.Set("QueryParameter", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=queryparameter&QueryId={0}&Showvalues=true&WhereFilter={1}&frameObjId={2}"));
            hdnConfiguration.Set("TicketPicker", UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=listpicker&pageTitle=Picker List&IsDlg=1&Module={0}&TicketId={1}&Type={2}&&ControlId={3}"));
            hdnConfiguration.Set("TicketUrl", "");
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            _HtmlEditorControl = (HtmlEditorControl)Page.LoadControl("~/controltemplates/uGovernIT/HtmlEditorControl.ascx");

            tdHtmlEditor.Controls.Add(_HtmlEditorControl);

            FillReportType();
            BindModule();
            FillQuery();

            //Recurring option
            FillDaysOfMonth(ddlDayOfMonth, 30);
            FillDaysOfMonth(ddlBusinessDayOfMonth, 20);
            LoadControl();
            base.OnInit(e);
        }

        private void LoadControl()
        {
            if (requestId > 0)
            {
                spListitem = scheduleActionsManager.LoadByID(requestId);
                if (spListitem != null)
                {
                    FillForm();
                    lnkDelete.Visible = true;
                }
            }
            else
            {
                lnkDelete.Visible = false;
                spListitem = new SchedulerAction();
                ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), ddlScheduleActionType.SelectedValue);

                trCondition.Visible = trParameter.Visible = trAttachformat.Visible = trQuery.Visible = trReportType.Visible = trTicketPicker.Visible = trmodule.Visible = trtemplate.Visible = trsendnotification.Visible = false;
                if (actionType == ScheduleActionType.Query)
                    trParameter.Visible = trAttachformat.Visible = trQuery.Visible = (actionType == ScheduleActionType.Query);
                if (actionType == ScheduleActionType.Alert)
                    trParameter.Visible = trAttachformat.Visible = trQuery.Visible = trCondition.Visible = (actionType == ScheduleActionType.Alert);
                if (actionType == ScheduleActionType.Report)
                    trAttachformat.Visible = trReportType.Visible = (actionType == ScheduleActionType.Report);
                if (actionType == ScheduleActionType.Reminder)
                    trTicketPicker.Visible = (actionType == ScheduleActionType.Reminder);
                if (actionType == ScheduleActionType.ScheduledTicket)
                    trmodule.Visible = trtemplate.Visible = trsendnotification.Visible = (actionType == ScheduleActionType.ScheduledTicket);
                if (actionType == ScheduleActionType.Reminder)
                {
                    string url = UGITUtility.GetAbsoluteURL(string.Format(Convert.ToString(hdnConfiguration.Get("TicketPicker")), "TSR", string.Empty, "ScheduleAddTicket", txtTicketId.ClientID));
                    aTicket.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Picker List"));
                }

                if (Request[hdnControls.UniqueID] == "true")
                {
                    int queryId = 0;
                    if (hdnfield.Contains("QueryId") && int.TryParse(Convert.ToString(hdnfield.Get("QueryId")), out queryId) && queryId > 0)
                    {
                        _QueryParameter = (QueryParameters)Page.LoadControl("~/ControlTemplates/uGovernIT/QueryParameters.ascx");
                        _QueryParameter.QueryId = queryId;
                        _QueryParameter.Where = "";
                        _QueryParameter.Id = requestId;
                        //Spdelta 70
                        //_QueryParameter.ScheduleEnable = true;
                        //
                        _QueryParameter.OnSubmit += _QueryParameter_OnSubmit;
                        tdParameter.Controls.Add(_QueryParameter);
                    }
                    else
                    {
                        queryId = UGITUtility.StringToInt(cmbQuery.Items.FindByText("Hi-Priority Open Tickets").Value);
                        _QueryParameter = (QueryParameters)Page.LoadControl("~/ControlTemplates/uGovernIT/QueryParameters.ascx");
                        _QueryParameter.QueryId = queryId;
                        _QueryParameter.Where = "";
                        _QueryParameter.Id = requestId;
                        _QueryParameter.OnSubmit += _QueryParameter_OnSubmit;
                        tdParameter.Controls.Add(_QueryParameter);
                    }
                }
            }
            string serialized = string.Empty;
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(spListitem.ActionTypeData)))
                    serialized = Convert.ToString(spListitem.ActionTypeData);
            }



        }

        //Spdelta 70
        void FillDaysOfMonth(DropDownList dropdown, int days)
        {
            if (dropdown.Items.Count == 0)
            {
                for (int i = 1; i <= days; i++)
                {
                    dropdown.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                dropdown.Items.Add(new ListItem("Last Day", Convert.ToString(days + 1)));
            }

        }
        //
        void _QueryParameter_OnSubmit(object sender, EventArgsSubmitEventHandler e)
        {
            hdnfield.Set(ReportScheduleConstant.Where, e.Value);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ddlScheduleActionType.SelectedValue == ReportScheduleConstant.Report)
                {
                    if (uHelper.ReportScheduleDict != null && uHelper.ReportScheduleDict.Count == 0)
                    {
                        string serialized = spListitem.CustomProperties;

                        if (string.IsNullOrEmpty(serialized))
                            serialized = spListitem.ActionTypeData;

                        if (!string.IsNullOrEmpty(serialized))
                            uHelper.ReportScheduleDict = UGITUtility.DeserializeDicObject(serialized);
                    }
                }
            }
            lblMsg.Text = "";
            //Spdelta 70
            SetFrequencyTypeChanges();
        }

        private void FillForm()
        {
            txtScheduleTitle.Text = Convert.ToString(spListitem.Title);
            dtcScheduleStartTime.Date = Convert.ToDateTime(spListitem.StartTime);
            txtScheduleEmailTo.Text = Convert.ToString(spListitem.EmailIDTo);
            txtScheduleEmailCC.Text = Convert.ToString(spListitem.EmailIDCC);
            txtScheduleSubject.Text = Convert.ToString(spListitem.MailSubject);
            _HtmlEditorControl.Html = Convert.ToString(spListitem.EmailBody);

            ddlScheduleActionType.SelectedValue = Convert.ToString(spListitem.ActionType);

            ddlFileLocation.SelectedValue = Convert.ToString(spListitem.FileLocation);

            ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), ddlScheduleActionType.SelectedValue);
            trmodule.Visible = trtemplate.Visible = trsendnotification.Visible = (actionType == ScheduleActionType.ScheduledTicket);
            if (actionType == ScheduleActionType.Query)
                trParameter.Visible = trAttachformat.Visible = trQuery.Visible = (actionType == ScheduleActionType.Query);

            if (actionType == ScheduleActionType.Alert)
                trParameter.Visible = trCondition.Visible = trQuery.Visible = (actionType == ScheduleActionType.Alert);

            trTicketPicker.Visible = (actionType == ScheduleActionType.Reminder);
            if (ddlFileLocation.SelectedValue.ToLower() == "ftp")
            {
                trEmailTo.Visible = false;
                dvftp1.Visible = true;
                dvftp2.Visible = true;
            }
            else
            {
                trEmailTo.Visible = true;
                dvftp1.Visible = false;
                dvftp2.Visible = false;
            }
            #region Report
            if (actionType == ScheduleActionType.Report)
            {
                FillReportType();
                ddlTypeofReport.SelectedValue = spListitem.TicketId;
                trAttachformat.Visible = trReportType.Visible = true;

                if (!string.IsNullOrEmpty(spListitem.AttachmentFormat))
                    ddlAttachFormat.SelectedValue = spListitem.AttachmentFormat;

                List<string> strCustomPropOther = new List<string>();
                //if we provide file name in case of ftp.
                if (!string.IsNullOrEmpty(Convert.ToString(spListitem.CustomProperties)) && ddlFileLocation.SelectedValue.ToLower() == "ftp")
                {
                    strCustomPropOther = Convert.ToString(spListitem.CustomProperties).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<string> strFileName = strCustomPropOther[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> strFtpurl = strCustomPropOther[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> strftpCredential = strCustomPropOther[2].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (strFileName.Count > 1 && !string.IsNullOrEmpty(strFileName[1]))
                        txtFileName.Text = strFileName[1];

                    if (strFtpurl.Count > 1 && !string.IsNullOrEmpty(strFtpurl[1]))
                        txtftpurl.Text = strFtpurl[1];

                    if (strftpCredential.Count > 1 && !string.IsNullOrEmpty(strftpCredential[1]))
                    {
                        string ftpCredential = uGovernITCrypto.Decrypt(Convert.ToString(strftpCredential[1]), Constants.UGITAPass);
                        string[] credentials = ftpCredential.Split(new string[] { "," }, StringSplitOptions.None);

                        if (credentials.Length > 0 && !string.IsNullOrEmpty(credentials[0]))
                            txtuserName.Text = credentials[0];

                        if (credentials.Length > 0 && !string.IsNullOrEmpty(credentials[1]))
                            txtPassword.Text = credentials[1];
                    }
                }
            }
            #endregion

            #region Query
            if (actionType == ScheduleActionType.Query)
            {
                string serialized = string.Empty;
                string whereClause = string.Empty;
                Dictionary<string, object> reportOptions = null;
                if (!string.IsNullOrEmpty(Convert.ToString(spListitem.ActionTypeData)))
                    serialized = Convert.ToString(spListitem.ActionTypeData);
                else if (!string.IsNullOrEmpty(Convert.ToString(spListitem.CustomProperties)))
                    serialized = Convert.ToString(spListitem.CustomProperties);
                if (serialized != null)
                {
                    reportOptions = UGITUtility.DeserializeDicObject(serialized);
                    whereClause = reportOptions.ContainsKey(ReportScheduleConstant.Where) ? Convert.ToString(reportOptions[ReportScheduleConstant.Where]) : string.Empty;
                }
                int queryId = UGITUtility.StringToInt(spListitem.TicketId);

                _QueryParameter = (QueryParameters)Page.LoadControl("~/ControlTemplates/uGovernIT/QueryParameters.ascx");
                if (Request[hdnControls.UniqueID] == "true")
                {
                    queryId = 0;
                    if (hdnfield.Contains("QueryId") && int.TryParse(Convert.ToString(hdnfield.Get("QueryId")), out queryId) && queryId > 0)
                    {
                        _QueryParameter.QueryId = queryId;
                        _QueryParameter.Where = "";
                    }
                    else
                    {
                        queryId = UGITUtility.StringToInt(cmbQuery.Items.FindByText("Hi-Priority Open Tickets").Value);
                        _QueryParameter.QueryId = queryId;
                        _QueryParameter.Where = "";
                    }
                }
                else
                {
                    _QueryParameter.QueryId = queryId;
                    if (!string.IsNullOrEmpty(whereClause))
                        _QueryParameter.Where = whereClause;
                }

                _QueryParameter.Id = requestId;
                //SpDelta 70
                //_QueryParameter.ScheduleEnable = true;
                //
                _QueryParameter.OnSubmit += _QueryParameter_OnSubmit;
                tdParameter.Controls.Add(_QueryParameter);

                cmbQuery.SelectedIndex = cmbQuery.Items.FindByValue(Convert.ToString(queryId)).Index;
                if (!string.IsNullOrEmpty(whereClause))
                    hdnfield.Set(ReportScheduleConstant.Where, whereClause);

                string attachFormat = reportOptions.ContainsKey(ReportScheduleConstant.AttachmentFormat) ? Convert.ToString(reportOptions[ReportScheduleConstant.AttachmentFormat]) : string.Empty;
                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = Convert.ToString(spListitem.AttachmentFormat);
                if (string.IsNullOrEmpty(attachFormat))
                    attachFormat = "pdf";
                ddlAttachFormat.SelectedValue = attachFormat;
                List<string> strCustomPropOther = new List<string>();
                //if we provide file name in case of ftp.
                if (!string.IsNullOrEmpty(Convert.ToString(spListitem.CustomProperties)))
                {
                    strCustomPropOther = Convert.ToString(spListitem.CustomProperties).Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<string> strFileName = strCustomPropOther[0].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> strFtpurl = strCustomPropOther[1].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<string> strftpCredential = strCustomPropOther[2].Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (strFileName.Count > 1 && !string.IsNullOrEmpty(strFileName[1]))
                        txtFileName.Text = strFileName[1];

                    if (strFtpurl.Count > 1 && !string.IsNullOrEmpty(strFtpurl[1]))
                        txtftpurl.Text = strFtpurl[1];

                    if (strftpCredential.Count > 1 && !string.IsNullOrEmpty(strftpCredential[1]))
                    {
                        string ftpCredential = uGovernITCrypto.Decrypt(Convert.ToString(strftpCredential[1]), Constants.UGITAPass);
                        string[] credentials = ftpCredential.Split(new string[] { "," }, StringSplitOptions.None);


                        if (credentials.Length > 0 && !string.IsNullOrEmpty(credentials[0]))
                            txtuserName.Text = credentials[0];

                        if (credentials.Length > 0 && !string.IsNullOrEmpty(credentials[1]))
                            txtPassword.Text = credentials[1];
                    }

                }

            }
            #endregion

            #region Alert
            if (actionType == ScheduleActionType.Alert)
            {
                string serialized = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(spListitem.ActionTypeData)))
                {
                    serialized = Convert.ToString(spListitem.ActionTypeData);
                }
                else if (!string.IsNullOrEmpty(Convert.ToString(spListitem.CustomProperties)))
                {
                    serialized = Convert.ToString(spListitem.CustomProperties);
                }

                Dictionary<string, object> tempdic = UGITUtility.DeserializeDicObject(serialized);
                int queryId = Convert.ToInt32(spListitem.TicketId);

                _QueryParameter = (QueryParameters)Page.LoadControl("~/ControlTemplates/uGovernIT/QueryParameters.ascx");
                _QueryParameter.QueryId = queryId;
                _QueryParameter.Where = Convert.ToString(tempdic[ReportScheduleConstant.Where]);
                _QueryParameter.Id = requestId;
                //SpDelta 70
                //_QueryParameter.ScheduleEnable = true;
                //
                _QueryParameter.OnSubmit += _QueryParameter_OnSubmit;
                tdParameter.Controls.Add(_QueryParameter);
                cmbQuery.SelectedIndex = cmbQuery.Items.FindByValue(Convert.ToString(queryId)).Index;
                hdnfield.Set(ReportScheduleConstant.Where, Convert.ToString(tempdic[ReportScheduleConstant.Where]));

                txtCondition.Text = Convert.ToString(spListitem.AlertCondition);

            }
            #endregion

            #region Reminder
            if (actionType == ScheduleActionType.Reminder)
            {
                string url = UGITUtility.GetAbsoluteURL(string.Format(Convert.ToString(hdnConfiguration.Get("TicketPicker")), "TSR", string.Empty, "ScheduleAddTicket", txtTicketId.ClientID));
                aTicket.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Picker List"));

                txtTicketId.Value = Convert.ToString(spListitem.TicketId);
                if (!string.IsNullOrEmpty(txtTicketId.Value))
                {
                    string moduleName = uHelper.getModuleNameByTicketId(txtTicketId.Value);
                    url = Ticket.GenerateTicketURL(context, moduleName, txtTicketId.Value);
                    hdnConfiguration.Set("TicketUrl", url);
                }
            }
            #endregion

            #region Schedule Tickets
            if (actionType == ScheduleActionType.ScheduledTicket)
            {
                chkSendNotification.Checked = false;
                if (!string.IsNullOrEmpty(txtScheduleEmailTo.Text))
                    chkSendNotification.Checked = true;

                string fieldLookup = null;
                if (spListitem.ModuleNameLookup != null)
                    fieldLookup = spListitem.ModuleNameLookup;
                if (fieldLookup != null)
                    cmbmodule.Value = fieldLookup;

                string actionData = Convert.ToString(spListitem.ActionTypeData);
                if (!string.IsNullOrEmpty(actionData) && fieldLookup != null)
                {
                    Dictionary<string, object> reuslt = UGITUtility.DeserializeDicObject(actionData);
                    FillTemplate(fieldLookup);
                    //cmbtemplate.Value = reuslt[RecurringTicketScheduleConstant.TemplateId];     
                    if (reuslt.Count > 0)
                        cmbtemplate.Value = reuslt[RecurringTicketScheduleConstant.TemplateId];
                }
            }
            #endregion

            recurrTable.Visible = chkScheduleRecurring.Checked = Convert.ToBoolean(spListitem.Recurring);
            if (chkScheduleRecurring.Checked)
            {
                txtScheduleRecurrInterval.Text = Convert.ToString(spListitem.RecurringInterval);
                double escalationMinutes = Convert.ToDouble(spListitem.RecurringInterval);
                string fieldLookup = null;
                if (spListitem.ModuleNameLookup != null)
                    fieldLookup = Convert.ToString(spListitem.ModuleNameLookup);
                if (Convert.ToString(spListitem.ActionType) == "Reminder" && fieldLookup != null && (fieldLookup == ModuleNames.CMT || fieldLookup == ModuleNames.PMM))
                {
                    txtScheduleRecurrInterval.Text = string.Format("{0:0.##}", escalationMinutes / (24 * 60));
                    ddlIntervalUnit.SelectedValue = Constants.SLAConstants.Days;
                }
                else
                {
                    int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
                    if (escalationMinutes % (workingHoursInADay * 60) == 0)
                    {
                        txtScheduleRecurrInterval.Text = string.Format("{0:0.##}", escalationMinutes / (workingHoursInADay * 60));
                        ddlIntervalUnit.SelectedValue = Constants.SLAConstants.Days;
                    }
                    else if (escalationMinutes % 60 == 0)
                    {
                        txtScheduleRecurrInterval.Text = string.Format("{0:0.##}", escalationMinutes / 60);
                        ddlIntervalUnit.SelectedValue = Constants.SLAConstants.Hours;
                    }
                    else
                    {
                        txtScheduleRecurrInterval.Text = string.Format("{0:0.##}", escalationMinutes);
                        ddlIntervalUnit.SelectedValue = Constants.SLAConstants.Minutes;
                    }
                }

                dtcRecurrEndDate.Date = UGITUtility.StringToDateTime(spListitem.RecurringEndDate);

                #region Custom Recurrence
                string customProperty = Convert.ToString(spListitem.CustomProperties);

                Dictionary<string, object> customDic = UGITUtility.DeserializeDicObject(customProperty);
                //SpDelta 70
                if (customDic != null)
                {
                    if (customDic.ContainsKey(ReportScheduleConstant.FrequencyType))
                    {
                        string frequencyType = Convert.ToString(customDic[ReportScheduleConstant.FrequencyType]);
                        rdnfrequencylist.SelectedIndex = rdnfrequencylist.Items.IndexOf(rdnfrequencylist.Items.FindByValue(frequencyType));
                        switch (frequencyType)
                        {
                            case "1":
                                if (customDic.ContainsKey(ReportScheduleConstant.Frequency))
                                    txtScheduleRecurrInterval.Text = string.IsNullOrEmpty(Convert.ToString(customDic[ReportScheduleConstant.Frequency])) ? "1" : Convert.ToString(customDic[ReportScheduleConstant.Frequency]);
                                if (customDic.ContainsKey(ReportScheduleConstant.FrequencyUnit))
                                    ddlIntervalUnit.SelectedIndex = ddlIntervalUnit.Items.IndexOf(ddlIntervalUnit.Items.FindByValue(Convert.ToString(customDic[ReportScheduleConstant.FrequencyUnit])));

                                if (customDic.ContainsKey(ReportScheduleConstant.BusinessHours) && UGITUtility.StringToBoolean(customDic[ReportScheduleConstant.BusinessHours]))
                                    chkbxBusinessHours.Checked = true;
                                else
                                    chkbxBusinessHours.Checked = false;

                                break;
                            case "2":

                                if (customDic.ContainsKey(ReportScheduleConstant.MonthFrequencyType))
                                {
                                    ddlMonthFrequencyType.SelectedIndex = ddlMonthFrequencyType.Items.IndexOf(ddlMonthFrequencyType.Items.FindByValue(Convert.ToString(customDic[ReportScheduleConstant.MonthFrequencyType])));
                                    if (ddlMonthFrequencyType.SelectedIndex != -1 && ddlMonthFrequencyType.SelectedValue == "1" && customDic.ContainsKey(ReportScheduleConstant.DayOfMonth))
                                        ddlBusinessDayOfMonth.SelectedIndex = ddlBusinessDayOfMonth.Items.IndexOf(ddlBusinessDayOfMonth.Items.FindByValue(Convert.ToString(customDic[ReportScheduleConstant.DayOfMonth])));
                                    else
                                        ddlDayOfMonth.SelectedIndex = ddlDayOfMonth.Items.IndexOf(ddlDayOfMonth.Items.FindByValue(Convert.ToString(customDic[ReportScheduleConstant.DayOfMonth])));
                                }

                                if (customDic.ContainsKey(ReportScheduleConstant.EveryXMonths))
                                {
                                    int reportingPeriod = UGITUtility.StringToInt(Convert.ToString(customDic[ReportScheduleConstant.EveryXMonths]));
                                    spnEditEveryOfMonth.Value = reportingPeriod == 0 ? 1 : reportingPeriod;
                                }

                                break;
                            case "3":
                                if (customDic.ContainsKey(ReportScheduleConstant.CustomRecurrence))
                                {
                                    chkbxBusinessHours.Visible = false;
                                    chkbxBusinessHours.Checked = false;
                                    List<string> pValues = Convert.ToString(customDic[ReportScheduleConstant.CustomRecurrence]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    foreach (string item in pValues)
                                    {
                                        chkbxCustomRecurranceInterval.Items.FindByValue(item).Selected = true;
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }


                    else
                    {
                        //

                        if (customDic.ContainsKey(ReportScheduleConstant.CustomRecurrence))
                        {
                            chkbxBusinessHours.Visible = false;
                            chkbxBusinessHours.Checked = false;
                            List<string> pValues = Convert.ToString(customDic[ReportScheduleConstant.CustomRecurrence]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            ddlIntervalUnit.SelectedIndex = ddlIntervalUnit.Items.IndexOf(ddlIntervalUnit.Items.FindByText("Custom"));
                            ddlIntervalUnit_SelectedIndexChanged(null, null);
                            txtScheduleRecurrInterval.Text = "";
                            foreach (string item in pValues)
                            {
                                chkbxCustomRecurranceInterval.Items.FindByValue(item).Selected = true;
                            }
                        }
                        else if (customDic.ContainsKey(ReportScheduleConstant.BusinessHours) && Convert.ToBoolean(customDic[ReportScheduleConstant.BusinessHours]) == true)
                            chkbxBusinessHours.Checked = true;
                        else
                            chkbxBusinessHours.Checked = false;
                        //SpDelta 70
                    }
                }
                //
                #endregion
            }
        }

        protected void chkScheduleRecurring_CheckedChanged(object sender, EventArgs e)
        {
            //Spdelta 70
            SetFrequencyTypeChanges();
            //
            recurrTable.Visible = chkScheduleRecurring.Checked;
        }
        //Spdelta 70
        void SetFrequencyTypeChanges()
        {
            divfrequencylist.Visible = chkScheduleRecurring.Checked;
            //divfrequency.Visible = false;
            divMonth.Visible = false;
            spanCustomRecInterval.Visible = false;
            ddlDayOfMonth.Visible = false;
            ddlBusinessDayOfMonth.Visible = false;
            dtcRecurrEndDate.Visible = false;
            //divcustom.Visible = false;
            recurrTable.Visible = false;
            chkbxBusinessHours.Visible = false;
            if (rdnfrequencylist != null && rdnfrequencylist.SelectedIndex != -1 && chkScheduleRecurring.Checked)
            {
                dtcRecurrEndDate.Visible = true;
                //divcustom.Visible = true;
                recurrTable.Visible = true;

                string selectRadioOption = Convert.ToString(rdnfrequencylist.Value);
                switch (selectRadioOption)
                {
                    case "1":
                        //divfrequency.Visible = true;

                        //spdelta 70
                        if (ddlIntervalUnit.SelectedValue != "Custom")
                        {
                            txtScheduleRecurrInterval.Visible = true;
                            ddlIntervalUnit.Visible = true;
                            chkbxBusinessHours.Visible = true;
                            spanCustomRecInterval.Visible = false;
                        }
                        else
                        {
                            spanCustomRecInterval.Visible = true;
                        }




                        ddlDayOfMonth.Visible = false;
                        //
                        break;
                    case "2":
                        divMonth.Visible = true;
                        //spdelta 70
                        spanCustomRecInterval.Visible = false;
                        chkbxBusinessHours.Visible = false;
                        ddlDayOfMonth.Visible = false;
                        txtScheduleRecurrInterval.Visible = false;
                        ddlIntervalUnit.Visible = false;
                        chkbxBusinessHours.Visible = false;
                        //
                        if (ddlMonthFrequencyType.SelectedValue == "1")
                            ddlBusinessDayOfMonth.Visible = true;
                        else
                            ddlDayOfMonth.Visible = true;

                        break;
                    case "3":
                        spanCustomRecInterval.Visible = true;
                        //spdelta 70
                        ddlDayOfMonth.Visible = false;
                        txtScheduleRecurrInterval.Visible = false;
                        ddlIntervalUnit.Visible = false;
                        chkbxBusinessHours.Visible = false;
                        //
                        break;
                    default:
                        break;
                }
            }
        }
        protected void rdnfrequencylist_ValueChanged(object sender, EventArgs e)
        {
            SetFrequencyTypeChanges();
        }
        protected void ddlMonthFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFrequencyTypeChanges();
        }
        //
        protected void ddlScheduleActionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScheduleActionType actionType = (ScheduleActionType)Enum.Parse(typeof(ScheduleActionType), ddlScheduleActionType.SelectedValue);

            trAttachformat.Visible = trReportType.Visible = (actionType == ScheduleActionType.Report);
            trmodule.Visible = trtemplate.Visible = trsendnotification.Visible = (actionType == ScheduleActionType.ScheduledTicket);
            trTicketPicker.Visible = (actionType == ScheduleActionType.Reminder);
            trParameter.Visible = false;
            trQuery.Visible = false;
            trCondition.Visible = false;
            ddlFileLocation.SelectedIndex = 0;


            switch (actionType)
            {
                case ScheduleActionType.Reminder:
                    string url = UGITUtility.GetAbsoluteURL(string.Format(Convert.ToString(hdnConfiguration.Get("TicketPicker")), "PRS", string.Empty, "ScheduleAddTicket", txtTicketId.ClientID));
                    aTicket.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), "Picker List"));
                    break;
                case ScheduleActionType.Query:
                    trParameter.Visible = trAttachformat.Visible = trQuery.Visible = (actionType == ScheduleActionType.Query);
                    break;
                case ScheduleActionType.Report:
                    FillReportType();
                    break;
                case ScheduleActionType.Alert:
                    trParameter.Visible = trCondition.Visible = trQuery.Visible = (actionType == ScheduleActionType.Alert);
                    break;
                default:
                    break;
            }

            if (ddlFileLocation.SelectedValue.ToLower() == "ftp")
            {
                trEmailTo.Visible = false;
                dvftp1.Visible = true;
                dvftp2.Visible = true;
            }
            else
            {
                trEmailTo.Visible = true;
                dvftp1.Visible = false;
                dvftp2.Visible = false;
            }
        }

        private void FillQuery()
        {
            DashboardManager dashboardManager = new DashboardManager(context);
            if (cmbQuery.Items.Count == 0)
            {
                cmbQuery.Columns.Clear();
                List<Dashboard> DashboardList = dashboardManager.Load().Where(x => x.DashboardType == DashboardType.Query).ToList();
                if (DashboardList != null && DashboardList.Count > 0)
                {
                    cmbQuery.Columns.Add(DatabaseObjects.Columns.Title);
                    cmbQuery.Columns[0].Width = new Unit("150px");
                    cmbQuery.Columns.Add(DatabaseObjects.Columns.DashboardDescription);
                    cmbQuery.DropDownWidth = new Unit("400px");

                    cmbQuery.DataSource = DashboardList;
                    cmbQuery.DataBind();
                }
                cmbQuery.Items.Insert(0, new ListEditItem("--Select--", ""));
            }
        }


        private void FillReportType()
        {
            if (ddlScheduleActionType.SelectedValue == ScheduleActionType.Report.ToString())
            {
                List<ReportConfigData> dtReportType = reportConfigManager.Load().Where(y => y.TenantID == context.TenantID).OrderBy(x => x.ReportTitle).ToList();
                if (dtReportType == null && dtReportType.Count == 0)
                    return;

                if (dtReportType.Count > 0)
                {
                    ddlTypeofReport.DataSource = dtReportType;
                    ddlTypeofReport.DataTextField = DatabaseObjects.Columns.ReportTitle;
                    ddlTypeofReport.DataValueField = DatabaseObjects.Columns.ReportType;
                    ddlTypeofReport.DataBind();
                }
                ddlTypeofReport.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            spListitem.ActionType = ddlScheduleActionType.SelectedValue;
            spListitem.Title = txtScheduleTitle.Text;
            spListitem.StartTime = dtcScheduleStartTime.Date;
            spListitem.EmailIDTo = txtScheduleEmailTo.Text;
            spListitem.EmailIDCC = txtScheduleEmailCC.Text;
            spListitem.MailSubject = txtScheduleSubject.Text;
            spListitem.EmailBody = _HtmlEditorControl.Html;
            spListitem.FileLocation = ddlFileLocation.SelectedValue;
            spListitem.Recurring = chkScheduleRecurring.Checked;
            //SpDelta 70
            string frequencyType = Convert.ToString(rdnfrequencylist.Value);
            //
            if (chkScheduleRecurring.Checked)
            {
                Dictionary<string, object> customIntervalDic = new Dictionary<string, object>();
                if ((frequencyType == "1" && ddlIntervalUnit.SelectedValue == "Custom") || frequencyType == "3")
                {
                    //if (ddlIntervalUnit.SelectedValue == "Custom")
                    //{ 
                    List<string> pValues = new List<string>();
                    foreach (ListItem item in chkbxCustomRecurranceInterval.Items)
                    {
                        if (item.Selected)
                            pValues.Add(item.Value);
                    }
                    customIntervalDic.Add(ReportScheduleConstant.CustomRecurrence, string.Join(",", pValues));
                    spListitem.CustomProperties = UGITUtility.SerializeDicObject(customIntervalDic);
                    DateTime myStartTime = Convert.ToDateTime(spListitem.StartTime);
                    DateTime recurringEnd = Convert.ToDateTime(spListitem.RecurringEndDate);
                    int startDay = (int)myStartTime.DayOfWeek;
                    int nextDay = 7;
                    double nxtDay = 0;
                    if (pValues.Exists(x => UGITUtility.StringToInt(x) > startDay))
                    {
                        nextDay = UGITUtility.StringToInt(pValues.FirstOrDefault(x => UGITUtility.StringToInt(x) > startDay));
                        nxtDay = myStartTime.AddDays(nextDay - startDay).Subtract(myStartTime).TotalDays;
                    }

                    else
                    {
                        if (pValues.Exists(x => UGITUtility.StringToInt(x) >= 0))
                        {
                            nextDay = UGITUtility.StringToInt(pValues.FirstOrDefault(x => UGITUtility.StringToInt(x) >= 0));
                            DateTime nextWeekStartDay = uHelper.FirstDayOfWeek(context, myStartTime.AddDays(7));
                            nxtDay = nextWeekStartDay.AddDays(nextDay).Subtract(myStartTime).TotalDays + 1;
                        }
                    }

                    if (myStartTime.AddDays(nxtDay) <= recurringEnd)
                        spListitem.RecurringInterval = Convert.ToInt32(nxtDay * 60 * uHelper.GetWorkingHoursInADay(context, true));

                    //SpDelta 70
                    if (ddlIntervalUnit.SelectedValue != "Custom")
                    {
                        customIntervalDic.Add(ReportScheduleConstant.FrequencyType, frequencyType);
                        customIntervalDic.Add(ReportScheduleConstant.BusinessHours, chkbxBusinessHours.Checked);
                        spListitem.CustomProperties = UGITUtility.SerializeDicObject(customIntervalDic);
                    }
                    //}
                    //
                }
                else
                {
                    //SpDelta 70
                    if (frequencyType == "2")
                    {
                        customIntervalDic.Add(ReportScheduleConstant.FrequencyType, frequencyType);
                        if (ddlMonthFrequencyType.SelectedValue == "1")
                            customIntervalDic.Add(ReportScheduleConstant.DayOfMonth, ddlBusinessDayOfMonth.SelectedValue);
                        else
                            customIntervalDic.Add(ReportScheduleConstant.DayOfMonth, ddlDayOfMonth.SelectedValue);

                        customIntervalDic.Add(ReportScheduleConstant.MonthFrequencyType, ddlMonthFrequencyType.SelectedValue);
                        int reportingFrequencyPeriod = UGITUtility.StringToInt(spnEditEveryOfMonth.Value);
                        customIntervalDic.Add(ReportScheduleConstant.EveryXMonths, reportingFrequencyPeriod == 0 ? 1 : reportingFrequencyPeriod);
                    }
                    else
                    {
                        //
                        // Converting days,hours into minutes
                        if (!string.IsNullOrEmpty(txtScheduleRecurrInterval.Text))
                        {
                            if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Days)
                            {
                                spListitem.RecurringInterval = UGITUtility.StringToInt(txtScheduleRecurrInterval.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, true);
                            }
                            else if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Hours)
                            {
                                spListitem.RecurringInterval = UGITUtility.StringToInt(txtScheduleRecurrInterval.Text.Trim()) * 60;
                            }
                            else if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Minutes)
                            {
                                spListitem.RecurringInterval = UGITUtility.StringToInt(txtScheduleRecurrInterval.Text.Trim());
                            }
                        }

                        //SpDelta 70
                    }
                    customIntervalDic.Add(ReportScheduleConstant.BusinessHours, chkbxBusinessHours.Checked);
                    spListitem.CustomProperties = UGITUtility.SerializeDicObject(customIntervalDic);
                    //

                }
                if (dtcRecurrEndDate.Date != null)
                    spListitem.RecurringEndDate = dtcRecurrEndDate.Date;
                else
                    spListitem.RecurringEndDate = null;
            }
            else
                spListitem.CustomProperties = string.Empty;

            if (ddlScheduleActionType.SelectedValue == ScheduleActionType.Report.ToString())
            {
                spListitem.TicketId = ddlTypeofReport.SelectedValue;
                spListitem.AttachmentFormat = ddlAttachFormat.SelectedValue;
                if (ddlFileLocation.SelectedValue.ToLower() == "ftp")
                {
                    string strCustomPropertyOther = string.Empty;
                    string ftpCredentail = uGovernITCrypto.Encrypt(string.Format("{0},{1}", txtuserName.Text.Trim(), txtPassword.Text.Trim()), Constants.UGITAPass);
                    strCustomPropertyOther = string.Format("filename{0}{2}{1}ftpurl{0}{3}{1}credentail{0}{4}", Constants.Separator1, Constants.Separator, txtFileName.Text.Trim(), txtftpurl.Text.Trim(), ftpCredentail);
                    spListitem.CustomProperties = strCustomPropertyOther;
                }
            }
            else if (ddlScheduleActionType.SelectedValue == ScheduleActionType.Query.ToString())
            {
                if (cmbQuery.SelectedIndex > 0)
                {
                    if (uHelper.ReportScheduleDict != null && uHelper.ReportScheduleDict.Count > 0)
                    {
                        spListitem.TicketId = Convert.ToString(cmbQuery.SelectedItem.Value);
                        spListitem.AttachmentFormat = ddlAttachFormat.SelectedValue;

                        Dictionary<string, object> formdict = uHelper.ReportScheduleDict;

                        spListitem.ActionTypeData = UGITUtility.SerializeDicObject(formdict);

                    }
                    else
                    {
                        // lblMsg.Text = "Please select Query from Query List.";
                        // return;
                    }
                }
                if (ddlFileLocation.SelectedValue.ToLower() == "ftp")
                {
                    string strCustomPropertyOther = string.Empty;
                    string ftpCredentail = uGovernITCrypto.Encrypt(string.Format("{0},{1}", txtuserName.Text.Trim(), txtPassword.Text.Trim()), Constants.UGITAPass);
                    strCustomPropertyOther = string.Format("filename{0}{2}{1}ftpurl{0}{3}{1}credentail{0}{4}", Constants.Separator1, Constants.Separator, txtFileName.Text.Trim(), txtftpurl.Text.Trim(), ftpCredentail);
                    spListitem.CustomProperties = strCustomPropertyOther;
                }
            }
            else if (ddlScheduleActionType.SelectedValue == ScheduleActionType.Reminder.ToString())
            {
                if (!string.IsNullOrEmpty(txtTicketId.Value))
                    spListitem.TicketId = txtTicketId.Value;
                else
                {
                    lblMsg.Text = "Please select ticketid.";
                    return;
                }
            }
            else if (ddlScheduleActionType.SelectedValue == ScheduleActionType.Alert.ToString())
            {
                if (cmbQuery.SelectedIndex > 0)
                {
                    int id = Convert.ToInt32(cmbQuery.SelectedItem.GetFieldValue("ID"));
                    dashboardManager = new DashboardManager(context);
                    Dashboard uDashboard = dashboardManager.LoadPanelById(id);
                    var query = uDashboard.panel as DashboardQuery;
                    var columncount = query.QueryInfo.Tables.Sum(x => x.Columns.Where(y => y.Selected).Count());
                    if (columncount > 1)
                    {
                        lblMsg.Text = "Please select query which have only one column.";
                        return;
                    }
                    else
                    {
                        spListitem.TicketId = Convert.ToString(cmbQuery.SelectedItem.Value);
                        Dictionary<string, object> formdict = uHelper.ReportScheduleDict;
                        Condition alertCondition = new Condition();
                        string field = string.Empty;
                        //Regex regexCondition = new Regex("\\[([A-Z])\\w+](>|<|>=|<=|==)[0-9]+");
                        Regex regexCondition = new Regex(RegularExpressionConstant.ConditionExpress);
                        if (regexCondition.IsMatch(txtCondition.Text.Trim()))
                        {
                            // Operator from Condition
                            Regex regexOperator = new Regex(RegularExpressionConstant.OperatorTypeExpress, RegexOptions.IgnoreCase);
                            if (regexOperator.IsMatch(txtCondition.Text.Trim()))
                            {
                                var operatorType = regexOperator.Match(txtCondition.Text.Trim()).Value;
                                switch (operatorType)
                                {
                                    case "==":
                                        alertCondition.Operator = OperatorType.Equal;
                                        break;
                                    case ">=":
                                        alertCondition.Operator = OperatorType.GreaterThanEqualTo;
                                        break;
                                    case "<=":
                                        alertCondition.Operator = OperatorType.LessThanEqualTo;
                                        break;
                                    case ">":
                                        alertCondition.Operator = OperatorType.GreaterThan;
                                        break;
                                    case "<":
                                        alertCondition.Operator = OperatorType.LessThan;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            // Value from Condition
                            Regex regexValue = new Regex(RegularExpressionConstant.ValueExpress);
                            if (regexValue.IsMatch(txtCondition.Text.Trim()))
                            {
                                var value = Convert.ToInt32(regexValue.Match(txtCondition.Text.Trim()).Value);
                                alertCondition.Value = value;
                            }

                            if (formdict.ContainsKey(ReportScheduleConstant.AlertCondition))
                                formdict[ReportScheduleConstant.AlertCondition] = alertCondition;
                            else
                                formdict.Add(ReportScheduleConstant.AlertCondition, alertCondition);

                            spListitem.ActionTypeData = UGITUtility.SerializeDicObject(formdict);
                            spListitem.AlertCondition = txtCondition.Text.Trim();
                        }
                        else
                        {
                            lblMsg.Text = "Please enter condition in correct format(e.g. '[Value]>0').";
                            return;
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "Please select Query from Query List.";
                    return;
                }
            }
            else if (ddlScheduleActionType.SelectedValue == ScheduleActionType.ScheduledTicket.ToString())
            {
                if (cmbmodule.Value == null || cmbtemplate.Value == null)
                {
                    lblMsg.Text = string.Format("Please select {0}.", cmbmodule.Value == null ? "module" : "template");
                    return;
                }

                if (chkSendNotification.Checked && string.IsNullOrEmpty(txtScheduleEmailTo.Text))
                {
                    lblMsg.Text = "Please enter Email To.";
                    return;
                }

                spListitem.EmailIDTo = string.Empty;
                if (chkSendNotification.Checked)
                    spListitem.EmailIDTo = txtScheduleEmailTo.Text;

                Dictionary<string, object> dicSchedule = new Dictionary<string, object>();
                dicSchedule.Add(RecurringTicketScheduleConstant.TemplateId, cmbtemplate.Value);
                spListitem.ModuleNameLookup = Convert.ToString(cmbmodule.Value);
                spListitem.ActionTypeData = UGITUtility.SerializeDicObject(dicSchedule);
            }

            if (spListitem.ID > 0)
                scheduleActionsManager.Update(spListitem);
            else
                scheduleActionsManager.Insert(spListitem);

            if (spListitem.ID > 0)
            {
                long id = spListitem.ID;
                if (hdnSchdeuleId != null)
                    hdnSchdeuleId.Set("ID", id);
                uHelper.ClosePopUpAndEndResponse(Context);
                //
            }
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            if (spListitem != null)
            {
                scheduleActionsManager.Delete(spListitem);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void cmbQuery_Load(object sender, EventArgs e)
        {
            FillQuery();
        }
        protected void cmbQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            int queryId = UGITUtility.StringToInt(cmbQuery.Value);
            if (_QueryParameter != null)
                _QueryParameter.ClearQueryParameterHTML(queryId);
        }
        protected void ddlIntervalUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            spanCustomRecInterval.Visible = false;
            txtScheduleRecurrInterval.Enabled = true;
            txtScheduleRecurrInterval.Visible = true;
            chkbxBusinessHours.Visible = true;
            if (ddlIntervalUnit.SelectedValue == "Custom")
            {
                spanCustomRecInterval.Visible = true;
                txtScheduleRecurrInterval.Text = "";
                txtScheduleRecurrInterval.Enabled = false;
                txtScheduleRecurrInterval.Visible = false;
                chkbxBusinessHours.Visible = false;
            }

        }
        private void BindModule()
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            RequestTypeManager requestTypeManager = new RequestTypeManager(context);
            List<UGITModule> dtSelectedModules = moduleViewManager.Load(x => x.ModuleType.Equals(ModuleType.SMS) || x.ModuleName.Equals("NPR")).OrderBy(x => x.ModuleName).ToList();
            if (dtSelectedModules != null && dtSelectedModules.Count() > 0)
            {
                foreach (UGITModule module in dtSelectedModules)
                {
                    List<ModuleRequestType> selectedRTS = requestTypeManager.Load(x => x.ModuleNameLookup.Equals(module.ModuleName));
                    if (selectedRTS != null && selectedRTS.Count() > 0)
                    {
                        cmbmodule.Items.Add(new ListEditItem(Convert.ToString(module.Title), Convert.ToString(module.ModuleName)));
                    }
                }
            }
        }
        protected void cmbmodule_Callback(object sender, CallbackEventArgsBase e)
        {
            FillTemplate(e.Parameter);
        }

        protected void FillTemplate(string module)
        {
            if (string.IsNullOrEmpty(module)) return;

            cmbtemplate.DataSource = null;
            List<TicketTemplate> TemplateList = TemplateManager.Load().Where(x => x.ModuleNameLookup == module).ToList();
            cmbtemplate.DataSource = TemplateList;
            cmbtemplate.DataBind();

        }
        protected void ddlFileLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFileLocation.SelectedValue.ToLower() == "ftp")
            {
                trEmailTo.Visible = false;
                dvftp1.Visible = true;
                dvftp2.Visible = true;
            }
            else
            {
                trEmailTo.Visible = true;
                dvftp1.Visible = false;
                dvftp2.Visible = false;
            }
        }

    }
}
