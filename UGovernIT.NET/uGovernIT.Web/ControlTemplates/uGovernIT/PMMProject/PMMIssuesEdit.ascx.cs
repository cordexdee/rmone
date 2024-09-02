using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class PMMIssuesEdit : UserControl
    {
        public string ProjectID { get; set; }
        public int IssueID { get; set; }
        public int ViewType { get; set; }

        string moduleName = "PMM";
        bool isProjectManager;
        private DMSManagerService _dmsManagerService = null;
        StringBuilder linkFile = new StringBuilder();

        DataRow projectItem;
        UGITTask issueItem;
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        TicketManager TicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(HttpContext.Current.GetManagerContext());
                }
                return _dmsManagerService;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            UGITModule moduleConfig = ModuleManager.LoadByName(moduleName);   // uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, moduleName);
            List<string> viewFields = moduleConfig.List_ModuleUserTypes.Select(x => x.FieldName).ToList();
            viewFields.Add(DatabaseObjects.Columns.TicketId);
            viewFields.Add(DatabaseObjects.Columns.Title);
            viewFields.Add(DatabaseObjects.Columns.StageStep);
            viewFields.Add(DatabaseObjects.Columns.TicketStageActionUserTypes);
            projectItem = TicketManager.GetByTicketID(moduleConfig, ProjectID);  // Ticket.getCurrentTicket(moduleName, ProjectID, viewFields, true);
            Ticket ticket = new Ticket(HttpContext.Current.GetManagerContext(), moduleName);
            isProjectManager = Ticket.IsActionUser(HttpContext.Current.GetManagerContext(), projectItem, User);

            //if (IssueID <= 0)
            //    SPUtility.TransferToErrorPage("Malformed parameter");

            if (!isProjectManager || ViewType == 1)
            {
                ViewType = 1;
                txtTitle.Visible = false;
                lbTitle.Visible = true;
                txtDescription.Visible = false;
                lbDescription.Visible = true;
                dtcStartDate.Visible = false;
                lbStartDate.Visible = true;
                peAssignedTo.Visible = false;
                lbAssignedTo.Visible = true;
                btDelete.Visible = false;
            }

            issueItem = TaskManager.LoadByID(IssueID); // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMIssues, IssueID);

            if (issueItem != null)
            {
                bool archiveStatus = UGITUtility.StringToBoolean(issueItem.Deleted);
                if (archiveStatus)
                {
                    btDelete.Visible = true;
                    btMoveFromArchive.Visible = true;
                }
                else
                {
                    btArchiveIssue.Visible = true;
                }

                lbTitle.Text = txtTitle.Text = Convert.ToString(issueItem.Title);

                lbDescription.Text = txtDescription.Text = Convert.ToString(issueItem.Description);
                double pctComplete = issueItem.PercentComplete;
                //double.TryParse(Convert.ToDouble( issueItem.PercentComplete), out pctComplete);
                lbPctComplete.Text = txtPctComplete.Text = Math.Round(pctComplete * 100, 0).ToString();
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(Convert.ToString(issueItem.Status)));
                lbStatus.Text = ddlStatus.SelectedValue;

                ddlPriority.SelectedIndex = ddlPriority.Items.IndexOf(ddlPriority.Items.FindByValue(Convert.ToString(issueItem.Priority)));
                lbPripority.Text = ddlPriority.SelectedValue;

                ddlIssueImpact.SelectedIndex = ddlIssueImpact.Items.IndexOf(ddlIssueImpact.Items.FindByValue(Convert.ToString(issueItem.IssueImpact)));
                ddlIssueImpact.Text = ddlIssueImpact.SelectedValue;

                UserProfile lookupAuthorVal = UserManager.GetUserById(issueItem.Author);  // new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(issueItem.Author));
                if (lookupAuthorVal != null)
                    lblCreatedBy.Text = string.Format("Created at {0} by {1}", issueItem.Created, lookupAuthorVal.UserName);
                //UserProfile lookupEditorVal = UserManager.GetUserById(issueItem.editor);   // new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(issueItem["Editor"]));
                //if (lookupEditorVal != null)
                //    lblModifiedBy.Text = string.Format("Last modified at {0} by {1}", Convert.ToString(uHelper.GetSPItemValue(issueItem, DatabaseObjects.Columns.Modified)), lookupEditorVal.LookupValue);


                //SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(SPContext.Current.Web, Convert.ToString(uHelper.GetSPItemValue(issueItem, DatabaseObjects.Columns.AssignedTo)));
                //if (userValues != null && userValues.Count > 0)
                //{
                //    peAssignedTo.UpdateEntities(uHelper.getUsersListFromCollection(userValues));
                //    lbAssignedTo.Text = UserProfile.CommaSeparatedAccountsFrom(userValues, "; ");
                //}

                peAssignedTo.SetValues(issueItem.AssignedTo);

                DateTime startDate = issueItem.StartDate;   
                if (startDate != DateTime.MinValue)
                {
                    dtcStartDate.Date = startDate;
                    lbStartDate.Text = startDate.ToString("MMM-dd-yyyy");
                }

                DateTime dueDate = issueItem.DueDate;
                if(dueDate != DateTime.MinValue)
                {
                    dtcDueDate.Date = dueDate;
                    
                }

                DateTime resolutionDate = issueItem.ResolutionDate;  // DateTime.MinValue;
                //DateTime.TryParse(Convert.ToString(uHelper.GetSPItemValue(issueItem, DatabaseObjects.Columns.UGITResolutionDate)), out resolutionDate);
                if (resolutionDate != DateTime.MinValue)
                    dtcResolutionDate.Date = resolutionDate;

                txtResolution.Text = Convert.ToString(issueItem.Resolution); // Convert.ToString(uHelper.GetSPItemValue(issueItem, DatabaseObjects.Columns.UGITResolution));
                                                                             //List<HistoryEntry> comments = uHelper.GetHistory(issueItem, DatabaseObjects.Columns.UGITComment);
                                                                             //if (comments != null && comments.Count > 0)
                                                                             //{
                                                                             //    rComments.DataSource = comments;
                                                                             //    rComments.DataBind();
                                                                             //}
                if (!string.IsNullOrEmpty(issueItem.Attachments))
                {
                    FileUploadControl.SetValue(issueItem.Attachments);
                }
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            txtPctComplete.Attributes.Add("onblur", "modifyStatusFromPctComplete()");
            ddlStatus.Attributes.Add("onchange", "modifyPctCompleteFromStatus()");
        }

        protected void BtSaveTask_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(Page, "Task"))
                return;

            //Creates uservaluecollection object from peoplepicker control

            List<UserProfile> userMultiLookup = UserManager.GetUserInfosById(peAssignedTo.GetValues());

            List<UGITTask> issueList = TaskManager.Load(x => x.SubTaskType == "Issue");

            if (projectItem != null && issueItem != null)
            {
                issueItem.DueDate = new DateTime(1800, 1, 1);
                issueItem.ProposedDate = new DateTime(1800, 1, 1);
                issueItem.ResolutionDate = new DateTime(1800, 1, 1);
                issueItem.StartDate = new DateTime(1800, 1, 1);
                List<UserProfile> users = userMultiLookup;
                if (ViewType != 1)
                {
                    issueItem.TicketId = Convert.ToString(projectItem[DatabaseObjects.Columns.TicketId]);  // new SPFieldLookupValue(projectItem.ID, ProjectID);
                    issueItem.Title = txtTitle.Text.Trim();
                    issueItem.StartDate = dtcStartDate.Date;
                    issueItem.EndDate = dtcDueDate.Date;
                    if (dtcDueDate.Date != null && dtcResolutionDate.Date != new DateTime(1800, 1, 1))
                    {
                        issueItem.DueDate = dtcDueDate.Date;
                    }
                    else
                    {
                        issueItem.DueDate = dtcDueDate.Date;
                    }
                    issueItem.Description = txtDescription.Text.Trim();
                    issueItem.AssignedTo = peAssignedTo.GetValues();
                }

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

                issueItem.Priority = ddlPriority.SelectedValue;
                issueItem.IssueImpact = ddlIssueImpact.SelectedValue;

                //Remove deleted attachmen from existing attachment list
                //List<string> deleteFiles = UGITUtility.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
                // foreach (string dFile in deleteFiles)
                {
                    //for (int ii = 0; ii < issueItem.Attachments.Count; ii++)
                    //{
                    //    if (issueItem.Attachments[ii] == dFile)
                    //    {
                    //        issueItem.Attachments.Delete(dFile);
                    //    }
                    //}
                }

                HttpFileCollection file = Request.Files;
                for (int i = 0; i < file.Count; i++)
                {
                    if (file[i] != null && file[i].ContentLength > 0 && !string.IsNullOrEmpty(file[i].FileName))
                    {
                        HttpPostedFile filetype = Request.Files[i];
                        if (filetype.ContentType.Contains("application/x-msdownload"))
                        {
                            // lblerror.Visible = true;
                            //lblerror.Text = file[i].FileName + " has invalid file.";
                            return;
                        }

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(Request.Files[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(Request.Files[i].ContentLength);
                        }
                        //bool addFile = true;
                        //if (issueItem.Attachments.Count > 0)
                        //{
                        //    for (int ii = 0; ii < issueItem.Attachments.Count; ii++)
                        //    {
                        //        if (issueItem.Attachments[ii] == file[i].FileName)
                        //        {
                        //            addFile = false;
                        //            break;
                        //        }
                        //    }
                        //}

                        //if (addFile)
                        //    issueItem.Attachments.Add(file[i].FileName, fileData);
                    }
                }

                if (txtComment.Text.Trim() != string.Empty)
                {
                    //string comment = uHelper.GetVersionString(User, txtComment.Text.Trim(), issueItem, DatabaseObjects.Columns.UGITComment);
                    //issueItem[DatabaseObjects.Columns.UGITComment] = comment;
                }

                if (dtcResolutionDate.Date != null)
                    issueItem.ResolutionDate = dtcResolutionDate.Date;
                else
                    issueItem.ResolutionDate = new DateTime(1800, 1, 1);

                issueItem.Resolution = txtResolution.Text.Trim();
                issueItem.SubTaskType = "Issue";
                

                //Bind files
                if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                    issueItem.Attachments = FileUploadControl.GetValue();

                TaskManager.Update(issueItem);
                //issueItem.UpdateOverwriteVersion();
                //Mail to new assignees if issue is not completed

                if (isProjectManager && ViewType != 1 && pctComplete < 100 && status.ToLower() != Constants.Completed.ToLower() && userMultiLookup != null)
                {
                    //List<string> newAssignees = userMultiLookup.Where(x => !oldAssignees.Exists(y => y.LookupId == x.LookupId)).Select(x => x.LookupId.ToString()).ToList();
                    List<UserProfile> users1 = userMultiLookup;  // UserProfile.LoadUsersByIds(newAssignees);
                    List<string> emails = new List<string>();
                    StringBuilder mailToNames = new StringBuilder();

                    foreach (UserProfile userProfile in users1)
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
                        string title = issueItem.Title;

                        Dictionary<string, string> taskToEmail = new Dictionary<string, string>();
                        taskToEmail.Add("Type", "Project");
                        taskToEmail.Add("SubType", "Issue");
                        taskToEmail.Add("ProjectID", ProjectID);
                        taskToEmail.Add("ProjectTitle", Convert.ToString(projectItem[DatabaseObjects.Columns.Title]));
                        taskToEmail.Add("Title", title);
                        taskToEmail.Add("Description", issueItem.Description);
                        taskToEmail.Add("StartDate", dtcStartDate.Date.ToString("MMM-dd-yyyy"));

                        string url = string.Format("{0}?TicketId={1}&ModuleName={2}", UGITUtility.GetAbsoluteURL(Constants.HomePage), ProjectID, moduleName);
                        string emailFooter = UGITUtility.GetTaskDetailsForEmailFooter(taskToEmail, url, true, false, HttpContext.Current.GetManagerContext().TenantID);

                        StringBuilder taskEmail = new StringBuilder();
                        taskEmail.AppendFormat("Hi {0}<br /><br />", mailToNames.ToString());
                        taskEmail.AppendFormat("A new issue <b>\"{0}\"</b> has been assigned to you by {1}.<br>", title, User.UserName);
                        taskEmail.Append(emailFooter);

                        MailMessenger mailMessage = new MailMessenger(HttpContext.Current.GetManagerContext());
                        mailMessage.SendMail(string.Join(",", emails.ToArray()), "New Issue Assigned: " + title, "", taskEmail.ToString(), true, new string[] { }, true);
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

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (issueItem != null)
            {
                //issueItem.Delete();
                TaskManager.Delete(issueItem);
                //TaskCache.ReloadProjectTasks(Constants.PMMIssue, ProjectID);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btMoveFromArchive_Click(object sender, EventArgs e)
        {
            if (issueItem != null)
            {
                issueItem.Deleted = false;
                //issueItem.Update();
                TaskManager.Update(issueItem);
                //TaskCache.ReloadProjectTasks(Constants.PMMIssue, ProjectID);

                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btArchiveIssue_Click(object sender, EventArgs e)
        {
            if (issueItem != null)
            {
                issueItem.Deleted = true;
                //issueItem.Update();
                TaskManager.Update(issueItem);
                //TaskCache.ReloadProjectTasks(Constants.PMMIssue, ProjectID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btSaveAndNotify_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Bind comments list 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HistoryEntry hEntry = (HistoryEntry)e.Item.DataItem;

                Literal lCommentOwner = (Literal)e.Item.FindControl("lCommentOwner");
                Literal lCommentDate = (Literal)e.Item.FindControl("lCommentDate");
                Literal lCommentDesc = (Literal)e.Item.FindControl("lCommentDesc");

                lCommentOwner.Text = hEntry.createdBy;
                lCommentDate.Text = hEntry.created;
                lCommentDesc.Text = hEntry.entry;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                ShowAttachmentList();
            }
            catch (Exception) { }

            base.OnPreRender(e);
        }

        private void ShowAttachmentList()
        {
            // pAttachmentContainer.Visible = true;
            string element = string.Empty;
            //if (issueItem != null && issueItem.Attachments.Count > 0)
            //{
            //    for (int i = 0; i < issueItem.Attachments.Count; i++)
            //    {
            //        element = string.Format("<label onclick='removeAttachment(this);'><img src='/_layouts/15/images/ugovernit/delete-icon.png' alt='Delete'/></label>");
            //        pAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileread'><span> <a href='{0}{1}'> <b>{1}</b> </a></span> {2}</div>", issueItem.Attachments.UrlPrefix, issueItem.Attachments[i], element)));
            //    }
            //}
        }
    }
}
