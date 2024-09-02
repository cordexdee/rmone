
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class SendSurvey : UserControl
    {
        public bool show { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["SelectedModule"] != null && Convert.ToString(Request["SelectedModule"]) != "Generic")
                show = true;
        }
        UserProfile User;
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            string emailBody = ((AddTicketEmail)IdAddTicketEmail).EmailBody;
            string emailSubject = ((AddTicketEmail)IdAddTicketEmail).EmailSubject;
            string userids = ((BatchCreateWizard)IdBatchCreateWizard).GetSelectedUsers();
            string EmailTo = string.Empty;

            if (!string.IsNullOrEmpty(userids))
            {
                var userid = userids.Split(',');
                foreach (var item in userid)
                {
                    UserProfile user = UserManager.GetUserById(item);
                    if (user != null)
                    {
                        UserProfile userinfo = UserManager.LoadById(user.Id);
                        if (userinfo != null && !string.IsNullOrEmpty(userinfo.Email))
                        {
                            if (EmailTo != string.Empty)
                                EmailTo += ";";
                            EmailTo += userinfo.Email;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(EmailTo))
            {
                MailMessenger mail = new MailMessenger(HttpContext.Current.GetManagerContext());
                mail.SendMail(EmailTo, emailSubject, "", emailBody, true, new string[] { }, true);
            }

            uHelper.ClosePopUpAndEndResponse(Context);
        }
    }
}
