using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class DecisionLogList : UserControl
    {
        public int PMMID { get; set; }      
        public bool IsReadOnly { get; set; }
        public bool IsShowBaseline { get; set; }
        public double BaselineId { get; set; }    


        Ticket ticketRequest = null;
        //LifeCycle projectLifeCycle = null;
        //LifeCycleStage currentLifeCycleStage = null;
        //HtmlEditorControl htmlEditor = null;
        //private bool isIssuesDone;
        private int deleted = 0;
        private DecisionLogManager _decisionLogManager = null;
        private ApplicationContext _context = null;
        private DecisionLogHistoryManager _decisionLogHistoryManager = null;

        protected string ModuleName = "PMM";
        protected DataRow pmmItem = null;
        protected string currentModulePagePath;
        protected string pmmPublicId;
        protected bool showArchiveDecisionLogList = false;

        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule Module;

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

        protected DecisionLogManager DecisionLogManager
        {
            get
            {
                if (_decisionLogManager == null)
                {
                    _decisionLogManager = new DecisionLogManager(ApplicationContext);
                }
                return _decisionLogManager;
            }
        }

        protected DecisionLogHistoryManager DecisionLogHistoryManager
        {
            get
            {
                if (_decisionLogHistoryManager == null)
                {
                    _decisionLogHistoryManager = new DecisionLogHistoryManager(ApplicationContext);
                }
                return _decisionLogHistoryManager;
            }
        }

        // DecisionLogManager DecisionLogMGR = new DecisionLogManager(HttpContext.Current.GetManagerContext());

        protected void Page_Load(object sender, EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), "PMM");
            Module = ModuleManager.LoadByName(ModuleName);
            pmmItem = ticketManager.GetByID(Module, PMMID);
            pmmPublicId = UGITUtility.ObjectToString(pmmItem[DatabaseObjects.Columns.TicketId]);

            
             DecisionLogGrid();
            if (IsReadOnly)
            {
                gridDecisionLog.Enabled = false;
                btDecisionLog.Visible = false;
                ImageButton imgaddBtn = gridDecisionLog.FindHeaderTemplateControl(gridDecisionLog.Columns[1], "issueAddbtn") as ImageButton;
                if (imgaddBtn != null && IsReadOnly)
                    imgaddBtn.Visible = false;

                CheckBox chkArchive = gridDecisionLog.FindHeaderTemplateControl(gridDecisionLog.Columns[6], "chHeaderShowDecisonLogArchive") as CheckBox;
                if (chkArchive != null && IsReadOnly)
                    chkArchive.Visible = false;
            }


            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            
        }

        protected void gridDecisionLog_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow currentRow = gridDecisionLog.GetDataRow(e.VisibleIndex);
           
            if (currentRow != null)
            {
                HtmlAnchor aUnArchive = (HtmlAnchor)gridDecisionLog.FindRowCellTemplateControl(e.VisibleIndex, gridDecisionLog.Columns[DatabaseObjects.Columns.ID] as GridViewDataColumn, "aUnArchive");
                HtmlAnchor aArchive = (HtmlAnchor)gridDecisionLog.FindRowCellTemplateControl(e.VisibleIndex, gridDecisionLog.Columns[DatabaseObjects.Columns.ID] as GridViewDataColumn, "aArchive");
                HtmlAnchor aDelete = (HtmlAnchor)gridDecisionLog.FindRowCellTemplateControl(e.VisibleIndex, gridDecisionLog.Columns[DatabaseObjects.Columns.ID] as GridViewDataColumn, "aDelete");
                HtmlAnchor aEdit = (HtmlAnchor)gridDecisionLog.FindRowCellTemplateControl(e.VisibleIndex, gridDecisionLog.Columns[DatabaseObjects.Columns.ID] as GridViewDataColumn, "aEdit");
                HtmlAnchor aTitle = (HtmlAnchor)gridDecisionLog.FindRowCellTemplateControl(e.VisibleIndex, gridDecisionLog.Columns[DatabaseObjects.Columns.Title] as GridViewDataColumn, "aTitle");
                aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                aTitle.Attributes.Add("onclick", string.Format("editDecisionLog({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));

                aArchive.Attributes.Add("onclick", string.Format("ArchiveDecisionLog({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));
                aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveDecisionLog({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));
                aDelete.Attributes.Add("onclick", string.Format("DeleteDecisionLog({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));
                aEdit.Attributes.Add("onclick", string.Format("editDecisionLog({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.ID])));

                bool isDeleted = UGITUtility.StringToBoolean(currentRow["Deleted"]);
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
                    //aDelete.Attributes["onclick"] = "#";
                    aArchive.Attributes["onclick"] = "#";
                    //aEdit.Attributes["onclick"] = "#";
                    aDelete.Visible = false;
                    aEdit.Visible = false;
                    aArchive.Visible = false;
                }

                
            }
            
        }

        protected void gridDecisionLog_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            {
                string Id = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string action = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];
                int id = 0;
                int.TryParse(Id, out id);
                DecisionLog decisionLogItem = DecisionLogManager.LoadByID(id);  // SPListHelper.GetSPListItem(DatabaseObjects.Lists.DecisionLog, id);

                if (decisionLogItem != null)
                {
                    if (action == "DELETE")
                    {
                        try
                        {
                            DecisionLogManager.Delete(decisionLogItem); // decisionLogItem.Delete();                
                        }
                        catch { }
                    }
                    else if (action == "ARCHIVE")
                    {
                        try
                        {
                            decisionLogItem.Deleted = true;
                            DecisionLogManager.Update(decisionLogItem);
                        }
                        catch { }
                    }
                    else if (action == "UNARCHIVE")
                    {
                        try
                        {
                            decisionLogItem.Deleted = false;
                            DecisionLogManager.Update(decisionLogItem);
                            
                        }
                        catch { }
                    }                    
                }
                else if (action == "SHOWARCHIVE")
                {
                    if (Id == "true")
                    {
                        showArchiveDecisionLogList = true;
                    }
                }
                DecisionLogGrid();
            }
        }

        void DecisionLogGrid()
        {
            List<DecisionLog> decisionLogList = DecisionLogManager.Load();  

            if (decisionLogList == null)
                return;

            DataTable decisionLogTable = null;
           DataTable decisionLogHistory = null;

            if (IsShowBaseline)
            {

                if (chShowDecisonLogArchive.Checked)
                {
                    deleted = 1;
                     //decisionLogHistory = DecisionLogHistoryManager.Load(x => x.TicketId == pmmPublicId && x.BaselineId == BaselineId);
                }
                if (showArchiveDecisionLogList)
                {
                    deleted = 1;
                     //decisionLogHistory = DecisionLogHistoryManager.Load(x => x.TicketId == pmmPublicId && x.BaselineId == BaselineId);
                }

                 //decisionLogHistory = DecisionLogHistoryManager.Load(x => x.TicketId == pmmPublicId && x.BaselineId == BaselineId && x.Deleted == deleted);

                 decisionLogHistory = DecisionLogHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pmmPublicId}' and {DatabaseObjects.Columns.BaselineId}={BaselineId} and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");

                if (decisionLogHistory != null )
                {
                    DataView dataView = decisionLogHistory.DefaultView;

                    dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.ReleaseDate);

                    decisionLogTable = dataView.ToTable();
                }
                if (decisionLogTable != null)
                {
                    foreach (DataRow currentRow in decisionLogTable.Rows)
                    {
                        currentRow[DatabaseObjects.Columns.AssignedTo] = UserManager.GetUserNamesById(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.AssignedTo]), Constants.Separator6);
                        currentRow[DatabaseObjects.Columns.DecisionMaker] = UserManager.GetUserNamesById(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.DecisionMaker]), Constants.Separator6);
                    }
                }
                gridDecisionLog.DataSource = decisionLogTable;
            }
            else
            {

                List<DecisionLog> itemColl;

                if (chShowDecisonLogArchive.Checked ||  showArchiveDecisionLogList)
                {
                    itemColl = decisionLogList.Where(x => x.ModuleName == ModuleName && x.TicketId == pmmPublicId).ToList();
                }
                else
                {
                    itemColl = decisionLogList.Where(x => x.ModuleName == ModuleName && x.TicketId == pmmPublicId && x.Deleted == false).ToList();
                }

                if (itemColl != null && itemColl.Count > 0)
                {
                    DataView dataView = UGITUtility.ToDataTable<DecisionLog>(itemColl).DefaultView;
                    dataView.Sort = string.Format("{0} DESC", DatabaseObjects.Columns.ReleaseDate);
                    decisionLogTable = dataView.ToTable();
                }
                if (decisionLogTable != null)
                {
                    foreach (DataRow currentRow in decisionLogTable.Rows)
                    {
                        currentRow[DatabaseObjects.Columns.AssignedTo] = UserManager.GetUserNamesById(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.UGITAssignedTo]), Constants.Separator6);
                        currentRow[DatabaseObjects.Columns.DecisionMaker] = UserManager.GetUserNamesById(UGITUtility.ObjectToString(currentRow[DatabaseObjects.Columns.DecisionMaker]), Constants.Separator6);
                    }
                }

                gridDecisionLog.DataSource = decisionLogTable;

            
            }
            gridDecisionLog.DataBind();
        }

        protected void chHeaderShowDecisonLogArchive_Init(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (showArchiveDecisionLogList == true)
            {
                chbox.Checked = true;

            }
        }


    }
}