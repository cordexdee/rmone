using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using DevExpress.XtraCharts.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using DevExpress.XtraCharts;

namespace uGovernIT.Web
{
    public partial class CustomDashboardPanel : UserControl
    {
        public Utility.Dashboard dashboard { get; set; }
        public Unit PanelWidth { get; set; }
        public Unit PanelHeight { get; set; }
        public bool IsCheckPanel { get; set; }
        protected string spTheme;
        public Control ContentControl { get; set; }
        public string Title { get; set; }
        public bool UseAjax { get; set; }
        public bool Sidebar { get; set; }
        public string dasbhoardViewPage;
        public string filteredDataDetailPage;

        public string GlobalFilter { get; set; }
        public string LocalFilter { get; set; }
        public int DrillDownIndex { get; set; }
        public string ExpressionFilter { get; set; }
        public int ViewID { get; set; }
        public string BorderStyle { get; set; }
        public bool IsZoom { get; set; }
        public string panelInstanceID;
        public string localFilterValue;
        public bool IsServiceDocPanel { get; set; }

        //ChartSetting setting = null;
        //DevxChartHelper chartHelper = null;
        //static string filter;

        /// <summary>
        /// It puts the drill level filter inside spans
        /// </summary>
        public string DrillDownBackArray { get; set; }
        public CustomDashboardPanel()
        {
            GlobalFilter = string.Empty;
            LocalFilter = string.Empty;
            DrillDownIndex = 0;
            ExpressionFilter = string.Empty;
            panelInstanceID = Guid.NewGuid().ToString();
        }

        //protected override void OnInit(object sender, EventArgs e)
        //{

        //}
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        protected override void OnInit(EventArgs e)
        {
            if (IsServiceDocPanel)
            {
                dashboard_MainContainer.Attributes.Add("class", "dashboard-panel col-md-12 col-xs-12 noPadding");
                //if (Convert.ToBoolean(Request.QueryString["IsServiceDocPanel"]))
            }
            //  uGITDashboardScript.Name = string.Format("{0}?rev={1}", uGITDashboardScript.Name, uGITCache.GetUGovernITDeploymentID());
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ShowDashboardDetails.aspx");
            ConfigurationVariableManager configManager = new ConfigurationVariableManager(context);
            filteredDataDetailPage = UGITUtility.GetAbsoluteURL(configManager.GetValue("FilterTicketsPageUrl"));
            cdpIcon.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px !important;");
            dashboardScriptPanel.Visible = false;
            if (dashboard != null && dashboard.panel != null)
            {
                if (dashboard.panel.StartFromNewLine && !Sidebar)
                {
                    startFromNewLine.Text = "<div style='float:left;width:100%;'></div>";
                }
                if (dashboard.panel is ChartSetting)
                {

                    if (dashboard.ThemeColor.ToLower() == "accent1" || string.IsNullOrEmpty(dashboard.ThemeColor))
                    {
                        dashboard.ThemeColor = "RoundedRectangle";
                    }
                    if (!dashboard.panel.HideZoomView)
                    {

                        Image maximizeIcon = new Image();
                        maximizeIcon.ID = "maximizeicon_" + Guid.NewGuid().ToString().Replace("-","_");
                        maximizeIcon.ImageUrl = "/content/images/maximize-icon12x12.png";
                        maximizeIcon.ToolTip = "Zoom";
                        maximizeIcon.Attributes.Add("onClick", string.Format("maximizeDashboard(this, '{0}');return false;", panelInstanceID));

                        maximizeIcon.CssClass = "cg-dashboardaction-icon";
                        maximizeIcon.Style.Add("float", "right");
                        //maximizeIcon.Style.Add("margin-top", "3px");
                        dashboardActionIcons.Controls.Add(maximizeIcon);
                    }


                    Label moreActions = new Label();
                    moreActions.Style.Add("float", "right");
                    //moreActions.Style.Add(HtmlTextWriterStyle.Display, "none");
                    moreActions.CssClass = "dashboardacton-moreaction";
                    dashboardActionIcons.Controls.Add(moreActions);

                    if (!dashboard.panel.HideTableView)
                    {
                        Image tableIcon = new Image();
                        tableIcon.ID = "tableIcon";
                        tableIcon.Style.Add("margin-right", "3px");
                        tableIcon.ToolTip = "Show as Table";
                        tableIcon.ImageUrl = "/content/images/table-icon12x12.png";
                        tableIcon.Attributes.Add("onClick", string.Format("convertDashboardInTable(this, '{0}')", panelInstanceID));
                        tableIcon.CssClass = "cg-dashboardaction-icon";
                        moreActions.Controls.Add(tableIcon);

                    }

                    if (!dashboard.panel.HidewDownloadView)
                    {
                        Image csvIcon = new Image();
                        csvIcon.ID = "csvIcon";
                        csvIcon.Style.Add("margin-right", "3px");
                        csvIcon.ToolTip = "Export to CSV";
                        csvIcon.ImageUrl = "/content/images/csv-icon12x12.png";
                        csvIcon.Attributes.Add("onClick", string.Format("getCSVOfDashboard(this, '{0}')", panelInstanceID));
                        csvIcon.CssClass = "cg-dashboardaction-icon";
                        moreActions.Controls.Add(csvIcon);
                    }

                    ChartSetting chartSetting = dashboard.panel as ChartSetting;
                    List<ChartDimension> dimensionViews = chartSetting.Dimensions.Where(x => x.ShowInDropDown).ToList(); //chartSetting.Dimensions.Where(x => chartSetting.Dimensions.Exists(y=>y==x) && x.ShowInDropDown).ToList();
                    if (dimensionViews.Count > 0)
                    {
                        DropDownList ddlDimensionFilter = new DropDownList();
                        //   ddlDimensionFilter.Items.Add(new ListItem(chartSetting.Dimensions[0].Title, "0"));
                        foreach (ChartDimension dims in dimensionViews)
                        {
                            ddlDimensionFilter.Items.Add(new ListItem(dims.Title, chartSetting.Dimensions.IndexOf(dims).ToString()));
                        }
                        ddlDimensionFilter.Attributes.Add("onchange", "pmenuDimensionItemClick(this)");
                        ddlDimensionFilter.CssClass = dashboard.ThemeColor.ToLower() + "-dimensionmenu";
                        dashboardLocalContainer.Controls.Add(ddlDimensionFilter);
                        ddlDimensionFilter.SelectedIndex = ddlDimensionFilter.Items.IndexOf(ddlDimensionFilter.Items.FindByValue(DrillDownIndex.ToString()));
                    }

                    if (!string.IsNullOrEmpty(chartSetting.BasicDateFitlerStartField) && !chartSetting.HideDateFilterDropdown)
                    {
                        DropDownList ddlLocalDateFilter = new DropDownList();
                        List<string> dateViews = DashboardCache.GetDateViewList();
                        foreach (string view in dateViews)
                        {
                            ddlLocalDateFilter.Items.Add(new ListItem(view, view));
                        }
                        ddlLocalDateFilter.Items.Insert(0, new ListItem("Select All", "Select All"));
                        ddlLocalDateFilter.Attributes.Add("onChange", string.Format("setLocalDateFilter(this,'{0}')", panelInstanceID));
                        ddlLocalDateFilter.CssClass = dashboard.ThemeColor.ToLower() + "-localdatefilter";
                        if (!string.IsNullOrEmpty(chartSetting.BasicDateFilterDefaultView))
                        {
                            localFilterValue = chartSetting.BasicDateFilterDefaultView;
                            ListItem item = ddlLocalDateFilter.Items.FindByValue(chartSetting.BasicDateFilterDefaultView);
                            if (item != null)
                            {
                                ddlLocalDateFilter.ClearSelection();
                                item.Selected = true;
                            }
                        }

                        //set location filter from request paramenter
                        if (Request["datefilter"] != null && Request["datefilter"].Trim() != string.Empty)
                            this.LocalFilter = Request["datefilter"];

                        if (!string.IsNullOrEmpty(this.LocalFilter))
                        {
                            ListItem item = ddlLocalDateFilter.Items.Cast<ListItem>().FirstOrDefault(x => x.Value.ToLower() == this.LocalFilter.ToLower());
                            if (item != null)
                            {
                                this.LocalFilter = item.Value;
                                ddlLocalDateFilter.ClearSelection();
                                item.Selected = true;
                            }
                            localFilterValue = this.LocalFilter;
                        }

                        ddlLocalDateFilter.Width = 110;
                        dashboardLocalContainer.Controls.Add(ddlLocalDateFilter);
                    }

                    if (!string.IsNullOrEmpty(DrillDownBackArray))
                    {
                        List<string> arrys = UGITUtility.ConvertStringToList(DrillDownBackArray, Constants.Separator);
                        foreach (string ar in arrys)
                        {
                            Label lb = new Label();
                            lb.Text = Uri.UnescapeDataString(ar);
                            drillDownbackFilters.Controls.Add(lb);
                        }
                    }
                }
                else
                {
                    dashboardActionIcons.Visible = false;
                }


                // some width, height and  classs changes with dashboard panel in case of sidebar
                if (Sidebar)
                {
                    dashboardInfoBlock.Style.Add(HtmlTextWriterStyle.FontSize, "9px");
                    panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main-mini");
                    cdpIcon.Width = 20;
                    cdpIcon.Height = 20;
                }
                else
                {
                    cdpIcon.Width = 32;
                    cdpIcon.Height = 32;
                }

                //In case of chart, add class to not move on mouse over
                if (dashboard.DashboardType == DashboardType.Chart && dashboard.panel is ChartSetting)
                {
                    panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main");
                    dashboardInfoBlock.Attributes.Add("blockType", "detail");
                }

                if (!string.IsNullOrEmpty(Title))
                    dashboard.Title = Title;

                //Hide title if not specify to show
                dashboardScriptPanel.Visible = true;
                if (Convert.ToBoolean(dashboard.IsHideTitle) || dashboard.Title.Trim() == string.Empty)
                {
                    cdpTitle.Visible = false;
                }
                else
                {
                    //cdpTitleContainer.Attributes.Add("titletext", Uri.EscapeUriString(dashboard.Title.Trim()));
                    //cdpTitleContainer.Attributes.Add("title", Uri.EscapeUriString(dashboard.DashboardDescription.Trim()));
                    cdpTitleContainer.Attributes.Add("titletext", dashboard.Title.Trim());
                    cdpTitleContainer.Attributes.Add("title", dashboard.DashboardDescription);
                    cdpTitle.Text = dashboard.Title.Trim();
                    string fontStyle = string.Empty;
                    if (!string.IsNullOrEmpty(dashboard.HeaderFontStyle))
                    {
                        string[] vals = dashboard.HeaderFontStyle.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                        if (vals.Length == 3)
                        {
                            cdpTitleContainer.Style.Add("font-weight", vals[0]);
                            cdpTitleContainer.Style.Add("font-size", vals[1]);
                            cdpTitleContainer.Style.Add("color", vals[2]);
                        }
                    }

                }

                //Hide description if not specify to show
                if (Convert.ToBoolean(dashboard.IsHideDescription) || string.IsNullOrEmpty(dashboard.DashboardDescription))
                {
                    cdpDesciption.Visible = false;
                }
                else
                {
                    cdpDesciption.Text = string.Format("<Br/><b class='dashboard-desc'>{0}</b>", dashboard.DashboardDescription);

                }

                //Show icon if exist
                if (string.IsNullOrWhiteSpace(dashboard.Icon))
                {
                    cdpIcon.Visible = false;
                }
                else
                {
                    cdpIcon.ImageUrl = dashboard.Icon;
                }

                PanelWidth = new Unit(dashboard.PanelWidth);
                PanelHeight = new Unit(dashboard.PanelHeight);


                spTheme = dashboard.ThemeColor.ToLower();
                string panelDashbaordClass = panelDashbaordPLink.Attributes["class"];
                if (dashboard.ThemeColor.ToLower() == "rectangle")
                {
                    if (!string.IsNullOrEmpty(panelDashbaordClass))
                    {
                        panelDashbaordClass += " " + dashboard.ThemeColor.ToLower() + "-panelDashboard";
                    }
                    else
                    {
                        panelDashbaordClass = dashboard.ThemeColor.ToLower() + "-panelDashboard";
                    }

                }
                else if (dashboard.ThemeColor.ToLower() == "none")
                {
                    if (!string.IsNullOrEmpty(panelDashbaordClass))
                    {
                        panelDashbaordClass += " panelDashboard";
                    }
                    else
                    {
                        panelDashbaordClass = "panelDashboard";
                    }
                }
                panelDashbaordPLink.Attributes.Add("class", panelDashbaordClass);

                //Do show twick with panel width and height when panel is not in sidebar
                if (!Sidebar)
                {
                    PanelHeight = new Unit(dashboard.PanelHeight - 10);
                    PanelWidth = new Unit(dashboard.PanelWidth - 10);

                    if (dashboard.DashboardType == DashboardType.Chart && dashboard.panel is ChartSetting)
                    {
                        PanelWidth = new Unit(PanelWidth.Value + 5);
                    }
                }

                // width:<%=PanelWidth.Value-10 %>px;height:<%= PanelHeight.Value-30 %>px;top:-3px
                dashboardMainContainer.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", PanelWidth.Value));
                dashboardMainContainer.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", PanelHeight.Value - 30));
                dashboardMainContainer.Style.Add(HtmlTextWriterStyle.Top, "-3px");

                // HtmlGenericControl container = new HtmlGenericControl("Div");
                dashboardMainContainer.Attributes.Add("panelID", dashboard.ID.ToString());
                dashboardMainContainer.Attributes.Add("panelInstanceID", panelInstanceID);


                if (dashboard.DashboardType == DashboardType.Chart && dashboard.panel is ChartSetting)
                {
                    cdpDesciption.Visible = false;

                    WebChartControl chart = new WebChartControl();
                    ChartSetting panel = (ChartSetting)dashboard.panel;
                    DevxChartHelper chartHelper = new DevxChartHelper(panel, context);
                    chart = chartHelper.GetChart(true, dashboard, PanelHeight, false);
                    chart.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
                    chart.BackColor = System.Drawing.Color.White;
                    chart.Legend.Border.Visibility = DevExpress.Utils.DefaultBoolean.False;
                    chart.Legend.MarkerSize = new System.Drawing.Size(10, 10);
                    chart.Legend.Font = new System.Drawing.Font("Poppins !important", 10, System.Drawing.FontStyle.Regular);
                    chart.Legend.TextColor = System.Drawing.ColorTranslator.FromHtml("#9DB9D9");
                    panelDashboardContent.Controls.Add(chart);

                }
                else
                {
                    XmlDocument paneldoc = new XmlDocument();
                    if (dashboard.panel is PanelSetting)
                    {
                        PanelSetting panel = (PanelSetting)dashboard.panel;
                        if (dashboard.Icon.Trim() == string.Empty && panel.IconUrl != null && Convert.ToString(panel.IconUrl) != string.Empty)
                        {
                            cdpIcon.Visible = true;
                            cdpIcon.ImageUrl = UGITUtility.GetAbsoluteURL(panel.IconUrl);
                        }
                        List<DashboardPanelLink> Kpis = panel.Expressions.Where(x => x.IsHide == false).ToList();

                        //It impacts on performance. Don't need to load expression calc to load data. we are showing stats on ajax any way.
                        //List<ExpressionCalc> expressionsCal = new List<ExpressionCalc>();
                        //foreach (DashboardPanelLink kpi in Kpis)
                        //{
                        //    expressionsCal.Add(new ExpressionCalc(SPContext.Current.Web, kpi.DashboardTable));
                        //}
                        //ExpressionCalc.LoadFilterData(ref expressionsCal, filterTable, expressionTable);

                        // For Panel URL
                        if (panel.Expressions.Count > 0 && panel.Expressions.Exists(x => x.UseAsPanel == true))
                        {
                            DashboardPanelLink panelLink = panel.Expressions.FirstOrDefault(x => x.UseAsPanel == true);
                            panelDashbaordPLink.Attributes.Add("title", panelLink.Title);
                            ExpressionCalc exp = new ExpressionCalc(panelLink.DashboardTable, context);
                            string href = string.Empty;
                            if (panelLink.LinkUrl.Trim() != string.Empty)
                            {
                                string url = UGITUtility.GetAbsoluteURL(panelLink.LinkUrl);
                                string startQry = "?";
                                if (panelLink.LinkUrl.Contains("?"))
                                {
                                    startQry = "&";
                                }
                                if (panelLink.FormulaId > 0 || panelLink.ExpressionID > 0)
                                    url += string.Format("{0}fID={1}&eID={2}", startQry, panelLink.FormulaId, panelLink.ExpressionID);
                                href = url;
                                if (panelLink.ViewType == 1)
                                {
                                    href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, panelLink.Title);
                                }
                            }
                            else
                            {
                                href = panelLink.LinkUrl;
                                if (panelLink.ViewType == 1)
                                {
                                    href = string.Format("window.parentUgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", panelLink.LinkUrl, panelLink.Title);
                                }

                                if (panelLink.LinkUrl == string.Empty)
                                {
                                    href = string.Empty;
                                }
                            }
                            if (href != string.Empty)
                            {
                                if (Request["isdlg"] != null)
                                {
                                    if (href.IndexOf("?") == -1)
                                        href += "?";
                                    href = string.Format("{0}&isdlg={1}", href, Request["isdlg"]);
                                }
                                panelDashbaordPLink.Attributes.Add("onclick", string.Format("javascript:window.location.href='{0}'", href));
                            }
                            else
                            {
                                panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main-notmove");
                            }

                            if (panelLink.ViewType == 1)
                            {
                                panelDashbaordPLink.Attributes.Add("onClick", href);
                            }
                        }

                        HtmlGenericControl pContainer = new HtmlGenericControl("Div");

                        hdnChartHide.Value = panel.ChartHide.ToString();
                        tdDounutChart.Visible = true;
                        doughnutChart.Visible = true;

                        //BTS-22-000857: Code changed by Inderjeet Kaur to Hide the blank doughnut chart panel when ChartHide = 0 or 2, display it for ChartHide = 1
                        //Show/hide will be done from JS code in markup.
                        if (panel.ChartHide == 1)
                        {
                            tdDounutChart.Attributes.Add("class", "pr-2");
                        }
                        if ((panel.ChartHide == 1) && panel.ChartType == "DoughnutOnly" || panel.ChartType == "PieOnly")
                        {
                            tdDounutChart.Attributes.Remove("class");
                        }

                        //HtmlGenericControl pTable = new HtmlGenericControl("Table");
                        HtmlGenericControl pTable = new HtmlGenericControl("div");
                        //pTable.Controls.Add(new LiteralControl("<div class='dropdown js__drop_down'>" +
                        //                                            "<a href='#' style='color:#4A6EE1;' class='dropdown-icon fa fa-ellipsis-h js__drop_down_button'></a>" +
                        //                                        "</div>"));
                        pTable.Attributes.Add("cellspacing", "0");

                        pContainer.Controls.Add(pTable);
                        foreach (DashboardPanelLink kpi in Kpis)
                        {
                            if (kpi.Title.Trim() != string.Empty)
                            {
                                string url = ExpressionCalc.GetKPIUrl(context, dashboard.ID, kpi);
                                string href = string.Empty;
                                if (!kpi.StopLinkDetail)
                                {
                                    if (kpi.ScreenView == 1)
                                    {
                                        href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, kpi.Title);
                                    }
                                    else
                                    {
                                        if (Request["isdlg"] != null)
                                        {
                                            if (url.IndexOf("?") == -1)
                                                url += "?";
                                            url = string.Format("{0}&isdlg={1}", url, Request["isdlg"]);
                                        }
                                        href = string.Format("window.location.href=\"{0}\"", url);
                                    }
                                }

                                string cursorType = !string.IsNullOrEmpty(href) ? "pointer" : "default";
                                //string kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'><td style='text-align:right;'>{3}</td><td class='dashboardkpi-td kpiresult' align='center' style='color:{4}'><span class='dashboardkpi-txt' >{0}</span></td></tr>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor); //kpi.ExpressionFormat
                                string kpiString = string.Format("<div class='kpitr progress-group'  style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' onclick='{1}'>" +
                                                                "<span class='progress-text'>{3}</span>" +
                                                                "<span class='progress-number'>{0}</span>" +
                                                                "<div style='height: 2px;'>&nbsp;</div>" +
                                                                "<div class='progress xs'>" +
                                                                    "<div id='dynamic_{2}' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                        "<span id='current-progress'></span>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType); //kpi.ExpressionFormat
                                if (kpi.HideTitle)
                                {
                                    //kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'><td colspan='2' class='dashboardkpi-td kpiresult' align='center' style='color:{4}'><span class='dashboardkpi-txt' >{0}</span></td></tr>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor);
                                    kpiString = string.Format("<div style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'>" +
                                                                "<div class='dashboardkpi-td kpiresult' align='left' style='color:{4}'>" +
                                                                    "<span class='dashboardkpi-txt progress-text'>{0}</span>" +
                                                                "</div>" +
                                                                "<div style='height: 2px;'>&nbsp;</div>" +
                                                                "<div class='progress xs'>" +
                                                                    "<div id='dynamic_{2}' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                        "<span id='current-progress'></span>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType);
                                }
                                //Code added by Inderjeet Kaur on 23-09-2022 to display preview of dashboard panel when Show Bar option is on.
                                if (kpi.ShowBar) {
                                    kpiString = string.Format("<div style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr col-lg-12' onclick='event.cancelBubble = true;{1}'><div class='col-lg-5' style='text-align:right;color:#2D2D2D;'>{3}</div><div class='dashboardkpi-td kpiresult col-lg-7' align='center'  style='color:{4}'><span class='dashboardkpi-txt'>{0}</span></div></div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor);// kpi.ExpressionFormat
                                }
                                if (panel.PanelViewType.EqualsIgnoreCase(((Enum)PanelViewType.Bars).ToString()))
                                {
                                    kpiString = string.Format("<div class='progress-group'  style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'>" +
                                                               "<span class='progress-text'>{3}</span>" +
                                                               "<span class='progress-number'>{0}</span>" +
                                                               "<div style='height: 2px;'>&nbsp;</div>" +
                                                               "<div class='progress xs'>" +
                                                                   "<div class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                       "<span id='current-progress'></span>" +
                                                                   "</div>" +
                                                               "</div>" +
                                                           "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType); //kpi.ExpressionFormat

                                    if (kpi.HideTitle)
                                    {
                                        //add required string 
                                        kpiString = string.Format("<div style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'>" +
                                                                "<div class='kpiresult col-md-4' align='left' style='color:{4}'>" +
                                                                    "<span class='dashboardkpi-txt progress-text'>{0}</span>" +
                                                                "</div>" +
                                                                "<div class='col-md-8 noPaddingRight'><div class='progress barcharts-progress xs'>" +
                                                                    "<div id='dynamic_{2}' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                        "<span id='current-progress'></span>" +
                                                                    "</div>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType);

                                    }


                                }
                                if (panel.PanelViewType.EqualsIgnoreCase(((Enum)PanelViewType.Circular).ToString()))
                                {
                                    kpiString = string.Format("<div class='progress-group'  style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr cicularChart-container' onclick='{1}'>" +
                                                               "<span class='progress-text'>{3}</span>" +
                                                               "<span class='progress-number'>{0}</span>" +
                                                               "<div style='height: 2px;'>&nbsp;</div>" +
                                                               "<div class='progress xs'>" +
                                                                   "<div class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                       "<div class='cicular-progress' id='current-progress'></div>" +
                                                                   "</div>" +
                                                               "</div>" +
                                                           "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType); //kpi.ExpressionFormat

                                    if (kpi.HideTitle)
                                    {
                                        //add required string 
                                        kpiString = string.Format("<div style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr cicularChart-container' onclick='{1}'>" +
                                                                "<div class=' noPaddingRight'><div class='progress barcharts-progress xs'>" +
                                                                    "<div id='dynamic_{2}' class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                        "<div class='cicular-progress' id='current-progress'></div>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                                "</div>" +
                                                                "<div class='kpiresult circular-chart-label' align='left' style='color:{4}'>" +
                                                                    "<span class='dashboardkpi-txt progress-text'>{0}</span>" +
                                                                "</div>" +
                                                                "</div>" +
                                                            "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType);

                                    }


                                }
                                //kpiString = kpiString.Replace("$exp$", kpi.ExpressionFormat);
                                if (panel.ChartType == "DoughnutOnly")
                                {
                                    // divProgressChart.Visible = false;
                                }
                                pTable.Controls.Add(new LiteralControl(kpiString));
                            }
                        }
                        panelDashboardContent.Controls.Add(pContainer);

                    }
                }

                if (ExpressionFilter != string.Empty)
                {
                    chartReturnIcon.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "block");
                }
            }
            else if (ContentControl != null)
            {
                panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main-mini");
                dashboardDetail.Visible = false;
                panelDashboardContent.Controls.Add(ContentControl);
                spTheme = "accent1";
            }
            base.OnInit(e);

            if (Page != null && Page.IsCallback)
                EnsureChildControls();
        }



    }
}

