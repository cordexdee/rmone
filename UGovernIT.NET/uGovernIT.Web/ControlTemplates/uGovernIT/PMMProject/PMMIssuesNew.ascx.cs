using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using System.IO;
using DevExpress.Web;
using uGovernIT.Utility.Entities;
using System.Data;

namespace uGovernIT.Web
{
    public partial class PMMIssuesNew : UserControl
    {
        public string ProjectID { get; set; }
        string moduleName = "PMM";
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        TicketManager TicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            txtPctComplete.Attributes.Add("onblur", "modifyStatusFromPctComplete()");
            ddlStatus.Attributes.Add("onchange", "modifyPctCompleteFromStatus()");
        }

        protected override void OnInit(EventArgs e)
        {
            dtcStartDate.Date = DateTime.Now;
            base.OnInit(e);
        }

        protected void BtSaveTask_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(Page, "Task"))
                return;

            List<UserProfile> userMultiLookup = UserManager.GetUserInfosById(peAssignedTo.GetValues());   // uHelper.GetFieldUserValueCollection(peAssignedTo.ResolvedEntities);

            List<string> viewFields = new List<string>();
            viewFields.Add(DatabaseObjects.Columns.Id);
            viewFields.Add(DatabaseObjects.Columns.Title);
            UGITModule module = ModuleManager.LoadByName(moduleName);
            DataRow projectItem = TicketManager.GetByTicketID(module, ProjectID); // Ticket.getCurrentTicket(moduleName, ProjectID, viewFields, true);
            List<UGITTask> issueList = TaskManager.Load(x=>x.SubTaskType == "Issue");   // SPListHelper.GetSPList(DatabaseObjects.Lists.PMMIssues);

            if (projectItem != null)
            {
                UGITTask issueItem = new UGITTask();
                issueItem.TicketId = Convert.ToString(projectItem[DatabaseObjects.Columns.TicketId]);   // new SPFieldLookupValue(projectItem.ID, ProjectID);


                //issueItem.TicketId = new SPFieldLookupValue(projectItem.ID, ProjectID);
                issueItem.Title = txtTitle.Text.Trim();

                double pctComplete = 0;
                double.TryParse(txtPctComplete.Text.Trim(), out pctComplete);
                string status = ddlStatus.SelectedValue;
                if (status.ToLower() == Constants.Completed.ToLower() || pctComplete >= 100)
                {
                    pctComplete = 100;
                    status = Constants.Completed;
                }
                issueItem.PercentComplete = Math.Round(pctComplete / 100, 4);
                issueItem.Status = status;
                issueItem.AssignedTo = peAssignedTo.GetValues();  // userMultiLookup;
                issueItem.Description = txtDescription.Text.Trim();

                issueItem.Priority = ddlPriority.SelectedValue;
                issueItem.IssueImpact = ddlIssueImpact.SelectedValue;

                if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                    issueItem.Attachments = FileUploadControl.GetValue();
                if (txtComment.Text.Trim() != string.Empty)
                {
                    //string comment = uHelper.GetVersionString(User.UserName, txtComment.Text.Trim(), issueItem, DatabaseObjects.Columns.UGITComment);
                    //issueItem.Comment = comment;
                }

                issueItem.StartDate = dtcStartDate.Date;
                issueItem.DueDate = dtcDueDate.MinDate;
                issueItem.ModuleNameLookup = module.ModuleName;
                if (dtcDueDate.Date != null && dtcResolutionDate.Date != new DateTime(1800, 1, 1))
                {
                    issueItem.DueDate = dtcDueDate.Date;
                }
                issueItem.ResolutionDate = new DateTime(1800,1,1);
                if (dtcResolutionDate.Date != null && dtcResolutionDate.Date!= DateTime.MinValue)
                {
                    issueItem.ResolutionDate = dtcResolutionDate.Date;
                }
                issueItem.Resolution = txtResolution.Text.Trim();
                issueItem.SubTaskType = "Issue";
                issueItem.ProposedDate = new DateTime(1800, 1, 1);
                TaskManager.Update(issueItem);
                //issueItem.UpdateOverwriteVersion();

                //Mail to new assignees if issue is not completed
                if (userMultiLookup != null && pctComplete < 100 && status.ToLower() != Constants.Completed.ToLower())
                {
                    List<UserProfile> users = userMultiLookup;   // UserProfile.LoadUsersByIds(userMultiLookup.Select(x => x.LookupId.ToString()).ToList());
                    List<string> emails = new List<string>();
                    StringBuilder mailToNames = new StringBuilder();

                    foreach (UserProfile userProfile in users)
                    {
                        if (userProfile != null && !string.IsNullOrEmpty(userProfile.Email))
                        {
                            emails.Add(userProfile.Email);
                            if (mailToNames.Length != 0)
                                mailToNames.Append(", ");
                            mailToNames.Append(userProfile.Name);
                        }
                    }

                    if (emails.Count > 0)
                    {
                        string title = Convert.ToString(issueItem.Title);

                        Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                        taskToEmail.Add("Type", "Project");
                        taskToEmail.Add("SubType", "Issue");
                        taskToEmail.Add("ProjectID", ProjectID);
                        taskToEmail.Add("ProjectTitle", Convert.ToString(projectItem[DatabaseObjects.Columns.Title]));
                        taskToEmail.Add("Title", title);
                        //taskToEmail.Add("Description", Convert.ToString(uHelper.GetSPItemValue(issueItem.Body)));
                        taskToEmail.Add("StartDate", dtcStartDate.Date.ToString("MMM-dd-yyyy"));

                        string url = string.Format("{0}?TicketId={1}&ModuleName={2}", UGITUtility.GetAbsoluteURL(Constants.HomePage), ProjectID, moduleName);
                        string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, HttpContext.Current.GetManagerContext().TenantID);
                        
                        StringBuilder taskEmail = new StringBuilder();
                        taskEmail.AppendFormat("Hi {0}<br /><br />", mailToNames.ToString());
                        taskEmail.AppendFormat("A new issue <b>\"{0}\"</b> has been assigned to you by {1}.<br>", title, User.UserName);
                        taskEmail.Append(emailFooter);

                        MailMessenger mailMessage = new MailMessenger(HttpContext.Current.GetManagerContext());
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), "New Issue Assigned: " + title, "", taskEmail.ToString(), true, new string[]{}, true);
                    }
                }
            }

            //TaskCache.ReloadProjectTasks(Constants.PMMIssue, ProjectID);

            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btDeleteMappingTask_Click(object sender, EventArgs e)
        {

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btSaveAndNotify_Click(object sender, EventArgs e)
        {

        }
    }
}
