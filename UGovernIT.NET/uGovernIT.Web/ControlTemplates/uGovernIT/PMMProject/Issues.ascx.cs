using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using uGovernIT.Core;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class Issues : System.Web.UI.UserControl
    {
        public int PMMID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }
        public double BaselineId { get; set; }

        private ApplicationContext _context = null;
        private TicketManager _ticketManager = null;
        private ModuleTaskHistoryManager _moduleTaskHistoryManager = null;

        protected string currentModulePagePath;
        protected DataRow pmmItem = null;

 

        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule module;
        private bool isIssuesDone;

        protected string moduleName = "PMM";

        UGITTaskManager pMMIssueManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
        //  TicketManager TicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());

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

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        protected ModuleTaskHistoryManager ModuleTaskHistoryManager
        {
            get
            {
                if (_moduleTaskHistoryManager == null)
                {
                    _moduleTaskHistoryManager = new ModuleTaskHistoryManager(ApplicationContext);
                }
                return _moduleTaskHistoryManager;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();

            module = ModuleManager.GetByName(moduleName);


            // Bind Issues
            if (!isIssuesDone)
            {
                BindIssues();
                gridIssues.DataBind();
            }

            if (IsReadOnly)
            {
                if (PMMID > 0)
                {
                    gridIssues.Enabled = false;
                    LinkButton3.Visible = false;
                    ImageButton imgaddBtn = gridIssues.FindHeaderTemplateControl(gridIssues.Columns["Title"], "issueAddbtn") as ImageButton;
                    if (imgaddBtn != null && IsReadOnly)
                        imgaddBtn.Visible = false;

                    CheckBox chkArchive = gridIssues.FindHeaderTemplateControl(gridIssues.Columns["ID"], "cbHeaderShowArchivedIssues") as CheckBox;
                    if (chkArchive != null && IsReadOnly)
                        chkArchive.Visible = false;
                }
            }

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {


        }
        #region Issues
        bool showArchiveIssues = false;
        protected void cbShowArchivedIssues_Load(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (chbox.Checked)
            {
                showArchiveIssues = true;
            }
        }

        private void BindIssues()
        {

            string orderby = string.Empty;
            bool includeArchives = showArchiveIssues;
            List<string> queryExp = new List<string>();
            //int deleted = 0;
            List<UGITTask> issues;

            DataRow pMMTicket = TicketManager.GetByID(module, PMMID);

            string pMMTicketId = Convert.ToString(pMMTicket["TicketId"]);
            //DataTable issueData = null;
            List<ModuleTasksHistory> issueHistory = null;
            //DataTable issueHistory = new DataTable();
            //Show baseline 
            if (IsShowBaseline)
            {
                //if (includeArchives)
                //{
                //    deleted = 1;
                //}

                issueHistory = ModuleTaskHistoryManager.Load(x => x.TicketId == pMMTicketId && x.SubTaskType == "Issue" && x.BaselineId == BaselineId && x.Deleted == false);
                //issueHistory = ModuleTaskHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}'  and {DatabaseObjects.Columns.UGITSubTaskType}='Issue' and {DatabaseObjects.Columns.IsDeletedColumn}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");

                if (issueHistory != null)
                {
                    foreach (ModuleTasksHistory row in issueHistory)
                    {
                        row.AssignedTo = Context.GetUserManager().GetUserNamesById(row.AssignedTo); // UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));
                    }
                }
                // issueData = UGITUtility.ToDataTable<ModuleTasksHistory>(issueHistory);

                gridIssues.DataSource = issueHistory;
            }

            else
            {
                if (includeArchives)
                {
                    //deleted = 1;
                    issues = pMMIssueManager.Load(x => x.TicketId == pMMTicketId && x.SubTaskType == "Issue");
                }
                else
                {
                    issues = pMMIssueManager.Load(x => x.TicketId == pMMTicketId && x.SubTaskType == "Issue" && x.Deleted == false);
                }
                //issues = pMMIssueManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}' and {DatabaseObjects.Columns.UGITSubTaskType}='Issue' and {DatabaseObjects.Columns.IsDeletedColumn}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");

                //issueData = UGITUtility.ToDataTable<UGITTask>(issues);
                if (issues != null)
                {
                    foreach (UGITTask row in issues)
                    {
                        row.AssignedTo = Context.GetUserManager().GetUserNamesById(row.AssignedTo); // UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row.AssignedTo));
                    }
                }

                gridIssues.DataSource = issues;
            }
            isIssuesDone = true;
        }

        protected void gridIssues_DataBinding(object sender, EventArgs e)
        {
            BindIssues();
        }

        protected void gridIssues_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            {
                string Id = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string action = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];

                int id = 0;
                int.TryParse(Id, out id);
                UGITTask issues = TaskManager.LoadByID(id);
                if (action == "DELETE")
                {
                    try
                    {
                        if (issues != null)
                        {
                            TaskManager.Delete(issues);
                        }
                    }
                    catch { }
                }
                else if (action == "ARCHIVE")
                {
                    try
                    {
                        issues.Deleted = true;
                        TaskManager.Update(issues);
                    }
                    catch { }
                }
                else if (action == "UNARCHIVE")
                {
                    try
                    {
                        issues.Deleted = false;
                        TaskManager.Update(issues);
                    }
                    catch { }
                }
                else if (action == "SHOWARCHIVE")
                {
                    if (Id == "true")
                    {
                        showArchiveIssues = true;
                    }
                }
                BindIssues();
                gridIssues.DataBind();
            }
        }

        protected void gridIssues_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            UGITTask currentRow = (UGITTask)gridIssues.GetRow(e.VisibleIndex);
            if (currentRow != null)
            {
                HtmlAnchor aUnArchive = (HtmlAnchor)gridIssues.FindRowCellTemplateControl(e.VisibleIndex, gridIssues.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aUnArchive");
                HtmlAnchor aArchive = (HtmlAnchor)gridIssues.FindRowCellTemplateControl(e.VisibleIndex, gridIssues.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aArchive");
                HtmlAnchor aDelete = (HtmlAnchor)gridIssues.FindRowCellTemplateControl(e.VisibleIndex, gridIssues.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aDelete");
                HtmlAnchor aEdit = (HtmlAnchor)gridIssues.FindRowCellTemplateControl(e.VisibleIndex, gridIssues.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aEdit");
                HtmlAnchor aTitle = (HtmlAnchor)gridIssues.FindRowCellTemplateControl(e.VisibleIndex, gridIssues.Columns[DatabaseObjects.Columns.Title] as GridViewDataColumn, "aTitle");
                //aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                //aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                //aTitle.Attributes.Add("onclick", string.Format("editIssue({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                //aArchive.Attributes.Add("onclick", string.Format("ArchiveIssue({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                //aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveIssue({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                //aDelete.Attributes.Add("onclick", string.Format("DeleteIssue({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                //aEdit.Attributes.Add("onclick", string.Format("editIssue({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                //bool isDeleted = UGITUtility.StringToBoolean(currentRow[DatabaseObjects.Columns.Deleted]);


                aTitle.InnerText = Convert.ToString(currentRow.Title);

                aTitle.Attributes.Add("onclick", string.Format("editIssue({0})", Convert.ToString(currentRow.ID)));

                aArchive.Attributes.Add("onclick", string.Format("ArchiveIssue({0})", Convert.ToString(currentRow.ID)));
                aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveIssue({0})", Convert.ToString(currentRow.ID)));
                aDelete.Attributes.Add("onclick", string.Format("DeleteIssue({0})", Convert.ToString(currentRow.ID)));
                aEdit.Attributes.Add("onclick", string.Format("editIssue({0})", Convert.ToString(currentRow.ID)));

                bool isDeleted = UGITUtility.StringToBoolean(currentRow.Deleted);
                if (isDeleted)
                {
                    //e.Row.Style.Add("background", "#A53421");
                    //e.Row.Style.Add("color", "white");
                    e.Row.CssClass = "archived-dataRow homeGrid_dataRow";
                    aUnArchive.Visible = true;
                    aArchive.Visible = false;
                    aDelete.Visible = true;
                }
                else
                {
                    aUnArchive.Visible = false;
                    aArchive.Visible = true;
                    aDelete.Visible = false;

                }
                if (IsShowBaseline || IsReadOnly)
                {
                    aTitle.Attributes["onclick"] = "#";
                    aUnArchive.Attributes["onclick"] = "#";
                    aDelete.Attributes["onclick"] = "#";
                    aArchive.Attributes["onclick"] = "#";
                    aEdit.Attributes["onclick"] = "#";

                    aEdit.Visible = false;
                    aDelete.Visible = false;
                    aArchive.Visible = false;
                }
            }
        }

        protected void gridIssues_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            headerFilterFillItems(sender, e);
        }

        protected void headerFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {

            if (e.Column.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
                List<string> values = new List<string>(); // List in string format used to keep out duplicates
                foreach (FilterValue fValue in e.Values)
                {
                    if (fValue.Value.Contains(";"))
                    {
                        // Found multiple semi-colon separated values
                        string[] vals = fValue.Value.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string val in vals)
                        {
                            // Add to filter list only if not already in it
                            string trimmedVal = val.Trim();
                            if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                            {
                                temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                                values.Add(trimmedVal);
                            }
                        }
                    }
                    else
                    {
                        // Single value, add to list if not already in it
                        string trimmedVal = fValue.Value.Trim();
                        if (!string.IsNullOrEmpty(trimmedVal) && !values.Contains(trimmedVal))
                        {
                            temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                            values.Add(trimmedVal);
                        }
                    }
                }

                // Add to filter list in order
                temp = temp.OrderBy(o => o.Value).ToList();
                e.Values.Clear();
                foreach (FilterValue fVal in temp)
                {
                    e.Values.Add(fVal);
                }
            }
        }

        #endregion

        protected void cbHeaderShowArchivedIssues_Init(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (showArchiveIssues == true)
            {
                chbox.Checked = true;

            }
        }

        protected void gridIssues_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.DueDate && Convert.ToDateTime(e.Value) == DateTime.MinValue)
                e.DisplayText = "";
        }
    }
}