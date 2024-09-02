using DevExpress.Web;
using DevExpress.Web.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class AccountChangeProcess : UserControl
    {
        public string Module { get; set; }
        public List<string> ExcludedTickets { get; set; }
        public bool IsFilteredTableExist { get; set; }
        public DataTable FilteredTable { get; set; }
        public bool IsMultiSelect { get; set; }
        public List<object> objobjTicketIds = null;
        public List<object> objTaskIds = null;        
        public UserProfile User;
        Ticket moduleRequest = null;
        public string moduleIds = string.Empty;
        List<string> lstTickets = new List<string>();
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ConfigurationVariableManager _configurationVariableManager;

        protected override void OnInit(EventArgs e)
        {            
            IsMultiSelect = true;
            _configurationVariableManager = new ConfigurationVariableManager(context);
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            //ticketGrid.AutoGenerateColumns = false;
            //ticketGrid.SettingsBehavior.AllowDragDrop = false;
            //ticketGrid.SettingsText.EmptyHeaders = "  ";
            //ticketGrid.SettingsBehavior.AllowSort = true;
            ticketGrid.SettingsBehavior.AllowSelectByRowClick = true;
            ticketGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            ticketGrid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            ticketGrid.SettingsBehavior.AllowSelectSingleRowOnly = false;

            taskGrid.SettingsBehavior.AllowSelectByRowClick = true;
            taskGrid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
            taskGrid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            taskGrid.SettingsBehavior.AllowSelectSingleRowOnly = false;
            taskGrid.Settings.GroupFormat = "{1} {2}";

            OldUser.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
            OldUser.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Please fill old User!";
            OldUser.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
            OldUser.UserTokenBoxAdd.ValidationSettings.ValidationGroup = "ValidateNext";

            NewUser.UserTokenBoxAdd.ValidationSettings.RequiredField.IsRequired = true;
            NewUser.UserTokenBoxAdd.ValidationSettings.RequiredField.ErrorText = "Please fill New User!";
            NewUser.UserTokenBoxAdd.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
            NewUser.UserTokenBoxAdd.ValidationSettings.ValidationGroup = "ValidateNext";
            
            ddlModule.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
            ddlModule.devexListBox.ValidationSettings.RequiredField.ErrorText = "Please select atleast one module";
            ddlModule.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
            ddlModule.devexListBox.ValidationSettings.ValidationGroup = "ValidateNext";
            ddlModule.devexListBox.Columns["Title"].Caption = "Choose Module(s):";
            ddlModule.devexListBox.Columns["Title"].Width = 270;
            //GenerateColumns();
            BindGrid(GetData(moduleIds), Module);
           
        }

        //private void GenerateColumns()
        //{
        //    ModuleColumnManager columnManager = new ModuleColumnManager(HttpContext.Current.GetManagerContext());
        //    List<ModuleColumn> moduleColumns = columnManager.Load().Where(x => x.IsDisplay == true && x.CategoryName == "ReplaceUser").OrderBy(x => x.FieldSequence).ToList();
        //    foreach(ModuleColumn col in moduleColumns)
        //    {
        //        GridViewDataTextColumn colId = null;
        //        colId = new GridViewDataTextColumn();

        //        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
        //        colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //        colId.PropertiesTextEdit.EncodeHtml = false;
        //        colId.FieldName = col.FieldName;
        //        colId.Caption = col.FieldDisplayName;
        //        colId.HeaderStyle.Font.Bold = true;
        //        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
        //        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
        //        ticketGrid.Columns.Add(colId);
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            dvPanel1.Visible = true;
            dvPanel2.Visible = false;
            dvPanel3.Visible = false;

            if (ExcludedTickets == null)
            {
                ExcludedTickets = new List<string>();
            }
        }

        protected void ticketGrid_DataBinding(object sender, EventArgs e)
        {
            ticketGrid.DataSource = GetData(moduleIds);
        }

        private DataTable GetData(string moduleIds)
        {
            if (string.IsNullOrEmpty(moduleIds))
            {
                return null;
            }

            string[] arrModuleIds = moduleIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            DataTable dtResult = null;
            DataTable dtFinalResult = new DataTable();
            foreach (var item in arrModuleIds)
            {
                Module = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), Convert.ToInt32(item));

                string ticketIds = ExcludedTickets == null ? string.Empty : string.Join(",", ExcludedTickets.Select(x => string.Format("'{0}'", x)).ToArray());

                //if (!IsFilteredTableExist && !string.IsNullOrEmpty(Module))
                if (!string.IsNullOrEmpty(Module))
                {
                    ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
                    mRequest.UserID = User.Id;                    
                    mRequest.CurrentTab = CustomFilterTab.OpenTickets.ToString(); //new Tab(CustomFilterTab.OpenTickets.ToString(),CustomFilterTab.OpenTickets.ToString());
                                                                                  //if (chkIncClosedTkts.Checked) {
                                                                                  //    mRequest.CurrentTab = CustomFilterTab.AllTickets.ToString(); new Tab(CustomFilterTab.AllTickets.ToString(), CustomFilterTab.AllTickets.ToString());
                                                                                  //}
                    mRequest.ModuleName = Module;
                    //mRequest.ModuleType = ModuleType.SMS;
                    mRequest.CurrentTab = "myopentickets";

                    ModuleStatistics moduleStats = new ModuleStatistics(HttpContext.Current.GetManagerContext());
                    ModuleStatisticResponse stat = moduleStats.Load(mRequest);
                    if (stat != null && stat.CurrentTabCount > 0)
                        dtResult = stat.ResultedData;
                    else
                        dtResult = new DataTable();

                    if (rbOpenAndclosedTicket.Checked)
                    {
                        mRequest.ModuleName = Module;
                        mRequest.CurrentTab = "myclosedtickets";
                        stat = moduleStats.Load(mRequest);

                        if (stat != null && stat.CurrentTabCount > 0)
                            dtResult.Merge(stat.ResultedData);
                    }
                }
                else
                {
                    dtResult = FilteredTable;
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
                    moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), Module);
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

                    if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketPriorityLookup))
                    {
                        dtResult.Columns.Remove(DatabaseObjects.Columns.TicketPriorityLookup);
                    }
                    if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketDesiredCompletionDate))
                    {
                        dtResult.Columns.Remove(DatabaseObjects.Columns.TicketDesiredCompletionDate);
                    }
                    if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        dtResult.Columns.Remove(DatabaseObjects.Columns.TicketRequestTypeLookup);
                    }
                    if (dtResult.Columns.Contains(DatabaseObjects.Columns.Category))
                    {
                        dtResult.Columns.Remove(DatabaseObjects.Columns.Category);
                    }

                    // Condition for excluding PMMID
                    if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketPMMIdLookup))
                    {
                        dtResult.Columns.Remove(DatabaseObjects.Columns.TicketPMMIdLookup);
                    }
                    #region Sorting using module columns
                    ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
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
                    ticketGrid.SettingsPager.Visible = false;
                }

                dtFinalResult.Merge(dtResult);

                if (Module == "SVC")
                {
                    lstTickets.AddRange(dtFinalResult.AsEnumerable().Select(x => x.Field<string>("TicketId")).ToList());
                }
            }

            lstTickets = lstTickets.Distinct().ToList();
            ViewState["lstTickets"] = lstTickets;
            spTotalTickets.Text = Convert.ToString(dtFinalResult.Rows.Count);
            //return dtResult;
            return dtFinalResult;
        }

        private List<UGITTask> GetTaskData()
        {
            DataTable dtFinalResult = new DataTable();
            DataTable dtTicket = new DataTable();
            List<UGITTask> allUgitTask = new List<UGITTask>();
            List<UGITTask> ugitTask = new List<UGITTask>();
            UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            TicketManager ObjTicketManager = new TicketManager(context);

            if(lstTickets == null || lstTickets.Count() <= 0)
            {
                lstTickets = ViewState["lstTickets"] as List<string>;
            }

            if (lstTickets != null && lstTickets.Count() > 0)
            {
                foreach (var ticketID in lstTickets)
                {
                    dtTicket.Clear();
                    var module = uHelper.getModuleNameByTicketId(Convert.ToString(ticketID));
                    if(module == "SVC")
                    {
                        ugitTask = taskManager.Load(x => x.TicketId == ticketID && x.Behaviour == "Task" && (x.AssignedTo.Contains(Convert.ToString(hdOldUserId.Value)) || x.Approver.Contains(Convert.ToString(hdOldUserId.Value))));
                        dtTicket = ObjTicketManager.GetTicketTableBasedOnTicketId(uHelper.getModuleNameByTicketId(Convert.ToString(ticketID)), Convert.ToString(ticketID));
                        DataRow ticket = dtTicket.Select(string.Format("{0} = '{1}' and {2} = '{3}'", DatabaseObjects.Columns.TicketId, Convert.ToString(ticketID), DatabaseObjects.Columns.TenantID, context.TenantID))[0];

                        // using ParentInstance field is used temporarily, to store 'TicketId : Title' value
                        ugitTask.ForEach(x => x.ParentInstance = Convert.ToString(ticket[DatabaseObjects.Columns.TicketId]) + " : " + Convert.ToString(ticket[DatabaseObjects.Columns.Title]));

                        allUgitTask.AddRange(ugitTask);
                    }
                }
            }

            spTotalTasks.Text = Convert.ToString(allUgitTask.Count);
            return allUgitTask;
        }

        private void BindGrid(DataTable dtResult, string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return;
            //TicketStatus MTicketStatus = TicketStatus.All;

            //moduleRequest = new Ticket(HttpContext.Current.GetManagerContext(), moduleName);
            //List<ModuleColumn> columns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();

            //Dictionary<string, string> customProperties = new Dictionary<string, string>();
            //StringBuilder filterDataFields = new StringBuilder();
            //GridViewDataTextColumn colId = null;
            //GridViewDataDateColumn dateTimeColumn = null;

            //ticketGrid.Columns.Clear();

            //if (IsMultiSelect)
            //{
            //    ticketGrid.SettingsBehavior.AllowSelectByRowClick = false;
            //    // added new column checkbox.
            //    GridViewCommandColumn dataTextColumn = new GridViewCommandColumn();
            //    dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //    dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //    //dataTextColumn.FieldName = "";
            //    dataTextColumn.Caption = " ";
            //    dataTextColumn.ShowSelectCheckbox = true;
            //    dataTextColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.Page;
            //    // dataTextColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            //    dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            //    dataTextColumn.HeaderStyle.Font.Bold = true;
            //    // dataTextColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            //    // dataTextColumn.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
            //    dataTextColumn.Width = GetColumnWidth("CheckBox");
            //    dataTextColumn.VisibleIndex = 0;
            //    ticketGrid.Columns.Add(dataTextColumn);
            //}
            //colId = new GridViewDataTextColumn();
            //colId.FieldName = DatabaseObjects.Columns.Id;
            //colId.Caption = DatabaseObjects.Columns.Id;
            //colId.Name = DatabaseObjects.Columns.Id;
            //colId.Visible = false;
            //ticketGrid.Columns.Add(colId);
            //foreach (ModuleColumn moduleColumn in columns)
            //{
            //    customProperties = UGITUtility.GetCustomProperties(Convert.ToString(moduleColumn.CustomProperties), Constants.Separator);


            //    //1. check for column exist is resultedtable or not
            //    //2. Check if closed tickets are being shown then only specified column will be shown.
            //    string fieldColumn = Convert.ToString(moduleColumn.FieldName);
            //    if (dtResult != null && dtResult.Columns.Contains(fieldColumn) && (MTicketStatus != TicketStatus.Closed || (customProperties.ContainsKey(CustomProperties.DisplayForClosed) && UGITUtility.StringToBoolean(customProperties[CustomProperties.DisplayForClosed]))))
            //    {
            //        Type dataType = dtResult.Columns[fieldColumn].DataType;
            //        if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketStatus.ToLower())
            //        {
            //            if (moduleRequest.Module.List_ModuleColumns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.ModuleStepLookup) != null || moduleRequest.Module.List_ModuleColumns.FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.StageStep) != null)
            //            {
            //                colId = new GridViewDataTextColumn();
            //                // colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
            //                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //                //colId.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            //                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //                colId.PropertiesTextEdit.EncodeHtml = false;
            //                colId.FieldName = DatabaseObjects.Columns.TicketStatus;
            //                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
            //                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            //                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

            //                colId.Caption = "Status";
            //                colId.Name = "ProgressBar";
            //                ticketGrid.Columns.Add(colId);
            //            }
            //        }
            //        else if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketAge.ToLower())
            //        {
            //            colId = new GridViewDataTextColumn();
            //            //colId.UnboundType = DevExpress.Data.UnboundColumnType.String;
            //            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //            colId.PropertiesTextEdit.EncodeHtml = false;
            //            colId.FieldName = DatabaseObjects.Columns.TicketAge;

            //            colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
            //            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            //            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

            //            colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
            //            colId.Name = DatabaseObjects.Columns.TicketAge;
            //            ticketGrid.Columns.Add(colId);
            //        }
            //        else if (fieldColumn.ToLower() == DatabaseObjects.Columns.Attachments.ToLower())
            //        {
            //            colId = new GridViewDataTextColumn();
            //            colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            //            colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
            //            colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
            //            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            //            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //            colId.PropertiesTextEdit.EncodeHtml = false;
            //            colId.FieldName = DatabaseObjects.Columns.Attachments;
            //            colId.Caption = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
            //            colId.HeaderStyle.Font.Bold = true;
            //            ticketGrid.Columns.Add(colId);
            //        }
            //        else if (dataType == typeof(DateTime))
            //        {
            //            dateTimeColumn = new GridViewDataDateColumn();
            //            dateTimeColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //            dateTimeColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //            dateTimeColumn.FieldName = Convert.ToString(moduleColumn.FieldName);
            //            dateTimeColumn.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
            //            dateTimeColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            //            dateTimeColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            //            dateTimeColumn.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
            //            dateTimeColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            //            dateTimeColumn.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //            ticketGrid.Columns.Add(dateTimeColumn);
            //        }
            //        else
            //        {
            //            colId = new GridViewDataTextColumn();
            //            colId.FieldName = Convert.ToString(moduleColumn.FieldName);
            //            colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);

            //            if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
            //            {
            //                colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            //            }
            //            if (fieldColumn.ToLower() == DatabaseObjects.Columns.Title.ToLower())
            //            {
            //                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            //                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            //            }
            //            else
            //            {
            //                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            //            }
            //            colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
            //            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            //            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //            ticketGrid.Columns.Add(colId);
            //        }
            //    }
            //}

            ticketGrid.DataBind();
        }

        private void BindTaskGrid(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return;

            taskGrid.DataBind();
            taskGrid.CollapseAll();
        }

        //private Unit GetColumnWidth(string columnName)
        //{
        //    Unit width = new Unit("30px");

        //    if (columnName == DatabaseObjects.Columns.TicketPriority || columnName == DatabaseObjects.Columns.TicketPriorityLookup)
        //        width = new Unit("60px");
        //    else if (columnName == DatabaseObjects.Columns.TicketId)
        //        width = new Unit("105px");
        //    else if (columnName == "DateTime")
        //        width = new Unit("105px");
        //    else if (columnName == DatabaseObjects.Columns.TicketStatus)
        //        width = new Unit("125px");
        //    else if (columnName == DatabaseObjects.Columns.TicketAge)
        //        width = new Unit("50px");
        //    else if (columnName == DatabaseObjects.Columns.ProjectHealth || columnName == DatabaseObjects.Columns.ProjectRank)
        //        width = new Unit("30px");
        //    else if (columnName == DatabaseObjects.Columns.SelfAssign)
        //        width = new Unit("20px");
        //    else if (columnName == DatabaseObjects.Columns.Attachments)
        //        width = new Unit("8px");
        //    else if (columnName == "CheckBox")
        //        width = new Unit("20px");
        //    // else use default width from above

        //    return width;
        //}

        protected void taskGrid_DataBinding(object sender, EventArgs e)
        {
            taskGrid.DataSource = GetTaskData();
        }

        protected void ticketGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {         
        }

        protected void ticketGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = ticketGrid.GetDataRow(e.VisibleIndex);

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

                Ticket objTicket = new Ticket(context, moduleName);

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
                                double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
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

        protected void ticketGrid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
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
                    LookUpValueCollection lookUPValueCollection = new LookUpValueCollection(context, e.Column.FieldName, values, true);
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

        protected void ticketGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Attachments)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                if (!string.IsNullOrEmpty(values))
                {
                    e.Cell.Text = string.Format("<img src='{0}'></img>", "/Content/images/attach.png");
                }
            }
        }

        protected void taskGrid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
        }

        protected void taskGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
        }

        protected void taskGrid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
        }

        protected void taskGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            moduleIds = ddlModule.GetValues().ToString();
                        
            List<UserProfile> oldUsr = HttpContext.Current.GetUserManager().GetUserInfosById(OldUser.GetValues());
            List<UserProfile> newUsr = HttpContext.Current.GetUserManager().GetUserInfosById(NewUser.GetValues());

            User = oldUsr.FirstOrDefault();

            if (oldUsr.FirstOrDefault().Id == newUsr.FirstOrDefault().Id)
            {
                lblUserError.Visible = true;
                lblUserError.Text = "Old and New users cannot be same.";
                return;
            }

            hdOldUserId.Value = oldUsr.FirstOrDefault().Id;
            hdNewUserId.Value = newUsr.FirstOrDefault().Id;
            hdOldUserName.Value = oldUsr.FirstOrDefault().UserName;
            hdNewUserName.Value = newUsr.FirstOrDefault().UserName;
            //hdSelectedModuleIds.Value = Convert.ToString(moduleIds);
            ViewState["SelectedModuleIds"] = Convert.ToString(moduleIds);

            BindGrid(GetData(moduleIds), Module);
            BindTaskGrid(Module);

            lbOldUser1.Text = oldUsr.FirstOrDefault().Name;
            lbOldUser2.Text = oldUsr.FirstOrDefault().Name;

            lbNewUser1.Text = newUsr.FirstOrDefault().Name;
            lbNewUser2.Text = newUsr.FirstOrDefault().Name;

            dvPanel1.Visible = false;
            dvPanel2.Visible = true;
            dvPanel3.Visible = false;
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            OldUser.SetValues(Convert.ToString(hdOldUserId.Value));
            NewUser.SetValues(Convert.ToString(hdNewUserId.Value));
            ddlModule.SetValues(Convert.ToString(ViewState["SelectedModuleIds"]));

            dvPanel1.Visible = true;
            dvPanel2.Visible = false;
            dvPanel3.Visible = false;
        }

        protected void btnReplaceUser_Click(object sender, EventArgs e)
        {            
            objobjTicketIds = ticketGrid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.TicketId });
            objTaskIds = taskGrid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });

            if (objobjTicketIds.Count <= 0 && objTaskIds.Count <= 0)
            {
                return;
            }
            //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
            if (!chkRunInBackground.Checked)
            {
                ReplaceUser();
                ReplaceUserTasks();
            }
            else
            {
                ThreadStart starter = delegate
                {
                    ReplaceUser();
                    ReplaceUserTasks();
                };

                Thread thread = new Thread(starter);
                thread.IsBackground = true;
                thread.Start();
            }
            //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
            dvPanel1.Visible = false;
            dvPanel2.Visible = false;
            dvPanel3.Visible = true;
        }

        private void ReplaceUser()
        {            
            objobjTicketIds = ticketGrid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.TicketId });

            TicketManager ObjTicketManager = new TicketManager(context);
            ModuleUserStatisticsManager moduleUserStatisticsManager = new ModuleUserStatisticsManager(context);
            ModuleWorkflowHistoryManager moduleWorkflowHistoryManager = new ModuleWorkflowHistoryManager(context);
            List<ModuleUserStatistic> moduleUserStatsColl;
            Ticket TicketRequest = null;
            DataTable dtTicket = new DataTable(); 

            foreach (var ticketID in objobjTicketIds)
            {
                dtTicket.Clear();

                TicketRequest = new Ticket(context, uHelper.getModuleNameByTicketId(Convert.ToString(ticketID)));
                dtTicket = ObjTicketManager.GetTicketTableBasedOnTicketId(uHelper.getModuleNameByTicketId(Convert.ToString(ticketID)), Convert.ToString(ticketID));
                DataRow ticket = dtTicket.Select(string.Format("{0} = '{1}' and {2} = '{3}'", DatabaseObjects.Columns.TicketId, Convert.ToString(ticketID), DatabaseObjects.Columns.TenantID, context.TenantID ))[0];

                ticket.AcceptChanges();
                ticket.SetModified();

                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.BusinessManager))
                    ticket[DatabaseObjects.Columns.BusinessManager] = Convert.ToString(ticket[DatabaseObjects.Columns.BusinessManager]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketInitiator))
                    ticket[DatabaseObjects.Columns.TicketInitiator] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketInitiator]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketRequestor))
                    ticket[DatabaseObjects.Columns.TicketRequestor] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketRequestor]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketPRP))
                    ticket[DatabaseObjects.Columns.TicketPRP] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketPRP]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketORP))
                    ticket[DatabaseObjects.Columns.TicketORP] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketORP]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketTester))
                    ticket[DatabaseObjects.Columns.TicketTester] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketTester]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.Owner))
                    ticket[DatabaseObjects.Columns.Owner] = Convert.ToString(ticket[DatabaseObjects.Columns.Owner]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.Comment))
                    ticket[DatabaseObjects.Columns.Comment] = Convert.ToString(ticket[DatabaseObjects.Columns.Comment]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.CreatedByUser))
                    ticket[DatabaseObjects.Columns.CreatedByUser] = Convert.ToString(ticket[DatabaseObjects.Columns.CreatedByUser]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                if (ticket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers))
                    ticket[DatabaseObjects.Columns.TicketStageActionUsers] = Convert.ToString(ticket[DatabaseObjects.Columns.TicketStageActionUsers]).Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                            
                TicketRequest.CommitChanges(ticket, string.Empty);

                moduleUserStatsColl = moduleUserStatisticsManager.Load(x => x.TicketId == Convert.ToString(ticketID)).ToList();// GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleUserStatistics).Select(query);
                if (moduleUserStatsColl != null && moduleUserStatsColl.Count() > 0)
                {                    
                    foreach (ModuleUserStatistic moduleUserStats in moduleUserStatsColl)
                    {                                                                      
                        moduleUserStats.UserName = moduleUserStats.UserName.Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));
                        moduleUserStatisticsManager.Update(moduleUserStats);                         
                    }
                }
                
                List<ModuleWorkflowHistory> moduleWorkFlowItemColl = moduleWorkflowHistoryManager.Load(x => x.TicketId == Convert.ToString(ticketID));
                if (moduleWorkFlowItemColl != null && moduleWorkFlowItemColl.Count() > 0)
                {
                    foreach (ModuleWorkflowHistory moduleWorkFlow in moduleWorkFlowItemColl)
                    {
                        if(moduleWorkFlow.StageClosedBy != null)
                            moduleWorkFlow.StageClosedBy = moduleWorkFlow.StageClosedBy.Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));

                        if(moduleWorkFlow.StageClosedByName != null)
                            moduleWorkFlow.StageClosedByName = moduleWorkFlow.StageClosedByName.Replace(Convert.ToString(hdOldUserName.Value), Convert.ToString(hdNewUserName.Value));

                        if(moduleWorkFlow.StageClosedBy != null || moduleWorkFlow.StageClosedByName != null)
                            moduleWorkflowHistoryManager.Update(moduleWorkFlow);
                    }
                }
            }

            lblStatusMesg.Text = "User is replaced successfully in selected ticket(s).";            
        }

        private void ReplaceUserTasks()
        {
            //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
            UGITTaskManager TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            objTaskIds = taskGrid.GetSelectedFieldValues(new string[] { DatabaseObjects.Columns.ID });
            UGITTask ugitTask;

            foreach (var taskID in objTaskIds)
            {
                ugitTask = TaskManager.LoadByID(Convert.ToInt64(taskID));
                if (ugitTask != null && ugitTask.ModuleNameLookup == "SVC")
                {
                    if (!string.IsNullOrEmpty(ugitTask.AssignedTo))                    
                        ugitTask.AssignedTo = ugitTask.AssignedTo.Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));

                    if (!string.IsNullOrEmpty(ugitTask.Approver))
                        ugitTask.Approver = ugitTask.Approver.Replace(Convert.ToString(hdOldUserId.Value), Convert.ToString(hdNewUserId.Value));

                    TaskManager.SaveTask(ref ugitTask);
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }        
    }
}