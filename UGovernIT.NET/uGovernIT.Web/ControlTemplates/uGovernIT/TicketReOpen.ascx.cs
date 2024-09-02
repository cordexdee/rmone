
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

namespace uGovernIT.Web
{
    public partial class TicketReOpen : UserControl
    {
        public string TicketIds { get; set; }
        DataRow reOpenTicket;
        protected Ticket ticketRequest;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TicketIds))
            {
                int moduleId;
                ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
                UserProfileManager userProfileManager = new UserProfileManager(applicationContext) ;
                ConfigurationVariableManager ConfigurationVariable = new ConfigurationVariableManager(applicationContext);
                UserProfile currentuser = HttpContext.Current.GetManagerContext().CurrentUser;
                string[] ticketids = UGITUtility.SplitString(TicketIds, ",", StringSplitOptions.RemoveEmptyEntries);
                List<string> unauthorizedToReopen = new List<string>();
                bool enableTicketReopenByRequestor = ConfigurationVariable.GetValueAsBool(ConfigConstants.EnableTicketReopenByRequestor);
                foreach (string ticketId in ticketids)
                {
                    moduleId = uHelper.getModuleIdByTicketID(applicationContext, ticketId);
                    ticketRequest = new Ticket(applicationContext, moduleId);
                    reOpenTicket = Ticket.GetCurrentTicket(applicationContext,ticketRequest.Module.ModuleName, ticketId);
                    
                    // if current login user is action user, then authorized to re-open
                    if (Ticket.IsActionUser(applicationContext, reOpenTicket, currentuser))
                        continue;

                    // if current login user is requestor and we allow re-open by requestors, then authorized
                    if (enableTicketReopenByRequestor && userProfileManager.IsUserPresentInField(currentuser, reOpenTicket, DatabaseObjects.Columns.TicketRequestor))
                        continue;

                    // Else un-authorized to re-open
                    unauthorizedToReopen.Add(ticketId);
                }

                if (unauthorizedToReopen.Count > 0)
                {
                    unAuthorizedTickets.Visible = true;
                    unAuthorizedTickets.Text = "You are not authorized to re-open ticket(s): " + string.Join(", ", unauthorizedToReopen);
                }

                if (unauthorizedToReopen.Count == ticketids.Length)
                {
                    commentTr.Visible = false;
                    actonbtnTr.Visible = false;
                }
            }
        }



        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TicketIds))
            {

                int moduleId;

                string[] ticketids = UGITUtility.SplitString(TicketIds, ",");

                foreach (string id in ticketids)
                {
                    //if the open ticket ids contain blank spaces then continue
                    if (string.IsNullOrWhiteSpace(id))
                        continue;

                    moduleId = uHelper.getModuleIdByTicketID(HttpContext.Current.GetManagerContext(),id);
                    ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), moduleId);
                    reOpenTicket = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(),ticketRequest.Module.ModuleName, id);
                    int reopencount;
                    if (reOpenTicket["ReopenCount"] == DBNull.Value)
                    {
                        reopencount = 0;
                    }
                    else {
                        reopencount = reOpenTicket.Field<int>("ReopenCount");
                    }
                                           
                    reopencount = reopencount + 1;
                    reOpenTicket.BeginEdit();
                    reOpenTicket["ReopenCount"] = reopencount;

                    reOpenTicket.EndEdit();


                    LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(reOpenTicket);

                    string returnComment = UGITUtility.WrapComment(txtComment.Text.Trim(), "reopen");

                    if (currentStage.StageTypeChoice != StageType.Closed.ToString() || currentStage.ReturnStage == null)//if current ticket does't have return stage then continue
                        continue;


                    ticketRequest.Return(moduleId, reOpenTicket, returnComment, false, null);
                    ticketRequest.CommitChanges(reOpenTicket, "",Request.Url);


                    ///to set Re-Open count for report.
                    //if (UGITUtility.IfColumnExists(DatabaseObjects.Columns.ReopenCount, saveTicket.Table))
                    //{
                        
                    //        int reopencount = Convert.ToInt32(UGITUtility.StringToInt(saveTicket[DatabaseObjects.Columns.ReopenCount]));
                    //        reopencount = reopencount + 1;
                    //    ticketReq = reopencount;
                       
                    //}
                    //send mail
                    //ticketRequest.SendEmailToActionUsers(currentStage.ReturnStage.ID.ToString(), reOpenTicket, moduleId,null,null,UGITUtility.WrapCommentForEmail(returnComment, "reopen"));
                }

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        
    }        


    
}
