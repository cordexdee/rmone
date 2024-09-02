using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using System.Xml;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class DashboardKPI : UserControl
    {
        protected bool errorInEditDashboardGroup;
        private long dashboardID;
        private int templateID;
        private string dashboardTitle = string.Empty;
        private string dimensionFactTable = string.Empty;
        private string expressionFactTable = string.Empty;
        private string basicFactTable = string.Empty;
        private List<FactTableField> factTableFields;
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        protected DataTable modulesTable;
        protected DataTable expressionsTable;
        protected DataTable filtersTable;
        string defaultColor = "#FF7F7F";
        string defaultFontColor = "#000000";
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {

            tabMenu.ActiveTab = tabMenu.Tabs[0];
            hTabNumber.Value = "1";
            if (!IsPostBack && Request["factTable"] != null)
            {
                basicFactTable = expressionFactTable = dimensionFactTable = Request["factTable"].Trim();
            }
            uHelper.BindOrderDropDown(cmbOrder, _context);
            BindPanelViewType();
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get parameter from request
                long.TryParse(Request["dashboardID"], out dashboardID);
                int.TryParse(Request["templateID"], out templateID);
                hDashboardID.Value = dashboardID.ToString();
                if (Request["dashboardTitle"] != null)
                {
                    dashboardTitle = Request["dashboardTitle"].Trim();
                }
                FillNonModuleColumn();
                //Get dashboard from template and save it 
                if (templateID > 0)
                {
                    DataTable chartTemplate = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ChartTemplates, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
                    DataRow item = chartTemplate.Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, templateID))[0];
                    string xml = Convert.ToString(item[DatabaseObjects.Columns.ChartObject]);
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(xml);
                    Dashboard dashboard = new Dashboard();
                    dashboard = (Dashboard)uHelper.DeSerializeAnObject(xmlDoc, dashboard);

                    //Get dashboard which is comming from query parameter if any
                    if (dashboardTitle != string.Empty)
                    {
                        dashboard.Title = dashboardTitle;
                        if (dashboard.panel != null)
                        {
                            dashboard.panel.ContainerTitle = dashboardTitle;
                        }
                    }
                    dashboard.ID = 0;
                    if (dashboard.panel != null)
                    {
                        dashboard.panel.DashboardID = Guid.NewGuid();
                    }

                    //We are saving in get mode so we need to set allowunsafeupdates property to true.
                    //.AllowUnsafeUpdates = true;

                    byte[] iconContents = new byte[0];
                    string fileName = string.Empty;
                    dManager.SaveDashboardPanel(iconContents, fileName, false, dashboard);
                    //.AllowUnsafeUpdates = false;

                    //Set new dashboardID in dashboard hidden field for further use and set global filter dashboardID
                    hDashboardID.Value = dashboard.ID.ToString();
                    dashboardID = dashboard.ID;
                }
            }
            else
            {
                long.TryParse(hDashboardID.Value.Trim(), out dashboardID);
            }
        }

        #region  events
        protected void CblDashboardGroups_PreRender(object sender, EventArgs e)
        {

        }

        protected void DashboardGroup_Init(object sender, EventArgs e)
        {


        }

        protected override void OnPreRender(EventArgs e)
        {
            Dashboard uDashboard = null;
            long.TryParse(hDashboardID.Value, out dashboardID);
            if (Request.QueryString.AllKeys.Contains("dashboardTitle"))
            {
                dashboardTitle = Request["dashboardTitle"].Trim();
            }
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
            }

            //Ask maxlimit value if stop Auto scaling is true;
            txtMaxLimitContainer.Visible = false;
            if (cbStopAutoScale.Checked)
            {
                txtMaxLimitContainer.Visible = true;
            }

            txtTitle.Text = dashboardTitle;
            hDashboardID.Value = dashboardID.ToString();

            if (uDashboard != null)
            {
                hDashboardID.Value = uDashboard.ID.ToString();

                //Basic info
                txtTitle.Text = uDashboard.Title.Trim();
                cbHideTitle.Checked = Convert.ToBoolean(uDashboard.IsHideTitle);
                txtDescription.Text = uDashboard.DashboardDescription.Trim();
                cbHideDesc.Checked = Convert.ToBoolean(uDashboard.IsHideDescription);
                cmbOrder.Value = uDashboard.ItemOrder;
                //veiw setting
                txtWidth.Text = uDashboard.PanelWidth.ToString();
                txtHeight.Text = uDashboard.PanelHeight.ToString();

                ddlDashboardTheme.SelectedValue = uDashboard.ThemeColor;

                if (!string.IsNullOrEmpty(uDashboard.FontStyle))
                {
                    string[] vals = uDashboard.FontStyle.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    if (vals.Length == 3)
                    {
                        ddlFontStyle.SelectedValue = vals[0];
                        ddlFontSize.SelectedValue = vals[1];
                        ceFont.Value = vals[2];
                    }
                }

                if (!string.IsNullOrEmpty(uDashboard.HeaderFontStyle))
                {
                    string[] vals = uDashboard.HeaderFontStyle.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    if (vals.Length == 3)
                    {
                        ddlHeaderFontStyle.SelectedValue = vals[0];
                        ddlHeaderFontSize.SelectedValue = vals[1];
                        ceHeaderFont.Value = vals[2];
                    }
                }


                PanelSetting cSetting = null;
                if (uDashboard.panel != null)
                {
                    cSetting = (PanelSetting)uDashboard.panel;
                    cbStopAutoScale.Checked = cSetting.StopAutoScale;
                    //Ask maxlimit value if stop Auto scaling is true;
                    txtMaxLimitContainer.Visible = false;
                    if (cbStopAutoScale.Checked)
                    {
                        txtMaxLimitContainer.Visible = true;
                    }

                    txtIconUrl.Text = cSetting.IconUrl;

                    //Show Dimension list
                    rprDimensions.DataSource = cSetting.Expressions;
                    rprDimensions.DataBind();
                    //set 12/12/2018 
                    ddlChartType.SelectedValue = cSetting.ChartType;
                    ddlPanelViewType.SelectedIndex = ddlPanelViewType.Items.IndexOf(ddlPanelViewType.Items.FindByValue(cSetting.PanelViewType));
                    txtCentreTitle.Text = cSetting.CentreTitle;
                    ddlDoughnutOnlyTextFormat.SelectedValue = cSetting.TextFormat;
                    ddlEnableToolTip.SelectedValue = cSetting.Legendvisible;

                    if (cSetting.ChartHide == 1)
                    {
                        CheckBoxHideChart.Checked = false; //BTS-22-000857
                    }
                    else
                    {
                        CheckBoxHideChart.Checked = true;
                    }

                }

            }

            base.OnPreRender(e);
        }

        #region General

        #region General Info    
        protected void BtnUpdateGeneralInfo_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            Dashboard uDashboard = null;
            PanelSetting dashboardPanel = null;
            //bool newDashboard = false;
            long.TryParse(hDashboardID.Value, out dashboardID);
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
                dashboardPanel = (PanelSetting)uDashboard.panel;
            }

            if (uDashboard == null)
            {
                uDashboard = new Dashboard();
                dashboardPanel = new PanelSetting();
                //newDashboard = true;
            }

            if (dashboardPanel == null)
            {
                dashboardPanel = new PanelSetting();
            }

            dashboardPanel.DashboardID = Guid.NewGuid();
            uDashboard.panel = dashboardPanel;
            uDashboard.DashboardType = DashboardType.Panel;
            dashboardPanel.type = DashboardType.Panel;
            if (cmbOrder.SelectedIndex == -1)
                uDashboard.ItemOrder = UGITUtility.StringToInt(cmbOrder.Items[cmbOrder.Items.Count - 1].Value);
            else
            {
                int order = Convert.ToInt32(cmbOrder.Value);
                uDashboard.ItemOrder = order > 1 ? order - 1 : order;
            }


            //Basic Info
            uDashboard.IsHideTitle = cbHideTitle.Checked;
            uDashboard.IsHideDescription = true;
            uDashboard.Title = txtTitle.Text;
            uDashboard.IsHideDescription = cbHideDesc.Checked;
            uDashboard.DashboardDescription = txtDescription.Text;
            dashboardPanel.ContainerTitle = txtTitle.Text.Trim();
            dashboardPanel.Description = txtDescription.Text.Trim();
            dashboardPanel.StopAutoScale = cbStopAutoScale.Checked;

            //View Dashboard

            if (uDashboard.PanelWidth <= 0)
            {
                uDashboard.PanelWidth = 300;
            }
            if (uDashboard.PanelHeight <= 0)
            {
                uDashboard.PanelHeight = 300;
            }

            dashboardPanel.IconUrl = txtIconUrl.Text.Trim();
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);

            //Save new dashboard id in hiddenfield and dashboardid variable
            hDashboardID.Value = uDashboard.ID.ToString();
            dashboardID = uDashboard.ID;

            uHelper.PerformAjaxPanelCallBack(Page, Context);

            //Get ID after save and preserve it into page
            hDashboardID.Value = uDashboard.ID.ToString();
            //uHelper.ClosePopUpAndEndResponse(Context,false,true);
            //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
        }
        #endregion
        #endregion
        #region Dimension KPI
        #region Edit Dimensions
        protected void DdlDashboardTable_Load(object sender, EventArgs e)
        {
            List<string> factTables = DashboardCache.DashboardFactTables(HttpContext.Current.GetManagerContext()); //added on 12-07-2018

            if (ddlDashboardTable.Items.Count <= 0)
            {
                foreach (string factTable in factTables)
                {
                    ddlDashboardTable.Items.Add(new ListItem(factTable, factTable));
                }
                ddlDashboardTable.Items.Insert(0, new ListItem("", ""));

                ddlDashboardTable.SelectedIndex = ddlDashboardTable.Items.IndexOf(ddlDashboardTable.Items.FindByValue(basicFactTable));

                FillFactTableFields(ddlFactTableFieldsForFilter, expressionFactTable);
                FillFactTableFields(dllAggragateFields, expressionFactTable);
                FillFactTableFields(ddlBasicDateFilterStartField, expressionFactTable, "DateTime");

            }
        }
        protected void DdlDashboardTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            expressionFactTable = ddlDashboardTable.SelectedValue;
            FillFactTableFields(ddlFactTableFieldsForFilter, expressionFactTable);
            FillFactTableFields(dllAggragateFields, expressionFactTable);
            FillFactTableFields(ddlBasicDateFilterStartField, expressionFactTable, "DateTime");
            txtFilter.Text = string.Empty;
            txtAggragateOf.Text = string.Empty;
        }
        private void FillFactTableFields(object sender, string factTable)
        {
            FillFactTableFields(sender, factTable, string.Empty);
        }
        private void FillFactTableFields(object sender, string factTable, string typeFilter)
        {
            DropDownList list = (DropDownList)sender;
            list.Items.Clear();

            if (factTable != null && factTable.Trim() != string.Empty)
            {
                factTableFields = new List<FactTableField>();
                factTableFields = DashboardCache.GetFactTableFields(HttpContext.Current.GetManagerContext(), factTable.Trim());
                if (factTableFields != null)
                {
                    if (!string.IsNullOrEmpty(typeFilter))
                        factTableFields = factTableFields.Where(x => x.DataType.ToLower() == "system.datetime").ToList();

                    factTableFields = factTableFields.OrderBy(x => x.FieldName).ToList();
                    foreach (FactTableField field in factTableFields)
                    {
                        ListItem item = new ListItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                        item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                        list.Items.Add(item);
                    }

                    list.Items.Insert(0, new ListItem("", ""));
                }
            }
        }

        protected void BtnEditDimension_Click(object sender, EventArgs e)
        {
            ClearEditDimensionForm();
            ImageButton button = (ImageButton)sender;
            Guid dimensionID = Guid.Empty;
            try
            {
                dimensionID = new Guid(button.CommandArgument);
            }
            catch
            {
            }

            if (dimensionID != Guid.Empty)
            {
                long.TryParse(hDashboardID.Value.Trim(), out dashboardID);
                Dashboard uDashboard = null;
                if (dashboardID > 0)
                {
                    uDashboard = dManager.LoadPanelById(dashboardID, false);
                }

                //return in case of no dashboard in edit mode
                if (uDashboard == null)
                {
                    return;
                }

                PanelSetting cPanel = (PanelSetting)uDashboard.panel;
                if (cPanel == null)
                {
                    //return when chart object in not exist in dashboard
                    return;
                }

                List<DashboardPanelLink> dimensions = cPanel.Expressions;

                //delete dimension
                DashboardPanelLink dimension = dimensions.FirstOrDefault(x => x.LinkID == dimensionID);
                FillEditDimensionForm(dimension);

            }
        }

        protected void BtnSaveDimension_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                // return in case of error
                return;
            }

            Guid dimensionID = Guid.Empty;
            try
            {
                dimensionID = new Guid(hEditDimension.Value.Trim());
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }


            long.TryParse(hDashboardID.Value.Trim(), out dashboardID);
            Dashboard uDashboard = null;
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);

            }
            else
            {
                uDashboard = new Dashboard();
                uDashboard.panel = new PanelSetting();
            }

            //return in case of no dashboard in edit mode
            if (uDashboard == null)
            {
                return;
            }
            DashboardPanel panel = new DashboardPanel();

            PanelSetting chartPanel = (PanelSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<DashboardPanelLink> dimensions = chartPanel.Expressions;
            if (dimensions == null)
            {
                dimensions = new List<DashboardPanelLink>();
            }

            //check whether existing dimension edited or new dimension is added
            DashboardPanelLink dimension = null;
            if (dimensionID != Guid.Empty)
            {
                dimension = dimensions.FirstOrDefault(x => x.LinkID == dimensionID);
            }
            else
            {
                dimension = new DashboardPanelLink();
                dimensions.Add(dimension);
            }
            uDashboard.Title = txtTitle.Text;
            //Save dimension
            dimension.Title = dpLinkTitle.Text.Trim();
            dimension.HideTitle = cbHideKpiTitle.Checked;
            dimension.ExpressionFormat = dpLinkName.Text.Trim();
            int order = 0;
            int.TryParse(dpLinkOrder.Text.Trim(), out order);

            double maxLimit = 0;
            double.TryParse(txtMaxLimit.Text.Trim(), out maxLimit);
            if (maxLimit <= 0)
            {
                maxLimit = 100;
            }
            dimension.MaxLimit = maxLimit;
            dimension.BarUnit = txtBarUnit.Text.Trim();
            dimension.DashboardTable = ddlDashboardTable.SelectedValue;
            dimension.Order = order;
            dimension.AggragateFun = ddlDimensionAggragate.SelectedValue;
            dimension.AggragateOf = txtAggragateOf.Text;
            dimension.IsPct = cbCalculatePct.Checked;
            dimension.Filter = txtFilter.Text;
            dimension.ShowColumns_Category = slNonModuleColumns.Value.Trim();

            dimension.StopLinkDetail = slDrillDownType.Value.Trim() == "No Drill Down" ? true : false;

            dimension.DateFilterDefaultView = ddlBasicDateFilterDefaultView.SelectedValue;
            dimension.DateFilterStartField = ddlBasicDateFilterStartField.SelectedValue;

            dimension.DefaultLink = slDrillDownType.Value.Trim() == "Default Drill Down" ? true : false;
            if (!dimension.DefaultLink)
            {
                dimension.LinkUrl = dpLinkUrl.Text.Trim();
            }

            dimension.IsHide = dpHideKPI.Checked;
            dimension.UseAsPanel = dpUseAsPanel.Checked;


            if (dpPopupView.Checked)
            {
                dimension.ScreenView = 1;
            }
            else
            {
                dimension.ScreenView = 0;
            }
            dimension.ShowBar = false;
            if (cbShowBar.Checked)
            {
                dimension.ShowBar = true;
                dimension.BarDefaultColor = txtBarDefaulColor.Text;
                dimension.FontColor = txtFontColor.Text;
                dimension.Conditions = new List<KPIBarCondition>();
                if (txtCond1Bar.Text.Trim() != string.Empty)
                {
                    KPIBarCondition cond1 = new KPIBarCondition();
                    cond1.Score = Convert.ToDouble(txtCond1Bar.Text.Trim());
                    cond1.Operator = txtCond1Operator.SelectedValue;
                    cond1.Color = txtCond1Color.Text.Trim();
                    dimension.Conditions.Add(cond1);
                }
                if (txtCond2Bar.Text.Trim() != string.Empty)
                {
                    KPIBarCondition cond1 = new KPIBarCondition();
                    cond1.Score = Convert.ToDouble(txtCond2Bar.Text.Trim());
                    cond1.Operator = txtCond2Operator.SelectedValue;
                    cond1.Color = txtCond2Color.Text.Trim();
                    dimension.Conditions.Add(cond1);
                }
                if (txtCond3Bar.Text.Trim() != string.Empty)
                {
                    KPIBarCondition cond1 = new KPIBarCondition();
                    cond1.Score = Convert.ToDouble(txtCond3Bar.Text.Trim());
                    cond1.Operator = txtCond3Operator.SelectedValue;
                    cond1.Color = txtCond3Color.Text.Trim();
                    dimension.Conditions.Add(cond1);
                }
                if (txtCond4Bar.Text.Trim() != string.Empty)
                {
                    KPIBarCondition cond1 = new KPIBarCondition();
                    cond1.Score = Convert.ToDouble(txtCond4Bar.Text.Trim());
                    cond1.Operator = txtCond4Operator.SelectedValue;
                    cond1.Color = txtCond4Color.Text.Trim();
                    dimension.Conditions.Add(cond1);
                }
            }

            chartPanel.Expressions = dimensions.OrderBy(x => x.Order).ToList();
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dashboardID = dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
            ClearEditDimensionForm();
            if (dashboardID > 0)
                hDashboardID.Value = UGITUtility.ObjectToString(dashboardID);
            var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
            if (!string.IsNullOrWhiteSpace(refreshParentID))
            {
                UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
            }
        }
        private void FillNonModuleColumn()
        {
            DataTable myModuleColumns = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleColumns, $"{DatabaseObjects.Columns.TenantID}='{_context.TenantID}'");
            if (myModuleColumns.Rows.Count > 0)
            {
                DataTable dtMyModuleColumns = myModuleColumns;
                dtMyModuleColumns.DefaultView.Sort = DatabaseObjects.Columns.CategoryName + " ASC";
                dtMyModuleColumns = dtMyModuleColumns.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.CategoryName });

                foreach (DataRow moduleRow in dtMyModuleColumns.Rows)
                {
                    slNonModuleColumns.Items.Add(new ListItem { Text = Convert.ToString(moduleRow[DatabaseObjects.Columns.CategoryName]), Value = Convert.ToString(moduleRow[DatabaseObjects.Columns.CategoryName]) });
                }
                slNonModuleColumns.Items.Insert(0, new ListItem(""));
                slNonModuleColumns.DataBind();
            }
        }
        private void FillEditDimensionForm(DashboardPanelLink dimension)
        {
            if (dimension == null)
            {
                return;
            }
            string bgColor = string.Empty;

            ddlDashboardTable.SelectedIndex = ddlDashboardTable.Items.IndexOf(ddlDashboardTable.Items.FindByValue(dimension.DashboardTable));
            basicFactTable = dimension.DashboardTable;
            FillFactTableFields(ddlFactTableFieldsForFilter, basicFactTable);
            FillFactTableFields(dllAggragateFields, basicFactTable);
            FillFactTableFields(ddlBasicDateFilterStartField, basicFactTable, "DateTime");

            hEditDimension.Value = dimension.LinkID.ToString();
            dpLinkTitle.Text = dimension.Title;
            cbHideKpiTitle.Checked = dimension.HideTitle;
            txtBarUnit.Text = dimension.BarUnit;
            dpLinkName.Text = dimension.ExpressionFormat;
            txtMaxLimit.Text = dimension.MaxLimit.ToString();
            dpLinkUrl.Text = dimension.LinkUrl;
            dpHideKPI.Checked = dimension.IsHide;
            dpUseAsPanel.Checked = dimension.UseAsPanel;
            dpLinkOrder.Text = dimension.Order.ToString();
            if (dimension.ScreenView == 1)
            {
                dpPopupView.Checked = true;
                dpWindowView.Checked = false;
            }
            else
            {
                dpWindowView.Checked = true;
                dpPopupView.Checked = false;
            }

            if (dimension.StopLinkDetail)
            {
                slDrillDownType.Value = "No Drill Down";
                dpLinkUrl.Style.Add("display", "none");
            }
            else if (dimension.DefaultLink)
            {
                slDrillDownType.Value = "Default Drill Down";
                dpLinkUrl.Style.Add("display", "none");
            }
            else
            {
                slDrillDownType.Value = "Custom Url";
                dpLinkUrl.Style.Add("display", "inline-block");
            }

            slNonModuleColumns.Value = dimension.ShowColumns_Category;
            if (slDrillDownType.Value == "Default Drill Down")
                slNonModuleColumns.Style.Add("display", "inline-block");
            else
                slNonModuleColumns.Style.Add("display", "none");

            //cbDefaultLink.Checked = dimension.DefaultLink;
            cbShowBar.Checked = dimension.ShowBar;
            pShowBar.Visible = false;
            if (cbShowBar.Checked)
            {
                pShowBar.Visible = true;
            }
            txtFilter.Text = dimension.Filter;

            ddlBasicDateFilterStartField.SelectedIndex = ddlBasicDateFilterStartField.Items.IndexOf(ddlBasicDateFilterStartField.Items.FindByValue(dimension.DateFilterStartField));
            ddlBasicDateFilterDefaultView.SelectedIndex = ddlBasicDateFilterDefaultView.Items.IndexOf(ddlBasicDateFilterDefaultView.Items.FindByValue(dimension.DateFilterDefaultView));

            txtAggragateOf.Text = dimension.AggragateOf;
            ddlDimensionAggragate.SelectedIndex = ddlDimensionAggragate.Items.IndexOf(ddlDimensionAggragate.Items.FindByValue(dimension.AggragateFun));
            cbCalculatePct.Checked = dimension.IsPct;

            txtFontColor.Text = dimension.FontColor;
            bgColor = dimension.FontColor;
            if (!dimension.FontColor.StartsWith("#"))
                bgColor = "#" + dimension.FontColor;
            txtFontColor.Text = bgColor;



            txtBarDefaulColor.Text = dimension.BarDefaultColor;
            bgColor = dimension.BarDefaultColor;
            if (!dimension.BarDefaultColor.StartsWith("#"))
                bgColor = "#" + dimension.BarDefaultColor;
            txtBarDefaulColor.Text = bgColor;

            if (dimension.Conditions.Count >= 1)
            {
                KPIBarCondition condition = dimension.Conditions[0];
                txtCond1Bar.Text = condition.Score.ToString();
                txtCond1Operator.SelectedIndex = txtCond1Operator.Items.IndexOf(txtCond1Operator.Items.FindByValue(condition.Operator));
                txtCond1Color.Text = condition.Color;
                bgColor = condition.Color;
                if (!condition.Color.StartsWith("#"))
                    bgColor = "#" + condition.Color;
                txtCond1Color.Text = bgColor;
            }
            if (dimension.Conditions.Count >= 2)
            {
                KPIBarCondition condition = dimension.Conditions[1];
                txtCond2Bar.Text = condition.Score.ToString();
                txtCond2Operator.SelectedIndex = txtCond1Operator.Items.IndexOf(txtCond1Operator.Items.FindByValue(condition.Operator));
                txtCond2Color.Text = condition.Color;
                bgColor = condition.Color;
                if (!condition.Color.StartsWith("#"))
                    bgColor = "#" + condition.Color;
                txtCond2Color.Text = bgColor;
            }

            if (dimension.Conditions.Count >= 3)
            {
                KPIBarCondition condition = dimension.Conditions[2];
                txtCond3Bar.Text = condition.Score.ToString();
                txtCond3Operator.SelectedIndex = txtCond1Operator.Items.IndexOf(txtCond1Operator.Items.FindByValue(condition.Operator));
                txtCond3Color.Text = condition.Color;
                bgColor = condition.Color;
                if (!condition.Color.StartsWith("#"))
                    bgColor = "#" + condition.Color;
                txtCond3Color.Text = bgColor;
            }

            if (dimension.Conditions.Count >= 4)
            {
                KPIBarCondition condition = dimension.Conditions[3];
                txtCond4Bar.Text = condition.Score.ToString();
                txtCond4Operator.SelectedIndex = txtCond1Operator.Items.IndexOf(txtCond1Operator.Items.FindByValue(condition.Operator));
                txtCond4Color.Text = condition.Color;
                bgColor = condition.Color;
                if (!condition.Color.StartsWith("#"))
                    bgColor = "#" + condition.Color;
                txtCond4Color.Text = bgColor;
            }
        }
        private void ClearEditDimensionForm()
        {
            hEditDimension.Value = string.Empty;
            ddlDashboardTable.ClearSelection();
            dpLinkTitle.Text = string.Empty;
            dpLinkName.Text = string.Empty;
            dpLinkOrder.Text = "0";
            dpHideKPI.Checked = false;
            dpLinkUrl.Text = string.Empty;
            dpPopupView.Checked = true;
            dpUseAsPanel.Checked = false;
            txtAggragateOf.Text = string.Empty;
            cbCalculatePct.Checked = false;
            txtFilter.Text = string.Empty;
            txtMaxLimit.Text = "100";
            txtBarDefaulColor.Text = defaultColor;
            slDrillDownType.SelectedIndex = 0;
            cbShowBar.Checked = false;
            pShowBar.Visible = false;
            txtCond1Bar.Text = string.Empty;
            txtCond1Color.Text = defaultColor;
            txtCond1Operator.ClearSelection();
            txtCond2Bar.Text = string.Empty;
            txtCond2Color.Text = defaultColor;
            txtCond2Operator.ClearSelection();
            txtCond3Bar.Text = string.Empty;
            txtCond3Color.Text = defaultColor;
            txtCond3Operator.ClearSelection();
            txtCond4Bar.Text = string.Empty;
            txtCond4Color.Text = defaultColor;
            txtCond4Operator.ClearSelection();
            txtFontColor.Text = defaultFontColor;
            ddlBasicDateFilterDefaultView.ClearSelection();
            ddlBasicDateFilterStartField.ClearSelection();
            cbHideKpiTitle.Checked = true;
        }
        protected void BtnResetDimension_Click(object sender, EventArgs e)
        {
            ClearEditDimensionForm();
        }
        protected void BtnDeleteDimension_Click(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;

            Guid dimensionID = Guid.Empty;
            try
            {
                dimensionID = new Guid(button.CommandArgument.Trim());
            }
            catch
            {
            }

            if (dimensionID != Guid.Empty)
            {
                long.TryParse(hDashboardID.Value.Trim(), out dashboardID);
                Dashboard uDashboard = null;
                if (dashboardID > 0)
                {
                    uDashboard = dManager.LoadPanelById(dashboardID, false);
                }

                //return in case of no dashboard in edit mode
                if (uDashboard == null)
                {
                    return;
                }

                PanelSetting cPanel = (PanelSetting)uDashboard.panel;
                if (cPanel == null)
                {
                    //return when chart object in not exist in dashboard
                    return;
                }

                List<DashboardPanelLink> dimensions = cPanel.Expressions;

                //delete dimension
                DashboardPanelLink dimension = dimensions.FirstOrDefault(x => x.LinkID == dimensionID);
                if (dimension != null)
                {
                    dimensions.Remove(dimension);
                    byte[] iconContents = new byte[0];
                    string fileName = string.Empty;
                    dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
                    //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
                }
            }
        }

        protected void DPModuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void DPModuleType_Load(object sender, EventArgs e)
        {
        }

        protected void DPExpression_PreRender(object sender, EventArgs e)
        {
            // LoadExpression(dpExpression);
        }

        protected void DPModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void LoadExpression(object sender)
        {
            DropDownList list = (DropDownList)sender;
            list.Items.Clear();
            if (list.Items.Count <= 0)
            {
                if (expressionsTable != null && expressionsTable.Rows.Count > 0)
                {
                    foreach (DataRow item in expressionsTable.Rows)
                    {
                        list.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                }
            }
        }

        private void LoadModulesByType(object sender, ModuleType type)
        {
            DropDownList dlist = (DropDownList)sender;
            //DataTable moudles =uGITCache.GetModuleList(type);
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            List<UGITModule> moudles = moduleViewManager.Load(x => x.ModuleType == type);

            dlist.Items.Clear();
            if (modulesTable != null && moudles.Count > 0)
            {
                foreach (UGITModule module in moudles)
                {
                    dlist.Items.Add(new ListItem(module.ModuleName, module.ModuleName));
                }
                dlist.Items.Insert(0, new ListItem("All", "0"));
            }
        }

        private void LoadDashboardFilter(object sender, ModuleType moduleType, string moduleName)
        {
            string query = string.Empty;
            if (moduleType != ModuleType.All && !string.IsNullOrEmpty(moduleName) && !moduleName.Trim().Equals("0", StringComparison.CurrentCultureIgnoreCase))
            {
                query = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.ModuleType, moduleType.ToString(), DatabaseObjects.Columns.ModuleNameLookup, moduleName);
            }
            else if (moduleType != ModuleType.All && (string.IsNullOrEmpty(moduleName) || moduleName.Trim().Equals("0", StringComparison.CurrentCultureIgnoreCase)))
            {
                query = string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleType, moduleType.ToString());
            }
            else if (moduleType == ModuleType.All && !string.IsNullOrEmpty(moduleName) && moduleName.Trim().Equals("0", StringComparison.CurrentCultureIgnoreCase))
            {
                query = string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleType, DatabaseObjects.Columns.ModuleNameLookup);
            }

            DropDownList list = (DropDownList)sender;
            list.Items.Clear();
            DataRow[] formulies = filtersTable.Select(query);
            if (formulies.Length > 0)
            {
                if (formulies != null)
                {
                    foreach (DataRow item in formulies)
                    {
                        list.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                }
            }
        }

        protected string GetDashboardFormulaName(int formulaID)
        {
            DataRow[] rows = filtersTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, formulaID));
            if (rows.Length > 0)
            {
                return Convert.ToString(rows[0][DatabaseObjects.Columns.Title]);
            }
            return string.Empty;
        }

        protected string GetDashboardExpressionName(int ExpressionID)
        {
            DataRow[] rows = expressionsTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, ExpressionID));
            if (rows.Length > 0)
            {
                return Convert.ToString(rows[0][DatabaseObjects.Columns.Title]);
            }
            return string.Empty;
        }
        protected void DdlBasicDateFilterDefaultView_Load(object sender, EventArgs e)
        {
            if (ddlBasicDateFilterDefaultView.Items.Count <= 0)
            {
                List<string> dateViews = DashboardCache.GetDateViewList();
                foreach (string item in dateViews)
                {
                    ddlBasicDateFilterDefaultView.Items.Add(new ListItem(item, item));
                }
                ddlBasicDateFilterDefaultView.Items.Insert(0, new ListItem("Select All", string.Empty));
            }
        }

        protected void CBShowBar_CheckedChanged(object sender, EventArgs e)
        {
            pShowBar.Visible = false;
            if (cbShowBar.Checked)
            {
                pShowBar.Visible = true;
            }
        }
        #endregion

        #endregion

        #region Preview
        #region Appearance
        protected void BtSaveApperanceChanges_Click(object sender, EventArgs e)
        {
            Dashboard uDashboard = null;
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
            }
            //return in case of no dashboard in edit mode
            if (uDashboard == null)
            {
                return;
            }
            PanelSetting chartPanel = (PanelSetting)uDashboard.panel;
            if (CheckBoxHideChart.Checked == true)
            {
                chartPanel.ChartHide = 0; //BTS-22-000857
            }
            else
            {
                chartPanel.ChartHide = 1;
            }
            chartPanel.ChartType = ddlChartType.SelectedValue.ToString();
            chartPanel.PanelViewType = ddlPanelViewType.SelectedValue.ToString();
            chartPanel.HorizontalAlignment = ddlDoughnutOnlyHorizontalPosition.SelectedValue.ToString();
            chartPanel.VerticalAlignment = ddlDoughnutOnlyVerticalPosition.SelectedValue.ToString();
            chartPanel.TextFormat = ddlDoughnutOnlyTextFormat.SelectedValue.ToString();
            chartPanel.Legendvisible = ddlEnableToolTip.SelectedValue.ToString();
            chartPanel.CentreTitle = txtCentreTitle.Text;


            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }
            uDashboard.PanelWidth = Convert.ToInt32(txtWidth.Text.Trim());
            uDashboard.PanelHeight = Convert.ToInt32(txtHeight.Text.Trim());
            uDashboard.ThemeColor = ddlDashboardTheme.SelectedValue;
            uDashboard.FontStyle = string.Format("{0};#{1};#{2}", ddlFontStyle.SelectedValue, ddlFontSize.SelectedValue, string.Format("#{0}", ceFont.Color.Name.Substring(2)));
            uDashboard.HeaderFontStyle = string.Format("{0};#{1};#{2}", ddlHeaderFontStyle.SelectedValue, ddlHeaderFontSize.SelectedValue, string.Format("#{0}", ceHeaderFont.Color.Name.Substring(2)));
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
            //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
        }

        #endregion
        #region Preview
        protected void DashboardPreview_PreRender(object sender, EventArgs e)
        {
            long.TryParse(hDashboardID.Value.Trim(), out dashboardID);
            Dashboard uDashboard = null;
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
            }

            //return if dashboard not exist
            if (uDashboard == null)
            {
                return;
            }

            dashboardPreview.Attributes.Add("onLoad", "loadPreview()");
            CustomDashboardPanel panel = (CustomDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomDashboardPanel.ascx");
            panel.dashboard = uDashboard;
            panel.UseAjax = true;

            dashboardPreview.Controls.Add(panel);
        }
        #endregion
        #region save template
        protected void BtSaveAsTemplate_Click(object sender, EventArgs e)
        {
            Dashboard uDashboard = null;
            long.TryParse(hDashboardID.Value, out dashboardID);
            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
            }

            if (uDashboard != null)
            {
                bool createNew = true;
                ChartTemplate item = null;
                ChartTemplatesManager chartTemplateMGR = new ChartTemplatesManager(HttpContext.Current.GetManagerContext());
                List<ChartTemplate> lstChartTemplates = chartTemplateMGR.Load();
                //DataTable chartTemplate = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ChartTemplates);

                long templateID = 0;
                long.TryParse(ddlTemplateList.SelectedValue.Trim(), out templateID);
                if (templateID > 0)
                {
                    item = lstChartTemplates.FirstOrDefault(x => x.ID == templateID);
                    createNew = false;
                }
                else
                {
                    item = new ChartTemplate();
                }
                uDashboard.ID = 0;
                System.Xml.XmlDocument xmlDoc = uHelper.SerializeObject((object)uDashboard);

                item.Title = uDashboard.Title;
                item.ChartObject = xmlDoc.OuterXml;
                item.TemplateType = "0";
                if (item.ID > 0)
                    chartTemplateMGR.Update(item);
                else
                    chartTemplateMGR.Insert(item);

                if (createNew)
                {
                    LoadTemplateList();
                }

                ListItem templateItem = ddlTemplateList.Items.FindByValue(Convert.ToString(item.ID));
                if (templateItem != null)
                {
                    ddlTemplateList.SelectedIndex = ddlTemplateList.Items.IndexOf(templateItem);
                }
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

        private void LoadTemplateList()
        {
            DataTable templateTable = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ChartTemplates, $"TenantID='{_context.TenantID}'");
            ddlTemplateList.Items.Clear();
            if (templateTable != null && templateTable.Rows.Count > 0)
            {
                foreach (DataRow item in templateTable.Rows)
                {
                    if (Convert.ToString(item[DatabaseObjects.Columns.TemplateType]) == ((int)DashboardType.Panel).ToString())
                    {
                        ddlTemplateList.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Title]), Convert.ToString(item[DatabaseObjects.Columns.Id])));
                    }
                }
            }
            ddlTemplateList.Items.Insert(0, new ListItem("--New Template--", "0"));
        }

        #endregion

        #endregion


        public void BindPanelViewType()
        {
            ddlPanelViewType.Items.Clear();
            ddlPanelViewType.Items.Add(new ListItem("Default", ((Enum)PanelViewType.Default).ToString()));
            ddlPanelViewType.Items.Add(new ListItem("Bars", ((Enum)PanelViewType.Bars).ToString()));
            ddlPanelViewType.Items.Add(new ListItem("Circular", ((Enum)PanelViewType.Circular).ToString()));
            ddlPanelViewType.DataBind();

        }
        #endregion
      
       
        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
