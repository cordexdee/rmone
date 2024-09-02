using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Web
{
    public partial class TicketEmailView : UserControl
    {
        public string ticketEmailID { get; set; }

        DataRow listItemCol;
        //DataRow listitem;
        //DataRow spTicket;

        UserProfile User;

        ApplicationContext context = HttpContext.Current.GetManagerContext();

        TicketManager ticketManager;
        private EmailsManager _emailsManager = null;

        protected EmailsManager EmailsManager
        {
            get
            {
                if (_emailsManager == null)
                {
                    _emailsManager = new EmailsManager(this.context);
                }
                return _emailsManager;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            Email email = new Email();
            //ticketEmailID = "39";
            ticketManager = new TicketManager(this.context);
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());

           // EmailsManager emailsManager = new EmailsManager(this.context);

            email = EmailsManager.LoadByID(Convert.ToInt64(ticketEmailID));
            if (email != null)
            {
                string TicketIIID = email.TicketId; //Convert.ToString(GetTableDataManager.GetTableData(DatabaseObjects.Tables.Emails).Rows[0][DatabaseObjects.Columns.TicketId]);

                string ModuleName = uHelper.getModuleNameByTicketId(TicketIIID);

                //listItemCol = ticketManager.GetAllTickets(moduleViewManager.GetByName(ModuleName)).Select(DatabaseObjects.Columns.TicketId + "='" + TicketIIID + "'");
                listItemCol = ticketManager.GetByTicketID(moduleViewManager.GetByName(ModuleName), TicketIIID); //(moduleViewManager.GetByName(ModuleName)).Select(DatabaseObjects.Columns.TicketId + "='" + TicketIIID + "'");

                string ticketID = string.Empty;

                if (listItemCol != null)
                {
                    ticketID = Convert.ToString(listItemCol[DatabaseObjects.Columns.TicketId]);
                }


                txtEmailTo.Text = email.EmailIDTo;
                txtEmailCC.Text = email.EmailIDCC;
                txtSubject.Text = email.MailSubject;
                htmlEditorTicketEmailBody.Html = email.EscalationEmailBody;

                //var emails = EmailsManager.Load().Where(x => x.TicketId == ticketID).ToList();

                // DataTable dt = UGITUtility.ToDataTable<Email>(emails);

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    listitem = dt.Rows[0];
                //txtEmailTo.Text = Convert.ToString(listitem[DatabaseObjects.Columns.EmailIDTo]);
                //txtEmailCC.Text = Convert.ToString(listitem[DatabaseObjects.Columns.EmailIDCC]);
                //txtSubject.Text = Convert.ToString(listitem[DatabaseObjects.Columns.MailSubject]);
                //htmlEditorTicketEmailBody.Html = Convert.ToString(listitem[DatabaseObjects.Columns.EscalationEmailBody]);
                /*
                if (emails.Count > 0)
                {
                    foreach (var item in emails)
                    {
                        txtEmailTo.Text = item.EmailIDTo;
                        txtEmailCC.Text = item.EmailIDCC;
                        txtSubject.Text = item.MailSubject;
                        htmlEditorTicketEmailBody.Html = item.EscalationEmailBody;
                    }
                }
                */
            }
           // }

            //foreach (String attachmentname in listitem.Attachments)
            //{
            //    String attachmentAbsoluteURL = listitem.Attachments.UrlPrefix // gets the containing directory URL
            //    + attachmentname;

            //    pAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileread'><span style='cursor:pointer;' onclick='window.open(\"{1}\")'>{0}</span></div>", attachmentname.Replace("'", "\'").Replace("\"", "\\\""), attachmentAbsoluteURL)));
            //}

            //string ticket = Convert.ToString(listitem[DatabaseObjects.Columns.TicketId]);
            //spTicket = Ticket.getCurrentTicket(uHelper.getModuleNameByTicketId(ticket), ticket);


            //UserProfile.UsersInfo actionUseruserInfo = UserProfile.GetUserInfo(spTicket, Convert.ToString(spTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]));
            //if (UserProfile.IsSuperAdmin() || UserProfile.IsTicketAdmin() || (actionUseruserInfo != null && actionUseruserInfo.userIds.Contains(SPContext.Current.Web.CurrentUser.ID)))
            //
            bool IsSuperAdmin = true;
            if (IsSuperAdmin)
            {
                lnkDelete.Visible = true;
            }
            else
            {
                lnkDelete.Visible = false;
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserProfile"] != null)
                User = (UserProfile)Session["UserProfile"];
        }

        protected void htmlEditorTicketEmailBody_HtmlCorrecting(object sender, DevExpress.Web.ASPxHtmlEditor.HtmlCorrectingEventArgs e)
        {
            e.Handled = true;
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
           var listitem= EmailsManager.LoadByID(Convert.ToInt64(ticketEmailID));

            string historyTxt = $"Email: {listitem.MailSubject} has been deleted";

            //EmailsManager emailsManager = new EmailsManager(context);

           EmailsManager.Delete(listitem);
            //listitem.Delete();
            //if (spTicket != null)
            //{
            //    uHelper.CreateHistory(User, historyTxt, spTicket,context);
            //}
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }
    }
}