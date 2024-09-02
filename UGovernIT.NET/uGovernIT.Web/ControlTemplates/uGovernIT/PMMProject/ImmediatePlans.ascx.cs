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
    public partial class ImmediatePlans : UserControl
    {
        public int PMMID { get; set; }
       
        public bool IsReadOnly { get; set; }
        public double BaselineId { get; set; }
        public bool IsShowBaseline { get; set; }

        protected string currentModulePagePath;
        protected DataRow pmmItem = null;

        

        ApplicationContext context = null;

        ProjectMonitorStateManager MonitorStateManager = new ProjectMonitorStateManager(HttpContext.Current.GetManagerContext());
        ModuleMonitorOptionManager MonitorOptionManager = new ModuleMonitorOptionManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule module;
        int deleted = 0;
        private bool isImmediatePlansDone;
        private PMMCommentHistoryManager _pmmCommentHistoryManager = null;



        protected string moduleName = "PMM";

        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (context == null)
                {
                    context =HttpContext.Current.GetManagerContext();
                }
                return context;
            }
        }



        protected PMMCommentHistoryManager PMMCommentHistoryManager
        {
            get
            {
                if (_pmmCommentHistoryManager == null)
                {
                    _pmmCommentHistoryManager = new PMMCommentHistoryManager(HttpContext.Current.GetManagerContext());
                }
                return _pmmCommentHistoryManager;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();

            module = ModuleManager.GetByName(moduleName);

            
            //Bind Immediate Plans
            if (!isImmediatePlansDone)
            {
                BindImmediatePlans();
                gridImmediatePlans.DataBind();
            }



            /// code moved from pre render event
            if (IsReadOnly)
            {
                gridImmediatePlans.Enabled = false;
                LinkButton1.Visible = false;
                ImageButton imgaddBtn = gridImmediatePlans.FindHeaderTemplateControl(gridImmediatePlans.Columns["Title"], "issueAddbtn") as ImageButton;
                if (imgaddBtn != null && IsReadOnly)
                    imgaddBtn.Visible = false;

                CheckBox chkArchive = gridImmediatePlans.FindHeaderTemplateControl(gridImmediatePlans.Columns["ID"], "cbHeaderShowArchivedImmediate") as CheckBox;
                if (chkArchive != null && IsReadOnly)
                    chkArchive.Visible = false;
            }

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            
        }

        #region Planned Items
        bool showArchiveImmediate = false;

        protected void cbShowArchivedImmediate_Load(object sender, EventArgs e)
        {
            
            CheckBox chbox = (CheckBox)sender;
            if (chbox.Checked)
            {
                showArchiveImmediate = true;
            }
        }

        private void BindImmediatePlans()
        {
            bool includeArchives = showArchiveImmediate;
            string orderby = string.Empty;
            List<string> queryExp = new List<string>();
            if (includeArchives)
            {
                orderby = string.Format("<OrderBy><FieldRef Name='{0}' Ascending='FALSE'/></OrderBy>", DatabaseObjects.Columns.Deleted);
                queryExp.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketPMMIdLookup, PMMID));
                queryExp.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Choice'>{1}</Value></Eq>", DatabaseObjects.Columns.ProjectNoteType, "Immediate Plans"));
            }
            else
            {
                orderby = string.Format("<OrderBy><FieldRef Name='{0}' /></OrderBy>", DatabaseObjects.Columns.Created);
                queryExp.Add(string.Format("<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq>", DatabaseObjects.Columns.TicketPMMIdLookup, PMMID));
                queryExp.Add(string.Format("<Eq><FieldRef Name='{0}' /><Value Type='Choice'>{1}</Value></Eq>", DatabaseObjects.Columns.ProjectNoteType, "Immediate Plans"));
                queryExp.Add(string.Format("<Neq><FieldRef Name='{0}' /><Value Type='Boolean'>1</Value></Neq>", DatabaseObjects.Columns.Deleted));
            }

            DataTable dtImmediatePlans = null;

            //SPQuery immediatePlanQuery = new SPQuery();
            //immediatePlanQuery.Query = string.Format("<Where>{0}</Where>{1}", UGITUtility.GenerateWhereQueryWithAndOr(queryExp, queryExp.Count - 1, true), orderby);
            //SPListItemCollection immediatePlans = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMComments, immediatePlanQuery);
            PMMCommentManager pMMCommentManager = new PMMCommentManager(HttpContext.Current.GetManagerContext());
            DataTable immediatePlansHistory = null;

            if (IsShowBaseline)
            {
                //Fetch directly from datatables.
               
              
                 // immediatePlansHistory= PMMCommentHistoryManager.Load(x => x.PMMIdLookup == PMMID && x.ProjectNoteType == "Immediate Plans" && x.BaselineId == BaselineId && x.Deleted == deleted);

              immediatePlansHistory = PMMCommentHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Immediate Plans' and {DatabaseObjects.Columns.BaselineId}={BaselineId} and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                if (includeArchives)
                {
                    deleted = 1;
                    immediatePlansHistory = PMMCommentHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Immediate Plans' and {DatabaseObjects.Columns.BaselineId}={BaselineId}  and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                }
                if (immediatePlansHistory != null)
                {
                    DataView dataView = immediatePlansHistory.DefaultView; 

                    dataView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.UGITEndDate);

                    dtImmediatePlans = dataView.ToTable();
                }

                gridImmediatePlans.DataSource = dtImmediatePlans;

            }
            else
            {
                DataTable immediatePlans; // SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.PMMComments, accompleshmentQuery);                
                immediatePlans = pMMCommentManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Immediate Plans'  and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                if (includeArchives)
                {                    
                    immediatePlans = pMMCommentManager.GetDataTable($"{DatabaseObjects.Columns.TicketPMMIdLookup}={PMMID} and {DatabaseObjects.Columns.ProjectNoteType}='Immediate Plans'   and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                }
                if (immediatePlans != null)
                {
                    DataView dataView = immediatePlans.DefaultView;

                    dataView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.UGITEndDate);

                    dtImmediatePlans = dataView.ToTable();
                }

                gridImmediatePlans.DataSource = dtImmediatePlans;

                isImmediatePlansDone = true;
            }
        }

        protected void gridImmediatePlans_DataBinding(object sender, EventArgs e)
        {
            BindImmediatePlans();
        }

        protected void gridImmediatePlans_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            PMMCommentManager pMMCommentManager = new PMMCommentManager(HttpContext.Current.GetManagerContext());
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            {
                string Id = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string action = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];

                int id = 0;
                int.TryParse(Id, out id);
                PMMComments immediateItem = pMMCommentManager.LoadByID(id);
                if (action == "DELETE")
                {
                    try
                    {
                        if (immediateItem != null)
                            pMMCommentManager.Delete(immediateItem);
                    }
                    catch { }
                }
                else if (action == "ARCHIVE")
                {
                    try
                    {
                        if (immediateItem != null)
                        {
                            immediateItem.Deleted = true;
                            pMMCommentManager.Update(immediateItem);
                        }
                    }
                    catch { }
                }
                else if (action == "UNARCHIVE")
                {
                    try
                    {
                        if (immediateItem != null)
                        {
                            immediateItem.Deleted = false;
                            pMMCommentManager.Update(immediateItem);
                        }
                    }
                    catch { }
                }
                else if (action == "MOVETOACCOMP")
                {
                    if (immediateItem != null)
                    {
                        immediateItem.ProjectNoteType = "Accomplishments";
                        immediateItem.AccomplishmentDate = DateTime.Now;
                        pMMCommentManager.Update(immediateItem);
                    }
                    BindImmediatePlans();
                    //gridAccomplishment.DataBind();
                }
                else if (action == "SHOWARCHIVE")
                {
                    if (Id == "true")
                    {
                        showArchiveImmediate = true;
                    }
                }
                //BindAccomplishments();
                gridImmediatePlans.DataBind();

            }
        }

        protected void gridImmediatePlans_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow currentRow = gridImmediatePlans.GetDataRow(e.VisibleIndex);

            if (currentRow != null)
            {
                HtmlAnchor aUnArchive = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aUnArchive");
                HtmlAnchor aArchive = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aArchive");
                HtmlAnchor aDelete = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aDelete");
                HtmlAnchor aPlannedItemsEdit = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aPlannedItemsEdit");
                HtmlAnchor aMoveToAccomp = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aMoveToAccomp");
                HtmlAnchor aTitle = (HtmlAnchor)gridImmediatePlans.FindRowCellTemplateControl(e.VisibleIndex, gridImmediatePlans.Columns[DatabaseObjects.Columns.Title] as GridViewDataColumn, "aTitle");
                aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                aTitle.Attributes.Add("onclick", string.Format("editImmediatePlansItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                aArchive.Attributes.Add("onclick", string.Format("ArchiveImmediatePlansItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveImmediatePlansItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aDelete.Attributes.Add("onclick", string.Format("DeleteImmediatePlansItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aPlannedItemsEdit.Attributes.Add("onclick", string.Format("editImmediatePlansItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aMoveToAccomp.Attributes.Add("onclick", string.Format("MoveToAccomp({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                bool isDeleted = UGITUtility.StringToBoolean(currentRow[DatabaseObjects.Columns.Deleted]);

                if (isDeleted)
                {
                    //e.Row.Style.Add("background", "#A53421");
                    //e.Row.Style.Add("color", "white");
                    e.Row.CssClass = "archived-dataRow homeGrid_dataRow";
                    aUnArchive.Visible = true;
                    aMoveToAccomp.Visible = false;
                    aDelete.Visible = true;
                    aArchive.Visible = false;
                }
                else
                {
                    aUnArchive.Visible = false;
                    aMoveToAccomp.Visible = true;
                    aDelete.Visible = false;
                    aArchive.Visible = true;
                }
                if (IsShowBaseline || IsReadOnly)
                {
                    aTitle.Attributes["onclick"] = "#";
                    aUnArchive.Attributes["onclick"] = "#";
                    //aDelete.Attributes["onclick"] = "#";
                    aArchive.Attributes["onclick"] = "#";
                    //aPlannedItemsEdit.Attributes["onclick"] = "#";
                    //aMoveToAccomp.Attributes["onclick"] = "#";
                    aDelete.Visible = false;
                    aArchive.Visible = false;
                    aPlannedItemsEdit.Visible = false;
                    aMoveToAccomp.Visible = false;
                    aUnArchive.Visible = false;
                }
            }
        }

        #endregion

        protected void cbHeaderShowArchivedImmediate_Init(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (showArchiveImmediate == true)
            {
                chbox.Checked = true;

            }
        }
    }
}