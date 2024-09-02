using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Helpers;
using System.Data;
using System.Text;
using DevExpress.Web;
using uGovernIT.Utility.Entities;
using System.Web.UI.HtmlControls;
using DevExpress.Web.Rendering;

namespace uGovernIT.Web
{
    public partial class TicketsByStage : UserControl
    {
        public string ModuleName { get; set; }
        public int StageStep { get; set; }
        public string StageTitle { get; set; }
        //Added by mudassir 30 jan 2020
        public string LifeCycle { get; set; }
        //
        Ticket moduleRequest;
        public string TicketAlertUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=ManualEscalation");
        public string StageTrendChartUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=BottleneckTrendChart");
        string sourceURL = string.Empty;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        FieldConfigurationManager fmanger = null;
        FieldConfiguration field = null;
        public List<UserProfile> userProfiles = new List<UserProfile>();
        private ConfigurationVariableManager _configurationVariableManager;

        protected override void OnInit(EventArgs e)
        {
            fmanger = new FieldConfigurationManager(context);
            _configurationVariableManager = new ConfigurationVariableManager(context);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            moduleRequest = new Ticket(context, ModuleName);
            sourceURL = Request["Source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
            if (!IsPostBack)
            {
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
                grid.SettingsBehavior.EnableRowHotTrack = true;
                grid.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
                grid.Styles.AlternatingRow.CssClass = "ms-alternatingstrong";
                grid.Styles.SelectedRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
                grid.Styles.RowHotTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#d9e4fd");
                BindGrid(GetData(), ModuleName);

                
            }
        }
        private DataTable GetData()
        {
            ModuleStatisticRequest mRequest = new ModuleStatisticRequest();
            
            mRequest.UserID = Convert.ToString(((uGovernIT.Utility.Entities.UserProfile)HttpContext.Current.Items["CurrentUser"]).Id);
            if(StageTitle.StartsWith("Close"))            
                mRequest.CurrentTab = Convert.ToString(CustomFilterTab.CloseTickets);
            else
                mRequest.CurrentTab = Convert.ToString(CustomFilterTab.AllTickets);

            mRequest.ModuleName = ModuleName;
            DataTable dt = new DataTable(); 
            DataRow[] moduleColumns;
            if (string.IsNullOrWhiteSpace(this.ModuleName))
            {
                dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.CategoryName}='{Constants.MyDashboardTicket}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                moduleColumns = dt.Select(); //UGITUtility.GetDataTable(DatabaseObjects.Tables.ModuleColumns, DatabaseObjects.Columns.CategoryName, Constants.MyDashboardTicket);
            }
            else
            {
                dt = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.CategoryName}='{ModuleName}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                moduleColumns = dt.Select(); //UGITUtility.GetDataTable(DatabaseObjects.Tables.ModuleColumns, DatabaseObjects.Columns.ModuleNameLookup, ModuleName);
            }
            ModuleStatisticResponse stat = new ModuleStatistics(context).Load(mRequest);   // ModuleStatistics.Load(mRequest);
            DataTable dtResult = stat.ResultedData;

            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                
                
                if (ModuleName == ModuleNames.PMM && dtResult.Columns.Contains(DatabaseObjects.Columns.ProjectLifeCycleLookup))
                {
                   
                    DataRow[] dtRowColl = dtResult.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ProjectLifeCycleLookup) && x.Field<Int64>(DatabaseObjects.Columns.ProjectLifeCycleLookup).ToString() == LifeCycle.ToString()).ToArray();
                    if (dtRowColl != null && dtRowColl.Length > 0)
                        dtResult = dtRowColl.CopyToDataTable();
                    else
                        dtResult = dtResult.Clone();
                }
                //
                DataRow[] dtResultRow = dtResult.Select(string.Format("StageStep = '{0}'", StageStep));
                
                if (dtResultRow != null && dtResultRow.Length > 0)
                {
                    dtResult = dtResultRow.CopyToDataTable();

                    if (moduleColumns.Length > 0)
                    {
                        DataRow[] multiuserColumns = moduleColumns.CopyToDataTable().Select(string.Format("{0} = 'MultiUser' or {0} = 'MultiLookup'", DatabaseObjects.Columns.ColumnType));
                        
                        if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketDueIn) || multiuserColumns.Length > 0)
                        {
                            foreach (DataRow row in dtResult.Rows)
                            {
                                if (dtResult.Columns.Contains(DatabaseObjects.Columns.TicketDueIn) && Convert.ToString(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) != string.Empty)
                                    row[DatabaseObjects.Columns.TicketDueIn] = (UGITUtility.StringToDateTime(row[DatabaseObjects.Columns.TicketDesiredCompletionDate]) - DateTime.Now).Days;

                                if (moduleColumns.Length > 0)
                                {
                                    foreach (DataRow moduleColumn in multiuserColumns)
                                    {
                                        if (dtResult.Columns.Contains(Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])))
                                            row[Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])] = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(row[Convert.ToString(moduleColumn[DatabaseObjects.Columns.FieldName])]));
                                    }
                                }
                            }
                        }
                    }

                    // Exclude private projects for non-admin users
                   // dtResult = uHelper.ExcludeIsPrivateMarked(context, dtResult, HttpContext.Current.CurrentUser());
                   // Remove above comment later
                    return dtResult;
                }
            }
            return null;
        }
        private void BindGrid(DataTable dtResult, string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                return;
            TicketStatus MTicketStatus = TicketStatus.All;
            List<ModuleColumn> columns = new List<ModuleColumn>();
            if (MTicketStatus == TicketStatus.Closed)
                columns = moduleRequest.Module.List_ModuleColumns.OrderBy(x => x.FieldSequence).ToList();
            else
                columns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true || (!string.IsNullOrEmpty(x.SelectedTabs)
              && (UGITUtility.SplitString(x.SelectedTabs, Constants.Separator).Any(y => y.Equals("open", StringComparison.InvariantCultureIgnoreCase)) || UGITUtility.SplitString(x.SelectedTabs, Constants.Separator).Any(y => y.Equals("all", StringComparison.InvariantCultureIgnoreCase))))).OrderBy(x => x.FieldSequence).ToList();

            Dictionary<string, string> customProperties = new Dictionary<string, string>();
            StringBuilder filterDataFields = new StringBuilder();
            GridViewDataTextColumn colId = null;
            GridViewDataDateColumn dateTimeColumn = null;

            grid.Columns.Clear();

            // added new column checkbox.
            GridViewCommandColumn dataTextColumn = new GridViewCommandColumn();
            dataTextColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            dataTextColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            //dataTextColumn.FieldName = "";
            dataTextColumn.Caption = " ";
            dataTextColumn.ShowSelectCheckbox = true;

            // dataTextColumn.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            dataTextColumn.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
            dataTextColumn.HeaderStyle.Font.Bold = true;
            // dataTextColumn.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            // dataTextColumn.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
            dataTextColumn.Width = new Unit("30px");
            dataTextColumn.VisibleIndex = 0;
            grid.Columns.Add(dataTextColumn);


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
                            colId.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
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
                    else if (fieldColumn.ToLower() == DatabaseObjects.Columns.Attachments.ToLower())
                    {
                        colId = new GridViewDataTextColumn();
                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                        colId.CellStyle.HorizontalAlign = alignment;
                        colId.PropertiesTextEdit.EncodeHtml = false;
                        colId.FieldName = DatabaseObjects.Columns.Attachments;
                        colId.Caption = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
                        colId.HeaderStyle.Font.Bold = true;
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
                        colId.FieldName = Convert.ToString(moduleColumn.FieldName);
                        string dollorCol = (fieldColumn.ToLower().EndsWith("lookup") || fieldColumn.ToLower().EndsWith("user")) ? string.Format("{0}$", fieldColumn) : fieldColumn;
                        if (dollorCol.EndsWith("$") && dtResult.Rows.Count > 0 && UGITUtility.IfColumnExists(dtResult.Rows[0], dollorCol))
                            colId.FieldName = dollorCol;

                        colId.Caption = Convert.ToString(moduleColumn.FieldDisplayName);
                        if (fieldColumn.ToLower() == DatabaseObjects.Columns.TicketId.ToLower())
                        {
                            colId.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.False;
                        }

                        colId.CellStyle.HorizontalAlign = alignment;
                        colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                        grid.Columns.Add(colId);
                    }
                }
            }

            if (dtResult != null && dtResult.Rows.Count > 0)
                grid.Columns[0].HeaderTemplate = new CommandColumnHeaderTemplate(grid as uGovernIT.Web.ASPxGridView);
            grid.DataBind();
        }
        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            bool needToHandleMultiValue = false;
            List<ModuleColumn> mColumns = moduleRequest.Module.List_ModuleColumns.Where(x => x.IsDisplay == true).OrderBy(x => x.FieldSequence).ToList();

            if (mColumns.Count > 0)
            {
                ModuleColumn multiuserColumn = mColumns.Where(x => x.FieldName == e.Column.FieldName).FirstOrDefault();
                if (multiuserColumn != null && multiuserColumn.ColumnType == "MultiUser")
                {
                    needToHandleMultiValue = true;
                }
            }

            if (needToHandleMultiValue)
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
            string fieldName = e.Column.FieldName;

            //DataRow[] drs = moduleMonitorsTable != null ? moduleMonitorsTable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.ProjectMonitorName, fieldName)) : null;
            //if ((drs != null && drs.Length == 0) || ModuleName != "PMM")
            //{
            FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
            FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));

            e.Values.Insert(0, fvNonBlanks);
            e.Values.Insert(0, fvBlanks);
            //}
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            DataRow currentRow = grid.GetDataRow(e.VisibleIndex);

            string moduleName = string.Empty;
            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            {
                moduleName = Convert.ToString(currentRow[DatabaseObjects.Columns.ModuleNameLookup]);
            }
            if (string.IsNullOrEmpty(moduleName))
            {
                string ticketId = Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketId));
                moduleName = uHelper.getModuleNameByTicketId(ticketId);
            }

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
                            LifeCycle defaultLifeCycle = moduleRequest.GetTicketLifeCycle(currentRow); //Ticket.GetTicketLifeCycle(currentRow);
                           
                            double totalWeight = defaultLifeCycle.Stages.Sum(x => x.StageWeight);
                            currentStage = moduleRequest.GetTicketCurrentStage(defaultLifeCycle, currentRow); //Ticket.GetTicketCurrentStage(context, moduleName, currentRow);
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

            if (currentRow != null && moduleRequest != null)
            {
                string viewUrl = UGITUtility.GetAbsoluteURL(moduleRequest.Module.StaticModulePagePath);
                string title = string.Empty;
                string func = string.Empty;

                //Get Row ID
                string ticketId = string.Empty;
                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketId) && Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]) != string.Empty)
                    ticketId = Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]).Trim();
                else if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id))
                    ticketId = Convert.ToString(currentRow[DatabaseObjects.Columns.Id]);

                if (ticketId != string.Empty)
                {
                    string ticketTitle = string.Empty;
                    if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Title))
                        ticketTitle = UGITUtility.TruncateWithEllipsis(Convert.ToString(currentRow[DatabaseObjects.Columns.Title]), 100, string.Empty);

                    title = string.Format("{0}: {1}", Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]), ticketTitle);
                }

                title = uHelper.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
                title = title.Replace("\r\n", ""); // Embedded newlines in title prevent popups from opening

                if (!string.IsNullOrEmpty(viewUrl))
                {
                    string width = "90";
                    string height = "90";

                    func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceURL, width, height);
                }

                e.Row.Attributes.Add("onClick", func);

                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && currentRow[DatabaseObjects.Columns.Id] != null)
                    e.Row.Attributes.Add("ticketId", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                e.Row.Style.Add("cursor", "pointer");
            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            grid.DataSource = GetData();
        }
        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.Attachments)
            {
                if (UGITUtility.StringToBoolean(e.Value))
                {
                    e.DisplayText = string.Format("<img src='{0}'></img>", "/Content/images/attach.gif");
                }
                else
                {
                    e.DisplayText = "";
                }
            }
            else
            {
                field = fmanger.GetFieldByFieldName(e.Column.FieldName);
                if (field != null)
                {                    
                        string value = fmanger.GetFieldConfigurationData(field, Convert.ToString(e.Value));
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            e.DisplayText = value;
                        }
                        else
                        {
                            e.DisplayText = "";
                        }
                }
            }
        }
    }
    public class CommandColumnHeaderTemplate : ITemplate
    {
        ASPxGridView gridView = null;
        public CommandColumnHeaderTemplate(ASPxGridView gridView)
        {
            this.gridView = gridView;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            HtmlInputCheckBox checkBox = new HtmlInputCheckBox();
            container.Controls.Add(checkBox);
            //checkBox.Attributes.Add("onclick", string.Format("{0}.SelectAllRowsOnPage(this.checked);", gridView));
            checkBox.Attributes.Add("onclick", "CheckHeaderCheckBox(this);");
        }

        #endregion
    }
}

