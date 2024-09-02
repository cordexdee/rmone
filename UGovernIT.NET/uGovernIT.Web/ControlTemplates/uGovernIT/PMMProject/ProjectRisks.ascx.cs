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
    public partial class ProjectRisks : UserControl
    {
        public int PMMID { get; set; }
        public double BaselineId { get; set; }
        public bool IsShowBaseline { get; set; }
        public bool IsReadOnly { get; set; }

        private bool isRisksDone;
        private int deleted = 0;
        protected string currentModulePagePath;
        protected string moduleName = "PMM";
        //DataTable projectMonitorState;
        //DataTable projectMonitorOptions;
        //Ticket ticketRequest = null;
        //LifeCycle projectLifeCycle = null;
        //LifeCycleStage currentLifeCycleStage = null;
        //HtmlEditorControl htmlEditor = null;
        private ApplicationContext _context = null;
        private UGITTaskManager _taskManager = null;
        private ModuleTaskHistoryManager _moduleTaskHistoryManager = null;    
        TicketManager TicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        UserProfileManager UserManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        UGITModule module;

        protected DataRow pmmItem = null;

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

        protected UGITTaskManager UGITTaskManager
        {
            get
            {
                if (_taskManager == null)
                {
                    _taskManager = new UGITTaskManager(ApplicationContext);
                }
                return _taskManager;
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
            
            if (!isRisksDone)
            {
                BindRisks();
                gridRisks.DataBind();
            }

            /// code moved from pre render event
            if (IsReadOnly)
            {
                gridRisks.Enabled = false;
                LinkButton2.Visible = false;
                ImageButton imgaddBtn = gridRisks.FindHeaderTemplateControl(gridRisks.Columns["Title"], "issueAddbtn") as ImageButton;
                if (imgaddBtn != null && IsReadOnly)
                    imgaddBtn.Visible = false;

                CheckBox chkArchive = gridRisks.FindHeaderTemplateControl(gridRisks.Columns["ID"], "cbHeaderShowArchivedRisks") as CheckBox;
                if (chkArchive != null && IsReadOnly)
                    chkArchive.Visible = false;
            }
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            
        }

        #region Risks
        bool showArchvieRisk = false;

        protected void cbShowArchivedRisks_Load(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (chbox.Checked)
            {
                showArchvieRisk = true;
            }
        }

        private void BindRisks()
        {
            bool includeArchives = showArchvieRisk;

            DataRow pMMTicket = TicketManager.GetByID(module, PMMID);

            string pMMTicketId = Convert.ToString(pMMTicket["TicketId"]);

            //List<string> queryExp = new List<string>();

           // string orderby = string.Empty;
            
            //DataTable riskData = null;
            DataTable riskHistory = null;

            if (IsShowBaseline)
            {

                
               
                riskHistory = ModuleTaskHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}' and {DatabaseObjects.Columns.UGITSubTaskType}='Risk' and {DatabaseObjects.Columns.BaselineId}={BaselineId} and {DatabaseObjects.Columns.Deleted}={deleted}  and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");
                if (includeArchives)
                {
                    //deleted = 1; //riskHistory = ModuleTaskHistoryManager.Load(x => x.TicketId == pMMTicketId && x.SubTaskType == "Risk" && x.BaselineId == BaselineId);
                    riskHistory = ModuleTaskHistoryManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}' and {DatabaseObjects.Columns.UGITSubTaskType}='Risk' and {DatabaseObjects.Columns.BaselineId}={BaselineId}  and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");

                }
                if (riskHistory != null)
                {
                    foreach (DataRow row in riskHistory.Rows)
                    {
                        row[DatabaseObjects.Columns.AssignedTo] = Context.GetUserManager().GetUserNamesById(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));
                    }
                }

                gridRisks.DataSource = riskHistory != null ? riskHistory : null;
            }
            else
            {
                DataTable risksData;
                
                risksData = UGITTaskManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}' and {DatabaseObjects.Columns.UGITSubTaskType}='Risk'  and {DatabaseObjects.Columns.Deleted}={deleted} and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");
                if (includeArchives)
                {                    
                    risksData = UGITTaskManager.GetDataTable($"{DatabaseObjects.Columns.TicketId}='{pMMTicketId}' and {DatabaseObjects.Columns.UGITSubTaskType}='Risk'   and {DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}' ");
                }
                if (risksData != null)
                {
                    foreach (DataRow row in risksData.Rows)
                    {
                        row[DatabaseObjects.Columns.AssignedTo] = Context.GetUserManager().GetUserNamesById(Convert.ToString(row[DatabaseObjects.Columns.AssignedTo]));
                    }
                }

                gridRisks.DataSource = risksData != null ? risksData : null;

            }
            isRisksDone = true;
        }

        protected void gridRisks_DataBinding(object sender, EventArgs e)
        {
            BindRisks();
        }

        protected void gridRisks_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.Contains("|"))
            {
                string Id = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string action = e.Parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0];

                int id = 0;
                int.TryParse(Id, out id);
                UGITTask issues = UGITTaskManager.LoadByID(id);
                if (action == "DELETE")
                {
                    try
                    {
                        if (issues != null)
                        {
                            UGITTaskManager.Delete(issues);
                        }
                    }
                    catch { }
                }
                if (action == "ARCHIVE")
                {
                    try
                    {
                        issues.Deleted = true;
                        UGITTaskManager.Update(issues);
                    }
                    catch { }
                }
                else if (action == "UNARCHIVE")
                {
                    try
                    {
                        issues.Deleted = false;
                        UGITTaskManager.Update(issues);
                    }
                    catch { }
                }
                else if (action == "SHOWARCHIVE")
                {
                    //var cbHeaderShowArchivedIssues = (CheckBox)gridIssues.FindHeaderTemplateControl(gridIssues.Columns[8] as GridViewDataColumn, "cbHeaderShowArchivedIssues");
                    if (Id == "true")
                    {
                        showArchvieRisk= true;
                        //cbHeaderShowArchivedIssues.Checked = true;
                    }
                }
                BindRisks();
                gridRisks.DataBind();
            }
        }

        protected void gridRisks_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow currentRow = gridRisks.GetDataRow(e.VisibleIndex);

            if (currentRow != null)
            {
                HtmlAnchor aUnArchive = (HtmlAnchor)gridRisks.FindRowCellTemplateControl(e.VisibleIndex, gridRisks.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aUnArchive");
                HtmlAnchor aDelete = (HtmlAnchor)gridRisks.FindRowCellTemplateControl(e.VisibleIndex, gridRisks.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aDelete");
                HtmlAnchor aArchive = (HtmlAnchor)gridRisks.FindRowCellTemplateControl(e.VisibleIndex, gridRisks.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aArchive");
                HtmlAnchor aEdit = (HtmlAnchor)gridRisks.FindRowCellTemplateControl(e.VisibleIndex, gridRisks.Columns[DatabaseObjects.Columns.Id] as GridViewDataColumn, "aEdit");
                HtmlAnchor aTitle = (HtmlAnchor)gridRisks.FindRowCellTemplateControl(e.VisibleIndex, gridRisks.Columns[DatabaseObjects.Columns.Title] as GridViewDataColumn, "aTitle");
                aTitle.InnerText = Convert.ToString(currentRow[DatabaseObjects.Columns.Title]);
                aTitle.Attributes.Add("onclick", string.Format("editRiskItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                aArchive.Attributes.Add("onclick", string.Format("ArchiveRisk({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aUnArchive.Attributes.Add("onclick", string.Format("UnArchiveRisk({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aDelete.Attributes.Add("onclick", string.Format("DeleteRisk({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));
                aEdit.Attributes.Add("onclick", string.Format("editRiskItem({0})", Convert.ToString(currentRow[DatabaseObjects.Columns.Id])));

                bool isDeleted = UGITUtility.StringToBoolean(currentRow[DatabaseObjects.Columns.Deleted]);

                if (isDeleted)
                {
                    //e.Row.Style.Add("background", "#A53421");
                    //e.Row.Style.Add("color", "white");
                    e.Row.CssClass = "archived-dataRow homeGrid_dataRow";
                    aUnArchive.Visible = true;
                    aDelete.Visible = true;
                    aArchive.Visible = false;
                }
                else
                {
                    aUnArchive.Visible = false;
                    aDelete.Visible = false;
                    aArchive.Visible = true;
                }

                if (IsShowBaseline || IsReadOnly)
                {
                    aTitle.Attributes["onclick"] = "#";
                    aUnArchive.Attributes["onclick"] = "#";
                    aDelete.Attributes["onclick"] = "#";
                   // aArchive.Attributes["onclick"] = "#";
                    aEdit.Attributes["onclick"] = "#";

                    aArchive.Visible = false;
                    aEdit.Visible = false;
                }
            }
        }

        protected void gridRisks_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
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

        protected void gridRisks_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            e.Cell.Enabled = false;
        }

        protected void cbHeaderShowArchivedRisks_Init(object sender, EventArgs e)
        {
            CheckBox chbox = (CheckBox)sender;
            if (showArchvieRisk == true)
            {
                chbox.Checked = true;

            }
        }
    }
}