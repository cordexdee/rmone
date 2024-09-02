using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class QueryWizard : UserControl
    {
        #region Enums
        public enum QueryClause
        {
            Select,
            From,
            Where,
            GroupBy,
            OrderBy,
            Totals,
            Union
        }

        public enum QueryWizardState
        {
            GeneralInfo,
            TablesJoin,
            Columns,
            WhereClause,
            GroupBy,
            SortBy,
            Totals,
            QueryFormat,
            Drilldown
        }
        #endregion

        #region Local Variables

        private string cschedule = "scheduleaction";
        private string selfurl = "/layouts/ugovernit/uGovernITConfiguration.aspx?control=configdashboardquery&dashboardID={0}&isudlg=1&factTable={1}";
        private string absoluteUrlEdit = "/layouts/ugovernit/DelegateControl.aspx?control={0}&dashboardID={1}&filterID={2}";
        private string formTitle = "Query Dashboard";
        private string newParam = "queryfilter";

        string queryTableName = string.Empty;
        string dashboardTitle = string.Empty;
        string[] essentialFields = new string[] { DatabaseObjects.Columns.Title, DatabaseObjects.Columns.Author, DatabaseObjects.Columns.Editor, DatabaseObjects.Columns.Created, DatabaseObjects.Columns.Modified, DatabaseObjects.Columns.ContentType };

        int templateID = 0;

        public string absoluteUrlQueryExpression = "/layouts/ugovernit/DelegateControl.aspx?control=queryexpression";
        public string factTable
        {
            get { return (String)ViewState["factTable"]; }
            set { ViewState["factTable"] = value; }
        }

        public QueryWizardState FormState
        {
            get
            {
                return (QueryWizardState)Enum.Parse(typeof(QueryWizardState), hdnFormState.Value);
            }
            set
            {
                hdnFormState.Value = value.ToString();
            }
        }

        public long dashboardID = 0;

        DashboardQuery panel;
        Dashboard uDashboard = new Dashboard();

        List<Joins> joins;
        List<TableInfo> tables;
        List<TableInfo> ddtables;

        List<Utility.ColumnInfo> columns;
        List<Utility.ColumnInfo> ddcolumns;
        List<Utility.ColumnInfo> listColumnsTosSave;

        List<GroupByInfo> groupBy;
        List<OrderByInfo> orderBy;
        List<Utility.ColumnInfo> selectedtotals;
        List<Utility.ColumnInfo> selectedColumn;
        public List<WhereInfo> whereClauses;

        QueryFormat queryformat;
        ApplicationContext context = null;
        DashboardManager objDashboardManager = null;
        QueryHelperManager objQueryHelperManager = null;
        private ModuleViewManager _moduleViewManager = null;
        ChartTemplatesManager chartTempManager = null;
        string indentation = "&nbsp;&nbsp;";
        bool isUnion;
        List<Utility.ColumnInfo> unionColumns;
        Dictionary<string, List<string>> dictMissingColumns = null;
        string missingColumns;
        #endregion

        #region Page Events
        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                }
                return _moduleViewManager;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            objDashboardManager = new DashboardManager(context);
            objQueryHelperManager = new QueryHelperManager(context);
            chartTempManager = new ChartTemplatesManager(context);

            // Bind order drop down
            uHelper.BindOrderDropDown(cmbOrder, context);

            // grid.GroupBy(grid.Columns["Country"]);
            if (Request["factTable"] != null)
                factTable = Request["factTable"].Trim();

            FormState = QueryWizardState.GeneralInfo;

            if (Request["templateID"] != null)
            {
                int.TryParse(Request["templateID"], out templateID);
            }
            else
            {
                if (Request[hDashboardId.UniqueID] == null)
                    long.TryParse(Request["dashboardID"], out dashboardID);
                else
                    long.TryParse(Request[hDashboardId.UniqueID], out dashboardID);
            }

            if (dashboardID > 0)
            {
                uDashboard = objDashboardManager.LoadPanelById(dashboardID, false);
                panel = (DashboardQuery)uDashboard.panel;
                tables = panel.QueryInfo.Tables;
                ddtables = panel.QueryInfo.DrillDownTables;
                joins = panel.QueryInfo.JoinList;
                whereClauses = panel.QueryInfo.WhereClauses;
                groupBy = panel.QueryInfo.GroupBy;
                orderBy = panel.QueryInfo.OrderBy;
                selectedtotals = panel.QueryInfo.Totals;
                queryformat = panel.QueryInfo.QueryFormats;
                missingColumns = panel.QueryInfo.MissingColumns;

                if (whereClauses == null)
                    whereClauses = new List<WhereInfo>();
            }
            else if (templateID > 0)
            {
                DataRow chartTemplateItem = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ChartTemplates, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.ID}={templateID}").Select()[0];
                string xmlValue = Convert.ToString(chartTemplateItem[DatabaseObjects.Columns.ChartObject]);
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlValue);
                uDashboard = (Dashboard)uHelper.DeSerializeAnObject(xmlDoc, uDashboard);

                if (Request["dashboardTitle"] != null)
                    uDashboard.Title = Request["dashboardTitle"];

                panel = (DashboardQuery)uDashboard.panel;
                tables = panel.QueryInfo.Tables;
                joins = panel.QueryInfo.JoinList;
                whereClauses = panel.QueryInfo.WhereClauses;
                groupBy = panel.QueryInfo.GroupBy;
                orderBy = panel.QueryInfo.OrderBy;
                selectedtotals = panel.QueryInfo.Totals;
                queryformat = panel.QueryInfo.QueryFormats;
                missingColumns = panel.QueryInfo.MissingColumns;

                if (whereClauses == null)
                    whereClauses = new List<WhereInfo>();
            }
            else
            {
                uDashboard = new Dashboard();
                if (Request["dashboardTitle"] != null)
                    uDashboard.Title = Request["dashboardTitle"];

                if (Request["categoryName"] != null)
                    uDashboard.CategoryName = Request["categoryName"];

                if (Request["queryTable"] != null)
                {
                    panel = new DashboardQuery();
                    panel.QueryTable = Request["queryTable"];
                }

                if (Request["templateID"] != null)
                    int.TryParse(Request["templateID"], out templateID);

                uDashboard.DashboardType = DashboardType.Query;
                uDashboard.ItemOrder = objDashboardManager.LoadAll(true).Count + 1;
                // Save dashboard data
                SaveQueryData();

                // Set new dashboardID in dashboard hidden field for further use and set global filter dashboardID
                hDashboardId.Value = uDashboard.ID.ToString();
                dashboardID = uDashboard.ID;
                tables = new List<TableInfo>();
                joins = new List<Joins>();
                whereClauses = new List<WhereInfo>();
                groupBy = new List<GroupByInfo>();
                orderBy = new List<OrderByInfo>();
                selectedtotals = new List<Utility.ColumnInfo>();
                queryformat = panel.QueryInfo.QueryFormats;
                missingColumns = string.Empty;
            }

            if (joins != null && joins.Count > 0)
                isUnion = joins.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

            if (isUnion)
                DdlJoin.SelectedValue = Constants.Union;

            if (dashboardID > 0 || templateID > 0)
            {
                // Find the columns details from the query which aren't available in Request Lists
                // here we are updating the variable MissingColumns if it has some old values
                if (!string.IsNullOrEmpty(missingColumns))
                {
                    dictMissingColumns = objQueryHelperManager.GetMissingModuleColumns(panel);

                    // Check if the query has missing columns and assign the value to MissingColumns if available
                    bool hasMissingColumns = dictMissingColumns != null && dictMissingColumns.Any(x => x.Value.Count > 0);

                    if (dictMissingColumns == null || dictMissingColumns.Count == 0)
                        missingColumns = string.Empty;
                    else
                        missingColumns = objQueryHelperManager.GetMissingColInString(dictMissingColumns);
                }

                // Update where clause if the TableName is not set for the columns in where clause
                List<WhereInfo> lstWhereClause = QueryHelperManager.UpdateTableNameInWhereClause(panel);

                if (lstWhereClause != null && lstWhereClause.Count > 0)
                {
                    whereClauses = lstWhereClause;
                    SaveQueryData();
                }
            }

            string addNewFilter = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, dashboardID, -1));
            string addQueryUrl = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Filter','600','400',0,'{1}','true')", addNewFilter, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
            aspxAddQuery.ClientSideEvents.Click = string.Format("function(s,e){{ {0} }}", addQueryUrl);

            object cacheVal = Context.Cache.Get(string.Format("QUERYFILTER-{0}", context.CurrentUser.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("QUERYFILTER-{0}", context.CurrentUser.Id));
                FormState = QueryWizardState.WhereClause;
            }
            cacheVal = Context.Cache.Get(string.Format("QueryTabColumn-{0}", context.CurrentUser.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("QueryTabColumn-{0}", context.CurrentUser.Id));
                FormState = QueryWizardState.Columns;
            }

            FillForm();

            if (!IsPostBack && !string.IsNullOrEmpty(queryTableName))
                DdlQueryTables.SelectedValue = queryTableName;

            string schedule = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control={0}&Id={1}", cschedule, dashboardID));
            aSchedule.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Schedule Actions','600','580',0,'{1}','true')", schedule, Server.UrlEncode(Request.Url.AbsolutePath), uDashboard.Title));

            string expression = UGITUtility.GetAbsoluteURL(absoluteUrlQueryExpression + string.Format("&dashboardID={0}", dashboardID));
            btnExpression.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','Query Expression','550','360',0,'{1}','true')", expression, Server.UrlEncode(Request.Url.AbsolutePath)));
            base.OnInit(e);
        }

        private bool isBindWhereClause = false;

        protected override void OnPreRender(EventArgs e)
        {
            BindWhereClause();
            BtJoin2.Visible = DdlQueryTables2.Visible == true && tables.Where(m => m.Name == DdlQueryTables2.SelectedValue).ToList().Count > 0;
            BtJoin3.Visible = DdlQueryTables3.Visible == true && tables.Where(m => m.Name == DdlQueryTables3.SelectedValue).ToList().Count > 0;
            BtJoin4.Visible = DdlQueryTables4.Visible == true && tables.Where(m => m.Name == DdlQueryTables4.SelectedValue).ToList().Count > 0;
            BtJoin5.Visible = DdlQueryTables5.Visible == true && tables.Where(m => m.Name == DdlQueryTables5.SelectedValue).ToList().Count > 0;
            FillQueryFormat();

            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (tabMenu.ActiveTab == null)
                tabMenu.ActiveTab = tabMenu.Tabs[0];

            isPreview.Value = "false";

            if (!IsPostBack)
            {
                queryTableName = string.Empty;
                factTable = string.Empty;

                // Get parameter from request
                long.TryParse(Request["dashboardID"], out dashboardID);
                int.TryParse(Request["templateID"], out templateID);

                if (templateID == 0)
                {
                    if (Request["queryTable"] != null)
                        queryTableName = Request["queryTable"].Trim();

                    if (Request["dashboardTitle"] != null)
                        dashboardTitle = Request["dashboardTitle"].Trim();

                    if (Request["factTable"] != null)
                        factTable = Request["factTable"].Trim();
                }

                // Check if the query has missing columns
                bool hasMissingColumns = !string.IsNullOrEmpty(missingColumns);

                //Bind Missing columns grid if missing columns are found
                if (hasMissingColumns)
                {
                    dictMissingColumns = objQueryHelperManager.GetMissingColInDictionary(missingColumns);

                    if (dictMissingColumns != null && dictMissingColumns.Count > 0)
                    {
                        missingColumnsGrid.DataBind();
                        missingColumnsContainer.ShowOnPageLoad = true;
                    }
                }
            }

            hDashboardId.Value = dashboardID.ToString();
            long.TryParse(Request["dashboardID"], out dashboardID);

            if (dashboardID == 0 && templateID == 0)
            {
                // changes to save Table name in case of new query
                string prevSelectedTable = string.IsNullOrEmpty(queryTableName) ? factTable : queryTableName;

                tables = new List<TableInfo>() { new TableInfo { Name = prevSelectedTable, ID = 1 } };
                panel.QueryInfo.Tables = tables;
                SaveQueryData();
                dashboardID = uDashboard.ID;
                string url = UGITUtility.GetAbsoluteURL(string.Format(selfurl, dashboardID, factTable));
                Response.Redirect(url);
            }
            else if (templateID > 0)
            {
                DataTable dashboard = GetTableDataManager.GetTableData(DatabaseObjects.Tables.DashboardPanels, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
                if (dashboard != null)
                {
                    int order = dashboard.Rows.Count + 1;
                    uDashboard.ItemOrder = order;
                }
                uDashboard.IsActivated = false;
                SaveQueryData();
                dashboardID = uDashboard.ID;
                string url = UGITUtility.GetAbsoluteURL(string.Format(selfurl, dashboardID, factTable));
                Response.Redirect(url, false);
            }
            if (whereClauses != null)
            {
                List<WhereInfo> wiList = whereClauses.Where(m => m.Valuetype == qValueType.Parameter).ToList();

                if (wiList != null && wiList.Count > 0)
                    GenerateDynamicControl(whereClauses);
            }

            if (tables.Count > 1)
            {
                pnlTablesJoin.Visible = true;

                if (DdlJoin.SelectedValue == Constants.Union)
                    JoinTextArea.Text = GetUnionClause(joins);
                else
                    JoinTextArea.Text = GetFromClause(joins, false);

                if (!IsPostBack)
                {
                    BtEditJoin_Click(BtJoin2, e);
                }
            }
            else
            {
                pnlTablesJoin.Visible = false;
            }

            lblmsg.Text = "";

            if (DdlJoin.SelectedValue == Constants.Union)
                DdlAvlTables.Enabled = DdlFirstTabCols.Enabled = DdlSecondTabCols.Enabled = DdlOperator.Enabled = false;
            else
                DdlAvlTables.Enabled = DdlFirstTabCols.Enabled = DdlSecondTabCols.Enabled = DdlOperator.Enabled = true;

            // Disable group labels for Fact Table and Non-Fact Table in tables dropdownlists
            DisableDropDownGroupItems();

            // Enable/Disable grouping in columns tab
            ManageGrouping(grdColumns);

            // Enable/Disable grouping in totals tab
            ManageGrouping(spgrid_totals);
        }
        #endregion

        #region Private Methods
        private void OnTab_Change(QueryWizardState formState)
        {
            btnBack.Visible = true;
            btnNext.Visible = true;
            tabPanel_1.Visible = false;
            tabPanel_2.Visible = false;
            tabPanel_3.Visible = false;
            tabPanel_4.Visible = false;
            tabPanel_5.Visible = false;
            tabPanel_6.Visible = false;
            tabPanel_7.Visible = false;
            tabPanel_8.Visible = false;
            tabPanel_9.Visible = false;
            btnExpression.Visible = false;
            btnValidate.Visible = true;

            switch (FormState)
            {
                case QueryWizardState.GeneralInfo:
                    tabPanel_1.Visible = true;
                    LoadCategories();
                    if (!string.IsNullOrEmpty(uDashboard.CategoryName))
                        ddlCategories.SelectedValue = uDashboard.CategoryName;

                    if (!string.IsNullOrEmpty(uDashboard.SubCategory))
                        ddlSubCategories.SelectedValue = uDashboard.SubCategory;

                    btnBack.Visible = false;
                    break;
                case QueryWizardState.TablesJoin:
                    tabPanel_2.Visible = true;
                    break;
                case QueryWizardState.Columns:
                    btnValidate.Visible = false;
                    btnExpression.Visible = true;
                    tabPanel_3.Visible = true;
                    BindColumnGridView();
                    break;
                case QueryWizardState.WhereClause:
                    tabPanel_4.Visible = true;
                    break;
                case QueryWizardState.GroupBy:
                    tabPanel_5.Visible = true;
                    FillGroupByDropDown();
                    break;
                case QueryWizardState.SortBy:
                    tabPanel_6.Visible = true;
                    FillOrderByDropDown();
                    break;
                case QueryWizardState.Totals:
                    tabPanel_7.Visible = true;
                    // Uncommented, as function in dropdown under Totals,
                    //is not getting selected, when click on Save Changes button.
                    FillTotals(); 
                    break;
                case QueryWizardState.QueryFormat:
                    tabPanel_8.Visible = true;
                    break;
                case QueryWizardState.Drilldown:
                    tabPanel_9.Visible = true;
                    btnNext.Visible = false;
                    BindDrillDownColumnGridView();
                    break;
                default:
                    break;
            }
            tabMenu.ActiveTab = tabMenu.Tabs.FindByName(formState.ToString());
        }

        private void FillGroupByDropDown()
        {
            ddlFirstGroupBy.Items.Clear();
            ddlSecondGroupBy.Items.Clear();
            ddlThirdGroupBy.Items.Clear();
            if(columns==null)
                columns = SelectColumns();
            if (columns == null) return;

            if (isUnion)
            {
                foreach (Utility.ColumnInfo column in unionColumns)
                {
                    // Add only selected columns in Group By dropdowns
                    if (column.Selected && !column.Hidden)
                    {
                        ddlFirstGroupBy.Items.Add(new ListItem(column.FieldName));
                        ddlSecondGroupBy.Items.Add(new ListItem(column.FieldName));
                        ddlThirdGroupBy.Items.Add(new ListItem(column.FieldName));
                    }
                }
            }
            else
            {
                foreach (Utility.ColumnInfo column in columns)
                {
                    // Add only selected columns in Group By dropdowns
                    if (column.Selected && !column.Hidden)
                    {
                        ddlFirstGroupBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                        ddlSecondGroupBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                        ddlThirdGroupBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                    }
                }
            }

            ddlFirstGroupBy.Items.Insert(0, new ListItem("None"));
            ddlSecondGroupBy.Items.Insert(0, new ListItem("None"));
            ddlThirdGroupBy.Items.Insert(0, new ListItem("None"));

            ///Select value from group by object
            if (groupBy != null && groupBy.Count > 0)
            {
                foreach (var groupby in groupBy)
                {
                    switch (groupby.Num)
                    {
                        case 1:
                            if (isUnion)
                            {
                                ddlFirstGroupBy.SelectedIndex = ddlFirstGroupBy.Items.IndexOf(
                                ddlFirstGroupBy.Items.FindByText(groupby.Column.FieldName));
                            }
                            else
                            {
                                ddlFirstGroupBy.SelectedIndex = ddlFirstGroupBy.Items.IndexOf(
                                ddlFirstGroupBy.Items.FindByText(groupby.Column.TableName + '.' + groupby.Column.FieldName));
                            }

                            if (ddlFirstGroupBy.SelectedIndex > 0)
                                txtFirstGroupByDisplaytext.Text = groupby.Column.DisplayName;

                            break;
                        case 2:
                            if (isUnion)
                            {
                                ddlSecondGroupBy.SelectedIndex = ddlSecondGroupBy.Items.IndexOf(
                                ddlSecondGroupBy.Items.FindByText(groupby.Column.FieldName));
                            }
                            else
                            {
                                ddlSecondGroupBy.SelectedIndex = ddlSecondGroupBy.Items.IndexOf(
                                ddlSecondGroupBy.Items.FindByText(groupby.Column.TableName + '.' + groupby.Column.FieldName));
                            }

                            if (ddlSecondGroupBy.SelectedIndex > 0)
                                txtSecondGroupByDisplayText.Text = groupby.Column.DisplayName;

                            break;
                        case 3:
                            if (isUnion)
                            {
                                ddlThirdGroupBy.SelectedIndex = ddlThirdGroupBy.Items.IndexOf(
                                ddlThirdGroupBy.Items.FindByText(groupby.Column.FieldName));
                            }
                            else
                            {
                                ddlThirdGroupBy.SelectedIndex = ddlThirdGroupBy.Items.IndexOf(
                                ddlThirdGroupBy.Items.FindByText(groupby.Column.TableName + '.' + groupby.Column.FieldName));
                            }


                            if (ddlThirdGroupBy.SelectedIndex > 0)
                                txtThirdGroupByDisplayText.Text = groupby.Column.DisplayName;

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void FillOrderByDropDown()
        {
            ddlFirstOrderBy.Items.Clear();
            ddlSecondOrderBy.Items.Clear();
            ddlThirdOrderBy.Items.Clear();
            if(columns== null)
                columns = SelectColumns();

            if (columns == null) return;

            if (isUnion)
            {
                foreach (Utility.ColumnInfo column in unionColumns)
                {
                    // Add only selected columns in Order By dropdowns
                    if (column.Selected && !column.Hidden)
                    {
                        ddlFirstOrderBy.Items.Add(new ListItem(column.FieldName));
                        ddlSecondOrderBy.Items.Add(new ListItem(column.FieldName));
                        ddlThirdOrderBy.Items.Add(new ListItem(column.FieldName));
                    }
                }
            }
            else
            {
                foreach (Utility.ColumnInfo column in columns)
                {
                    // Add only selected columns in Order By dropdowns
                    if (column.Selected && !column.Hidden)
                    {
                        ddlFirstOrderBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                        ddlSecondOrderBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                        ddlThirdOrderBy.Items.Add(new ListItem(column.TableName + "." + column.FieldName));
                    }
                }
            }

            ddlFirstOrderBy.Items.Insert(0, new ListItem("None"));
            ddlSecondOrderBy.Items.Insert(0, new ListItem("None"));
            ddlThirdOrderBy.Items.Insert(0, new ListItem("None"));

            ///Select value from group by object
            if (orderBy != null && orderBy.Count > 0)
            {
                foreach (var orderby in orderBy)
                {
                    switch (orderby.Num)
                    {
                        case 1:
                            if (isUnion)
                            {
                                ddlFirstOrderBy.SelectedIndex = ddlFirstOrderBy.Items.IndexOf(
                                ddlFirstOrderBy.Items.FindByText(orderby.Column.FieldName));
                            }
                            else
                            {
                                ddlFirstOrderBy.SelectedIndex = ddlFirstOrderBy.Items.IndexOf(
                                ddlFirstOrderBy.Items.FindByText(orderby.Column.TableName + '.' + orderby.Column.FieldName));
                            }

                            if (orderby.orderBy == OrderBY.ASC)
                            {
                                rdFirstAscending.Checked = true;
                                rdFirstDescending.Checked = false;
                            }
                            else
                            {
                                rdFirstDescending.Checked = true;
                                rdFirstAscending.Checked = false;
                            }
                            break;
                        case 2:
                            if (isUnion)
                            {
                                ddlSecondOrderBy.SelectedIndex = ddlSecondOrderBy.Items.IndexOf(
                                ddlSecondOrderBy.Items.FindByText(orderby.Column.FieldName));
                            }
                            else
                            {
                                ddlSecondOrderBy.SelectedIndex = ddlSecondOrderBy.Items.IndexOf(
                                ddlSecondOrderBy.Items.FindByText(orderby.Column.TableName + '.' + orderby.Column.FieldName));
                            }

                            if (orderby.orderBy == OrderBY.ASC)
                            {
                                rdSecondAscending.Checked = true;
                                rdSecondDescending.Checked = false;
                            }
                            else
                            {
                                rdSecondDescending.Checked = true;
                                rdSecondAscending.Checked = false;
                            }
                            break;
                        case 3:
                            if (isUnion)
                            {
                                ddlThirdOrderBy.SelectedIndex = ddlThirdOrderBy.Items.IndexOf(
                                ddlThirdOrderBy.Items.FindByText(orderby.Column.FieldName));
                            }
                            else
                            {
                                ddlThirdOrderBy.SelectedIndex = ddlThirdOrderBy.Items.IndexOf(
                                ddlThirdOrderBy.Items.FindByText(orderby.Column.TableName + '.' + orderby.Column.FieldName));
                            }

                            if (orderby.orderBy == OrderBY.ASC)
                            {
                                rdThirdAscending.Checked = true;
                                rdThirdDescending.Checked = false;
                            }
                            else
                            {
                                rdThirdDescending.Checked = true;
                                rdThirdAscending.Checked = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void FillForm()
        {
            FillModule();
            LoadCategories();
            LoadQueryTableLists();

            templateName.Text = txtTitle.Text = uDashboard.Title;
            cmbOrder.Value = uDashboard.ItemOrder;
            hdnTitle.Value = uDashboard.Title;
            txtDescription.Text = uDashboard.DashboardDescription;
            ddlCategories.SelectedValue = (uDashboard.CategoryName == "") ? "none" : uDashboard.CategoryName;
            ddlSubCategories.SelectedValue = (uDashboard.SubCategory == "") ? "none" : uDashboard.SubCategory;
            chkIsFormattedPreview.Checked = panel.QueryInfo.IsPreviewFormatted;
            chkbxExpandGrouping.Checked = panel.QueryInfo.IsGroupByExpanded;

            // Authorized to View Field multiUser field fill
            if (!IsPostBack)
                ppeAuthorizeToView.SetValues(uDashboard.AuthorizedToView);

            if (uDashboard.DashboardModuleMultiLookup != null)
            {
                string[] Modules = UGITUtility.SplitString(uDashboard.DashboardModuleMultiLookup, Constants.Separator6);
                foreach (string spfieldModule in Modules)
                {
                    foreach (ListItem chkmodule in cbModules.Items)
                    {
                        if (spfieldModule == chkmodule.Value)
                        {
                            chkmodule.Selected = true;
                            break;
                        }
                    }
                }
            }
            lnkPublish.Text = !Convert.ToBoolean(uDashboard.IsActivated) ? "Publish" : "Hide";
            if (tables != null)
            {
                if (tables.Count > 0)
                {
                    DdlQueryTables.Visible = true;
                    DdlQueryTables.SelectedValue = tables[0].Name;
                }
                if (tables.Count > 1)
                {
                    pnlTable2.Visible = true;
                    DdlQueryTables2.SelectedValue = tables[1].Name;
                }
                if (tables.Count > 2)
                {
                    pnlTable3.Visible = true;
                    DdlQueryTables3.SelectedValue = tables[2].Name;
                }
                if (tables.Count > 3)
                {
                    pnlTable4.Visible = true;
                    DdlQueryTables4.SelectedValue = tables[3].Name;
                }
                if (tables.Count > 4)
                {
                    pnlTable5.Visible = true;
                    DdlQueryTables5.SelectedValue = tables[4].Name;
                }
            }

            BindColumnGridView();
            BindDrillDownColumnGridView();
            FillGroupByDropDown();
            FillOrderByDropDown();
            FillTotals();
            FillQuery();
            FillQueryFontFamily();
            FillQueryFormat();
            LoadQuery();
            OnTab_Change(FormState);
        }

        private void FillQuery()
        {
            string strWhere = string.Format("{0}={1}", DatabaseObjects.Columns.DashboardType, (int)DashboardType.Query);
            DataTable dashboardTable = objDashboardManager.GetDataTable(strWhere);

            if (dashboardTable != null && dashboardTable.Rows.Count > 0)
            {
                ddlqueries.DataTextField = "Title";
                ddlqueries.DataValueField = "ID";
                ddlqueries.DataSource = dashboardTable;
                ddlqueries.DataBind();
            }
        }

        private void FillQueryFontFamily()
        {
            string[] fontNames = { "Times New Roman", "Tahoma", "verdana", "Arial", "MS Sans Serif", "Courier", "Segoe UI", "Helvetica" };

            fontNames.ToList().ForEach(x => { ddlFontName.Items.Add(new ListItem(x)); });
            fontNames.ToList().ForEach(x => { ddlLabelFontName.Items.Add(new ListItem(x)); });
            fontNames.ToList().ForEach(x => { ddlTextFontName.Items.Add(new ListItem(x)); });
        }

        private void FillQueryFormat()
        {
            if (queryformat != null)
            {
                txtText.Text = queryformat.Text;
                ddlTextFontName.SelectedIndex = ddlTextFontName.Items.IndexOf(ddlTextFontName.Items.FindByText(queryformat.TextFontName));
                ddlFontStyle.SelectedIndex = ddlFontStyle.Items.IndexOf(ddlFontStyle.Items.FindByValue(queryformat.TextFontStyle.ToString()));
                ddlFontSize.SelectedIndex = ddlFontSize.Items.IndexOf(ddlFontSize.Items.FindByValue(queryformat.TextFontSize));
                ceFont.Color = queryformat.TextForeColor != null ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.TextForeColor)) : ColorTranslator.FromHtml("#000000");
                chkTextHide.Checked = queryformat.HideText;

                txtLabel.Text = queryformat.Label;
                ddlLabelFontName.SelectedIndex = ddlLabelFontName.Items.IndexOf(ddlLabelFontName.Items.FindByText(queryformat.LabelFontName));
                ddlLabelFontStyle.SelectedIndex = ddlLabelFontStyle.Items.IndexOf(ddlLabelFontStyle.Items.FindByValue(queryformat.LabelFontStyle.ToString()));
                ddlLabelFontSize.SelectedIndex = ddlLabelFontSize.Items.IndexOf(ddlLabelFontSize.Items.FindByValue(queryformat.LabelFontSize));
                ceLabelFont.Color = queryformat.LabelForeColor != null ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.LabelForeColor)) : ColorTranslator.FromHtml("#000000");
                chkLabelHide.Checked = queryformat.HideLabel;

                ddlFontName.SelectedIndex = ddlFontName.Items.IndexOf(ddlFontName.Items.FindByText(queryformat.ResultFontName));
                ddlNumFontStyle.SelectedIndex = ddlNumFontStyle.Items.IndexOf(ddlNumFontStyle.Items.FindByValue(queryformat.ResultFontStyle.ToString()));
                ddlNumFontSize.SelectedIndex = ddlNumFontSize.Items.IndexOf(ddlNumFontSize.Items.FindByValue(queryformat.ResultFontSize));
                ceNumFont.Color = queryformat.ResultForeColor != null ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.ResultForeColor)) : ColorTranslator.FromHtml("#000000");

                ddlTitlePosition.SelectedIndex = ddlTitlePosition.Items.IndexOf(ddlTitlePosition.Items.FindByValue(Convert.ToInt32(queryformat.TitlePosition).ToString()));
                ddlQueryType.SelectedIndex = ddlQueryType.Items.IndexOf(ddlQueryType.Items.FindByValue(Convert.ToInt32(queryformat.FormatType).ToString()));
                txtBackgroundimage.Text = queryformat.BackgroundImage;
                txtWidth.Text = Convert.ToString(queryformat.SizeOfFrame.Width);
                txtHeight.Text = Convert.ToString(queryformat.SizeOfFrame.Height);
                txtLocationLeft.Text = Convert.ToString(queryformat.Location.X);
                txtLocationTop.Text = Convert.ToString(queryformat.Location.Y);
                ddlResultPnlDesign.SelectedIndex = ddlResultPnlDesign.Items.IndexOf(ddlResultPnlDesign.Items.FindByValue(Convert.ToInt32(queryformat.ResultPanelDesign).ToString()));
                txtIconImage.Text = queryformat.IconImage;

                txticonPositionLeft.Text = Convert.ToString(queryformat.IconLocation.X);
                txticonPositionTop.Text = Convert.ToString(queryformat.IconLocation.Y);
                txticonHeight.Text = Convert.ToString(queryformat.IconSize.Height);
                txticonWidth.Text = Convert.ToString(queryformat.IconSize.Width);

                ceBGColor.Color = uHelper.TranslateColorCode(queryformat.BackgroundColor, ColorTranslator.FromHtml("#FFFFFF"));// !string.IsNullOrEmpty(queryformat.BackgroundColor) ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.BackgroundColor)) : ColorTranslator.FromHtml("#FFFFFF");
                ceBorderColor.Color = !string.IsNullOrEmpty(queryformat.BorderColor) ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.BorderColor)) : ColorTranslator.FromHtml("#FFFFFF");
                txtBorderWidth.Text = Convert.ToString(queryformat.BorderWidth);
                ceHeaderColor.Color = !string.IsNullOrEmpty(queryformat.HeaderColor) ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.HeaderColor)) : ColorTranslator.FromHtml("#FFFFFF");
                ceRowColor.Color = !string.IsNullOrEmpty(queryformat.RowColor) ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.RowColor)) : ColorTranslator.FromHtml("#FFFFFF");
                ceRowAlterColor.Color = !string.IsNullOrEmpty(queryformat.RowAlternateColor) ? ColorTranslator.FromHtml(string.Format("#{0}", queryformat.RowAlternateColor)) : ColorTranslator.FromHtml("#FFFFFF");
                ddlDrillDown.SelectedIndex = ddlDrillDown.Items.IndexOf(ddlDrillDown.Items.FindByText(queryformat.DrillDownType));
                drilldowntype.Value = ddlDrillDown.SelectedValue;
                ddlNavigationType.SelectedIndex = ddlNavigationType.Items.IndexOf(ddlNavigationType.Items.FindByText(queryformat.NavigateType));
                txtCustomUrl.Text = queryformat.CustomUrl;

                chkShowCompanyLogo.Checked = queryformat.ShowCompanyLogo;
                //chkHideActions.Checked = queryformat.HideActions;
                //chkHideFilterbar.Checked = queryformat.HideFilterbar;
                //ddlPagerPosition.SelectedIndex = ddlPagerPosition.Items.IndexOf(ddlPagerPosition.Items.FindByText(queryformat.PagePosition));
                //chkHideGH.Checked = queryformat.HideGroupingHeader;
                //chkHidePager.Checked = queryformat.HidePager;
                //if (queryformat.PageSize == 0)
                //    queryformat.PageSize = 20;
                //txtPageSize.Value = queryformat.PageSize;
                txtFooter.Text = queryformat.Footer;
                txtAdditionalInfo.Text = queryformat.AdditionalInfo;
                chkShowDateInFooter.Checked = queryformat.ShowDateInFooter;
                txtHeader.Text = string.IsNullOrEmpty(queryformat.Header) ? txtTitle.Text : queryformat.Header;
                txtAdditionalFooterInfo.Text = queryformat.AdditionalFooterInfo;
                chkEnableEditUrl.Checked = queryformat.EnableEditUrl;

                if (queryformat.QueryId != null)
                    ddlqueries.SelectedIndex = ddlqueries.Items.IndexOf(ddlqueries.Items.FindByValue(queryformat.QueryId));
            }
            else
            {
                txtHeader.Text = string.IsNullOrEmpty(txtHeader.Text) ? txtTitle.Text : txtHeader.Text;
                if (!IsPostBack)
                {
                    txtAdditionalFooterInfo.Text = string.IsNullOrEmpty(txtAdditionalFooterInfo.Text) ? DateTime.Now.ToString("MMM-dd-yyyy") : txtAdditionalFooterInfo.Text;
                }
            }
        }

        private void FillTotals()
        {
            selectedColumn = new List<Utility.ColumnInfo>();
            int Id = 0;

            if (tables == null)
                return;

            foreach (TableInfo table in tables.OrderBy(x => x.Name))
            {
                List<Utility.ColumnInfo> lstcolumns = table.Columns.OrderByDescending(c => c.Selected)
                          .ThenBy(c => c.TableName)
                          .ThenBy(c => c.FieldName)
                          .ThenBy(c => c.Sequence)
                          .ToList();
                foreach (Utility.ColumnInfo column in lstcolumns)
                {
                    Id++;
                    bool selected = selectedtotals.Exists(c => c.TableName == column.TableName && c.FieldName == column.FieldName);
                    string function = "none";
                    if (selected)
                    {
                        function = selectedtotals.Find(c => c.TableName == column.TableName && c.FieldName == column.FieldName).Function;
                    }
                    selectedColumn.Add(new Utility.ColumnInfo
                    {
                        ID = Id,
                        DataType = column.DataType,
                        DisplayName = column.DisplayName,
                        FieldName = column.FieldName,
                        Function = function == "" ? "none" : function,
                        Selected = selected,
                        Sequence = column.Sequence,
                        TableName = column.TableName,
                        Hidden = column.Hidden
                    });
                }
            }

            BindTotals();
        }

        private void BindTotals()
        {
            var totalDataSource = selectedColumn.FindAll(c => !c.Hidden);

            //Filtering totalDataSource to get distinct columns only in case of Union
            if (isUnion)
                totalDataSource = totalDataSource.GroupBy(x => new { field = x.FieldName }).Select(z => z.FirstOrDefault()).ToList();

            spgrid_totals.DataSource = totalDataSource;
            spgrid_totals.DataBind();
        }

        private void AddColumnsInTotals()
        {
            string selectedColumns = string.Empty;
            for (int i = 0; i < spgrid_totals.VisibleRowCount; i++)
            {
                if (spgrid_totals.IsGroupRow(i))
                    continue;

                CheckBox chkSelect = spgrid_totals.FindRowCellTemplateControl(i, spgrid_totals.Columns["Column Name"] as GridViewDataColumn, "chkSelect") as CheckBox;
                string tableName = (spgrid_totals.FindRowCellTemplateControl(i, spgrid_totals.Columns["Column Name"] as GridViewDataColumn, "hTableName") as HiddenField).Value;
                string fieldName = chkSelect.Text;

                if (chkSelect.Checked)
                {
                    string aggregationfunc = (spgrid_totals.FindRowCellTemplateControl(i, spgrid_totals.Columns["Functions"] as GridViewDataColumn, "ddlFunctions") as DropDownList).SelectedValue;


                    if (selectedtotals.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        selectedtotals.Where(c => c.FieldName == fieldName && c.TableName == tableName).ToList()
                         .ForEach(ci =>
                         {
                             ci.Selected = true;
                             ci.Function = aggregationfunc;
                         });
                    }
                    else
                    {
                        selectedtotals.Add(selectedColumn.Where(c => c.FieldName == fieldName && c.TableName == tableName).SingleOrDefault());
                    }

                    selectedColumn.Where(c => c.FieldName == fieldName && c.TableName == tableName).ToList()
                                .ForEach(ci =>
                                {
                                    ci.Selected = true;
                                    ci.Function = aggregationfunc;
                                });

                }
                else
                {
                    if (selectedtotals.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        selectedtotals.Remove(selectedtotals.Find(c => c.FieldName == fieldName && c.TableName == tableName));
                    }
                }
            }
            panel.QueryInfo.Totals = selectedtotals;
        }

        private List<Utility.ColumnInfo> SelectColumns()
        {
            List<FactTableField> columnData = new List<FactTableField>();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            int sque = 0;
            int Id = 0;
            columns = new List<Utility.ColumnInfo>();
            if (tables == null) return columns;

            foreach (TableInfo table in tables)
            {
                columnData = GetColsListWithDataType(table.Name);
                foreach (Utility.ColumnInfo cInfo in table.Columns)
                {
                    if (cInfo.IsExpression)
                    {
                        if (!columnData.Any(x => x.FieldName == cInfo.FieldName))
                            columnData.Add(new FactTableField(cInfo.TableName, cInfo.FieldName, cInfo.DataType, cInfo.DisplayName));
                    }
                }
                if (columnData != null)
                {
                    columnData = columnData.OrderBy(cd => cd.FieldName).ToList();

                    if (table.Name == DatabaseObjects.Tables.PMMProjects)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.PMMProjects, "ProjectHealth", "none", "ProjectHealth"));
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.PMMProjects, "GanttView", "none", "GanttView"));
                    }
                    else if (table.Name == DatabaseObjects.Tables.NPRRequest)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.NPRRequest, "GanttView", "none", "GanttView"));
                    }
                    else if (table.Name == DatabaseObjects.Tables.TSKProjects)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.TSKProjects, "GanttView", "none", "GanttView"));
                    }

                    foreach (FactTableField item in columnData)
                    {
                        bool selected = table.Columns.Exists(c => c.FieldName == item.FieldName);

                        if (selected)
                        {
                            var column = table.Columns.Find(c => c.FieldName == item.FieldName);
                            column.ID = ++Id;
                            columns.Add(column);
                            continue;
                        }
                        columns.Add(new Utility.ColumnInfo
                        {
                            ID = ++Id,
                            FieldName = item.FieldName,
                            DisplayName = item.FieldDisplayName,
                            DataType = QueryHelperManager.GetStandardDataType(item.DataType),
                            Function = "none",
                            TableName = table.Name,
                            Selected = selected
                        });
                    }
                }
            }

            //changing order of 'TableName' & Sequence to solve grouping issue on columns tab.
            List<Utility.ColumnInfo> selectedColns = columns.Where(x => x.Selected).OrderBy(x => x.TableName).ThenBy(x => x.Sequence).ToList();
            List<Utility.ColumnInfo> nonSelectedColns = columns.Where(x => !x.Selected).OrderBy(x => x.TableName).ThenBy(x => x.FieldName).ToList();

            List<Utility.ColumnInfo> selectedColmns = selectedColns.OrderBy(x => x.Sequence).Distinct().ToList();
            foreach (var sci in selectedColmns)
            {
                sci.Sequence = ++sque;
            }

            List<Utility.ColumnInfo> unSelectedColmns = nonSelectedColns.OrderBy(x => x.Sequence).ToList();
            foreach (var ci in unSelectedColmns)
            {
                ci.Sequence = ++sque;
            }

            columns = new List<Utility.ColumnInfo>();
            columns = selectedColns;
            columns.AddRange(nonSelectedColns);

            unionColumns = new List<Utility.ColumnInfo>();
            List<string> columnsList = new List<string>();
            foreach (Utility.ColumnInfo col in columns)
            {
                if (columnsList.Count > 0 && isUnion)
                {
                    bool isColSelected = columnsList.Any(x => x.Contains(col.FieldName));

                    if (isColSelected)
                        continue;
                }
                unionColumns.Add(col);
                columnsList.Add(col.FieldName);
            }

            return columns.Distinct().ToList();
        }

        private List<Utility.ColumnInfo> SelectDrillDownColumns()
        {
            List<FactTableField> columnData = new List<FactTableField>();
            int sque = 0;
            int Id = 0;
            ddcolumns = new List<Utility.ColumnInfo>();
            if (ddtables == null) return ddcolumns;

            foreach (TableInfo table in ddtables)
            {
                columnData = GetColsListWithDataType(table.Name);

                foreach (Utility.ColumnInfo cInfo in table.Columns)
                {
                    if (cInfo.IsExpression)
                    {
                        columnData.Add(new FactTableField(cInfo.TableName, cInfo.FieldName, cInfo.DataType, cInfo.DisplayName));
                    }
                }
                if (columnData != null)
                {
                    columnData = columnData.OrderBy(cd => cd.FieldName).ToList();
                    if (table.Name == DatabaseObjects.Tables.PMMProjects)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.PMMProjects, "ProjectHealth", "none", "ProjectHealth"));
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.PMMProjects, "GanttView", "none", "GanttView"));
                    }
                    else if (table.Name == DatabaseObjects.Tables.NPRRequest)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.NPRRequest, "GanttView", "none", "GanttView"));
                    }
                    else if (table.Name == DatabaseObjects.Tables.TSKProjects)
                    {
                        columnData.Add(new FactTableField(DatabaseObjects.Tables.TSKProjects, "GanttView", "none", "GanttView"));
                    }

                    foreach (FactTableField item in columnData)
                    {
                        bool selected = table.Columns.Exists(c => c.FieldName == item.FieldName && c.IsDrillDown);

                        if (selected)
                        {
                            var column = table.Columns.Find(c => c.FieldName == item.FieldName);
                            column.ID = ++Id;
                            ddcolumns.Add(column);
                            continue;
                        }
                        ddcolumns.Add(new Utility.ColumnInfo
                        {
                            ID = ++Id,
                            FieldName = item.FieldName,
                            DisplayName = item.FieldDisplayName,
                            DataType = item.DataType,
                            Function = "none",
                            TableName = table.Name,
                            Selected = selected
                        });
                    }
                }
            }

            //changing order of 'TableName' & Sequence to solve grouping issue on columns tab.
            ddcolumns = ddcolumns.OrderByDescending(c => c.Selected)
                             .ThenBy(c => c.TableName)
                             .ThenBy(c => c.Sequence)
                             .ToList();


            List<Utility.ColumnInfo> selectedColumns = ddcolumns.Where(x => x.Selected).OrderBy(x => x.Sequence).ToList();
            foreach (var sci in selectedColumns)
            {
                sci.Sequence = ++sque;
            }

            List<Utility.ColumnInfo> unSelectedColmns = ddcolumns.Where(x => !x.Selected).OrderBy(x => x.Sequence).ToList();
            foreach (var ci in unSelectedColmns)
            {
                ci.Sequence = ++sque;
            }

            return ddcolumns;
        }

        private void FillModule()
        {
            var modules = ModuleViewManager.Load(x => x.EnableModule).Select(x => new { x.ID, x.ModuleName }).OrderBy(x => x.ModuleName).ToList();
            cbModules.DataTextField = DatabaseObjects.Columns.ModuleName;
            cbModules.DataValueField = DatabaseObjects.Columns.Id;
            cbModules.DataSource = modules;
            cbModules.DataBind();
        }

        private void LoadQueryTableLists()
        {
            ListItemCollection listItems = GetListsFromWeb();
            DdlQueryTables2.DataSource = listItems;
            DdlQueryTables2.DataBind();

            DdlQueryTables3.DataSource = listItems;
            DdlQueryTables3.DataBind();

            DdlQueryTables4.DataSource = listItems;
            DdlQueryTables4.DataBind();

            DdlQueryTables5.DataSource = listItems;
            DdlQueryTables5.DataBind();

            listItems.Remove(new ListItem("--Select--", "-1"));

            DdlQueryTables.DataSource = listItems;
            DdlQueryTables.DataBind();

            if (!string.IsNullOrEmpty(queryTableName))
                DdlQueryTables.SelectedIndex = DdlQueryTables.Items.IndexOf(DdlQueryTables.Items.FindByValue(queryTableName));
            else if (!string.IsNullOrEmpty(factTable))
                DdlQueryTables.SelectedIndex = DdlQueryTables.Items.IndexOf(DdlQueryTables.Items.FindByText(string.Format("{0}{1}", HttpUtility.HtmlDecode(indentation), factTable)));

            if (queryTableName.Equals("-1"))
                queryTableName = DdlQueryTables.Items[0].Value;
            else if (queryTableName.Equals(String.Empty))
                queryTableName = factTable;
        }

        private ListItemCollection GetListsFromWeb()
        {
            List<string> lists = HttpContext.Current.TableList();
            ListItemCollection listItems = new ListItemCollection();
            List<string> factTables = DashboardCache.DashboardFactTables(context);

            factTables.Remove(uDashboard.Title);

            if (factTables.Count > 0)
            {
                // Add list item for Group label - Fact Table
                ListItem liFactTable = new ListItem("Fact Tables", "FactTables", false);
                listItems.Add(liFactTable);

                factTables = factTables.OrderBy(x => x).ToList();
                foreach (string factTable in factTables)
                {
                    listItems.Add(new ListItem(string.Format("{0}{1}", HttpUtility.HtmlDecode(indentation), factTable), factTable));
                }
            }

            // Add list item for Group label - Table
            ListItem liNonFactTable = new ListItem("Tables", "Tables", false);
            listItems.Add(liNonFactTable);

            foreach (string li in lists)
            {
                listItems.Add(new ListItem(string.Format("{0}{1}", HttpUtility.HtmlDecode(indentation), li), li));
            }

            listItems.Insert(0, new ListItem("--Select--", "-1"));
            return listItems;
        }

        private List<ListItem> DistinctList(List<ListItem> source)
        {
            List<ListItem> uniques = new List<ListItem>();
            foreach (ListItem item in source)
            {
                if (!uniques.Contains(item)) uniques.Add(item);
            }
            return uniques;
        }

        private void LoadCategories()
        {
            ddlCategories.Items.Clear();
            ddlSubCategories.Items.Clear();
            List<string> categoryList = new List<string>();
            List<string> subCategoryList = new List<string>();
            DataTable dashboardTable = null;

            string query = string.Format("{0}={1} and {2}='{3}'", DatabaseObjects.Columns.DashboardType, (int)DashboardType.Query,DatabaseObjects.Columns.TenantID, context.TenantID);
            DataTable dtDashbaordPanels = objDashboardManager.GetDataTable(query);
            dashboardTable = dtDashbaordPanels != null && dtDashbaordPanels.Rows.Count > 0 ? dtDashbaordPanels : null;

            if (dashboardTable != null)
            {
                foreach (DataRow row in dashboardTable.Rows)
                {
                    if (dashboardTable.Columns.Contains(DatabaseObjects.Columns.CategoryName))
                    {
                        categoryList.Add(Convert.ToString(row[DatabaseObjects.Columns.CategoryName]));
                        subCategoryList.Add(Convert.ToString(row[DatabaseObjects.Columns.SubCategory]));
                    }
                }

                string[] categories = categoryList.Distinct().ToArray();
                foreach (string str in categories)
                {
                    if (!string.IsNullOrEmpty(str))
                        ddlCategories.Items.Add(new ListItem(str, str));
                }

                string[] subCategories = subCategoryList.Distinct().ToArray();
                foreach (string str in subCategories)
                {
                    if (!string.IsNullOrEmpty(str))
                        ddlSubCategories.Items.Add(new ListItem(str, str));
                }
            }
            else
            {
                ddlCategories.Items.Clear();
                ddlSubCategories.Items.Clear();
            }

            if (!ddlCategories.Items.Contains(new ListItem("none")))
                ddlCategories.Items.Insert(0, new ListItem("none"));

            if (!ddlSubCategories.Items.Contains(new ListItem("none")))
                ddlSubCategories.Items.Insert(0, new ListItem("none"));
        }

        private ListItemCollection FillQueryTableFields(string queryTable)
        {
            return FillQueryTableFields(queryTable, string.Empty, false);
        }

        /// <summary>
        /// fills the  query table drop downs with column names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="queryTable"></param>
        /// <param name="typeFilter"></param>
        private ListItemCollection FillQueryTableFields(string queryTable, string typeFilter, bool includeLookupIDColumns)
        {
            ListItemCollection listItemColl = new ListItemCollection();
            string value = "";
            string text = "";
            DataTable inputList = null;
            List<FactTableField> queryTableFields = new List<FactTableField>();
            ModuleViewManager moduleManager = new ModuleViewManager(context);
            string moduleTable = moduleManager.GetModuleByTableName(queryTable);
            if (queryTable != null && queryTable.Trim() != string.Empty && !queryTable.Equals("-1"))
            {
                queryTableFields = DashboardCache.GetFactTableFields(context, queryTable);

                if (queryTableFields == null)
                    queryTableFields = new List<FactTableField>();

                inputList = GetTableDataManager.GetTableData(queryTable);

                if (queryTableFields != null && queryTableFields.Count > 0)
                {
                    foreach (FactTableField fld in queryTableFields)
                    {
                        fld.FieldDisplayName = UGITUtility.AddSpaceBeforeWord(fld.FieldName);
                        fld.DataType = fld.DataType.Replace("System.", "");
                    }
                    queryTableFields.RemoveAll(x => x.FieldName.EndsWith("User$Id"));
                }
                else if (inputList != null)
                {
                    DataColumnCollection fieldCollection = inputList.Columns;

                    foreach (DataColumn field in fieldCollection)
                    {
                        //if (essentialFields.Contains(field.ColumnName))

                        if ((!field.ReadOnly && Convert.ToString(field.DataType) != "Attachments") ||
                            Convert.ToString(field.DataType) == "Lookup" || essentialFields.Contains(field.ColumnName))
                        {
                            if (queryTableFields != null)
                            {
                                if (includeLookupIDColumns && Convert.ToString(field.DataType) == "Lookup")
                                {
                                    queryTableFields.Add(new FactTableField(queryTable, field.ColumnName, QueryHelperManager.GetStandardDataType("Lookup")));
                                    queryTableFields.Add(new FactTableField(queryTable, field.ColumnName + "_RefID", QueryHelperManager.GetStandardDataType("Counter")));
                                }
                                else
                                {
                                    queryTableFields.Add(new FactTableField(queryTable, field.ColumnName, QueryHelperManager.GetStandardDataType(field.DataType.ToString())));
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(typeFilter))
                {
                    queryTableFields = queryTableFields != null ? queryTableFields.Where(x => x.DataType.ToLower() == "system.datetime").ToList() : null;
                }

                if (queryTableFields != null)
                {
                    queryTableFields = queryTableFields.AsEnumerable().OrderBy(x => x.FieldName).ToList();
                    foreach (FactTableField field in queryTableFields)
                    {
                        text = string.Format("{0}{1}({2})", queryTable + ".", field.FieldName, field.DataType.Replace("System.", string.Empty));
                        value = string.Format("{0}{1}", queryTable + ".", field.FieldName);
                        ListItem item = new ListItem(text, value);
                        item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                        listItemColl.Add(item);

                    }
                }
            }
            return listItemColl;
        }

        private string GetQueryHTML(QueryClause clause)
        {
            return GetQueryHTML(null, clause);
        }

        private string GetQueryHTML(object value, QueryClause clause)
        {
            StringBuilder sbQueryString = new StringBuilder();
            switch (clause)
            {
                case QueryClause.Select:
                    if (columns != null && columns.Count > 0)
                    {
                        columns = columns.OrderBy(c => c.Sequence).ToList();
                        sbQueryString.Append("<div><b>SELECT</b></div><br>");

                        List<string> selectedColumnsList = new List<string>();

                        for (int i = 0; i < columns.Count; i++)
                        {
                            Utility.ColumnInfo column = columns[i];
                            if (column.Selected && !column.Hidden)
                            {
                                if (selectedColumnsList.Count > 0 && isUnion)
                                {
                                    bool isColSelected = selectedColumnsList.Any(x => x.Contains(column.FieldName));

                                    if (isColSelected)
                                        continue;
                                }

                                if (column.Function != "none" && column.Function != "")
                                {
                                    if (column.IsExpression)
                                    {
                                        sbQueryString.Append("<span class='data'>" + string.Format("{0}({1})", column.Function, column.Expression) + "</span>");
                                    }
                                    else
                                    {
                                        if (isUnion)
                                            sbQueryString.Append("<span class='data'>" + string.Format("{0}({1})", column.Function, column.FieldName) + "</span>");
                                        else
                                            sbQueryString.Append("<span class='data'>" + string.Format("{0}({1}.{2})", column.Function, column.TableName, column.FieldName) + "</span>");
                                    }
                                }
                                else
                                {
                                    if (column.IsExpression)
                                    {
                                        sbQueryString.Append("<span class='data'>" + string.Format("{0}", column.Expression) + "</span>");
                                    }
                                    else
                                    {
                                        if (isUnion)
                                            sbQueryString.Append("<span class='data'>" + string.Format("{0}", column.FieldName) + "</span>");
                                        else
                                            sbQueryString.Append("<span class='data'>" + string.Format("{0}.{1}", column.TableName, column.FieldName) + "</span>");
                                    }
                                }

                                if (i < columns.Count)
                                {
                                    sbQueryString.Append(",<br>");
                                    selectedColumnsList.Add(column.FieldName);
                                }
                                else
                                    sbQueryString.Append("<br>");
                            }
                        }
                    }
                    break;
                case QueryClause.From:

                    if (value != null && Convert.ToString(value) != string.Empty)
                    {
                        sbQueryString.Append("<div><b>FROM</b></div><br>");
                        sbQueryString.Append("<span class='data'>" + Convert.ToString(value) + "</span>");
                    }
                    break;
                case QueryClause.Where:
                    if (whereClauses != null && whereClauses.Count > 0)
                    {
                        sbQueryString.Append("<div><b>WHERE</b></div><br>");
                        sbQueryString.Append("<div style='padding-left:10px;'>");
                        List<WhereInfo> rootWhere = whereClauses.Where(x => x.ParentID == 0).ToList();
                        foreach (WhereInfo rWhere in rootWhere)
                        {
                            if (sbQueryString.ToString() != string.Empty && rWhere.RelationOpt != RelationalOperator.None)
                            {
                                sbQueryString.AppendFormat("<br><div><b>{0}</b></div><br>", rWhere.RelationOpt.ToString());
                            }

                            List<WhereInfo> subWhere = new List<WhereInfo>();
                            WhereInfo rWhereCopy = rWhere.Clone() as WhereInfo;
                            rWhereCopy.RelationOpt = RelationalOperator.None;
                            subWhere.Add(rWhereCopy);
                            subWhere.AddRange(whereClauses.Where(x => x.ParentID == rWhere.ID));


                            List<string> expList = new List<string>();
                            for (int i = 0; i < subWhere.Count; i++)
                            {
                                StringBuilder subQuery = new StringBuilder();
                                WhereInfo where = subWhere[i];
                                if (where.Operator != OperatorType.None && where.Valuetype != qValueType.None)
                                {
                                    if (expList.Count == 0)
                                        subQuery.Append("<span>");
                                    else
                                        subQuery.Append("<span class='data'>&nbsp;");

                                    if (expList.Count > 0 && where.RelationOpt != RelationalOperator.None)
                                        subQuery.AppendFormat("<b>&nbsp;{0}&nbsp;</b>", where.RelationOpt.ToString());

                                    subQuery.Append(where.ColumnName);
                                    subQuery.AppendFormat("&nbsp;{0}&nbsp;", QueryHelperManager.GetOperatorDisplayFormatFromType(where.Operator));
                                    if (where.Valuetype == qValueType.Constant)
                                        subQuery.AppendFormat("'{0}'", where.Value);
                                    else if (where.Valuetype == qValueType.Variable)
                                        subQuery.AppendFormat("'{0}'", where.Value);
                                    else
                                        subQuery.AppendFormat("[{0}]", where.ParameterName);
                                    subQuery.Append("</span>");
                                    expList.Add(subQuery.ToString());
                                }
                            }

                            if (expList.Count == 1)
                                sbQueryString.AppendFormat("<span class='data'>&nbsp;&nbsp;{0}</span>", string.Join("", expList));
                            else if (expList.Count > 1)
                                sbQueryString.AppendFormat("<span class='data'>&nbsp;&nbsp;({0})</span>", string.Join("<br>", expList));
                        }
                        sbQueryString.Append("</div>");

                    }
                    break;
                case QueryClause.GroupBy:

                    if (groupBy != null && groupBy.Count > 0)
                    {
                        sbQueryString.Append("<div><b>GROUP BY</b></div><br>");
                        for (int i = 0; i < groupBy.Count; i++)
                        {
                            var item = groupBy[i];

                            if (isUnion)
                                sbQueryString.Append("<span class='data'>" + item.Column.FieldName + "</span>");
                            else
                                sbQueryString.Append("<span class='data'>" + item.Column.TableName + "." + item.Column.FieldName + "</span>");

                            if ((groupBy.Count - 1) > i)
                                sbQueryString.Append(",<br>");
                            else
                                sbQueryString.Append("<br>");
                        }
                    }
                    break;
                case QueryClause.OrderBy:
                    if (orderBy != null && orderBy.Count > 0)
                    {
                        sbQueryString.Append("<div><b>ORDER BY</b></div><br>");
                        for (int i = 0; i < orderBy.Count; i++)
                        {
                            var item = orderBy[i];

                            if (isUnion)
                                sbQueryString.Append("<span class='data'>" + item.Column.FieldName +
                                "</span> <b>" + Convert.ToString(item.orderBy) + "</b>");
                            else
                                sbQueryString.Append("<span class='data'>" + item.Column.TableName + "." + item.Column.FieldName +
                                "</span> <b>" + Convert.ToString(item.orderBy) + "</b>");

                            if ((orderBy.Count - 1) > i)
                            {
                                sbQueryString.Append(",<br>");
                            }
                        }
                    }
                    break;
                case QueryClause.Totals:
                    if (selectedtotals != null && selectedtotals.Count > 0)
                    {
                        sbQueryString.Append("<div><b>TOTALS ON</b></div><br>");
                        for (int i = 0; i < selectedtotals.Count; i++)
                        {
                            var item = selectedtotals[i];

                            if (isUnion)
                                sbQueryString.Append("<span class='data'>" + item.FieldName + "</span>");
                            else
                                sbQueryString.Append("<span class='data'>" + item.TableName + "." + item.FieldName + "</span>");

                            if ((selectedtotals.Count - 1) > i)
                            {
                                sbQueryString.Append(",<br>");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return sbQueryString.ToString();
        }
        public void ColumnsToSave()
        {
            listColumnsTosSave = new List<Utility.ColumnInfo>();
            if (columns != null && columns.Count > 0)
            {
                columns = columns.OrderBy(c => c.Sequence).ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    Utility.ColumnInfo column = columns[i];
                    if (column.Selected && !column.Hidden)
                    {
                        if (i < columns.Count)
                        {
                            listColumnsTosSave.Add(column);
                        }
                    }
                }
            }
        }

        private string GetFromClause(List<Joins> joins, bool isHtml)
        {
            StringBuilder query = new StringBuilder();

            if ((joins == null || joins.Count == 0) && (tables != null && tables.Count == 1))
            {
                query.Append(tables[0].Name);
            }
            else if ((joins == null || joins.Count == 0) && (tables != null && tables.Count > 1))
            {
                query.Append(tables[0].Name);
            }
            else
            {
                if (joins.Count > 0)
                {
                    query.Append(joins[0].FirstTable + " ");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;" : Environment.NewLine + "  ") + joins[0].JoinType.ToString() + " ");
                    query.Append(joins[0].SecondTable + " ON ");
                    query.Append(joins[0].FirstColumn + " ");
                    query.Append(joins[0].OperatorType + " ");
                    query.Append(joins[0].SecondColumn + " ");
                }
                if (joins.Count > 1)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "    ") + joins[1].JoinType.ToString().Trim() + " ");
                    query.Append(((query.ToString().Contains(joins[1].FirstTable)) ? joins[1].SecondTable : joins[1].FirstTable).Trim() + " ON ");
                    query.Append(joins[1].FirstColumn + " ");
                    query.Append(joins[1].OperatorType + " ");
                    query.Append(joins[1].SecondColumn + " ");
                }

                if (joins.Count > 2)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "        ") + joins[2].JoinType.ToString() + " ");
                    query.Append(((query.ToString().Contains(joins[2].FirstTable)) ? joins[2].SecondTable : joins[2].FirstTable) + " ON ");
                    query.Append(joins[2].FirstColumn + " ");
                    query.Append(joins[2].OperatorType + " ");
                    query.Append(joins[2].SecondColumn + " ");
                }
                if (joins.Count > 3)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;;&nbsp;&nbsp;" : Environment.NewLine + "        ") + joins[3].JoinType.ToString() + " ");
                    query.Append(((query.ToString().Contains(joins[3].FirstTable)) ? joins[3].SecondTable : joins[3].FirstTable) + " ON ");
                    query.Append(joins[3].FirstColumn + " ");
                    query.Append(joins[3].OperatorType + " ");
                    query.Append(joins[3].SecondColumn + " ");
                }
            }
            return query.ToString();
        }

        public void SaveQueryData()
        {
            if (whereClauses != null && whereClauses.Count > 0)
                whereClauses = whereClauses.OrderBy(x => x.ParentID == 0 ? x.ID : x.ParentID).ThenBy(x => x.ID).ToList();

            panel.QueryInfo.WhereClauses = whereClauses;

           PrepareSaveData();
            if (tables != null)
            {
                foreach (var table in tables)
                {
                    DataTable list = !string.IsNullOrEmpty(table.Name) ? DashboardCache.GetCachedDashboardData(context, table.Name) : null;

                    if (table.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.ID))
                        table.Columns.RemoveAll(x => x.FieldName == DatabaseObjects.Columns.ID && !x.Selected && (x.Alignment != null || x.Function != null));

                    if (list != null && list.Rows.Count > 0 && list.Columns[DatabaseObjects.Columns.ID].DataType.Name.EqualsIgnoreCase("String"))
                    {
                        string dataType = string.Empty;
                        if (list.Columns[DatabaseObjects.Columns.ID].DataType.Name.EqualsIgnoreCase("String"))
                            dataType = "String";
                        else
                            dataType = "Integer";
                        var column = new Utility.ColumnInfo { Hidden = true, FieldName = DatabaseObjects.Columns.ID, TableName = table.Name, DisplayName = DatabaseObjects.Columns.ID, DataType = dataType };
                        table.Columns.Add(column);
                    }
                    if (panel.QueryInfo.QueryFormats != null && panel.QueryInfo.QueryFormats.EnableEditUrl && !table.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.VendorMSALookup)
                         && list != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.VendorMSALookup, list))
                    {
                        var column = new Utility.ColumnInfo { Hidden = true, FieldName = DatabaseObjects.Columns.VendorMSALookup, TableName = table.Name, DisplayName = DatabaseObjects.Columns.VendorMSALookup, DataType = "String" };
                        table.Columns.Add(column);
                    }
                    if (!table.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.TicketId) && list != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.TicketId, list))
                    {
                        var column = new Utility.ColumnInfo { Hidden = true, FieldName = DatabaseObjects.Columns.TicketId, TableName = table.Name, DisplayName = DatabaseObjects.Columns.TicketId, DataType = "String" };
                        table.Columns.Add(column);
                    }
                }
            }

            panel.QueryInfo.Tables = tables;
            panel.QueryInfo.JoinList = joins;
            panel.QueryInfo.GroupBy = groupBy;
            panel.QueryInfo.IsGroupByExpanded = chkbxExpandGrouping.Checked;
            panel.QueryInfo.OrderBy = orderBy;
            panel.QueryInfo.Totals = selectedtotals;
            panel.QueryInfo.IsPreviewFormatted = chkIsFormattedPreview.Checked;
            panel.QueryInfo.DrillDownTables = ddtables;
            panel.QueryInfo.QueryFormats = queryformat;
            panel.QueryInfo.MissingColumns = missingColumns;

            // Updating panel into udashboard object
            uDashboard.panel = panel;

            byte[] iconContents = new byte[0];
            string fileName = string.Empty;

            // Save/Update dashboard
            objDashboardManager.SaveDashboardPanel(iconContents, fileName, true, uDashboard);
        }

        private void PrepareSaveData()
        {
            if (tables != null)
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    tables[i].Columns = new List<Utility.ColumnInfo>();
                }

                ColumnsToSave();

                for (int i = 0; i < tables.Count; i++)
                {
                    tables[i].Columns = listColumnsTosSave.Where(C => C.TableName == tables[i].Name).ToList();
                    //for (int j = 0; j < listColumnsTosSave.Count; j++)
                    //{
                    //    if (tables[i].Name == listColumnsTosSave[j].TableName)
                    //    {
                    //        tables[i].Columns.Add(listColumnsTosSave[j]);
                    //    }
                    //}
                }
            }
        }

        private List<string> GetSelectedModules()
        {
            List<string> collection = new List<string>();
            foreach (ListItem item in cbModules.Items)
            {
                if (item.Selected)
                {
                    collection.Add(item.Value);
                }
            }
            return collection;
        }

        private void BindColumnGridView()
        {
            var columnsData = SelectColumns();

            //Filtering columns to get distinct columns only in case of Union
            if (isUnion)
                columnsData = columnsData.GroupBy(x => new { field = x.FieldName }).Select(z => z.FirstOrDefault()).ToList();

            grdColumns.DataSource = columnsData;
            grdColumns.DataBind();
            ManageGrouping(grdColumns);

        }

        private void BindDrillDownColumnGridView()
        {
            gvDrillDownColumns.DataSource = SelectDrillDownColumns();
            gvDrillDownColumns.DataBind();
        }

        private List<FactTableField> GetColsListWithDataType(string queryTable)
        {
            return objQueryHelperManager.GetColsListWithDataType(queryTable, string.Empty);
        }

        private void AddSelectedColumnInQuery()
        {
            for (int i = 0; i < grdColumns.VisibleRowCount; i++)
            {
                if (grdColumns.IsGroupRow(i))
                    continue;

                CheckBox chkSelect = grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Column Name"] as GridViewDataColumn, "chkSelect") as CheckBox;
                string tableName = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Column Name"] as GridViewDataColumn, "hTableName") as HiddenField).Value;
                string fieldName = chkSelect.Text;
                //if (fieldName.EndsWith("Lookup") || fieldName.EndsWith("User"))
                //{
                //    fieldName = fieldName + "$";
                //}

                //if (fieldName.EndsWith("$Id"))
                //{
                //    fieldName = fieldName.Remove(fieldName.Length - 3);
                //}


                var table = (from t in tables
                             where t.Name == tableName
                             select t).FirstOrDefault();
                //table.Columns = selectedColumn;
                if (chkSelect.Checked && table!=null)
                {
                    TableInfo t = new TableInfo();
                    string alignment = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Alignment"] as GridViewDataColumn, "ddlAlignment") as DropDownList).SelectedValue;
                    string aggregationfunc = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Functions"] as GridViewDataColumn, "ddlFunctions") as DropDownList).SelectedValue;
                    string dataType = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Format"] as GridViewDataColumn, "ddlDataType") as DropDownList).SelectedValue;
                    int sequence = Convert.ToInt32((grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Sequence"] as GridViewDataColumn, "ddlSequence") as DropDownList).SelectedValue);
                    string displayname = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Display Name"] as GridViewDataColumn, "txtLabel") as TextBox).Text;
                    string width = (grdColumns.FindRowCellTemplateControl(i, grdColumns.Columns["Width"] as GridViewDataColumn, "txtWidthColumn") as TextBox).Text;

                    if (table.Columns.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        table.Columns.Where(c => c.FieldName == fieldName && c.TableName == tableName).ToList()
                         .ForEach(ci =>
                         {
                             ci.Selected = true;
                             ci.Function = aggregationfunc;
                             ci.DataType = dataType;
                             ci.Sequence = sequence;
                             ci.DisplayName = displayname;
                             ci.Hidden = false;
                             ci.Alignment = alignment;
                             ci.Width = Convert.ToInt32("" + width);
                         });
                    }
                    else
                    {
                        var ci = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                        if (ci == null)
                            continue;
                        ci.Hidden = false;
                        table.Columns.Add(ci);

                    }
                    columns.Where(c => c.FieldName == fieldName && c.TableName == tableName)
                           .ToList()
                           .ForEach(ci =>
                           {
                               ci.Selected = true;
                               ci.Function = aggregationfunc;
                               ci.DataType = dataType;
                               ci.DisplayName = displayname;
                               ci.Sequence = sequence;
                               ci.Hidden = false;
                               ci.Alignment = alignment;
                               ci.Width = Convert.ToInt32("" + width);
                           });

                    if (groupBy.Exists(g => g.Column.FieldName == fieldName && g.Column.TableName == tableName))
                    {
                        groupBy.Where(g => g.Column.FieldName == fieldName && g.Column.TableName == tableName)
                            .ToList()
                            .ForEach(gi =>
                            {
                                gi.Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                            });
                    }
                    if (orderBy.Exists(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName))
                    {
                        orderBy.Where(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName)
                            .ToList()
                            .ForEach(oi =>
                            {
                                oi.Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                            });
                    }
                    if (selectedtotals.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        selectedtotals.Where(c => c.FieldName == fieldName && c.TableName == tableName)
                            .ToList()
                            .ForEach(ci =>
                            {
                                Utility.ColumnInfo cInfo = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                                ci.Selected = true;
                                ci.Sequence = sequence;
                                ci.Width = Convert.ToInt32("" + width);
                                if (cInfo != null)
                                {
                                    ci.DisplayName = cInfo.DisplayName;
                                    ci.DataType = cInfo.DataType;
                                }
                            });
                    }
                }
                else
                {
                    if (table != null && table.Columns.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        if (groupBy.Exists(g => g.Column.FieldName == fieldName && g.Column.TableName == tableName))
                        {
                            var col = table.Columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                            col.Hidden = true;
                            col.Selected = false;
                        }
                        else
                        {
                            if (fieldName == DatabaseObjects.Columns.ID)
                            {
                                var col = table.Columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                                col.Hidden = true;
                                col.Selected = false;
                            }
                            else
                            {
                                table.Columns.Remove(table.Columns.Find(c => c.FieldName == fieldName && c.TableName == tableName));
                            }
                        }
                        columns.Where(c => c.FieldName == fieldName && c.TableName == tableName)
                        .ToList()
                        .ForEach(ci =>
                        {
                            ci.Selected = false;
                        });
                    }
                    if (groupBy.Exists(g => g.Column.FieldName == fieldName && g.Column.TableName == tableName))
                    {
                        //groupBy.Remove(groupBy.Find(g => g.Column.FieldName == fieldName && g.Column.TableName == tableName));
                    }

                    if (orderBy.Exists(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName))
                    {
                        orderBy.Remove(orderBy.Find(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName));
                    }

                    if (selectedtotals.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        selectedtotals.Remove(selectedtotals.Find(c => c.FieldName == fieldName && c.TableName == tableName));
                    }
                }
            }
            //Auto select mandatory columns if gantt view selected.
            tables.ForEach(m =>
            {
                if (m.Columns.Where(n => n.FieldName == "GanttView").ToList().Count > 0)
                {
                    if (!m.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate))
                    {
                        var ci = columns.Find(c => c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate && c.TableName == m.Name);
                        ci.Hidden = false;
                        ci.Selected = true;
                        m.Columns.Add(ci);
                    }
                    if (!m.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.TicketActualStartDate))
                    {
                        var ci = columns.Find(c => c.FieldName == DatabaseObjects.Columns.TicketActualStartDate && c.TableName == m.Name);
                        ci.Hidden = false;
                        ci.Selected = true;
                        m.Columns.Add(ci);
                    }
                }
            });
            panel.QueryInfo.Tables = tables;
            panel.QueryInfo.OrderBy = orderBy;
            panel.QueryInfo.GroupBy = groupBy;
            panel.QueryInfo.Totals = selectedtotals;

        }

        private void LoadQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetQueryHTML(QueryClause.Select));

            if (isUnion)
                sb.Append(GetQueryHTML(GetFormClauseForUnion(joins, true), QueryClause.From));
            else
                sb.Append(GetQueryHTML(GetFromClause(joins, true), QueryClause.From));

            sb.Append(GetQueryHTML(QueryClause.Where));
            sb.Append(GetQueryHTML(QueryClause.GroupBy));
            sb.Append(GetQueryHTML(QueryClause.OrderBy));
            sb.Append(GetQueryHTML(QueryClause.Totals));

            litQuery.Text = sb.ToString();
        }

        public void EnumToListBox(Type EnumType, ListControl TheListBox)
        {
            Array Values = System.Enum.GetValues(EnumType);

            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                ListItem Item = new ListItem(Display, Value.ToString());
                TheListBox.Items.Add(Item);
            }
        }

        #region List View Related Methods
        private void BindWhereClause()
        {
            if (isBindWhereClause)
                return;

            if (whereClauses != null && whereClauses.Count > 0)
                whereClauses = whereClauses.OrderBy(x => x.ParentID == 0 ? x.ID : x.ParentID).ThenBy(x => x.ID).ToList();

            WhereClauseList.DataSource = whereClauses;
            WhereClauseList.DataBind();

            if (whereClauses.Count > 0)
            {
                Label lblRelationOpt = (Label)WhereClauseList.Items[0].FindControl("lblRelationOpt");
                if (lblRelationOpt != null)
                    lblRelationOpt.Visible = false;
            }
            isBindWhereClause = true;
        }

        private void AddWhereClause(ListViewItem item)
        {
            DropDownList drpColumn = (DropDownList)item.FindControl("drpColumn");
            string columnName = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[0];
            string DataType = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[1];
            OperatorType optType = (OperatorType)Enum.Parse(typeof(OperatorType), ((DropDownList)item.FindControl("drpOpt")).SelectedValue);
            RelationalOperator RelationOpt = (RelationalOperator)Enum.Parse(typeof(RelationalOperator), ((DropDownList)item.FindControl("drpRelOpt")).SelectedValue);
            qValueType vtype = (qValueType)Enum.Parse(typeof(qValueType), ((DropDownList)item.FindControl("drpValuetype")).SelectedValue);
            TextBox txtValue = (TextBox)item.FindControl("txtValue");
            string value = txtValue.Text;
            HtmlInputCheckBox chkValue = (HtmlInputCheckBox)item.FindControl("chkValue");
            ASPxDateEdit dtcValue = ((ASPxDateEdit)item.FindControl("dtcValue"));
            UserValueBox ppeValue = ((UserValueBox)item.FindControl("ppeValue"));
            string parameterType = ((DropDownList)item.FindControl("ddlParameterType")).SelectedItem.Text;

            int Id = 1;
            if (whereClauses != null && whereClauses.Count > 0)
                Id = whereClauses.Max(x => x.ID) + 1;

            var dataType = DataType.Replace("System.", "");

            if (vtype == qValueType.Constant)
            {
                switch (dataType)
                {
                    case "Number":
                    case "Counter":
                    case "Currency":
                        value = txtValue.Text;
                        break;
                    case "DateTime":
                        value = Convert.ToString(dtcValue.Date);
                        break;
                    case "User":
                        value = ppeValue.GetValues();
                        break;
                    default:
                        value = txtValue.Text;
                        break;
                }
            }
            else
            {
                value = txtValue.Text;
            }
            whereClauses.Add(new WhereInfo
            {
                ID = Id,
                DataType = DataType,
                ColumnName = columnName,
                Operator = optType,
                RelationOpt = whereClauses.Count == 0 ? RelationalOperator.None : RelationOpt,
                Valuetype = vtype,
                ParameterType = parameterType,
                Value = (vtype == qValueType.Constant ? value : ""),
                ParameterName = (vtype == qValueType.Parameter ? value : "")
            });

            panel.QueryInfo.WhereClauses = whereClauses;
            SaveQueryData();
            GenerateDynamicControl(whereClauses);
        }

        private void UpdateWhereClause(ListViewItem item, int Id)
        {
            DropDownList drpColumn = (DropDownList)item.FindControl("drpColumn");
            string columnName = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[0];
            string DataType = UGITUtility.SplitString(drpColumn.SelectedItem.Text, new string[] { "(", ")" })[1];
            OperatorType optType = (OperatorType)Enum.Parse(typeof(OperatorType), ((DropDownList)item.FindControl("drpOpt")).SelectedValue);
            RelationalOperator RelationOpt = (RelationalOperator)Enum.Parse(typeof(RelationalOperator), ((DropDownList)item.FindControl("drpRelOpt")).SelectedValue);
            qValueType vtype = (qValueType)Enum.Parse(typeof(qValueType), ((DropDownList)item.FindControl("drpValuetype")).SelectedValue);

            TextBox txtValue = (TextBox)item.FindControl("txtValue");
            string value = txtValue.Text;
            HtmlInputCheckBox chkValue = (HtmlInputCheckBox)item.FindControl("chkValue");
            ASPxDateEdit dtcValue = ((ASPxDateEdit)item.FindControl("dtcValue"));
            UserValueBox ppeValue = ((UserValueBox)item.FindControl("ppeValue"));
            string parameterType = ((DropDownList)item.FindControl("ddlParameterType")).SelectedItem.Text;
            var dataType = DataType.Replace("System.", "");

            if (vtype == qValueType.Constant)
            {
                switch (dataType)
                {
                    case "Number":
                    case "Counter":
                    case "Currency":
                        value = txtValue.Text;
                        break;
                    case "DateTime":
                        value = Convert.ToString(dtcValue.Date);
                        break;
                    case "User":
                        value = Convert.ToString(ppeValue.GetValues());
                        break;
                    default:
                        value = txtValue.Text;
                        break;
                }
            }
            else
            {
                value = txtValue.Text;
            }


            whereClauses.Where(w => w.ID == Id).ToList().
            ForEach(wi =>
            {
                wi.DataType = DataType;
                wi.ColumnName = columnName;
                wi.Operator = optType;
                wi.RelationOpt = Id == 1 ? RelationalOperator.None : RelationOpt;
                wi.Valuetype = vtype;
                wi.Value = (vtype == qValueType.Constant ? value : "");
                wi.ParameterName = (vtype == qValueType.Parameter ? value : "");
                wi.ParameterType = parameterType;
            });

            panel.QueryInfo.WhereClauses = whereClauses;
            SaveQueryData();
            WhereClauseList.EditIndex = -1;
            GenerateDynamicControl(whereClauses);
        }
        #endregion

        private void GetGroupBy()
        {
            groupBy = new List<GroupByInfo>();
            if (ddlFirstGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlFirstGroupBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlFirstGroupBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlFirstGroupBy.SelectedValue, ".")[0];
                }

                string displayName = txtFirstGroupByDisplaytext.Text.Trim();
                if (groupBy.Exists(g => g.Num == 1))
                {
                    groupBy.Where(g => g.Num == 1).ToList().ForEach(gb =>
                    {
                        var col = new Utility.ColumnInfo();
                        if (isUnion)
                            col = columns.Find(c => c.FieldName == fieldName);
                        else
                            col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                        col.DisplayName = displayName;
                        gb.Column = col;
                    });
                }
                else
                {
                    var col = new Utility.ColumnInfo();
                    if (isUnion)
                        col = columns.Find(c => c.FieldName == fieldName);
                    else
                        col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                    col.DisplayName = displayName;
                    groupBy.Add(new GroupByInfo
                    {
                        Column = col,
                        Num = 1
                    });
                }

                if (isUnion)
                {
                    if (orderBy.Exists(o => o.Column.FieldName == fieldName))
                    {
                        orderBy.Where(o => o.Column.FieldName == fieldName)
                            .ToList()
                            .ForEach(oi =>
                            {
                                oi.Column.DisplayName = displayName;
                            });
                    }
                }
                else
                {
                    AddHiddenColumns(fieldName, tableName, displayName);
                    if (orderBy.Exists(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName))
                    {
                        orderBy.Where(o => o.Column.FieldName == fieldName && o.Column.TableName == tableName)
                            .ToList()
                            .ForEach(oi =>
                            {
                                oi.Column.DisplayName = displayName;
                            });
                    }
                }
            }
            else
            {
                groupBy.Remove(groupBy.Find(g => g.Num == 1));
            }

            if (ddlSecondGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlSecondGroupBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlSecondGroupBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlSecondGroupBy.SelectedValue, ".")[0];
                }
                string displayName = txtSecondGroupByDisplayText.Text.Trim();
                if (groupBy.Exists(g => g.Num == 2))
                {
                    groupBy.Where(g => g.Num == 2).ToList().ForEach(gb =>
                    {
                        var col = new Utility.ColumnInfo();
                        if (isUnion)
                            col = columns.Find(c => c.FieldName == fieldName);
                        else
                            col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                        col.DisplayName = displayName;
                        gb.Column = col;
                    });
                }
                else
                {
                    var col = new Utility.ColumnInfo();
                    if (isUnion)
                        col = columns.Find(c => c.FieldName == fieldName);
                    else
                        col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                    col.DisplayName = displayName;
                    groupBy.Add(new GroupByInfo
                    {
                        Column = col,
                        Num = 2
                    });
                }
                if (!isUnion)
                    AddHiddenColumns(fieldName, tableName, displayName);
            }
            else
            {
                groupBy.Remove(groupBy.Find(g => g.Num == 2));
            }

            if (ddlThirdGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlThirdGroupBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlThirdGroupBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlThirdGroupBy.SelectedValue, ".")[0];
                }
                string displayName = txtThirdGroupByDisplayText.Text.Trim();
                if (groupBy.Exists(g => g.Num == 3))
                {
                    groupBy.Where(g => g.Num == 3).ToList().ForEach(gb =>
                    {
                        var col = new Utility.ColumnInfo();
                        if (isUnion)
                            col = columns.Find(c => c.FieldName == fieldName);
                        else
                            col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                        col.DisplayName = displayName;
                        gb.Column = col;
                    });
                }
                else
                {
                    var col = new Utility.ColumnInfo();
                    if (isUnion)
                        col = columns.Find(c => c.FieldName == fieldName);
                    else
                        col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                    col.DisplayName = displayName;
                    groupBy.Add(new GroupByInfo
                    {
                        Column = col,
                        Num = 3
                    });
                }

                if (!isUnion)
                    AddHiddenColumns(fieldName, tableName, displayName);
            }
            else
            {
                groupBy.Remove(groupBy.Find(g => g.Num == 3));
            }
        }

        private void AddHiddenColumns(string fieldName, string tableName, string displayName)
        {
            var ci = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
            if (!ci.Selected)
            {
                var table = tables.Find(t => t.Name == tableName);
                if (!table.Columns.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                {
                    var col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                    col.DisplayName = displayName;
                    col.Hidden = true;
                    table.Columns.Add(col);
                }
            }
        }

        private void GetOrderBy()
        {
            orderBy = new List<OrderByInfo>();
            if (ddlFirstOrderBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlFirstOrderBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlFirstOrderBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlFirstOrderBy.SelectedValue, ".")[0];
                }

                if (orderBy.Exists(o => o.Num == 1))
                {
                    orderBy.Where(o => o.Num == 1).ToList().ForEach(ob =>
                    {
                        if (isUnion)
                            ob.Column = columns.Find(c => c.FieldName == fieldName);
                        else
                            ob.Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                        ob.orderBy = (rdFirstAscending.Checked) ? OrderBY.ASC : OrderBY.DESC;
                    });
                }
                else
                {
                    if (isUnion)
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName),
                            orderBy = (rdFirstAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 1
                        });
                    }
                    else
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName),
                            orderBy = (rdFirstAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 1
                        });
                    }
                }
            }
            else
            {
                orderBy.Remove(orderBy.Find(o => o.Num == 1));
            }

            if (ddlSecondOrderBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlSecondOrderBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlSecondOrderBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlSecondOrderBy.SelectedValue, ".")[0];
                }

                if (orderBy.Exists(o => o.Num == 2))
                {
                    orderBy.Where(o => o.Num == 2).ToList().ForEach(ob =>
                    {
                        if (isUnion)
                            ob.Column = columns.Find(c => c.FieldName == fieldName);
                        else
                            ob.Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);

                        ob.orderBy = (rdSecondAscending.Checked) ? OrderBY.ASC : OrderBY.DESC;
                    });
                }
                else
                {
                    if (isUnion)
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName),
                            orderBy = (rdSecondAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 2
                        });
                    }
                    else
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName),
                            orderBy = (rdSecondAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 2
                        });
                    }
                }
            }
            else
            {
                orderBy.Remove(orderBy.Find(o => o.Num == 2));
            }

            if (ddlThirdOrderBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                string tableName = string.Empty;

                if (isUnion)
                {
                    fieldName = ddlThirdOrderBy.SelectedValue;
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlThirdOrderBy.SelectedValue, ".")[1];
                    tableName = UGITUtility.SplitString(ddlThirdOrderBy.SelectedValue, ".")[0];
                }

                if (orderBy.Exists(o => o.Num == 3))
                {
                    orderBy.Where(o => o.Num == 3).ToList().ForEach(ob =>
                    {
                        if (isUnion)
                            ob.Column = columns.Find(c => c.FieldName == fieldName);
                        else
                            ob.Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                        ob.orderBy = (rdThirdAscending.Checked) ? OrderBY.ASC : OrderBY.DESC;
                    });
                }
                else
                {
                    if (isUnion)
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName),
                            orderBy = (rdThirdAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 3
                        });
                    }
                    else
                    {
                        orderBy.Add(new OrderByInfo
                        {
                            Column = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName),
                            orderBy = (rdThirdAscending.Checked) ? OrderBY.ASC : OrderBY.DESC,
                            Num = 3
                        });
                    }
                }
            }
            else
            {
                orderBy.Remove(orderBy.Find(o => o.Num == 3));
            }
        }

        protected void GenerateDynamicControl(List<WhereInfo> whereList)
        {
            parameterPnl.Controls.Clear();
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
            footerTable.CssClass = "table-footer";

            if (parameterPnl.FindControl("Table1") == null)
            {
                parameterPnl.Controls.Add(headerTable);
                parameterPnl.Controls.Add(table);
                parameterPnl.Controls.Add(footerTable);
            }

            TableRow lbRow = new TableRow();
            TableCell lbCell = new TableCell();
            lbCell.Width = Unit.Percentage(100);
            lbCell.HorizontalAlign = HorizontalAlign.Center;
            lbCell.Controls.Add(new LiteralControl("<span class='span-header'>Please enter value for parameter(s)</span>" +
                                                    "<span class='close-button'><img src='/content/buttonimages/cancel.png' /></span>"));
            lbRow.Controls.Add(lbCell);
            headerTable.Rows.Add(lbRow);
            // Now iterate through the table and add controls 
            int i = 0;
            foreach (var item in whereList.Where(x => x.Valuetype == qValueType.Parameter))
            {
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TextBox tb = new TextBox();
                tb.Width = Unit.Pixel(240);
                Label lb = new Label();
                cell1.Controls.Add(lb);

                cell1.CssClass = "ms-formlabel";
                cell2.CssClass = "ms-formbody param-value";
                ASPxDateEdit date = new ASPxDateEdit();
                string colName = item.ParameterName;
                if (item.ParameterRequired)
                {
                    lb.Text = colName + ": <span class='mandatory' style='color:#FF0000;'>*</span>";
                }
                else
                {
                    lb.Text = colName;
                }

                switch (item.ParameterType)
                {
                    case "TextBox":
                        // Add the control to the TableCell
                        cell2.Attributes.Add("isInputControl", "Yes");
                        cell2.Controls.Add(tb);
                        tb.ID = "TextBoxRow_" + i;
                        tb.EnableViewState = true;
                        tb.Text = item.Value;
                        tb.CssClass = "param-value";
                        break;
                    case "DateTime":
                        cell2.Attributes.Add("paramtype", "date");
                        cell2.Attributes.Add("isInputControl", "Yes");
                        cell1.Controls.Add(lb);
                        cell2.Controls.Add(date);
                        //date.DatePickerFrameUrl = uHelper.GetAbsoluteURL("/_layouts/15/iframe.aspx");
                        date.EditFormat = EditFormat.Date;
                        date.EditFormatString = "MMM/d/yyyy";
                        DateTime dt = DateTime.Now.Date; ;
                        DateTime.TryParse(item.Value, out dt);
                        date.Date = dt;
                        break;
                    case "DropDown":
                        DropDownList ddlOptions = new DropDownList();
                        string[] vals = item.DrpOptions.OptionsDropdown.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (vals != null && vals.Length > 0)
                        {
                            for (int k = 0; k < vals.Length; k++)
                            {
                                ddlOptions.Items.Add(vals[k]);
                            }
                        }

                        ddlOptions.SelectedValue = item.DrpOptions.DropdownDefaultValue;
                        cell2.Attributes.Add("isInputControl", "Yes");
                        cell2.Controls.Add(ddlOptions);
                        break;
                    case "Lookup":
                        DropDownList drpLookup = new DropDownList();
                        cell2.Attributes.Add("isInputControl", "Yes");
                        cell2.Controls.Add(drpLookup);

                        DataTable list = GetTableDataManager.GetTableData(item.LookupList.LookupListName, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
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

                                columnVals = dView.ToTable(true, field.ColumnName, DatabaseObjects.Columns.ID);
                                drpLookup.DataValueField = field.ColumnName;
                                drpLookup.DataTextField = field.ColumnName;
                                drpLookup.DataSource = columnVals;
                                drpLookup.DataBind();
                            }
                        }
                        break;
                    case "UserField":
                        cell2.Attributes.Add("paramtype", "user");
                        UserValueBox ppeUser = new UserValueBox();
                        ppeUser.isMulti = false;
                        ppeUser.Width = Unit.Pixel(200);
                        ppeUser.SelectionSet = "User";
                        ppeUser.CssClass = " clsparameter";
                        ppeUser.Attributes.Add("ugselectionset", ppeUser.SelectionSet);
                        cell2.Attributes.Add("isInputControl", "Yes");
                        cell2.Controls.Add(ppeUser);
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
            TableCell cell = new TableCell();
            cell.Controls.Add(new LiteralControl("<span class='span-submit'><input type='submit' id='param_button' value='Submit' onclick='return paramButtonClick();'  /></span>"));
            cell.Width = Unit.Percentage(100);
            cell.HorizontalAlign = HorizontalAlign.Right;
            btRow.Controls.Add(cell);
            footerTable.Rows.Add(btRow);
        }

        private void LoadTemplateList()
        {
            // Load all templates
            List<ChartTemplate> lstChartTemplates = chartTempManager.Load();

            if (lstChartTemplates != null && lstChartTemplates.Count > 0)
            {
                lstChartTemplates = lstChartTemplates.Where(x => x.TemplateType == Convert.ToString((int)DashboardType.Query)).ToList();

                foreach (ChartTemplate template in lstChartTemplates)
                {
                    ddlTemplateList.Items.Add(new ListItem(template.Title, Convert.ToString(template.ID)));
                }
            }

            ddlTemplateList.Items.Insert(0, new ListItem("--New Template--", "0"));
        }

        #endregion

        #region Control Events
        protected void btnUpdateGeneralInfo_Click(object sender, EventArgs e)
        {
            btnNext_Click(sender, e);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            switch (FormState)
            {
                case QueryWizardState.GeneralInfo:
                    uDashboard.Title = txtTitle.Text.Trim();
                    uDashboard.DashboardDescription = txtDescription.Text.Trim();
                    if (cmbOrder.SelectedIndex == -1)
                        uDashboard.ItemOrder = UGITUtility.StringToInt(cmbOrder.Items[cmbOrder.Items.Count - 1].Value);
                    else
                    {
                        int order = Convert.ToInt32(cmbOrder.Value);
                        uDashboard.ItemOrder = order > 1 ? order - 1 : order;
                    }
                    uDashboard.CategoryName = queryCategory.Text != string.Empty ? queryCategory.Text : ddlCategories.SelectedValue.Trim();
                    uDashboard.SubCategory = querySubCategory.Text != string.Empty ? querySubCategory.Text : ddlSubCategories.SelectedValue.Trim();
                    List<string> ModuleLookup = new List<string>();
                    foreach (ListItem item in cbModules.Items)
                    {
                        if (item.Selected)
                        {
                            ModuleLookup.Add(item.Value);
                        }
                    }
                    uDashboard.DashboardModuleMultiLookup = string.Join(",", ModuleLookup.ToArray());
                    ///For Authorized To View People Picker                   
                    uDashboard.AuthorizedToView = ppeAuthorizeToView.GetValues();
                    break;
                case QueryWizardState.TablesJoin:
                    tables = GetAllTables();
                    ddtables = GetAllDrilldownTables();
                    //AddSelectedColumnInQuery();
                    panel.QueryInfo.Tables = tables;
                    panel.QueryInfo.DrillDownTables = ddtables;
                    if (tables.Count < 2)
                    {
                        panel.QueryInfo.JoinList = null;
                        joins = null;
                    }
                    break;
                case QueryWizardState.Columns:
                    AddSelectedColumnInQuery();
                    break;
                case QueryWizardState.WhereClause:

                    break;
                case QueryWizardState.GroupBy:
                    GetGroupBy();
                    break;
                case QueryWizardState.SortBy:
                    GetOrderBy();
                    break;
                case QueryWizardState.Totals:
                    AddColumnsInTotals();
                    break;
                case QueryWizardState.QueryFormat:
                    SaveQueryFormat();
                    break;
                case QueryWizardState.Drilldown:
                    AddDrillDownColumnsInFormat();
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(missingColumns))
                RemoveMissingColumnsFromTable();

            // Invoke SaveQueryData() only when User clicks on Save/Update button & thus we skip the calling of this function when user switches between tabs
            if (sender != null && !string.IsNullOrEmpty(((Control)sender).ID) && ((Control)sender).ID == "btnUpdateGeneralInfo")
                SaveQueryData();

            var lb = ((ASPxButton)sender);

            if (lb != null && lb.Text.Contains("Save Changes"))
            {
                LoadCategories();
                OnTab_Change(FormState);
                LoadQuery();
                var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
                if (!string.IsNullOrWhiteSpace(refreshParentID))
                {
                    UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
                }

                uHelper.PerformAjaxPanelCallBack(Page, Context);
                return;
            }

            FormState = (QueryWizardState)(Convert.ToInt32(FormState) + 1);
            OnTab_Change(FormState);
            LoadQuery();

        }

        private void AddDrillDownColumnsInFormat()
        {
            for (int i = 0; i < gvDrillDownColumns.VisibleRowCount; i++)
            {
                if (gvDrillDownColumns.IsGroupRow(i))
                    continue;

                CheckBox chkSelect = gvDrillDownColumns.FindDetailRowTemplateControl(i, "chkSelect") as CheckBox;
                string tableName = (gvDrillDownColumns.FindDetailRowTemplateControl(i, "hTableName") as HiddenField).Value;
                string fieldName = chkSelect.Text;

                var table = (from t in ddtables
                             where t.Name == tableName
                             select t).FirstOrDefault();

                if (chkSelect.Checked)
                {
                    TableInfo t = new TableInfo();
                    string aggregationfunc = (gvDrillDownColumns.FindDetailRowTemplateControl(i, "ddlFunctions") as DropDownList).SelectedValue;
                    string dataType = (gvDrillDownColumns.FindDetailRowTemplateControl(i, "ddlDataType") as DropDownList).SelectedValue;
                    int sequence = Convert.ToInt32((gvDrillDownColumns.FindDetailRowTemplateControl(i, "ddlDataType") as DropDownList).SelectedValue);
                    string displayname = (gvDrillDownColumns.FindDetailRowTemplateControl(i, "txtLabel") as TextBox).Text;
                    //string width = (gvDrillDownColumns.FindDetailRowTemplateControl(i, "Width") as TextBox, "txtWidthColumn") as TextBox).Text;

                    if (table.Columns.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        table.Columns.Where(c => c.FieldName == fieldName && c.TableName == tableName).ToList()
                         .ForEach(ci =>
                         {
                             ci.Selected = true;
                             ci.Function = aggregationfunc;
                             ci.DataType = dataType;
                             ci.Sequence = sequence;
                             ci.DisplayName = displayname;
                             ci.Hidden = false;
                             ci.IsDrillDown = true;
                         });
                    }
                    else
                    {
                        var ci = ddcolumns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                        ci.Hidden = false;
                        table.Columns.Add(ci);

                    }

                    ddcolumns.Where(c => c.FieldName == fieldName && c.TableName == tableName)
                           .ToList()
                           .ForEach(ci =>
                           {
                               ci.Selected = true;
                               ci.Function = aggregationfunc;
                               ci.DataType = dataType;
                               ci.DisplayName = displayname;
                               ci.Sequence = sequence;
                               ci.Hidden = false;
                               ci.IsDrillDown = true;
                           });
                }
                else
                {
                    if (table.Columns.Exists(c => c.FieldName == fieldName && c.TableName == tableName))
                    {
                        ddcolumns.Where(c => c.FieldName == fieldName && c.TableName == tableName)
                        .ToList()
                        .ForEach(ci =>
                        {
                            ci.Selected = false;
                            ci.IsDrillDown = false;
                        });
                    }
                }
            }

            // Auto select mandatory columns if gantt view selected.
            ddtables.ForEach(m =>
            {
                if (m.Columns.Where(n => n.FieldName == "GanttView").ToList().Count > 0)
                {
                    if (!m.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate))
                    {
                        var ci = columns.Find(c => c.FieldName == DatabaseObjects.Columns.TicketActualCompletionDate && c.TableName == m.Name);
                        ci.Hidden = false;
                        ci.Selected = true;
                        m.Columns.Add(ci);
                    }

                    if (!m.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.TicketActualStartDate))
                    {

                        var ci = columns.Find(c => c.FieldName == DatabaseObjects.Columns.TicketActualStartDate && c.TableName == m.Name);
                        ci.Hidden = false;
                        ci.Selected = true;
                        m.Columns.Add(ci);
                    }

                }

                m.Columns.RemoveAll(x => !x.Selected);
            });

            panel.QueryInfo.DrillDownTables = ddtables;
        }

        private void SaveQueryFormat()
        {
            if (queryformat == null)
                queryformat = new QueryFormat();

            string fileName = string.Empty;
            string uploadFileURL = string.Empty;

            queryformat.Text = txtText.Text.Trim();
            queryformat.TextFontName = ddlTextFontName.SelectedItem.Text;
            queryformat.TextFontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), ddlFontStyle.SelectedValue);
            queryformat.TextFontSize = ddlFontSize.SelectedValue;
            queryformat.TextForeColor = ceFont.Color.Name.Length > 1 ? string.Format("{0}", ceFont.Color.Name.Substring(2)) : string.Empty;
            queryformat.HideText = chkTextHide.Checked;

            queryformat.Label = txtLabel.Text.Trim();
            queryformat.LabelFontName = ddlLabelFontName.SelectedItem.Text;
            queryformat.LabelFontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), ddlLabelFontStyle.SelectedValue);
            queryformat.LabelFontSize = ddlLabelFontSize.SelectedValue;
            queryformat.LabelForeColor = ceLabelFont.Color.Name.Length > 1 ? string.Format("{0}", ceLabelFont.Color.Name.Substring(2)) : string.Empty;
            queryformat.HideLabel = chkLabelHide.Checked;

            queryformat.ResultFontName = ddlFontName.SelectedItem.Text;
            queryformat.ResultFontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), ddlNumFontStyle.SelectedValue);
            queryformat.ResultFontSize = ddlNumFontSize.SelectedValue;
            queryformat.ResultForeColor = ceNumFont.Color.Name.Length > 1 ? string.Format("{0}", ceNumFont.Color.Name.Substring(2)) : string.Empty;

            queryformat.TitlePosition = (FloatType)Enum.Parse(typeof(FloatType), ddlTitlePosition.SelectedValue);
            queryformat.FormatType = (QueryFormatType)Enum.Parse(typeof(QueryFormatType), ddlQueryType.SelectedValue);


            if (uploadBackgroundImage.HasFile)
            {
                //Commented By Munna on 16-08-2017 

                //SPSecurity.CodeToRunElevated uploadBGfiles = new SPSecurity.CodeToRunElevated(delegate
                //{
                //    fileName = uploadBackgroundImage.FileName;
                //    uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //    uploadBackgroundImage.PostedFile.SaveAs(path);
                //    txtBackgroundimage.Text = uploadFileURL;
                //});
                //SPSecurity.RunWithElevatedPrivileges(uploadBGfiles);
            }

            queryformat.BackgroundImage = txtBackgroundimage.Text.Trim();
            queryformat.SizeOfFrame = new System.Drawing.Size(Convert.ToInt32(txtWidth.Text.Trim() == "" ? "0" : txtWidth.Text.Trim()),
                                                              Convert.ToInt32(txtHeight.Text.Trim() == "" ? "0" : txtHeight.Text.Trim()));
            queryformat.Location = new System.Drawing.Point(Convert.ToInt32(txtLocationLeft.Text.Trim() == "" ? "0" : txtLocationLeft.Text.Trim()),
                                                            Convert.ToInt32(txtLocationTop.Text.Trim() == "" ? "0" : txtLocationTop.Text.Trim()));
            queryformat.ResultPanelDesign = (ResultPanelType)Enum.Parse(typeof(ResultPanelType), ddlResultPnlDesign.SelectedValue);

            if (uploadIconImage.HasFile)
            {
                //SPSecurity.CodeToRunElevated uploadIconfiles = new SPSecurity.CodeToRunElevated(delegate
                //{

                //    fileName = uploadIconImage.FileName;
                //    uploadFileURL = string.Format("/_layouts/15/images/ugovernit/uploadedfiles/{0}", fileName);
                //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileName);
                //    uploadIconImage.PostedFile.SaveAs(path);
                //    txtIconImage.Text = uploadFileURL;
                //});
                //SPSecurity.RunWithElevatedPrivileges(uploadIconfiles);
            }
            queryformat.IconImage = txtIconImage.Text.Trim();
            queryformat.IconLocation = new System.Drawing.Point(Convert.ToInt32(txticonPositionLeft.Text.Trim() == "" ? "0" : txticonPositionLeft.Text.Trim()),
                                                                Convert.ToInt32(txticonPositionTop.Text.Trim() == "" ? "0" : txticonPositionTop.Text.Trim()));
            queryformat.IconSize = new System.Drawing.Size(Convert.ToInt32(txticonWidth.Text.Trim() == "" ? "0" : txticonWidth.Text.Trim()),
                                                              Convert.ToInt32(txticonHeight.Text.Trim() == "" ? "0" : txticonHeight.Text.Trim()));

            queryformat.BackgroundColor = ceBGColor.Color.Name.Length > 1 ? string.Format("{0}", ceBGColor.Color.Name.Substring(2)) : string.Empty;
            queryformat.BorderColor = ceBorderColor.Color.Name.Length > 1 ? string.Format("{0}", ceBorderColor.Color.Name.Substring(2)) : string.Empty;
            queryformat.BorderWidth = Convert.ToInt32(txtBorderWidth.Text.Trim());
            queryformat.HeaderColor = ceHeaderColor.Color.Name.Length > 1 ? string.Format("{0}", ceHeaderColor.Color.Name.Substring(2)) : string.Empty;
            queryformat.RowColor = ceRowColor.Color.Name.Length > 1 ? string.Format("{0}", ceRowColor.Color.Name.Substring(2)) : string.Empty;
            queryformat.RowAlternateColor = ceRowAlterColor.Color.Name.Length > 1 ? string.Format("{0}", ceRowAlterColor.Color.Name.Substring(2)) : string.Empty;

            queryformat.NavigateType = ddlNavigationType.SelectedItem.Text;
            queryformat.DrillDownType = ddlDrillDown.SelectedItem.Text;

            if (ddlDrillDown.SelectedValue == "2")
                queryformat.CustomUrl = txtCustomUrl.Text;

            queryformat.QueryId = ddlqueries.SelectedValue;
            queryformat.ShowCompanyLogo = chkShowCompanyLogo.Checked;
            //queryformat.HidePager = chkHidePager.Checked;
            //queryformat.PageSize = Convert.ToInt32(txtPageSize.Value);
            //queryformat.HideGroupingHeader = chkHideGH.Checked;
            //queryformat.PagePosition = ddlPagerPosition.SelectedValue;
            //queryformat.HideFilterbar = chkHideFilterbar.Checked;
            //queryformat.HideActions = chkHideActions.Checked;
            queryformat.ShowDateInFooter = chkShowDateInFooter.Checked;
            queryformat.Footer = txtFooter.Text;
            queryformat.Header = txtHeader.Text;
            queryformat.AdditionalInfo = txtAdditionalInfo.Text;
            queryformat.AdditionalFooterInfo = txtAdditionalFooterInfo.Text;

            //To Enable Edit Url
            queryformat.EnableEditUrl = chkEnableEditUrl.Checked;

            panel.QueryInfo.QueryFormats = queryformat;
        }

        private TableInfo GetTable(string name, int Id)
        {
            var _table = new TableInfo
            {
                Name = name,
                ID = Id
            };
            if (tables.Exists(t => t.Name == name && t.ID == Id))
            {
                _table.Columns = tables.Find(t => t.Name == name && t.ID == Id).Columns;
                if (!_table.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.ID))
                {
                    if (columns.Exists(c => c.FieldName == DatabaseObjects.Columns.ID && c.TableName == _table.Name))
                    {
                        var column = columns.Find(c => c.FieldName == DatabaseObjects.Columns.ID && c.TableName == _table.Name);
                        column.Hidden = true;
                        _table.Columns.Add(column);
                    }
                    else
                    {
                        var column = new Utility.ColumnInfo { Hidden = true, FieldName = DatabaseObjects.Columns.ID, TableName = _table.Name, DisplayName = DatabaseObjects.Columns.ID };
                        _table.Columns.Add(column);
                    }
                    //BTS-23-001429: This line has been commented as it was erasing the list of columns selected for a table.
                    //_table.Columns = new List<Utility.ColumnInfo>();
                }
            }
            return _table;
        }

        private List<TableInfo> GetAllTables()
        {
            var _listTable = new List<TableInfo>();

            _listTable.Add(GetTable(DdlQueryTables.SelectedValue, 1));

            if (DdlQueryTables2.SelectedIndex > 0)
            {
                _listTable.Add(GetTable(DdlQueryTables2.SelectedValue, 2));
            }
            if (DdlQueryTables3.SelectedIndex > 0)
            {
                _listTable.Add(GetTable(DdlQueryTables3.SelectedValue, 3));
            }
            if (DdlQueryTables4.SelectedIndex > 0)
            {
                _listTable.Add(GetTable(DdlQueryTables4.SelectedValue, 4));
            }
            if (DdlQueryTables5.SelectedIndex > 0)
            {
                _listTable.Add(GetTable(DdlQueryTables5.SelectedValue, 5));
            }

            return _listTable;
        }

        private List<TableInfo> GetAllDrilldownTables()
        {
            var _listTable = new List<TableInfo>();

            _listTable.Add(GetDrilldownTable(DdlQueryTables.SelectedValue, 1));

            if (DdlQueryTables2.SelectedIndex > 0)
            {
                _listTable.Add(GetDrilldownTable(DdlQueryTables2.SelectedValue, 2));
            }
            if (DdlQueryTables3.SelectedIndex > 0)
            {
                _listTable.Add(GetDrilldownTable(DdlQueryTables3.SelectedValue, 3));
            }
            if (DdlQueryTables4.SelectedIndex > 0)
            {
                _listTable.Add(GetDrilldownTable(DdlQueryTables4.SelectedValue, 4));
            }
            if (DdlQueryTables5.SelectedIndex > 0)
            {
                _listTable.Add(GetDrilldownTable(DdlQueryTables5.SelectedValue, 5));
            }

            return _listTable;
        }

        private TableInfo GetDrilldownTable(string name, int Id)
        {
            var _table = new TableInfo
            {
                Name = name,
                ID = Id
            };
            if (ddtables.Exists(t => t.Name == name && t.ID == Id))
            {
                _table.Columns = ddtables.Find(t => t.Name == name && t.ID == Id).Columns;
                if (!_table.Columns.Exists(c => c.FieldName == DatabaseObjects.Columns.ID))
                {
                    if (ddcolumns.Exists(c => c.FieldName == DatabaseObjects.Columns.ID && c.TableName == _table.Name))
                    {
                        var column = ddcolumns.Find(c => c.FieldName == DatabaseObjects.Columns.ID && c.TableName == _table.Name);
                        column.Hidden = true;
                        _table.Columns.Add(column);
                    }
                    else
                    {
                        var column = new Utility.ColumnInfo { Hidden = true, FieldName = DatabaseObjects.Columns.ID, TableName = _table.Name, DisplayName = DatabaseObjects.Columns.ID };
                        _table.Columns.Add(column);
                    }
                }
            }
            else
                _table.Columns = new List<Utility.ColumnInfo>();

            return _table;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            FormState = (QueryWizardState)(Convert.ToInt32(FormState) - 1);
            if (FormState == QueryWizardState.GeneralInfo)
            {
                FormState = QueryWizardState.GeneralInfo;
            }

            OnTab_Change(FormState);
        }

        protected void ibAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (!pnlTable2.Visible)
            {
                pnlTable2.Visible = true; return;
            }
            if (!pnlTable3.Visible)
            {
                pnlTable3.Visible = true; return;
            }
            if (!pnlTable4.Visible)
            {
                pnlTable4.Visible = true; return;
            }
            if (!pnlTable5.Visible)
            {
                pnlTable5.Visible = true; return;
            }
        }

        protected void ibRemoveTable_Click(object sender, ImageClickEventArgs e)
        {
            string deletedTable = string.Empty;
            switch ((sender as ImageButton).ID)
            {
                case "ibRemoveTable2":
                    joins = joins.Where(m => m.SecondTable != DdlQueryTables2.SelectedValue && m.FirstTable != DdlQueryTables2.SelectedValue).ToList();
                    TableInfo tInfo = tables.Where(m => m.Name == DdlQueryTables2.SelectedValue).FirstOrDefault();
                    TableInfo ddtInfo = ddtables.Where(m => m.Name == DdlQueryTables2.SelectedValue).FirstOrDefault();
                    tables.Remove(tInfo);
                    ddtables.Remove(ddtInfo);
                    deletedTable = DdlQueryTables2.SelectedValue;
                    break;
                case "ibRemoveTable3":
                    joins = joins.Where(m => m.SecondTable != DdlQueryTables3.SelectedValue && m.FirstTable != DdlQueryTables3.SelectedValue).ToList();
                    tInfo = tables.Where(m => m.Name == DdlQueryTables3.SelectedValue).FirstOrDefault();
                    ddtInfo = ddtables.Where(m => m.Name == DdlQueryTables3.SelectedValue).FirstOrDefault();
                    tables.Remove(tInfo);
                    ddtables.Remove(ddtInfo);
                    deletedTable = DdlQueryTables3.SelectedValue;
                    break;
                case "ibRemoveTable4":
                    joins = joins.Where(m => m.SecondTable != DdlQueryTables4.SelectedValue && m.FirstTable != DdlQueryTables4.SelectedValue).ToList();
                    tInfo = tables.Where(m => m.Name == DdlQueryTables4.SelectedValue).FirstOrDefault();
                    ddtInfo = ddtables.Where(m => m.Name == DdlQueryTables4.SelectedValue).FirstOrDefault();
                    tables.Remove(tInfo);
                    ddtables.Remove(ddtInfo);
                    deletedTable = DdlQueryTables4.SelectedValue;
                    break;
                case "ibRemoveTable5":
                    joins = joins.Where(m => m.SecondTable != DdlQueryTables5.SelectedValue && m.FirstTable != DdlQueryTables5.SelectedValue).ToList();
                    tInfo = tables.Where(m => m.Name == DdlQueryTables5.SelectedValue).FirstOrDefault();
                    ddtInfo = ddtables.Where(m => m.Name == DdlQueryTables5.SelectedValue).FirstOrDefault();
                    tables.Remove(tInfo);
                    ddtables.Remove(ddtInfo);
                    deletedTable = DdlQueryTables5.SelectedValue;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(deletedTable))
            {
                orderBy.Remove(orderBy.Find(o => o.Column.TableName == deletedTable));
                groupBy.Remove(groupBy.Find(g => g.Column.TableName == deletedTable));
                selectedtotals.Remove(selectedtotals.Find(c => c.TableName == deletedTable));

                if (isUnion)
                {
                    columns = SelectColumns();
                    whereClauses = whereClauses.AsEnumerable().Where(x => columns.Any(y => x.ColumnName == y.FieldName)).ToList();

                    if (tables.Count == 1)
                        whereClauses.ForEach(x => { x.ColumnName = string.Format("{0}.{1}", tables[0].Name, x.ColumnName); });
                }
                else
                {
                    whereClauses.Remove(whereClauses.Find(m => m.ColumnName.Contains(deletedTable)));
                }
            }

            isUnion = joins.Any(x => Convert.ToString(x.JoinType) == Constants.Union);
            FillOrderByDropDown();
            FillGroupByDropDown();
            BindWhereClause();
            SaveQueryData();

            pnlTable2.Visible = false;
            pnlTable3.Visible = false;
            pnlTable4.Visible = false;
            pnlTable5.Visible = false;
            DdlQueryTables2.SelectedIndex = 0;
            DdlQueryTables3.SelectedIndex = 0;
            DdlQueryTables4.SelectedIndex = 0;
            DdlQueryTables5.SelectedIndex = 0;

            if (tables.Count > 1)
            {
                for (int i = 2; i <= tables.Count; i++)
                {
                    switch (i)
                    {
                        case 2:
                            DdlQueryTables2.SelectedValue = tables[i - 1].Name;
                            pnlTable2.Visible = true;
                            break;
                        case 3:
                            DdlQueryTables3.SelectedValue = tables[i - 1].Name;
                            pnlTable3.Visible = true;
                            break;
                        case 4:
                            DdlQueryTables4.SelectedValue = tables[i - 1].Name;
                            pnlTable4.Visible = true;
                            break;
                        case 5:
                            DdlQueryTables5.SelectedValue = tables[i - 1].Name;
                            pnlTable5.Visible = true;
                            break;

                    }
                }

                if (DdlJoin.SelectedValue == Constants.Union)
                    JoinTextArea.Text = GetUnionClause(joins);
                else
                    JoinTextArea.Text = GetFromClause(joins, false);

                BtEditJoin_Click(BtJoin2, e);
            }
            else
            {
                pnlTablesJoin.Visible = false;
                JoinTextArea.Text = string.Empty;
            }
            LoadQuery();
        }

        void UpdateTable(string tableName, int id)
        {
            TableInfo table = tables.Find(t => t.ID == id);

            if (table == null)
            {
                table = new TableInfo { Name = tableName, ID = id };
                table.Columns = new List<Utility.ColumnInfo>();
                tables.Add(table);
            }
            else
            {
                if (tableName != table.Name)
                {
                    tables.Where(t => t.ID == id).ToList().ForEach(t =>
                    {
                        t.Name = tableName;
                        t.Columns = new List<Utility.ColumnInfo>();
                    });
                }
                else
                {
                    tables.Where(t => t.ID == id).ToList().ForEach(t => { t.Name = tableName; });
                }
            }
        }

        void UpdateDDTable(string tableName, int id)
        {
            TableInfo table = ddtables.Find(t => t.ID == id);

            if (table == null)
            {
                table = new TableInfo { Name = tableName, ID = id };
                table.Columns = new List<Utility.ColumnInfo>();
                ddtables.Add(table);
            }
            else
            {
                if (tableName != table.Name)
                {
                    ddtables.Where(t => t.ID == id).ToList().ForEach(t =>
                    {
                        t.Name = tableName;
                        t.Columns = new List<Utility.ColumnInfo>();
                    });
                }
                else
                {
                    ddtables.Where(t => t.ID == id).ToList().ForEach(t => { t.Name = tableName; });
                }
            }
        }

        protected void BtJoin_Click(object sender, EventArgs e)
        {
            pnlTablesJoin.Visible = true;
            DdlAvlTables.Items.Clear();
            DropDownList b = sender as DropDownList;
            switch (b.ID)
            {
                case "DdlQueryTables2": // "BtJoin2":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables2.SelectedValue;
                    UpdateTable(DdlQueryTables.SelectedItem.Value, 1);
                    UpdateTable(DdlQueryTables2.SelectedItem.Value, 2);
                    UpdateDDTable(DdlQueryTables.SelectedItem.Value, 1);
                    UpdateDDTable(DdlQueryTables.SelectedItem.Value, 2);
                    break;
                case "DdlQueryTables3": //"BtJoin3":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables3.SelectedValue;
                    UpdateTable(DdlQueryTables3.SelectedItem.Value, 3);
                    UpdateDDTable(DdlQueryTables3.SelectedItem.Value, 3);
                    break;
                case "DdlQueryTables4":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables3.SelectedItem.Value, DdlQueryTables3.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables4.SelectedValue;
                    UpdateTable(DdlQueryTables4.SelectedItem.Value, 4);
                    UpdateDDTable(DdlQueryTables4.SelectedItem.Value, 4);
                    break;
                case "DdlQueryTables5":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables3.SelectedItem.Value, DdlQueryTables3.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables4.SelectedItem.Value, DdlQueryTables4.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables5.SelectedValue;
                    UpdateTable(DdlQueryTables5.SelectedItem.Value, 5);
                    UpdateDDTable(DdlQueryTables5.SelectedItem.Value, 5);
                    break;
                default:
                    break;
            }

            if (DdlQueryTables2.SelectedIndex == 0 && tables.Count > 1)
            {
                tables.RemoveAt(1);
                ddtables.RemoveAt(1);
            }
            if (DdlQueryTables3.SelectedIndex == 0 && tables.Count > 2)
            {
                tables.RemoveAt(2);
                ddtables.RemoveAt(2);
            }
            if (DdlQueryTables4.SelectedIndex == 0 && tables.Count > 3)
            {
                tables.RemoveAt(3);
                ddtables.RemoveAt(3);
            }
            if (DdlQueryTables5.SelectedIndex == 0 && tables.Count > 4)
            {
                tables.RemoveAt(4);
                ddtables.RemoveAt(4);
            }

            SaveQueryData();

            lbFirstTableName.Text = DdlAvlTables.SelectedValue;
            ListItemCollection itemColl1 = FillQueryTableFields(DdlAvlTables.SelectedValue, string.Empty, true);
            ListItemCollection itemColl2 = FillQueryTableFields(lbSecondTableName.Text, string.Empty, true);
            DdlFirstTabCols.DataSource = itemColl1;
            DdlFirstTabCols.DataBind();
            DdlSecondTabCols.DataSource = itemColl2;
            DdlSecondTabCols.DataBind();
        }

        protected void BtEditJoin_Click(object sender, EventArgs e)
        {
            pnlTablesJoin.Visible = true;
            DdlAvlTables.Items.Clear();
            ImageButton b = sender as ImageButton;
            switch (b.ID)
            {
                case "BtJoin2":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables2.SelectedValue;
                    UpdateTable(DdlQueryTables.SelectedItem.Value, 1);
                    UpdateTable(DdlQueryTables2.SelectedItem.Value, 2);
                    break;
                case "BtJoin3":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables3.SelectedValue;
                    UpdateTable(DdlQueryTables3.SelectedItem.Value, 3);
                    break;
                case "BtJoin4":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables3.SelectedItem.Value, DdlQueryTables3.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables4.SelectedValue;
                    UpdateTable(DdlQueryTables4.SelectedItem.Value, 4);
                    break;
                case "BtJoin5":
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables.SelectedItem.Value, DdlQueryTables.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables2.SelectedItem.Value, DdlQueryTables2.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables3.SelectedItem.Value, DdlQueryTables3.SelectedItem.Value));
                    DdlAvlTables.Items.Add(new ListItem(DdlQueryTables4.SelectedItem.Value, DdlQueryTables4.SelectedItem.Value));
                    lbSecondTableName.Text = DdlQueryTables5.SelectedValue;
                    UpdateTable(DdlQueryTables5.SelectedItem.Value, 5);
                    break;
                default:
                    break;
            }

            if (DdlQueryTables2.SelectedIndex == 0 && tables.Count > 1)
            {
                tables.RemoveAt(1);
            }
            if (DdlQueryTables3.SelectedIndex == 0 && tables.Count > 2)
            {
                tables.RemoveAt(2);
            }
            if (DdlQueryTables4.SelectedIndex == 0 && tables.Count > 3)
            {
                tables.RemoveAt(3);
            }
            if (DdlQueryTables5.SelectedIndex == 0 && tables.Count > 4)
            {
                tables.RemoveAt(4);
            }

            // Checking Postback condition to skip the calling of SaveQueryData() on Page Load
            if (IsPostBack)
                SaveQueryData();

            lbFirstTableName.Text = DdlAvlTables.SelectedValue;
            ListItemCollection itemColl1 = FillQueryTableFields(DdlAvlTables.SelectedValue, string.Empty, true);
            ListItemCollection itemColl2 = FillQueryTableFields(lbSecondTableName.Text, string.Empty, true);
            DdlFirstTabCols.DataSource = itemColl1;
            DdlFirstTabCols.DataBind();

            DdlSecondTabCols.DataSource = itemColl2;
            DdlSecondTabCols.DataBind();
        }

        protected void btnAddJoin_Click(object sender, EventArgs e)
        {
            bool isJoinChanged = false;
            int numberOfTable = tables.Count;

            if (numberOfTable >= 2)
            {
                Joins myjoin = new Joins
                {
                    FirstTable = lbFirstTableName.Text,
                    SecondTable = lbSecondTableName.Text,
                    FirstColumn = UGITUtility.SplitString(DdlFirstTabCols.SelectedValue, new string[] { "(", ")" })[0],
                    SecondColumn = UGITUtility.SplitString(DdlSecondTabCols.SelectedValue, new string[] { "(", ")" })[0],
                    JoinType = (JoinType)Enum.Parse(typeof(JoinType), DdlJoin.SelectedValue),
                    OperatorType = DdlOperator.SelectedValue,
                    DataTypeFirstCol = UGITUtility.SplitString(DdlFirstTabCols.SelectedValue, new string[] { "(", ")" })[1],
                    DataTypeSecondCol = UGITUtility.SplitString(DdlSecondTabCols.SelectedValue, new string[] { "(", ")" })[1]
                };

                if (joins != null && joins.Count > 0)
                {
                    bool joinEdited = false;
                    bool needJoinChange = false;

                    for (int i = 0; i < joins.Count; i++)
                    {
                        needJoinChange = isJoinChanged = (Convert.ToString(myjoin.JoinType) == Constants.Union && Convert.ToString(joins[i].JoinType) != Constants.Union) ||
                            (Convert.ToString(myjoin.JoinType) != Constants.Union && Convert.ToString(joins[i].JoinType) == Constants.Union);

                        if (joins[i].FirstTable == myjoin.FirstTable && joins[i].SecondTable == myjoin.SecondTable)
                        {
                            joins[i] = myjoin;
                            joinEdited = true;
                            continue;
                        }
                        else
                        {
                            if (needJoinChange)
                                joins[i].JoinType = myjoin.JoinType;
                        }
                    }

                    if (!joinEdited)
                    {

                        if (joins == null)
                        {
                            joins = new List<Joins>();
                        }
                        joins.Add(myjoin);
                    }
                }
                else
                {
                    joins = new List<Joins>();
                    joins.Add(myjoin);
                }
            }
            else
            {
                joins = null;
            }

            isUnion = joins.Any(x => Convert.ToString(x.JoinType) == Constants.Union);

            if (isJoinChanged || (joins.Count == 1 && isUnion))
            {
                whereClauses = new List<WhereInfo>();
                BindWhereClause();
                FillOrderByDropDown();
                FillGroupByDropDown();
                selectedtotals = new List<Utility.ColumnInfo>();
                FillTotals();
            }

            SaveQueryData();
            if (DdlJoin.SelectedValue == Constants.Union)
                JoinTextArea.Text = GetUnionClause(joins);
            else
                JoinTextArea.Text = GetFromClause(joins, false);

            LoadQuery();
        }

        protected void ddlSequence_Load(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;

            if (ddcolumns == null)
            {
                ddcolumns = SelectDrillDownColumns();
            }

            if (columns == null)
            {
                columns = SelectColumns();
            }

            int columnCount = ddcolumns.Count > columns.Count ? ddcolumns.Count : columns.Count;

            for (int i = 1; i <= columnCount; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString()));
            }
        }

        protected void grdColumns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Utility.ColumnInfo col = (Utility.ColumnInfo)e.Row.DataItem;

                CheckBox ch = (CheckBox)e.Row.FindControl("chkSelect");
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlFunctions");
                HiddenField hdn = (HiddenField)e.Row.FindControl("hdnFunctions");
                DropDownList ddl1 = (DropDownList)e.Row.FindControl("ddlDataType");
                HiddenField hdn1 = (HiddenField)e.Row.FindControl("hdnDataType");
                DropDownList ddlAlignment = (DropDownList)e.Row.FindControl("ddlAlignment");
                HiddenField hdnAlignment = (HiddenField)e.Row.FindControl("hdnAlignment");
                if (ch.Text == DatabaseObjects.Columns.ProjectHealth || ch.Text == "GanttView")
                {
                    TextBox tb = (TextBox)e.Row.FindControl("txtLabel");
                    tb.ReadOnly = true;
                    ddl.Enabled = false;
                    ddl1.Enabled = false;
                    return;
                }
                ddl1.Items.Clear();
                ddl.Items.Clear();
                switch (col.DataType)
                {
                    case "Double":
                    case "Currency":
                    case "Integer":
                    case "Percent":
                    case "Days":
                    case "Hours":
                    case "Minutes":
                        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Avg"), new ListItem("Sum"), new ListItem("Max"), new ListItem("Min") });
                        ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("Boolean"),
                                                             new ListItem("Integer"), new ListItem("Double"), new ListItem("Currency"),
                                                             new ListItem("Percent"), new ListItem("Percent*100"),
                                                             new ListItem("Days"), new ListItem("Hours"), new ListItem("Minutes") });
                        break;
                    case "DateTime":
                    case "Time":
                    case "Date":
                        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Max"), new ListItem("Min") });
                        ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("Date"), new ListItem("DateTime"), new ListItem("Time") });
                        break;
                    default:
                        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count") });

                        if (col.FieldName == DatabaseObjects.Columns.History || col.FieldName == DatabaseObjects.Columns.TicketComment || col.FieldName == DatabaseObjects.Columns.TicketResolutionComments)
                            ddl1.Items.AddRange(new ListItem[] { new ListItem("All Entry"), new ListItem("Last Entry") });
                        else if (col.FieldName == DatabaseObjects.Columns.TicketStatus)
                            ddl1.Items.AddRange(new ListItem[] { new ListItem("Progress"), new ListItem("String") });
                        else
                            ddl1.Items.AddRange(new ListItem[] {
                                                new ListItem("String"), new ListItem("Boolean"),
                                                new ListItem("Integer"), new ListItem("Double"), new ListItem("Currency"),
                                                new ListItem("Percent"), new ListItem("Percent*100"),
                                                new ListItem("Date"), new ListItem("DateTime"), new ListItem("Time"),
                                                new ListItem("User"), new ListItem("MultiUser") });
                        break;
                }
                if (hdn1 != null && hdn1.Value != string.Empty)
                {
                    ddl1.SelectedValue = hdn1.Value;
                }

                if (hdn != null && hdn.Value != string.Empty)
                {
                    ddl.SelectedValue = hdn.Value;
                }
                if (hdnAlignment != null && hdnAlignment.Value != string.Empty)
                {
                    ddlAlignment.SelectedValue = hdnAlignment.Value;
                }
            }
        }

        protected void spgrid_totals_RowDataBound(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            ASPxGridView grid = sender as ASPxGridView;
            object rowData = grid.GetRow(e.VisibleIndex);
            Utility.ColumnInfo col = (Utility.ColumnInfo)rowData;

            // Added below lines to fetch nested controls, which was not happening using 'FindControlIterative'.
            DropDownList ddl = (DropDownList)e.Row.FindControlRecursive("ddlFunctions");
            HiddenField hdn = (HiddenField)e.Row.FindControlRecursive("hdnFunctions");
            ddl.Items.Clear();

            switch (col.DataType)
            {
                case "Double":
                case "Currency":
                case "Integer":
                case "Minutes":
                case "Hours":
                case "Days":
                    ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Avg"), new ListItem("Sum"), new ListItem("Max"), new ListItem("Min") });
                    break;
                case "DateTime":
                    ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Max"), new ListItem("Min") });
                    break;
                default:
                    ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count") });
                    break;
            }
            if (hdn != null && hdn.Value != string.Empty)
            {
                ddl.SelectedValue = hdn.Value;
            }

        }

        protected void Preview_Click(object sender, EventArgs e)
        {
            if (paramValue.Value != string.Empty)
            {
                string[] param_values = paramValue.Value.Split(',');
                int j = 0;
                for (int i = 0; i < whereClauses.Count; i++)
                {
                    if (!string.IsNullOrEmpty(whereClauses[i].ParameterName))
                    {
                        whereClauses[i].Value = param_values[j];
                        j++;
                    }
                }
            }

            panel.QueryInfo.WhereClauses = whereClauses;
            SaveQueryData();
            isPreview.Value = "true";
        }

        protected void ddlFirstGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFirstGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                var col = new Utility.ColumnInfo();

                if (isUnion)
                {
                    fieldName = ddlFirstGroupBy.SelectedValue;
                    col = columns.Find(c => c.FieldName == fieldName);
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlFirstGroupBy.SelectedValue, ".")[1];
                    string tableName = UGITUtility.SplitString(ddlFirstGroupBy.SelectedValue, ".")[0];
                    col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                }

                txtFirstGroupByDisplaytext.Text = col.DisplayName;
            }
            else
            {
                txtFirstGroupByDisplaytext.Text = "";
            }
        }

        protected void ddlSecondGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSecondGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                var col = new Utility.ColumnInfo();

                if (isUnion)
                {
                    fieldName = ddlSecondGroupBy.SelectedValue;
                    col = columns.Find(c => c.FieldName == fieldName);
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlSecondGroupBy.SelectedValue, ".")[1];
                    string tableName = UGITUtility.SplitString(ddlSecondGroupBy.SelectedValue, ".")[0];
                    col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                }

                txtSecondGroupByDisplayText.Text = col.DisplayName;
            }
            else
            {
                txtSecondGroupByDisplayText.Text = "";
            }
        }

        protected void ddlThirdGroupBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlThirdGroupBy.SelectedIndex > 0)
            {
                string fieldName = string.Empty;
                var col = new Utility.ColumnInfo();

                if (isUnion)
                {
                    fieldName = ddlThirdGroupBy.SelectedValue;
                    col = columns.Find(c => c.FieldName == fieldName);
                }
                else
                {
                    fieldName = UGITUtility.SplitString(ddlThirdGroupBy.SelectedValue, ".")[1];
                    string tableName = UGITUtility.SplitString(ddlThirdGroupBy.SelectedValue, ".")[0];
                    col = columns.Find(c => c.FieldName == fieldName && c.TableName == tableName);
                }

                txtThirdGroupByDisplayText.Text = col.DisplayName;
            }
            else
            {
                txtThirdGroupByDisplayText.Text = "";
            }
        }
        #endregion

        #region List View Events

        protected void WhereClauseList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem item = (ListViewDataItem)e.Item;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                List<WhereInfo> whereInfos = (List<WhereInfo>)WhereClauseList.DataSource;
                int Id = whereInfos[item.DataItemIndex].ID;
                if (Id > -1)
                {
                    ImageButton lnkEdit = (ImageButton)item.FindControl("lnkEdit");
                    string addNewFilter = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, newParam, dashboardID, Id));
                    lnkEdit.Attributes.Add("onclick", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Edit Filter','600','400',0,'{1}','true'); return false;", addNewFilter, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
                }
            }
        }

        protected void WhereClauseList_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            int Id;
            int.TryParse(WhereClauseList.DataKeys[e.ItemIndex].Value.ToString(), out Id);
            WhereInfo wi = whereClauses.Find(w => w.ID == Id);
            whereClauses.Remove(wi);
            SaveQueryData();
            BindWhereClause();
            LoadQuery();

            GenerateDynamicControl(whereClauses);
        }
        protected void drpColumn_Load(object sender, EventArgs e)
        {
            DropDownList drpColumn = (DropDownList)sender;
            foreach (Utility.ColumnInfo c in columns)
            {
                drpColumn.Items.Add(new ListItem((c.TableName + "." + c.FieldName + "(" + c.DataType + ")")));
            }
        }


        #endregion

        protected void DdlAvlTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItemCollection itemColl1 = FillQueryTableFields(DdlAvlTables.SelectedValue, string.Empty, true);
            lbFirstTableName.Text = DdlAvlTables.SelectedValue;
            DdlFirstTabCols.DataSource = itemColl1;
            DdlFirstTabCols.DataBind();
        }

        protected void lnkPublish_Click(object sender, EventArgs e)
        {
            ASPxButton lnkPublish = (ASPxButton)sender;
            if (lnkPublish.Text == "Hide")
            {
                lnkPublish.Text = "Publish";
                uDashboard.IsActivated = false;
            }
            else if (lnkPublish.Text == "Publish")
            {
                lnkPublish.Text = "Hide";
                uDashboard.IsActivated = true;
            }
            SaveQueryData();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (dashboardID > 0)
            {
                uDashboard = objDashboardManager.LoadPanelById(dashboardID, false);
            }

            if (uDashboard != null)
            {
                // Load all templates
                List<ChartTemplate> lstChartTemplates = chartTempManager.Load();
                if (lstChartTemplates == null)
                    lstChartTemplates = new List<ChartTemplate>();

                ChartTemplate template = null;
                bool createNew = true;

                if (saveTemplate.Checked)
                {
                    lstChartTemplates = lstChartTemplates.Where(x => x.Title.ToLower() == templateName.Text.Trim().ToLower()).ToList();
                    if (lstChartTemplates != null && lstChartTemplates.Count > 0)
                    {
                        lblmsg.Text = "Template already exists. Please enter different name.";
                        return;
                    }

                    template = new ChartTemplate();
                    template.Title = templateName.Text.Trim();
                }
                else if (overrideTemplate.Checked)
                {
                    long templateID = 0;
                    long.TryParse(ddlTemplateList.SelectedValue.Trim(), out templateID);
                    if (templateID > 0)
                    {
                        template = chartTempManager.LoadByID(templateID);

                        if (template == null)
                            return;

                        template.Title = ddlTemplateList.SelectedItem.Text;
                        createNew = false;
                    }
                    else
                    {
                        template = new ChartTemplate();
                        template.Title = uDashboard.Title;
                    }
                }

                uDashboard.ID = 0;
                System.Xml.XmlDocument xmlDoc = uHelper.SerializeObject((object)uDashboard);
                template.ChartObject = xmlDoc.OuterXml;
                template.TemplateType = Convert.ToString((int)DashboardType.Query);

                // Add/Update template in DB
                chartTempManager.Save(template);

                if (createNew)
                    LoadTemplateList();

                ListItem templateItem = ddlTemplateList.Items.FindByValue(Convert.ToString(template.ID));
                if (templateItem != null)
                {
                    ddlTemplateList.SelectedIndex = ddlTemplateList.Items.IndexOf(templateItem);
                }
            }

            pcTemplate.ShowOnPageLoad = false;
            var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
            if (!string.IsNullOrWhiteSpace(refreshParentID))
            {
                UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
            }
        }

        /// <summary>
        /// Fills chart template list which will be used to override template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlTemplateList_PreRender(object sender, EventArgs e)
        {
            if (ddlTemplateList.Items.Count <= 0)
            {
                LoadTemplateList();
            }
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            btnNext_Click(btnUpdateGeneralInfo, e);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false, false);
        }

        protected void grdColumns_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data)
                return;

            CheckBox ch = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "chkSelect") as CheckBox;
            DropDownList ddl = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "ddlFunctions") as DropDownList;
            HiddenField hdn = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "hdnFunctions") as HiddenField;
            DropDownList ddl1 = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "ddlDataType") as DropDownList;
            HiddenField hdn1 = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "hdnDataType") as HiddenField;
            DropDownList ddlAlignment = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "ddlAlignment") as DropDownList;
            HiddenField hdnAlignment = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "hdnAlignment") as HiddenField;

            if (ch.Text == DatabaseObjects.Columns.ProjectHealth || ch.Text == "GanttView")
            {
                TextBox tb = grdColumns.FindRowCellTemplateControl(e.VisibleIndex, null, "txtLabel") as TextBox;
                tb.ReadOnly = true;
                ddl.Enabled = false;
                ddl1.Enabled = false;
                return;
            }
            // ddl1.Items.Clear();
            //ddl.Items.Clear();
            //switch (col.DataType)
            //{
            //    case "Double":
            //    case "Currency":
            //    case "Integer":
            //    case "Percent":
            //    case "Days":
            //    case "Hours":
            //    case "Minutes":
            //        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Avg"), new ListItem("Sum"), new ListItem("Max"), new ListItem("Min") });
            //        ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("Currency"), new ListItem("Double"), new ListItem("Percent"), new ListItem("Percent*100"), new ListItem("Integer"), new ListItem("Days"), new ListItem("Hours"), new ListItem("Minutes") });
            //        break;
            //    case "DateTime":
            //    case "Time":
            //    case "Date":
            //        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count"), new ListItem("Max"), new ListItem("Min") });
            //        ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("DateTime"), new ListItem("Time"), new ListItem("Date") });
            //        break;
            //    default:
            //        ddl.Items.AddRange(new ListItem[] { new ListItem("none"), new ListItem("Count") });
            //        if (col.FieldName == DatabaseObjects.Columns.History || col.FieldName == DatabaseObjects.Columns.TicketComment)
            //        {
            //            ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("Currency"), new ListItem("Double"), new ListItem("Percent"), new ListItem("Percent*100"), new ListItem("Integer"), new ListItem("DateTime"), new ListItem("Time"), new ListItem("Date"), new ListItem("User"), new ListItem("MultiUser"), new ListItem("Boolean"), new ListItem("All Entry"), new ListItem("Last Entry") });
            //        }
            //        else
            //        {
            //            ddl1.Items.AddRange(new ListItem[] { new ListItem("String"), new ListItem("Currency"), new ListItem("Double"), new ListItem("Percent"), new ListItem("Percent*100"), new ListItem("Integer"), new ListItem("DateTime"), new ListItem("Time"), new ListItem("Date"), new ListItem("User"), new ListItem("MultiUser"), new ListItem("Boolean") });
            //        }
            //        break;
            //}
            if (hdn1 != null && hdn1.Value != string.Empty)
            {
                ddl1.SelectedValue = hdn1.Value;
            }

            if (hdn != null && hdn.Value != string.Empty)
            {
                ddl.SelectedValue = hdn.Value;
            }
            if (hdnAlignment != null && hdnAlignment.Value != string.Empty)
            {
                ddlAlignment.SelectedValue = hdnAlignment.Value;
            }
        }

        #region Methods to disable group labels for Fact Table and Non-Fact Table in tables dropdownlists
        /// <summary>
        /// These methods are used to disable group labels for Fact Table and Non-Fact Table in tables dropdownlists
        /// </summary>
        public void DisableDropDownGroupItems()
        {
            // Disable group labels for all five table dropdowns
            DisableListItem(DdlQueryTables);

            DisableListItem(DdlQueryTables2);

            DisableListItem(DdlQueryTables3);

            DisableListItem(DdlQueryTables4);

            DisableListItem(DdlQueryTables5);
        }

        public void DisableListItem(DropDownList ddlQueryTable)
        {
            string factTables = "FactTables";
            string nonFactTables = "Tables";

            ListItem liFactTable = ddlQueryTable.Items.FindByValue(factTables);

            if (liFactTable != null)
            {
                liFactTable.Attributes.Add("disabled", "disabled");
                liFactTable.Attributes.Add("class", "disabledItems");
            }

            ListItem liNonFactTable = ddlQueryTable.Items.FindByValue(nonFactTables);

            if (liNonFactTable != null)
            {
                liNonFactTable.Attributes.Add("disabled", "disabled");
                liNonFactTable.Attributes.Add("class", "disabledItems");
            }
        }
        #endregion Methods to disable group labels for Fact Table and Non-Fact Table in tables dropdownlists

        /// <summary>
        /// This method is used to check if current whereclause item is a Parent of any other item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsGroupParent(int id)
        {
            WhereInfo wInfo = whereClauses.FirstOrDefault(x => x.ID == id);
            if (wInfo != null)
            {
                return whereClauses.Exists(x => x.ParentID == wInfo.ID);
            }

            return false;
        }

        /// <summary>
        /// This method is used to create groups on filter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCreateGroup_Click(object sender, EventArgs e)
        {
            if (!hdnQueryParams.Contains("items") || string.IsNullOrWhiteSpace(Convert.ToString(hdnQueryParams["items"])))
                return;

            var param = Convert.ToString(hdnQueryParams["items"]);
            List<string> items = UGITUtility.ConvertStringToList(param, ",");

            if (items == null || items.Count == 0)
                return;

            int parentID = UGITUtility.StringToInt(items[0]);
            List<int> children = items.Select(x => UGITUtility.StringToInt(x)).Where(x => x != parentID).ToList();
            List<WhereInfo> wInfos = whereClauses.Where(x => children.Contains(x.ID)).ToList();
            foreach (WhereInfo wf in wInfos)
            {
                wf.ParentID = parentID;
            }

            SaveQueryData();
            BindWhereClause();
            LoadQuery();
            hdnQueryParams.Remove("items");
        }

        /// <summary>
        /// This method is used to delete groups on filter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btRemoveGroup_Click(object sender, EventArgs e)
        {
            if (!hdnQueryParams.Contains("items") || string.IsNullOrWhiteSpace(Convert.ToString(hdnQueryParams["items"])))
                return;

            int itemID = UGITUtility.StringToInt(hdnQueryParams["items"]);
            List<WhereInfo> wInfos = whereClauses.Where(x => x.ParentID == itemID).ToList();
            foreach (WhereInfo wf in wInfos)
            {
                wf.ParentID = 0;
            }

            SaveQueryData();
            BindWhereClause();
            LoadQuery();
            hdnQueryParams.Remove("items");
        }

        /// <summary>
        /// Method to update first table in query on if the selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlQueryTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTable(DdlQueryTables.SelectedItem.Value, 1);
            UpdateDDTable(DdlQueryTables.SelectedItem.Value, 1);

            LoadQuery();
        }

        protected string GetUnionClause(List<Joins> joins)
        {
            StringBuilder query = new StringBuilder();

            if ((joins == null || joins.Count == 0) &&
                (tables != null && tables.Count == 1))
            {
                query.Append(tables[0].Name);
            }
            else
            {
                if (joins.Count > 0)
                {
                    query.Append(joins[0].FirstTable);
                    query.Append(Environment.NewLine + joins[0].JoinType.ToString());
                    query.Append(Environment.NewLine + joins[0].SecondTable);
                }
                if (joins.Count > 1)
                {
                    query.Append(Environment.NewLine + joins[1].JoinType.ToString());
                    query.Append(Environment.NewLine + ((query.ToString().Contains(joins[1].FirstTable)) ? joins[1].SecondTable : joins[1].FirstTable).Trim());
                }

                if (joins.Count > 2)
                {
                    query.Append(Environment.NewLine + joins[2].JoinType.ToString());
                    query.Append(Environment.NewLine + ((query.ToString().Contains(joins[2].FirstTable)) ? joins[2].SecondTable : joins[2].FirstTable).Trim());
                }

                if (joins.Count > 3)
                {
                    query.Append(Environment.NewLine + joins[3].JoinType.ToString());
                    query.Append(Environment.NewLine + ((query.ToString().Contains(joins[3].FirstTable)) ? joins[3].SecondTable : joins[3].FirstTable).Trim());
                }
            }
            return query.ToString();
        }

        private string GetFormClauseForUnion(List<Joins> joins, bool isHtml)
        {
            StringBuilder query = new StringBuilder();

            if ((joins == null || joins.Count == 0) &&
                (tables != null && tables.Count == 1))
            {
                query.Append(tables[0].Name);
            }
            else
            {
                if (joins.Count > 0)
                {
                    query.Append(joins[0].FirstTable + " ");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;" : Environment.NewLine + "  ") + "<b>" + Constants.Union + "</b>");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "    ") + joins[0].SecondTable);
                }
                if (joins.Count > 1)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;" : Environment.NewLine + "  ") + "<b>" + Constants.Union + "</b>");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "    ") + ((query.ToString().Contains(joins[1].FirstTable)) ? joins[1].SecondTable : joins[1].FirstTable).Trim());
                }

                if (joins.Count > 2)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;" : Environment.NewLine + "        ") + "<b>" + Constants.Union + "</b>");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "    ") + ((query.ToString().Contains(joins[2].FirstTable)) ? joins[2].SecondTable : joins[2].FirstTable).Trim());
                }
                if (joins.Count > 3)
                {
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;" : Environment.NewLine + "  ") + "<b>" + Constants.Union + "</b>");
                    query.Append((isHtml ? "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" : Environment.NewLine + "    ") + ((query.ToString().Contains(joins[3].FirstTable)) ? joins[3].SecondTable : joins[3].FirstTable).Trim());
                }
            }
            return query.ToString();
        }

        #region Data Binding Method of Missing Module Column grid
        protected void missingColumnsGrid_DataBinding(object sender, EventArgs e)
        {
            missingColumnsGrid.DataSource = objQueryHelperManager.GetMissingModuleColumns(dictMissingColumns);
        }
        #endregion Data Binding Method of Missing Module Column grid

        #region Method to Remove Missing Module Columns from Tables
        protected void RemoveMissingColumnsFromTable()
        {
            if (columns == null || columns.Count == 0 || tables == null || tables.Count == 0)
                return;

            bool refreshAllFilters = false;
            missingColumns = string.Empty;

            foreach (TableInfo table in tables)
            {
                List<Utility.ColumnInfo> missingColumnsList = table.Columns.Where(x => !columns.Exists(y => y.TableName == x.TableName && y.FieldName == x.FieldName)).ToList();

                if (missingColumnsList == null || missingColumnsList.Count == 0)
                    continue;

                // removing missing columns from the current table
                table.Columns = table.Columns.Where(x => !missingColumnsList.Exists(y => y.TableName == x.TableName && y.FieldName == x.FieldName)).ToList();
                refreshAllFilters = true;
            }

            if (!refreshAllFilters)
            {
                bool hasMissingCol = groupBy.Any(x => !columns.Exists(y => y.TableName == x.Column.TableName && y.FieldName == x.Column.FieldName))
                    || orderBy.Any(x => !columns.Exists(y => y.TableName == x.Column.TableName && y.FieldName == x.Column.FieldName))
                    || selectedtotals.Any(x => !columns.Exists(y => y.TableName == x.TableName && y.FieldName == x.FieldName));

                if (hasMissingCol)
                    refreshAllFilters = true;


                // Check if where clause has some missing columns to get them removed
                if (!hasMissingCol)
                {
                    List<string> columnsPresentInWhereClause = new List<string>();

                    bool hasWhereClause = whereClauses != null && whereClauses.Count > 0;

                    if (hasWhereClause && isUnion)
                        columnsPresentInWhereClause = whereClauses.AsEnumerable().Select(x => x.ColumnName).ToList();
                    else if (hasWhereClause && !isUnion)
                        columnsPresentInWhereClause = whereClauses.AsEnumerable().Select(x => x.ColumnName.Split('.')[1]).ToList();

                    if (columnsPresentInWhereClause != null && columnsPresentInWhereClause.Count > 0)
                        columnsPresentInWhereClause = columnsPresentInWhereClause.Where(x => !columns.Exists(y => y.FieldName == x)).ToList();

                    if (columnsPresentInWhereClause.Count > 0)
                        refreshAllFilters = true;
                }
            }

            // Remove missing columns from all filters of query
            if (refreshAllFilters)
            {
                if (whereClauses != null && whereClauses.Count > 0)
                {
                    if (isUnion)
                        whereClauses = whereClauses.Where(x => columns.Exists(y => y.FieldName == x.ColumnName)).ToList();
                    else
                        whereClauses = whereClauses.Where(x => columns.Exists(y => y.FieldName == x.ColumnName.Split('.')[1])).ToList();

                    BindWhereClause();
                }

                groupBy = groupBy.Where(x => columns.Exists(y => y.TableName == x.Column.TableName && y.FieldName == x.Column.FieldName)).ToList();
                orderBy = orderBy.Where(x => columns.Exists(y => y.TableName == x.Column.TableName && y.FieldName == x.Column.FieldName)).ToList();
                selectedtotals = selectedtotals.Where(x => columns.Exists(y => y.TableName == x.TableName && y.FieldName == x.FieldName)).ToList();
                FillTotals();
            }

        }
        #endregion Method to Remove Missing Module Columns from Tables

        #region Method to Validate Query to find the Missing Columns
        /// <summary>
        /// Method to Validate Query to find the Missing Columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            // Find the columns details from the query which aren't available in Request Lists
            dictMissingColumns = objQueryHelperManager.GetMissingModuleColumns(panel);

            // Check if the query has missing columns and assign the value to QueryInfo.MissingColumns on it's basis
            bool hasMissingColumns = dictMissingColumns != null && dictMissingColumns.Any(x => x.Value.Count > 0);

            if (hasMissingColumns)
            {
                missingColumns = objQueryHelperManager.GetMissingColInString(dictMissingColumns);
                SaveQueryData();

                missingColumnsGrid.DataBind();
                missingColumnsContainer.ShowOnPageLoad = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(missingColumns))
                {
                    missingColumns = string.Empty;
                    SaveQueryData();
                }

                if (dictMissingColumns != null && dictMissingColumns.Count > 0)
                    dictMissingColumns.Clear();

                missingColumnsGrid.DataBind();
                noColFoundContainer.Visible = true;
                missingColGridContainer.Visible = false;
                missingColumnsGrid.Visible = false;
                missingColumnsContainer.HeaderStyle.ForeColor = Color.Green;
                missingColumnsContainer.PopupVerticalOffset = 100;
                missingColumnsContainer.ShowOnPageLoad = true;
            }
        }
        #endregion Method to Validate Query to find the Missing Columns

        /// <summary>
        /// This method is used to enable/disable grouping on TableName column in Columns & Totoals tab
        /// </summary>
        /// <param name="gridView"></param>
        protected void ManageGrouping(ASPxGridView gridView)
        {
            if (tables == null || tables.Count == 0 || gridView == null || gridView.Columns == null)
                return;

            GridViewDataColumn dataColumn = gridView.Columns["TableName"] as GridViewDataColumn;
            if (dataColumn == null)
                return;

            if (tables.Count == 1 || isUnion)        // Remove Grouping
            {
                gridView.SettingsBehavior.AllowGroup = false;
                ((GridViewDataColumn)gridView.Columns["TableName"]).UnGroup(); 
                dataColumn.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
                dataColumn.Visible = false;
                dataColumn.GroupIndex = -1;
            }
            else        // Add Grouping
            {
                gridView.SettingsBehavior.AllowGroup = true;
                dataColumn.Settings.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
                dataColumn.Visible = true;
                dataColumn.GroupIndex = 0;
            }
        }
    }
}
