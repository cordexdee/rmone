using System;
using System.Collections.Generic;
using System.Text;
using uGovernIT.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Web;
using System.Data;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class SurveyFeedbackDetail : UserControl
    {
        SurveyFeedbackManager FeedBackManager = new SurveyFeedbackManager(HttpContext.Current.GetManagerContext());
        ServicesManager ServiceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {

            int feedbackId = 0;
            int.TryParse(Request["feedbackid"], out feedbackId);

            if (feedbackId <= 0)
            {
                return;
            }


            SurveyFeedback surveyItem = FeedBackManager.LoadByID(feedbackId);  // SPListHelper.GetSPListItem(DatabaseObjects.Tables.SurveyFeedback, feedbackId);
            if (surveyItem == null)
            {
                return;
            }

            //Ticket Link
            string moduleName = Convert.ToString(surveyItem.ModuleName);
            string ticketID = Convert.ToString(surveyItem.TicketId);

            Services service = ServiceManager.LoadSurvey(moduleName);

            lbTicketID.Text = ticketID;

            //Enter user detail in tooltip and put mailto
            string userEmails = string.Empty, userNames = string.Empty, userToolTip = string.Empty;
            UserProfile userInfo = new UserProfile();
            UserProfileManager userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            userInfo = userManager.GetUserById(surveyItem.CreatedBy);
            if (userInfo != null)
            {
                userEmails = userInfo.Email;
                userNames = userInfo.UserName.Replace(";", ", ");
                string currentUserName = HttpContext.Current.CurrentUser().UserName;
                StringBuilder href = new StringBuilder();
                href.AppendFormat("mailto:{0}?subject=Feedback for Request ID {1}&body=Hi {2}%0A%0A[Your content goes here]%0A%0AThanks, %0A {3}%0A%0A", userEmails, ticketID, userNames, currentUserName);
                lbSubmitBy.NavigateUrl = href.ToString();
                lbSubmitBy.Text = userNames;
                //lbSubmitBy.ToolTip = userInfo.userToolTip;
            }

            try
            {
                lbTitle.Text = Convert.ToString(surveyItem.TicketId);
                DataRow ticket = null;
                if (!string.IsNullOrEmpty(ticketID) && ticketID.Contains('-'))
                {
                    ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    
                    Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(HttpContext.Current.GetManagerContext());
                    ticket = ticketManager.GetByTicketID(moduleManager.LoadByName(moduleName), ticketID);   // Ticket.getCurrentTicket(moduleName, ticketID);
                    lbTitle.Text = Convert.ToString(ticket[DatabaseObjects.Columns.Title]);
                    lblPRP.Text = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(ticket[DatabaseObjects.Columns.TicketPRP]));
                    lblOwner.Text = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(ticket[DatabaseObjects.Columns.Owner]));
                    lblCategory.Text = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(ticket[DatabaseObjects.Columns.Category]));
                    string title = string.Format("{0}: {1}", ticketID, lbTitle.Text.Trim());
                    ticketdiv.Visible = true;
                    lbTicketID.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','90','90')", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), moduleName, ticketID), title);
                }


                string detail = Convert.ToString(surveyItem.Description);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(detail);
                List<ServiceQuestionAnswer> questAnsList = new List<ServiceQuestionAnswer>();
                questAnsList = (List<ServiceQuestionAnswer>)uHelper.DeSerializeAnObject(xmlDoc, questAnsList);

                foreach (ServiceQuestionAnswer ans in questAnsList)
                {
                    ServiceQuestion quest = service.Questions.FirstOrDefault(x => x.TokenName == ans.Token && x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating);
                    if (quest != null)
                    {
                        ans.Answer = string.Format("Rating: {0}", ans.Answer);
                    }
                }

                rFeedbackD.DataSource = questAnsList;
                rFeedbackD.DataBind();
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
        }
    }
}
