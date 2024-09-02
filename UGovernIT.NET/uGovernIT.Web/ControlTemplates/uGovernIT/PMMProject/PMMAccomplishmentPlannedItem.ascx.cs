using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.DMS.Amazon;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.Web
{
    public partial class PMMAccomplishmentPlannedItem : UserControl
    {
        public string ProjectID { get; set; }
        public int ItemId { get; set; }
        public int ViewType { get; set; }
        public string Itemtype { get; set; }
        StringBuilder linkFile = new StringBuilder();

        string moduleName = "PMM";
        private ApplicationContext _context = null;
        private DMSManagerService _dmsManagerService = null;

        PMMComments IssueItem;
        PMMCommentManager pMMCommentManager = new PMMCommentManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }
        }

        protected DMSManagerService DMSManagerService
        {
            get
            {
                if (_dmsManagerService == null)
                {
                    _dmsManagerService = new DMSManagerService(ApplicationContext);
                }
                return _dmsManagerService;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            if (ViewType == 0)
            {
                //trviewType.Visible = false;
                dtcAccomplishmentDate.Date = DateTime.Now;
                dtcDueDate.Date = DateTime.Now;
                if (Itemtype.ToLower() == "accomplishment")
                {
                    lblTitle.Text = "Accomplishment";
                    trDueDate.Visible = false;
                    trAccomplishmentDate.Visible = true;
                }
                else
                {
                    lblTitle.Text = "Planned Item";
                    trDueDate.Visible = true;
                    trAccomplishmentDate.Visible = false;
                }
                lblCreatedBy.Visible = false;
                lblModifiedBy.Visible = false;
            }
            else
            {
                //trviewType.Visible = true;
                IssueItem = pMMCommentManager.LoadByID(ItemId);  // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMComments, itemId);

                if (IssueItem != null)
                {
                    bool archiveStatus = UGITUtility.StringToBoolean(IssueItem.Deleted);
                    if (archiveStatus)
                    {
                        btArchiveAccomplishmentplan.Visible = false;
                        btDelete.Visible = true;
                        btMoveFromArchive.Visible = true;
                        btMoveToAccomplishment.Visible = false;
                    }
                    else
                    {
                        btArchiveAccomplishmentplan.Visible = true;
                        btDelete.Visible = false;
                        btMoveFromArchive.Visible = false;
                        btMoveToAccomplishment.Visible = true;
                        if (Itemtype.ToLower() == "accomplishment")
                        {
                            btMoveToAccomplishment.Text = "Move to Planned";
                        }
                        else
                        {
                            btMoveToAccomplishment.Text = "Move to Accomplishment";
                        }
                    }
                    txtTitle.Text = Convert.ToString(IssueItem.Title);

                    txtNote.Text = Server.HtmlDecode(Convert.ToString(IssueItem.ProjectNote));

                    if (Itemtype.ToLower() == "accomplishment")
                    {
                        // chkviewType.Text = "Move to Planned.";
                        lblTitle.Text = "Accomplishment";
                        DateTime accomplishmentDate = DateTime.MinValue;
                        DateTime.TryParse(Convert.ToString(IssueItem.AccomplishmentDate), out accomplishmentDate);
                        if (accomplishmentDate != DateTime.MinValue)
                        {
                            dtcAccomplishmentDate.Date = accomplishmentDate;
                            trAccomplishmentDate.Visible = true;
                            trDueDate.Visible = false;
                        }
                    }
                    else
                    {
                        //chkviewType.Text = "Move to Accomplishments.";
                        lblTitle.Text = "Planned Item";
                        DateTime dueDate = DateTime.MinValue;
                        DateTime.TryParse(Convert.ToString(IssueItem.EndDate), out dueDate);
                        if (dueDate != DateTime.MinValue)
                            dtcDueDate.Date = dueDate;
                        trAccomplishmentDate.Visible = false;
                        trDueDate.Visible = true;
                    }
                    //Bind files
                    if (!string.IsNullOrEmpty(IssueItem.Attachments))
                    {
                        FileUploadControl.SetValue(IssueItem.Attachments);

                    }

                    ////UserProfile lookupAuthorVal = UserManager.GetUserById(IssueItem.);   // new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(IssueItem[DatabaseObjects.Columns.Author]));
                    //if (lookupAuthorVal != null)
                    //    lblCreatedBy.Text = string.Format("Created at {0} by {1}", Convert.ToString(uHelper.GetSPItemValue(IssueItem, DatabaseObjects.Columns.Created)), lookupAuthorVal.LookupValue);
                    //SPFieldUserValue lookupEditorVal = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(IssueItem["Editor"]));
                    //if (lookupEditorVal != null)
                    //    lblModifiedBy.Text = string.Format("Last modified at {0} by {1}", Convert.ToString(uHelper.GetSPItemValue(IssueItem, DatabaseObjects.Columns.Modified)), lookupEditorVal.LookupValue);
                }
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btSaveAccomplishmentplan_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(Page, "AccomplishmentPlan"))
                return;

            Ticket ticket = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");

            if (ViewType == 0)
            {
                List<string> viewFields = new List<string>();

                viewFields.Add(DatabaseObjects.Columns.Id);
                viewFields.Add(DatabaseObjects.Columns.TicketId);

                DataRow projectItem = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), moduleName, ProjectID);

                //SPList issueList = SPListHelper.GetSPList(DatabaseObjects.Lists.PMMComments);
                //IssueItem = issueList.AddItem();
                PMMComments newIssueItem = new PMMComments();

                newIssueItem.Title = txtTitle.Text.Trim();

                newIssueItem.ProjectNote = txtNote.Text.Trim();

                newIssueItem.PMMIdLookup = Convert.ToInt64(projectItem[DatabaseObjects.Columns.ID]);  // new SPFieldLookupValue(projectItem.ID, ProjectID);

                newIssueItem.TicketId = ProjectID;

                if (Itemtype.ToLower() == "accomplishment")
                {
                    newIssueItem.AccomplishmentDate = dtcAccomplishmentDate.Date;
                    newIssueItem.ProjectNoteType = "Accomplishments";
                }
                else
                {
                    if (dtcDueDate.Date != DateTime.MinValue)
                        newIssueItem.EndDate = dtcDueDate.Date;
                    else
                        newIssueItem.EndDate = null;
                    newIssueItem.ProjectNoteType = "Immediate Plans";
                }
                if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                    newIssueItem.Attachments = FileUploadControl.GetValue();
                pMMCommentManager.Insert(newIssueItem);
            }
            else
            {
                if (IssueItem != null)
                {
                    IssueItem.Title = txtTitle.Text.Trim();
                    IssueItem.ProjectNote = txtNote.Text.Trim();
                    if (Itemtype.ToLower() == "accomplishment")
                    {
                        IssueItem.AccomplishmentDate = dtcAccomplishmentDate.Date;
                    }
                    else
                    {
                        if (dtcDueDate.Date != DateTime.MinValue)
                            IssueItem.EndDate = dtcDueDate.Date;
                        else
                            IssueItem.EndDate = null;
                    }

                    //var currentLinkedDocument = Convert.ToString(((HiddenField)UploadAndLinkDocuments.FindControl("fileAttchmentId")).Value);

                    if (!string.IsNullOrEmpty(FileUploadControl.GetValue()))
                        IssueItem.Attachments = FileUploadControl.GetValue();
                }
                pMMCommentManager.Update(IssueItem);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true, "control=projectstatusdetail");
        }

        protected void btArchiveAccomplishmentplan_Click(object sender, EventArgs e)
        {
            if (IssueItem != null)
            {
                IssueItem.Deleted = true;
                pMMCommentManager.Update(IssueItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (IssueItem != null)
            {
                pMMCommentManager.Delete(IssueItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btMoveFromArchive_Click(object sender, EventArgs e)
        {
            if (IssueItem != null)
            {
                IssueItem.Deleted = false;
                pMMCommentManager.Update(IssueItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void btMoveToAccomplishment_Click(object sender, EventArgs e)
        {
            if (IssueItem != null)
            {
                if (Itemtype.ToLower() == "accomplishment")
                {
                    IssueItem.ProjectNoteType = "Immediate Plans";
                    IssueItem.AccomplishmentDate = null;
                }
                else
                {
                    IssueItem.ProjectNoteType = "Accomplishments";
                    IssueItem.AccomplishmentDate = DateTime.Now;
                }

                pMMCommentManager.Update(IssueItem);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }
    }
}
