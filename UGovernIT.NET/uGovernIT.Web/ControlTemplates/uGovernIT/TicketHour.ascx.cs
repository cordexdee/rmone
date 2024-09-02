using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DevExpress.Web;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.DAL;
using System.Globalization;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Specialized;

namespace uGovernIT.Web
{
    public partial class TicketHour : UserControl
    {
        UserProfile user = null;
        public string TicketId { get; set; }
        public  ControlMode ctmode { get; set; }
        public string ModuleName { get; set; }
        DataTable ticketHourList = null;
        DataRow currentTicket = null;
        public bool batchEdit { get; set; }
        Ticket TicketRequest = null;
        public string DateFormat = null;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected string ajaxPageURL;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            
        }
        protected override void OnInit(EventArgs e)
        {
            user = context.CurrentUser;
            UserManager = context.UserManager;
            bool IsAdmin = UserManager.IsUGITSuperAdmin(user) || UserManager.IsTicketAdmin(user);
            ajaxPageURL = UGITUtility.GetAbsoluteURL("/api/rmmapi/");
            DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            //dtcTicketHoursDate.DisplayFormatString= System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            ModuleName = TicketId.Split('-')[0].ToString();
            TicketRequest = new Ticket(context,ModuleName);
            if (!string.IsNullOrEmpty(TicketId))
            {
                TicketHoursManager thManager = new TicketHoursManager(context);
                currentTicket = Ticket.GetCurrentTicket(context,ModuleName, TicketId);
                double totalActualHours = UGITUtility.StringToDouble(currentTicket[DatabaseObjects.Columns.TicketActualHours]);
                bool isActionUser = Ticket.IsActionUser(context, currentTicket, user);
                if (isActionUser|| IsAdmin)
                {
                    string strEditbtn = string.Empty;
                    strEditbtn += "<div style='float:left;margin-top: 3px;'>";
                    strEditbtn += string.Format("<span style='padding-right:10px;float:left;padding-top:7px;font-weight:bold;'>Total Hours: {0}</span>", totalActualHours);
                    strEditbtn += "<a  onclick = 'showTicketHoursPopUp()' style ='padding-top: 10px;'>";
                    strEditbtn += "<span class='button-bg'>";
                    strEditbtn += "<b style = 'float: left; font-weight: normal;' > Add </b>";
                    strEditbtn += "<i style ='float: left; position: relative; left: 3px' >";
                    strEditbtn += "<img src='/Content/images/add_icon.png' style='border: none;' title='' alt=''>";
                    strEditbtn += "</i>";
                    strEditbtn += "</span>";
                    strEditbtn += "</a>";
                    strEditbtn += " </div>";
                    ButtonControl.Text = strEditbtn;
                }

                if (thManager.TicketHourList(TicketId).Count > 0)
                {
                    grid.Visible = true;

                    if (!batchEdit)
                    {

                        ticketHourList = thManager.TicketHours(TicketId);
                        grid.DataSource = ticketHourList;
                        grid.DataBind();
                    }
                }
                else
                {
                    grid.Visible = false;
                }
            }
        }
        protected void dxGvACR_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "aEdit")
            {
                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string date = Convert.ToString(e.GetValue(DatabaseObjects.Columns.WorkDate));
                DateTime dd = Convert.ToDateTime(date);
                string datee = dd.Year + "-" + dd.Month + "-" + dd.Day;
                string hour = Convert.ToString(e.GetValue(DatabaseObjects.Columns.HoursTaken));
                string desc = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Comment));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/DelegateControl.aspx?control=acrtypeedit&ID={0} ", dataKeyValue));
                string Url = string.Format("javascript:ShowPopUpID('{0}','{1}','{2}','{3}')", dataKeyValue, datee, hour, desc);
                HtmlAnchor aHtml = (HtmlAnchor)grid.FindRowCellTemplateControl(index, e.DataColumn, "editLink");
                aHtml.Attributes.Add("href", Url);
                if (e.DataColumn.FieldName == "Title")
                {
                    aHtml.InnerText = e.CellValue.ToString();
                }
            }
            if (e.DataColumn.FieldName == "Resource")
            {
                UserProfile user = UserManager.GetUserById(Convert.ToString(e.GetValue("Resource")));
                e.Cell.Text = user.Name;
            }
            if (e.DataColumn.FieldName == "StageStep")
            {
                LifeCycleStore lifecyclestore = new LifeCycleStore(context);
                LifeCycle lifeCycle = lifecyclestore.LoadByModule(ModuleName).FirstOrDefault(x => x.ID == 1 || x.ID == 0);
                LifeCycleStage lstage = lifeCycle.Stages.Where(x => x.StageStep == Convert.ToInt32("" + e.GetValue("StageStep"))).FirstOrDefault();
                e.Cell.Text = lstage.StageTitle;
            }
        }

        protected void addTicketHoursBt_Click(object sender, EventArgs e)
        {
            TicketHoursManager thManager = new TicketHoursManager(context);
            if (string.IsNullOrEmpty(hdnId.Value))
            {
                int currentStep = Convert.ToInt32(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.StageStep));
                if (UGITUtility.StringToDouble(txtTicketHours.Value) > 0 && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketResolutionComments))
                {
                    DataTable spTicketHours = thManager.GetDataTable();
                    Utility.ActualHour ticketHour = new Utility.ActualHour();
                    ticketHour.TicketID = TicketId;
                    ticketHour.StageStep = currentStep;
                    //Allow user to enter max 24 hours in a day
                    double workHours = UGITUtility.StringToDouble(txtTicketHours.Value);
                    if (workHours > 24)
                        workHours = 24;
                    DateTime workDate = dtcTicketHoursDate.Date;
                    ticketHour.HoursTaken = workHours;
                    ticketHour.Comment = txtResolutionDescription.Text;
                    ticketHour.Resource = user.Id;
                    ticketHour.WorkDate = workDate;
                    thManager.AddUpdate(ticketHour);
                    //Get Total actual hours for current stage
                    string query = string.Format("{0}='{1}' and {2}={3}", DatabaseObjects.Columns.TicketId, TicketId, DatabaseObjects.Columns.StageStep, currentStep);
                    DataRow[] spColl = spTicketHours.Select(query);
                    double totalActualHours = 0;
                    if (spColl != null && spColl.Count() > 0)
                    {
                        DataTable dt = spColl.CopyToDataTable();
                        totalActualHours = UGITUtility.StringToDouble(dt.Compute("SUM(" + DatabaseObjects.Columns.HoursTaken + ")", string.Empty));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(txtResolutionDescription.Text)))
                    {
                        currentTicket[DatabaseObjects.Columns.TicketResolutionComments] = uHelper.GetVersionString(user.Id, Convert.ToString(txtResolutionDescription.Text), currentTicket, DatabaseObjects.Columns.TicketResolutionComments);
                    }
                    TicketRequest.CommitChanges(currentTicket, "", donotUpdateEscalations: false);
                    pcTicketHours.ShowOnPageLoad = false;
                    // Notify requestor or action user of new comment if configured in Modules list for this module
                    if (TicketRequest.Module.ActionUserNotificationOnComment || chkNotifyCommentRequestor.Checked || TicketRequest.Module.InitiatorNotificationOnComment)
                    {
                        string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
                        string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}", user.Name, txtResolutionDescription.Text.Trim());
                        string subject = string.Format("New Comment added to ticket: {0}", ticketId);
                        if (TicketRequest.Module.ActionUserNotificationOnComment)
                            TicketRequest.SendEmailToActionUsers(currentTicket, subject, mailBody);
                        //if (chkNotifyCommentRequestor.Checked)
                            //TicketRequest.SendEmailToRequestor(saveTicket, subject, mailBody);
                            //if (TicketRequest.Module.InitiatorNotificationOnComment) ;
                        //TicketRequest.SendEmailToInitiator(saveTicket, subject, mailBody);
                    }

                    ConfigurationVariableManager cvHelper = new ConfigurationVariableManager(context);                    //Update user working hours inside resource timesheet if setting is enabled
                    if (UGITUtility.StringToBoolean(cvHelper.GetValue(ConfigConstants.CopyTicketActualsToRMM)))
                    {
                        int requestTypeLookup = Convert.ToInt16(currentTicket[DatabaseObjects.Columns.TicketRequestTypeLookup]);
                        if (requestTypeLookup > 0)
                        {
                            ModuleRequestType requestType = TicketRequest.Module.List_RequestTypes.FirstOrDefault(x => x.ID == requestTypeLookup);
                            if (requestType != null)
                            {
                                //ResourceWorkItem workItem = new Helpers.ResourceWorkItem(SPContext.Current.Web.CurrentUser.ID);
                                //workItem.Level1 = requestType.RMMCategory;
                                //workItem.Level2 = requestType.Category;

                                //ResourceTimeSheet.UpdateWorkingHours(SPContext.Current.Web, workItem, SPContext.Current.Web.CurrentUser.ID, workDate, workHours, false);
                            }
                        }
                    }
                }
            }
            else
            {
                Utility.ActualHour ticketHour = thManager.TicketHourList().Where(x => x.ID == Convert.ToInt32(hdnId.Value)).FirstOrDefault();
                double workHours = UGITUtility.StringToDouble(txtTicketHours.Value);
                if (workHours > 24)
                    workHours = 24;
                DateTime workDate = dtcTicketHoursDate.Date;
                ticketHour.HoursTaken = workHours;
                ticketHour.Comment = txtResolutionDescription.Text;
                ticketHour.Resource = user.Id;
                ticketHour.WorkDate = workDate;
                thManager.AddUpdate(ticketHour);
            }
            grid.EnableCallBacks = true;
            pcTicketHours.ShowOnPageLoad = false;
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}
