using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class TicketAction : System.Web.UI.UserControl
    {
        public string ModuleName { get; set; }
        public string TicketCommand { get; set; }
        public string TicketPublicID { get; set; }
        DataRow currentTicket;
        ApplicationContext context = null;
        Ticket ticketRequest;
        int moduleid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            ticketRequest = new Ticket(context, ModuleName);
            currentTicket = Ticket.GetCurrentTicket(context, ModuleName, TicketPublicID);
            moduleid = uHelper.getModuleIdByModuleName(context, ModuleName);
            if (!Ticket.IsActionUser(context, currentTicket, context.CurrentUser))
            {
                isAuthrizedPanel.Visible = true;
                isAuthrizedMsg.Text = "You are not authorized to perform this action";
                return;
            }

            if (TicketCommand == "approve")
            {
                approve.Visible = true;
            }
            else if (TicketCommand == "reject")
            {
                reject.Visible = true;
            }
            else if (TicketCommand == "return")
            {
                returnTicket.Visible = true;
            }
            else if (TicketCommand == "hold")
            {
                putOnHold.Visible = true;
                if (currentTicket != null && ddlOnHoldReason.Items.Count == 0 && uHelper.IfColumnExists(DatabaseObjects.Columns.OnHoldReason, currentTicket.Table))
                {
                    string[] onHoldReasonFd = UGITUtility.SplitString(currentTicket[DatabaseObjects.Columns.OnHoldReason], Constants.Separator6);
                    foreach (string choice in onHoldReasonFd)
                    {
                        ddlOnHoldReason.Items.Add(new ListItem(choice, choice));
                    }
                }
            }
            else if (TicketCommand == "unhold")
            {
                unHold.Visible = true;
            }
            if (ModuleName == "PMM")
            {
                if (TicketCommand == "approvebudget")
                {
                    approveBudget.Visible = true;
                }
                else if (TicketCommand == "rejectbudget")
                {
                    rejectBudget.Visible = true;
                }
            }
            base.OnInit(e);
        }
        private void ActionButtonClick(string action)
        {
            if (action == "approve")
            {
                ticketRequest.Approve(HttpContext.Current.CurrentUser(), currentTicket);
                ticketRequest.CommitChanges(currentTicket);

                //Send Email notification.
                ticketRequest.SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, Convert.ToString(moduleid));
            }
            else if (action == "reject")
            {
                string rejectComments = UGITUtility.WrapComment(popedRejectComments.Text, "reject");
                ticketRequest.Reject(currentTicket, rejectComments);
                ticketRequest.CommitChanges(currentTicket);

                //Send Email notification.
                ticketRequest.SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, Convert.ToString(moduleid));
            }
            else if (action == "hold")
            {
                DateTime holdTill = new DateTime(aspxdtOnHoldDate.Date.Year, aspxdtOnHoldDate.Date.Month, aspxdtOnHoldDate.Date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                ticketRequest.HoldTicket(currentTicket, Convert.ToString(moduleid), popedHoldComments.Text.Trim(), holdTill, ddlOnHoldReason.SelectedValue);
                ticketRequest.CommitChanges(currentTicket);

                //Send Email notification.
                ticketRequest.sendEmailToActionUsersOnHoldStage(currentTicket, Convert.ToString(moduleid), UGITUtility.WrapCommentForEmail(popedHoldComments.Text.Trim(), "hold", holdTill, ddlOnHoldReason.SelectedValue));
            }
            else if (action == "return")
            {
                string returnComments = UGITUtility.WrapComment(popedReturnComments.Text.Trim(), "return");
                ticketRequest.Return(moduleid, currentTicket, returnComments);
                ticketRequest.CommitChanges(currentTicket);

                //Send Email notification.
                ticketRequest.SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, Convert.ToString(moduleid));
            }
            else if (action == "unhold")
            {
                string unholdComments = UGITUtility.WrapComment(popedUnHoldComments.Text.Trim(), "unhold");
                ticketRequest.UnHoldTicket(currentTicket, ModuleName, unholdComments);
                ticketRequest.CommitChanges(currentTicket);

                AgentJobHelper agentHelper = new AgentJobHelper(HttpContext.Current.GetManagerContext());
                agentHelper.CancelUnHoldTicket(TicketPublicID);

                //Send Email notification.
                ticketRequest.SendEmailToActionUsers(Convert.ToString(currentTicket[DatabaseObjects.Columns.ModuleStepLookup]), currentTicket, Convert.ToString(moduleid),
                                                    string.Format("This {0} has been taken off hold.<br />{1}", UGITUtility.moduleTypeName("NPR"), UGITUtility.WrapCommentForEmail(popedUnHoldComments.Text.Trim(), "UnHold")), string.Empty);

            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        //PMM budget handeling
        private void ActionOnProject(string command)
        {
            int status = -1;
            if (command == "approvebudget")
                status = (int)(Enums.BudgetStatus.Approve);
            else if (command == "rejectbudget")
                status = (int)(Enums.BudgetStatus.Reject);
            else
                return;

            int pmmId = Convert.ToInt32(currentTicket["ID"]);
            //DataRow[] pendingBudgetCollection= GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleBudget, $"{DatabaseObjects.Columns.TenantID}= '{context.TenantID}'and {DatabaseObjects.Columns.ModuleNameLookup}= '{ModuleName}' and {DatabaseObjects.Columns.TicketPMMIdLookup}= '{pmmId}', {DatabaseObjects.Columns.BudgetStatus}= {(int)Enums.BudgetStatus.PendingApproval}").Select();
            ModuleBudgetManager objmodulebgtmgr = new ModuleBudgetManager(context);
            List<ModuleBudget> budgets = objmodulebgtmgr.Load();
            if (budgets.Count > 0)
            {
                if (status == (int)Enums.BudgetStatus.Approve)
                {
                    foreach (ModuleBudget bgt in budgets)
                    {
                        //PMMBudget oldBudget = (PMMBudget)bgt.Clone();
                        ModuleBudget oldBudget = ModuleBudgetManager.Clone(bgt);
                        bgt.BudgetAmount = bgt.BudgetAmount + bgt.UnapprovedAmount;
                        bgt.UnapprovedAmount = 0;
                        bgt.BudgetStatus = status;
                        bgt.Comment = popedApproveBudgetComments.Text.Trim();
                        objmodulebgtmgr.Update(bgt);

                        // Update Monthly distribution of budget item in PMM Monthly budget list.
                        objmodulebgtmgr.UpdateProjectMonthlyDistributionBudget(oldBudget, bgt);
                        BudgetActualsManager objBudgetActualsManager = new BudgetActualsManager(context);
                        // Update Monthly distribution of budget's actual in PMM Monthly budget list.
                        objBudgetActualsManager.UpdateProjectMonthlyDistributionActual(bgt.TicketId, ModuleName);

                        // Update budget's subcategory monthly distribution.
                        objmodulebgtmgr.UpdateNonProjectMonthlyDistributionBudget(bgt.BudgetCategoryLookup);

                        // Update budget's actuals monthly distribution by subcategory.
                        objBudgetActualsManager.UpdateNonProjectMonthlyDistributionActual(bgt.BudgetCategoryLookup);
                    }

                    //Update total cost
                    objmodulebgtmgr.Load(TicketPublicID, false);
                }
                else
                {
                    foreach (ModuleBudget bgt in budgets)
                    {
                        bgt.BudgetStatus = status;
                        bgt.Comment = popedRejectBudgetComments.Text.Trim();
                        objmodulebgtmgr.Update(bgt);
                    }
                }

                List<ModuleBudget> budgetObjList = objmodulebgtmgr.Load(x => x.TicketId == TicketPublicID);
                if (budgetObjList.Count > 0)
                {
                    Ticket ticketRequest = new Ticket(context, ModuleName);
                    currentTicket[DatabaseObjects.Columns.TicketTotalCost] = budgetObjList.Sum(x => x.BudgetAmount);
                    currentTicket.AcceptChanges();
                    ticketRequest.UpdateTicketCache(currentTicket, ModuleName);
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void HoldButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick(TicketCommand);
        }

        protected void UnHoldButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick(TicketCommand);
        }

        protected void returnButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick(TicketCommand);
        }

        protected void rejectButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick(TicketCommand);
        }

        protected void approveButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick(TicketCommand);
        }

        protected void rejectBudgetButton_Click(object sender, EventArgs e)
        {
            ActionOnProject(TicketCommand);
        }

        protected void approveBudgetButton_Click(object sender, EventArgs e)
        {
            ActionOnProject(TicketCommand);
        }
    }
}