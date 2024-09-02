using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Text;
using System.Collections;
using DevExpress.Web;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Util.Log;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class HelpCardListPicker : System.Web.UI.UserControl
    {
        public string CallBackWiki { get; set; }
        public string Module { get; set; }
        public DataTable FilteredTable { get; set; }
        public List<string> selectedTicketIds { get; set; }
        public List<string> ExcludedTickets { get; set; }
        public bool EnableModuleDropdown { get; set; }
        public bool IsFilteredTableExist { get; set; }
        public string ParentTicketId { get; set; }
        Ticket moduleRequest;
        protected DataTable dtResult = null;
        public string SelectedModule { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ConfigurationVariableManager _configurationVariableManager = null;

        int ticketAge;
        bool ageColorByTargetCompletion = false;

        protected override void OnInit(EventArgs e)
        {
            grid.SettingsPager.PageSize = 10;
            grid.SettingsPager.AlwaysShowPager = true;
            grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            grid.SettingsPager.ShowDisabledButtons = true;
            grid.SettingsPager.ShowNumericButtons = true;
            grid.SettingsPager.ShowSeparators = true;
            grid.SettingsPager.ShowDefaultImages = true;
            grid.AutoGenerateColumns = false;
            grid.SettingsBehavior.AllowDragDrop = false;
            grid.SettingsText.EmptyHeaders = "  ";
            grid.SettingsPager.PageSizeItemSettings.Visible = true;
            grid.SettingsPager.PageSizeItemSettings.Position = DevExpress.Web.PagerPageSizePosition.Right;
            grid.SettingsBehavior.AllowSort = true;
            grid.SettingsBehavior.AllowSelectByRowClick = true;
            grid.SettingsBehavior.EnableRowHotTrack = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
            grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            grid.DataBind();
            //foreach (string val in selectedTicketIds)
            //{
            //    grid.Selection.SelectRowByKey(val);
            //}

            _configurationVariableManager = new ConfigurationVariableManager(context);

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                return;
            }

            if (ExcludedTickets == null)
            {
                ExcludedTickets = new List<string>();
            }


            if (selectedTicketIds == null)
            {
                selectedTicketIds = new List<string>();
            }

            System.Collections.Generic.List<object> selectedVals = grid.GetSelectedFieldValues(new string[] { "TicketId" });

            if (selectedVals != null && selectedVals.Count > 0)
            {
                selectedTicketIds.Clear();
                foreach (object obj in selectedVals)
                {
                    selectedTicketIds.Add(Convert.ToString(obj));
                }
            }
            if (!Page.IsPostBack)
            {
                dvModule.Visible = true;
                BindCategory();


            }
            else
            {
                dvModule.Visible = true;
                BindCategory();
            }
            GetData();
            ageColorByTargetCompletion = _configurationVariableManager.GetValueAsBool(ConfigConstants.AgeColorByTargetCompletion);
        }

        protected void cbpanel_Callback(object sender, CallbackEventArgsBase e)
        {

            GetData();
        }

        private void GetData()
        {
            if (Module == null)
                return;
            moduleRequest = new Ticket(context, Module);
            string ticketIds = string.Join(",", ExcludedTickets.Select(x => string.Format("'{0}'", x)).ToArray());
            if (!IsFilteredTableExist)
            {
                ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                // mRequest.SPWebObj = SPContext.Current.Web;
                // mRequest.UserID = SPContext.Current.Web.CurrentUser.ID;
                mRequest.CurrentTab = FilterTab.alltickets; //new Tab(FilterTab.alltickets);
                mRequest.ModuleName = Module;
                ModuleStatistics moduleStats = new ModuleStatistics(context);
                ModuleStatisticResponse stat = moduleStats.Load(mRequest);
                dtResult = stat.ResultedData;
            }
            else
            {
                dtResult = FilteredTable;
            }

            if (dtResult != null)
            {

                string category = Convert.ToString(ddlHelpCardCategory.Value);
                List<string> queryCollection = new List<string>();
                if (!string.IsNullOrEmpty(category))
                {
                    if (category != "0")
                    {
                        queryCollection.Add(string.Format("Category = '{0}'", category));
                    }
                }



                if (!string.IsNullOrEmpty(ticketIds))
                {
                    queryCollection.Add(string.Format("TicketId NOT IN ({0})", ticketIds));
                }

                string query = string.Empty;
                if (queryCollection.Count > 0)
                {
                    query = string.Join(" AND ", queryCollection.ToArray());
                }

                var result = dtResult.Select(query);
                if (result.ToList().Count > 0)
                {
                    dtResult = result.CopyToDataTable();
                }
                else
                {
                    dtResult = null;
                }
            }


            if (dtResult != null)
            {
                // Condition for including Age column
                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketAge)
                  && dtResult.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");   //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                    // Check if we have a column by TicketAge in the ModulesColumns table for the current module
                    DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ModuleNameLookup) == Module
                        && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.TicketAge).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();
                    if ((selectedColumns != null && selectedColumns.Count() > 0) || Module == string.Empty)
                    {
                        dtResult.Columns.Add(DatabaseObjects.Columns.TicketAge, typeof(int));
                    }
                }

                // Condition for excluding PMMID
                if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketPMMIdLookup))
                {
                    dtResult.Columns.Remove(DatabaseObjects.Columns.TicketPMMIdLookup);
                }
            }

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (grid.Columns.Count == 0)
                {
                    BindGrid(Module);
                }
                else
                {
                    grid.DataBind();
                }
            }
            else
            {
                grid.DataBind();
            }
        }

        protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "ProgressBar")
            {
                string statusLookup = (string)e.GetListSourceFieldValue(DatabaseObjects.Columns.ModuleStepLookup);
                string status = (string)e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketStatus);
                bool onHold = false;
                bool.TryParse(Convert.ToString(e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketOnHold)), out onHold);
                DataRow currentRow = grid.GetDataRow(e.ListSourceRowIndex);

                if (currentRow != null)
                {
                    Ticket objTicket = new Ticket(context, "WIKI");
                    LifeCycle defaultLifeCycle = objTicket.GetTicketLifeCycle(currentRow);
                    double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                    LifeCycleStage currentStage = objTicket.GetTicketCurrentStage(currentRow);
                    double tillStageWeight = defaultLifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                    int pctComplete = (int)(tillStageWeight / totalWeight * 100);
                    e.Value = UGITUtility.GetProgressBar(defaultLifeCycle, currentStage, currentRow, DatabaseObjects.Columns.TicketStatus, onHold, false);
                }
            }
            else if (e.Column.FieldName == "CustomTicketAge")
            {
                DataRow currentRow = grid.GetDataRow(e.ListSourceRowIndex);

                DateTime completionDate = DateTime.MinValue;
                if (e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketDesiredCompletionDate) != null)
                    DateTime.TryParse(Convert.ToString(e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketDesiredCompletionDate)), out completionDate);

                DateTime createdDate = DateTime.MinValue;
                if (e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketCreationDate) != null)
                    DateTime.TryParse(Convert.ToString(e.GetListSourceFieldValue(DatabaseObjects.Columns.TicketCreationDate)), out createdDate);

                if (currentRow != null)
                {
                    ticketAge = UGITUtility.StringToInt(currentRow[DatabaseObjects.Columns.TicketAge]);

                    if (completionDate != DateTime.MinValue && createdDate != DateTime.MinValue)
                    {
                        e.Value = UGITUtility.GetAgeBar(currentRow, ageColorByTargetCompletion, ticketAge); //UGITUtility.GetAgeBar(createdDate, completionDate);
                    }
                    else if (createdDate != DateTime.MinValue)
                    {
                        e.Value = UGITUtility.GetAgeBar(currentRow, ageColorByTargetCompletion, ticketAge); //UGITUtility.GetAgeBar(createdDate, DateTime.MinValue);
                    }
                }
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //if (dtResult == null)
            //{
            //    GetData();
            //}
            grid.DataSource = dtResult;
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                grid.SettingsPager.Visible = false;
            }
        }
        private void BindGrid(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName) || moduleRequest.Module == null)
                return;
            TicketStatus MTicketStatus = TicketStatus.All;

            List<ModuleColumn> columns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true || string.IsNullOrEmpty(x.SelectedTabs)).OrderBy(x => x.FieldSequence).ToList();

            Dictionary<string, string> customProperties = new Dictionary<string, string>();
            StringBuilder filterDataFields = new StringBuilder();
            GridViewDataTextColumn colId = null;
            GridViewDataDateColumn dateTimeColumn = null;

            grid.Columns.Clear();
            foreach (ModuleColumn moduleColumn in columns)
            {
                customProperties = UGITUtility.GetCustomProperties(Convert.ToString(moduleColumn.CustomProperties), Constants.Separator);
                //1. check for column exist is resultedtable or not
                //2. Check if closed tickets are being shown then only specified column will be shown.
                string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                if (dtResult != null && dtResult.Columns.Contains(fieldColumn) && (MTicketStatus != TicketStatus.Closed || (customProperties.ContainsKey(CustomProperties.DisplayForClosed) && UGITUtility.StringToBoolean(customProperties[CustomProperties.DisplayForClosed]))))
                {
                    Type dataType = dtResult.Columns[fieldColumn].DataType;
                    if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketStatus.ToLower())
                    {
                        if (moduleRequest.Module.List_ModuleColumns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.ModuleStepLookup) != null || moduleRequest.Module.List_ModuleColumns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.StageStep) != null)
                        {
                            colId = new GridViewDataTextColumn();
                            colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = "ProgressBar";
                            colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                            colId.Caption = "Status";
                            colId.Name = "ProgressBar";
                            grid.Columns.Add(colId);
                        }
                    }
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketAge.ToLower())
                    {
                        colId = new GridViewDataTextColumn();
                        colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        colId.FieldName = "CustomTicketAge";

                        colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        colId.Name = "CustomTicketAge";
                        grid.Columns.Add(colId);
                    }
                    else if (dataType == typeof(DateTime))
                    {
                        dateTimeColumn = new GridViewDataDateColumn();
                        dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        dateTimeColumn.FieldName = Convert.ToString(moduleColumn.FieldName);
                        dateTimeColumn.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                        dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        dateTimeColumn.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        grid.Columns.Add(dateTimeColumn);
                    }
                    else
                    {
                        colId = new GridViewDataTextColumn();
                        colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);

                        if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                        {
                            colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        }
                        if (fieldColumn.ToLower() == DatabaseObjects.Columns.Title.ToLower())
                        {
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        }
                        else
                        {
                            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                        }
                        colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        grid.Columns.Add(colId);
                    }
                }
            }
            grid.DataBind();
        }


        private void BindCategory()
        {
            ddlHelpCardCategory.Items.Clear();
            List<string> listCategories = new List<string>();
            try
            {
                HelpCardManager helpCardManager = new HelpCardManager(context);
                listCategories = helpCardManager.Load().Select(x => x.Category).ToList();
                listCategories = listCategories != null && listCategories.Count > 0 ? listCategories.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList() : null;
                if (listCategories != null)
                    foreach (string str in listCategories)
                    {
                        ddlHelpCardCategory.Items.Add(new ListEditItem(str, str));
                    }
                ddlHelpCardCategory.Items.Insert(0, new ListEditItem("All Category", "0"));
                if (ddlHelpCardCategory.SelectedIndex == -1)
                    ddlHelpCardCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            //ddlModuleService.Items.Insert(0, new ListEditItem("All Modules", "0"));
        }

        protected void ddlHelpCardCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }


        protected void ddlHelpCardCategory_Callback(object sender, CallbackEventArgsBase e)
        {
            BindCategory();
            GetData();
        }
    }
}