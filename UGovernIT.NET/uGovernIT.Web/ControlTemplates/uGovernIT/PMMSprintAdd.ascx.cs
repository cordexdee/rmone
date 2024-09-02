using System;
using System.Collections.Generic;
using System.Data;
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
using uGovernIT.Web.ControlTemplates.Shared;

namespace uGovernIT.Web
{
    public partial class PMMSprintAdd : UserControl
    {
        public string ticketId { get; set; }
        public int TaskId { get; set; }
        public bool IsNew { get; set; }
        public DataTable dtSelectedTasks { get; set; }
        public bool ReadOnly;
        public long Id = 0;
        public string DocumentManagementUrl { get; set; }
        public string FolderName { get; set; }
        public string DocumentName { get; set; }
        public string Iframe { get; set; }
        public bool IsTabActive { get; set; }
        protected string projectID = string.Empty;
        protected string projectPublicID = string.Empty;
        public string ModuleName { get; set; }

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        PMMReleaseManager pMMReleaseManager = null;
        SprintManager sprintManager = null;
        SprintTaskManager sprintTaskManager = null;
        UserProfile currentUser = null;
        UserProfileManager UserManager = null;
        TicketManager ticketManager = null;
        FieldConfigurationManager fieldManager = null;
        FieldConfiguration field = null;
        private DMSManagerService _dmsManagerService = null;
        StringBuilder linkFile = new StringBuilder();

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
            pMMReleaseManager = new PMMReleaseManager(context);
            sprintManager = new SprintManager(context);
            sprintTaskManager = new SprintTaskManager(context);
            currentUser = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetUserManager();
            ticketManager = new TicketManager(context);
            fieldManager = new FieldConfigurationManager(context);
            UploadAndLinkDocuments uploadAndLinkDocuments = new UploadAndLinkDocuments();

            Id = Convert.ToInt64(ticketManager.GetSingleValueByTicketID("PMM", DatabaseObjects.Columns.ID, ticketId));

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            Guid newFrameId = Guid.NewGuid();

            DocumentManagementUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&frameObjId={1}&control=DocumentControl&ticketId={0}&module={2}",
                                                           projectID, newFrameId, ModuleName));
            FolderName = Convert.ToString(Request["folderName"]);

            if (string.IsNullOrEmpty(ModuleName))

                ModuleName = string.IsNullOrEmpty(Request["ModuleName"]) ? Request["Module"] : Request["ModuleName"];

            projectID = projectPublicID = string.IsNullOrEmpty(Request["PublicTicketID"]) ? Request["ticketId"] : Request["PublicTicketID"];

            FolderName = Convert.ToString(Request["folderName"]);

            IsTabActive = Convert.ToBoolean(Request["isTabActive"]);
            if (!IsPostBack)
            {
                BindSprints();
                BindReleases();
                if (!IsNew)
                {
                    SetData();
                    lnkDeleteTask.Visible = true;
                    trComment.Visible = true;
                    trAttachment.Visible = true;
                }
                else
                {
                    trComment.Visible = false;
                    trAttachment.Visible = false;
                }
            }
        }

        private void BindReleases()
        {
            List<ProjectReleases> pmmReleaseList = pMMReleaseManager.Load(x => x.PMMIdLookup == Id);
            if (pmmReleaseList != null && pmmReleaseList.Count > 0)
            {
                ddlReleases.DataSource = pmmReleaseList;
                ddlReleases.DataTextField = DatabaseObjects.Columns.Title;
                ddlReleases.DataValueField = DatabaseObjects.Columns.Id;
                ddlReleases.DataBind();
            }

            ddlReleases.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void BindSprints()
        {
            List<Sprint> pmmSprintList = sprintManager.Load(x => x.PMMIdLookup == Id);
            if (pmmSprintList != null && pmmSprintList.Count > 0)
            {
                ddlSprints.DataSource = pmmSprintList;
                ddlSprints.DataTextField = DatabaseObjects.Columns.Title;
                ddlSprints.DataValueField = DatabaseObjects.Columns.Id;
                ddlSprints.DataBind();
            }

            ddlSprints.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void SetData()
        {
            List<SprintTasks> pmmSprintTaskList = sprintTaskManager.Load(x => x.ID == Convert.ToInt64(TaskId));

            if (pmmSprintTaskList != null && pmmSprintTaskList.Count > 0)
            {
                SprintTasks item = pmmSprintTaskList[0];
                txtTitle.Text = Convert.ToString(item.Title); //Convert.ToString(item[DatabaseObjects.Columns.Title]);
                txtEstimatedHours.Text = Convert.ToString(item.TaskEstimatedHours); //Convert.ToString(item[DatabaseObjects.Columns.TaskEstimatedHours]);
                txtPctComplete.Text = Convert.ToString(Convert.ToDouble(item.PercentComplete)); //Convert.ToString(Convert.ToDouble(item[DatabaseObjects.Columns.PercentComplete]) * 100);
                //if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.TaskStatus])))
                if (!string.IsNullOrEmpty(Convert.ToString(item.TaskStatus)))
                {
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(Convert.ToString(item.TaskStatus)));
                }
                trAssignedTo.Visible = true;
                if (!string.IsNullOrEmpty(Convert.ToString(item.AssignedTo)) || !string.IsNullOrEmpty(Convert.ToString(item.SprintLookup)))
                {
                    string userLookups = Convert.ToString(item.AssignedTo);
                    peAssignedTo.SetValues(userLookups);

                    trPctComplete.Visible = true;
                    trStatus.Visible = true;

                }
                if (!string.IsNullOrEmpty(item.Comment))
                {
                    List<HistoryEntry> history = uHelper.GetHistory(Convert.ToString(item.Comment), false);
                    if (history != null && history.Count > 0)
                    {
                        rComments.DataSource = history;
                        rComments.DataBind();
                    }
                }
                //txtDescription.Text = uHelper.StripHTML(Convert.ToString(item[DatabaseObjects.Columns.Description]));
                txtDescription.Text = UGITUtility.StripHTML(Convert.ToString(item.Body));
                // currently using Comment as Description is not available in Table, need to check this later.

                long releaseLookup = item.ReleaseLookup ?? 0;
                if (releaseLookup > 0)
                {
                    ddlReleases.SelectedIndex = ddlReleases.Items.IndexOf(ddlReleases.Items.FindByValue(Convert.ToString(releaseLookup)));
                }
                else
                {
                    ddlReleases.SelectedIndex = 0;
                }
                long sprintLookup = item.SprintLookup ?? 0;
                if (sprintLookup > 0)
                {
                    ddlSprints.SelectedIndex = ddlSprints.Items.IndexOf(ddlSprints.Items.FindByValue(Convert.ToString(sprintLookup)));
                }
                else
                {
                    ddlSprints.SelectedIndex = 0;
                }
                ShowAttachmentList(item);

                if (!string.IsNullOrEmpty(item.Attachments))
                {
                    List<DMSDocument> attachedFileList = DMSManagerService.GetFileListByFileId(item.Attachments);

                    foreach (var file in attachedFileList)
                    {
                        linkFile = linkFile.Append($"<a id='file_{file.FileId}' style='cursor: pointer;' onclick='window.downloadSelectedFile({file.FileId})'>{file.FileName}</a><img src='/content/images/close-red.png' id='img_{file.FileId}' class='cancelUploadedFiles' onclick='window.deleteLinkedDocument(\"" + file.FileId + "\")'></img><br/>");
                    }

                    bindMultipleLink.InnerHtml = Convert.ToString(linkFile);
                }
            }
        }

        protected void btAddTask_Click(object sender, EventArgs e)
        {
            int sprintId = 0;
            int newsprintId = 0;
            List<SprintTasks> pmmSprintTaskList = sprintTaskManager.Load(x => x.PMMIdLookup == Id).OrderBy(x => x.SprintOrder).ToList();
            SprintTasks item = new SprintTasks();
            if (IsNew)
            {
                if (pmmSprintTaskList != null && pmmSprintTaskList.Count > 0)
                {
                    int count = pmmSprintTaskList.Where(x => x.Title.Equals(txtTitle.Text.Trim(), StringComparison.InvariantCultureIgnoreCase)).Count();
                    if (count > 0)
                    {
                        lbTitle.Text = "Task with the same name already exists in " + ticketId;
                        lbTitle.Visible = true;
                        return;
                    }
                }
                //item = lstSprintTask.AddItem();
                // check later
            }
            else
            {
                //item = coll.GetItemById(TaskId);
                item = pmmSprintTaskList.Where(x => x.ID == TaskId).FirstOrDefault();
            }

            string userMultiLookup = peAssignedTo.GetValues();

            if (item != null)
            {
                item.Title = txtTitle.Text.Trim();
                item.TaskEstimatedHours = !string.IsNullOrEmpty(txtEstimatedHours.Text.Trim()) ? Convert.ToInt32(txtEstimatedHours.Text.Trim()) : 0;
                item.Body = txtDescription.Text.Trim();
                if (!IsNew)
                {
                    item.AssignedTo = userMultiLookup;
                    item.TaskStatus = ddlStatus.SelectedItem.Text;

                    if (Convert.ToInt32(txtPctComplete.Text) > 100)
                        item.PercentComplete = 1;
                    else if (Convert.ToInt32(txtPctComplete.Text) < 0)
                        item.PercentComplete = 0;
                    else
                        item.PercentComplete = Convert.ToInt32(txtPctComplete.Text.Trim()); //Convert.ToInt32(txtPctComplete.Text.Trim()) / 100;

                    if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
                    {
                        //string comment = uHelper.GetVersionString(SPContext.Current.Web.CurrentUser, txtComment.Text.Trim(), item, DatabaseObjects.Columns.UGITComment);
                        DataRow drItem = UGITUtility.ObjectToData(item).Select()[0];
                        string comment = uHelper.GetVersionString(currentUser.Id, txtComment.Text.Trim(), drItem, DatabaseObjects.Columns.Comment);
                        item.Comment = comment;
                    }
                    int sprintLookup = Convert.ToInt32(item.SprintLookup);
                    if (sprintLookup > 0)
                    {
                        sprintId = sprintLookup;
                    }
                }
                else
                {
                    DataRow projectItem = Ticket.GetCurrentTicket(context, "PMM", ticketId); //Ticket.getCurrentTicket("PMM", ticketId);
                    item.TaskStatus = ddlStatus.Items[0].Text;
                    item.PercentComplete = 0;
                    item.ItemOrder = (pmmSprintTaskList != null) ? pmmSprintTaskList.Count + 1 : 1;  //(coll != null) ? coll.Count + 1 : 1;
                    item.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]);
                }

                if (ddlSprints.SelectedIndex != 0)
                {
                    int newSelectedSprint = Convert.ToInt32(ddlSprints.SelectedItem.Value);
                    if (newSelectedSprint > 0)
                    {
                        newsprintId = newSelectedSprint;
                    }

                    item.SprintLookup = newSelectedSprint;

                    if (IsNew)
                        item.SprintOrder = (pmmSprintTaskList.Count != 0) ? Convert.ToInt32(pmmSprintTaskList[0].SprintOrder) + 1 : 1; //(coll.Count != 0) ? Convert.ToInt32(coll[0][DatabaseObjects.Columns.SprintOrder]) + 1 : 1;
                }
                else
                {
                    item.SprintLookup = 0; //null;
                }
                if (ddlReleases.SelectedIndex != 0)
                {
                    item.ReleaseLookup = Convert.ToInt64(ddlReleases.SelectedItem.Value);
                }
                else
                {
                    item.ReleaseLookup = null;
                }
                //item.Update();
                //@chetan
                //item.Attachments = ugitupAttachments.GetValue();
                var currentLink = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);

                if (!string.IsNullOrEmpty(item.Attachments) && !string.IsNullOrEmpty(currentLink))
                {
                    var attachment = item.Attachments + "," + currentLink;
                    item.Attachments = attachment;
                }
                else
                    item.Attachments = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);
                //item.Attachments = currentLink;


                if (item.ID <= 0)
                    sprintTaskManager.Insert(item);
                else
                    sprintTaskManager.Update(item);

                if (sprintId > 0)
                {
                    UpdateRemainingHoursSprint(sprintId);
                }
                if (newsprintId > 0 && newsprintId != sprintId)
                {
                    if (dtSelectedTasks != null && dtSelectedTasks.Rows.Count > 0)
                        dtSelectedTasks.Rows.Clear();
                    UpdateRemainingHoursSprint(newsprintId);
                }
            }


            if (IsNew)
            {
                ReOrderGridRows(Convert.ToInt32(ddlSprints.SelectedValue), DatabaseObjects.Columns.ItemOrder);

                if (ddlSprints.SelectedValue != "0")
                    ReOrderGridRows(Convert.ToInt32(ddlSprints.SelectedValue), DatabaseObjects.Columns.SprintOrder);
            }
            else
            {

                if (ddlSprints.SelectedValue != "0")
                {
                    ReOrderGridRows(sprintId, DatabaseObjects.Columns.SprintOrder);
                    ReOrderGridRows(Convert.ToInt32(ddlSprints.SelectedValue), DatabaseObjects.Columns.SprintOrder);
                }
                else
                {
                    ReOrderGridRows(sprintId, DatabaseObjects.Columns.SprintOrder);
                    ReOrderGridRows(Convert.ToInt32(ddlSprints.SelectedValue), DatabaseObjects.Columns.ItemOrder);
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void RComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HistoryEntry hEntry = (HistoryEntry)e.Item.DataItem;

                Literal lCommentOwner = (Literal)e.Item.FindControl("lCommentOwner");
                Literal lCommentDate = (Literal)e.Item.FindControl("lCommentDate");
                Literal lCommentDesc = (Literal)e.Item.FindControl("lCommentDesc");

                field = fieldManager.GetFieldByFieldName(DatabaseObjects.Columns.CreatedByUser);
                if (field != null)
                {
                    if (field.Datatype == "UserField")
                    {
                        List<uGovernIT.Utility.Entities.UserProfile> userProfiles = HttpContext.Current.GetUserManager().GetUserInfosById(hEntry.createdBy);
                        if (userProfiles != null && userProfiles.Count > 0)
                        {
                            lCommentOwner.Text = string.Join(Constants.Separator6, userProfiles.Select(x => x.Name));
                        }
                    }
                }

                //lCommentOwner.Text = hEntry.createdBy;
                lCommentDate.Text = hEntry.created;
                lCommentDesc.Text = hEntry.entry;
            }
        }

        protected void lnkDeleteTask_Click(object sender, EventArgs e)
        {
            if (TaskId > 0)
            {
                int sprintId = 0;
                SprintTasks backLogTask = sprintTaskManager.Load(x => x.ParentTask == TaskId).FirstOrDefault();
                if (backLogTask != null)
                {
                    int sprintLookup = Convert.ToInt32(backLogTask.SprintLookup);
                    if (sprintLookup > 0)
                    {
                        sprintId = sprintLookup;
                    }
                    //backLogTask.Delete();
                    sprintTaskManager.Delete(backLogTask);
                    if (sprintId > 0)
                    {
                        UpdateRemainingHoursSprint(sprintId);
                    }
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
        }

        private void UpdateRemainingHoursSprint(int sprintId)
        {
            if (dtSelectedTasks == null || dtSelectedTasks.Rows.Count == 0)
            {
                CreateSprintTaskTable(sprintId);
            }
            Sprint selectedSprint = sprintManager.LoadByID(Convert.ToInt32(sprintId));

            double estimatedHours = 0;
            double remainingHours = 0;
            double pctComplete = 0;
            if (dtSelectedTasks != null && dtSelectedTasks.Rows.Count > 0)
            {
                double sprintRemainingHours = Convert.ToDouble(dtSelectedTasks.Compute(string.Format("SUM({0})", "TaskRemainingHours"), string.Empty));
                double sprintEstimatedHours = Convert.ToDouble(dtSelectedTasks.Compute(string.Format("Sum({0})", DatabaseObjects.Columns.TaskEstimatedHours), string.Empty));
                remainingHours = Math.Round(sprintRemainingHours, 2);
                estimatedHours = Math.Round(sprintEstimatedHours, 2);
                if (estimatedHours > 0)
                    pctComplete = Math.Round((((estimatedHours - remainingHours) / estimatedHours) * 100), 2);
                else
                    pctComplete = 0;
            }
            selectedSprint.RemainingHours = Convert.ToInt32(remainingHours);
            selectedSprint.TaskEstimatedHours = Convert.ToInt32(estimatedHours);
            if (pctComplete > 0)
                selectedSprint.PercentComplete = Convert.ToInt32(pctComplete);
            else
                selectedSprint.PercentComplete = 0;

            sprintManager.Update(selectedSprint);

            new UGITTaskManager(context).UpdateProjectTask(UGITUtility.ObjectToData(selectedSprint).Select()[0]);
        }

        private void CreateSprintTaskTable(int sprintId)
        {
            if (dtSelectedTasks == null)
            {
                dtSelectedTasks = new DataTable();
                if (dtSelectedTasks.Columns == null || dtSelectedTasks.Columns.Count == 0)
                {
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Id));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Title));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.SprintOrder));
                    dtSelectedTasks.Columns.Add(new DataColumn("SprintId", typeof(int)));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.PercentComplete));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.Status));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.AssignedTo));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.TaskEstimatedHours, typeof(float)));
                    dtSelectedTasks.Columns.Add(new DataColumn("TaskRemainingHours", typeof(float)));
                    dtSelectedTasks.Columns.Add(new DataColumn(DatabaseObjects.Columns.RemainingHours, typeof(string)));
                }
            }
            if (dtSelectedTasks != null && dtSelectedTasks.Columns != null && dtSelectedTasks.Columns.Count > 0)
            {
                List<SprintTasks> collSprints = sprintTaskManager.Load(x => x.PMMIdLookup == Convert.ToInt64(ID) && x.SprintLookup == sprintId);

                if (collSprints != null && collSprints.Count > 0)
                {
                    List<string> assignees = new List<string>();

                    foreach (SprintTasks item in collSprints)
                    {
                        assignees.Clear();
                        assignees = UGITUtility.ConvertStringToList(Convert.ToString(item.AssignedTo), ",");

                        string assignedTo = UserManager.GetUserOrGroupName(assignees);
                        int sprintLookup = Convert.ToInt32(item.SprintLookup);

                        DataRow dr = dtSelectedTasks.NewRow();
                        dr[DatabaseObjects.Columns.PercentComplete] = string.Format("{0}%", Convert.ToDouble(item.PercentComplete) * 100);
                        dr[DatabaseObjects.Columns.Status] = item.TaskStatus;
                        dr[DatabaseObjects.Columns.AssignedTo] = string.IsNullOrEmpty(assignedTo) ? assignedTo : string.Empty;
                        dr[DatabaseObjects.Columns.TaskEstimatedHours] = item.TaskEstimatedHours;
                        dr[DatabaseObjects.Columns.Id] = item.ID;
                        dr[DatabaseObjects.Columns.Title] = item.Title;
                        dr[DatabaseObjects.Columns.SprintOrder] = item.SprintOrder;
                        dr["SprintId"] = sprintLookup > 0 ? sprintLookup : 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(item.PercentComplete)) && !string.IsNullOrEmpty(Convert.ToString(item.TaskEstimatedHours)))
                        {
                            Double remaininghrs = (1 - (Convert.ToDouble(item.PercentComplete) / 100)) * (Convert.ToDouble(item.TaskEstimatedHours));
                            remaininghrs = Math.Round(remaininghrs, 2);
                            dr["TaskRemainingHours"] = remaininghrs;
                            dr[DatabaseObjects.Columns.RemainingHours] = string.Format("{0} / {1}", remaininghrs, item.TaskEstimatedHours);
                        }
                        dtSelectedTasks.Rows.Add(dr);
                    }
                    dtSelectedTasks.AcceptChanges();
                }
            }
        }

        private void ShowAttachmentList(SprintTasks item)
        {
            if (item != null)
            {
                //@chetan
                //pAttachmentContainer.Visible = true;

                // if(!string.IsNullOrEmpty(item.Attachments))
                // ugitupAttachments.SetValue(item.Attachments);
            }
        }

        //new method for reorder while add and update sprint task...
        private void ReOrderGridRows(int selectedSprintId, string colName)
        {
            List<SprintTasks> newsplstCol = new List<SprintTasks>();
            SprintTasks item = new SprintTasks();

            if (colName == DatabaseObjects.Columns.SprintOrder)
                newsplstCol = sprintTaskManager.Load().Where(x => x.SprintLookup == selectedSprintId).ToList();
            else
                newsplstCol = sprintTaskManager.Load().Where(x => x.PMMIdLookup == Id).ToList();

            for (int i = 0; i < newsplstCol.Count(); i++)
            {
                item = newsplstCol[i];
                if (colName == DatabaseObjects.Columns.SprintOrder)
                    item.SprintOrder = (i + 1);
                else
                    item.ItemOrder = (i + 1);
            }

            sprintTaskManager.UpdateItems(newsplstCol);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}