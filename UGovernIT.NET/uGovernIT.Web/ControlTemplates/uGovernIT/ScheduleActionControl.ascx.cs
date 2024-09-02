using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ScheduleActionControl : UserControl
    {
        public int DashboardPanelID { get; set; }
        public ScheduleAction Action { get; set; }
        public Dictionary<string, string> Parameter { get; set; }
        DashboardQuery panel = null;
        Dashboard uDashboard = null;
        DashboardManager objDashboardManager = null;
        ApplicationContext context = null;
        ScheduleActionsManager actionsManager = null;

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objDashboardManager = new DashboardManager(context);
            actionsManager = new ScheduleActionsManager(context);
            
            if (DashboardPanelID > 0)
            {
                uDashboard = objDashboardManager.LoadPanelById(DashboardPanelID, false);
                panel = (DashboardQuery)uDashboard.panel;
                Action = panel.ScheduleActionValue;
                LoadScheduleActions(Action);
                GenerateDynamicControl(panel.QueryInfo.WhereClauses);
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void LoadScheduleActions(ScheduleAction scheduleAction)
        {
            if (scheduleAction != null)
            {
                txtScheduleTitle.Text = scheduleAction.Title;
                dtcScheduleStartTime.Date = scheduleAction.StartTime;
                txtScheduleEmailTo.Text = scheduleAction.EmailTo;
                txtScheduleEmailCC.Text = scheduleAction.EmailCC;
                txtScheduleSubject.Text = scheduleAction.EmailSubject;
                txtScheduleEmailBody.Text = scheduleAction.EmailBody;
                chkScheduleRecurring.Checked = scheduleAction.Recurring;

                double escalationMinutes = Convert.ToDouble(scheduleAction.RecurringInterval);
                int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, false);

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
                
                if (scheduleAction.RecurringEndDate != null)
                    dtcRecurrEndDate.Date = (DateTime)scheduleAction.RecurringEndDate;
                
                //chkIsEnable.Checked = scheduleAction.IsEnable;
                hdnScheduleId.Value = Convert.ToString(scheduleAction.ScheduleId);
                ddlAttachFormat.SelectedIndex = ddlAttachFormat.Items.IndexOf(ddlAttachFormat.Items.FindByValue(scheduleAction.AttachmentFormat));
            }
            recurrTable.Visible = chkScheduleRecurring.Checked;
            
            if (!string.IsNullOrEmpty(scheduleAction.CustomProperty))
            {
                try
                {
                    Dictionary<string, object> customDic = UGITUtility.DeserializeDicObject(scheduleAction.CustomProperty);
                    if (customDic != null && customDic.Count > 0)
                    {
                        if (customDic.ContainsKey(ReportScheduleConstant.CustomRecurrence))
                        {
                            List<string> pValues = Convert.ToString(customDic[ReportScheduleConstant.CustomRecurrence]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            ddlIntervalUnit.SelectedIndex = ddlIntervalUnit.Items.IndexOf(ddlIntervalUnit.Items.FindByText("Custom"));
                            ddlIntervalUnit_SelectedIndexChanged(null, null);
                            txtScheduleRecurrInterval.Text = "";
                            foreach (string item in pValues)
                                chkbxCustomRecurranceInterval.Items.FindByValue(item).Selected = true;
                        }
                        else if (customDic.ContainsKey(ReportScheduleConstant.BusinessHours))
                        {
                            chkbxBusinessHours.Visible = true;
                            chkbxBusinessHours.Checked = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
        }

        private void GenerateDynamicControl(List<WhereInfo> whereList)
        {
            if (whereList != null && whereList.Count > 0)
            {
                string table = "<table>{0}</table>";

                string tableheader = "<tr>" +
                                       "<td colspan='2'>" +
                                           "<span class='span-header'>Parameters values</span>" +
                                           "<span class='close-button'><img src='/Content/Images/cross_icn.png' /></span>" +
                                       "</td>" +
                                   "</tr>";

                string tablerow = "<tr>" +
                                        "<td class='param-col1'>" +
                                            "<label>{0}</label>" +
                                        "</td>" +
                                        "<td class='param-col2'>" +
                                            "<input class='param-value' type='text' name='value' placeholder='Enter {0}...' />" +
                                        "</td>" +
                                    "</tr>";
                string tablefooter = "<tr>" +
                                         "<td colspan='2'>" +
                                             "<span class='span-submit'><input type='submit' id='param_button' class='input-button-bg' value='Submit' onclick='javascript:paramButtonClick();'  /></span>" +
                                         "</td>" +
                                     "</tr>";

                if (whereList.Exists(w => w.ParameterName != string.Empty))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(tableheader);

                    foreach (var wi in whereList)
                    {
                        if (!string.IsNullOrEmpty(wi.ParameterName))
                        {
                            sb.AppendFormat(tablerow, wi.ParameterName);
                        }
                    }
                    sb.Append(tablefooter);
                    string divcontainer = string.Format(table, sb.ToString());
                    parameterDiv.InnerHtml = string.Empty;
                    parameterDiv.InnerHtml = divcontainer;
                }
                else
                {
                    parameterDiv.InnerHtml = string.Empty;
                }
            }
            else
            {
                parameterDiv.InnerHtml = string.Empty;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var whereClauses = panel.QueryInfo.WhereClauses;
            if (paramValue.Value != string.Empty)
            {
                string[] param_values = paramValue.Value.Split(',');
                int j = 0;
                for (int i = 0; i < whereClauses.Count; i++)
                {
                    if (!string.IsNullOrEmpty(whereClauses[i].ParameterName))
                    {
                        whereClauses[i].Value = param_values[j];
                        j++;
                    }
                }
            }

            panel.ScheduleActionValue = Save(panel.QueryInfo.WhereClauses);
            uDashboard.panel = panel;

            byte[] iconContents = new byte[0];
            string fileName = string.Empty;

            // Save/Update dashboard
            objDashboardManager.SaveDashboardPanel(iconContents, fileName, true, uDashboard);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        internal ScheduleAction Save(List<WhereInfo> whereList)
        {
            ScheduleAction action = new ScheduleAction();

            action.ScheduleId = Convert.ToInt32(hdnScheduleId.Value);
            action.Title = txtScheduleTitle.Text.Trim();
            action.StartTime = dtcScheduleStartTime.Date;
            action.ActionType = ScheduleActionType.Query;
            action.EmailTo = txtScheduleEmailTo.Text.Trim();
            action.EmailCC = txtScheduleEmailCC.Text.Trim();
            action.EmailSubject = txtScheduleSubject.Text.Trim();
            action.EmailBody = txtScheduleEmailBody.Text.Trim();
            action.Recurring = chkScheduleRecurring.Checked;
            Dictionary<string, object> customPropDic = new Dictionary<string, object>();
            if (chkScheduleRecurring.Checked)
            {
                if (!string.IsNullOrEmpty(txtScheduleRecurrInterval.Text))
                {
                    if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Days)
                    {
                        action.RecurringInterval = Convert.ToInt32(txtScheduleRecurrInterval.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(context, false);
                    }
                    else if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Hours)
                    {
                        action.RecurringInterval = Convert.ToInt32(txtScheduleRecurrInterval.Text.Trim()) * 60;
                    }
                    else if (ddlIntervalUnit.SelectedValue == Constants.SLAConstants.Minutes)
                    {
                        action.RecurringInterval = Convert.ToInt32(txtScheduleRecurrInterval.Text.Trim());
                    }
                }

                action.RecurringEndDate = dtcRecurrEndDate.Date;
                if (ddlIntervalUnit.SelectedValue == "Custom")
                {
                    List<string> pValues = new List<string>();
                    foreach (ListItem item in chkbxCustomRecurranceInterval.Items)
                    {
                        if (item.Selected)
                            pValues.Add(item.Value);
                    }

                    customPropDic.Add(ReportScheduleConstant.CustomRecurrence, string.Join(",", pValues));
                    action.CustomProperty = UGITUtility.SerializeDicObject(customPropDic);
                    DateTime myStartTime = Convert.ToDateTime(action.StartTime);
                    DateTime recurringEnd = Convert.ToDateTime(action.RecurringEndDate);
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
                            DateTime nextWeekStartDay = uHelper.FirstDayOfWeek(myStartTime.AddDays(7));
                            nxtDay = nextWeekStartDay.AddDays(nextDay).Subtract(myStartTime).TotalDays + 1;
                        }
                    }

                    if (myStartTime.AddDays(nxtDay) <= recurringEnd)
                        action.RecurringInterval = Convert.ToInt32(nxtDay * 60 * uHelper.GetWorkingHoursInADay(context, false));
                }
                else
                {
                    customPropDic.Add(ReportScheduleConstant.BusinessHours, chkbxBusinessHours.Checked);
                    action.CustomProperty = UGITUtility.SerializeDicObject(customPropDic);
                }
            }
            else
            {
                action.RecurringInterval = 0;
                action.RecurringEndDate = null;
                action.CustomProperty = string.Empty;
            }

            if (whereList.Exists(w => w.ParameterName != string.Empty))
            {
                bool isParameter = false;
                Dictionary<string, object> formdict = new Dictionary<string, object>();
                List<string> pValues = new List<string>();
                for (int i = 0; i < whereList.Count; i++)
                {
                    if (!String.IsNullOrEmpty(whereList[i].ParameterName))
                    {
                        isParameter = true;
                        pValues.Add(whereList[i].Value);
                    }
                }

                if (isParameter)
                {
                    formdict.Add("Where", string.Join(",", pValues));
                    customPropDic.Add("Where", string.Join(",", pValues));
                }

                action.ActionTypeData = UGITUtility.SerializeDicObject(formdict);
            }

            //action.IsEnable = chkIsEnable.Checked;
            action.AttachmentFormat = ddlAttachFormat.SelectedValue;

            // Create Schedule
            if (chkIsEnable.Checked)
            {
                Dictionary<string, object> scheduleDic = new Dictionary<string, object>();

                scheduleDic.Add(DatabaseObjects.Columns.Title, action.Title);
                scheduleDic.Add(DatabaseObjects.Columns.StartTime, action.StartTime);
                scheduleDic.Add(DatabaseObjects.Columns.TicketId, DashboardPanelID);
                scheduleDic.Add(DatabaseObjects.Columns.ActionType, action.ActionType);
                scheduleDic.Add(DatabaseObjects.Columns.EmailIDTo, action.EmailTo);
                scheduleDic.Add(DatabaseObjects.Columns.EmailIDCC, action.EmailCC);
                scheduleDic.Add(DatabaseObjects.Columns.MailSubject, action.EmailSubject);
                scheduleDic.Add(DatabaseObjects.Columns.EmailBody, action.EmailBody);
                scheduleDic.Add(DatabaseObjects.Columns.Recurring, action.Recurring);
                scheduleDic.Add(DatabaseObjects.Columns.RecurringInterval, action.RecurringInterval);
                scheduleDic.Add(DatabaseObjects.Columns.RecurringEndDate, action.RecurringEndDate);
                scheduleDic.Add(DatabaseObjects.Columns.ModuleNameLookup, 0);

                scheduleDic.Add(DatabaseObjects.Columns.CustomProperties, action.CustomProperty);
                scheduleDic.Add(DatabaseObjects.Columns.ActionTypeData, action.ActionTypeData);
                scheduleDic.Add(DatabaseObjects.Columns.AttachmentFormat, action.AttachmentFormat);
                
                AgentJobHelper agent = new AgentJobHelper(context);
                action.ScheduleId = agent.CreateSchedule(scheduleDic);
            }
            else
            {
                SchedulerAction scheduleItem = actionsManager.LoadByID(action.ScheduleId);
                if(scheduleItem != null)
                    actionsManager.Delete(scheduleItem);

            }

            return action;
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
                txtScheduleRecurrInterval.Enabled = false;
                txtScheduleRecurrInterval.Visible = false;
                chkbxBusinessHours.Visible = false;
                txtScheduleRecurrInterval.Text = "";
            }
            dtcScheduleStartTime.Date = Action.StartTime;
            if (Action.RecurringEndDate != null)
                dtcRecurrEndDate.Date = (DateTime)Action.RecurringEndDate;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        protected void chkScheduleRecurring_CheckedChanged(object sender, EventArgs e)
        {
            recurrTable.Visible = chkScheduleRecurring.Checked;
        }
    }
}