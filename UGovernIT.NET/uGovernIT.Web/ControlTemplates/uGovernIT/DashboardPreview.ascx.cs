using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using DevExpress.XtraCharts.Web;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Web.ControlTemplates;
using static uGovernIT.Utility.Constants;
using uGovernIT.Web.ControlTemplates.CoreUI;
using uGovernIT.Web.ControlTemplates.Bench;

namespace uGovernIT.Web
{
    public partial class DashboardPreview : UserControl
    {
        protected string filterTicketsPageUrl = string.Empty;
        protected string dasbhoardViewPage = string.Empty;
        public string DashboardView { get; set; }
        public int DashboardViewID { get; set; }
        string analyticUrl = string.Empty;
        protected int defaultHeight = 120;
        protected int defaultWidth = 150;
        protected int defaultTop = 0;
        protected int defaultLeft = 0;

        protected int heightOffset = 36;
        protected int widthOffset = 20;
        protected int topOffset = 0;
        protected int leftOffset = 0;
        protected DataTable table = new DataTable();
        DashboardPanelViewManager dViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        private string GetFontStyle(string value)
        {
            StringBuilder sbfontStyle = new StringBuilder();


            if (!string.IsNullOrEmpty(value))
            {
                List<string> lst = value.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (lst[0] == "Bold")
                {
                    sbfontStyle.Append("font-weight: bold;");
                }
                else if (lst[0] == "Italic")
                {
                    sbfontStyle.Append("font-style: italic;font-weight: normal;");
                }
                else if (lst[0] == "Underline")
                {
                    sbfontStyle.Append("text-decoration: underline;font-weight: normal;");
                }
                else if (lst[0] == "Regular")
                {
                    sbfontStyle.Append("font-weight: normal;");
                }

                if (lst.Count > 2)
                {
                    sbfontStyle.Append("font-size:" + lst[1] + ";color:" + lst[2] + ";");
                }
                else
                {
                    sbfontStyle.Append("font-size:" + lst[1] + ";color:black;");
                }

                if (lst.Count > 3)
                {
                    sbfontStyle.AppendFormat("font-family:{0};", lst[3]);
                }
            }

            return sbfontStyle.ToString();
        }

        private int GetViewHeight(List<DashboardPanelProperty> lstDashboard)
        {
            int maxHeight = 0;

            foreach (DashboardPanelProperty dashboardPanelProperty in lstDashboard)
            {
                if (dashboardPanelProperty.Top + dashboardPanelProperty.Height > maxHeight)
                {
                    maxHeight = dashboardPanelProperty.Top + dashboardPanelProperty.Height;
                }
            }
            return maxHeight;
        }

        private int GetViewWidth(List<DashboardPanelProperty> lstDashboard)
        {
            int maxWidth = 0;

            foreach (DashboardPanelProperty dashboardPanelProperty in lstDashboard)
            {
                if (dashboardPanelProperty.Left + dashboardPanelProperty.Width > maxWidth)
                {
                    maxWidth = dashboardPanelProperty.Left + dashboardPanelProperty.Width;
                }
            }
            return maxWidth;
        }

        private void GetPixelFromPercentage(List<DashboardPanelProperty> lstDashboard)
        {
            //if (!string.IsNullOrEmpty(UGITUtility.GetCookieValue(Request, "originalScreenWidth")))
            //{
            //    strWidth = UGITUtility.GetCookieValue(Request, "originalScreenWidth");
            //}
            //else
            //{
            //    strWidth = UGITUtility.GetCookieValue(Request, "screenWidth");
            //}
            string strWidth = UGITUtility.GetCookieValue(Request, "screenWidth");
            strWidth = (string.IsNullOrWhiteSpace(strWidth) || string.IsNullOrEmpty(strWidth) || strWidth=="0") ? "1360" : strWidth;
            if (int.TryParse(strWidth, out int screenWidth))
            {
                foreach (DashboardPanelProperty dashboardPanelProperty in lstDashboard)
                {
                    if (dashboardPanelProperty.WidthUnitType == UnitType.Percentage)
                    {
                        if (dashboardPanelProperty.Width > 0 && dashboardPanelProperty.Width <= 100)
                        {
                            dashboardPanelProperty.Width = (screenWidth - 55) * dashboardPanelProperty.Width / 100;
                            dashboardPanelProperty.WidthUnitType = UnitType.Pixel;
                        }
                    }
                    if (dashboardPanelProperty.LeftUnitType == UnitType.Percentage)
                    {
                        if (dashboardPanelProperty.Left > 0 && dashboardPanelProperty.Left <= 100)
                        {
                            dashboardPanelProperty.Left = (screenWidth - 55) * dashboardPanelProperty.Left / 100;
                            dashboardPanelProperty.LeftUnitType = UnitType.Pixel;
                        }
                    }
                }
            }
        }

        private int GetDefaultTopOffset(List<DashboardPanelProperty> lstDashboard)
        {

            int minHeight = lstDashboard.Count > 0 ? lstDashboard[0].Top : 0;
            foreach (DashboardPanelProperty dashboardPanelProperty in lstDashboard)
            {
                if (dashboardPanelProperty.Top < minHeight)
                {
                    minHeight = dashboardPanelProperty.Top;
                }
            }
            return minHeight;
        }

        private int GetDefaultLeftOffset(List<DashboardPanelProperty> lstDashboard)
        {
            int minWidth = lstDashboard.Count > 0 ? lstDashboard[0].Left : 0;
            foreach (DashboardPanelProperty dashboardPanelProperty in lstDashboard)
            {
                if (dashboardPanelProperty.Left < minWidth)
                {
                    minWidth = dashboardPanelProperty.Left;
                }
            }
            return minWidth;
        }

        protected override void OnInit(EventArgs e)
        {
            //uGITDashboardScript.Name = string.Format("{0}?rev={1}", uGITDashboardScript.Name, uGITCache.GetUGovernITDeploymentID());


            // Check if the request is coming from a blackberry device.
            // If yes then redirect it to the mobile page.
            // This should be done in compat.browser.
            //if (Redirect.IsMobileRequest(Request.UserAgent))
            //{
            //    Response.Redirect(Redirect.GetMobileDashboardURL(SPContext.Current.Web));
            //}


            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ShowDashboardDetails.aspx");
            filterTicketsPageUrl = UGITUtility.GetAbsoluteURL(configManager.GetValue("FilterTicketsPageUrl"));

            DashboardPanelView dashboardView = null;
            if (DashboardViewID > 0)
                dashboardView = dViewManager.LoadViewByID(DashboardViewID, false);
            else
                dashboardView = dViewManager.LoadViewByName(DashboardView);

            List<Dashboard> dashboards = null;
            if (dashboardView != null && dashboardView.ViewType == "Common Dashboards")
            {
                CommonDashboardsView cmDashboardView = (dashboardView.ViewProperty as CommonDashboardsView);
                
                if (cmDashboardView == null)
                {
                    return;
                }

                //int ogScreenWidth = 0;
                
                //if(!Page.IsPostBack)
                //{
                //    //UGITUtility.DeleteCookie(Request, Response, "originalScreenWidth");
                //    SetCookie("originalScreenWidth", "");
                //}
                
                //ogScreenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "originalScreenWidth"));

                //if (ogScreenWidth == 0 || string.IsNullOrEmpty(ogScreenWidth.ToString()))
                //{
                //    int screenWidth = UGITUtility.StringToInt(UGITUtility.GetCookieValue(Request, "screenWidth"));
                //    //UGITUtility.DeleteCookie(Request, Response, "originalScreenWidth");
                //    SetCookie("originalScreenWidth", Convert.ToString(screenWidth));
                //}

                GetPixelFromPercentage(cmDashboardView.Dashboards);
                if (cmDashboardView.LayoutType == DashboardLayoutType.FixedSize)
                {
                    dvDashboards.Style.Add("width", cmDashboardView.ViewWidth + "px");
                    dvDashboards.Style.Add("height", cmDashboardView.ViewHeight + "px");

                    dvDashboardBG.Style.Add("width", cmDashboardView.ViewWidth + "px");
                    dvDashboardBG.Style.Add("height", cmDashboardView.ViewHeight + "px");

                    divContainer.Style.Add("width", cmDashboardView.ViewWidth + "px");
                    divContainer.Style.Add("height", cmDashboardView.ViewWidth + "px");
                }
                else
                {
                    topOffset = cmDashboardView.PaddingTop - Math.Abs(GetDefaultTopOffset(cmDashboardView.Dashboards));
                    leftOffset = cmDashboardView.PaddingLeft - Math.Abs(GetDefaultLeftOffset(cmDashboardView.Dashboards));
                    dvDashboards.Style.Add("width", GetViewWidth(cmDashboardView.Dashboards) + leftOffset + cmDashboardView.PaddingRight + "px");
                    dvDashboards.Style.Add("height", GetViewHeight(cmDashboardView.Dashboards) + topOffset + cmDashboardView.PaddingBottom + "px");

                    int bgOffset = 0;

                    dvDashboardBG.Style.Add("width", GetViewWidth(cmDashboardView.Dashboards) + bgOffset + leftOffset + cmDashboardView.PaddingRight + "px");
                    dvDashboardBG.Style.Add("height", GetViewHeight(cmDashboardView.Dashboards) + bgOffset + topOffset + cmDashboardView.PaddingBottom + "px");

                    divContainer.Style.Add("width", GetViewWidth(cmDashboardView.Dashboards) + bgOffset + leftOffset + cmDashboardView.PaddingRight + "px");
                    divContainer.Style.Add("height", GetViewHeight(cmDashboardView.Dashboards) + 26 + bgOffset + topOffset + cmDashboardView.PaddingBottom + "px");
                }
                dvDashboardBG.Style.Add("background-image", "url(" + cmDashboardView.ViewBackground + ");");
                dvDashboardBG.Style.Add("background-size", "cover");
                if (cmDashboardView.IsThemable)
                {
                    string currentDevExTheme = DevExpress.Web.ASPxWebControl.GlobalTheme;
                    //if (currentDevExTheme.ToLower() != "ugitclassic")
                    //{
                    dvDashboardBG.CssClass += string.Format("dxnb-itemHover");
                    dvDashBoard.CssClass += string.Format(" dxnbLite_{0}", currentDevExTheme);
                    //}
                }
                else
                {
                    dvDashboardBG.Style.Add("background-color", (!string.IsNullOrEmpty(cmDashboardView.ViewBackgroundColor) ? string.Format("#{0}", cmDashboardView.ViewBackgroundColor) : "#FFFFFF"));
                }
                //   
                string borderStyle = string.Empty; // 1px solid
                if (!string.IsNullOrEmpty(cmDashboardView.BorderWidth))
                {
                    borderStyle = string.Format("{0}px solid ", cmDashboardView.BorderWidth);
                    if (!string.IsNullOrEmpty(cmDashboardView.BorderColor))
                    {
                        borderStyle += string.Format("#{0}", cmDashboardView.BorderColor);
                    }
                }

                dvDashboardBG.Style.Add("border", borderStyle);
                dvDashboards.Style.Add("border", borderStyle);
                //dvDashboardBG.Style.Add("background-repeat", "no-repeat");
                //dvDashboardBG.Style.Add("position", "relative");

                dvDashboardBG.Style.Add("opacity", cmDashboardView.Opacity.ToString());
                //dvDashboards.Style.Add("overflow", "auto");

                //  dvDashboards.Style.Add("position", "absolute");
                dvDashboardBG.Style.Add("position", "absolute");




                var dashboradList = cmDashboardView.Dashboards;
                dashboradList.OrderBy(m => m.ItemOrder);
                dashboards = dManager.LoadDashboardsByNames(dashboradList.Select(x => x.DashboardName).ToList(), false);
                for (int i = 0; i < dashboards.Count; i++)
                {
                    var DashboardIndView = dashboradList.FirstOrDefault(x => x.DashboardName == dashboards[i].Title);
                    if (DashboardIndView != null && DashboardIndView.DisableInheritDefault)
                    {
                        dashboards[i].PanelHeight = DashboardIndView.Height;
                        dashboards[i].ThemeColor = DashboardIndView.Theme;
                        dashboards[i].ItemOrder = DashboardIndView.ItemOrder;
                        dashboards[i].PanelWidth = DashboardIndView.Width;
                        dashboards[i].Title = DashboardIndView.DisplayName;
                        dashboards[i].Icon = DashboardIndView.IconUrl;
                        dashboards[i].Top = DashboardIndView.Top;
                        dashboards[i].Left = DashboardIndView.Left;
                    }
                }


                dashboards = dashboards.OrderBy(t => t.ItemOrder).ToList();

                foreach (DashboardPanelProperty dashboardPanelProperty in cmDashboardView.Dashboards)
                {
                    if (string.IsNullOrEmpty(dashboardPanelProperty.DashboardType))
                        continue;

                    DashboardType dType = (DashboardType)Enum.Parse(typeof(DashboardType), dashboardPanelProperty.DashboardType, true);


                    List<Dashboard> lstDasboards = dManager.LoadDashboardPanelsByType(dType, true, HttpContext.Current.GetManagerContext().TenantID);
                    //UDashboard dashboard = UDashboard.LoadDashboardsByNames(new List<string>() { dashboardPanelProperty.DashboardName }).FirstOrDefault();
                    Dashboard dashboard = lstDasboards.FirstOrDefault(x => x.Title == dashboardPanelProperty.DashboardName);


                    //Check current user has permission to see dashboard or not
                    if (dashboard == null || string.IsNullOrWhiteSpace(dashboard.DashboardPermission) || dashboard.DashboardPermission.Contains(HttpContext.Current.CurrentUser().Id))
                    {
                        // Opt out dashboard which are not inside fixed width container
                        if (cmDashboardView.LayoutType == DashboardLayoutType.FixedSize)
                        {
                            if (dashboardPanelProperty.Top + dashboardPanelProperty.Height / 2 > cmDashboardView.ViewHeight
                                || dashboardPanelProperty.Left + dashboardPanelProperty.Width / 2 > cmDashboardView.ViewWidth
                                )
                            {
                                continue;
                            }
                        }

                        
                        int width = dashboardPanelProperty.Width > 0 ? dashboardPanelProperty.Width : defaultWidth;
                        int height = dashboardPanelProperty.Height > 0 ? dashboardPanelProperty.Height : defaultHeight;


                        int top = (dashboardPanelProperty.Top > 0 ? dashboardPanelProperty.Top : defaultTop) + topOffset;
                        int left = (dashboardPanelProperty.Left > 0 ? dashboardPanelProperty.Left : defaultLeft) + leftOffset;
                        int zindex = (dashboardPanelProperty.Zindex > 0 && dashboardPanelProperty.Zindex <= 100 ? dashboardPanelProperty.Zindex : 100);

                        string theme = string.Empty;
                        string style = string.Empty;
                        if (!string.IsNullOrEmpty(dashboardPanelProperty.Theme))
                        {
                            if (dashboardPanelProperty.Theme == "Rectangle")
                            {
                                theme = "rectangle";
                                style = string.Format("border: 1px solid {0};", dashboardPanelProperty.BorderColor);
                                //height -= heightOffset;
                            }
                            else if (dashboardPanelProperty.Theme == "Rounded Rectangle")
                            {
                                theme = "roundedrectangle";
                                height -= heightOffset;
                            }
                        }

                        HtmlGenericControl QContainer = new HtmlGenericControl("Div");
                        HtmlGenericControl panelDashbaordPLink = new HtmlGenericControl("Div");
                        HtmlGenericControl table = new HtmlGenericControl("table");
                        HtmlGenericControl tr = new HtmlGenericControl("tr");
                        HtmlGenericControl td = new HtmlGenericControl("td");

                        QContainer.Attributes.Add("class", "dashboard-panel-new");
                        QContainer.Style.Add("top", top + "px");
                        QContainer.Style.Add("left", left + "px");
                        QContainer.Style.Add("position", "absolute");
                        QContainer.Style.Add("z-index", Convert.ToString(zindex));

                        // Currently it is only for Panels & Messageboard
                        if (dashboardPanelProperty.Theme == "Transparent")
                        {
                            QContainer.Attributes.Add("theme", "Transparent");
                            QContainer.Style.Add("background", "transparent");
                        }

                        QContainer.Attributes.Add("onmouseover", "dashboardShowActions(this);");
                        QContainer.Attributes.Add("onmouseout", "dashboardHideActions(this);");

                        if ((dashboardPanelProperty.Theme == "Rectangle" && !dashboardPanelProperty.IsLink && (dashboard != null && dashboard.DashboardType != DashboardType.Panel)) || (dashboardPanelProperty.Theme == "Rectangle" && dashboardPanelProperty.DashboardType == DashboardType.Control.ToString()))
                        {
                            QContainer.Style.Add("border", string.Format("1px solid {0}", dashboardPanelProperty.BorderColor));
                            panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main fillShape-Rectangle");
                        }
                        else if (dashboardPanelProperty.Theme == "Rounded Rectangle")
                        {
                            panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main fillShape-RoundedRectangle");
                        }
                        else if(dashboardPanelProperty.Theme == "Transparent")
                        {
                            panelDashbaordPLink.Attributes.Add("class", "no-class");
                        }
                        else
                        {
                            panelDashbaordPLink.Attributes.Add("class", "dashboard-panel-main");
                        }



                        panelDashbaordPLink.Attributes.Add("id", "panelDashbaordPLink");

                        table.Attributes.Add("cellpadding", "0");
                        table.Attributes.Add("cellspacing", "0");
                        table.Attributes.Add("id", "panelDashbaordPLink");
                        table.Style.Add("border-collapse", "collapse");

                        string topBorder = "<tr><td class=\"" + theme + "-panel-topleft-corner\" align='left' width='20px'><img class=\"drilldownback\" onclick='returnFromDrillDown(this)' id=\"chartReturnIcon\" runat=\"server\" style=\"display:none\" src=\"/content/images/return.png\" alt=\"Back\"/>"
                         + "<span class='drilldownback-filters' style='display:none;'></span> </td>"
                         + "<td class='" + theme + "-middletop-rep' width='" + (width - 40) + "px; align='right' height=0px>";

                        string title = dashboardPanelProperty.IsHideTitle ? string.Empty : dashboardPanelProperty.DisplayName;

                        if (dashboard != null)
                        {
                            if (dashboard.panel is ChartSetting)
                            {

                                topBorder = "<tr><td class=\"" + theme + "-panel-topleft-corner\" align='left' width='20px'><img class=\"drilldownback\" onclick='returnFromDrillDown(this)' id=\"chartReturnIcon\" runat=\"server\" style=\"display:none\" src=\"/content/images/return.png\" alt=\"Back\"/>"
                        + "<span class='drilldownback-filters' style='display:none;'></span> </td>"
                        + "<td class='" + theme + "-middletop-rep' width='" + (width - 40) + "px; align='right' height=" + (string.IsNullOrEmpty(theme) ? 0 : 15) + "px>";

                                if (!dashboard.panel.HideZoomView)
                                    topBorder += "<img id='maximizeicon' class='" + theme + "-dashboardaction-icon' title='Zoom' onclick='maximizeDashboard(this, \"" + dashboard.ID + "\")' src='/content/images/maximize-icon12x12.png' style='border-width:0px;z-index:1;'/>";
                                topBorder += "<span class='dashboardacton-moreaction' style='display:none;'>";
                                if (!dashboard.panel.HideTableView)
                                    topBorder += "<img id='tableIcon' class='" + theme + "-dashboardaction-icon' title='Show as Table' onclick='convertDashboardInTable(this, \"" + dashboard.ID + "\")' src='/content/images/table-icon12x12.png' style='border-width:0px;z-index:1;'/>";
                                if (!dashboard.panel.HidewDownloadView)
                                    topBorder += "<img id='csvIcon' class='" + theme + "-dashboardaction-icon' title='Export to CSV' onclick='getCSVOfDashboard(this, \"" + dashboard.ID + "\")' src='/content/images/csv-icon12x12.png' style='border-width:0px;z-index:1;'/>";
                                topBorder += "</span>";
                            }

                        }

                        topBorder += "</td><td class='" + theme + "-panel-topright-corner' align='right'></td></tr>";
                        table.Controls.Add(new LiteralControl(topBorder));
                        table.Controls.Add(tr);
                        tr.Controls.Add(td);
                        td.Attributes.Add("class", "panel-content");
                        td.Attributes.Add("colspan", "3");

                        HtmlGenericControl tbMain = new HtmlGenericControl("table");
                        HtmlGenericControl trMain = new HtmlGenericControl("tr");
                        HtmlGenericControl tdMain = new HtmlGenericControl("td");

                        td.Controls.Add(tbMain);
                        tbMain.Controls.Add(trMain);
                        tbMain.Attributes.Add("cellpadding", "0");
                        tbMain.Attributes.Add("cellspacing", "0");
                        tbMain.Attributes.Add("width", "100%");
                        tbMain.Style.Add("border-collapse", "collapse");

                        trMain.Controls.Add(new LiteralControl("<td class='" + theme + "-leftside-border'></td>"));
                        trMain.Controls.Add(tdMain);
                        trMain.Controls.Add(new LiteralControl("<td class='" + theme + "-rightside-border'></td>"));

                        tdMain.Attributes.Add("align", "center");
                        //tdMain.Style.Add("background", "#fff");
                        tdMain.Attributes.Add("id", "tdMain");
                        if (theme == "roundedrectangle")
                            tdMain.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F8F8F8");





                        string bottomBorder = "<tr><td class='" + theme + "-panel-bottomleft-corner' align='left' valign='top'></td><td class='" + theme + "-middledown-rep' align='right' valign='top'>";

                        #region "filter dropdown" and dimension dropdown

                        if (dashboard != null && dashboard.panel is ChartSetting)
                        {
                            ChartSetting chartSetting = dashboard.panel as ChartSetting;
                            List<ChartDimension> dimensionViews = chartSetting.Dimensions.Where(x => chartSetting.Dimensions.IndexOf(x) != 0 && x.ShowInDropDown).ToList();
                            if (dimensionViews.Count > 0)
                            {
                                bottomBorder += "<select class='" + theme + "-dimensionmenu' onchange='pmenuDimensionItemClick(this)' style='width:110px;'>";
                                bottomBorder += string.Format("<option value='{1}'>{0}</option>", chartSetting.Dimensions[0].Title, "0");
                                foreach (ChartDimension dims in dimensionViews)
                                {
                                    bottomBorder += string.Format("<option value='{1}'>{0}</option>", dims.Title, chartSetting.Dimensions.IndexOf(dims));
                                }
                                bottomBorder += "</select>";
                            }


                            if (!string.IsNullOrEmpty(chartSetting.BasicDateFitlerStartField) && !chartSetting.HideDateFilterDropdown)
                            {
                                bottomBorder += "<select name='ctl01' class='" + theme + "-localdatefilter' onchange='setLocalDateFilter(this,\"" + dashboard.ID + "\")' style='width:110px;'>"
                                       + "<option value='Select All'>Select All</option>";

                                List<string> dateViews = DashboardCache.GetDateViewList();
                                foreach (string view in dateViews)
                                {
                                    if (view.Trim().ToLower() == chartSetting.BasicDateFilterDefaultView.Trim().ToLower())
                                    {
                                        bottomBorder += string.Format("<option selected='selected' value='{0}'>{0}</option>", view);
                                    }
                                    else
                                    {
                                        bottomBorder += string.Format("<option value='{0}'>{0}</option>", view);
                                    }
                                }

                                bottomBorder += "</select>";
                            }



                        }

                        #endregion

                        bottomBorder += "</td><td align='right' valign='top' class='" + theme + "-panel-bottomright-corner' ></td></tr>";

                        table.Controls.Add(new LiteralControl(bottomBorder));

                        if (dashboard != null && dashboard.DashboardType == DashboardType.Query)
                        {
                            QueryWizardPreview queryWizardPreview = (QueryWizardPreview)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/QueryWizardPreview.ascx");
                            queryWizardPreview.Id = dashboard.ID;
                            queryWizardPreview.setWidth = width;
                            queryWizardPreview.setHeight = height;
                            queryWizardPreview.where = dashboardPanelProperty.QueryParameter;

                            HtmlGenericControl dvUserControl = new HtmlGenericControl("Div");
                            dvUserControl.Style.Add("width", width + "px");
                            dvUserControl.Style.Add("height", height + "px");
                            dvUserControl.Style.Add("overflow", "auto");
                            dvUserControl.Controls.Add(queryWizardPreview);
                            tdMain.Controls.Add(dvUserControl);
                        }
                        else if (dashboard != null && dashboard.DashboardType == DashboardType.Chart && dashboard.panel is ChartSetting)
                        {
                            if (dashboardPanelProperty.Theme != "Rounded Rectangle")
                            {
                                height -= heightOffset;
                            }
                            ChartSetting panel = (ChartSetting)dashboard.panel;

                            object json = "{ViewID:" + DashboardViewID + ", PanelID: " + (dashboard != null ? Convert.ToString(dashboard.ID) : string.Empty) + ", Sidebar:false, Width:" + width + ", Height:" + height + ", viewType: \"0\", Type:\"" + (dashboard.DashboardType == DashboardType.Chart ? "chart" : "panel") + "\", LocalFilter:\"" + panel.BasicDateFilterDefaultView + "\", DimensionFilter :\"\", ExpressionFilter:\"\", GlobalFilter:\"\", BorderStyle:\"" + dashboardPanelProperty.Theme + "\", Description:\"" + Uri.EscapeDataString(dashboard.DashboardDescription) + "\" }";

                            HtmlGenericControl div1 = new HtmlGenericControl();



                            div1.Controls.Add(new LiteralControl("<div class='dashboardtitle111' title='" + Uri.EscapeDataString(dashboard.DashboardDescription) + "' titletext='" + Uri.EscapeDataString(title) + "'><b><p style='margin-top: 2px;margin-bottom: 2px;text-align:left;'>" + title + "</p></b></div>"));
                            HtmlGenericControl div2 = new HtmlGenericControl();
                            div2.ID = "panelDashboardContent";
                            div2.Attributes.Add("class", "d-content");
                            div2.Style.Add(HtmlTextWriterStyle.Position, "relative");

                            WebChartControl chart = new WebChartControl();

                            DevxChartHelper chartHelper = new DevxChartHelper(panel, HttpContext.Current.GetManagerContext());

                            dashboard.PanelWidth = width - widthOffset - 10;
                            chart = chartHelper.GetChart(true, dashboard, new Unit(height, UnitType.Pixel), false);
                            chart.Style.Add(HtmlTextWriterStyle.Margin, "3px");

                            div1.Attributes.Add("class", "panel-content-header  dashboardpanelcontainer");
                            div1.Attributes.Add("panelInstanceID", dashboard.ID.ToString());
                            div1.Style.Add(HtmlTextWriterStyle.Width, width.ToString() + "px");
                            div1.Style.Add(HtmlTextWriterStyle.Height, height.ToString() + "px");
                            div1.Style.Add(HtmlTextWriterStyle.TextAlign, "center");

                            div2.Controls.Add(chart);
                            div1.Controls.Add(div2);
                            tdMain.Controls.Add(div1);
                            div1.Controls.Add(new LiteralControl("<img style='float: right;padding-right: 2px;padding-top: 2px;display:none' onload='loadDashboard(" + json + ");' onclick='CloseDashboard(this);' src='/content/images/cross_icn.png'/>"));
                        }
                        else if (dashboard != null && dashboard.DashboardType == DashboardType.Panel)
                        {
                            #region KPIs

                            object json = "{ViewID:" + DashboardViewID + ", PanelID: " + (dashboard != null ? Convert.ToString(dashboard.ID) : string.Empty) + ", Sidebar:false, Width:" + width + ", Height:" + height + ", viewType: \"0\", Type:\"" + (dashboard.DashboardType == DashboardType.Chart ? "chart" : "panel") + "\", LocalFilter:\"\", DimensionFilter :\"\", ExpressionFilter:\"\", GlobalFilter:\"\" }";


                            string fontStyle = GetFontStyle(dashboardPanelProperty.FontStyle);
                            string fontHeaderStyle = GetFontStyle(dashboardPanelProperty.HeaderFontStyle);

                            int titleTop = dashboardPanelProperty.TitleTop > dashboardPanelProperty.Height ? dashboardPanelProperty.Height : dashboardPanelProperty.TitleTop;
                            int titleLeft = dashboardPanelProperty.TitleLeft > dashboardPanelProperty.Width ? dashboardPanelProperty.Width : dashboardPanelProperty.TitleLeft;


                            string titlePosition = string.Format("position: absolute;left: {0}px;top:{1}px;", titleLeft, titleTop);

                            //string KPIsHeaderHtml = "<div id='dashboardInfoBlock' style='float: left; width: 100%; font-weight:bold;'>" +
                            string KPIsHeaderHtml = "<div id='dashboardInfoBlock' style='height: 6px;'>" +
                           "<span style ='" + titlePosition + fontHeaderStyle + "' id='cdpTitleContainer' class='dashboardtitle111' title='" + Uri.EscapeDataString(dashboard.DashboardDescription) + "' titletext='" + Uri.EscapeDataString(title) + "'>" + title + "</span></div>";


                            string KPIsHtml = string.Format("{0}<div class='bars-cont d-flex flex-column justify-content-between'><Table style='display: none' cellspacing='0' width='80%'>", KPIsHeaderHtml); ;

                            if (dashboardPanelProperty.PanelLeft > 0 || dashboardPanelProperty.PanelTop > 0)
                            {
                                int pLeft = dashboardPanelProperty.PanelLeft > width ? width : dashboardPanelProperty.PanelLeft;
                                int pTop = dashboardPanelProperty.PanelTop > height ? height : dashboardPanelProperty.PanelTop;
                                KPIsHtml = string.Format("{0}<div style='position: absolute;top: " + pTop + "px;left: " + pLeft + "px;'><Table style='display: none' cellspacing='0' width='80%'>", KPIsHeaderHtml);
                            }

                            string KPIsHtmlEnd = "</Table></div>";
                            XmlDocument paneldoc = new XmlDocument();
                            if (dashboard.panel is PanelSetting)
                            {
                                PanelSetting panel = (PanelSetting)dashboard.panel;




                                List<DashboardPanelLink> Kpis = panel.Expressions.Where(x => x.IsHide == false).ToList();
                                List<ExpressionCalc> expressionsCal = new List<ExpressionCalc>();
                                foreach (DashboardPanelLink kpi in Kpis)
                                {
                                    expressionsCal.Add(new ExpressionCalc(kpi.DashboardTable, HttpContext.Current.GetManagerContext()));
                                }
                                //ExpressionCalc.LoadFilterData(ref expressionsCal, filterTable, expressionTable);

                                // For Panel URL
                                if (panel.Expressions.Count > 0 && panel.Expressions.Exists(x => x.UseAsPanel == true))
                                {
                                    DashboardPanelLink panelLink = panel.Expressions.FirstOrDefault(x => x.UseAsPanel == true);
                                    panelDashbaordPLink.Attributes.Add("title", panelLink.Title);


                                    ExpressionCalc exp = new ExpressionCalc(panelLink.DashboardTable, HttpContext.Current.GetManagerContext());
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
                                            href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", panelLink.LinkUrl, panelLink.Title);
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

                                ExpressionCalc calc = new ExpressionCalc(HttpContext.Current.GetManagerContext());
                                foreach (DashboardPanelLink kpi in Kpis)
                                {
                                    if (kpi.Title.Trim() != string.Empty)
                                    {
                                        string url = calc.GetKPIUrl(dashboard.ID, kpi);
                                        string href = string.Empty;
                                        if (!kpi.StopLinkDetail)
                                        {
                                            if (kpi.ScreenView == 1)
                                            {
                                                href = string.Format("showPanel_filterData(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",\"{2}\")", url, kpi.Title, dashboard.ID);
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
                                        string kpiString = string.Format("<div class='progress-group'  style='cursor:{5};' usepopup='1' kpilinkid='{2}'  title='{3}' fStyle='" + dashboard.FontStyle + "' class='kpitr' onclick='{1}'>" +
                                                                        "<span class='progress-text'>{3}</span>" +
                                                                        "<span class='progress-number'>{0}</span>" +
                                                                        "<div style='height: 2px;'>&nbsp;</div>" +
                                                                        "<div class='progress xs'>" +
                                                                            "<div class='progress-bar' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width: 0%'>" +
                                                                                "<span id='current-progress'></span>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor, cursorType);
                                        //string kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='{1}'><td style='text-align:right;" + fontStyle + "'>{3}</td><td class='kpiresult' align='center' style='" + fontStyle + "'><span class='dashboardkpi-txt' >{0}</span></td></tr>", kpi.ExpressionFormat, href, kpi.LinkID, kpi.Title, kpi.FontColor);  //'dashboardkpi-td 
                                        if (kpi.HideTitle)
                                        {
                                            //kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='{1}'><td colspan='2' class='kpiresult' align='center' style='" + fontStyle + "'><span  class='dashboardkpi-txt'  >{0}</span></td></tr>", kpi.ExpressionFormat, href, kpi.LinkID, kpi.Title, kpi.FontColor);
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

                                        kpiString = kpiString.Replace("$exp$", kpi.ExpressionFormat);
                                        KPIsHtml += kpiString;

                                    }
                                }
                                //panelDashboardContent.Controls.Add(pContainer);
                                KPIsHtml += KPIsHtmlEnd;
                            }

                            string shape = string.Empty; //-moz-border-radius: 115px/102px;-webkit-border-radius: 115px 102px;
                            string background = string.Empty;
                            if (dashboardPanelProperty.Theme == "Rectangle" && !string.IsNullOrEmpty(dashboardPanelProperty.LinkType))
                            {
                                shape = string.Format("border: 1px solid {0};", dashboardPanelProperty.BorderColor);
                            }
                            else if (dashboardPanelProperty.Theme == "Elliptical")
                            {
                                shape = "border: 1px solid " + dashboardPanelProperty.BorderColor + ";border-radius: " + width.ToString() + "px/" + height.ToString() + "px;";
                            }

                            if (string.IsNullOrEmpty(dashboardPanelProperty.BackGroundUrl))
                            {
                                background = string.Empty;
                            }
                            else if (dashboardPanelProperty.BackGroundUrl.Contains(";#"))
                            {
                                string[] values = dashboardPanelProperty.BackGroundUrl.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length > 1)
                                {
                                    if (values[0] == "Url")
                                    {
                                        background = "background-image:url(" + values[1] + ");background-size: cover;background-repeat: no-repeat;";
                                    }
                                    else
                                    {
                                        background = "background-color:" + values[1] + ";";
                                    }
                                }
                            }
                            else
                            {
                                background = "background-image:url(" + dashboardPanelProperty.BackGroundUrl + ");background-size: cover;background-repeat: no-repeat;";
                            }


                            string imageHtml = "";
                            if (string.IsNullOrEmpty(dashboardPanelProperty.IconUrl) && !string.IsNullOrEmpty(dashboard.Icon))
                            {
                                dashboardPanelProperty.IconUrl = dashboard.Icon;
                            }

                            if (!string.IsNullOrEmpty(dashboardPanelProperty.IconUrl))
                            {
                                int actualWidth = dashboardPanelProperty.iconWidth;
                                if (dashboardPanelProperty.iconWidth > dashboardPanelProperty.Width)
                                {
                                    actualWidth = dashboardPanelProperty.Width;
                                }

                                int actualHeight = dashboardPanelProperty.iconHeight;
                                if (dashboardPanelProperty.iconHeight > dashboardPanelProperty.Height)
                                {
                                    actualHeight = dashboardPanelProperty.Height;
                                }

                                int iconTop = dashboardPanelProperty.iconTop > dashboardPanelProperty.Height ? dashboardPanelProperty.Height : dashboardPanelProperty.iconTop;
                                //iconTop = iconTop + actualHeight > dashboardPanelProperty.Height ? iconTop + actualHeight - dashboardPanelProperty.Height : iconTop;
                                int iconLeft = dashboardPanelProperty.iconLeft > dashboardPanelProperty.Width ? dashboardPanelProperty.Width : dashboardPanelProperty.iconLeft;
                                //iconLeft = iconLeft + actualWidth > dashboardPanelProperty.Width ? iconLeft + actualWidth - dashboardPanelProperty.Width : iconLeft;
                                string iconShape = string.Empty;
                                if (!string.IsNullOrEmpty(dashboardPanelProperty.IconShape) && dashboardPanelProperty.IconShape == "Elliptical")
                                {
                                    iconShape = string.Format("border-radius:{0}px/{1}px;", dashboardPanelProperty.iconWidth, dashboardPanelProperty.iconHeight);
                                }
                                imageHtml = "<img style='padding-right: 2px;padding-top: 2px;max-width: 60%;max-height: 60%;width:" + dashboardPanelProperty.iconWidth + "px ;height:" + dashboardPanelProperty.iconHeight + "px;top:" + iconTop + "px;left:" + iconLeft + "px;" + iconShape + "' src='" + dashboardPanelProperty.IconUrl + "'/>";

                                //imageHtml = "<img style='padding-right: 2px;padding-top: 2px;position: absolute;max-width: 60%;max-height: 60%;width:" + dashboardPanelProperty.iconWidth + "px ;height:" + dashboardPanelProperty.iconHeight + "px;top:" + iconTop + "px;left:" + iconLeft + "px;" + iconShape + "' src='" + dashboardPanelProperty.IconUrl + "'/>";
                            }

                            tdMain.Controls.Add(new LiteralControl("<div id='" + dashboard.ID + "' panelInstanceID='" + dashboard.ID + "' class='panel-content-header  dashboardpanelcontainer'   style='width: " + width.ToString() + "px;height: " + height + "px;text-align: center;" + shape + background + "' class='dragresize'><img style='float: right;padding-right: 2px;padding-top: 2px;display:none' onload='loadDashboard(" + json + ");' onclick='CloseDashboard(this);' src='/content/images/cross_icn.png'/><div ID='panelDashboardContent' style='padding-top: 10px; padding-bottom: 10px;' class='d-content'>" + imageHtml + KPIsHtml + "</div></div>"));
                            #endregion
                        }
                        else if (dashboard != null && dashboard.DashboardType == DashboardType.Analytic)
                        {

                            dashboardPanelProperty.NavigationType = 1;
                            analyticUrl = string.Format("{0}/runs/QuickSurveyResult?modelID={1}&resultType=True&dashboardId={2}&relativeRunID=0&IsDlg=true&popupId=dashboardRun&width=1092.8&height=600.3", UGITUtility.GetAbsoluteURL(configManager.GetValue(ConfigConstants.AnalyticUrl)), dashboard.SecondryID, dashboard.ID);
                            string html = string.Empty;
                            html += "<div id='analyticID' onclick = OpenLink('" + Uri.EscapeDataString(UGITUtility.GetAbsoluteURL(analyticUrl)) + "'," + dashboardPanelProperty.NavigationType + "); class='panel-content-header  dashboardpanelcontainer'  style='width: " + width.ToString() + "px;height: " + height + "px;text-align: center;" + (!string.IsNullOrEmpty(dashboardPanelProperty.BackGroundUrl) ? "background-image:url(" + dashboardPanelProperty.BackGroundUrl + ");background-size: 100%;" : string.Empty) + "' class='dragresize'><b><p>" + title;
                            html += "<img id='maximizeicon' class='dashboardaction-icon' onclick=OpenLink('" + Uri.EscapeDataString(UGITUtility.GetAbsoluteURL(analyticUrl)) + "',1); src='/content/images/maximize-icon12x12.png' style='border-width:0px;padding-right:22px'/>";
                            html += "</p></b><div ID='panelDashboardContent' class='d-content'><iframe id='frm1' isrc='" + analyticUrl + "&showOnlyChart=true' Width='" + (width - widthOffset) + "px' Height='" + (height - heightOffset) + "px'></iframe>";
                            html += "</div></div>";
                            tdMain.Controls.Add(new LiteralControl(html));
                        }
                        else if (dashboardPanelProperty.DashboardType == DashboardType.Control.ToString())
                        {
                            HtmlGenericControl dvControl = new HtmlGenericControl("div");
                            HtmlGenericControl panelDashboardContent = new HtmlGenericControl("div");

                            dvControl.Attributes.Add("id", "controlID");
                            dvControl.Attributes.Add("class", "dashboardpanelcontainer");
                            dvControl.Style.Add("text-align", "center");

                            panelDashboardContent.Attributes.Add("id", "panelDashboardContent");
                            panelDashboardContent.Attributes.Add("class", "d-content");
                            //dvControl.Style.Add("height", "fit-content");
                            //dvControl.Style.Add("width", "fit-content");

                            if (dashboardPanelProperty.Theme != "Rounded Rectangle")
                            {
                                //height -= heightOffset;
                            }

                            if (dashboardPanelProperty.DashboardSubType != "MessageBoard")
                            {
                                if (!dashboardPanelProperty.IsHideTitle)
                                {
                                    HtmlGenericControl dvTitle = new HtmlGenericControl("div");
                                    dvTitle.Attributes.Add("id", "dvTitle");
                                    dvTitle.Attributes.Add("class", "dvTitle");
                                    dvTitle.Style.Add("text-align", "center");
                                    dvTitle.Style.Add("font-size", "13px");
                                    dvTitle.InnerHtml = string.Format("<b>{0}", dashboardPanelProperty.DisplayName);
                                    tdMain.Controls.Add(dvTitle);
                                }
                                dashboardPanelProperty.PageSize = dashboardPanelProperty.PageSize == 0 ? 100 : dashboardPanelProperty.PageSize;
                            }

                            switch (dashboardPanelProperty.DashboardSubType)
                            {
                                case "MessageBoard":
                                    uGovernITMessageBoardUserControl messageBoard = (uGovernITMessageBoardUserControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/uGovernITMessageBoard/uGovernITMessageBoardUserControl.ascx");
                                    messageBoard.Width = string.Format("{0}px", width.ToString());
                                    messageBoard.Height = string.Format("{0}px", height.ToString());
                                    messageBoard.DisplayOnDashboard = true;
                                    messageBoard.BorderStyle = dashboardPanelProperty.Theme;
                                    panelDashboardContent.Controls.Add(messageBoard);
                                    break;
                                //case "MyTasks":

                                //    MyTasks myTasks = (MyTasks)Page.LoadControl("~/_controltemplates/15/uGovernIT/MyTasks.ascx");
                                //    myTasks.IsPreview = true;
                                //    myTasks.PageSize = dashboardPanelProperty.PageSize;
                                //    myTasks.ByDueDate = true;
                                //    myTasks.ByProject = true;
                                //    myTasks.Width = string.Format("{0}px", width.ToString());
                                //    myTasks.Height = string.Format("{0}px", height.ToString());
                                //    myTasks.DisplayOnDashboard = true;
                                //    tdMain.Controls.Add(myTasks);
                                //    panelDashboardContent.Controls.Add(table);
                                //    break;

                                case "WaitingOnMe":

                                    CustomFilteredTickets myWaitingOnlistView = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/uGovernIT/CustomFilteredTickets.ascx");
                                    myWaitingOnlistView.ModuleName = string.Empty;
                                    myWaitingOnlistView.UserType = "my";
                                    myWaitingOnlistView.MTicketStatus = TicketStatus.WaitingOnMe;
                                    myWaitingOnlistView.PageSize = dashboardPanelProperty.PageSize;
                                    myWaitingOnlistView.IsPreview = true;
                                    myWaitingOnlistView.HideModuleDetail = true;
                                    myWaitingOnlistView.MyHomeTab = Constants.MyHomeTicketTab;
                                    myWaitingOnlistView.HideGlobalSearch = true;
                                    myWaitingOnlistView.IsHomePage = true;
                                    myWaitingOnlistView.Width = string.Format("{0}px", width.ToString());
                                    myWaitingOnlistView.Height = string.Format("{0}px", height.ToString());
                                    myWaitingOnlistView.DisplayOnDashboard = true;
                                    tdMain.Controls.Add(myWaitingOnlistView);
                                    panelDashboardContent.Controls.Add(table);

                                    break;

                                case "MyRequests":

                                    //my ticket preview
                                    CustomFilteredTickets fTickets = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/uGovernIT/CustomFilteredTickets.ascx");
                                    fTickets.ModuleName = string.Empty;
                                    fTickets.UserType = "my";
                                    fTickets.MTicketStatus = TicketStatus.Open;
                                    fTickets.PageSize = dashboardPanelProperty.PageSize;
                                    fTickets.IsPreview = true;
                                    fTickets.HideModuleDetail = true;
                                    fTickets.HideGlobalSearch = true;
                                    fTickets.MyHomeTab = Constants.MyHomeTicketTab;
                                    fTickets.IsHomePage = true;

                                    fTickets.Width = string.Format("{0}px", width.ToString());
                                    fTickets.Height = string.Format("{0}px", height.ToString());
                                    fTickets.DisplayOnDashboard = true;
                                    tdMain.Controls.Add(fTickets);
                                    panelDashboardContent.Controls.Add(table);

                                    break;

                                case "MyProjects":

                                    CustomFilteredTickets myProjectlistView = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/Shared/CustomFilteredTickets.ascx");
                                    myProjectlistView.ModuleName = "PMM";
                                    myProjectlistView.UserType = "my";
                                    myProjectlistView.MTicketStatus = TicketStatus.Open;
                                    myProjectlistView.PageSize = dashboardPanelProperty.PageSize;
                                    myProjectlistView.IsPreview = true;
                                    myProjectlistView.HideModuleDetail = true;
                                    myProjectlistView.MyHomeTab = Constants.MyProjectTab;
                                    myProjectlistView.IsHomePage = true;

                                    myProjectlistView.Width = string.Format("{0}px", width.ToString());
                                    myProjectlistView.Height = string.Format("{0}px", height.ToString());
                                    myProjectlistView.DisplayOnDashboard = true;

                                    //myProjectlistView.FilteredTable = myProjectData;
                                    //myProjectPreviewItems.Controls.Add(myProjectlistView);
                                    tdMain.Controls.Add(myProjectlistView);
                                    panelDashboardContent.Controls.Add(table);

                                    break;

                                case "CustomFilter":

                                    CustomFilteredTickets customFilter = (CustomFilteredTickets)Page.LoadControl("~/controltemplates/uGovernIT/CustomFilteredTickets.ascx");
                                    if (string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                    {
                                        customFilter.ModuleName = "PRS";
                                    }
                                    else
                                    {
                                        customFilter.ModuleName = dashboardPanelProperty.Module;
                                    }

                                    //customFilter.UserType = "my";
                                    customFilter.HideMyFiltersLinks = false;
                                    customFilter.MTicketStatus = TicketStatus.Open;
                                    customFilter.PageSize = dashboardPanelProperty.PageSize;
                                    customFilter.IsPreview = false;
                                    customFilter.HideModuleDetail = true;
                                    customFilter.HideGlobalSearch = true;
                                    customFilter.HideModuleDesciption = true;
                                    customFilter.HideModuleImage = true;
                                    customFilter.IsHomePage = false;
                                    customFilter.HideNewTicketButton = true;
                                    customFilter.Width = string.Format("{0}px", width.ToString());
                                    customFilter.Height = string.Format("{0}px", height.ToString());
                                    customFilter.DisplayOnDashboard = true;
                                    tdMain.Controls.Add(customFilter);
                                    panelDashboardContent.Controls.Add(table);

                                    break;

                                //case "UserHotList":

                                //    UserHotList userHotList = (UserHotList)Page.LoadControl("~/_controltemplates/15/uGovernIT/UserHotList.ascx");
                                //    userHotList.PageSize = dashboardPanelProperty.PageSize;
                                //    userHotList.DueDate = dashboardPanelProperty.DueDate;
                                //    userHotList.Priority = dashboardPanelProperty.Priority;
                                //    userHotList.IsCritial = dashboardPanelProperty.IsCritical;
                                //    userHotList.UserId = dashboardPanelProperty.UserID;
                                //    userHotList.Width = string.Format("{0}px", width.ToString());
                                //    userHotList.Height = string.Format("{0}px", height.ToString());
                                //    userHotList.DisplayOnDashboard = true;
                                //    tdMain.Controls.Add(userHotList);
                                //    panelDashboardContent.Controls.Add(table);
                                //    break;

                                case "WorkflowBottleneck":

                                    WorkflowBottleneck workflowBottleneck = (WorkflowBottleneck)Page.LoadControl("~/controltemplates/uGovernIT/WorkflowBottleneck.ascx");

                                    if (string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                        workflowBottleneck.ModuleName = "TSR";
                                    else
                                        workflowBottleneck.ModuleName = dashboardPanelProperty.Module;

                                    workflowBottleneck.Width = string.Format("{0}px", width.ToString());
                                    workflowBottleneck.Height = string.Format("{0}px", height.ToString());
                                    workflowBottleneck.DisplayOnDashboard = true;
                                    tdMain.Controls.Add(workflowBottleneck);
                                    panelDashboardContent.Controls.Add(table);
                                    break;

                                case "SLAPerformanceTimeLine":
                                    {
                                        SLAPerformanceDashboard slaPerformance = (SLAPerformanceDashboard)Page.LoadControl("~/controltemplates/uGovernIT/SLAPerformanceDashboard.ascx");
                                        //slaPerformance.Width = string.Format("{0}px", width.ToString());
                                        //slaPerformance.Height = string.Format("{0}px", height.ToString());
                                        slaPerformance.Module = string.Empty;
                                        if (!string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                            slaPerformance.Module = Convert.ToString(dashboardPanelProperty.Module);
                                        if (!dashboardPanelProperty.IsHideTitle)
                                            slaPerformance.Title = dashboardPanelProperty.DisplayName;

                                        slaPerformance.ShowMode = dashboardPanelProperty.ShowMode;
                                        slaPerformance.DisplayUnit = dashboardPanelProperty.DisplayUnit;
                                        slaPerformance.ShowTotal = dashboardPanelProperty.ShowTotal;
                                        slaPerformance.CreatedOn = dashboardPanelProperty.CreatedOn;
                                        slaPerformance.CompletedOn = dashboardPanelProperty.CompletedOn;
                                        slaPerformance.IncludeOpenTickets = dashboardPanelProperty.IncludeOpenTickets;
                                        tdMain.Controls.Add(slaPerformance);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;

                                case "SLAPerformanceTabular":
                                    {
                                        SLAPerformanceTabularDashboard slaPerformanceTabular = (SLAPerformanceTabularDashboard)Page.LoadControl("~/controltemplates/uGovernIT/SLAPerformanceTabularDashboard.ascx");
                                        slaPerformanceTabular.Width = string.Format("{0}px", width.ToString());
                                        slaPerformanceTabular.Height = string.Format("{0}px", height.ToString());
                                        slaPerformanceTabular.Module = string.Empty;
                                        if (!string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                            slaPerformanceTabular.Module = Convert.ToString(dashboardPanelProperty.Module);
                                        if (!dashboardPanelProperty.IsHideTitle)
                                            slaPerformanceTabular.Title = dashboardPanelProperty.DisplayName;
                                        tdMain.Controls.Add(slaPerformanceTabular);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;

                                //case "OperationalDashboard":
                                //    {
                                //        OperationalDashboard _operationalDashboard = (OperationalDashboard)Page.LoadControl("~/_controltemplates/15/uGovernIT/OperationalDashboard.ascx");
                                //        _operationalDashboard.Width = string.Format("{0}px", width.ToString());
                                //        _operationalDashboard.Height = string.Format("{0}px", height.ToString());
                                //        tdMain.Controls.Add(_operationalDashboard);
                                //        panelDashboardContent.Controls.Add(table);
                                //    }
                                //    break;

                                case "ScoreCard":

                                    GroupScorecard scorecard = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    scorecard.ReportType = TypeOfReport.ScoreCard;
                                    scorecard.Width = Unit.Pixel(width);
                                    scorecard.Height = Unit.Pixel(height);
                                    scorecard.IsFilterActive = dashboardPanelProperty.EnableFilterScoreCard;
                                    if (dashboardPanelProperty.ScoreCardStartDate != DateTime.MinValue)
                                        scorecard.StartDate = dashboardPanelProperty.ScoreCardStartDate;
                                    if (dashboardPanelProperty.ScoreCardEndDate != DateTime.MinValue)
                                        scorecard.EndDate = dashboardPanelProperty.ScoreCardEndDate;

                                    tdMain.Controls.Add(scorecard);
                                    panelDashboardContent.Controls.Add(table);
                                    break;


                                case "TicketFlow":

                                    GroupScorecard ticketFlow = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    ticketFlow.ReportType = TypeOfReport.TicketFlow;
                                    ticketFlow.IsFilterActive = dashboardPanelProperty.EnableFilterTicketFlow;
                                    ticketFlow.StartDate = dashboardPanelProperty.TicketFlowStartDate;
                                    ticketFlow.EndDate = dashboardPanelProperty.TicketFlowEndDate;
                                    ticketFlow.Width = Unit.Pixel(width);
                                    ticketFlow.Height = Unit.Pixel(height);
                                    tdMain.Controls.Add(ticketFlow);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "WeeklyRollingAverage":

                                    GroupScorecard weeklyRollingAverage = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    weeklyRollingAverage.ReportType = TypeOfReport.WeeklyRollingAverage;
                                    weeklyRollingAverage.Width = Unit.Pixel(width);
                                    weeklyRollingAverage.Height = Unit.Pixel(height);
                                    weeklyRollingAverage.IsFilterActive = dashboardPanelProperty.EnableFilter;
                                    weeklyRollingAverage.WeeklyAverage = string.Empty;
                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.WeeklyAverage))
                                        weeklyRollingAverage.WeeklyAverage = Convert.ToString(dashboardPanelProperty.WeeklyAverage);
                                    tdMain.Controls.Add(weeklyRollingAverage);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "PredictBacklog":

                                    GroupScorecard predictBacklog = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    predictBacklog.ReportType = TypeOfReport.PredictBacklog;
                                    predictBacklog.Width = Unit.Pixel(width);
                                    predictBacklog.Height = Unit.Pixel(height);
                                    predictBacklog.IsFilterActive = dashboardPanelProperty.EnableFilterPredictBacklog;
                                    predictBacklog.WeeklyAverage = string.Empty;
                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.WeeklyAverage))
                                        predictBacklog.WeeklyAverage = Convert.ToString(dashboardPanelProperty.WeeklyAverage);

                                    tdMain.Controls.Add(predictBacklog);
                                    panelDashboardContent.Controls.Add(table);
                                    break;

                                case "OldestUnsolved":

                                    GroupScorecard oldestUnsolved = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    oldestUnsolved.ReportType = TypeOfReport.OldestUnsolved;
                                    oldestUnsolved.Width = Unit.Pixel(width);
                                    oldestUnsolved.Height = Unit.Pixel(height);

                                    tdMain.Controls.Add(oldestUnsolved);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "TSRTrendChart":

                                    GroupScorecard tSRTrendChart = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    tSRTrendChart.ReportType = TypeOfReport.TSRTrendChart;
                                    tSRTrendChart.Width = Unit.Pixel(width);
                                    tSRTrendChart.Height = Unit.Pixel(height);
                                    tdMain.Controls.Add(tSRTrendChart);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "TicketCreatedByWeek":

                                    GroupScorecard ticketsCreatedByWeek = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    ticketsCreatedByWeek.ReportType = TypeOfReport.TicketCreatedByWeek;
                                    ticketsCreatedByWeek.IsFilterActive = dashboardPanelProperty.EnableFilterTicketCreatedByWeek;
                                    ticketsCreatedByWeek.Width = Unit.Pixel(width);
                                    ticketsCreatedByWeek.Height = Unit.Pixel(height);
                                    ticketsCreatedByWeek.WeeklyAverage = string.Empty;
                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.WeeklyAverage))
                                        ticketsCreatedByWeek.WeeklyAverage = Convert.ToString(dashboardPanelProperty.WeeklyAverage);
                                    tdMain.Controls.Add(ticketsCreatedByWeek);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "GroupUnsolvedTickets":

                                    GroupScorecard groupUnsolvedTickets = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    groupUnsolvedTickets.ReportType = TypeOfReport.GroupUnsolvedTickets;
                                    groupUnsolvedTickets.Width = Unit.Pixel(width);
                                    groupUnsolvedTickets.Height = Unit.Pixel(height);
                                    tdMain.Controls.Add(groupUnsolvedTickets);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "TicketByCategoryReport":

                                    GroupScorecard problemReport = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                    problemReport.ReportType = TypeOfReport.ProblemReport;

                                    problemReport.Module = string.Empty;
                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                        problemReport.Module = Convert.ToString(dashboardPanelProperty.Module);
                                    problemReport.Status = string.Empty;

                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.Status))
                                        problemReport.Status = Convert.ToString(dashboardPanelProperty.Status);
                                    if (!string.IsNullOrEmpty(dashboardPanelProperty.Category))
                                    {
                                        problemReport.Category = Convert.ToString(dashboardPanelProperty.Category);
                                    }
                                    problemReport.Width = Unit.Pixel(width);
                                    problemReport.Height = Unit.Pixel(height);
                                    tdMain.Controls.Add(problemReport);
                                    panelDashboardContent.Controls.Add(table);
                                    break;
                                case "AgentPerformance":
                                    {
                                        GroupScorecard agentPerformance = (GroupScorecard)Page.LoadControl("~/controltemplates/uGovernIT/GroupScorecard.ascx");
                                        agentPerformance.ReportType = TypeOfReport.AgentPerformance;
                                        agentPerformance.Width = Unit.Pixel(width);
                                        agentPerformance.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(agentPerformance);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "LeftTicketCountBar":
                                    {
                                        LeftTicketCountBar leftCountBar = Page.LoadControl("~/ControlTemplates/LeftTicketCountBar.ascx") as LeftTicketCountBar;
                                        leftCountBar.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        leftCountBar.ShowDetails = UGITUtility.ObjectToString(dashboardPanelProperty.ShowKPIDetail);
                                        leftCountBar.Width = Unit.Pixel(width);
                                        leftCountBar.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(leftCountBar);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UserProjectPanel":
                                    {
                                        UserProjectPanel topUserProjectBar = Page.LoadControl("~/ControlTemplates/UserProjectPanel.ascx") as UserProjectPanel;
                                        topUserProjectBar.Width = Unit.Pixel(width);
                                        topUserProjectBar.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(topUserProjectBar);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "MyProjectPanel":
                                    {
                                        MyProjectPanel topUserProjectBar = Page.LoadControl("~/ControlTemplates/MyProjectPanel.ascx") as MyProjectPanel;
                                        topUserProjectBar.Width = Unit.Pixel(width);
                                        topUserProjectBar.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(topUserProjectBar);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "LeftLinkBar":
                                    {
                                        LeftLinkBar leftLinkBar = Page.LoadControl("~/ControlTemplates/LeftLinkBar.ascx") as LeftLinkBar;
                                        leftLinkBar.Width = Unit.Pixel(width);
                                        leftLinkBar.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(leftLinkBar);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ProjectPendingAllocation":
                                    {
                                        ProjectPendingAllocation pendingAllocation = Page.LoadControl("~/ControlTemplates/ProjectPendingAllocation.ascx") as ProjectPendingAllocation;
                                        pendingAllocation.Width = Unit.Pixel(width);
                                        pendingAllocation.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(pendingAllocation);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UserWelcomePanel":
                                    {
                                        UserWelcomePanel topUserWelcomeBar = Page.LoadControl("~/ControlTemplates/UserWelcomePanel.ascx") as UserWelcomePanel;
                                        topUserWelcomeBar.Width = Unit.Pixel(width);
                                        topUserWelcomeBar.Height = Unit.Pixel(height);
                                        topUserWelcomeBar.WelcomeMessage = dashboardPanelProperty.WelcomeMessage;
                                        topUserWelcomeBar.viewId = UGITUtility.ObjectToString(DashboardViewID);
                                        tdMain.Controls.Add(topUserWelcomeBar);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UserDetailsPanel":
                                    {
                                        UserDetailsPanel userDetailsPanel = Page.LoadControl("~/ControlTemplates/UserDetailsPanel.ascx") as UserDetailsPanel;
                                        userDetailsPanel.Width = Unit.Pixel(width);
                                        userDetailsPanel.Height = Unit.Pixel(height);
                                        userDetailsPanel.UserId = HttpContext.Current.CurrentUser().Id;
                                        tdMain.Controls.Add(userDetailsPanel);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UserChartPanel":
                                    {
                                        UserChartPanel userChartPanel = Page.LoadControl("~/ControlTemplates/UserChartPanel.ascx") as UserChartPanel;
                                        userChartPanel.Width = Unit.Pixel(width);
                                        userChartPanel.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(userChartPanel);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "HelpCartCtrl":
                                    {
                                        HelpCardCtrl helpCardCtrl = Page.LoadControl("~/ControlTemplates/HelpCard/HelpCardCtrl.ascx") as HelpCardCtrl;
                                        helpCardCtrl.Width = Unit.Pixel(width);
                                        helpCardCtrl.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(helpCardCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UserChartDetail":
                                    {
                                        UserChartDetailPanel userChartDetailPanel = Page.LoadControl("~/ControlTemplates/UserChartDetailPanel.ascx") as UserChartDetailPanel;
                                        userChartDetailPanel.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        lstDasboards = dManager.LoadDashboardPanelsByType(DashboardType.Chart, true, HttpContext.Current.GetManagerContext().TenantID);
                                        dashboard = lstDasboards.FirstOrDefault(x => x.Title == dashboardPanelProperty.DashboardName);
                                        // chart element title and custom control title should be same else control won't add 
                                        if (dashboard != null)
                                        {
                                            userChartDetailPanel.panelId = dashboard.ID.ToString();
                                            userChartDetailPanel.ShowDetails = UGITUtility.ObjectToString(dashboardPanelProperty.ShowKPIDetail);
                                            userChartDetailPanel.Width = Unit.Pixel(width);
                                            userChartDetailPanel.Height = Unit.Pixel(height);
                                            panelDashboardContent.Controls.Add(userChartDetailPanel);
                                        }

                                    }
                                    break;
                                case "HelpCardsPanel":
                                    {
                                        HelpCardsPanel helpCardsPanel = Page.LoadControl("~/ControlTemplates/HelpCardsPanel.ascx") as HelpCardsPanel;
                                        helpCardsPanel.Width = Unit.Pixel(width);
                                        helpCardsPanel.Height = Unit.Pixel(height);
                                        helpCardsPanel.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        tdMain.Controls.Add(helpCardsPanel);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ITSMImpact":
                                    {
                                        ITSMCtrl itsmCtrl = Page.LoadControl("~/ControlTemplates/uGovernIT/ITSMCtrl.ascx") as ITSMCtrl;
                                        itsmCtrl.Width = Unit.Pixel(width);
                                        itsmCtrl.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(itsmCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ServiceCatalog":
                                    {
                                        ServiceCatalog serviceCatalog = (ServiceCatalog)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceCatalog.ascx");
                                        serviceCatalog.ShowServiceIcons = true;
                                        serviceCatalog.ServiceCatalogViewMode = ServiceViewType.ButtonView;
                                        serviceCatalog.IconSize = 60;
                                        serviceCatalog.Width = Unit.Pixel(width);
                                        serviceCatalog.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(serviceCatalog);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "DailyTicketTrendsCount":
                                    {
                                        DailyTicketCountTrendsControl dailyTicketCountTrends = (DailyTicketCountTrendsControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Dashboard/DailyTicketCountTrendsControl.ascx");
                                        //dailyTicketCountTrends.Width = Unit.Pixel(width);
                                        //dailyTicketCountTrends.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(dailyTicketCountTrends);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "SLAMetric":
                                    {
                                        uGovernITDashboardSLAUserControl dailyTicketCountTrends = (uGovernITDashboardSLAUserControl)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Dashboard/uGovernITDashboardSLAUserControl.ascx");
                                        //dailyTicketCountTrends.Width = Unit.Pixel(width);
                                        //dailyTicketCountTrends.Height = Unit.Pixel(height);
                                        if (!string.IsNullOrEmpty(dashboardPanelProperty.Module))
                                            dailyTicketCountTrends.Module = Convert.ToString(dashboardPanelProperty.Module);
                                        dailyTicketCountTrends.SLAEnableModules = "Application Change Request (ACR);Bug Tracking (BTS);Outage Incidents (INC);Problem Resolution (PRS);Root Cause Analysis (RCA);Shared Services (SVC);Technical Service Request (TSR)";
                                        dailyTicketCountTrends.StringOfSelectedModule = "Application Change Request (ACR);Bug Tracking (BTS);Outage Incidents (INC);Problem Resolution (PRS);Root Cause Analysis (RCA);Shared Services (SVC);Technical Service Request (TSR)";
                                        tdMain.Controls.Add(dailyTicketCountTrends);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "AllocationTimeline":
                                    {
                                        //ResourceAllocationGrid _resourceAllocationGrid = Page.LoadControl("~/CONTROLTEMPLATES/RMM/ResourceAllocationGrid.ascx") as ResourceAllocationGrid;
                                        ResourceAllocationGridNew _resourceAllocationGrid = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ResourceAllocationGridNew.ascx") as ResourceAllocationGridNew;
                                        _resourceAllocationGrid.SelectedUser = UGITUtility.ObjectToString(HttpContext.Current.GetManagerContext().CurrentUser?.Id);
                                        _resourceAllocationGrid.HideTopBar = dashboardPanelProperty.HideResourceAllocationFilter;
                                        _resourceAllocationGrid.ShowCurrentUserDetailsOnly = dashboardPanelProperty.ShowCurrentUserDetailsOnly;
                                        _resourceAllocationGrid.HideAllocationType = dashboardPanelProperty.HideAllocationType;
                                        _resourceAllocationGrid.Width = Unit.Pixel(width);
                                        _resourceAllocationGrid.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(_resourceAllocationGrid);
                                        panelDashboardContent.Controls.Add(table);

                                    }
                                    break;
                                case "AllocationTimelineNew":
                                    {
                                        ResourceAllocationGridNew _resourceAllocationGrid = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ResourceAllocationGridNew.ascx") as ResourceAllocationGridNew;
                                        _resourceAllocationGrid.HideTopBar = dashboardPanelProperty.HideResourceAllocationFilter;
                                        _resourceAllocationGrid.ShowCurrentUserDetailsOnly = dashboardPanelProperty.ShowCurrentUserDetailsOnly;
                                        _resourceAllocationGrid.HideAllocationType = dashboardPanelProperty.HideAllocationType;
                                        _resourceAllocationGrid.SelectedUser = HttpContext.Current.CurrentUser().Id.ToString();
                                        _resourceAllocationGrid.Width = Unit.Pixel(width);
                                        _resourceAllocationGrid.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(_resourceAllocationGrid);
                                        panelDashboardContent.Controls.Add(table);

                                    }
                                    break;
                                case "ManagerProjectAllocationView":
                                    {
                                        ManagerProjectAllocationView _managerProjectAllocationView = Page.LoadControl("~/CONTROLTEMPLATES/RMONE/ManagerProjectAllocationView.ascx") as ManagerProjectAllocationView;
                                        _managerProjectAllocationView.Width = Unit.Pixel(width);
                                        _managerProjectAllocationView.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(_managerProjectAllocationView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "RoleWiseAllocation":
                                    {
                                        RoleAllocationsView _roleAllocationView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/RoleAllocationsView.ascx") as RoleAllocationsView;
                                        _roleAllocationView.Width = Unit.Pixel(width);
                                        _roleAllocationView.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(_roleAllocationView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "BillingForMonth":
                                    {
                                        BillingForMonth _billingCtrl = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/BillingForMonth.ascx") as BillingForMonth;
                                        _billingCtrl.Width = Unit.Pixel(width);
                                        _billingCtrl.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(_billingCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "BarGaugeChart":
                                    {
                                        BarGaugeChart gaugeChart = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/SalesBySectorChart.ascx") as BarGaugeChart;
                                        gaugeChart.Width = Unit.Pixel(width);
                                        gaugeChart.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(gaugeChart);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "SaleByDivision":
                                    {
                                        SalesByDivisionChart divisionChart = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/SalesByDivisionChart.ascx") as SalesByDivisionChart;
                                        divisionChart.Width = Unit.Pixel(width);
                                        divisionChart.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(divisionChart);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "CommittedSalesBySector":
                                    {
                                        CommittedSalesBySector CsectorChart = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/CommittedSalesBySector.ascx") as CommittedSalesBySector;
                                        CsectorChart.Width = Unit.Pixel(width);
                                        CsectorChart.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(CsectorChart);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "CommittedSalesByDivision":
                                    {
                                        CommittedSalesByDivision CDivisionChart = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/CommittedSalesByDivision.ascx") as CommittedSalesByDivision;
                                        CDivisionChart.Width = Unit.Pixel(width);
                                        CDivisionChart.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(CDivisionChart);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "CommonResourceChart":
                                    {
                                        FinancialView financialView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/FinancialView.ascx") as FinancialView;
                                        financialView.Width = Unit.Pixel(width);
                                        financialView.Height = Unit.Pixel(height);
                                        financialView.HeadType = "Financial";
                                        tdMain.Controls.Add(financialView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ResourceView":
                                    {
                                        ResourceView resourceViewCtrl = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/ResourceView.ascx") as ResourceView;
                                        resourceViewCtrl.Width = Unit.Pixel(width);
                                        resourceViewCtrl.Height = Unit.Pixel(height);
                                        resourceViewCtrl.HeadType = "Resource";
                                        tdMain.Controls.Add(resourceViewCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "BillingWorkMonth":
                                    {
                                        BillingWorkMonth billingworkmonth = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/BillingWorkMonth.ascx") as BillingWorkMonth;
                                        billingworkmonth.Width = Unit.Pixel(width);
                                        billingworkmonth.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(billingworkmonth);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ResourceNeededChart":
                                    {
                                        RecruitmentView recruitmentView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/RecruitmentView.ascx") as RecruitmentView;
                                        recruitmentView.Width = Unit.Pixel(width);
                                        recruitmentView.Height = Unit.Pixel(height);
                                        recruitmentView.HeadType = "Resource";
                                        tdMain.Controls.Add(recruitmentView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ProjectView":
                                    {
                                        ProjectView projectView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/ProjectView.ascx") as ProjectView;
                                        projectView.Width = Unit.Pixel(width);
                                        projectView.Height = Unit.Pixel(height);
                                        projectView.HeadType = "Project";
                                        tdMain.Controls.Add(projectView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "RecruitmentView":
                                    {
                                        RecruitmentView recruitmentView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/RecruitmentView.ascx") as RecruitmentView;
                                        recruitmentView.Width = Unit.Pixel(width);
                                        recruitmentView.Height = Unit.Pixel(height);
                                        recruitmentView.HeadType = "Recruitment";
                                        tdMain.Controls.Add(recruitmentView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "CardKpis":
                                    {
                                        NewCardKpis newCardKpis = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/NewCardKpis.ascx") as NewCardKpis;
                                        newCardKpis.Width = Unit.Pixel(width);
                                        newCardKpis.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(newCardKpis);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "AllocationConflicts":
                                    {
                                        AllocationConflicts allocationConflicts = Page.LoadControl("~/ControlTemplates/AllocationConflicts.ascx") as AllocationConflicts;
                                        allocationConflicts.Width = Unit.Pixel(width);
                                        allocationConflicts.Height = Unit.Pixel(height);
                                        allocationConflicts.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        allocationConflicts.ShowByUsersDivision = dashboardPanelProperty.ShowByUsersDivision;
                                        tdMain.Controls.Add(allocationConflicts);
                                        panelDashboardContent.Controls.Add(table);
                                    }                                    
                                    break;
                                case "UnfilledAllocations":
                                    {
                                        UnfilledAllocations unfilledAllocations = Page.LoadControl("~/ControlTemplates/UnfilledAllocations.ascx") as UnfilledAllocations;
                                        unfilledAllocations.Width = Unit.Pixel(width);
                                        unfilledAllocations.Height = Unit.Pixel(height);
                                        unfilledAllocations.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        unfilledAllocations.Caption = dashboardPanelProperty.DisplayName;
                                        //unfilledAllocations.UnfilledAllocationType = dashboardPanelProperty.UnfilledAllocationType;
                                        tdMain.Controls.Add(unfilledAllocations);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UnfilledProjectAllocations":
                                    {
                                        UnfilledProjectAllocations unfilledProjectAllocations = Page.LoadControl("~/ControlTemplates/UnfilledProjectAllocations.ascx") as UnfilledProjectAllocations;
                                        unfilledProjectAllocations.Width = Unit.Pixel(width);
                                        unfilledProjectAllocations.Height = Unit.Pixel(height);
                                        unfilledProjectAllocations.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        unfilledProjectAllocations.Caption = dashboardPanelProperty.DisplayName;
                                        //unfilledProjectAllocations.UnfilledAllocationType = dashboardPanelProperty.UnfilledAllocationType;
                                        unfilledProjectAllocations.ShowByUsersDivision = dashboardPanelProperty.ShowByUsersDivision;
                                        tdMain.Controls.Add(unfilledProjectAllocations);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "UnfilledPipelineAllocations":
                                    {
                                        UnfilledPipelineAllocations unfilledPipelineAllocations = Page.LoadControl("~/ControlTemplates/UnfilledPipelineAllocations.ascx") as UnfilledPipelineAllocations;
                                        unfilledPipelineAllocations.Width = Unit.Pixel(width);
                                        unfilledPipelineAllocations.Height = Unit.Pixel(height);
                                        unfilledPipelineAllocations.ViewID = UGITUtility.ObjectToString(DashboardViewID);
                                        unfilledPipelineAllocations.Caption = dashboardPanelProperty.DisplayName;
                                        //unfilledPipelineAllocations.UnfilledAllocationType = dashboardPanelProperty.UnfilledAllocationType;
                                        unfilledPipelineAllocations.ShowByUsersDivision = dashboardPanelProperty.ShowByUsersDivision;
                                        tdMain.Controls.Add(unfilledPipelineAllocations);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ExecutiveView":
                                    {
                                        ExecutiveView executiveView = Page.LoadControl("~/CONTROLTEMPLATES/CoreUI/ExecutiveView.ascx") as ExecutiveView;
                                        executiveView.Width = Unit.Pixel(width);
                                        executiveView.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(executiveView);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "ResourceAllocation":
                                    {
                                        CustomResourceAllocation allocations = Page.LoadControl("~/ControlTemplates/RMM/CustomResourceAllocation.ascx") as CustomResourceAllocation;
                                        allocations.Width = Unit.Pixel(width);
                                        allocations.Height = Unit.Pixel(height);
                                        //allocations.IncludeClosed = true;   //set this true if you want to show closed project by default.
                                        tdMain.Controls.Add(allocations);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "MyProjectCount":
                                    {
                                        MyProjectCount myprojectCtrl = Page.LoadControl("~/ControlTemplates/CoreUI/MyProjectCount.ascx") as MyProjectCount;
                                        myprojectCtrl.Width = Unit.Pixel(width);
                                        myprojectCtrl.Height = Unit.Pixel(height);
                                        tdMain.Controls.Add(myprojectCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "PMOHome":
                                    {
                                        PMOHome pmoHomeCtrl = Page.LoadControl("~/ControlTemplates/CoreUI/PMOHome.ascx") as PMOHome;
                                        pmoHomeCtrl.Width = Unit.Pixel(width);
                                        pmoHomeCtrl.Height = Unit.Pixel(height);
                                        pmoHomeCtrl.HeadType = "Project";
                                        tdMain.Controls.Add(pmoHomeCtrl);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "OuterPanel":
                                    {
                                        OuterPanel outerPanel = Page.LoadControl("~/ControlTemplates/OuterPanel.ascx") as OuterPanel;
                                        outerPanel.Width = Unit.Pixel(width);
                                        outerPanel.Height = Unit.Pixel(height);
                                        //pmoHomeCtrl.HeadType = "Project";
                                        tdMain.Controls.Add(outerPanel);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                case "BenchAnalytics":
                                    {
                                        BenchAnalytics _benchAnalytics = Page.LoadControl("~/ControlTemplates/Bench/BenchAnalytics.ascx") as BenchAnalytics;
                                        tdMain.Controls.Add(_benchAnalytics);
                                        panelDashboardContent.Controls.Add(table);
                                    }
                                    break;
                                default:
                                    break;
                            }

                            dvControl.Controls.Add(panelDashboardContent);
                            panelDashbaordPLink.Controls.Add(dvControl); //tbMain
                        }
                        else
                        {
                            string html = string.Empty;
                            string shape = string.Empty;
                            string background = string.Empty;
                            string fontHeaderStyle = GetFontStyle(dashboardPanelProperty.HeaderFontStyle);
                            if (dashboardPanelProperty.Theme == "Rectangle" && !string.IsNullOrEmpty(dashboardPanelProperty.LinkType))
                            {
                                shape = string.Format("border: 1px solid {0};", dashboardPanelProperty.BorderColor);
                            }
                            else if (dashboardPanelProperty.Theme == "Elliptical")
                            {
                                shape = "border: 1px solid " + dashboardPanelProperty.BorderColor + ";border-radius: " + width.ToString() + "px/" + height.ToString() + "px; ";
                            }
                            if (string.IsNullOrEmpty(dashboardPanelProperty.BackGroundUrl))
                            {
                                background = string.Empty;
                            }
                            else if (dashboardPanelProperty.BackGroundUrl.Contains(";#"))
                            {
                                string[] values = dashboardPanelProperty.BackGroundUrl.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length > 1)
                                {
                                    if (values[0] == "Url")
                                    {
                                        background = "background-image:url(" + values[1] + ");background-size: cover;background-repeat: no-repeat;";
                                    }
                                    else
                                    {
                                        background = "background-color:" + values[1] + ";";
                                    }
                                }
                            }
                            else
                            {
                                background = "background-image:url(" + dashboardPanelProperty.BackGroundUrl + ");background-size: cover;background-repeat: no-repeat;";
                            }

                            int titleTop = dashboardPanelProperty.TitleTop > dashboardPanelProperty.Height ? dashboardPanelProperty.Height : dashboardPanelProperty.TitleTop;
                            int titleLeft = dashboardPanelProperty.TitleLeft > dashboardPanelProperty.Width ? dashboardPanelProperty.Width : dashboardPanelProperty.TitleLeft;

                            string clickEvent = string.Empty;
                            string cursor = string.Empty;

                            if (dashboardPanelProperty.LinkType == "None")
                            {
                                cursor = "cursor:default;";
                            }
                            else
                            {
                                clickEvent = "onclick = OpenLink('" + Uri.EscapeDataString(UGITUtility.GetAbsoluteURL(dashboardPanelProperty.LinkUrl)) + "'," + dashboardPanelProperty.NavigationType + ");";

                            }

                            html += "<div id='linkID' " + clickEvent + " class='panel-content-header  dashboardpanelcontainer'  style='" + cursor + "width: " + width.ToString() + "px;height: " + height + "px;text-align: center;" + shape + background + "' class='dragresize'><b><p style='" + fontHeaderStyle + " position: absolute;top: " + titleTop + "px;left: " + titleLeft + "px;text-align:left;'>" + title + "</p></b><div ID='panelDashboardContent' class='d-content'>";
                            if (!string.IsNullOrEmpty(dashboardPanelProperty.IconUrl))
                            {
                                int actualWidth = dashboardPanelProperty.iconWidth;
                                if (dashboardPanelProperty.iconWidth > dashboardPanelProperty.Width)
                                {
                                    actualWidth = dashboardPanelProperty.Width;
                                }

                                int actualHeight = dashboardPanelProperty.iconHeight;
                                if (dashboardPanelProperty.iconHeight > dashboardPanelProperty.Height)
                                {
                                    actualHeight = dashboardPanelProperty.Height;
                                }

                                int iconTop = dashboardPanelProperty.iconTop > dashboardPanelProperty.Height ? dashboardPanelProperty.Height : dashboardPanelProperty.iconTop;

                                int iconLeft = dashboardPanelProperty.iconLeft > dashboardPanelProperty.Width ? dashboardPanelProperty.Width : dashboardPanelProperty.iconLeft;

                                string iconShape = string.Empty;
                                if (!string.IsNullOrEmpty(dashboardPanelProperty.IconShape) && dashboardPanelProperty.IconShape == "Elliptical")
                                {
                                    iconShape = string.Format("border-radius:{0}px/{1}px;", actualWidth, actualHeight);
                                }
                                //max-width: 60%;max-height: 60%;
                                html += "<img style='padding-right: 2px;padding-top: 2px;position: absolute;width:" + actualWidth + "px ;height:" + actualHeight + "px;top:" + iconTop + "px;left:" + iconLeft + "px;" + iconShape + "' src='" + dashboardPanelProperty.IconUrl + "'/>";
                            }
                            html += "</div></div>";
                            tdMain.Controls.Add(new LiteralControl(html));
                        }

                        if (dashboardPanelProperty.DashboardType != DashboardType.Control.ToString())
                        {
                            panelDashbaordPLink.Controls.Add(table);
                        }
                        QContainer.Controls.Add(panelDashbaordPLink);
                        dvDashboards.Controls.Add(QContainer);

                    }
                }

                LoadDashboardFilter(dashboardView.ID, cmDashboardView.GlobalFilers);

            }

            base.OnInit(e);
        }

        private void LoadDashboardFilter(long viewID, List<DashboardFilterProperty> sfilters)
        {
            if (sfilters == null || sfilters.Count <= 0)
                return;
            List<DashboardFilterProperty> filters = sfilters.Where(x => !x.Hidden).ToList();
            #region Create all global filter

            HtmlTable filterTable = new HtmlTable();
            filterTable.Width = "100%";
            filterTable.CellPadding = 0;
            filterTable.CellSpacing = 0;
            HtmlTableRow filterRow = new HtmlTableRow();
            HtmlTableCell filterCell = new HtmlTableCell();
            filterTable.Rows.Add(filterRow);
            filterRow.Cells.Add(filterCell);

            dashboardPanelsContainer.Width = "80%";
            globalFilterContainer.Width = "20%";
            globalFilterContainer.VAlign = "top";

            #region Globalfilter header which show clear filter button start
            Panel globalFilterHead = new Panel();
            Label filterLabel = new Label();
            filterLabel.EnableViewState = true;
            filterLabel.Text = "Filters";
            filterLabel.CssClass = "filterheader";
            LinkButton bt = new LinkButton();
            bt.Text = "Clear&nbsp;Filters";
            bt.CssClass = "filterlink removeallglobalfilterbutton ugitsellinkbg ugitsellinkborder faddedlink";
            bt.ID = "btClearAllFilter";
            bt.Attributes.Add("href", "javascript:");
            bt.Enabled = false;
            globalFilterHead.Controls.Add(filterLabel);
            globalFilterHead.Controls.Add(bt);
            globalFilterContainer.Controls.Add(globalFilterHead);
            #endregion

            #region Create global filter
            Panel dynamicFilter = new Panel();
            dynamicFilter.ID = "dynamicFilter";


            // Load to show all global filter
            foreach (DashboardFilterProperty globalFilter in filters)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(globalFilter.ColumnName)))
                {
                    //Get global filter data based on which user can apply fitler
                    string filterDataType = "String";
                    DataTable filterValues = DevxChartHelper.GetDatatableForGlobalFilter(HttpContext.Current.GetManagerContext(), globalFilter, ref filterDataType);
                    if (filterValues.Rows.Count > 0)
                    {
                        ListBox listBox = new ListBox();
                        listBox.AutoPostBack = false;
                        listBox.Attributes.Add("globalfilterid", Convert.ToString(globalFilter.ID));
                        listBox.Attributes.Add("dataType", filterDataType);
                        listBox.CssClass = "globalfilterlistbox";
                        listBox.SelectionMode = ListSelectionMode.Multiple;
                        listBox.Attributes.Add("viewID", viewID.ToString());
                        foreach (DataRow row in filterValues.Rows)
                        {
                            listBox.Items.Add(new ListItem(row["Value"].ToString(), row["Value"].ToString()));
                        }
                        if (globalFilter.DefaultValues != null)
                        {
                            ListItem item = null;
                            foreach (string val in globalFilter.DefaultValues)
                            {
                                item = listBox.Items.FindByValue(val);
                                if (item != null)
                                    item.Selected = true;
                            }
                        }
                        Panel globalFilterpanel = new Panel();
                        globalFilterpanel.Controls.Add(listBox);
                        if (filterDataType == "DateTime")
                        {
                            globalFilterpanel.Controls.Add(new LiteralControl("<span style='float:left;padding-left:2px;' class='daterangep'><input type='text' class='to watermark' watermark='MMYYYY' value='MMYYYY' style='width:75px;height:15px;font-size:11px'/>&nbsp;To&nbsp;<input type='text' style='width:75px;height:15px;font-size:11px' class='from watermark'  watermark='MMYYYY' value='MMYYYY'/><input class='globalfilterdaterangeButton' type='button' value='Go'/></span>"));
                        }
                        CustomPanelBox filterPanelBlock = (CustomPanelBox)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomPanelBox.ascx");
                        filterPanelBlock.ContentControl = globalFilterpanel;
                        filterPanelBlock.BoxTitle = globalFilter.Title;
                        dynamicFilter.Controls.Add(filterPanelBlock);
                    }
                }
            }


            globalFilterContainer.Controls.Add(dynamicFilter);

            #endregion


            #endregion

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetMetInfoForIE();
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/ShowDashboardDetails.aspx");

            //Get FilterTicket page url from config
            ConfigurationVariable callLogVariable = configManager.LoadVaribale("FilterTicketsPageUrl");
            if (callLogVariable != null)
            {
                filterTicketsPageUrl = UGITUtility.GetAbsoluteURL(callLogVariable.KeyValue.Trim());
            }
        }

        private void SetMetInfoForIE()
        {
            foreach (Control ctl in Page.Header.Controls)
            {
                if (ctl is System.Web.UI.HtmlControls.HtmlMeta)
                {
                    System.Web.UI.HtmlControls.HtmlMeta meta = (System.Web.UI.HtmlControls.HtmlMeta)ctl;
                    if (meta != null && meta.HttpEquiv == "X-UA-Compatible")
                    {
                        meta.Content = "IE=9";
                        break;
                    }
                }
            }

        }

        private void SetCookie(string Name, string Value)
        {
            UGITUtility.CreateCookie(Response, Name, Value);
        }
    }
}
