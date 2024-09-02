
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class TicketCloseOrReject : UserControl
    {
        public string TicketId { get; set; }
        UserProfile User;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        EscalationProcess objEscalationProcess = null;        
        ModuleViewManager ObjModuleViewManager = null;
        TicketManager ObjTicketManager = null;

        protected override void OnInit(EventArgs e)
        {
            objEscalationProcess = new EscalationProcess(context);
            ObjModuleViewManager = new ModuleViewManager(context);
            ObjTicketManager = new TicketManager(context);

            if (Request["comment"] == "true")
            {
                tr_dropdown.Visible = false;
                tr_Type.Visible = false;
                tr_ActualHrs.Visible = false;
                spanComments.Visible = false;
                //tr_comment.Attributes.Add("class", "col-md-12 col-sm-12 col-xs-12 noPadding");
                //txtComment.CssClass = "form-control bg-light-blue addComment-textArea";
                //txtComment.Attributes.Add("Width", "450px");
                //txtComment.Rows = 3;
                //txtComment.Columns = 52;
                tr_Checkboxes.Visible = true;
            }
            else
            {
                //tr_comment.Attributes.Add("class", "col-md-12 col-sm-12 colForXS");
                txtComment.CssClass = "form-control bg-light-blue quickClose-comment";
                if (!IsPostBack)
                    BindData();
            }
            base.OnInit(e);
        }

        private void BindData()
        {
            try
            {
                ddlResolutionType.Items.Clear();

                List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                UGITModule module = ObjModuleViewManager.LoadByName(uHelper.getModuleNameByTicketId(ticketIds.First())); 
                DataTable spListTicket = ObjTicketManager.GetAllTickets(module);
                if (spListTicket.Columns.Contains(DatabaseObjects.Columns.TicketResolutionType))
                {
                    FieldConfiguration fieldConfig = new FieldConfiguration();
                    FieldConfigurationManager fieldConfigManager = new FieldConfigurationManager(context);
                    fieldConfig = fieldConfigManager.GetFieldByFieldName(DatabaseObjects.Columns.TicketResolutionType);
                    List<string> listItem = new List<string>();
                    DataRow[] items = spListTicket.Select();
                    string[] fieldConfigData = UGITUtility.SplitString(fieldConfig.Data, Constants.Separator);
                    foreach (string str in fieldConfigData)
                    {
                        listItem.Add(str);
                    }
                    string inclause = string.Empty;
                    foreach(string s in ticketIds)
                    {
                        inclause = "'" + s + "',";
                    }

                    DataRow[] ticketsItems = spListTicket.Select(string.Format("{0} IN ({1})",DatabaseObjects.Columns.TicketId, inclause));
                    List<long> requestTypes = new List<long>();
                    foreach (DataRow row in ticketsItems)
                    {
                        requestTypes.Add(UGITUtility.StringToInt(row[DatabaseObjects.Columns.TicketRequestTypeLookup]));
                    }

                    RequestTypeManager requestTypeManager = new RequestTypeManager(context);
                    //DataTable moduleRequestTypeDt = UGITUtility.ToDataTable<ModuleRequestType>(requestTypeManager.GetConfigModuleRequestTypeData());
                    //List<ModuleRequestType> resolutionTypes = moduleRequestTypeDt.AsEnumerable().Where(x => requestTypes.Contains(x.Field<int>(DatabaseObjects.Columns.ID))).ToList();
                    //.Select(x => new
                    //{
                    //    ResolutionTypes = x.Field<string>(DatabaseObjects.Columns.ResolutionTypes),
                    //    RequestType = x.Field<string>(DatabaseObjects.Columns.ID)
                    //}); //.Distinct().ToList();
                    List<ModuleRequestType> moduleRequestTypeList = requestTypeManager.Load(x => requestTypes.Contains(x.ID)).ToList();

                    List<ResolutionTypeItems> resolutionTypeItems = new List<ResolutionTypeItems>();
                    foreach (var item in moduleRequestTypeList)
                    {
                        if (!string.IsNullOrEmpty(item.ResolutionTypes))
                        {
                            if (item.ResolutionTypes.Contains(Constants.Separator))
                            {
                                foreach (var rt in item.ResolutionTypes.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    resolutionTypeItems.Add(new ResolutionTypeItems { RequestType = item.RequestType, ResolutionType = rt });
                                }
                            }
                            else
                            { resolutionTypeItems.Add(new ResolutionTypeItems { RequestType = item.RequestType, ResolutionType = item.ResolutionTypes }); }
                        }
                    }

                    if (resolutionTypeItems.Count() > 0)
                    {
                        var groups = resolutionTypeItems
                                    .GroupBy(n => n.ResolutionType)
                                    .Select(n => new
                                    {
                                        Type = n.Key,
                                        Count = n.Count()
                                    }).OrderBy(n => n.Type);
                        var resList = groups.Where(x => x.Count.Equals(moduleRequestTypeList.Count())).Select(s => s.Type);
                        foreach (string str in resList)
                        {
                            listItem.Add(str);
                        }
                    }
                    else
                    {
                        FieldConfigurationManager fieldManager = new FieldConfigurationManager(context);
                        DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.TicketResolutionType, module.ModuleTable);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            listItem = dt.AsEnumerable().Select(r => r[0].ToString()).ToList();
                        }
                    }


                    listItem.Sort();
                    ddlResolutionType.DataSource = listItem.Distinct();
                    ddlResolutionType.DataBind();

                    tr_Type.Attributes.Add("showCtrl", "true");
                    tr_ActualHrs.Attributes.Add("showCtrl", "true");
                    ddlActionType.Enabled = true;
                }
                else
                {
                    tr_Type.Style.Add("display", "none");
                    tr_Type.Attributes.Add("showCtrl", "false");

                    tr_ActualHrs.Style.Add("display", "none");
                    tr_ActualHrs.Attributes.Add("showCtrl", "false");
                    spanComments.Style.Add("display", "none");

                    ddlActionType.SelectedIndex = 1;
                    ddlActionType.Enabled = false;

                   
                }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }

        protected void ddlActionType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            try
            {
                List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (ticketIds != null)
                {
                    UGITModule module = null;

                    foreach (string ticketId in ticketIds)
                    {
                        Ticket TicketRequest = new Ticket(context,uHelper.getModuleNameByTicketId(ticketId), User);

                        module = ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(ticketId));
                        //UGITModule module =ObjModuleViewManager.GetByName(uHelper.getModuleNameByTicketId(ticketId));
                        //DataTable ticketTable = ObjTicketManager.GetAllTickets(module); 
                        //DataRow ticket = ticketTable.Select(string.Format("{0} = '{1}'",DatabaseObjects.Columns.TicketId, ticketId))[0]; 

                        DataTable ticketTable = ObjTicketManager.GetCachedModuleTableSchema(module);
                        DataRow ticket = ObjTicketManager.GetByTicketID(module, ticketId);

                        if (ticketTable != null && ticket != null)
                        {
                            ticket.AcceptChanges();
                            ticket.SetModified();

                            if (Request["comment"] == "true")
                            {
                                //ticket.AcceptChanges();
                                //ticket.SetModified();
                                if (ticketTable.Columns.Contains(DatabaseObjects.Columns.TicketComment))
                                {
                                    ticket[DatabaseObjects.Columns.TicketComment] = uHelper.GetCommentString(User, txtComment.Text.Trim(), ticket, DatabaseObjects.Columns.TicketComment, chkAddPrivate.Checked);
                                }
                                string error = TicketRequest.CommitChanges(ticket, string.Empty, Request.Url, true);

                                // Notify requestor or action user of new comment if configured in Modules list for this module
                                if (TicketRequest.Module.ActionUserNotificationOnComment || chkNotifyRequestor.Checked)
                                {
                                    string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}", User.UserName, txtComment.Text.Trim());
                                    string subject = string.Format("New Comment added to ticket: {0}", ticketId);
                                    if (TicketRequest.Module.ActionUserNotificationOnComment)
                                        TicketRequest.SendEmailToActionUsers(ticket, subject, mailBody);
                                    if (chkNotifyRequestor.Checked)
                                        TicketRequest.SendEmailToRequestor(ticket, subject, mailBody);
                                }
                            }
                            else
                            {
                                bool valid = true;

                                if (ddlActionType.SelectedValue == "Close")
                                {
                                    if (ticketTable.Columns.Contains(DatabaseObjects.Columns.TicketResolutionType))
                                        ticket[DatabaseObjects.Columns.TicketResolutionType] = ddlResolutionType.SelectedValue;

                                    if (ticketTable.Columns.Contains(DatabaseObjects.Columns.TicketActualHours))
                                        ticket[DatabaseObjects.Columns.TicketActualHours] = string.IsNullOrEmpty(txtActualHours.Text.Trim()) ? "0" : txtActualHours.Text.Trim();

                                    TicketRequest.CloseTicket(ticket, txtComment.Text);
                                    string mailBody = string.Format("{0} added the following comment to this ticket: <br/><br/>{1}", User.UserName, txtComment.Text.Trim());
                                    string subject = string.Format("Ticket Closed: {0}", ticketId);
                                    //if (TicketRequest.Module.ActionUserNotificationOnComment)
                                    //    TicketRequest.SendEmailToActionUsers(ticket, subject, mailBody);
                                   // if (chkNotifyRequestor.Checked)
                                        TicketRequest.SendEmailToRequestor(ticket, subject, mailBody);
                                }
                                else if (ddlActionType.SelectedValue == "Reject")
                                {
                                    //if (uHelper.getModuleNameByTicketId(ticketId).Equals("PMM", StringComparison.OrdinalIgnoreCase))
                                    //    TicketRequest.ClosePMMTicket(ticket, txtComment.Text);
                                    //else
                                    //{
                                        string rejectComments = UGITUtility.WrapComment(txtComment.Text, "Reject");
                                        TicketRequest.Reject(ticket, rejectComments);
                                    //}
                                }

                                List<TicketColumnError> errors = new List<TicketColumnError>();
                                string error = TicketRequest.CommitChanges(ticket, string.Empty, Request.Url);
                                if (!string.IsNullOrEmpty(error))
                                {
                                    errors.Add(TicketColumnError.AddError(error));
                                    valid = false;
                                }
                                if (valid)
                                {
                                    // Delete any pending escalations for this ticket
                                    // Got to do it here since escalations block below may not be executed if not valid
                                    objEscalationProcess.DeleteEscalation(ticket);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Util.Log.ULog.WriteException(ex); }

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }

    class ResolutionTypeItems
    {
        public string RequestType { get; set; }
        public string ResolutionType { get; set; }
    }
}
