using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Web.Rendering;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using DevExPrinting = DevExpress.XtraPrinting;
using mesureText = System.Windows.Forms;

namespace uGovernIT.Web
{
    public partial class QueryWizardPreview : UserControl
    {
        public long Id { get; set; }
        public int setWidth { get; set; }
        public int setHeight { get; set; }
        public string where { get; set; }
        public string serverrelativeurl = VirtualPathUtility.MakeRelative("~", HttpContext.Current.Request.Url.AbsolutePath);
        private const string V = "████████████████";
        public int Month = 0;
        string title = string.Empty;
        bool ShowWhereDialog = false;
        DashboardQuery query = null;
        List<WhereInfo> whereInfo = null;
        protected bool diplayGantt = false;
        protected ZoomLevel zoomLevel;
        public bool FilterMode { get; set; }
        DataTable moduleMonitorsTable = null;
        DataTable projectMonitorsStateTable = null;
        //DataTable moduleMonitorOptions = null;
        DataTable ResultData = null;
        protected bool IsCustomCallBack = false;
        public bool IsFormattedView { get; set; }
        public bool IsDrilldown { get { return Request["drilldown"] == "1" ? true : false; } }
        ConfigurationVariableManager objConfigurationVariableManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public string companyLogo = "";
        DashboardManager dmanager = new DashboardManager(HttpContext.Current.GetManagerContext());
        QueryHelperManager objQueryHelperManager = new QueryHelperManager(HttpContext.Current.GetManagerContext());
        public bool isUnion;
        DashboardQuery panel = null;
        protected override void OnInit(EventArgs e)
        {
            companyLogo = objConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);
            hdnConfiguration.Set("RequestUrl", Request.Url.AbsolutePath);
            InitializeGrid();
            //moduleMonitorOptions = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitorOptions, $"{DatabaseObjects.Columns.TenantID}='{HttpContext.Current.GetManagerContext().TenantID}'");

            if (!string.IsNullOrEmpty(companyLogo))
            {
                if (!companyLogo.Contains(HttpContext.Current.Request.Url.ToString()))
                {
                    companyLogo = "~/" + companyLogo;
                }
                else
                {
                    companyLogo.Replace(HttpContext.Current.Request.Url.ToString(), "~/");
                }
            }

            try
            {
                if (Id > 0)
                {
                    Dashboard uDashboard = dmanager.LoadPanelById(Id);
                    query = uDashboard.panel as DashboardQuery;
                    if (query == null)
                        return;
                    whereInfo = query.QueryInfo.WhereClauses;
                    title = uDashboard.Title;

                    if (query.QueryInfo.JoinList != null && query.QueryInfo.JoinList.Count > 0)
                        isUnion = query.QueryInfo.JoinList.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

                    if (string.IsNullOrEmpty(where))
                    {
                        whereInfo = query.QueryInfo.WhereClauses;
                        title = uDashboard.Title;
                        if (string.IsNullOrEmpty(where))
                        {
                            if (!string.IsNullOrEmpty(Request["whereFilter"]))
                            {
                                where = Request["whereFilter"];
                            }
                            else if (!string.IsNullOrEmpty(Request[hdnwhereFilter.UniqueID]))
                            {
                                where = Request[hdnwhereFilter.UniqueID];
                            }
                            else
                            {
                                if (whereInfo.Exists(x => x.Valuetype == qValueType.Parameter))
                                {
                                    ShowWhereDialog = true;
                                    CreateDynamicContols(whereInfo);
                                }
                                where = string.Empty;
                            }

                        }
                    }
                }

                if (IsPostBack)
                {
                    if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "ZoomLevel")))
                    {
                        zoomLevel = (ZoomLevel)Enum.Parse(typeof(ZoomLevel), UGITUtility.GetCookieValue(Request, "ZoomLevel"));
                    }
                    else
                    {
                        zoomLevel = ZoomLevel.Yearly;
                    }
                    if (Request.Form["__CALLBACKPARAM"] != null && Request.Form["__CALLBACKPARAM"].Contains("CUSTOMCALLBACK"))
                    {
                        IsCustomCallBack = true;
                        ZoomLevel zoom = zoomLevel;
                        if (Request.Form["__CALLBACKPARAM"].Contains("+"))
                        {
                            if (zoom == ZoomLevel.Yearly)
                            {
                                zoomLevel = ZoomLevel.HalfYearly;
                            }
                            else if (zoom == ZoomLevel.HalfYearly)
                            {
                                zoomLevel = ZoomLevel.Quarterly;
                            }
                            else if (zoom == ZoomLevel.Quarterly)
                            {
                                zoomLevel = ZoomLevel.Monthly;
                            }
                            else if (zoom == ZoomLevel.Monthly)
                            {
                                zoomLevel = ZoomLevel.Weekly;
                            }
                        }
                        else if (Request.Form["__CALLBACKPARAM"].Contains("-"))
                        {
                            if (zoom == ZoomLevel.Weekly)
                            {
                                zoomLevel = ZoomLevel.Monthly;
                            }
                            else
                                if (zoom == ZoomLevel.Monthly)
                            {
                                zoomLevel = ZoomLevel.Quarterly;
                            }
                            else if (zoom == ZoomLevel.Quarterly)
                            {
                                zoomLevel = ZoomLevel.HalfYearly;
                            }
                            else if (zoom == ZoomLevel.HalfYearly)
                            {
                                zoomLevel = ZoomLevel.Yearly;
                            }

                        }

                        UGITUtility.CreateCookie(Response, "ZoomLevel", zoomLevel.ToString());
                    }
                }
                else
                {
                    Util.Log.ULog.WriteLog($"{HttpContext.Current.GetManagerContext()?.TenantAccountId}|{HttpContext.Current.CurrentUser().Name}: Run Query Report: {title}");
                    zoomLevel = ZoomLevel.Yearly;
                    UGITUtility.CreateCookie(Response, "ZoomLevel", zoomLevel.ToString());
                }


                if (!ShowWhereDialog)
                {

                    ResultData = GetData();
                    gvPreview.Columns.Clear();
                    gvPreview.TotalSummary.Clear();
                    gvPreview.GroupSummary.Clear();
                    BindGrid(ResultData);
                }

                bool advanceMode;
                Boolean.TryParse(UGITUtility.GetCookieValue(Request, Constants.ShowAdvanceFilter), out advanceMode);
                FilterMode = advanceMode;
                if (query.QueryInfo.IsPreviewFormatted && (query.QueryInfo.QueryFormats.FormatType == QueryFormatType.SimpleNumber
                                                       || query.QueryInfo.QueryFormats.FormatType == QueryFormatType.FormattedNumber) && !IsDrilldown)
                {
                    pnlWizardPreview.Visible = false;
                    pnlFormatPreview.Visible = true;
                    ShowFormattedData();
                }
                else if (query.QueryInfo.IsPreviewFormatted && !IsDrilldown)
                {
                    phStyleGrid.Visible = true;
                    trToolBar.Visible = false;
                    QueryFormat queryformat = query.QueryInfo.QueryFormats;
                    gvPreview.Theme = "Default";
                    pnlWizardPreview.Visible = true;
                    pnlFormatPreview.Visible = false;

                    pnlParameter.Style.Remove(HtmlTextWriterStyle.Display);
                    gvPreview.Settings.ShowGroupPanel = false;
                    gvPreview.Settings.ShowFooter = false;
                    gvPreview.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;

                    gvPreview.ForeColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.ResultForeColor));
                    string fontsize = queryformat.ResultFontSize.Substring(0, queryformat.ResultFontSize.Length - 2);
                    gvPreview.Font.Size = FontUnit.Point(Convert.ToInt32(fontsize));
                    switch (queryformat.ResultFontStyle)
                    {
                        case FontStyle.Bold:
                            gvPreview.Font.Bold = true;
                            break;
                        case FontStyle.Italic:
                            gvPreview.Font.Italic = true;
                            break;
                        case FontStyle.Underline:
                            gvPreview.Font.Underline = true;
                            break;
                        default:
                            break;
                    }
                    gvPreview.Font.Name = queryformat.ResultFontName;
                    gvPreview.BackgroundImage.ImageUrl = queryformat.BackgroundImage;
                    gvPreview.Styles.FilterBar.BackColor = gvPreview.Styles.Header.BackColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.HeaderColor));
                    gvPreview.Border.BorderColor = gvPreview.Styles.Cell.Border.BorderColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.BorderColor));
                    gvPreview.Styles.AlternatingRow.BackColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.RowAlternateColor));
                    gvPreview.Styles.Row.BackColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.RowColor));
                    gvPreview.Styles.PagerBottomPanel.BackColor = ColorTranslator.FromHtml(string.Format("#{0}", queryformat.HeaderColor));
                }
                else
                {
                    phStyleGrid.Visible = false;
                    trToolBar.Visible = true;
                    pnlWizardPreview.Visible = true;
                    pnlFormatPreview.Visible = false;
                    pnlParameter.Style.Remove(HtmlTextWriterStyle.Display);
                    gvPreview.Theme = "DevEx";
                }
            }
            catch (Exception ex)
            {
                ULog.AuditTrail(ex.StackTrace.ToString());
            }

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {

                if (Context.Request.Form["__EVENTARGUMENT"] != null &&
                      Context.Request.Form["__EVENTARGUMENT"].EndsWith("__ShowFilterMode__"))
                {
                    FilterMode = true;
                }
                else if (Context.Request.Form["__EVENTARGUMENT"] != null &&
                    Context.Request.Form["__EVENTARGUMENT"].EndsWith("__HideFilterMode__"))
                {
                    FilterMode = false;
                }
                UGITUtility.CreateCookie(Response, Constants.ShowAdvanceFilter, FilterMode.ToString());
            }

            if (FilterMode)
            {
                gvPreview.Settings.ShowFilterRow = true;
                gvPreview.Settings.ShowFilterRowMenu = true;

                imgAdvanceMode.Attributes.Add("title", "Simple Filter");
                imgAdvanceMode.Src = UGITUtility.GetAbsoluteURL("/content/Images/Newfilter.png");

                gvPreview.Settings.ShowHeaderFilterButton = false;
            }
            else
            {
                gvPreview.Settings.ShowFilterRow = false;
                gvPreview.Settings.ShowHeaderFilterButton = true;
                imgAdvanceMode.Attributes.Add("title", "Advanced Filter");
                imgAdvanceMode.Src = UGITUtility.GetAbsoluteURL("/content/Images/Filter_Red_24.png");
            }
            if (whereInfo != null && whereInfo.Exists(x => x.Valuetype == qValueType.Parameter) && pnlParameter.FindControl("Table1") != null && hdnbuttonClicked.Value == "true")
            {
                int counter = 0;
                foreach (var item in whereInfo.Where(x => x.Valuetype == qValueType.Parameter))
                {
                    if (pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) is ASPxDateEdit)
                    {
                        ASPxDateEdit date = pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) as ASPxDateEdit;
                        //if (date != null && ((TextBox)(date.Controls[1])).Text != "")
                        if (date != null && date.Date != DateTime.MinValue)
                        {
                            DateTime dt = date.Date;
                            item.Value = dt.ToString("M/dd/yyyy");
                        }
                    }

                    else if (pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) is TextBox)
                    {
                        TextBox textBox = pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) as TextBox;
                        if (textBox != null)
                        {
                            item.Value = textBox.Text;
                        }
                    }

                    else if (pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) is DropDownList)
                    {
                        DropDownList ddl = pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) as DropDownList;
                        if (ddl != null)
                        {
                            item.Value = ddl.SelectedValue;
                        }
                    }
                    else if (pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) is UserValueBox)
                    {
                        UserValueBox ppeUser = pnlParameter.FindControl(string.Format("Ctrl_{0}", counter)) as UserValueBox;

                        //ArrayList userPRPGroupList = ppeUser.ResolvedEntities;
                        List<string> userPRPGroupList = ppeUser.GetValuesAsList();
                        foreach (var userEntity in userPRPGroupList)
                        {
                            //PickerEntity entity = (PickerEntity)userEntity;
                            UserProfile user = HttpContext.Current.GetUserManager().GetUserById(userEntity);
                            // SPUser user = UserProfile.GetUserByName(entity.Key);
                            if (user != null)
                            {
                                item.Value = user.Name;
                            }

                            else
                            {
                                //SPGroup group = HttpContext.Current.GetUserManager().GetGroupByName(entity.Key);
                                //if (group != null)
                                //{
                                //    item.Value = group.Name;
                                //}

                            }
                        }
                    }
                    counter++;

                }

                if (!whereInfo.Exists(x => x.ParameterRequired && string.IsNullOrEmpty(x.Value)))
                {
                    lblMsg.Text = "";
                    where = GetWhereExpression(whereInfo);
                }
                else
                {
                    lblMsg.Text = "(*) Marked fields are required.";
                }
            }

            if (pnlParameter.Controls.Count == 0)
            {
                pnlgrid.Style.Remove("display");
                pnlParameter.Style.Add("display", "none");
            }
            else
            {
                pnlgrid.Style.Add("display", "none");
                pnlParameter.Style.Remove("display");
            }

            if (IsCustomCallBack)
            {
                // Clear gantt columns if zoom level changed
                int bndColumnIndex = gvPreview.Columns.Count - gvPreview.AllColumns.Where(m => m as GridViewBandColumn != null).ToList().Count;
                while (gvPreview.Columns.Count > bndColumnIndex)
                {
                    gvPreview.Columns.RemoveAt(bndColumnIndex);
                }
                GenerateGanttColumn(ResultData);
            }

            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            gvPreview.Columns.Clear();
            gvPreview.TotalSummary.Clear();
            gvPreview.GroupSummary.Clear();
            BindGrid(ResultData);

            gvPreview.DataBind();

            base.OnPreRender(e);
        }
        private void EnableDrillDown()
        {
            if (ResultData != null && ResultData.Rows.Count > 0)
            {
                var uDashboard = dmanager.LoadPanelById(Id, false);
                var panel = (DashboardQuery)uDashboard.panel;
                if (panel != null && panel.QueryInfo.QueryFormats != null && panel.QueryInfo.QueryFormats.EnableEditUrl && panel.QueryInfo.Tables != null && panel.QueryInfo.Tables.Count >= 1)
                {
                    EditUrlEntity obj = EditUrlManager.GetEditUrl(panel.QueryInfo.Tables[0].Name);
                    if (obj == null)
                        return;
                    TableInfo tableinfo = panel.QueryInfo.Tables.Where(x => x.Name == panel.QueryInfo.Tables[0].Name).FirstOrDefault();
                    List<uGovernIT.Utility.ColumnInfo> columnInfo = tableinfo.Columns;
                    Dictionary<string, string> dicParamMapping = obj.ParamMapping;
                    if (dicParamMapping == null || dicParamMapping.Keys.Count == 0)
                        return;
                    //Get actual mapping of internal name with display name
                    Dictionary<string, string> currentlyMapping = new Dictionary<string, string>();
                    foreach (var param in dicParamMapping)
                    {
                        uGovernIT.Utility.ColumnInfo current = columnInfo.Where(x => x.FieldName == param.Value).FirstOrDefault();
                        if (current != null && !currentlyMapping.ContainsKey(current.FieldName))
                            currentlyMapping.Add(current.FieldName, current.DisplayName);
                    }
                    if (currentlyMapping.Count == 0 || currentlyMapping.Keys.Count != dicParamMapping.Keys.Count)
                        return;

                    ResultData = EditUrlManager.UpdateEditUrlForEach(ResultData, obj, currentlyMapping);
                    if (ResultData != null && ResultData.Rows.Count > 0 && UGITUtility.IfColumnExists("EnableEditUrl", ResultData))
                    {
                        gvPreview.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gvPreview.ClientSideEvents.RowClick = "function(s,e){OpenEditDialog(s,e);}";
                    }
                }
            }
        }
        public DataTable GetData()
        {
            DataTable dt = new DataTable();
            lblMsg.Text = "";
            pnlParameter.Controls.Clear();
            hdnwhereFilter.Value = where;
            DataTable dtTotals = new DataTable();
            dt = objQueryHelperManager.GetReportData(Id, where, ref dtTotals, IsDrilldown);

            return dt;
        }

        private Unit GetWidthByZoomLevel(ZoomLevel zoom)
        {
            Unit width = Unit.Pixel(30);
            switch (zoom)
            {
                case ZoomLevel.Yearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.HalfYearly:
                    width = Unit.Pixel(90);
                    break;
                case ZoomLevel.Quarterly:
                    width = Unit.Pixel(45);
                    break;
                case ZoomLevel.Monthly:
                    width = Unit.Pixel(30);
                    break;
                case ZoomLevel.Weekly:
                    width = Unit.Pixel(30);
                    break;
                default:
                    width = Unit.Pixel(30);
                    break;
            }
            return width;
        }

        private void InitializeGrid()
        {
            gvPreview.Settings.ShowFilterRowMenu = true;
            gvPreview.Settings.ShowHeaderFilterButton = true;
            gvPreview.Settings.ShowFooter = true;
            gvPreview.SettingsPopup.HeaderFilter.Height = 200;
            gvPreview.Settings.ShowGroupPanel = true;
            gvPreview.SettingsPager.PageSize = 15;
            gvPreview.SettingsPager.Mode = GridViewPagerMode.ShowPager;
            gvPreview.Settings.ShowGroupFooter = DevExpress.Web.GridViewGroupFooterMode.VisibleAlways;
            gvPreview.Settings.ShowFilterBar = GridViewStatusBarMode.Visible;
            gvPreview.Styles.AlternatingRow.Enabled = DevExpress.Utils.DefaultBoolean.True;
            gvPreview.Styles.GroupRow.Font.Bold = true;
            gvPreview.Styles.AlternatingRow.BackColor = Color.FromArgb(234, 234, 234);
            gvPreview.Styles.Row.BackColor = Color.White;
        }

        private void BindGrid(DataTable dtable)
        {
            if (gvPreview.Columns.Count <= 0)
            {
                var columns = new List<uGovernIT.Utility.ColumnInfo>();
                string groupedcol = string.Empty;
                if (panel == null)
                {
                    var uDashboard = dmanager.LoadPanelById(Id, false);
                    //var panel = (DashboardQuery)uDashboard.panel;
                    panel = (DashboardQuery)uDashboard.panel;
                }
                if (panel != null && panel.QueryInfo != null)
                {
                    if (panel.QueryInfo.IsGroupByExpanded)
                        gvPreview.ExpandAll();
                    else
                        gvPreview.CollapseAll();
                }
                if (dtable != null && dtable.Rows.Count > 0)
                {
                    if (IsDrilldown)
                    {
                        if (panel.QueryInfo.DrillDownTables != null && panel.QueryInfo.DrillDownTables.Count > 0)
                        {
                            foreach (var t in panel.QueryInfo.DrillDownTables)
                            {
                                columns.AddRange(t.Columns);
                            }
                        }
                    }
                    else
                    {
                        if (panel.QueryInfo.Tables != null && panel.QueryInfo.Tables.Count > 0)
                        {
                            foreach (var t in panel.QueryInfo.Tables)
                            {
                                columns.AddRange(t.Columns);
                            }
                        }
                    }

                    
                    // Default Width of GridView Column
                    columns = UGITUtility.GetColumnStandardWidth(columns);
                    columns = columns.OrderBy(x => x.Sequence).ToList();

                    if (isUnion)
                        columns = columns.AsEnumerable().OrderBy(y => !y.Selected).GroupBy(x => new { field = x.FieldName }).Select(z => z.FirstOrDefault()).ToList();

                    ///Adding Columns in Grid 
                    foreach (var col in columns.OrderBy(x => x.Sequence).ToList())
                    {
                        if (col.FieldName == DatabaseObjects.Columns.ProjectHealth)
                        {
                            if (moduleMonitorsTable == null)
                            {
                                //Load All Monitor in case of pmm projects

                                projectMonitorsStateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState);
                                //SPQuery monitroQuery = new SPQuery();
                                string query = string.Format("{0}='{1}' and TenantID='{2}'", DatabaseObjects.Columns.ModuleNameLookup, "PMM", HttpContext.Current.GetManagerContext().TenantID);
                                moduleMonitorsTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleMonitors).Select(query).CopyToDataTable();

                            }
                            if (moduleMonitorsTable != null && moduleMonitorsTable.Rows.Count > 0)
                            {
                                int i = 1;
                                //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                Font verdana = new Font("Verdana", 8.25F);
                                Size textSize = new Size();
                                //
                                foreach (DataRow monitor in moduleMonitorsTable.Rows)
                                {
                                    int visibleindex = col.Sequence + i;
                                    if (UGITUtility.IsSPItemExist(monitor, DatabaseObjects.Columns.Title))
                                    {
                                        GridViewDataTextColumn colId = new GridViewDataTextColumn();
                                        colId.PropertiesTextEdit.EncodeHtml = false;
                                        colId.FieldName = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.MonitorName).ToString();
                                        colId.Name = string.Format("projecthealth{0}", moduleMonitorsTable.Rows.IndexOf(monitor));
                                        colId.Caption = UGITUtility.GetSPItemValue(monitor, DatabaseObjects.Columns.Title).ToString(); // "Issues Scope $$$$ OnTime Risk";  // 5 monitors: Issues, Scope, Budget, Time, Risk
                                        //colId.Width = new Unit("50px");
                                        //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                        //Get width dynamically
                                        textSize = mesureText.TextRenderer.MeasureText(colId.Caption, verdana);
                                        if (textSize != null && textSize.Width > 0)
                                            colId.Width = textSize.Width + 5;
                                        else
                                            colId.Width = new Unit("40px");
                                        //
                                        colId.PropertiesEdit.DisplayFormatString = "picture";
                                        //colId.VisibleIndex = visibleindex;
                                        colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                        colId.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                                        colId.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                                        colId.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.True;
                                        colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                                        colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                        colId.HeaderStyle.Font.Bold = true;
                                        gvPreview.Columns.Add(colId);
                                        i = i + 1;
                                    }
                                }
                            }
                        }
                        else if (col.FieldName == "GanttView")
                        {
                            diplayGantt = true;
                        }
                        else if (!col.Hidden)
                            AddColumn(col);
                        else
                        {
                            if (panel.QueryInfo.GroupBy != null && panel.QueryInfo.GroupBy.Count > 0)
                            {
                                if (panel.QueryInfo.GroupBy.Exists(x => x.Column.FieldName == col.FieldName))
                                {
                                    AddColumn(col);
                                }
                            }

                        }
                    }
                    
                    ///Group by Section
                    if (panel.QueryInfo.GroupBy != null && panel.QueryInfo.GroupBy.Count > 0)
                    {
                        imgExpand.Visible = true;
                        imgCollapse.Visible = true;
                        foreach (GroupByInfo groupby in panel.QueryInfo.GroupBy)
                        {
                            if (gvPreview.Columns[groupby.Column.DisplayName] != null)
                                gvPreview.GroupBy(gvPreview.Columns[groupby.Column.DisplayName]);
                        }

                    }
                    else
                    {
                        imgExpand.Visible = false;
                        imgCollapse.Visible = false;
                    }
                    
                    ///Order by section
                    if (panel.QueryInfo.OrderBy != null && panel.QueryInfo.OrderBy.Count > 0)
                    {
                        foreach (OrderByInfo ob in panel.QueryInfo.OrderBy)
                        {
                            DevExpress.Data.ColumnSortOrder orderby = (ob.orderBy == OrderBY.ASC) ? DevExpress.Data.ColumnSortOrder.Ascending : DevExpress.Data.ColumnSortOrder.Descending;
                            if (gvPreview.Columns[ob.Column.DisplayName] != null)
                                gvPreview.SortBy(gvPreview.Columns[ob.Column.DisplayName], orderby);
                        }
                    }


                    ///Adding Total Fields.
                    if (panel.QueryInfo.Totals != null && panel.QueryInfo.Totals.Count > 0)
                    {
                        foreach (Utility.ColumnInfo c in panel.QueryInfo.Totals)
                        {
                            DevExpress.Data.SummaryItemType sumtype = DevExpress.Data.SummaryItemType.None;
                            switch (c.Function)
                            {
                                case "Sum":
                                    sumtype = DevExpress.Data.SummaryItemType.Sum;
                                    break;
                                case "Count":
                                    sumtype = DevExpress.Data.SummaryItemType.Count;
                                    break;
                                case "Avg":
                                    sumtype = DevExpress.Data.SummaryItemType.Average;
                                    break;
                                case "Min":
                                    sumtype = DevExpress.Data.SummaryItemType.Min;
                                    break;
                                case "Max":
                                    sumtype = DevExpress.Data.SummaryItemType.Max;
                                    break;
                                default:
                                    break;
                            }

                            ASPxSummaryItem sumItem = new ASPxSummaryItem(c.DisplayName, sumtype);
                            sumItem.ShowInGroupFooterColumn = c.DisplayName;
                            gvPreview.GroupSummary.Add(sumItem);
                            gvPreview.TotalSummary.Add(new ASPxSummaryItem(c.DisplayName, sumtype));
                        }
                    }

                    // To create gantt view.
                    if(diplayGantt)
                        GenerateGanttColumn(dtable);

                }
            }

        }

        private void GenerateGanttColumn(DataTable dtable)
        {
            if (diplayGantt && dtable != null && dtable.Rows.Count > 0)
            {

                dvZoomLevel.Visible = true;
                gvPreview.Settings.ShowGroupPanel = false;
                gvPreview.SettingsBehavior.AllowDragDrop = false;
                gvPreview.Settings.HorizontalScrollBarMode = DevExpress.Web.ScrollBarMode.Auto;

                DateTime dtMax = DateTime.MaxValue;
                DateTime dtMin = DateTime.MinValue;

                query.QueryInfo.Tables.ForEach(x =>
                {
                    if (x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate) != null)
                    {
                        string dspName = x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate).DisplayName;
                        var compDate = dtable.AsEnumerable().Where(m => !m.IsNull(dspName));
                        dtMax = compDate.Count() == 0 ? DateTime.MaxValue : compDate.Max(m => m.Field<DateTime>(dspName));
                    }

                    if (x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate) != null)
                    {
                        string dspName = x.Columns.FirstOrDefault(c => dtable.Columns.Contains(c.DisplayName) && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate).DisplayName;

                        var stDate = dtable.AsEnumerable().Where(m => !m.IsNull(dspName));
                        dtMin = stDate.Count() == 0 ? DateTime.MinValue : stDate.Min(m => m.Field<DateTime>(dspName));
                    }
                });

                if (dtMax != DateTime.MaxValue & dtMin != DateTime.MinValue)
                {
                    switch (zoomLevel)
                    {
                        case ZoomLevel.Monthly:

                            for (DateTime startDate = dtMin; startDate < dtMax.AddMonths(6); startDate = startDate.AddMonths(6))
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                int monthBar = startDate.Month > 6 ? 12 : 6;
                                bndColumn.Name = (monthBar == 6 ? 1 : 7) + " " + startDate.Year.ToString();

                                bndColumn.Caption = (monthBar == 6 ? "January" : "July") + " " + startDate.Year.ToString();

                                bndColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = monthBar > 6 ? 7 : 1; i <= monthBar; i++)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = i.ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);

                                    bndColumn.Columns.Add(subcol);
                                }

                                gvPreview.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.Quarterly:

                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = 0; i < 12; i += 3)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = (i + 1).ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);

                                    bndColumn.Columns.Add(subcol);
                                }
                                gvPreview.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.HalfYearly:
                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;

                                for (int i = 0; i < 12; i += 6)
                                {
                                    GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                    subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                    subcol.Name = (i + 1).ToString();
                                    subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1).Substring(0, 3);
                                    subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                    subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                    subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                    subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                    subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                    subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                    subcol.HeaderStyle.Font.Bold = true;

                                    string colName = subcol.Name + "-" + bndColumn.Name;
                                    if (!ResultData.Columns.Contains(colName))
                                        ResultData.Columns.Add(colName);
                                    bndColumn.Columns.Add(subcol);
                                }
                                gvPreview.Columns.Add(bndColumn);
                            }
                            break;

                        case ZoomLevel.Yearly:
                            for (int yrs = dtMin.Year; yrs <= dtMax.Year; yrs++)
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();
                                bndColumn.Caption = yrs.ToString();
                                bndColumn.Name = yrs.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;


                                GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                subcol.Name = "1"; // For january
                                subcol.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1);
                                subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;

                                subcol.HeaderStyle.Font.Bold = true;
                                bndColumn.Columns.Add(subcol);

                                string colName = subcol.Name + "-" + bndColumn.Name;
                                if (!ResultData.Columns.Contains(colName))
                                    ResultData.Columns.Add(colName);

                                gvPreview.Columns.Add(bndColumn);
                            }
                            break;
                        case ZoomLevel.Weekly:
                            for (DateTime startDate = dtMin; startDate < dtMax; startDate = startDate.AddMonths(1))
                            {
                                GridViewBandColumn bndColumn = new GridViewBandColumn();

                                bndColumn.Name = startDate.Month.ToString() + " " + startDate.Year.ToString();

                                bndColumn.Caption = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month) + " " + startDate.Year.ToString();
                                bndColumn.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                                // col.FixedStyle = GridViewColumnFixedStyle.Left;
                                bndColumn.HeaderStyle.Font.Bold = true;
                                DateTime dt = new DateTime(startDate.Year, startDate.Month, 1);
                                for (DateTime ctrDate = dt; ctrDate.Day < DateTime.DaysInMonth(startDate.Year, startDate.Month); ctrDate = ctrDate.AddDays(1))
                                {
                                    if (ctrDate.DayOfWeek == DayOfWeek.Monday)
                                    {
                                        GridViewDataTextColumn subcol = new GridViewDataTextColumn();
                                        subcol.UnboundType = DevExpress.Data.UnboundColumnType.String;
                                        subcol.Name = (ctrDate.Day).ToString();
                                        subcol.Caption = (ctrDate.Day).ToString();
                                        subcol.Width = GetWidthByZoomLevel(zoomLevel);
                                        subcol.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                                        subcol.CellStyle.Paddings.PaddingLeft = Unit.Pixel(0);
                                        subcol.CellStyle.Paddings.PaddingRight = Unit.Pixel(0);
                                        subcol.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowInFilterControl = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;

                                        subcol.Settings.AllowAutoFilter = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowDragDrop = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                                        subcol.HeaderStyle.Font.Bold = true;

                                        string colName = subcol.Name + "-" + bndColumn.Name;
                                        if (!ResultData.Columns.Contains(colName))
                                            ResultData.Columns.Add(colName);


                                        bndColumn.Columns.Add(subcol);
                                    }
                                }
                                gvPreview.Columns.Add(bndColumn);
                            }
                            break;
                    }

                }
            }
            else
            {
                dvZoomLevel.Visible = false;
            }
        }
        void AddColumn(Utility.ColumnInfo columnInfo)
        {
            // Skip if a field with same display name is already added to grid
            if (gvPreview.AllColumns.Any(x => x.Caption == columnInfo.DisplayName))
                return;

            GridViewDataTextColumn gvDatatextCol = new GridViewDataTextColumn();
            gvDatatextCol.FieldName = columnInfo.DisplayName;
            gvDatatextCol.Caption = columnInfo.DisplayName;
            gvDatatextCol.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;

            if (FilterMode)
            {
                gvDatatextCol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                gvDatatextCol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                gvDatatextCol.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.False;
                gvDatatextCol.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
            }

            gvDatatextCol.HeaderStyle.Font.Bold = true;
            gvDatatextCol.HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
            HorizontalAlign hAlign = HorizontalAlign.Center;
            switch (columnInfo.Alignment)
            {
                case "Center":
                    hAlign = HorizontalAlign.Center;
                    break;
                case "Left":
                    hAlign = HorizontalAlign.Left;
                    break;
                case "Right":
                    hAlign = HorizontalAlign.Right;
                    break;
                default:
                    if (columnInfo.DisplayName == "Title")
                        hAlign = HorizontalAlign.Left;
                    else
                        hAlign = HorizontalAlign.Center;
                    break;
            }
            gvDatatextCol.GroupFooterCellStyle.HorizontalAlign = hAlign;
            gvDatatextCol.FooterCellStyle.HorizontalAlign = hAlign;
            gvDatatextCol.HeaderStyle.HorizontalAlign = hAlign;
            gvDatatextCol.CellStyle.HorizontalAlign = hAlign;
            switch (columnInfo.DataType)
            {
                case "Currency":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:c}";
                    break;
                case "DateTime":
                case "System.DateTime":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:MMM-d-yyyy hh:mm tt}";
                    break;
                case "Date":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:MMM-d-yyyy}";
                    break;
                case "Time":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:hh:mm tt}";
                    break;
                case "Double":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:#,0.##}";
                    break;
                case "Percent":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:0.#}%";
                    break;
                case "Percent*100":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "{0:0.#%}";
                    break;
                case "Boolean":
                    gvDatatextCol.PropertiesEdit.DisplayFormatString = "Yes;No";
                    break;
                default:
                    break;
            }
            gvPreview.Columns.Add(gvDatatextCol);

        }

        protected void gvPreview_DataBinding(object sender, EventArgs e)
        {
            EnableDrillDown();
            gvPreview.DataSource = ResultData;
            if (panel == null)
            {
                var uDashboard = dmanager.LoadPanelById(Id, false);
                //var panel = (DashboardQuery)uDashboard.panel;
                panel = (DashboardQuery)uDashboard.panel;
            }
            if (panel != null && panel.QueryInfo != null)
            {
                if (panel.QueryInfo.IsGroupByExpanded)
                    gvPreview.ExpandAll();
                else
                    gvPreview.CollapseAll();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // ImageButton btn = (ImageButton)sender;

            switch ("Excel")
            {
                case "Excel":
                    gridExporter.WriteXlsToResponse(title, new DevExPrinting.XlsExportOptionsEx() { ExportType = ExportType.DataAware });
                    break;
                    //case "Pdf": 
                    //    gridExporter.WritePdfToResponse(title);
                    //    break;
                    //case "Print":
                    //    DevExPrinting.PdfExportOptions option = new DevExPrinting.PdfExportOptions();
                    //    option.ShowPrintDialogOnOpen = true;
                    //    gridExporter.WritePdfToResponse(title, option);
                    //    break;
                    //default:
                    //    break;
            }
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
            {
                if (query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                   .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                {
                    foreach (DataRow row in ResultData.Rows)
                    {
                        foreach (DataColumn column in ResultData.Columns)
                        {
                            string data = Convert.ToString(row[column]);

                            if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                            {
                                //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                //row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : "Green"));
                                row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : string.Empty));
                                //
                            }
                        }
                    }
                }
            }
            ReportQueryFormat qryFormat = new ReportQueryFormat();
            qryFormat.ShowDateInFooter = true;
            qryFormat.ShowCompanyLogo = true;
            if (query.QueryInfo.QueryFormats != null)
            {
                qryFormat.AdditionalInfo = query.QueryInfo.QueryFormats.AdditionalInfo;
                qryFormat.Header = query.QueryInfo.QueryFormats.Header;
                qryFormat.Footer = query.QueryInfo.QueryFormats.Footer;
                qryFormat.ShowCompanyLogo = query.QueryInfo.QueryFormats.ShowCompanyLogo;
                qryFormat.AdditionalFooterInfo = query.QueryInfo.QueryFormats.AdditionalFooterInfo;
                qryFormat.ShowDateInFooter = query.QueryInfo.QueryFormats.ShowDateInFooter;
            }
            FillGantViewdata();
            ReplaceExpressionValue();
            string url = UGITUtility.GetAbsoluteURL(Constants.HomePage);
            XtraReport report = reportHelper.GenerateReport(gvPreview, (DataTable)gvPreview.DataSource, title, 8F, "xls", null, qryFormat);
            reportHelper.homePageUrl = url;
            reportHelper.WriteXlsToResponse(Response, title + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

        }

        private string GetImage(string className)
        {
            string fileName = string.Empty;
            switch (className)
            {
                case "GreenLED":
                    fileName = @"/Content/Images/LED_Green.png";
                    break;
                case "YellowLED":
                    fileName = @"/Content/Images/LED_Yellow.png";
                    break;
                case "RedLED":
                    fileName = @"/Content/Images/LED_Red.png";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            ReportGenerationHelper reportHelper = new ReportGenerationHelper();
            reportHelper.companyLogo = companyLogo;
            reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
            reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
            if (query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
            {
                if (query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                   .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                {
                    foreach (DataRow row in ResultData.Rows)
                    {
                        foreach (DataColumn column in ResultData.Columns)
                        {
                            string data = Convert.ToString(row[column]);

                            if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                            {
                                //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                //row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : GetImage("GreenLED")));
                                row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : string.Empty));
                                //
                            }
                        }
                    }
                }
            }
            FillGantViewdata();
            ReplaceExpressionValue();
            ReportQueryFormat qryFormat = new ReportQueryFormat();
            if (query.QueryInfo.QueryFormats != null)
            {
                qryFormat.AdditionalInfo = query.QueryInfo.QueryFormats.AdditionalInfo;
                qryFormat.Header = query.QueryInfo.QueryFormats.Header;
                qryFormat.Footer = query.QueryInfo.QueryFormats.Footer;
                qryFormat.ShowCompanyLogo = query.QueryInfo.QueryFormats.ShowCompanyLogo;
                qryFormat.AdditionalFooterInfo = query.QueryInfo.QueryFormats.AdditionalFooterInfo;
                qryFormat.ShowDateInFooter = query.QueryInfo.QueryFormats.ShowDateInFooter;
            }

            XtraReport report = reportHelper.GenerateReport(gvPreview, ResultData, title, 6.75F, "pdf", null, qryFormat);
            string url = UGITUtility.GetAbsoluteURL(Constants.HomePage);
            reportHelper.homePageUrl = url;

            reportHelper.WritePdfToResponse(Response, title + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

            //gridExporter.WritePdfToResponse(title);
        }

        private void ReplaceExpressionValue()
        {
            foreach (DataRow row in ResultData.Rows)
            {
                foreach (DataColumn column in ResultData.Columns)
                {
                    string data = Convert.ToString(row[column]);
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (data.EndsWith("$yyyymmm") || data.EndsWith("$mmm") || data.EndsWith("$yyyy"))
                        {
                            row[column] = $"{data.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries)[0]}";
                        }
                    }
                }
            }
        }

        void reportHelper_CustomizeColumn(object source, ControlCustomizationEventArgs e)
        {

        }

        void control_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (Convert.ToBoolean(((XRShape)sender).Report.GetCurrentColumnValue("Discontinued")) == true)
                ((XRShape)sender).FillColor = Color.Yellow;
            else
                ((XRShape)sender).FillColor = Color.White;
        }

        void reportHelper_CustomizeColumnsCollection(object source, ColumnsCreationEventArgs e)
        {
            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Title"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Title").ColumnWidth *= 2;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Rank"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Rank").ColumnWidth = 50;

            if (e.ColumnsInfo.Exists(c => c.ColumnCaption == "Priority"))
                e.ColumnsInfo.Find(c => c.ColumnCaption == "Priority").ColumnWidth = 50;
        }

        private void FillGantViewdata()
        {
            List<GridViewColumn> gvDataColumn = gvPreview.AllColumns.Where(m => m as GridViewBandColumn == null).ToList();
            List<GridViewColumn> gvBandColumn = gvPreview.AllColumns.Where(m => m as GridViewBandColumn != null).ToList();

            if (gvBandColumn.Count == 0)
                return;



            for (int i = 0; i < gvPreview.VisibleRowCount; i++)
            {

                DataRow currentRow = gvPreview.GetDataRow(i);

                if (currentRow == null)
                    continue;

                DateTime dtStart = DateTime.MinValue;
                DateTime dtEnd = DateTime.MinValue;
                DateTime? pctCompleteDate = null;
                if (currentRow != null)
                {
                    query.QueryInfo.Tables.ForEach(x =>
                    {
                        if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate).DisplayName;
                            DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtStart);
                        }

                        if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate).DisplayName;

                            DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtEnd);
                        }

                        if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete) != null)
                        {
                            string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete).DisplayName;
                            if (!(currentRow[colName] is DBNull) && dtEnd != DateTime.MinValue && dtStart != DateTime.MinValue)
                            {
                                int days = (int)(Convert.ToDouble(currentRow[colName]) * (dtEnd - dtStart).Days);
                                pctCompleteDate = dtStart.AddDays(days);
                            }
                        }
                    });
                }

                for (int j = 0; j < gvDataColumn.Count; j++)
                {

                    #region GanttView
                    //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                    //int ganttStartIndex = gvPreview.Columns.Count - gvBandColumn.Count;
                    int ganttStartIndex = gvPreview.Columns.Count - gvPreview.AllColumns.Where(m => m as GridViewBandColumn != null).ToList().Count;
                    //
                    if (!string.IsNullOrEmpty(((GridViewDataColumn)gvDataColumn[j]).Name) &&
                        !((GridViewDataColumn)gvDataColumn[j]).Name.StartsWith("projecthealth"))
                    {
                        if (dtStart != DateTime.MinValue && dtEnd != DateTime.MinValue)
                        {
                            string header = ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Name;
                            int yr = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] : header);
                            int month = 1;
                            int day = 1;


                            if (zoomLevel == ZoomLevel.Weekly)
                            {

                                month = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] : "1");
                                day = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);

                                DateTime dtGrid = new DateTime(yr, month, day);
                                if (dtGrid >= dtStart && dtGrid <= dtEnd)
                                {
                                    int bandcolumncount = 0;

                                    for (int k = ganttStartIndex + 1; ((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index >= k; k++)
                                    {
                                        bandcolumncount += ((GridViewBandColumn)gvPreview.Columns[k - 1]).Columns.Count;
                                    }

                                    int index = j + ganttStartIndex + bandcolumncount;

                                    currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = V;
                                }
                            }
                            else
                            {
                                month = UGITUtility.StringToInt(((GridViewDataColumn)gvDataColumn[j]).Name);
                                int period = zoomLevel == ZoomLevel.Yearly ? 12 : (zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1));

                                DateTime dtGrid = new DateTime(yr, month, day);
                                DateTime dtPreGrid = dtGrid.AddMonths(-period);
                                DateTime dtPostGrid = dtGrid.AddMonths(+period);

                                if (dtGrid >= dtStart && dtGrid <= dtEnd && dtPostGrid <= dtEnd)
                                {
                                    //spdelta64
                                    // int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex]).Columns.Count;
                                    //
                                    string col = UGITUtility.ObjectToString(((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header);
                                    if (!UGITUtility.IfColumnExists(currentRow, col))
                                    {
                                        currentRow.Table.Columns.Add(col);
                                    }
                                    currentRow[col] = V;
                                }
                                else if (dtStart >= dtPreGrid && dtStart <= dtPostGrid && dtPostGrid <= dtEnd)
                                {
                                    //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                    //int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex]).Columns.Count;
                                    string col = UGITUtility.ObjectToString(((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header);
                                    if (!UGITUtility.IfColumnExists(currentRow, col))
                                    {
                                        currentRow.Table.Columns.Add(col);
                                        //currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "███████";
                                    }
                                    currentRow[col] = "███████";


                                    //currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "███████";

                                }
                                else if (dtEnd >= dtGrid && dtEnd <= dtPostGrid)
                                {
                                    //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                    //int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;
                                    int index = j + ganttStartIndex + (((GridViewDataColumn)gvDataColumn[j]).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex]).Columns.Count;
                                    //
                                    string col = UGITUtility.ObjectToString(((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header);
                                    if (!UGITUtility.IfColumnExists(currentRow, col))
                                    {
                                        currentRow.Table.Columns.Add(col);
                                        //currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "██████";
                                    }
                                    currentRow[col] = "███████";
                                    //currentRow[((GridViewDataColumn)gvDataColumn[j]).Name + "-" + header] = "██████";
                                }
                            }
                        }
                    }
                    #endregion


                }
            }
        }

        protected void gvPreview_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            DataRow currentRow = gvPreview.GetDataRow(e.VisibleIndex);
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MinValue;
            DateTime? pctCompleteDate = null;
            string ticketID = "";
            if (currentRow != null)
            {
                if (uHelper.IfColumnExists(DatabaseObjects.Columns.TicketId, currentRow.Table))
                    ticketID = Convert.ToString(currentRow[DatabaseObjects.Columns.TicketId]);
                else if (uHelper.IfColumnExists("Ticket ID", currentRow.Table))
                    ticketID = Convert.ToString(currentRow["Ticket ID"]);
                query.QueryInfo.Tables.ForEach(x =>
                {
                    if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate) != null)
                    {
                        string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualStartDate).DisplayName;
                        DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtStart);
                    }

                    if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate) != null)
                    {
                        string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate).DisplayName;

                        DateTime.TryParse(Convert.ToString(currentRow[colName]), out dtEnd);
                    }

                    if (x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete) != null)
                    {
                        string colName = x.Columns.FirstOrDefault(c => gvPreview.Columns[c.DisplayName] != null && c.FieldName == DatabaseObjects.Columns.TicketPctComplete).DisplayName;
                        if (!(currentRow[colName] is DBNull) && dtEnd != DateTime.MinValue && dtStart != DateTime.MinValue)
                        {
                            int days = (int)(Convert.ToDouble(currentRow[colName]) * (dtEnd - dtStart).Days);
                            pctCompleteDate = dtStart.AddDays(days);
                        }
                    }
                });
            }

            // Add links to each row if ticketUrl is available
            if (currentRow != null && currentRow.Table.Columns.Contains("TicketUrl"))
            {
                string ticketUrl = Convert.ToString(currentRow["TicketURL"]);

                if (!string.IsNullOrEmpty(ticketUrl))
                {
                    e.Row.Attributes.Add("onClick", ticketUrl);
                    if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && currentRow[DatabaseObjects.Columns.Id] != null)
                        e.Row.Attributes.Add("ticketId", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                    e.Row.Style.Add("cursor", "pointer");
                }
            }

            foreach (object cell in e.Row.Cells)
            {
                if (cell is GridViewTableDataCell)
                {
                    GridViewTableDataCell editCell = (GridViewTableDataCell)cell;

                    int cellIndex = ((GridViewDataColumn)editCell.Column).Index;

                    query.QueryInfo.Tables.ForEach(x =>
                    {
                        if (x.Columns.FirstOrDefault(c => c.DisplayName == ((GridViewDataColumn)editCell.Column).Caption && c.FieldName == DatabaseObjects.Columns.TicketId) != null)
                        {

                            if (currentRow != null)
                            {
                                string viewUrl = string.Empty;
                                string title = string.Empty;
                                string func = string.Empty;

                                string ticketId = Convert.ToString(e.GetValue(((GridViewDataColumn)editCell.Column).Caption));

                                if (!string.IsNullOrEmpty(ticketId))
                                {
                                    string moduleName = uHelper.getModuleNameByTicketId(ticketId);
                                    if (!string.IsNullOrEmpty(moduleName))
                                    {
                                        string typeName = UGITUtility.moduleTypeName(moduleName);
                                        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                                        UGITModule moduleDetail = ObjModuleViewManager.GetByName(moduleName);

                                        if (moduleDetail != null)
                                        {
                                            viewUrl = string.Empty;

                                            if (!string.IsNullOrEmpty(moduleDetail.StaticModulePagePath))
                                                viewUrl = UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath);

                                            if (ticketId != string.Empty)
                                            {
                                                string ticketTitle = string.Empty;
                                                if (typeName == "Asset")
                                                {
                                                    string colName = DatabaseObjects.Columns.AssetName;
                                                    if (currentRow.Table.Columns.Contains("Asset Name"))
                                                        colName = "Asset Name";

                                                    if (currentRow.Table.Columns.Contains(colName))
                                                    {
                                                        ticketTitle = UGITUtility.TruncateWithEllipsis(currentRow[colName].ToString(), 100, string.Empty);
                                                        title = string.Format("{0}: {1}", currentRow[DatabaseObjects.Columns.TicketId].ToString(), ticketTitle);
                                                    }
                                                }
                                                else
                                                {
                                                    if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Title))
                                                    {
                                                        ticketTitle = UGITUtility.TruncateWithEllipsis(currentRow[DatabaseObjects.Columns.Title].ToString(), 100, string.Empty);
                                                    }

                                                    if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketId) && currentRow[DatabaseObjects.Columns.TicketId] != null)
                                                    {
                                                        title = string.Format("{0}: ", currentRow[DatabaseObjects.Columns.TicketId]);
                                                    }
                                                    title = string.Format("{0}{1}", title, ticketTitle);
                                                }
                                            }

                                            title = uHelper.ReplaceInvalidCharsInURL(title);// # ' " cause issues!
                                            // Prasad: Edit the width & height of ticket popups here!
                                            var sourceUrl = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                                            if (!string.IsNullOrEmpty(viewUrl))
                                            {
                                                string width = "90";
                                                string height = "90";

                                                func = string.Format("openTicketDialog('{0}','{1}','{2}','{4}','{5}', 0, '{3}')", viewUrl, string.Format("TicketId={0}", ticketId), title, sourceUrl, width, height);
                                            }

                                            e.Row.Attributes.Add("onClick", func);
                                            if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && currentRow[DatabaseObjects.Columns.Id] != null)
                                                e.Row.Attributes.Add("ticketId", Convert.ToString(currentRow[DatabaseObjects.Columns.Id]));
                                            e.Row.Style.Add("cursor", "pointer");
                                            //
                                        }

                                    }
                                }
                            }
                        }

                    });

                    #region GanttView

                    if (currentRow == null)
                        return;

                    int ganttStartIndex = gvPreview.Columns.Count - gvPreview.AllColumns.Where(m => m as GridViewBandColumn != null).ToList().Count; //gvPreview.GroupCount + 

                    if (!string.IsNullOrEmpty(((GridViewDataColumn)editCell.Column).Name) &&
                        !((GridViewDataColumn)editCell.Column).Name.StartsWith("projecthealth"))
                    {
                        if (dtStart != DateTime.MinValue && dtEnd != DateTime.MinValue)
                        {
                            string header = ((GridViewDataColumn)editCell.Column).ParentBand.Name;
                            int yr = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1] : header);
                            int month = 1;
                            int day = 1;


                            if (zoomLevel == ZoomLevel.Weekly)
                            {

                                month = UGITUtility.StringToInt(header.Contains(" ") ? header.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] : "1");
                                day = UGITUtility.StringToInt(((GridViewDataColumn)editCell.Column).Name);

                                DateTime dtGrid = new DateTime(yr, month, day);
                                if (dtGrid >= dtStart && dtGrid <= dtEnd)
                                {
                                    int bandcolumncount = 0;

                                    for (int i = ganttStartIndex + 1; ((GridViewDataColumn)editCell.Column).ParentBand.Index >= i; i++)
                                    {
                                        bandcolumncount += ((GridViewBandColumn)gvPreview.Columns[i - 1]).Columns.Count;
                                    }

                                    int index = cellIndex + ganttStartIndex + bandcolumncount;
                                    string html = @"<div class='emptyProgressBar' style='float:left; position:relative; width:100%; min-width:30px;'>
                                        <div class='progressbar' style='float:left; position:absolute; width:10.42%;'>&nbsp;</div><div style='float:left; font-size:90%; position:absolute;'>&nbsp;</div></div>";
                                    if (pctCompleteDate != null && dtGrid >= dtStart && dtGrid <= pctCompleteDate)
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'> <div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                    }
                                    else
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                    }

                                    if (e.Row.Cells.Count > index)
                                        e.Row.Cells[index].Text = html;

                                }
                            }
                            else
                            {
                                month = UGITUtility.StringToInt(((GridViewDataColumn)editCell.Column).Name);
                                //int period = zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1);
                                int period = zoomLevel == ZoomLevel.Yearly ? 12 : (zoomLevel == ZoomLevel.HalfYearly ? 6 : (zoomLevel == ZoomLevel.Quarterly ? 3 : 1));

                                DateTime dtGrid = new DateTime(yr, month, day);
                                DateTime dtPreGrid = dtGrid.AddMonths(-period);
                                DateTime dtPostGrid = dtGrid.AddMonths(+period);

                                if (dtGrid >= dtStart && dtGrid <= dtEnd && dtPostGrid <= dtEnd)
                                {
                                    int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;

                                    string html = string.Empty;

                                    if (dtGrid >= dtStart && dtGrid <= pctCompleteDate)
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                    }
                                    else
                                    {
                                        html = " <div style='float:left;width:100%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                    }
                                    if (e.Row.Cells.Count > index)
                                        e.Row.Cells[index].Text = html;


                                }
                                else if (dtStart >= dtPreGrid && dtStart <= dtPostGrid && dtPostGrid <= dtEnd)
                                {
                                    int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;

                                    string html = string.Empty;
                                    if (dtStart >= dtPreGrid && dtPreGrid <= pctCompleteDate)
                                    {
                                        html = " <div style='float:right;width:50%;font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: right;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                    }
                                    else
                                    {
                                        html = " <div style='float:right;width:50%;font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                    }

                                    if (e.Row.Cells.Count > index)
                                        e.Row.Cells[index].Text = html;



                                }
                                else if (dtEnd >= dtGrid && dtEnd <= dtPostGrid)
                                {
                                    int index = cellIndex + ganttStartIndex + (((GridViewDataColumn)editCell.Column).ParentBand.Index - ganttStartIndex) * ((GridViewBandColumn)gvPreview.Columns[ganttStartIndex + 1]).Columns.Count;

                                    string html = string.Empty;
                                    string margin = string.Empty;
                                    if (dtStart > dtGrid)
                                    {
                                        margin = "margin-left:10%; ";
                                    }

                                    if (dtGrid <= pctCompleteDate)
                                    {
                                        html = " <div style='float:left;width:50%;" + margin + "font-weight: lighter;background-color: darkgray;height: 12px;'><div style='float: left;background-color: #008000;height: 4px;z-index: 6;width: 100%;margin-top: 4px;'></div></div>";
                                    }
                                    else
                                    {
                                        html = " <div style='float:left;width:50%;" + margin + "font-weight: lighter;background-color: darkgray;height: 12px;'></div>";
                                    }
                                    if (e.Row.Cells.Count > index)
                                        e.Row.Cells[index].Text = html;

                                }
                            }
                        }
                    }
                    #endregion

                    if (((GridViewDataColumn)editCell.Column).PropertiesEdit.DisplayFormatString == "Yes;No")
                    {
                        if (editCell.Controls[0] is LiteralControl)
                        {
                            var control = editCell.Controls[0] as LiteralControl;
                            if (control.Text == "1" || control.Text == "True")
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "Yes";
                            }
                            else
                            {
                                e.Row.Cells[((GridViewDataColumn)editCell.Column).Index].Text = "No";
                            }
                        }
                    }


                    //if (((GridViewDataColumn)editCell.Column).Name.StartsWith("projecthealth"))
                    //{
                    //    DataRow monitorRow = moduleMonitorsTable.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.MonitorName) == Convert.ToString(((GridViewDataColumn)editCell.Column).FieldName));
                    //    long monitorId = 0;
                    //    if (monitorRow != null)
                    //        monitorId = UGITUtility.StringToLong(monitorRow[DatabaseObjects.Columns.ID]);
                    //    DataRow[] projectMonitorState = projectMonitorsStateTable.Select(string.Format("{0}='{1}' And {2}='{3}'", DatabaseObjects.Columns.TicketId, ticketID, DatabaseObjects.Columns.ModuleMonitorNameLookup, monitorId));
                    //    DataTable dt = new DataTable();
                    //    dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorName);
                    //    dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup);
                    //    dt.Columns.Add(DatabaseObjects.Columns.ModuleMonitorOptionNameLookup);
                    //    dt.Columns.Add(DatabaseObjects.Columns.ProjectMonitorNotes);
                    //    if (projectMonitorState.Length > 0)
                    //    {
                    //        DataRow commRow = dt.NewRow();
                    //        DataRow monitorOptionRow = moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.ModuleMonitorNameLookup) == monitorId && x.Field<long>(DatabaseObjects.Columns.ID) == UGITUtility.StringToLong(projectMonitorState[0][DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                    //        //moduleMonitorOptions.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleMonitorNameLookup == Convert.ToString(monitorId) && x.Field<string>(DatabaseObjects.Columns.ID)== Convert.ToString(projectMonitorState[DatabaseObjects.Columns.ModuleMonitorOptionIdLookup]));
                    //        if (monitorOptionRow != null)
                    //        {
                    //            commRow[DatabaseObjects.Columns.ModuleMonitorOptionNameLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionName];
                    //            commRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClassLookup] = monitorOptionRow[DatabaseObjects.Columns.ModuleMonitorOptionLEDClass];
                    //        }
                    //        commRow[DatabaseObjects.Columns.ModuleMonitorName] = monitorRow[DatabaseObjects.Columns.MonitorName];
                    //        commRow[DatabaseObjects.Columns.ProjectMonitorNotes] = projectMonitorState[0][DatabaseObjects.Columns.ProjectMonitorNotes];

                    //        editCell.Text = UGITUtility.GetMonitorsGraphic(commRow);
                    //        DataRow ResultedRow = ResultData.AsEnumerable().FirstOrDefault(x => x.Field<string>("Ticket ID") == ticketID);
                    //        ResultedRow[Convert.ToString(((GridViewDataColumn)editCell.Column).FieldName)] = editCell.Text;
                    //        //e.Row.Cells[cellIndex].Text = UGITUtility.GetMonitorsGraphic(commRow);
                    //    }
                    //}

                }
            }

        }

        /// <summary>
        /// This function creates the dynamic textboxes and a button to get value for parameters.
        /// </summary>
        /// <param name="whereList"></param>
        protected void CreateDynamicContols(List<WhereInfo> whereList)
        {
            if (ShowWhereDialog)
            {
                pnlParameter.Controls.Clear();
                //Creat the Table and Add it to the where panel
                Table table = new Table();
                table.ID = "Table1";
                table.Width = Unit.Percentage(100);
                Table headerTable = new Table();
                headerTable.ID = "header";
                headerTable.Width = Unit.Percentage(100);
                headerTable.CssClass = "table-header";

                Table footerTable = new Table();
                footerTable.ID = "footer";
                footerTable.CellPadding = 2;

                if (pnlParameter.FindControl("Table1") == null)
                {
                    pnlParameter.Controls.Add(headerTable);
                    pnlParameter.Controls.Add(table);
                    pnlParameter.Controls.Add(footerTable);
                }
                TableRow lbRow = new TableRow();
                Label lbHeader = new Label();
                lbHeader.ID = "headerLabel";
                lbHeader.CssClass = "headerLabel";
                lbHeader.Text = "Please enter value for parameter(s)";

                TableCell lbCell = new TableCell();
                lbCell.Width = Unit.Percentage(100);
                lbCell.HorizontalAlign = HorizontalAlign.Center;
                lbCell.Controls.Add(lbHeader);
                lbRow.Controls.Add(lbCell);
                headerTable.Rows.Add(lbRow);
                // Now iterate through the table and add controls 
                int i = 0;
                foreach (var item in whereInfo.Where(x => x.Valuetype == qValueType.Parameter))
                {
                    TableRow row = new TableRow();
                    TableCell cell1 = new TableCell();
                    TableCell cell2 = new TableCell();
                    TextBox tb = new TextBox();
                    tb.Width = Unit.Pixel(240);
                    Label lb = new Label();
                    String colName = item.ParameterName;
                    if (item.ParameterRequired)
                        lb.Text = colName + ": <span class='mandatory' style='color:#FF0000;'>*</span>";
                    else
                        lb.Text = colName;
                    cell1.Controls.Add(lb);
                    cell1.CssClass = "pt-3 h6";
                    cell2.CssClass = "param-value pt-3";

                    UserValueBox peopleEditor = new UserValueBox();
                    ASPxDateEdit date = new ASPxDateEdit();
                    peopleEditor.Width = Unit.Pixel(200);

                    switch (item.ParameterType)
                    {
                        case "TextBox":
                            // Add the control to the TableCell
                            cell2.Controls.Add(tb);
                            // Set a unique ID for each control added
                            tb.ID = "Ctrl_" + i;
                            tb.EnableViewState = true;
                            tb.Text = item.Value;
                            break;

                        case "DateTime":
                            // Set a unique ID for each control added
                            cell2.Attributes.Add("paramtype", "date");
                            date.ID = "Ctrl_" + i;
                            date.CssClass = "CRMDueDate_inputField reportDateField";
                            date.DropDownButton.Image.Url = "/Content/Images/calendarNew.png";
                            date.DropDownButton.Image.Width = Unit.Pixel(18);
                            cell1.Controls.Add(lb);
                            cell2.Controls.Add(date);
                            //date.DateOnly = true;
                            DateTime dt = DateTime.Now.Date;
                            DateTime.TryParse(item.Value, out dt);
                            //date.SelectedDate = dt;
                            break;

                        case "DropDown":
                            DropDownList ddlOptions = new DropDownList();
                            ddlOptions.ID = "Ctrl_" + i;
                            ddlOptions.CssClass = "param-value";
                            string[] vals = item.DrpOptions.OptionsDropdown.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            if (vals != null && vals.Length > 0)
                            {
                                for (int k = 0; k < vals.Length; k++)
                                {
                                    ddlOptions.Items.Add(vals[k]);
                                }
                            }

                            ddlOptions.SelectedValue = item.DrpOptions.DropdownDefaultValue;
                            cell2.Controls.Add(ddlOptions);
                            // Set a unique ID for each control added
                            break;
                        case "Lookup":
                            DropDownList drpLookup = new DropDownList();
                            drpLookup.CssClass = "param-value";
                            drpLookup.ID = "Ctrl_" + i;
                            cell2.Controls.Add(drpLookup);

                            DataTable list = GetTableDataManager.GetTableData(item.LookupList.LookupListName);
                            DataColumn field = null;
                            if (list != null && list.Columns.Contains(item.LookupList.LookupField))
                            {
                                field = list.Columns[item.LookupList.LookupField];
                            }

                            if (field != null)
                            {
                                DataTable tb1 = list;
                                DataTable columnVals = new DataTable();
                                if (tb1 != null)
                                {
                                    DataView dView = tb1.DefaultView;
                                    dView.Sort = string.Format("{0} asc", field.ColumnName);
                                    if (tb1.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                                    {
                                        dView.RowFilter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, item.LookupList.LookupModuleName);
                                    }

                                    columnVals = dView.ToTable(true, field.ColumnName, DatabaseObjects.Columns.Id);
                                    drpLookup.DataValueField = field.ColumnName;
                                    //drpLookup.ValueType = typeof(string);
                                    drpLookup.DataTextField = field.ColumnName;
                                    drpLookup.DataSource = columnVals;
                                    drpLookup.DataBind();
                                }
                            }
                            break;
                        case "User":
                            cell2.Attributes.Add("paramtype", "user");
                            UserValueBox ppeUser = new UserValueBox();
                            ppeUser.ID = "Ctrl_" + i;
                            ppeUser.Width = Unit.Pixel(200);
                            ppeUser.SelectionSet = "User";
                            ppeUser.Attributes.Add("ugselectionset", ppeUser.SelectionSet);
                            cell2.Controls.Add(ppeUser);
                            break;
                        case "UserOrGroup":
                            cell2.Attributes.Add("paramtype", "usergroup");
                            UserValueBox ppeUserGroup = new UserValueBox();
                            ppeUserGroup.ID = "Ctrl_" + i;
                            ppeUserGroup.Width = Unit.Pixel(200);
                            ppeUserGroup.Attributes.Add("ugselectionset", ppeUserGroup.SelectionSet);
                            cell2.Controls.Add(ppeUserGroup);
                            break;
                        case "Group":
                            cell2.Attributes.Add("paramtype", "group");
                            UserValueBox ppeGroup = new UserValueBox();
                            ppeGroup.ID = "Ctrl_" + i;
                            ppeGroup.SelectionSet = "Group";
                            ppeGroup.Attributes.Add("ugselectionset", ppeGroup.SelectionSet);
                            cell2.Controls.Add(ppeGroup);
                            break;
                    }

                    // Add the TableCell to the TableRow
                    row.Cells.Add(cell1);
                    row.Cells.Add(cell2);

                    // Add the TableRow to the Table
                    table.Rows.Add(row);
                    i++;
                }

                //add a button to run the query after saving the parameter's value
                footerTable.Width = Unit.Percentage(100);
                TableRow btRow = new TableRow();

                Button bt = new Button();
                bt.ID = "runQuery";
                bt.OnClientClick = "return buttonClick();";
                bt.Click += bt_Click;
                bt.Text = "Run";
                bt.CssClass = "buildReport-btn";
                bt.Width = Unit.Pixel(100);
                bt.Style.Add("padding", "5px");
                HtmlGenericControl runDiv = new HtmlGenericControl("Div");
                runDiv.Style.Add("padding-top", "15px");
                runDiv.Style.Add("float", "right");
                runDiv.Controls.Add(bt);

                ASPxButton btCancel = new ASPxButton();
                btCancel.Text = "Cancel";
                btCancel.Click += BtCancel_Click;
                btCancel.ID = "cancel";
                btCancel.CssClass = "secondary-cancelBtn";
                HtmlGenericControl cancelDiv = new HtmlGenericControl("Div");
                cancelDiv.Style.Add("padding-top", "15px");
                cancelDiv.Style.Add("float", "right");
                cancelDiv.Controls.Add(btCancel);
                //TableCell cellCancel = new TableCell();

                TableCell cell = new TableCell();
                cell.Width = Unit.Percentage(100);
                //cell.HorizontalAlign = HorizontalAlign.Right;
                btRow.Controls.Add(cell);
                cell.Controls.Add(runDiv);
                cell.Controls.Add(cancelDiv);
                //cell.Width = Unit.Percentage(100);
                cell.HorizontalAlign = HorizontalAlign.Right;
                //btRow.Controls.Add(cellCancel);
                footerTable.Rows.Add(btRow);
            }
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        void bt_Click(object sender, EventArgs e)
        {

            ResultData = GetData();
            gvPreview.Columns.Clear();
            gvPreview.TotalSummary.Clear();
            gvPreview.GroupSummary.Clear();

            if (pnlParameter.Controls.Count == 0)
            {
                pnlgrid.Style.Remove("display");
                pnlParameter.Style.Add("display", "none");
            }
            else
            {
                pnlgrid.Style.Add("display", "none");
                pnlParameter.Style.Remove("display");
            }
        }

        /// <summary>
        /// Returns where filter as a string expression from where list.
        /// </summary>
        /// <param name="whereList"></param>
        /// <returns></returns>
        private string GetWhereExpression(List<WhereInfo> whereList)
        {
            string where = string.Empty;
            if (whereList.Count > 0)
            {
                var whereFilter = whereList.Where(w => w.Valuetype == qValueType.Parameter)
                                            .Select(w => w.Value).ToArray();
                where = string.Join(",", whereFilter);
            }
            return where;
            //return Server.UrlEncode(where);
        }

        protected void gvPreview_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            List<FilterValue> temp = new List<FilterValue>(); // List of filter value objects
            List<string> values = new List<string>(); // List in string format used to keep out duplicates
            foreach (FilterValue fValue in e.Values)
            {
                if (e.Column.PropertiesEdit.DisplayFormatString == "Yes;No")
                {
                    string trimmedVal = string.Empty;
                    if (fValue.Value == "1" && !values.Contains("Yes"))
                    {
                        trimmedVal = "Yes";
                        temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] = '{1}'", e.Column.FieldName, "1")));
                    }
                    else if (!values.Contains("No"))
                    {
                        trimmedVal = "No";
                        temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] <> '{1}'", e.Column.FieldName, "1")));
                    }
                    values.Add(trimmedVal);
                }
                else if (fValue.Value.Contains("GreenLED") || fValue.Value.Contains("YellowLED") || fValue.Value.Contains("RedLED"))
                {
                    // Found Monitor State Columns
                    string trimmedVal = string.Empty;
                    if (fValue.Value.Contains("GreenLED") && !values.Contains("Green"))
                    {
                        trimmedVal = "Green";
                        temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                    }
                    else if (fValue.Value.Contains("YellowLED") && !values.Contains("Yellow"))
                    {
                        trimmedVal = "Yellow";
                        temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                    }
                    else if (fValue.Value.Contains("RedLED") && !values.Contains("Red"))
                    {
                        trimmedVal = "Red";
                        temp.Add(new FilterValue(trimmedVal, trimmedVal, string.Format("[{0}] LIKE '%{1}%'", e.Column.FieldName, trimmedVal)));
                    }
                    values.Add(trimmedVal);
                }
                else if (fValue.Value.Contains(";"))
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

            if (!e.Column.Name.Contains("projecthealth"))
            {
                FilterValue fvBlanks = new FilterValue("(Blanks)", "Blanks", string.Format("not ([{0}] is not null and [{0}] <> '')", e.Column.FieldName)); //not ([{0}] is not null and [{0}] <> '')
                FilterValue fvNonBlanks = new FilterValue("(Non Blanks)", "Non Blanks", string.Format("[{0}] is not null and [{0}] <> ''", e.Column.FieldName));
                e.Values.Insert(0, fvNonBlanks);
                e.Values.Insert(0, fvBlanks);
            }
        }

        protected void btnAdvFilter_Click(object sender, EventArgs e)
        {
            ASPxButton btn = sender as ASPxButton;
            if (btn.CommandArgument == "inactive")
            {
                btn.CommandArgument = "active";
                btn.Image.Url = "/content/Images/Filter_Red_24.png";
            }
            //else if (btn.CommandArgument == "active")
            //{            
            //}
        }

        protected void cbMailsend_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "SendMail")
            {
                ReportGenerationHelper reportHelper = new ReportGenerationHelper();
                reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
                reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
                if (query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
                {
                    if (query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                       .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                    {
                        foreach (DataRow row in ResultData.Rows)
                        {
                            foreach (DataColumn column in ResultData.Columns)
                            {
                                string data = Convert.ToString(row[column]);

                                if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                                {
                                    //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                    //row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : GetImage("GreenLED")));
                                    row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : string.Empty));
                                    //
                                }
                            }
                        }
                    }
                }
                FillGantViewdata();
                XtraReport report = reportHelper.GenerateReport(gvPreview, ResultData, title, 6.75F);

                string fileName = string.Format("Query_Report_{0}", uHelper.GetCurrentTimestamp());
                //
                //string uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}.pdf", fileName);
                string uploadFileURL = string.Format("/content/images/ugovernit/upload/{0}.pdf", fileName);
                //
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                report.ExportToPdf(path);
                //
                //e.Result = UGITUtility.GetAbsoluteURL("/_Layouts/15/uGovernIT/DelegateControl.aspx?control=ticketemail&type=queryReport&localpath=" + path + "&relativepath=" + uploadFileURL);
                e.Result = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail&type=queryReport&localpath=" + path + "&relativepath=" + uploadFileURL);
                //
            }
            else if (e.Parameter == "SaveToDoc")
            {
                ReportGenerationHelper reportHelper = new ReportGenerationHelper();
                reportHelper.CustomizeColumnsCollection += reportHelper_CustomizeColumnsCollection;
                reportHelper.CustomizeColumn += reportHelper_CustomizeColumn;
                if (query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
                {
                    if (query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                       .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                    {
                        foreach (DataRow row in ResultData.Rows)
                        {
                            foreach (DataColumn column in ResultData.Columns)
                            {
                                string data = Convert.ToString(row[column]);

                                if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                                {
                                    //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
                                    //row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : GetImage("GreenLED")));
                                    row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? GetImage("GreenLED") : (data.ToLower().Contains("class='yellowled monitoricon'") ? GetImage("YellowLED") : (data.ToLower().Contains("class='redled monitoricon'") ? GetImage("RedLED") : string.Empty));
                                    //
                                }
                            }
                        }
                    }
                }
                FillGantViewdata();
                XtraReport report = reportHelper.GenerateReport(gvPreview, ResultData, title, 6.75F);

                string fileName = string.Format("Query_Report_{0}", uHelper.GetCurrentTimestamp());
                //Spdelta 64
                string uploadFileURL = string.Format("/content/images/ugovernit/upload/{0}.pdf", fileName);
                //
                string path = string.Format("{0}.pdf", System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName));

                report.ExportToPdf(path);
                e.Result = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.ReportUploadControlUrl + "&localpath=" + path + "&relativepath=" + uploadFileURL);
            }
        }

        private void ShowFormattedData()
        {
            StringBuilder sb = new StringBuilder();
            if (query.QueryInfo.IsPreviewFormatted)
            {
                QueryFormat qf = query.QueryInfo.QueryFormats;
                List<Joins> joinsList = query.QueryInfo.JoinList;
                StringBuilder stylesb = new StringBuilder();
                StringBuilder sbNumber = new StringBuilder();
                StringBuilder sbTitle = new StringBuilder();
                string htmlSpan = string.Empty;
                if (qf.FormatType == QueryFormatType.FormattedNumber)
                {
                    int NumberValue = 0;
                    if (ResultData != null && ResultData.Rows.Count > 0)
                    {
                        if (ResultData.Columns.Count > 1)
                        {
                            int x = 0;
                            foreach (DataColumn dc in ResultData.Columns)
                            {
                                if (dc.ColumnName.ToLower() != "id" && dc.ColumnName.ToLower() != "id" + x)
                                {
                                    NumberValue = Convert.ToInt32(Math.Round(UGITUtility.StringToDouble((ResultData.Rows[0][ResultData.Columns.IndexOf(dc)]))));
                                    break;
                                }
                                if (x <= joinsList.Count)
                                    x++;

                            }

                        }
                        else
                        {
                            NumberValue = Convert.ToInt32(Math.Round(UGITUtility.StringToDouble(ResultData.Rows[0][0])));
                        }
                    }




                    setWidth = setWidth > 0 ? setWidth : qf.SizeOfFrame.Width;
                    setHeight = setHeight > 0 ? setHeight : qf.SizeOfFrame.Height;
                    stylesb.AppendFormat("width:{0}px;height:{1}px;position:relative;left:{2}px;top:{3}px;",
                        setWidth, setHeight, qf.Location.X, qf.Location.Y);
                    if (!string.IsNullOrEmpty(qf.BackgroundImage))
                    {
                        stylesb.AppendFormat("background-image:url({0});", qf.BackgroundImage);
                    }
                    if (!string.IsNullOrEmpty(qf.BackgroundColor))
                    {
                        stylesb.AppendFormat("background-color:#{0};", qf.BackgroundColor);
                    }
                    ///for border
                    if (qf.ResultPanelDesign == ResultPanelType.WithIconAndBorder)
                    {
                        stylesb.AppendFormat("border:#{0} solid {1}px;", qf.BorderColor, qf.BorderWidth);
                    }
                    ///for text align
                    if (!string.IsNullOrEmpty(qf.TextAlign))
                    {
                        stylesb.AppendFormat("text-align:{0};", qf.TextAlign);
                    }

                    sb.AppendFormat("<div style=\"{0}\">", stylesb.ToString());

                    ///Top Title Added
                    if (qf.TitlePosition == FloatType.Top)
                    {
                        if (!string.IsNullOrEmpty(qf.Text))
                        {
                            stylesb = new StringBuilder();
                            stylesb.AppendFormat("font-family:{0};", qf.TextFontName);
                            stylesb.AppendFormat("font-size:{0};", qf.TextFontSize);
                            GetFontStyleString(qf.TextFontStyle, stylesb);
                            stylesb.AppendFormat("color:#{0};", qf.TextForeColor);
                            if (qf.HideText)
                            {
                                stylesb.Append("display:none;");
                            }
                            htmlSpan = string.Format("<span style=\"{0}\">{1}</span>", stylesb.ToString(), qf.Text);
                            sbTitle.AppendFormat("<div style=\"width:95%; margin-left:auto; margin-right:auto;\">{0}</div>", htmlSpan);
                            sb.Append(sbTitle.ToString());
                        }
                    }
                    stylesb = new StringBuilder();
                    stylesb.AppendFormat("font-family:{0};", qf.ResultFontName);
                    stylesb.AppendFormat("font-size:{0};", qf.ResultFontSize);
                    GetFontStyleString(qf.ResultFontStyle, stylesb);
                    stylesb.AppendFormat("color:#{0};", qf.ResultForeColor);
                    htmlSpan = string.Format("<span style=\"{0}\">{1}</span>", stylesb.ToString(), NumberValue);

                    stylesb = new StringBuilder();
                    stylesb.AppendFormat("font-family:{0};", qf.LabelFontName);
                    stylesb.AppendFormat("font-size:{0};", qf.LabelFontSize);
                    GetFontStyleString(qf.LabelFontStyle, stylesb);
                    stylesb.AppendFormat("color:#{0};", qf.LabelForeColor);
                    if (qf.HideLabel)
                    {
                        stylesb.Append("display:none;");
                    }
                    string htmlLabel = string.Format("<span style=\"{0}\">{1}</span>", stylesb.ToString(), qf.Label);
                    sbNumber.AppendFormat("<div style=\"width:95%; margin-left:auto; margin-right:auto;\">{0}{1}</div>", htmlSpan, htmlLabel);

                    sb.Append(sbNumber.ToString());

                    string IconElement = string.Empty;
                    ///for Icon

                    if (qf.ResultPanelDesign == ResultPanelType.WithIconAndBorder ||
                        qf.ResultPanelDesign == ResultPanelType.WithIconAndNoBorder)
                    {
                        string iconstyle = string.Format("position:absolute;right:{0}px;top:{1}px;width:{2}px;height:{3}px;",
                                                        qf.IconLocation.X, qf.IconLocation.Y, qf.IconSize.Width, qf.IconSize.Height);

                        IconElement = string.Format("<img src=\"{0}\" style=\"{1}\"/>",
                                                    qf.IconImage, iconstyle);
                        sb.Append(IconElement);
                    }

                    ///Bottom Title Added
                    if (qf.TitlePosition == FloatType.Bottom)
                    {
                        if (!string.IsNullOrEmpty(qf.Text))
                        {
                            stylesb = new StringBuilder();
                            stylesb.AppendFormat("font-family:{0};", qf.TextFontName);
                            stylesb.AppendFormat("font-size:{0};", qf.TextFontSize);
                            GetFontStyleString(qf.TextFontStyle, stylesb);
                            stylesb.AppendFormat("color:#{0};", qf.TextForeColor);
                            if (qf.HideText)
                            {
                                stylesb.Append("display:none;");
                            }
                            htmlSpan = string.Format("<span style=\"{0}\">{1}</span>", stylesb.ToString(), qf.Text);
                            sbTitle.AppendFormat("<div style=\"width:95%; margin-left:auto; margin-right:auto;\">{0}</div>", htmlSpan);
                            sb.Append(sbTitle.ToString());
                        }
                    }

                    sb.Append("</div>");

                }

                if (qf.FormatType == QueryFormatType.SimpleNumber)
                {
                    int NumberValue = 0;
                    if (ResultData != null && ResultData.Rows.Count > 0)
                    {
                        NumberValue = Convert.ToInt32(ResultData.Rows[0][0]);
                    }

                    stylesb.AppendFormat("width:{0}px;height:{1}px;position:relative;left:{2}px;top:{3}px;",
                       qf.SizeOfFrame.Width, qf.SizeOfFrame.Height, qf.Location.X, qf.Location.Y);

                    stylesb.AppendFormat("font-family:{0};", qf.ResultFontName);
                    stylesb.AppendFormat("font-size:{0};", qf.ResultFontSize);
                    GetFontStyleString(qf.ResultFontStyle, stylesb);
                    stylesb.AppendFormat("color:#{0};", qf.ResultForeColor);
                    ///for text align
                    if (!string.IsNullOrEmpty(qf.TextAlign))
                    {
                        stylesb.AppendFormat("text-align:{0};", qf.TextAlign);
                    }
                    sb.AppendFormat("<div style=\"{0}\">", stylesb.ToString());

                    ///Number to added
                    sb.AppendFormat("<span>{0}</span>", NumberValue);
                    if (!string.IsNullOrEmpty(qf.Text))
                    {

                        stylesb = new StringBuilder();
                        stylesb.AppendFormat("font-family:{0};", qf.TextFontName);
                        stylesb.AppendFormat("font-size:{0};", qf.TextFontSize);
                        GetFontStyleString(qf.TextFontStyle, stylesb);
                        stylesb.AppendFormat("color:#{0};", qf.TextForeColor);
                        if (qf.HideText)
                        {
                            stylesb.Append("display:none;");
                        }

                        sbTitle.AppendFormat("<span style=\"{0}\">{1}</span>", stylesb.ToString(), qf.Text);
                        sb.Append(sbTitle.ToString());

                    }
                }

                StringBuilder sblink = new StringBuilder();
                string url = string.Empty;
                string param = string.Empty;
                string href = string.Empty;
                if (qf.DrillDownType != "None")
                {
                    if (qf.DrillDownType == "Custom Query")
                    {
                        url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx");
                        param = string.Format("control=querywizardpreview&ItemId={0}&whereFilter=", qf.QueryId);
                        href = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 90, 80);", url, param, title);

                        sblink.AppendFormat("<a href=\"{0}\" title=\"{2}\" >{1}</a>", href, sb.ToString(), title);
                    }
                    else if (qf.DrillDownType == "Default Drill down")
                    {
                        url = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx");
                        param = string.Format("control=querywizardpreview&ItemId={0}&drilldown=1&whereFilter=", Id);
                        href = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 90, 80);", url, param, title);

                        sblink.AppendFormat("<a href=\"{0}\" title=\"{2}\" >{1}</a>", href, sb.ToString(), title);
                    }
                    else if (qf.DrillDownType == "Custom Url")
                    {
                        url = qf.CustomUrl;
                        if (qf.NavigateType == "Popup")
                        {
                            href = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 90, 80);", url, param, title);
                        }
                        else
                        {
                            href = url;
                        }

                        if (string.IsNullOrEmpty(url))
                        {
                            sblink = sb;
                        }
                        else
                        {
                            sblink.AppendFormat("<a href=\"{0}\" title=\"{2}\" >{1}</a>", href, sb.ToString(), title);
                        }
                    }

                }
                else
                {
                    sblink = sb;
                }

                LiteralControl lc = new LiteralControl();
                lc.Text = sblink.ToString();
                pnlFormatPreview.Controls.Add(lc);
            }
        }

        protected void gvPreview_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            //e.Text = "<b>" + e.Text + "</b>";
            e.Text = e.Text;
            if (e.Text.Contains("="))
            {
                string[] vals = e.Text.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (vals != null && vals.Length == 2)
                {
                    e.Text = string.Format("{0} = {1}", vals[0].Trim(), vals[1].Trim());
                    //e.Text = string.Format("<b>{0}</b>", vals[1].Trim());
                    //e.Text = string.Format("{0}", vals[1].Trim());
                }
            }
        }



        private static void GetFontStyleString(FontStyle fontStyle, StringBuilder stylesb)
        {
            switch (fontStyle)
            {
                case System.Drawing.FontStyle.Bold:
                    stylesb.Append("font-weight:bold;");
                    break;
                case System.Drawing.FontStyle.Italic:
                    stylesb.Append("font-style:italic;");
                    break;
                case System.Drawing.FontStyle.Regular:
                    stylesb.Append("font-style:normal;");
                    break;
                case System.Drawing.FontStyle.Underline:
                    stylesb.Append("text-decoration: underline;");
                    break;
                default:
                    break;
            }
        }

        protected void btnCSVExport_Click(object sender, EventArgs e)
        {
            //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
            // Replace CSS/HTML tags in PMM Project Health column with plain-text
            if (query.QueryInfo.Tables.Exists(x => x.Name == DatabaseObjects.Tables.PMMProjects))
            {
                if (query.QueryInfo.Tables.FirstOrDefault(x => x.Name == DatabaseObjects.Tables.PMMProjects)
                                   .Columns.Exists(x => x.FieldName == DatabaseObjects.Columns.ProjectHealth))
                {
                    foreach (DataRow row in ResultData.Rows)
                    {
                        foreach (DataColumn column in ResultData.Columns)
                        {
                            string data = Convert.ToString(row[column]);

                            if (data.ToLower().Contains("class='greenled monitoricon'") || data.ToLower().Contains("class='yellowled monitoricon'") || data.ToLower().Contains("class='redled monitoricon'"))
                            {
                                row[column] = data.ToLower().Contains("class='greenled monitoricon'") ? "Green" : (data.ToLower().Contains("class='yellowled monitoricon'") ? "Yellow" : (data.ToLower().Contains("class='redled monitoricon'") ? "Red" : string.Empty));
                            }
                        }
                    }
                }
            }

            // Remove unselected columns from result set based on query
            DataTable filterdt = new DataTable();
            Dictionary<string, string> colDatatype = new Dictionary<string, string>();
            if (query != null && query.QueryInfo != null && query.QueryInfo.Tables != null && ResultData != null && ResultData.Rows.Count > 0)
            {
                ReplaceExpressionValue();
                List<string> removeUnselectedCols = new List<string>();
                List<string> selectedCols = new List<string>();
                query.QueryInfo.Tables.ToList().ForEach(x =>
                {
                    selectedCols.AddRange(x.Columns.Where(y => y.Selected).Select(y => y.DisplayName).ToList());
                });
                //BTS-23-001177: Query Export CSV or Excel not recognized as Date format.
                foreach (var col in query.QueryInfo.Tables[0].Columns)
                {
                    if (col.Selected && !string.IsNullOrEmpty(col.DisplayName))
                        colDatatype.Add(col.DisplayName, col.DataType);
                }

                List<string> allColumns = ResultData.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                removeUnselectedCols = allColumns.Except(selectedCols).ToList();
                if (removeUnselectedCols.Count > 0)
                {
                    removeUnselectedCols.ForEach(x =>
                    {
                        ResultData.Columns.Remove(x);
                    });
                }

                filterdt = GetFilterTable(ResultData);
            }

            string csvData = UGITUtility.ConvertTableToCSV(filterdt, colDatatype);
            string attachment = string.Format("attachment; filename={0}.csv", UGITUtility.ReplaceInvalidCharsInFolderName(title).Replace(",", " "));
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "text/csv";
            Response.Write(csvData.ToString());
            Response.Flush();
            Response.End();

        }
        //spdelta 64(Minor fixes for query export to PDF, CSV, Email)
        protected DataTable GetFilterTable(DataTable dt)
        {
            DataTable filterdt = new DataTable();
            string express = gvPreview.FilterExpression;
            if (!string.IsNullOrEmpty(express))
            {
                DataRow[] filterRows = dt.Select(express);
                if (filterRows != null && filterRows.Length > 0)
                    filterdt = filterRows.CopyToDataTable();
                else
                    filterdt = dt.Clone();
            }
            else
                filterdt = dt;
            return filterdt;
        }
        //

        protected void gvPreview_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            var YearMonth = Convert.ToString(e.Value);
            if (!string.IsNullOrEmpty(YearMonth))
            {
                if (YearMonth.EndsWith("$yyyymmm"))
                {
                    Month = Convert.ToInt32(YearMonth.Split(new string[] { "-", " ", "$" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    e.DisplayText = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)} {YearMonth.Split(new string[] { "-", " ", "$" }, StringSplitOptions.RemoveEmptyEntries)[0]}";
                }
                else
                {
                    if (YearMonth.EndsWith("$mmm"))
                    {
                        Month = Convert.ToInt32(YearMonth.Split(new string[] { "-", " ", "$" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                        e.DisplayText = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)}";
                    }
                    else if (YearMonth.EndsWith("$yyyy"))
                    {
                        e.DisplayText = $"{YearMonth.Split(new string[] { "-", " ", "$" }, StringSplitOptions.RemoveEmptyEntries)[0]}";
                    }
                }

            }
        }
    }
}
