using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Web.UI;

namespace uGovernIT.Web
{
    public partial class Accomplishments : UserControl
    {
        public int PMMID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }
        public string TicketId { get; set; }
        public double BaselineId { get; set; }

        int deleted = 0;

 

        private bool isAccomplishmentsDone;

        private ApplicationContext _context = null;
        private PMMCommentManager _pmmCommentManager = null;
        private PMMCommentHistoryManager _pmmCommentHistoryManager = null;

        protected string moduleName = "PMM";
        protected DataRow pmmItem = null;
        protected string currentModulePagePath;

   



        ProjectMonitorStateManager MonitorStateManager = new ProjectMonitorStateManager(HttpContext.Current.GetManagerContext());
        ModuleMonitorOptionManager MonitorOptionManager = new ModuleMonitorOptionManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule module;




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

        protected PMMCommentManager PMMCommentManager
        {
            get
            {
                if (_pmmCommentManager == null)
                {
                    _pmmCommentManager = new PMMCommentManager(ApplicationContext);
                }
                return _pmmCommentManager;
            }
        }

        protected PMMCommentHistoryManager PMMCommentHistoryManager
        {
            get
            {
                if (_pmmCommentHistoryManager == null)
                {
                    _pmmCommentHistoryManager = new PMMCommentHistoryManager(ApplicationContext);
                }
                return _pmmCommentHistoryManager;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            {
                User = HttpContext.Current.CurrentUser();

                module = ModuleManager.GetByName(moduleName);
                
                
                //Bind Accomplishment
                if (!isAccomplishmentsDone)
                {
                    BindAccomplishments();
                    gridAccomplishment.DataBind();
                }


                /// code moved from pre render event
                if (IsReadOnly)
                {
                    gridAccomplishment.Enabled = false;
                    newTask.Visible = false;
                    ImageButton imgaddBtn = gridAccomplishment.FindHeaderTemplateControl(gridAccomplishment.Columns["Title"], "issueAddbtn") as ImageButton;
                    if (imgaddBtn != null && IsReadOnly)
                        imgaddBtn.Visible = false;

                    CheckBox chkArchive = gridAccomplishment.FindHeaderTemplateControl(gridAccomplishment.Columns["ID"], "cbHeaderShowArchivedAccomplishment") as CheckBox;
                    if (chkArchive != null && IsReadOnly)
                        chkArchive.Visible = false;
                }
            }
            base.OnInit(e);

        }

        protected override void OnPreRender(EventArgs e)
        {

        }

        #region Accomplishments
        bool showArchiveAccomplishment = false;

        protected void cbShowArchivedAccomplishment_Load(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (chbox.Checked)
            {
                showArchiveAccomplishment = true;
            }
        }

        private void BindAccomplishments()
        {
            bool includeArchives = showArchiveAccomplishment;

            List<string> queryExp = new List<string>();

            string orderby = string.Empty;

            DataTable accomplishmentsHistory = new DataTable();
            
            if (IsShowBaseline)
            {
                //directly fetch from datatable

                
             
                accomplishmentsHistory = PMMCommentHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Accomplishments' and {DatabaseObjects.Columns.BaselineId}={BaselineId} and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");
                if (includeArchives)
                {
                    deleted = 1;
                    accomplishmentsHistory = PMMCommentHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Accomplishments' and {DatabaseObjects.Columns.BaselineId}={BaselineId}  and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");
                }
                if (accomplishmentsHistory != null)
                {
                    DataView dataView = accomplishmentsHistory.DefaultView;

                    dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.AccomplishmentDate);
                    //save in same datatable

                }

                gridAccomplishment.DataSource = accomplishmentsHistory;

            }
            else
            {
                DataTable accomplishments=null;                // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMComments, accompleshmentQuery);

                

                accomplishments = PMMCommentManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Accomplishments' and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                if (includeArchives)
                {
                    //deleted = 1;
                    accomplishments = PMMCommentManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Accomplishments'  and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                }

                DataTable dtAcomplishment = null;

                if (accomplishments != null)
                {
                    DataView dataView = accomplishments.DefaultView;
                    dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.AccomplishmentDate);
                   dtAcomplishment = dataView.ToTable();
                }
                gridAccomplishment.DataSource = dtAcomplishment;

            }
            isAccomplishmentsDone = true;
        }

        protected void gridAccomplishment_DataBinding(object sender, EventArgs e)
        {
            BindAccomplishments();
        }

        protected void gridAccomplishment_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow currentRow = gridAccomplishment.GetDataRow(e.VisibleIndex);

            if (currentRow != null)
            {
                HtmlAnchor aArchive = (HtmlAnchor)gridAccomplishment.FindRowCellTemplateControl(e.VisibleIndex, gridAccomplishment.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aArchive");
                HtmlAnchor aUnArchive = (HtmlAnchor)gridAccomplishment.FindRowCellTemplateControl(e.VisibleIndex, gridAccomplishment.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aUnArchive");
                HtmlAnchor aDelete = (HtmlAnchor)gridAccomplishment.FindRowCellTemplateControl(e.VisibleIndex, gridAccomplishment.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aDelete");
                HtmlAnchor aEdit = (HtmlAnchor)gridAccomplishment.FindRowCellTemplateControl(e.VisibleIndex, gridAccomplishment.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aEdit");

                HtmlAnchor aTitle = (HtmlAnchor)gridAccomplishment.FindRowCellTemplateControl(e.VisibleIndex, gridAccomplishment.Columns[DatabaseObjects.Columns.Title] as GridViewDataColumn, "aTitle");
                aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                aTitle.Attributes.Add("onclick", string.Format("editAccomplishmentItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                aArchive.Attributes.Add("onclick", string.Format("ArchiveAccomplishment({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveAccomplishment({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aDelete.Attributes.Add("onclick", string.Format("DeleteAccomplishment({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aEdit.Attributes.Add("onclick", string.Format("editAccomplishmentItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));


                bool isDeleted = UGITUtility.StringToBoolean(currentRow[DatabaseObjects.Columns.Deleted]);

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
                    //aUnArchive.Attributes["onclick"] = "#";
                    //aDelete.Attributes["onclick"] = "#";
                    //aArchive.Attributes["onclick"] = "#";
                    //aEdit.Attributes["onclick"] = "#";
                    aDelete.Visible = false;
                    aArchive.Visible = false;
                    aUnArchive.Visible = false;
                    aEdit.Visible = false;
                    
                }
                
            }
        }

        protected void gridAccomplishment_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            PMMCommentManager pMMCommentManager = new PMMCommentManager(HttpContext.Current.GetManagerContext());

            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            {
                string Id = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string action = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];
                int id = 0;
                int.TryParse(Id, out id);
                PMMComments accomplishment = pMMCommentManager.LoadByID(id);
                if (action == "DELETE")
                {
                    try
                    {
                        // SPListHelper.GetSPListItem(DatabaseObjects.Lists.PMMComments, id);
                        if (accomplishment != null)
                        {
                            pMMCommentManager.Delete(accomplishment);  //.Delete();
                        }
                    }
                    catch { }
                }
                else if (action == "ARCHIVE")
                {
                    try
                    {
                        if (accomplishment != null)
                        {
                            accomplishment.Deleted = true;
                            pMMCommentManager.Update(accomplishment); //accomplishment.Update();
                        }
                    }
                    catch { }
                }
                else if (action == "UNARCHIVE")
                {
                    try
                    {
                        if (accomplishment != null)
                        {
                            accomplishment.Deleted = false;
                            pMMCommentManager.Update(accomplishment);
                        }
                    }
                    catch { }
                }
                else if (action == "SHOWARCHIVE")
                {
                    if (Id == "true")
                    {
                        showArchiveAccomplishment = true;
                    }
                }
                BindAccomplishments();
                gridAccomplishment.DataBind();
            }
        }

        #endregion

        protected void cbHeaderShowArchivedAccomplishment_Init(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (showArchiveAccomplishment == true)
            {
                chbox.Checked = true;

            }
        }
    }
}