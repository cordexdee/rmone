
using DevExpress.Web;
using DevExpress.Web.Rendering;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ListPicker : UserControl
    {
        public string Module { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string RequestType { get; set; }

        public DataTable FilteredTable { get; set; }

        public List<string> selectedTicketIds { get; set; }
        public List<string> ExcludedTickets { get; set; }
        public bool EnableModuleDropdown { get; set; }

        public bool IsFilteredTableExist { get; set; }
        public bool IsMultiSelect { get; set; }
        public bool IsDeleteTicket { get; set; }
        public bool LookAhead { get; set; }
      
        Ticket moduleRequest;
        public UserProfile User;
        private ApplicationContext _applicationContext = null;
        private ConfigurationVariableManager _configurationVariableManager;
        FieldConfigurationManager fmanger = null;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = HttpContext.Current.GetManagerContext();
                }
                return _applicationContext;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            User = HttpContext.Current.CurrentUser();
            _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
            fmanger = new FieldConfigurationManager(ApplicationContext);

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            grid.SettingsPager.PageSize = 15;
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
            //grid.SettingsBehavior.EnableRowHotTrack = true;
            grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
           // grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
           // grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
            //grid.Theme = "CustomMaterial";
           // grid.Styles.Header.Font.Size = FontUnit.Point(11);
            // grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
            BindGrid(GetData(), Module);
            if (selectedTicketIds != null)
            {
                foreach (string val in selectedTicketIds)
                {
                    grid.Selection.SelectRowByKey(val);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsDeleteTicket && !string.IsNullOrEmpty(Module))
            {
                //ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                //moduleRequest=moduleViewManager.LoadByName(Module);
                moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), Module);
                //Ticket moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), "CMDB");
            }
            if (Module != "NPR" && !this.Visible)
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
            List<object> selectedVals = null;
            if (grid != null)
                selectedVals = grid.GetSelectedFieldValues(new string[] { "TicketId" });
            if (selectedVals != null && Type == "AssetRelatedWithAssets")
                selectedVals = grid.GetSelectedFieldValues(new string[] { "TicketId" });//grid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.Id });
            if (selectedVals != null && selectedVals.Count > 0)
            {
                selectedTicketIds.Clear();
                foreach (object obj in selectedVals)
                {
                    selectedTicketIds.Add(Convert.ToString(obj));
                }
            }
            if (!IsPostBack)
            {
                chkIncClosedTkts.Checked = (Request["IncludeCloseTkts"] != null);
            }
            if (!string.IsNullOrEmpty(Module) && moduleRequest == null)
            {
                moduleRequest = new Ticket(ApplicationContext, Module);
            }
            //if (!Page.IsPostBack) //showing module dropdown, when displaying from PMm import wizard
            {
                if (EnableModuleDropdown)
                {
                    BindModule();
                    if (!Page.IsPostBack  && !string.IsNullOrEmpty(Module))
                    {
                       ddlModuleService.SelectedValue = Module;
                    }
                    ddlModuleService.Visible = true;
                    chkIncClosedTkts.Visible = true;
                    lblModule.Visible= true;
                }
                else
                {
                    ddlModuleService.Visible = false;
                    chkIncClosedTkts.Visible = false;
                    lblModule.Visible = false;
                }

                if (IsMultiSelect)
                {
                    grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                }
                else
                {
                    grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                }
            }
            if (ddlModuleService.SelectedIndex == 0)
            {
                lblNotificationText.Text = "Select a module from the dropdown";
                grid.SettingsPager.Visible = false;
            }
            else
            {
                if (IsMultiSelect)
                    lblNotificationText.Text = "Select item(s) using the checkboxes";
                else
                    lblNotificationText.Text = "Click on a row to select it";

                grid.SettingsPager.Visible = true;
            }

            if (LookAhead)
            {
                //If close ticket need  to show then  remove code.
                chkIncClosedTkts.Visible = false;
            }

        }

        #region Data & Binding
        private DataTable GetData()
        {
            if (string.IsNullOrEmpty(Module))
            {
                return null;
            }
            DataTable dtResult = null;
            string ticketIds = ExcludedTickets == null ? string.Empty : string.Join(",", ExcludedTickets.Select(x => string.Format("'{0}'", x)).ToArray());
            //SPList ticketRList = SPListHelper.GetSPList(DatabaseObjects.Lists.TicketRelationship);
            if (!IsFilteredTableExist && !string.IsNullOrEmpty(Module))
            {
                ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                //mRequest.SPWebObj = SPContext.Current.Web;
                mRequest.UserID = User.Id;
                mRequest.CurrentTab = CustomFilterTab.OpenTickets.ToString(); //new Tab(CustomFilterTab.OpenTickets.ToString(),CustomFilterTab.OpenTickets.ToString());
                //if (chkIncClosedTkts.Checked) {
                //    mRequest.CurrentTab = CustomFilterTab.AllTickets.ToString(); new Tab(CustomFilterTab.AllTickets.ToString(), CustomFilterTab.AllTickets.ToString());
                //}
                mRequest.ModuleName = Module;
                mRequest.Title = Title;
                mRequest.Lookahead = LookAhead;
                mRequest.RequestType = RequestType;

                ModuleStatistics moduleStats = new ModuleStatistics(HttpContext.Current.GetManagerContext());
                ModuleStatisticResponse stat = moduleStats.Load(mRequest);
                dtResult = stat.ResultedData;

                if (chkIncClosedTkts.Checked)
                {
                    mRequest = new ModuleStatisticRequest();
                    mRequest.ModuleName = Module;
                    mRequest.CurrentTab = CustomFilterTab.CloseTickets.ToString();
                    
                    stat = moduleStats.Load(mRequest);
                    dtResult.Merge(stat.ResultedData);
                }
            }
            else
            {
                dtResult = FilteredTable;
                chkIncClosedTkts.Visible = false;
            }
            if (dtResult != null)
            {
                if (!string.IsNullOrEmpty(ticketIds))
                {
                    var result = dtResult.Select(string.Format("TicketId NOT IN ({0})", ticketIds));
                    if (result.ToList().Count > 0)
                    {
                        dtResult = result.CopyToDataTable();
                    }
                    else
                    {
                        dtResult = null;
                    }
                }
            }
            if (dtResult != null)
            {

                List<ModuleColumn> mColumns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();
                if (mColumns.Count > 0)
                {
                    List<ModuleColumn> multiuserColumns = mColumns.Where(x => x.ColumnType == "MultiUser").ToList();//.CopyToDataTable().Select(string.Format("{0} = 'MultiUser' or {0} = 'MultiLookup'", DatabaseObjects.Columns.ColumnType));

                    foreach (DataRow row in dtResult.Rows)
                    {
                        if (mColumns.Count > 0)
                        {
                            foreach (ModuleColumn moduleColumn in multiuserColumns)
                            {
                                if (dtResult.Columns.Contains(Convert.ToString(moduleColumn.FieldName)))
                                {
                                    row[Convert.ToString(moduleColumn.FieldName)] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[Convert.ToString(moduleColumn.FieldName)]));
                                }
                            }
                        }
                    }
                }

                // Condition for including Age column
                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketAge)
                  && dtResult.Columns.Contains(DatabaseObjects.Columns.TicketCreationDate))
                {
                    DataTable columns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");
                    // Check if we have a column by TicketAge in the ModulesColumns table for the current module
                    DataRow[] selectedColumns = columns.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.CategoryName) == Module
                        && x.Field<string>(DatabaseObjects.Columns.FieldName) == DatabaseObjects.Columns.TicketAge).OrderBy(x => x.Field<Int32>(DatabaseObjects.Columns.FieldSequence)).ToArray();
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
                #region Sorting using module columns
                ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                //tsr value
                DataTable dtModuleCol = moduleViewManager.LoadModuleListByName(Module, DatabaseObjects.Tables.ModuleColumns);   //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns, DatabaseObjects.Columns.ModuleNameLookup, Module).CopyToDataTable();
                string sortString = string.Empty;

                if (dtModuleCol.Columns.Contains(DatabaseObjects.Columns.SortOrder))
                {
                    string moduleColExpression = string.Format("{0} IS NOT NULL OR  {0} <> '{1}'", DatabaseObjects.Columns.SortOrder, "0");
                    string moduleColSortExpression = string.Format("{0} ASC", DatabaseObjects.Columns.SortOrder);
                    DataRow[] dtModuleColRow = dtModuleCol.Select(moduleColExpression, moduleColSortExpression);
                    if (dtModuleColRow != null && dtModuleColRow.Length > 0)
                    {
                        List<string> ModuleColNameList = new List<string>();
                        foreach (DataRow rowitem in dtModuleColRow)
                        {
                            // if (resultedTable.Columns.Contains(Convert.ToString(rowitem[DatabaseObjects.Columns.FieldName])))
                            {
                                string strexp = string.Empty;
                                if (Convert.ToString(rowitem[DatabaseObjects.Columns.IsAscending]) == string.Empty || Convert.ToString(rowitem[DatabaseObjects.Columns.IsAscending]) == "1")
                                {
                                    strexp = "ASC";
                                }
                                else
                                {
                                    strexp = "DESC";
                                }
                                ModuleColNameList.Add(Convert.ToString(rowitem[DatabaseObjects.Columns.FieldName]) + " " + strexp);
                                // ModuleColNameList.Add(Convert.ToString(rowitem[DatabaseObjects.Columns.FieldName]));
                            }
                        }
                        sortString = String.Join(",", ModuleColNameList.ToArray());
                    }
                }

                #endregion
                DataView dvResult = dtResult.AsDataView();
                //dvResult.Sort = sortString;
                dtResult = dvResult.ToTable();
            }
            // Hide pager for no data.
            if (dtResult == null || dtResult.Rows.Count == 0)
            {
                grid.SettingsPager.Visible = false;
                lblNotificationText.Visible = false;
            }
            return dtResult;
        }

        private void BindGrid(DataTable dtResult, string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return;
            TicketStatus MTicketStatus = TicketStatus.All;

            // Get updated columns property.
            List<ModuleColumn> columns = CacheHelper<object>.Get($"ModuleColumnslistview{_applicationContext.TenantID}") as List<ModuleColumn>;
            if (columns == null)
            {
                ModuleColumnManager moduleColumnManager = new ModuleColumnManager(_applicationContext);
                columns = moduleColumnManager.Load(x => x.CategoryName == moduleName);
                CacheHelper<object>.AddOrUpdate($"ModuleColumnslistview{_applicationContext.TenantID}", moduleColumnManager.Load());
            }
            else
            {
                columns = columns.Where(x => x.CategoryName == moduleName).ToList();
            }

           columns = columns.Where(x => x.IsDisplay == true || (!string.IsNullOrEmpty(x.SelectedTabs)
           && (UGITUtility.SplitString(x.SelectedTabs, Constants.Separator).Any(y => y.Equals("open", StringComparison.InvariantCultureIgnoreCase)) || UGITUtility.SplitString(x.SelectedTabs, Constants.Separator).Any(y => y.Equals("all", StringComparison.InvariantCultureIgnoreCase))))).OrderBy(x => x.FieldSequence).ToList();

            Dictionary<string, string> customProperties = new Dictionary<string, string>();
            StringBuilder filterDataFields = new StringBuilder();
            GridViewDataTextColumn colId = null;
            GridViewDataDateColumn dateTimeColumn = null;

            grid.Columns.Clear();

            if (IsMultiSelect)
            {
                grid.SettingsBehavior.AllowSelectByRowClick = false;
                // added new column checkbox.
                GridViewCommandColumn dataTextColumn = new GridViewCommandColumn();
                dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                //dataTextColumn.FieldName = "";
                dataTextColumn.Caption = " ";
                dataTextColumn.ShowSelectCheckbox = true;
                dataTextColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
                // dataTextColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
                dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                dataTextColumn.HeaderStyle.Font.Bold = true;
                // dataTextColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                // dataTextColumn.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                dataTextColumn.Width = GetColumnWidth("CheckBox");
                dataTextColumn.VisibleIndex = 0;
                grid.Columns.Add(dataTextColumn);
            }
            colId = new GridViewDataTextColumn();
            colId.FieldName = DatabaseObjects.Columns.Id;
            colId.Caption = DatabaseObjects.Columns.Id;
            colId.Name = DatabaseObjects.Columns.Id;
            colId.Visible = false;
            grid.Columns.Add(colId);
            foreach (ModuleColumn moduleColumn in columns)
            {
                customProperties = UGITUtility.GetCustomProperties(Convert.ToString(moduleColumn.CustomProperties), Constants.Separator);
                HorizontalAlign alignment = HorizontalAlign.Center;
                if (!string.IsNullOrWhiteSpace(moduleColumn.TextAlignment))
                    alignment = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), moduleColumn.TextAlignment);

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
                            colId.CellStyle.HorizontalAlign = alignment;
                            colId.PropertiesTextEdit.EncodeHtml = false;
                            colId.FieldName = DatabaseObjects.Columns.TicketStatus;
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
                        colId.CellStyle.HorizontalAlign = alignment;
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        colId.FieldName = DatabaseObjects.Columns.TicketAge;

                        colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        colId.Name = DatabaseObjects.Columns.TicketAge;
                        grid.Columns.Add(colId);
                    }
                    else if (dataType == typeof(DateTime))
                    {
                        dateTimeColumn = new GridViewDataDateColumn();
                        dateTimeColumn.CellStyle.HorizontalAlign = alignment;
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
                        colId.FieldName = moduleColumn.FieldName;
                        string dollorCol = (fieldColumn.ToLower().EndsWith("lookup") || fieldColumn.ToLower().EndsWith("user")) ? string.Format("{0}$", fieldColumn) : fieldColumn;
                        if (dollorCol.EndsWith("$") && dtResult.Rows.Count > 0 && UGITUtility.IfColumnExists(dtResult.Rows[0], dollorCol))
                            colId.FieldName = dollorCol;

                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);

                        if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                        {
                            colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        }

                        colId.CellStyle.HorizontalAlign =alignment;
                        colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        grid.Columns.Add(colId);
                    }
                }
            }

            grid.DataBind();
        }

        private Unit GetColumnWidth(string columnName)
        {
            Unit width = new Unit("30px");

            if (columnName == DatabaseObjects.Columns.TicketPriority || columnName == DatabaseObjects.Columns.TicketPriorityLookup)
                width = new Unit("60px");
            else if (columnName == DatabaseObjects.Columns.TicketId)
                width = new Unit("105px");
            else if (columnName == "DateTime")
                width = new Unit("105px");
            else if (columnName == DatabaseObjects.Columns.TicketStatus)
                width = new Unit("125px");
            else if (columnName == DatabaseObjects.Columns.TicketAge)
                width = new Unit("50px");
            else if (columnName == DatabaseObjects.Columns.ProjectHealth || columnName == DatabaseObjects.Columns.ProjectRank)
                width = new Unit("30px");
            else if (columnName == DatabaseObjects.Columns.SelfAssign)
                width = new Unit("20px");
            else if (columnName == DatabaseObjects.Columns.Attachments)
                width = new Unit("8px");
            else if (columnName == "CheckBox")
                width = new Unit("20px");
            // else use default width from above

            return width;
        }

        private void BindModule()
        {
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            DataTable modules = moduleViewManager.LoadAllModules();//uGITCache.GetModuleList(ModuleType.All);
            if (IsDeleteTicket)
            {
                if (modules != null)
                {
                    DataRow[] selectedRows = modules.Select(string.Format("{0}=True And {1}=True", DatabaseObjects.Columns.EnableModule, DatabaseObjects.Columns.AllowTicketDelete), DatabaseObjects.Columns.ModuleName);
                    if (selectedRows.Length > 0)
                    {
                        DataTable dtSelectedModules = selectedRows.CopyToDataTable();
                        foreach (DataRow module in dtSelectedModules.Rows)
                        {
                            ddlModuleService.Items.Add(new ListItem(Convert.ToString(module[DatabaseObjects.Columns.Title]), Convert.ToString(module[DatabaseObjects.Columns.ModuleName])));
                        }
                    }
                }
            }
            else
            {
                DataTable modulesColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{ApplicationContext.TenantID}'");   //uGITCache.ModuleConfigCache.LoadModuleListByName("",DatabaseObjects.Tables.ModuleColumns);   //GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns);    //uGITCache.GetDataTable(DatabaseObjects.Lists.ModuleColumns);
                if (modules != null && modulesColumns != null && modulesColumns.Rows.Count > 0)
                {
                    DataView dtView = new DataView(modulesColumns);
                    List<string> distinctDt = dtView.ToTable(true, DatabaseObjects.Columns.CategoryName).AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.CategoryName)).ToList();
                    distinctDt.Remove(ModuleNames.EDM);
                    distinctDt.Remove(ModuleNames.CMDB);
                    //distinctDt.Remove(ModuleNames.PMM);
                    distinctDt.Remove(ModuleNames.APP);
                    distinctDt.Remove(ModuleNames.CMT);
                    distinctDt.Remove(ModuleNames.SVC);
                    distinctDt.Remove(ModuleNames.TSK);
                    distinctDt.Remove(ModuleNames.WIKI);
                    distinctDt.Sort();
                    bool enableModule = true;
                    DataRow[] moduleRows = modules.Select(DatabaseObjects.Columns.EnableModule + "=" + enableModule);
                    foreach (string moduleName in distinctDt)
                    {
                        DataRow row = moduleRows.Where(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == moduleName).FirstOrDefault();
                        if (row != null)
                        {
                            ddlModuleService.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.Title]), Convert.ToString(row[DatabaseObjects.Columns.ModuleName])));
                        }
                    }
                }
            }

            ddlModuleService.Items.Insert(0, new ListItem("--Select Module--", "0"));
        }

        #endregion

        #region Events

        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            string fieldName = e.Column.FieldName;
            FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
            FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));
            e.Values.Insert(0, fvNonBlanks);
            e.Values.Insert(0, fvBlanks);
            List<KeyValuePair<string, string>> nameCollection = new List<KeyValuePair<string, string>>();
            foreach (FilterValue fValue in e.Values)
            {
                if (fValue.ToString() != "(All)" && fValue.ToString() != "(Blanks)" && fValue.ToString() != "(Non Blanks)")
                {
                    string values = Convert.ToString(fValue);
                    LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(ApplicationContext, e.Column.FieldName, values, true);
                    if (lookUPValueCollection != null && lookUPValueCollection.Count > 0)
                    {
                        foreach (LookupValue lookUpValue in lookUPValueCollection)
                        {
                            nameCollection.Add(new KeyValuePair<string, string>(lookUpValue.Value, lookUpValue.ID));

                        }
                    }
                    else
                    {
                        nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                    }
                }
                else
                {
                    nameCollection.Add(new KeyValuePair<string, string>(fValue.ToString(), fValue.ToString()));
                }
            }

            e.Values.Clear();
            foreach (KeyValuePair<string, string> s in nameCollection)
            {
                FilterValue v = new FilterValue(s.Key, s.Value);
                e.Values.Add(v);
            }

        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = grid.GetDataRow(e.VisibleIndex);

            string moduleName = string.Empty;
            if (currentRow == null)
            {
                return;
            }
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                {
                    moduleName = Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]);
                }
                if (string.IsNullOrEmpty(moduleName))
                {
                    string ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                    moduleName = uHelper.getModuleNameByTicketId(ticketId);
                }

                Ticket objTicket = new Ticket(ApplicationContext, moduleName);

                foreach (object cell in e.Row.Cells)
                {
                    if (cell is GridViewTableDataCell)
                    {
                        GridViewTableDataCell editCell = (GridViewTableDataCell)cell;

                        if (((GridViewDataColumn)editCell.Column).FieldName == DatabaseObjects.Columns.TicketStatus)
                        {
                            if (!string.IsNullOrEmpty(moduleName))
                            {
                                bool onHold = false;
                                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketOnHold))
                                    bool.TryParse(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketOnHold)), out onHold);

                                LifeCycleStage currentStage = null;
                                LifeCycle defaultLifeCycle = objTicket.GetTicketLifeCycle();
                                double totalWeight = 0; //defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                                currentStage = objTicket.GetTicketCurrentStage();
                                if (currentStage != null)
                                {
                                    double tillStageWeight = defaultLifeCycle.Stages.Where(x => x.StageStep < currentStage.StageStep).Sum(x => x.StageWeight);
                                    int pctComplete = (int)(tillStageWeight / totalWeight * 100);
                                    e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = UGITUtility.GetProgressBar(defaultLifeCycle, currentStage, currentRow, DatabaseObjects.Columns.TicketStatus, onHold, false);
                                }
                            }
                        }
                        else if (((GridViewDataColumn)editCell.Column).FieldName == DatabaseObjects.Columns.TicketAge)
                        {
                            bool ageColorByTargetCompletion = _configurationVariableManager.GetValueAsBool(ConfigConstants.AgeColorByTargetCompletion);
                            int ticketAge = UGITUtility.StringToInt(e.GetValue(DatabaseObjects.Columns.TicketAge));
                            e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = UGITUtility.GetAgeBar(currentRow, ageColorByTargetCompletion, ticketAge);
                        }
                    }
                }
            }
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Attachments)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                if (!string.IsNullOrEmpty(values))
                {
                    e.Cell.Text = string.Format("<img src='{0}'></img>", "/Content/images/attach.png");
                }
            }

            if (LookAhead)
            {
                if (e.DataColumn.FieldName.EndsWith("Lookup") || e.DataColumn.FieldName.EndsWith("User"))
                {
                    string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                    if (!string.IsNullOrEmpty(values))
                    {
                        if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
                        {
                            string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                            e.Cell.Text = value;
                        }
                    } 
                }
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            var listOfRelatedTickets = (List<string>)Session["relatedTicket"];
            if (listOfRelatedTickets != null && listOfRelatedTickets.Count >0)
            {
                foreach (var item in listOfRelatedTickets)
                {
                    grid.Selection.SelectRowByKey(item);
                }
            }
            grid.DataSource = GetData();

            if (!string.IsNullOrEmpty(RequestType) && LookAhead)
            {
                lblCheckMsg.Visible = true;
                lblCheckMsg.Text = "Checked tickets are on the basis of title";

            }
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //if (e.Column.FieldName == DatabaseObjects.Columns.Attachments)
            //{
            //    //if (UGITUtility.StringToBoolean(e.Value))
            //    //{
            //    //    e.DisplayText = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
            //    //}
            //    //else
            //    //{
            //    //    e.DisplayText = "";
            //    //}
            //}
        }

        protected void ddlModuleService_SelectedIndexChanged(object sender, EventArgs e)
        {

            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            if (ddlModuleService.SelectedValue == "0")
            {
                nameValues.Set("Module", string.Empty);
            }
            else
            {
                nameValues.Set("Module", ddlModuleService.SelectedValue);
            }

            if (chkIncClosedTkts.Checked)
            {
                nameValues.Set("IncludeCloseTkts", "1");
            }
            else
            {
                nameValues.Remove("IncludeCloseTkts");
            }
            Response.Redirect(string.Format("{0}?{1}", UGITUtility.GetAbsoluteURL(Request.Url.AbsolutePath), nameValues.ToString()));
        }

        #endregion

    }
}
