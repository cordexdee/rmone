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

namespace uGovernIT.Web
{
    public partial class TicketHoldUnHold : UserControl
    {
        public string TicketId { get; set; }
        public string TitleText { get; set; }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        DataTable spList = null;
        string moduleTicketTable = string.Empty;
        UGITModule module = null;
        Ticket TicketRequest = null;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ProjectEstimatedAllocationManager projectEstimatedAllocationMGR;
        FieldConfigurationManager configFieldMGR;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            projectEstimatedAllocationMGR = new ProjectEstimatedAllocationManager(context);
            configFieldMGR = new FieldConfigurationManager(context);
            BindForm();
        }

        private void BindForm()
        {
            aspxdtOnHoldDate.MinDate = DateTime.Today.AddDays(1);
            aspxdtOnHoldDate.EditFormat = EditFormat.Date;
            ModuleViewManager moduleViewManager = new ModuleViewManager(context);
            module = moduleViewManager.GetByName(ModuleName);
            FieldConfiguration configField = null;
            if (module != null)
            {
                TicketRequest = new Ticket(context, module.ID);
                if (TicketRequest != null)
                {
                    //moduleTicketTable = module.ModuleName;
                    moduleTicketTable = module.ModuleTable;
                    if (!string.IsNullOrEmpty(moduleTicketTable))
                    {
                        spList = GetTableDataManager.GetTableData(moduleTicketTable, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
                        if (Action.ToLower() == "unhold")
                        {
                            lblMsg.Text = String.Format("Removing Hold From: {0} ", !string.IsNullOrWhiteSpace(TitleText) ? TitleText : TicketId.Replace(';', ','));
                            popedHoldComments.Visible = false;
                            popedUnHoldComments.Visible = true;
                            trHoldReason.Visible = false;
                            trHoldTill.Visible = false;
                            trHoldMessage.Visible = false;
                            HoldButton.Visible = false;
                            UnHoldButton.Visible = true;
                            return;
                        }
                        else
                        {
                            lblMsg.Text = String.Format("Putting on Hold: {0} ", !string.IsNullOrWhiteSpace(TitleText) ? TitleText : TicketId.Replace(';', ','));
                            popedHoldComments.Visible = true;
                            popedUnHoldComments.Visible = false;
                            trHoldReason.Visible = true;
                            trHoldTill.Visible = true;
                            trHoldMessage.Visible = true;
                            HoldButton.Visible = true;
                            UnHoldButton.Visible = false;
                            if (spList != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.OnHoldReasonChoice, spList))
                            {
                                //FieldConfigurationManager fieldManager = new FieldConfigurationManager();
                                //DataTable dt = fieldManager.GetFieldDataByFieldName(DatabaseObjects.Columns.OnHoldReason, module.ModuleName);
                                ////SPFieldChoice onHoldReasonFd = (SPFieldChoice)spList.Fields.GetFieldByInternalName(DatabaseObjects.Columns.OnHoldReason);
                                //foreach (DataRow choice in dt.Rows)
                                //{
                                //    ddlOnHoldReason.Items.Add(new ListItem(Convert.ToString(choice[DatabaseObjects.Columns.ID]), Convert.ToString(choice[DatabaseObjects.Columns.Name])));
                                //}
                                //FieldConfiguration configField = configFieldManager.Get(DatabaseObjects.Columns.OnHoldReason);                                                                                    
                                //FieldConfiguration configField = configFieldManager.GetFieldByFieldName(DatabaseObjects.Columns.OnHoldReasonChoice);
                                configField = configFieldMGR.GetFieldByFieldName(DatabaseObjects.Columns.OnHoldReasonChoice, moduleTicketTable);
                                if (configField != null)
                                {
                                    string[] dataRequestSource = UGITUtility.SplitString(configField.Data, Constants.Separator);
                                    ddlOnHoldReason.DataSource = dataRequestSource.ToList();
                                    ddlOnHoldReason.DataBind();
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void HoldButton_Click(object sender, EventArgs e)
        {
            FieldConfiguration configField = null;
            List<string> ticketIds = TicketId.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (module != null && ticketIds != null && !string.IsNullOrEmpty(moduleTicketTable))
            {
                foreach (string ticketId in ticketIds)
                {
                    DataRow saveTicket = GetTableDataManager.GetTableData(moduleTicketTable, $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows[0];
                    //FieldName='{DatabaseObjects.Columns.OnHoldReason}' and TenantId='{context.TenantID}'
                    if (spList != null && saveTicket != null)
                    {
                        bool ticketOnHold = UGITUtility.StringToBoolean(saveTicket[DatabaseObjects.Columns.TicketOnHold]);
                        if (!ticketOnHold)
                        {
                            string holdReason = string.Empty;
                            if (hdnOnHoldReason.Value == "0")
                            {
                                holdReason = ddlOnHoldReason.SelectedItem.Text;
                            }
                            else
                            {
                                holdReason = txtOnHoldReason.Text;
                                configField = configFieldMGR.GetFieldByFieldName(DatabaseObjects.Columns.OnHoldReasonChoice, moduleTicketTable);
                                if (configField != null && !string.IsNullOrWhiteSpace(holdReason))
                                {
                                    if (!string.IsNullOrWhiteSpace(configField.Data))
                                    {
                                        configField.Data = !string.IsNullOrWhiteSpace(hdnRequestOnHoldReason.Value)
                                            ? configField.Data.Replace(hdnRequestOnHoldReason.Value, holdReason)
                                            : !UGITUtility.SplitString(configField.Data, Constants.Separator).Contains(holdReason)
                                                ? configField.Data + Constants.Separator + holdReason
                                                : configField.Data;
                                    }
                                    else
                                    {
                                        configField.Data = holdReason;
                                    }
                                    configFieldMGR.Update(configField);
                                }
                            }

                            DateTime holdTill = aspxdtOnHoldDate.Date;
                            TicketRequest.HoldTicket(saveTicket, module.ModuleName, popedHoldComments.Text.Trim(), holdTill, holdReason);
                            TicketRequest.CommitChanges(saveTicket, "");
                            projectEstimatedAllocationMGR.UpdateProjectAllocationsAfterOnHold(ticketId);
                            TicketRequest.sendEmailToActionUsersOnHoldStage(saveTicket, module.ModuleName, UGITUtility.WrapCommentForEmail(popedHoldComments.Text.Trim(), "hold", holdTill, txtOnHoldReason.Text), Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]));
                        }
                    }
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void UnHoldButton_Click(object sender, EventArgs e)
        {
            List<string> ticketIds = TicketId.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (module != null && ticketIds != null && !string.IsNullOrEmpty(moduleTicketTable))
            {
                foreach (string ticketId in ticketIds)
                {
                    //DataRow saveTicket = GetTableDataManager.GetTableData(moduleTicketTable, DatabaseObjects.Columns.TicketId+"='"+ ticketId+"'").Rows[0];
                    DataRow saveTicket = GetTableDataManager.GetTableData(moduleTicketTable, $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows[0];
                    if (spList != null && saveTicket != null)
                    {
                        bool ticketOnHold = UGITUtility.StringToBoolean(saveTicket[DatabaseObjects.Columns.TicketOnHold]);
                        if (ticketOnHold)
                        {
                            TicketRequest.UnHoldTicket(saveTicket, string.Empty, popedUnHoldComments.Text.Trim());
                            TicketRequest.CommitChanges(saveTicket, Convert.ToString(HttpContext.Current.Request.Url));
                            projectEstimatedAllocationMGR.UpdatedAllocationType(ticketId, false);
                            TicketRequest.SendEmailToActionUsers(Convert.ToString(saveTicket[DatabaseObjects.Columns.ModuleStepLookup]), saveTicket, module.ModuleName,
                                                                     string.Format("This {0} has been taken off hold.<br />{1}", ModuleName, UGITUtility.WrapCommentForEmail(popedUnHoldComments.Text.Trim(), "UnHold")),
                                                                     string.Empty);
                        }
                    }
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
