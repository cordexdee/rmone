using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Manager;
using System.Drawing;

namespace uGovernIT.Web
{
    public partial class NewDashboardChartUI : UserControl
    {

        public string DashboardName = string.Empty;
        public string DataSource = string.Empty;
        public string TamplateID = string.Empty;
        public string Sourceurl = string.Empty;

        protected bool errorInEditDashboardGroup;
        private long dashboardID;
        private int templateID;
        public string chartUrl = string.Empty;
        //private int TamplateID;
        //private string DashboardName = string.Empty;
        //private string DataSource = string.Empty;
        private List<FactTableField> factTableFields;
        Dashboard uDashboard;
        TextBox txtFuntionValue;
        List<string> varience;
        private string expressionOperator;
        ChartTemplatesManager objChartTemplatesManager = new ChartTemplatesManager(HttpContext.Current.GetManagerContext());
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());

        #region Global Events
        protected override void OnInit(EventArgs e)
        {
            tabMenu.ActiveTab = tabMenu.Tabs[0];
            hTabName.Value = "general";

            int.TryParse(TamplateID, out templateID);
            if (!IsPostBack)
            {
                ////Get parameter from request
                long.TryParse(Request["dashboardID"], out dashboardID);
                int.TryParse(Request["templateID"], out templateID);
                hDashboardID.Value = dashboardID.ToString();

                if (Request["factTable"] != null)
                {
                    DataSource = Request["factTable"].Trim();
                }
                if (Request["dashboardTitle"] != null)
                {
                    DashboardName = Request["dashboardTitle"].Trim();
                }

                //Get dashboard from template and save it 
                if (templateID > 0)
                {
                    //DataTable chartTemplate = objChartTemplatesManager.GetDataTable();//[DatabaseObjects.Lists.ChartTemplates];
                    ChartTemplate item = objChartTemplatesManager.LoadByID(templateID);
                    //DataRow item = objChartTemplatesManager.GetDataTable().Select(string.Format("{0}={1}", DatabaseObjects.Columns.ID, templateID))[0];
                    string xml = "";
                    if (item != null)
                        xml = item.ChartObject;
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(xml);
                    Dashboard dashboard = new Dashboard();
                    dashboard = (Dashboard)uHelper.DeSerializeAnObject(xmlDoc, dashboard);

                    //Get dashboard which is comming from query parameter if any
                    if (DashboardName != string.Empty)
                    {
                        dashboard.Title = DashboardName;
                        if (dashboard.panel != null)
                        {
                            dashboard.panel.ContainerTitle = DashboardName;
                        }
                    }
                    dashboard.ID = 0;
                    if (dashboard.panel != null)
                    {
                        dashboard.panel.DashboardID = Guid.NewGuid();
                    }


                    //We are saving in get mode so we need to set allowunsafeupdates property to true.
                    //SPContext.Current.Web.AllowUnsafeUpdates = true;

                    byte[] iconContents = new byte[0];
                    string fileName = string.Empty;
                    dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
                    //SPContext.Current.Web.AllowUnsafeUpdates = true;

                    //Set new dashboardID in dashboard hidden field for further use and set global filter dashboardID
                    hDashboardID.Value = dashboard.ID.ToString();
                    dashboardID = dashboard.ID;
                }

                if (dashboardID > 0)
                {
                    uDashboard = dManager.LoadPanelById(dashboardID, false);
                    if (uDashboard.panel != null)
                    {
                        DataSource = ((ChartSetting)uDashboard.panel).FactTable;
                    }
                }
                else
                {
                    txtTitle.Text = DashboardName;
                }
            }
            else
            {
                long.TryParse(Request[hDashboardID.UniqueID], out dashboardID);
                if (dashboardID > 0)
                {
                    uDashboard = dManager.LoadPanelById(dashboardID, false);
                    if (uDashboard.panel is ChartSetting)
                        DataSource = ((ChartSetting)uDashboard.panel).FactTable;
                }

            }
            uHelper.BindOrderDropDown(cmbOrder, HttpContext.Current.GetManagerContext());
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (legendChk.Checked)
            {
                lagendAlignmentTr.Visible = true; ;
                lagendBoxTr.Visible = true;
                lagendDirectionTr.Visible = true;
            }
            else
            {
                lagendAlignmentTr.Visible = false;
                lagendBoxTr.Visible = false;
                lagendDirectionTr.Visible = false;
            }

            if (IsPostBack)
            {
                DataSource = ddlFactTable.SelectedValue;
            }

            if (xAxisCardView.EditingCardVisibleIndex == -1)
            {
                CardViewComboBoxColumn cbc = xAxisCardView.Columns["SelectedField"] as CardViewComboBoxColumn;
                FillCardviewFactTableFields(DataSource, string.Empty);
                if (factTableFields != null)
                {
                    List<string> factCollumnFields = new List<string>();
                    for (int item = 0; item <= factTableFields.Count - 1; item++)
                    {
                        factCollumnFields.Add(factTableFields[item].FieldName);
                    }
                    cbc.PropertiesComboBox.DataSource = factCollumnFields;
                }

            }

            #region Y-Axis(Expression)
            if (ExpCardView.EditingCardVisibleIndex == -1)
            {
                CardViewComboBoxColumn cbc = ExpCardView.Columns["GroupByField"] as CardViewComboBoxColumn;
                FillCardviewFactTableFields(ddlFactTable.SelectedValue, string.Empty);
                if (factTableFields != null)
                {
                    List<FactTableField> factCollumnFields = new List<FactTableField>();
                    foreach (FactTableField item in factTableFields)
                    {
                        FactTableField field = new FactTableField(item.TableName, string.Format("{0}({1})", item.FieldName, item.DataType.Replace("System.", string.Empty)), item.DataType, item.FieldName);
                        factCollumnFields.Add(field);
                    }
                    cbc.PropertiesComboBox.DataSource = factCollumnFields;
                    cbc.PropertiesComboBox.TextField = "FieldName";
                    cbc.PropertiesComboBox.ValueField = "FieldDisplayName";

                }
            }
            #endregion Y-Axis


        }
        protected override void OnPreRender(EventArgs e)
        {

            if (!IsPostBack)
            {
                ShowTab(true);
            }
            else
            {
                ShowTab();
            }

            FillFactTableFields(ddlFactTableFieldsForFilter, ddlFactTable.SelectedValue);
            hPreviousTab.Value = hTabName.Value;

            if (uDashboard != null)
            {
                string data = Uri.EscapeDataString(string.Format("'DashboardViewID':'0', 'DashboardID':'{0}','ZoomView':true, 'ZoomWidth':'{1}', 'ZoomHeight':'{2}'", uDashboard.ID, uDashboard.PanelWidth, uDashboard.PanelHeight));
                data = "[{" + data + "}]";
                chartUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/ShowDashboardDetails.aspx?dashboards={0}&nocache=true", data));
            }

            base.OnPreRender(e);
        }

        #endregion

        #region General Tab
        private void LoadGeneralTab()
        {
            if (uDashboard == null)
                return;

            txtTitle.Text = uDashboard.Title;
            txtDescription.Text = uDashboard.DashboardDescription;
            cmbOrder.Value = uDashboard.ItemOrder;
        }
        protected void cv_Dashboardname_ServerValidate(object source, ServerValidateEventArgs args)
        {
            cv_Dashboardname.ErrorMessage = string.Empty;
            if (txtTitle.Text.Trim() == string.Empty)
            {
                cv_Dashboardname.ErrorMessage = "Please enter title.";
                args.IsValid = false;
                return;
            }

            // SPQuery query = new SPQuery();
            string query = string.Empty;
            if (dashboardID > 0)
            {
                query = string.Format(@"{0}='{1}' and {2}<>{3}",
                                               DatabaseObjects.Columns.Title, txtTitle.Text.Trim(), DatabaseObjects.Columns.Id, dashboardID);
            }
            else
            {
                query = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, txtTitle.Text.Trim());
            }
            //query.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            DataTable dashboardList = dManager.GetDataTable();// SPListHelper.GetSPList(DatabaseObjects.Lists.DashboardPanels);
            DataRow[] collection = dashboardList.Select(query);
            if (collection.Count() > 0)
            {
                args.IsValid = false;
                cv_Dashboardname.ErrorMessage = "Dashboard with this name already exists";
            }
        }
        #endregion

        #region DataSource Tab
        private void LoadDataSourceTab()
        {
            if (uDashboard == null)
                return;

            ChartSetting chartSetting = uDashboard.panel as ChartSetting;
            ddlFactTable.SelectedIndex = ddlFactTable.Items.IndexOf(ddlFactTable.Items.FindByValue(chartSetting.FactTable));
            txtBasicFilterFormula.Text = chartSetting.BasicFilter;
        }
        protected void DdlFactTable_Load(object sender, EventArgs e)
        {
            List<string> factTables = DashboardCache.DashboardFactTables(HttpContext.Current.GetManagerContext());
            if (ddlFactTable.Items.Count <= 0)
            {
                foreach (string factTable in factTables)
                {
                    ddlFactTable.Items.Add(new ListItem(factTable, factTable));
                }

                ddlFactTable.SelectedIndex = ddlFactTable.Items.IndexOf(ddlFactTable.Items.FindByValue(DataSource));
            }
        }

        protected void DdlFactTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFactTableFields(ddlBasicDateFilterStartField, ddlFactTable.SelectedValue, "datetime");
            FillFactTableFields(ddlBasicDateFilterEndField, ddlFactTable.SelectedValue, "datetime");
            ddlBasicDateFilterStartField.Items.Insert(0, new ListEditItem("--Select Field--", string.Empty));
            ddlBasicDateFilterEndField.Items.Insert(0, new ListEditItem("--Select Field--", string.Empty));
            FillFactTableFields(ddlFactTableFieldsForFilter, ddlFactTable.SelectedValue);
        }
        #endregion

        #region X-Axis Tab
        private void LoadDimensionsTab()
        {

            ChartSetting chartSetting = uDashboard.panel as ChartSetting;
            xAxisCardView.DataSource = chartSetting.Dimensions;
            xAxisCardView.DataBind();
        }
        protected void xAxisCardView_CardUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string key = Convert.ToString(e.Keys[0]);

            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartDimension> dimensions = chartPanel.Dimensions;
            if (dimensions == null)
            {
                dimensions = new List<ChartDimension>();
            }

            //check whether existing dimension edited or new dimension is added
            ChartDimension dimension = null;
            if (dimensions.Count > 0)
            {
                dimension = dimensions.FirstOrDefault(x => x.Title == key);
            }

            UpdateDimension(chartPanel, dimension, e.NewValues);
            e.Cancel = true;
            xAxisCardView.CancelEdit();
            ShowTab(true);
        }

        protected void xAxisCardView_CardInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartDimension> dimensions = chartPanel.Dimensions;
            if (dimensions == null)
            {
                dimensions = new List<ChartDimension>();
            }

            //new dimension is added
            ChartDimension dimension = new ChartDimension();
            dimensions.Add(dimension);

            UpdateDimension(chartPanel, dimension, e.NewValues);
            e.Cancel = true;
            xAxisCardView.CancelEdit();
            ShowTab(true);
        }
        protected void xAxisCardView_CardDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string key = Convert.ToString(e.Keys[0]);

            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartDimension> dimensions = chartPanel.Dimensions;
            if (dimensions == null)
                return;

            ChartDimension dimension = null;
            if (dimensions.Count > 0)
            {
                dimension = dimensions.FirstOrDefault(x => x.Title == key);
            }

            if (dimension == null)
                return;

            //delete dimension
            if (dimension != null)
            {
                dimensions.Remove(dimension);
                byte[] iconContents = new byte[0];
                string fileName = string.Empty;
                dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);

                //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
                //ChartCache.RemoveChartFromCache(chartPanel.ChartId.ToString());
            }

            e.Cancel = true;
            xAxisCardView.CancelEdit();
            ShowTab(true);
        }
        protected void xAxisCardView_CancelCardEditing(object sender, ASPxStartCardEditingEventArgs e)
        {
            ASPxCardView card = sender as ASPxCardView;
            card.JSProperties.Add("cpcancel", "true");
            if (card.JSProperties.Count > 0 && card.JSProperties.ContainsKey("cpupdate"))
                card.JSProperties["cpcancel"] = false;
        }
        private void UpdateDimension(ChartSetting chartPanel, ChartDimension dimension, OrderedDictionary newValues)
        {
            string oldValue = dimension.Title;

            if (newValues["Title"] != null)
                dimension.Title = newValues["Title"].ToString();


            List<ChartExpression> expressions = chartPanel.Expressions;
            if (expressions != null && expressions.Count > 0 && !string.IsNullOrWhiteSpace(oldValue))
            {
                foreach (ChartExpression exp in expressions.Where(x => x.Dimensions != null && x.Dimensions.Count > 0))
                {
                    for (int i = 0; i < exp.Dimensions.Count; i++)
                    {
                        if (exp.Dimensions[i].ToLower() == oldValue.ToLower())
                            exp.Dimensions[i] = dimension.Title;
                    }
                }
            }

            int sequence = 0;
            int clickAction = 0;

            if (newValues["DataPointClickEvent"] != null)
            {
                if (newValues["DataPointClickEvent"].ToString() == "None")
                {
                    clickAction = 0;
                }
                else if (newValues["DataPointClickEvent"].ToString() == "NextDimension")
                {
                    clickAction = 1;
                }
                else if (newValues["DataPointClickEvent"].ToString() == "Detail")
                {
                    clickAction = 2;
                }
            }

            if (newValues["Sequence"] != null)
                int.TryParse(newValues["Sequence"].ToString(), out sequence);

            dimension.Sequence = sequence;
            dimension.DataPointClickEvent = (DatapointClickeEventType)clickAction;

            int picktopdatapoint = 0;
            if (newValues["PickTopDataPoint"] != null)
                int.TryParse(newValues["PickTopDataPoint"].ToString(), out picktopdatapoint);

            dimension.PickTopDataPoint = picktopdatapoint;
            if (newValues["DataPointOrder"] != null)
                dimension.DataPointOrder = newValues["DataPointOrder"].ToString();

            //if (newValues["ShowInDropDown"] != null)
            //    dimension.ShowInDropDown = Convert.ToBoolean(newValues["ShowInDropDown"].ToString());

            if (newValues["SelectedField"] != null)
                dimension.SelectedField = (newValues["SelectedField"].ToString());


            chartPanel.Dimensions = chartPanel.Dimensions.OrderBy(x => x.Sequence).ToList();
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);

            //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
            //ChartCache.RemoveChartFromCache(chartPanel.ChartId.ToString());
        }
        protected void AddnewDimension_Click(object sender, EventArgs e)
        {
            if (xAxisCardView.EditingCardVisibleIndex != -1)
                xAxisCardView.UpdateEdit();
            xAxisCardView.AddNewCard();
        }
        #endregion

        #region Expression Tab
        private void LoadExpressionsTab()
        {
            ChartSetting chartSetting = uDashboard.panel as ChartSetting;
            ExpCardView.DataSource = chartSetting.Expressions;
            ExpCardView.DataBind();
        }

        protected void ExpCardView_CardUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            string key = Convert.ToString(e.Keys[0]);



            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartExpression> expressions = chartPanel.Expressions;
            if (expressions == null)
            {
                expressions = new List<ChartExpression>();
            }

            //check whether existing dimension edited or new dimension is added
            ChartExpression expression = null;
            if (expressions.Count > 0)
            {
                expression = expressions.FirstOrDefault(x => x.Title == key);
            }

            UpdateExpression(chartPanel, expression, e.NewValues);
            e.Cancel = true;
            ExpCardView.CancelEdit();
            ShowTab(true);
        }
        protected void ExpCardView_CardInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartExpression> dimensions = chartPanel.Expressions;
            if (dimensions == null)
            {
                dimensions = new List<ChartExpression>();
            }

            //new dimension is added
            ChartExpression expression = new ChartExpression();
            dimensions.Add(expression);
            UpdateExpression(chartPanel, expression, e.NewValues);

            e.Cancel = true;
            ExpCardView.CancelEdit();
            ShowTab(true);
        }
        protected void ExpCardView_CardDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string key = Convert.ToString(e.Keys[0]);

            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }

            List<ChartExpression> expressions = chartPanel.Expressions;
            if (expressions == null)
                return;

            ChartExpression expression = null;
            if (expressions.Count > 0)
            {
                expression = expressions.FirstOrDefault(x => x.Title == key);
            }

            if (expression == null)
                return;

            //delete expression
            if (expression != null)
            {
                expressions.Remove(expression);
                byte[] iconContents = new byte[0];
                string fileName = string.Empty;
                dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
                //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
                //ChartCache.RemoveChartFromCache(chartPanel.ChartId.ToString());
            }

            e.Cancel = true;
            ExpCardView.CancelEdit();
            ShowTab(true);

        }
        protected void ExpCardView_CancelCardEditing(object sender, ASPxStartCardEditingEventArgs e)
        {
            ASPxCardView card = sender as ASPxCardView;
            card.JSProperties.Add("cpcancel", "true");
            if (card.JSProperties.Count > 0 && card.JSProperties.ContainsKey("cpupdate"))
                card.JSProperties["cpcancel"] = false;
        }
        private void UpdateExpression(ChartSetting chartPanel, ChartExpression expression, OrderedDictionary newValues)
        {
            if (newValues["Title"] != null)
                expression.Title = newValues["Title"].ToString();

            //if (newValues["ExpressionFormula"] != null)

            ASPxMemo txtExpressionFormulas = ExpCardView.FindEditFormLayoutItemTemplateControl("txtExpressionFormulas") as ASPxMemo;
            expression.ExpressionFormula = Convert.ToString(txtExpressionFormulas.Value);

            expression.GroupByField = string.Empty;
            if (newValues["GroupByField"] != null)
                expression.GroupByField = newValues["GroupByField"].ToString();

            expression.ChartType = "Column";
            if (newValues["ChartType"] != null)
                expression.ChartType = newValues["ChartType"].ToString();

            DropDownList ddlExpressionOperator = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlExpressionOperator") as DropDownList;
            if (ddlExpressionOperator != null)
                expression.Operator = ddlExpressionOperator.SelectedItem.Value;
            else if (!string.IsNullOrWhiteSpace(expressionOperator))
                expression.Operator = expressionOperator;

            TextBox txtExpression = ExpCardView.FindEditFormLayoutItemTemplateControl("txtExpression") as TextBox;
            if (txtExpression != null)
                expression.FunctionExpression = Convert.ToString(txtExpression.Text);
            else if (txtFuntionValue != null)
                expression.FunctionExpression = txtFuntionValue.Text;

            //CheckBox cbCalculatePercentage = ExpCardView.FindEditFormLayoutItemTemplateControl("cbCalculatePercentage") as CheckBox;
            //if (cbCalculatePercentage != null)
            //    expression.ShowInPercentage = cbCalculatePercentage.Checked;


            if (expression.Operator.ToLower() == "variance")
            {
                List<string> varianaceVars = new List<string>();
                DropDownList ddlVarianceVar1 = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlVarianceVar1") as DropDownList;
                if (ddlVarianceVar1 != null)
                    varianaceVars.Add(string.Format("[{0}]", ddlVarianceVar1.SelectedValue));

                DropDownList ddlVarianceVar2 = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlVarianceVar2") as DropDownList;
                if (ddlVarianceVar2 != null)
                    varianaceVars.Add(string.Format("[{0}]", ddlVarianceVar2.SelectedValue));

                HtmlSelect ddlVarianceVar3 = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlVarianceVar3") as HtmlSelect;
                if (ddlVarianceVar3 != null)
                    varianaceVars.Add(string.Format("[{0}]", Request[ddlVarianceVar3.UniqueID]));

                if (ddlVarianceVar1 == null && ddlVarianceVar2 == null && ddlVarianceVar3 == null)
                    varianaceVars = varience;
                expression.FunctionExpression = string.Join(",", varianaceVars.ToArray());
            }

            expression.Order = UGITUtility.StringToInt(newValues["Order"]);

            chartPanel.Expressions = chartPanel.Expressions.OrderBy(x => x.Order).ToList();
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
            //uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
            //ChartCache.RemoveChartFromCache(chartPanel.ChartId.ToString());
        }
        protected void AddnewExpression_Click(object sender, EventArgs e)
        {
            if (ExpCardView.EditingCardVisibleIndex != -1)
                ExpCardView.UpdateEdit();

            ExpCardView.AddNewCard();
            ShowTab(true);
        }
        protected void ddlExpressionFormulaFields_Load(object sender, EventArgs e)
        {
            DropDownList ddlExpressionFormulaFields = sender as DropDownList;
            string facttable = string.Empty;
            if (uDashboard != null && uDashboard.panel != null)
            {
                ChartSetting chart = uDashboard.panel as ChartSetting;
                facttable = chart.FactTable;
            }
            //  DropDownList ddlExpressionFormulaFields = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlExpressionFormulaFields") as DropDownList;
            if (ddlExpressionFormulaFields != null && ddlExpressionFormulaFields.Items.Count <= 0)
            {
                FillFactTableFields(ddlExpressionFormulaFields, facttable);
            }
        }

        protected void ddlVarianceVar1_Load(object sender, EventArgs e)
        {
            DropDownList ddlVarianceVar1 = sender as DropDownList;
            //  DropDownList ddlExpressionFormulaFields = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlExpressionFormulaFields") as DropDownList;
            if (ddlVarianceVar1 != null && ddlVarianceVar1.Items.Count <= 0)
            {
                FillFactTableFields(ddlVarianceVar1, ddlFactTable.SelectedValue);
            }
        }

        protected void ddlVarianceVar2_Load(object sender, EventArgs e)
        {
            DropDownList ddlVarianceVar2 = sender as DropDownList;
            //  DropDownList ddlExpressionFormulaFields = ExpCardView.FindEditFormLayoutItemTemplateControl("ddlExpressionFormulaFields") as DropDownList;
            if (ddlVarianceVar2 != null && ddlVarianceVar2.Items.Count <= 0)
            {
                FillFactTableFields(ddlVarianceVar2, ddlFactTable.SelectedValue);
            }
        }

        protected void panelFunction_Init(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            setFunctionExpression(panel);
        }

        protected void panelExpressionFormula_Init(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            setExpressionFormula(panel);
        }


        protected void setExpressionFormula(Control ctrl)
        {
            if (ExpCardView.EditingCardVisibleIndex == -1 || uDashboard == null)
                return;

            var key = ExpCardView.GetCardValues(ExpCardView.EditingCardVisibleIndex, DatabaseObjects.Columns.Title);
            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }
            List<ChartExpression> expressions = chartPanel.Expressions;
            if (expressions == null)
            {
                expressions = new List<ChartExpression>();
            }
            //check whether existing dimension edited or new dimension is added
            ChartExpression expression = null;
            if (expressions.Count > 0)
            {
                expression = expressions.FirstOrDefault(x => x.Title == Convert.ToString(key));
            }

            if (expression == null)
                return;


            ASPxMemo txtExpressionFormulas = ctrl.FindControl("txtExpressionFormulas") as ASPxMemo;
            if (txtExpressionFormulas != null)
                txtExpressionFormulas.Text = expression.ExpressionFormula;

        }

        protected void setFunctionExpression(Control ctrl)
        {
            if (ExpCardView.EditingCardVisibleIndex == -1 || uDashboard == null)
                return;

            var key = ExpCardView.GetCardValues(ExpCardView.EditingCardVisibleIndex, DatabaseObjects.Columns.Title);
            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel == null)
            {
                //return when chart object in not exist in dashboard
                return;
            }
            List<ChartExpression> expressions = chartPanel.Expressions;
            if (expressions == null)
            {
                expressions = new List<ChartExpression>();
            }
            //check whether existing dimension edited or new dimension is added
            ChartExpression expression = null;
            if (expressions.Count > 0)
            {
                expression = expressions.FirstOrDefault(x => x.Title == Convert.ToString(key));
            }

            if (expression == null)
                return;

            DropDownList ddlExpressionOperator = ctrl.FindControl("ddlExpressionOperator") as DropDownList;
            if (ddlExpressionOperator != null)
                ddlExpressionOperator.SelectedIndex = ddlExpressionOperator.Items.IndexOf(ddlExpressionOperator.Items.FindByValue(expression.Operator.ToLower()));

            TextBox txtExpression = ctrl.FindControl("txtExpression") as TextBox;
            if (txtExpression != null)
            {
                if (expression.FunctionExpression.Contains(":"))
                {
                    expression.FunctionExpression = (expression.FunctionExpression.Split(':')[1]).Trim();
                }
                txtExpression.Text = (expression.FunctionExpression);
            }

            Panel varianceExpPanel = ctrl.FindControl("varianceExpPanel") as Panel;
            Panel genericExpPanel = ctrl.FindControl("genericExpPanel") as Panel;


            if (ddlExpressionOperator.SelectedValue == "variance")
            {
                genericExpPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                varianceExpPanel.Style.Add(HtmlTextWriterStyle.Display, "block");

                DropDownList ddlVarianceVar1 = ctrl.FindControl("ddlVarianceVar1") as DropDownList;
                DropDownList ddlVarianceVar2 = ctrl.FindControl("ddlVarianceVar2") as DropDownList;
                HtmlSelect ddlVarianceVar3 = ctrl.FindControl("ddlVarianceVar3") as HtmlSelect;

                List<string> varianceVars = UGITUtility.ConvertStringToList(expression.FunctionExpression, ",");
                if (varianceVars.Count == 3)
                {
                    ddlVarianceVar1.SelectedIndex = ddlVarianceVar1.Items.IndexOf(ddlVarianceVar1.Items.FindByValue(varianceVars[0].Replace("[", string.Empty).Replace("]", string.Empty)));
                    ddlVarianceVar2.SelectedIndex = ddlVarianceVar2.Items.IndexOf(ddlVarianceVar2.Items.FindByValue(varianceVars[1].Replace("[", string.Empty).Replace("]", string.Empty)));

                    ddlVarianceVar3.SelectedIndex = -1;
                    ddlVarianceVar3.Items.Clear();

                    if (ddlVarianceVar1.SelectedIndex != -1)
                    {
                        ddlVarianceVar3.Items.Add(new ListItem(ddlVarianceVar1.SelectedItem.Text, ddlVarianceVar1.SelectedItem.Value));
                    }
                    if (ddlVarianceVar2.SelectedIndex != -1)
                    {
                        ddlVarianceVar3.Items.Add(new ListItem(ddlVarianceVar2.SelectedItem.Text, ddlVarianceVar2.SelectedItem.Value));
                    }
                    ddlVarianceVar3.SelectedIndex = ddlVarianceVar3.Items.IndexOf(ddlVarianceVar3.Items.FindByValue(varianceVars[2].Replace("[", string.Empty).Replace("]", string.Empty)));
                }
                txtExpression.Text = string.Empty;
            }
        }

        protected void panelFunction_Load(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;

            Panel varianceExpPanel = panel.FindControl("varianceExpPanel") as Panel;
            Panel genericExpPanel = panel.FindControl("genericExpPanel") as Panel;

            DropDownList ddlExpressionOperator = panel.FindControl("ddlExpressionOperator") as DropDownList;

            if (ddlExpressionOperator != null && ddlExpressionOperator.SelectedValue == "variance")
            {
                expressionOperator = ddlExpressionOperator.SelectedValue;

                genericExpPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                varianceExpPanel.Style.Add(HtmlTextWriterStyle.Display, "block");

                DropDownList ddlVarianceVar1 = varianceExpPanel.FindControl("ddlVarianceVar1") as DropDownList;
                DropDownList ddlVarianceVar2 = varianceExpPanel.FindControl("ddlVarianceVar2") as DropDownList;
                HtmlSelect ddlVarianceVar3 = varianceExpPanel.FindControl("ddlVarianceVar3") as HtmlSelect;
                varience = new List<string>();
                if (ddlVarianceVar1 != null && !string.IsNullOrWhiteSpace(ddlVarianceVar1.SelectedValue))
                    varience.Add(string.Format("[{0}]", ddlVarianceVar1.SelectedValue));
                if (ddlVarianceVar2 != null && !string.IsNullOrWhiteSpace(ddlVarianceVar2.SelectedValue))
                    varience.Add(string.Format("[{0}]", ddlVarianceVar2.SelectedValue));

                if (ddlVarianceVar3 != null && !string.IsNullOrWhiteSpace(ddlVarianceVar3.UniqueID))
                    varience.Add(string.Format("[{0}]", Request[ddlVarianceVar3.UniqueID]));
            }
            else
            {
                txtFuntionValue = genericExpPanel.FindControl("txtExpression") as TextBox;
                genericExpPanel.Style.Add(HtmlTextWriterStyle.Display, "block");
                varianceExpPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
            }


        }

        #endregion

        #region Cache chart Tab
        private void LoadFrequencyTab()
        {
            ChartSetting chartSetting = uDashboard.panel as ChartSetting;
            cbCacheChart.Checked = chartSetting.IsCacheChart;
            txtCacheChartSchedule.Text = chartSetting.CacheSchedule.ToString();
        }

        #endregion

        #region Preview Tab
        protected void DdlBasicDateFilterDefaultView_Load(object sender, EventArgs e)
        {
            if (ddlBasicDateFilterDefaultView.Items.Count <= 0)
            {
                List<string> dateViews = DashboardCache.GetDateViewList();
                foreach (string item in dateViews)
                {
                    ddlBasicDateFilterDefaultView.Items.Add(new ListEditItem(item, item));
                }
                ddlBasicDateFilterDefaultView.Items.Insert(0, new ListEditItem("Select All", string.Empty));
            }
        }
        #region Repeater x-asix

        protected void xAxisRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ChartSetting dashboardPanel = null;
            long.TryParse(hDashboardID.Value, out dashboardID);

            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
                dashboardPanel = (ChartSetting)uDashboard.panel;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string axis = ((HiddenField)e.Item.FindControl("hdnXAxisFormat")).Value;

                ChartDimension dimension = dashboardPanel.Dimensions.FirstOrDefault(x => x.Title == axis);

                ASPxComboBox cmbxAngle = e.Item.FindControl("cmbxAngle") as ASPxComboBox;
                cmbxAngle.SelectedIndex = cmbxAngle.Items.IndexOfValue(dimension.AxisLabelStyleAngle.ToString());

                ((ASPxCheckBox)e.Item.FindControl("chkbxShowInDropdown")).Checked = dimension.ShowInDropDown;
                HtmlTableRow trDateView = (HtmlTableRow)e.Item.FindControl("trDateView");
                if (trDateView != null)
                {
                    trDateView.Visible = false;
                    if (factTableFields != null && factTableFields.Count > 0)
                    {
                        FactTableField fieldName = factTableFields.FirstOrDefault(x => x.FieldName.ToLower() == dimension.SelectedField.ToLower());
                        if (fieldName != null && (fieldName.DataType == "System.DateTime"))
                        {
                            trDateView.Visible = true;
                            ASPxRadioButtonList rdblstDateView = ((ASPxRadioButtonList)e.Item.FindControl("rdblstDateView"));
                            rdblstDateView.SelectedIndex = rdblstDateView.Items.IndexOf(rdblstDateView.Items.FindByValue(dimension.DateViewType.ToLower()));
                        }
                    }
                }

                ASPxCheckBox chlbxNPoints = e.Item.FindControl("chlbxNPoints") as ASPxCheckBox;
                Panel chlbxNPointsPanel = e.Item.FindControl("chlbxNPointsPanel") as Panel;
                ASPxSpinEdit spinEdtNPoint = e.Item.FindControl("spinEdtNPoint") as ASPxSpinEdit;
                ASPxComboBox cmbxNPointExp = e.Item.FindControl("cmbxNPointExp") as ASPxComboBox;
                ASPxComboBox cmbx1NPointOrder = e.Item.FindControl("cmbx1NPointOrder") as ASPxComboBox;
                ASPxSpinEdit aspxAxisLength = e.Item.FindControl("aspxAxisLength") as ASPxSpinEdit;
                ASPxSpinEdit aspxLegendMaxLength = e.Item.FindControl("aspxLegendMaxLength") as ASPxSpinEdit;

                aspxAxisLength.Value = dimension.AxisLabelMaxLength;
                aspxLegendMaxLength.Value = dimension.LegendTxtMaxLength;
                chlbxNPoints.Checked = false;
                if (dimension.PickTopDataPoint > 0)
                {
                    chlbxNPoints.Checked = true;
                    spinEdtNPoint.Value = dimension.PickTopDataPoint;
                    cmbxNPointExp.SelectedIndex = cmbxNPointExp.Items.IndexOfValue(dimension.DataPointExpression.ToString());
                    cmbx1NPointOrder.SelectedIndex = cmbx1NPointOrder.Items.IndexOfValue(dimension.DataPointOrder);
                    if (cmbx1NPointOrder.SelectedIndex == -1)
                        cmbx1NPointOrder.SelectedIndex = 0;
                }



                ASPxCheckBox chkCumulative = e.Item.FindControl("chkCumulative") as ASPxCheckBox;
                chkCumulative.Checked = dimension.IsCumulative;

                ASPxCheckBox chkbxOrder = e.Item.FindControl("chkbxOrder") as ASPxCheckBox;
                Panel axisOrderPanel = e.Item.FindControl("axisOrderPanel") as Panel;
                ASPxComboBox cmbxOrderExp = e.Item.FindControl("cmbxOrderExp") as ASPxComboBox;
                ASPxComboBox cmbx1Order = e.Item.FindControl("cmbx1Order") as ASPxComboBox;

                chkbxOrder.Checked = false;
                if (dimension.EnableSorting)
                {
                    chkbxOrder.Checked = true;
                    cmbxOrderExp.SelectedIndex = cmbxOrderExp.Items.IndexOfValue(dimension.OrderByExpression.ToString());
                    cmbx1Order.SelectedIndex = cmbx1Order.Items.IndexOfValue(dimension.OrderBy);
                }
                ASPxComboBox cbxScaleType = e.Item.FindControl("cbxScaleType") as ASPxComboBox;
                if (cbxScaleType != null && !string.IsNullOrEmpty(dimension.ScaleType))
                {
                    cbxScaleType.SelectedIndex = cbxScaleType.Items.IndexOfValue(dimension.ScaleType);
                }
            }
        }

        #endregion Repeater x-asix
        #region Repeater Expression
        protected void expressionRpter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ChartSetting dashboardPanel = null;
            long.TryParse(hDashboardID.Value, out dashboardID);

            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
                dashboardPanel = (ChartSetting)uDashboard.panel;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string exp = ((HiddenField)e.Item.FindControl("hdnExpressionFormat")).Value;

                ChartExpression expItem = dashboardPanel.Expressions.FirstOrDefault(x => x.Title == exp);


                ((CheckBox)e.Item.FindControl("hideLevelChkbx")).Checked = expItem.HideLabel;

                #region chart level
                HtmlTable barChartlblTable = ((HtmlTable)e.Item.FindControl("barChartlblTable"));
                HtmlTable columnChartlblTable = ((HtmlTable)e.Item.FindControl("columnChartlblTable"));
                HtmlTable doughnutChartlblTable = ((HtmlTable)e.Item.FindControl("doughnutChartlblTable"));
                HtmlTable funnelChartlblTable = ((HtmlTable)e.Item.FindControl("funnelChartlblTable"));
                HtmlTable lineChartlblTable = ((HtmlTable)e.Item.FindControl("lineChartlblTable"));
                HtmlTable pieChartlblTable = ((HtmlTable)e.Item.FindControl("pieChartlblTable"));
                HtmlTable barStackedChartlblTable = ((HtmlTable)e.Item.FindControl("barStackedChartlblTable"));
                HtmlTable spLineChartlblTable = ((HtmlTable)e.Item.FindControl("spLineChartlblTable"));
                HtmlTable stepLineChartlblTable = ((HtmlTable)e.Item.FindControl("stepLineChartlblTable"));

                //hide all tables
                barChartlblTable.Visible = false;
                columnChartlblTable.Visible = false;
                doughnutChartlblTable.Visible = false;
                funnelChartlblTable.Visible = false;
                lineChartlblTable.Visible = false;
                pieChartlblTable.Visible = false;
                barStackedChartlblTable.Visible = false;
                spLineChartlblTable.Visible = false;
                stepLineChartlblTable.Visible = false;


                ASPxCheckBoxList chkLinedimensions = e.Item.FindControl("chkLineChartimensions") as ASPxCheckBoxList;
                if (chkLinedimensions != null && dashboardPanel != null && dashboardPanel.Dimensions != null && dashboardPanel.Dimensions.Count > 0)
                {
                    List<ChartDimension> lstDimensions = dashboardPanel.Dimensions;
                    if (chkLinedimensions.Items.Count == 0)
                    {
                        chkLinedimensions.Items.AddRange(lstDimensions.AsEnumerable().Select(x => x.Title).ToList());
                    }
                }
                if (expItem.Dimensions != null && expItem.Dimensions.Count > 0)
                {
                    foreach (ListEditItem item in chkLinedimensions.Items)
                    {
                        if (expItem.Dimensions.Exists(x => x.ToLower() == item.Text.ToLower()))
                            item.Selected = true;
                    }
                }

                if (expItem.HideLabel == false)
                {
                    ((ASPxTextBox)e.Item.FindControl("txtLabelPattern")).Text = expItem.LabelText.Trim();

                    switch (expItem.ChartType)
                    {
                        case "Bar":
                            {
                                barChartlblTable.Visible = true;
                                ASPxTextBox txtLabelPattern = e.Item.FindControl("txtLabelPattern") as ASPxTextBox;
                                ASPxComboBox cbxbarLablPosition = e.Item.FindControl("cbxbarLablPosition") as ASPxComboBox;
                                ASPxComboBox cbxbarLabelOrientation = e.Item.FindControl("cbxbarLabelOrientation") as ASPxComboBox;
                                DataItem cbxbarLablPositionItem = expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "barLablPosition");
                                DataItem cbxbarLabelOrientationItem = expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "barLabelOrientation");


                                if (!string.IsNullOrWhiteSpace(expItem.LabelText))
                                    txtLabelPattern.Text = expItem.LabelText;

                                if (cbxbarLablPositionItem != null)
                                    cbxbarLablPosition.SelectedIndex = cbxbarLablPosition.Items.IndexOfValue(cbxbarLablPositionItem.Value);

                                if (cbxbarLabelOrientationItem != null)
                                    cbxbarLabelOrientation.SelectedIndex = cbxbarLabelOrientation.Items.IndexOfValue(cbxbarLabelOrientationItem.Value);
                            }
                            break;
                        case "Column":
                            {
                                barChartlblTable.Visible = true;
                                //columnChartlblTable.Visible = true;

                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "sideBySideFullStackedBarValAsPertge"))
                                    ((ASPxCheckBox)e.Item.FindControl("chkSideBySideFullStackedBarValAsPertge")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "sideBySideFullStackedBarValAsPertge").Value);
                            }
                            break;
                        case "Doughnut":
                            {
                                doughnutChartlblTable.Visible = true;

                                ASPxComboBox cbxDNHoleRadius = e.Item.FindControl("cbxDNHoleRadius") as ASPxComboBox;
                                ASPxComboBox cbxDNLblPosition = e.Item.FindControl("cbxDNLblPosition") as ASPxComboBox;
                                ASPxComboBox cbxDNExplodedpoint = e.Item.FindControl("cbxDNExplodedpoint") as ASPxComboBox;

                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "DNLblPercentage"))
                                    ((ASPxCheckBox)e.Item.FindControl("chkbxdoughnutLblPercentage")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNLblPercentage").Value);
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "DNHoleRadius"))
                                    cbxDNHoleRadius.SelectedIndex = cbxDNHoleRadius.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNHoleRadius").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "DNLblPosition"))
                                    cbxDNLblPosition.SelectedIndex = cbxDNLblPosition.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNLblPosition").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "DNExplodedpoint"))
                                    cbxDNExplodedpoint.SelectedIndex = cbxDNExplodedpoint.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "DNExplodedpoint").Value.ToString());
                            }
                            break;
                        case "Funnel":
                            {
                                funnelChartlblTable.Visible = true;
                                ASPxComboBox cbxFunnellblposition = e.Item.FindControl("cbxFunnellblposition") as ASPxComboBox;
                                ASPxComboBox cbxFunnelHeightWidth = e.Item.FindControl("cbxFunnelHeightWidth") as ASPxComboBox;

                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnelLblAsPercentage"))
                                    ((ASPxCheckBox)e.Item.FindControl("chkfunnelLblAsPercentage")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelLblAsPercentage").Value);
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnellblposition"))
                                    cbxFunnellblposition.SelectedIndex = cbxFunnellblposition.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnellblposition").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnelPointDist"))
                                    ((TextBox)e.Item.FindControl("txtFunnelPointDist")).Text = expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelPointDist").Value.ToString();
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnelHeightWidth"))
                                    cbxFunnelHeightWidth.SelectedIndex = cbxFunnelHeightWidth.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelHeightWidth").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnelAutoHeightWidth"))
                                    ((ASPxCheckBox)e.Item.FindControl("ChkFunnelAutoHeightWidth")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelAutoHeightWidth").Value);
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "funnelAligntoCenter"))
                                    ((ASPxCheckBox)e.Item.FindControl("ChkFunnelAligntoCenter")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "funnelAligntoCenter").Value);
                            }
                            break;

                        case "Pie":
                            {
                                pieChartlblTable.Visible = true;
                                ASPxComboBox cbxPieLblPosition = e.Item.FindControl("cbxPieLblPosition") as ASPxComboBox;
                                ASPxComboBox cbxPieExploadPoints = e.Item.FindControl("cbxPieExploadPoints") as ASPxComboBox;

                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "pieValueAsPer"))
                                    ((ASPxCheckBox)e.Item.FindControl("ChkPieValueAsPer")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "pieValueAsPer").Value);
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "pieLblPosition"))
                                    cbxPieLblPosition.SelectedIndex = cbxPieLblPosition.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "pieLblPosition").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "pieExploadPoints"))
                                    cbxPieExploadPoints.SelectedIndex = cbxPieExploadPoints.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "pieExploadPoints").Value.ToString());
                            }
                            break;
                        case "StackedColumn":
                            {
                                barStackedChartlblTable.Visible = true;
                                ASPxComboBox cbxBarStackedLblPosition = e.Item.FindControl("cbxBarStackedLblPosition") as ASPxComboBox;
                                ASPxComboBox cbxBarStackedLblOrientation = e.Item.FindControl("cbxBarStackedLblOrientation") as ASPxComboBox;

                                if (!string.IsNullOrWhiteSpace(expItem.LabelText))
                                    ((ASPxTextBox)e.Item.FindControl("txtBarStackedLbPattern")).Text = expItem.LabelText;
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "barStackedLblPosition"))
                                    cbxBarStackedLblPosition.SelectedIndex = cbxBarStackedLblPosition.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "barStackedLblPosition").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "barStackedLblOrientation"))
                                    cbxBarStackedLblOrientation.SelectedIndex = cbxBarStackedLblOrientation.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "barStackedLblOrientation").Value.ToString());
                            }
                            break;

                        case "Line":
                            {
                                lineChartlblTable.Visible = true;
                                ASPxComboBox cbxLineMarkType = e.Item.FindControl("cbxLineMarkType") as ASPxComboBox;
                                ASPxComboBox cbxLineMarkSize = e.Item.FindControl("cbxLineMarkSize") as ASPxComboBox;


                                if (!string.IsNullOrWhiteSpace(expItem.LabelText))
                                    ((ASPxTextBox)e.Item.FindControl("aspxLineChartLbPattern")).Text = expItem.LabelText;
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "lineMarkType"))
                                    cbxLineMarkType.SelectedIndex = cbxLineMarkType.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "lineMarkType").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "lineMarkSize"))
                                    cbxLineMarkSize.SelectedIndex = cbxLineMarkSize.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "lineMarkSize").Value.ToString());


                            }
                            break;

                        case "Spline":
                            {
                                spLineChartlblTable.Visible = true;
                                ASPxComboBox cbxSPlineTensionPrcntg = e.Item.FindControl("cbxSPlineTensionPrcntg") as ASPxComboBox;
                                ASPxComboBox cbxSplineMarkerType = e.Item.FindControl("cbxSplineMarkerType") as ASPxComboBox;
                                ASPxComboBox cbxSplineMarkerSize = e.Item.FindControl("cbxSplineMarkerSize") as ASPxComboBox;

                                if (!string.IsNullOrWhiteSpace(expItem.LabelText))
                                    ((ASPxTextBox)e.Item.FindControl("aspxSPLineLbPattern")).Text = expItem.LabelText;
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "SPlineTensionPrcntg"))
                                    cbxSPlineTensionPrcntg.SelectedIndex = cbxSPlineTensionPrcntg.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineTensionPrcntg").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "SPlineMarkerType"))
                                    cbxSplineMarkerType.SelectedIndex = cbxSplineMarkerType.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineMarkerType").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "SPlineMarkerSize"))
                                    cbxSplineMarkerSize.SelectedIndex = cbxSplineMarkerSize.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "SPlineMarkerSize").Value.ToString());
                            }
                            break;
                        case "StepLine":
                            {
                                stepLineChartlblTable.Visible = true;
                                ASPxComboBox cbxStepLineLblAngle = e.Item.FindControl("cbxStepLineLblAngle") as ASPxComboBox;
                                ASPxComboBox cbxStepLineMarkerType = e.Item.FindControl("cbxStepLineMarkerType") as ASPxComboBox;
                                ASPxComboBox cbxStepLineMarkerSize = e.Item.FindControl("cbxStepLineMarkerSize") as ASPxComboBox;

                                if (!string.IsNullOrWhiteSpace(expItem.LabelText))
                                    ((ASPxTextBox)e.Item.FindControl("aspxStepLineLbPattern")).Text = expItem.LabelText;
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "stepLineLblAngle"))
                                    cbxStepLineLblAngle.SelectedIndex = cbxStepLineLblAngle.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineLblAngle").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "stepLineMarkerType"))
                                    cbxStepLineMarkerType.SelectedIndex = cbxStepLineMarkerType.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineMarkerType").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "stepLineMarkerSize"))
                                    cbxStepLineMarkerSize.SelectedIndex = cbxStepLineMarkerSize.Items.IndexOfValue(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "stepLineMarkerSize").Value.ToString());
                                if (expItem.ChartLevelProperties.Exists(x => x.Key == "steplineInverted"))
                                    ((ASPxCheckBox)e.Item.FindControl("chkSteplineInverted")).Checked = Convert.ToBoolean(expItem.ChartLevelProperties.FirstOrDefault(x => x.Key == "steplineInverted").Value);
                            }
                            break;
                    }
                }

                #endregion chart level
                ASPxComboBox expClickAction = e.Item.FindControl("expClickAction") as ASPxComboBox;

                expClickAction.SelectedIndex = expClickAction.Items.IndexOfValue(expItem.DataPointClickEvent.ToString());


                ASPxComboBox cmbxYaxisConfig = e.Item.FindControl("cmbxYaxisConfig") as ASPxComboBox;
                cmbxYaxisConfig.SelectedIndex = cmbxYaxisConfig.Items.IndexOfValue(((int)expItem.YAsixType).ToString());

                ASPxComboBox aspxYAxisPointFormat = e.Item.FindControl("aspxYAxisPointFormat") as ASPxComboBox;
                aspxYAxisPointFormat.SelectedIndex = aspxYAxisPointFormat.Items.IndexOfValue(expItem.LabelFormat);
                // ddlYAxisType.SelectedIndex = ddlYAxisType.Items.IndexOf(ddlYAxisType.Items.FindByValue(((int)expression.YAsixType).ToString()
            }
        }
        #endregion

        private void FillChartFormatNavBar(Dashboard dashboard)
        {
            nbpreView.Groups.Clear();

            NavBarGroup chartGroup = new NavBarGroup();
            NavBarGroup xAxisGroup = new NavBarGroup();
            NavBarGroup expressionGroup = new NavBarGroup();
            nbpreView.Groups.Add(chartGroup);
            nbpreView.Groups.Add(xAxisGroup);
            nbpreView.Groups.Add(expressionGroup);

            chartGroup.Text = "Chart Appearance";
            xAxisGroup.Text = "X-Axis";
            expressionGroup.Text = "Expressions";

            NavBarItem item = new NavBarItem();
            item.Text = "Basic";
            item.Name = "chart-basic";
            item.Image.Url = "/content/images/BasicChartSettings.PNG";
            chartGroup.Items.Add(item);
            item = new NavBarItem();
            item.Text = "Legends";
            item.Name = "chart-Legend";
            item.Image.Url = "/content/images/Chartlegend.PNG";
            chartGroup.Items.Add(item);

            ChartSetting setting = dashboard.panel as ChartSetting;

            foreach (ChartDimension dm in setting.Dimensions)
            {
                NavBarItem aTtem = new NavBarItem();
                aTtem.Text = dm.Title;
                aTtem.Name = string.Format("xaxis-{0}", setting.Dimensions.IndexOf(dm));
                xAxisGroup.Items.Add(aTtem);
            }

            foreach (ChartExpression dm in setting.Expressions)
            {
                NavBarItem aTtem = new NavBarItem();
                aTtem.Text = dm.Title;
                aTtem.Name = string.Format("expression-{0}", setting.Expressions.IndexOf(dm));
                expressionGroup.Items.Add(aTtem);
            }
            nbpreView.AllowSelectItem = true;
            nbpreView.SelectedItem = nbpreView.Groups[0].Items[0];
        }
        protected void txtExpressionFormulas_Load(object sender, EventArgs e)
        {

        }
        protected void hideLevelChkbx_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox hideLevelchkbx = sender as CheckBox;
            if (hideLevelchkbx != null)
            {
                HtmlTableRow trLabelProperties = (HtmlTableRow)hideLevelchkbx.Parent.FindControl("trLabelProperties");
                if (trLabelProperties == null)
                    return;

                if (hideLevelchkbx.Checked)
                {
                    trLabelProperties.Visible = false;
                }
                else
                {
                    trLabelProperties.Visible = true;
                }
            }
        }
        private void LoadPreviewTab()
        {
            FillChartFormatNavBar(uDashboard);

            ChartSetting chartSetting = uDashboard.panel as ChartSetting;

            chkHideTitle.Checked = Convert.ToBoolean(uDashboard.IsHideTitle);
            zoomchk.Checked = chartSetting.HideZoomView;
            downloadchk.Checked = chartSetting.HidewDownloadView;
            tablechk.Checked = chartSetting.HideTableView;

            hideGrid.Checked = chartSetting.HideGrid;
            aspxBGColor.Value = ColorTranslator.FromHtml(Constants.ChartBGColor);
            if (!string.IsNullOrWhiteSpace(chartSetting.BGColor))
            {
                try
                {
                    aspxBGColor.Value = ColorTranslator.FromHtml(chartSetting.BGColor);
                }
                catch (Exception)
                {
                    aspxBGColor.Value = ColorTranslator.FromHtml(Constants.ChartBGColor);
                }
            }

            chkbxReversePlotting.Checked = chartSetting.ReversePlotting;

            if (ddlFactTable.SelectedValue != null && ddlFactTable.SelectedValue != string.Empty)
            {
                FillFactTableFields(ddlBasicDateFilterStartField, ddlFactTable.SelectedValue, "System.DateTime");
                ddlBasicDateFilterStartField.Items.Insert(0, new ListEditItem("--Select Field--", string.Empty));
                ddlBasicDateFilterStartField.SelectedIndex = ddlBasicDateFilterStartField.Items.IndexOf(ddlBasicDateFilterStartField.Items.FindByValue(chartSetting.BasicDateFitlerStartField));

                FillFactTableFields(ddlBasicDateFilterEndField, ddlFactTable.SelectedValue, "System.DateTime");
                ddlBasicDateFilterEndField.Items.Insert(0, new ListEditItem("--Select Field--", string.Empty));
                ddlBasicDateFilterEndField.SelectedIndex = ddlBasicDateFilterEndField.Items.IndexOf(ddlBasicDateFilterEndField.Items.FindByValue(chartSetting.BasicDateFitlerEndField));
            }

            ddlBasicDateFilterDefaultView.SelectedIndex = ddlBasicDateFilterDefaultView.Items.IndexOf(ddlBasicDateFilterDefaultView.Items.FindByValue(chartSetting.BasicDateFilterDefaultView));
            cbHideDateFilterBox.Checked = chartSetting.HideDateFilterDropdown;

            chartWidth.Text = uDashboard.PanelWidth.ToString();
            chartHeight.Text = uDashboard.PanelHeight.ToString();

            //legends settings
            legendChk.Checked = !chartSetting.HideLegend;

            lagendAlignmentTr.Visible = false;
            lagendBoxTr.Visible = false;
            lagendDirectionTr.Visible = false;
            if (legendChk.Checked)
            {
                lagendAlignmentTr.Visible = true;
                lagendBoxTr.Visible = true;
                lagendDirectionTr.Visible = true;

                cmbxHorizontalAlignment.SelectedIndex = cmbxHorizontalAlignment.Items.IndexOfValue(chartSetting.HorizontalAlignment);
                cmbxVerticalAlignment.SelectedIndex = cmbxVerticalAlignment.Items.IndexOfValue(chartSetting.VerticalAlignment);
                cmbxDirection.SelectedIndex = cmbxDirection.Items.IndexOfValue(chartSetting.Direction);
                cmbxMaxHorizontalPercentage.SelectedIndex = cmbxMaxHorizontalPercentage.Items.IndexOfValue(chartSetting.MaxHorizontalPercentage);
                cmbxMaxVerticalPercentage.SelectedIndex = cmbxMaxVerticalPercentage.Items.IndexOfValue(chartSetting.MaxVerticalPercentage);
            }

            cmbxPallete.SelectedIndex = cmbxPallete.Items.IndexOfValue(chartSetting.Palette);


            //bind X-Axis settings
            xAxisRepeater.DataSource = chartSetting.Dimensions;
            xAxisRepeater.DataBind();


            //bind Expression settings
            expressionRpter.DataSource = chartSetting.Expressions;
            expressionRpter.DataBind();
        }
        private void SavePreviewProperties(ChartSetting dashboardPanel)
        {
            nbpreView.SelectedItem = nbpreView.Groups[nbpreView.SelectedItem.Group.Index].Items[nbpreView.SelectedItem.Index];

            uDashboard.IsHideTitle = chkHideTitle.Checked;
            //Action settings
            dashboardPanel.HideZoomView = zoomchk.Checked;
            dashboardPanel.HidewDownloadView = downloadchk.Checked;
            dashboardPanel.HideTableView = tablechk.Checked;

            //Hide grid settings
            dashboardPanel.HideGrid = hideGrid.Checked;
            dashboardPanel.BGColor = ColorTranslator.ToHtml((Color)aspxBGColor.Value);
            dashboardPanel.ReversePlotting = chkbxReversePlotting.Checked;

            dashboardPanel.BasicDateFitlerStartField = Convert.ToString(ddlBasicDateFilterStartField.Value);
            dashboardPanel.BasicDateFitlerEndField = Convert.ToString(ddlBasicDateFilterEndField.Value);
            dashboardPanel.BasicDateFilterDefaultView = Convert.ToString(ddlBasicDateFilterDefaultView.Value);
            dashboardPanel.HideDateFilterDropdown = cbHideDateFilterBox.Checked;
            //Chart size settings
            uDashboard.PanelWidth = Convert.ToInt32(chartWidth.Text.Trim());
            uDashboard.PanelHeight = Convert.ToInt32(chartHeight.Text.Trim());

            //legends settings
            dashboardPanel.HideLegend = !legendChk.Checked;
            if (legendChk.Checked)
            {
                if (cmbxHorizontalAlignment.SelectedItem != null)
                    dashboardPanel.HorizontalAlignment = cmbxHorizontalAlignment.SelectedItem.Value.ToString();

                if (cmbxVerticalAlignment.SelectedItem != null)
                    dashboardPanel.VerticalAlignment = cmbxVerticalAlignment.SelectedItem.Value.ToString();

                if (cmbxDirection.SelectedItem != null)
                    dashboardPanel.Direction = cmbxDirection.SelectedItem.Value.ToString();

                if (cmbxMaxHorizontalPercentage.SelectedItem != null)
                    dashboardPanel.MaxHorizontalPercentage = cmbxMaxHorizontalPercentage.SelectedItem.Value.ToString();

                if (cmbxMaxVerticalPercentage.SelectedItem != null)
                    dashboardPanel.MaxVerticalPercentage = cmbxMaxVerticalPercentage.SelectedItem.Value.ToString();
            }

            dashboardPanel.Palette = cmbxPallete.SelectedItem.Value.ToString();

            foreach (RepeaterItem item in xAxisRepeater.Items)
            {
                string dimentionVal = ((HiddenField)item.FindControl("hdnXAxisFormat")).Value;
                ChartDimension dimension = dashboardPanel.Dimensions.FirstOrDefault(x => x.Title == dimentionVal);

                if (((ASPxComboBox)item.FindControl("cmbxAngle")).SelectedIndex != -1)
                    dimension.AxisLabelStyleAngle = Convert.ToInt32(((ASPxComboBox)item.FindControl("cmbxAngle")).SelectedItem.Value.ToString());

                ASPxCheckBox chkbxShowInDropdown = (ASPxCheckBox)item.FindControl("chkbxShowInDropdown");
                if (chkbxShowInDropdown != null)
                    dimension.ShowInDropDown = chkbxShowInDropdown.Checked;


                ASPxRadioButtonList rdblstDateView = ((ASPxRadioButtonList)item.FindControl("rdblstDateView"));
                if (rdblstDateView != null && rdblstDateView.SelectedIndex != -1)
                    dimension.DateViewType = rdblstDateView.SelectedItem.Text.ToLower();
                else
                    dimension.DateViewType = string.Empty;

                ASPxCheckBox chkCumulative = (ASPxCheckBox)item.FindControl("chkCumulative");
                dimension.IsCumulative = chkCumulative.Checked;

                ASPxCheckBox chlbxNPoints = item.FindControl("chlbxNPoints") as ASPxCheckBox;
                Panel chlbxNPointsPanel = item.FindControl("chlbxNPointsPanel") as Panel;
                ASPxSpinEdit spinEdtNPoint = item.FindControl("spinEdtNPoint") as ASPxSpinEdit;
                ASPxComboBox cmbxNPointExp = item.FindControl("cmbxNPointExp") as ASPxComboBox;
                ASPxComboBox cmbx1NPointOrder = item.FindControl("cmbx1NPointOrder") as ASPxComboBox;
                ASPxSpinEdit aspxAxisLength = item.FindControl("aspxAxisLength") as ASPxSpinEdit;
                ASPxSpinEdit aspxLegendMaxLength = item.FindControl("aspxLegendMaxLength") as ASPxSpinEdit;

                dimension.PickTopDataPoint = 0;
                if (chlbxNPoints.Checked)
                {
                    dimension.PickTopDataPoint = UGITUtility.StringToInt(spinEdtNPoint.Value);
                    dimension.DataPointExpression = UGITUtility.StringToInt(cmbxNPointExp.Value);
                    dimension.DataPointOrder = Convert.ToString(cmbx1NPointOrder.Value);
                }

                dimension.AxisLabelMaxLength = UGITUtility.StringToInt(aspxAxisLength.Value);
                dimension.LegendTxtMaxLength = UGITUtility.StringToInt(aspxLegendMaxLength.Value);
                ASPxCheckBox chkbxOrder = item.FindControl("chkbxOrder") as ASPxCheckBox;
                Panel axisOrderPanel = item.FindControl("axisOrderPanel") as Panel;
                ASPxComboBox cmbxOrderExp = item.FindControl("cmbxOrderExp") as ASPxComboBox;
                ASPxComboBox cmbx1Order = item.FindControl("cmbx1Order") as ASPxComboBox;

                dimension.EnableSorting = false;
                if (chkbxOrder.Checked)
                {
                    dimension.EnableSorting = true;
                    dimension.OrderByExpression = UGITUtility.StringToInt(cmbxOrderExp.Value);
                    dimension.OrderBy = Convert.ToString(cmbx1Order.Value);
                }
                ASPxComboBox cbxScaleType = item.FindControl("cbxScaleType") as ASPxComboBox;
                if (cbxScaleType != null)
                {
                    dimension.ScaleType = cbxScaleType.Text;
                }
            }

            foreach (RepeaterItem item in expressionRpter.Items)
            {

                string expressionVal = ((HiddenField)item.FindControl("hdnExpressionFormat")).Value;
                ChartExpression expression = dashboardPanel.Expressions.FirstOrDefault(x => x.Title == expressionVal);

                expression.Dimensions = new List<string>();

                ASPxCheckBoxList linechkdimensions = (ASPxCheckBoxList)item.FindControl("chkLineChartimensions");
                if (linechkdimensions != null && linechkdimensions.SelectedIndex != -1)
                {
                    string values = string.Join(Constants.Separator6, linechkdimensions.SelectedItems.Cast<ListEditItem>().AsEnumerable().Select(x => Convert.ToString(x.Value)).ToList());
                    expression.Dimensions = linechkdimensions.SelectedItems.Cast<ListEditItem>().AsEnumerable().Select(x => Convert.ToString(x.Value)).ToList();
                }
                string action = "None";
                int clickAction = 0;

                if (((ASPxComboBox)item.FindControl("expClickAction")).Value != null)
                {
                    action = ((ASPxComboBox)item.FindControl("expClickAction")).SelectedItem.Value.ToString();
                }

                if (action == "None")
                {
                    clickAction = 0;
                }
                else if (action == "NextDimension")
                {
                    clickAction = 1;
                }
                else if (action == "Detail")
                {
                    clickAction = 2;
                }
                else if (action == "Inherit")
                {
                    clickAction = 3;
                }

                expression.DataPointClickEvent = (DatapointClickeEventType)clickAction;




                if (((ASPxComboBox)item.FindControl("cmbxYaxisConfig")).Value != null)
                {
                    string axistypevalue = ((ASPxComboBox)item.FindControl("cmbxYaxisConfig")).Value.ToString();
                    expression.YAsixType = (ChartAxisType)Enum.Parse(typeof(ChartAxisType), axistypevalue);
                }

                ASPxComboBox aspxYAxisPointFormat = item.FindControl("aspxYAxisPointFormat") as ASPxComboBox;
                expression.LabelFormat = Convert.ToString(aspxYAxisPointFormat.Value);

                #region chart level
                expression.HideLabel = ((CheckBox)item.FindControl("hideLevelChkbx")).Checked;

                if (expression.HideLabel == false)
                {
                    expression.ChartLevelProperties = new List<DataItem>();
                    switch (expression.ChartType)
                    {
                        case "Bar":
                            if (((ASPxComboBox)item.FindControl("cbxbarLablPosition")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("barLablPosition", Convert.ToString(((ASPxComboBox)item.FindControl("cbxbarLablPosition")).SelectedItem.Value)));


                            if (((ASPxComboBox)item.FindControl("cbxbarLabelOrientation")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("barLabelOrientation", ((ASPxComboBox)item.FindControl("cbxbarLabelOrientation")).SelectedItem.Value.ToString()));

                            ASPxTextBox txtLabelPattern = (ASPxTextBox)item.FindControl("txtLabelPattern");
                            expression.LabelText = txtLabelPattern.Text.Trim();

                            break;
                        case "Column":
                            {
                                ASPxTextBox txtLabelPattern1 = (ASPxTextBox)item.FindControl("txtLabelPattern");
                                expression.LabelText = txtLabelPattern1.Text.Trim();

                                expression.ChartLevelProperties.Add(new DataItem("sideBySideFullStackedBarValAsPertge", Convert.ToString(((ASPxCheckBox)item.FindControl("chkSideBySideFullStackedBarValAsPertge")).Checked)));
                            }
                            break;

                        case "Doughnut":
                            expression.ChartLevelProperties.Add(new DataItem("DNLblPercentage", Convert.ToString(((ASPxCheckBox)item.FindControl("chkbxdoughnutLblPercentage")).Checked)));

                            if (((ASPxComboBox)item.FindControl("cbxDNHoleRadius")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("DNHoleRadius", ((ASPxComboBox)item.FindControl("cbxDNHoleRadius")).SelectedItem.Value.ToString()));

                            if (((ASPxComboBox)item.FindControl("cbxDNLblPosition")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("DNLblPosition", ((ASPxComboBox)item.FindControl("cbxDNLblPosition")).SelectedItem.Value.ToString()));

                            if (((ASPxComboBox)item.FindControl("cbxDNExplodedpoint")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("DNExplodedpoint", ((ASPxComboBox)item.FindControl("cbxDNExplodedpoint")).SelectedItem.Value.ToString()));

                            break;

                        case "Funnel":
                            expression.ChartLevelProperties.Add(new DataItem("funnelLblAsPercentage", Convert.ToString(((ASPxCheckBox)item.FindControl("chkfunnelLblAsPercentage")).Checked)));

                            if (((ASPxComboBox)item.FindControl("cbxFunnellblposition")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("funnellblposition", ((ASPxComboBox)item.FindControl("cbxFunnellblposition")).SelectedItem.Value.ToString()));

                            expression.ChartLevelProperties.Add(new DataItem("funnelPointDist", ((TextBox)item.FindControl("txtFunnelPointDist")).Text.Trim()));

                            if (((ASPxComboBox)item.FindControl("cbxFunnelHeightWidth")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("funnelHeightWidth", ((ASPxComboBox)item.FindControl("cbxFunnelHeightWidth")).SelectedItem.Value.ToString()));


                            expression.ChartLevelProperties.Add(new DataItem("funnelAutoHeightWidth", Convert.ToString(((ASPxCheckBox)item.FindControl("ChkFunnelAutoHeightWidth")).Checked)));

                            expression.ChartLevelProperties.Add(new DataItem("funnelAligntoCenter", Convert.ToString(((ASPxCheckBox)item.FindControl("ChkFunnelAligntoCenter")).Checked)));
                            break;

                        case "Pie":
                            expression.ChartLevelProperties.Add(new DataItem("pieValueAsPer", Convert.ToString(((ASPxCheckBox)item.FindControl("ChkPieValueAsPer")).Checked)));

                            if (((ASPxComboBox)item.FindControl("cbxPieLblPosition")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("pieLblPosition", ((ASPxComboBox)item.FindControl("cbxPieLblPosition")).SelectedItem.Value.ToString()));


                            if (((ASPxComboBox)item.FindControl("cbxPieExploadPoints")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("pieExploadPoints", ((ASPxComboBox)item.FindControl("cbxPieExploadPoints")).SelectedItem.Value.ToString()));


                            break;
                        case "StackedColumn":

                            ASPxTextBox txtLabelPattern2 = (ASPxTextBox)item.FindControl("txtBarStackedLbPattern");
                            expression.LabelText = txtLabelPattern2.Text.Trim();

                            if (((ASPxComboBox)item.FindControl("cbxBarStackedLblPosition")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("barStackedLblPosition", ((ASPxComboBox)item.FindControl("cbxBarStackedLblPosition")).SelectedItem.Value.ToString()));


                            if (((ASPxComboBox)item.FindControl("cbxBarStackedLblOrientation")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("barStackedLblOrientation", ((ASPxComboBox)item.FindControl("cbxBarStackedLblOrientation")).SelectedItem.Value.ToString()));

                            break;

                        case "Line":

                            ASPxTextBox txtLabelPattern3 = (ASPxTextBox)item.FindControl("aspxLineChartLbPattern");
                            expression.LabelText = txtLabelPattern3.Text.Trim();

                            if (((ASPxComboBox)item.FindControl("cbxLineMarkType")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("lineMarkType", ((ASPxComboBox)item.FindControl("cbxLineMarkType")).SelectedItem.Value.ToString()));

                            if (((ASPxComboBox)item.FindControl("cbxLineMarkSize")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("lineMarkSize", ((ASPxComboBox)item.FindControl("cbxLineMarkSize")).SelectedItem.Value.ToString()));



                            break;

                        case "Spline":

                            ASPxTextBox txtLabelPattern4 = (ASPxTextBox)item.FindControl("aspxSPLineLbPattern");
                            expression.LabelText = txtLabelPattern4.Text.Trim();

                            if (((ASPxComboBox)item.FindControl("cbxSPlineTensionPrcntg")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("SPlineTensionPrcntg", ((ASPxComboBox)item.FindControl("cbxSPlineTensionPrcntg")).SelectedItem.Value.ToString()));


                            if (((ASPxComboBox)item.FindControl("cbxSplineMarkerType")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("SPlineMarkerType", ((ASPxComboBox)item.FindControl("cbxSplineMarkerType")).SelectedItem.Value.ToString()));

                            if (((ASPxComboBox)item.FindControl("cbxSplineMarkerSize")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("SPlineMarkerSize", ((ASPxComboBox)item.FindControl("cbxSplineMarkerSize")).SelectedItem.Value.ToString()));

                            break;
                        case "StepLine":

                            ASPxTextBox txtLabelPattern5 = (ASPxTextBox)item.FindControl("aspxStepLineLbPattern");
                            expression.LabelText = txtLabelPattern5.Text.Trim();

                            if (((ASPxComboBox)item.FindControl("cbxStepLineLblAngle")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("stepLineLblAngle", ((ASPxComboBox)item.FindControl("cbxStepLineLblAngle")).SelectedItem.Value.ToString()));


                            if (((ASPxComboBox)item.FindControl("cbxStepLineMarkerType")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("stepLineMarkerType", ((ASPxComboBox)item.FindControl("cbxStepLineMarkerType")).SelectedItem.Value.ToString()));

                            if (((ASPxComboBox)item.FindControl("cbxStepLineMarkerSize")).SelectedIndex != -1)
                                expression.ChartLevelProperties.Add(new DataItem("stepLineMarkerSize", ((ASPxComboBox)item.FindControl("cbxStepLineMarkerSize")).SelectedItem.Value.ToString()));

                            expression.ChartLevelProperties.Add(new DataItem("steplineInverted", Convert.ToString(((ASPxCheckBox)item.FindControl("chkSteplineInverted")).Checked)));

                            break;
                    }
                }

                #endregion chart level

            }
        }
        protected void orderByExpression_Init(object sender, EventArgs e)
        {
            ASPxComboBox box = sender as ASPxComboBox;
            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel.Expressions == null)
                return;

            box.Items.Add("Current X-Axis", "0");
            for (int i = 0; i < chartPanel.Expressions.Count; i++)
            {
                box.Items.Add(chartPanel.Expressions[i].Title, (i + 1).ToString());
            }
            box.SelectedIndex = 0;
        }

        protected void npointExpress_Init(object sender, EventArgs e)
        {
            ASPxComboBox box = sender as ASPxComboBox;
            ChartSetting chartPanel = (ChartSetting)uDashboard.panel;
            if (chartPanel.Expressions == null)
                return;

            box.Items.Add("Current X-Axis", "0");
            for (int i = 0; i < chartPanel.Expressions.Count; i++)
            {
                box.Items.Add(chartPanel.Expressions[i].Title, (i + 1).ToString());
            }
            box.SelectedIndex = 0;
        }
        #endregion

        #region Click events
        protected void Save_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                hTabName.Value = hPreviousTab.Value;
                return;
            }

            SaveTab(hTabName.Value.ToString());
            //uHelper.ClosePopUpAndEndResponse(Context, true, true);
        }
        protected void btnMoveToNxtTab_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                hTabName.Value = hPreviousTab.Value;
                return;
            }

            int currentTabIndex = tabMenu.ActiveTabIndex;
            if (currentTabIndex < tabMenu.Tabs.Count)
            {
                tabMenu.ActiveTabIndex = currentTabIndex + 1;
                hTabName.Value = tabMenu.ActiveTab.Name;
                SaveTab(hPreviousTab.Value.ToString());
            }

        }
        protected void btnTab_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                hTabName.Value = hPreviousTab.Value;
                return;
            }

            SaveTab(hPreviousTab.Value.ToString());
        }
        #endregion

        #region helpers

        private void ShowTab(bool forceReload = false)
        {
            string previousTab = hPreviousTab.Value;
            string tabName = hTabName.Value;

            tabMenu.ActiveTab = tabMenu.Tabs.FindByName(tabName);

            if (tabName == previousTab && !forceReload)
                return;


            tabPanelGeneral.Visible = false;
            tabPanelDatasource.Visible = false;
            tabPanelDimensions.Visible = false;
            tabPanelLoadfrequency.Visible = false;
            tabPanelPreview.Visible = false;
            tabPanelExpressions.Visible = false;
            //hide  addExpression , addDimension buttons
            AddnewDimensionDiv.Visible = false;
            AddnewExpressionDiv.Visible = false;

            tabMenu.ActiveTab = tabMenu.Tabs.FindByName(tabName);

            switch (tabName)
            {
                case "general":
                    tabPanelGeneral.Visible = true;
                    LoadGeneralTab();
                    break;
                case "datasource":
                    tabPanelDatasource.Visible = true;
                    LoadDataSourceTab();
                    break;
                case "dimensions":
                    tabPanelDimensions.Visible = true;
                    AddnewDimensionDiv.Visible = true;
                    LoadDimensionsTab();
                    break;
                case "expressions":
                    tabPanelExpressions.Visible = true;
                    AddnewExpressionDiv.Visible = true;
                    LoadExpressionsTab();
                    break;
                case "loadfrequency":
                    tabPanelLoadfrequency.Visible = true;
                    LoadFrequencyTab();
                    break;
                case "preview":
                    tabPanelPreview.Visible = true;
                    LoadPreviewTab();
                    break;
            }
        }
        public void SaveTab(string tabname)
        {
            //UDashboard uDashboard = null;
            ChartSetting dashboardPanel = null;
            bool newDashboard = false;
            long.TryParse(hDashboardID.Value, out dashboardID);

            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
                dashboardPanel = (ChartSetting)uDashboard.panel;
            }

            if (uDashboard == null)
            {
                uDashboard = new Dashboard();
                dashboardPanel = new ChartSetting();
                newDashboard = true;

            }

            if (dashboardPanel == null)
            {
                dashboardPanel = new ChartSetting();
            }

            dashboardPanel.DashboardID = Guid.NewGuid();
            uDashboard.panel = dashboardPanel;
            uDashboard.DashboardType = DashboardType.Chart;
            dashboardPanel.type = DashboardType.Chart;

            switch (tabname)
            {
                case "general":
                    uDashboard.IsHideDescription = false;
                    uDashboard.Title = txtTitle.Text;
                    uDashboard.DashboardDescription = txtDescription.Text;
                    if (cmbOrder.SelectedIndex == -1)
                        uDashboard.ItemOrder = UGITUtility.StringToInt(cmbOrder.Items[cmbOrder.Items.Count - 1].Value);
                    else
                    {
                        int order = Convert.ToInt32(cmbOrder.Value);
                        uDashboard.ItemOrder = order > 1 ? order - 1 : order;
                    }
                    uDashboard.CategoryName = txtTitle.Text.Trim();
                    dashboardPanel.ContainerTitle = txtTitle.Text.Trim();
                    dashboardPanel.Description = txtDescription.Text.Trim();
                    if (newDashboard)
                    {
                        dashboardPanel.HideLegend = false;
                        dashboardPanel.FactTable = Request["factTable"];
                        dashboardPanel.HorizontalAlignment = "Center";
                        dashboardPanel.VerticalAlignment = "TopOutside";
                        dashboardPanel.Direction = "LeftToRight";
                        dashboardPanel.MaxHorizontalPercentage = "100";
                        dashboardPanel.MaxVerticalPercentage = "100";
                    }
                    break;

                case "datasource":
                    //Basic filter
                    dashboardPanel.FactTable = ddlFactTable.SelectedValue;
                    dashboardPanel.BasicFilter = txtBasicFilterFormula.Text.Trim();

                    break;
                case "dimensions":
                    {
                        if (xAxisCardView.EditingCardVisibleIndex != -1)
                            xAxisCardView.UpdateEdit();
                    }
                    break;
                case "expressions":
                    {
                        if (ExpCardView.EditingCardVisibleIndex != -1)
                            ExpCardView.UpdateEdit();
                    }
                    break;
                case "loadfrequency":
                    {
                        dashboardPanel.IsCacheChart = cbCacheChart.Checked;
                        int scheduleMinutes = 0;
                        int.TryParse(txtCacheChartSchedule.Text.Trim(), out scheduleMinutes);
                        dashboardPanel.CacheSchedule = scheduleMinutes;
                    }
                    break;
                case "preview":
                    SavePreviewProperties(dashboardPanel);
                    break;
            }

            //View Dashboard

            if (uDashboard.PanelWidth <= 0)
            {
                uDashboard.PanelWidth = 300;
            }
            if (uDashboard.PanelHeight <= 0)
            {
                uDashboard.PanelHeight = 300;
            }

            //Update first in case of new dashboard to get dashboard id
            if (newDashboard)
            {
                byte[] iconContentss = new byte[0];
                string fileNames = string.Empty;
                dManager.SaveDashboardPanel(iconContentss, fileNames, false, uDashboard);

                //Save new dashboard id in hiddenfield and dashboardid variable
                hDashboardID.Value = uDashboard.ID.ToString();
                dashboardID = uDashboard.ID;
            }

            //Get ID after save and preserve it into page
            hDashboardID.Value = uDashboard.ID.ToString();

            //update dashboard
            byte[] iconContents = new byte[0];
            string fileName = string.Empty;
            dManager.SaveDashboardPanel(iconContents, fileName, false, uDashboard);
            uHelper.PerformAjaxPanelCallBack(Page, Context);
            // uGITCache.RefreshList(DatabaseObjects.Lists.DashboardPanels);
            //ChartCache.RemoveChartFromCache(dashboardPanel.ChartId.ToString());

        }

        private void FillExpressionDropdown(DropDownList dropdown, List<ChartExpression> expressions, int selectedIndex)
        {
            dropdown.Items.Clear();
            foreach (ChartExpression expression in expressions)
            {
                dropdown.Items.Add(new ListItem(expression.Title, expression.Title));
            }
            dropdown.Items.Insert(0, new ListItem("--All--", string.Empty));
        }
        private void FillFactTableFields(object sender, string factTable)
        {
            FillFactTableFields(sender, factTable, string.Empty);
            FillCardviewFactTableFields(factTable, string.Empty);
        }
        private List<string> FillCardviewFactTableFields(string factTable, string typeFilter)
        {
            List<string> list = new List<string>();

            if (factTable != null && factTable.Trim() != string.Empty)
            {
                factTableFields = DashboardCache.GetFactTableFields(HttpContext.Current.GetManagerContext(), factTable.Trim());
                if (factTableFields != null)
                {
                    factTableFields.RemoveAll(x => x.FieldName.EndsWith("User"));
                    if (!string.IsNullOrEmpty(typeFilter))
                    {
                        if (typeFilter.ToLower() == "number")
                        {
                            factTableFields = factTableFields.Where(x => x.DataType.ToLower() == "system.double" ||
                                x.DataType.ToLower() == "system.Int32").ToList();
                        }
                        else
                        {
                            factTableFields = factTableFields.Where(x => x.DataType.ToLower() == typeFilter.ToLower()).ToList();
                        }
                    }

                    factTableFields = factTableFields.OrderBy(x => x.FieldName).ToList();
                    foreach (FactTableField field in factTableFields)
                    {
                        ListItem item = new ListItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                        item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                        //list.Add(item);
                        list.Add(item.ToString());
                    }
                }
            }
            return list;
        }
        private void FillFactTableFields(object sender, string factTable, string typeFilter)
        {
            if (!string.IsNullOrWhiteSpace(factTable))
            {
                factTableFields = DashboardCache.GetFactTableFields(HttpContext.Current.GetManagerContext(), factTable.Trim());
                if (factTableFields != null && factTableFields.Count > 0)
                {
                    factTableFields.RemoveAll(x => x.FieldName.EndsWith("User$Id"));
                    if (!string.IsNullOrEmpty(typeFilter))
                    {
                        if (typeFilter.ToLower() == "number")
                        {
                            factTableFields = factTableFields.Where(x => x.DataType.ToLower() == "system.double" ||
                                x.DataType.ToLower() == "system.Int32").ToList();
                        }
                        else
                        {
                            factTableFields = factTableFields.Where(x => x.DataType.ToLower() == typeFilter.ToLower()).ToList();
                        }
                    }

                    factTableFields = factTableFields.OrderBy(x => x.FieldName).ToList();
                }
            }

            if (sender is DropDownList)
            {
                DropDownList list = (DropDownList)sender;
                list.Items.Clear();
                if (factTableFields != null && factTableFields.Count > 0)
                {
                    foreach (FactTableField field in factTableFields)
                    {
                        ListItem item = new ListItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                        item.Attributes.Add("datatype", field.DataType.Replace("System.", string.Empty));
                        list.Items.Add(item);
                    }
                }
            }
            else if (sender is ASPxComboBox)
            {
                ASPxComboBox list = (ASPxComboBox)sender;
                list.Items.Clear();
                if (factTableFields != null && factTableFields.Count > 0)
                {
                    foreach (FactTableField field in factTableFields)
                    {
                        ListEditItem item = new ListEditItem(string.Format("{0}({1})", field.FieldName, field.DataType.Replace("System.", string.Empty)), field.FieldName);
                        list.Items.Add(item);
                    }
                }
            }
        }

        #endregion

        protected void chkLineChartimensions_Load(object sender, EventArgs e)
        {
            ASPxCheckBoxList chkList = sender as ASPxCheckBoxList;
            ChartSetting dashboardPanel = null;
            long.TryParse(hDashboardID.Value, out dashboardID);

            if (dashboardID > 0)
            {
                uDashboard = dManager.LoadPanelById(dashboardID, false);
                dashboardPanel = (ChartSetting)uDashboard.panel;
            }
            if (chkList != null && dashboardPanel != null && dashboardPanel.Dimensions != null && dashboardPanel.Dimensions.Count > 0)
            {
                List<ChartDimension> lstDimensions = dashboardPanel.Dimensions;
                if (chkList.Items.Count == 0)
                    chkList.Items.AddRange(lstDimensions.AsEnumerable().Select(x => x.Title).ToList());
            }
        }


    }
}
