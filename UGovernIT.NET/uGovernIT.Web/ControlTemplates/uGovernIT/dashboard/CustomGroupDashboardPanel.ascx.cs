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
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Web
{
    public partial class CustomGroupDashboardPanel : UserControl
    {
        public List<Dashboard> DashboardList;

        protected List<Dashboard> needToViewDashboards;
        public string GlobalFilter { get; set; }
        public string LocalFilter { get; set; }
        public int DrillDownIndex { get; set; }
        public string ExpressionFilter { get; set; }
        public Unit PanelWidth { get; set; }
        public Unit PanelHeight { get; set; }
        public bool Sidebar { get; set; }
        public Control ContentControl { get; set; }
        public bool ShowControl { get; set; }
        public int ViewID { get; set; }
        private string filteredDataDetailPage = string.Empty;
        private string dasbhoardViewPage = string.Empty;
        ConfigurationVariableManager configManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        public CustomGroupDashboardPanel()
        {
            DashboardList = new List<Dashboard>();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
           // uGITDashboardScript.Name = string.Format("{0}?rev={1}", uGITDashboardScript.Name, uGITCache.GetUGovernITDeploymentID());
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/ShowDashboardDetails.aspx");
            filteredDataDetailPage = UGITUtility.GetAbsoluteURL(configManager.GetValue("FilterTicketsPageUrl"));


            if (Sidebar)
            {
                ctrContainers.CssClass = "ctrcontainer-sidebar";
            }

            if (ShowControl)
            {
                List<Control> controls = new List<Control>();
                controls.Add(ContentControl);
                rDashboardGroup.DataSource = controls;
                rDashboardGroup.DataBind();
            }
            else
            {
                needToViewDashboards = DashboardList;
                rDashboardGroup.DataSource = needToViewDashboards;
                rDashboardGroup.DataBind();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
          
            base.OnPreRender(e);
        }

        protected void rDashboardGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string newDashboardID = Guid.NewGuid().ToString();

                RepeaterItem rItem = e.Item;
                if (!(e.Item.DataItem is Dashboard) && ShowControl && ContentControl != null)
                {
                    Control control = (Control)e.Item.DataItem;

                    Panel panelDashboardContent = (Panel)rItem.FindControl("panelDashboardContent");
                    panelDashboardContent.Controls.Clear();
                    panelDashboardContent.Controls.Add(ContentControl);

                    Panel dashboardTopAction = (Panel)e.Item.FindControl("dashboardTopAction");
                    dashboardTopAction.Visible = false;

                    Panel dashboardBottomAction = (Panel)e.Item.FindControl("dashboardBottomAction");
                    dashboardBottomAction.Visible = false;

                    Panel dashboardDescription = (Panel)e.Item.FindControl("dashboardDescription");

                    dashboardDescription.Visible = false;

                    HtmlTableRow bottomType1 = (HtmlTableRow)rItem.FindControl("bottomType1");
                    HtmlTableRow bottomType2 = (HtmlTableRow)rItem.FindControl("bottomType2");
                    HtmlTableRow topType1 = (HtmlTableRow)rItem.FindControl("topType1");
                    topType1.Visible = true;
                    bottomType1.Visible = true;
                    bottomType2.Visible = false;

                    Panel dashboardContainerMain = (Panel)rItem.FindControl("dashboardContainerMain");
                    dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", PanelWidth.Value - 33));

                    dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", PanelHeight.Value - 29));
                }
                else
                {

                    Unit pHeight = new Unit();
                    Dashboard dashboard = (Dashboard)e.Item.DataItem;
                    dashboard.PanelWidth = (int)PanelWidth.Value;

                    ChartSetting chartSetting = dashboard.panel as ChartSetting;
                    PanelSetting panelSetting = dashboard.panel as PanelSetting;

                    Panel dashboardMailContainer = (Panel)e.Item.FindControl("dashboardMainContainer");
                    dashboardMailContainer.Attributes.Add("panelInstanceID", newDashboardID.ToString());

                    Panel dashboardDescription = (Panel)e.Item.FindControl("dashboardDescription");
                    Panel panelDashboardContent = (Panel)rItem.FindControl("panelDashboardContent");
                    Literal titleCtr = (Literal)e.Item.FindControl("cdpTitle");
                    HtmlGenericControl cdpTitleContainer = (HtmlGenericControl)rItem.FindControl("cdpTitleContainer");

                    HtmlTableRow bottomType1 = (HtmlTableRow)rItem.FindControl("bottomType1");
                    HtmlTableRow bottomType2 = (HtmlTableRow)rItem.FindControl("bottomType2");
                    HtmlTableRow topType1 = (HtmlTableRow)rItem.FindControl("topType1");

                    if (e.Item.ItemIndex != 0)
                    {
                        topType1.Visible = false;
                    }

                    if (e.Item.ItemIndex < DashboardList.Count - 1)
                    {
                        bottomType1.Visible = false;
                        bottomType2.Visible = true;
                    }

                    if (e.Item.ItemIndex == DashboardList.Count - 1)
                    {
                        bottomType1.Visible = true;
                        bottomType2.Visible = false;
                    }

                    Panel dashboardContainerMain = (Panel)rItem.FindControl("dashboardContainerMain");
                    dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", PanelWidth.Value - 24));
                    HtmlTableCell tdMiddleContent = (HtmlTableCell)e.Item.FindControl("tdMiddleContent");
                    Panel dashboardBottomAction = (Panel)e.Item.FindControl("dashboardBottomAction");

                    if  (topType1.Visible && bottomType1.Visible)
                    {
                        pHeight = new Unit(dashboard.PanelHeight - 16, UnitType.Pixel);
                        dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Top, "8px");
                        dashboardDescription.Style.Add(HtmlTextWriterStyle.Top, "-5px");
                    }
                    else if(topType1.Visible && bottomType2.Visible)
                    {
                        pHeight = new Unit(dashboard.PanelHeight - 16, UnitType.Pixel);
                    }
                    else if (!topType1.Visible && bottomType2.Visible)
                    {
                        pHeight = new Unit(dashboard.PanelHeight - 16, UnitType.Pixel);
                    }
                    else if (!topType1.Visible && bottomType1.Visible)
                    {
                        pHeight = new Unit(dashboard.PanelHeight - 16, UnitType.Pixel);
                        if (chartSetting != null)
                        {
                            tdMiddleContent.VAlign = "middle";
                        }
                    }
                    else if (!topType1.Visible && !bottomType1.Visible)
                    {
                         pHeight = new Unit(dashboard.PanelHeight, UnitType.Pixel);
                    }

                    if (bottomType1.Visible)
                    {
                        dashboardBottomAction.Style.Add(HtmlTextWriterStyle.Top, "9px");
                        dashboardBottomAction.Style.Add("right", "-5px");
                       
                    }
                    
                    dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", pHeight.Value));

                    HtmlGenericControl container = new HtmlGenericControl("Div");
                    container.Attributes.Add("panelID", dashboard.ID.ToString());
                    container.Attributes.Add("uniqueID", dashboard.ID.ToString());
                    panelDashboardContent.Controls.Clear();
                    panelDashboardContent.Controls.Add(container);

                    if (chartSetting != null)
                    {
                        WebChartControl chart = new WebChartControl();
                        ChartSetting panel = (ChartSetting)dashboard.panel;
                        DevxChartHelper chartHelper = new DevxChartHelper(panel,HttpContext.Current.GetManagerContext());

                        dashboard.PanelWidth = dashboard.PanelWidth + 17;
                        chart = chartHelper.GetChart(true, dashboard, pHeight, false);
                        container.Controls.Add(chart);
                    }

                    titleCtr.Text = dashboard.Title;
                    cdpTitleContainer.Attributes.Add("titletext", Uri.EscapeUriString(dashboard.Title.Trim()));
                    cdpTitleContainer.Attributes.Add("title", Uri.EscapeUriString(dashboard.DashboardDescription.Trim()));
                    if (this.Sidebar)
                    {
                        cdpTitleContainer.Style.Add(HtmlTextWriterStyle.FontSize, "9px");
                    }

                    if (e.Item.ItemIndex != 0)
                    {
                        dashboardDescription.CssClass = "cg-d-description1";
                        if (this.Sidebar)
                        {
                            dashboardDescription.Style.Add(HtmlTextWriterStyle.Top, "0px");
                        }
                    }

                    if (chartSetting != null)
                    {
                        dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Left, "-11px");
                    }
                    else if (panelSetting != null)
                    {
                        dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Left, "-5px");
                    }
                    //Actions
                    Panel dashboardLocalContainer = (Panel)e.Item.FindControl("dashboardLocalContainer");
                    if(bottomType1.Visible)
                    dashboardLocalContainer.Attributes.Add("style", "position: absolute;right:0px;top:-14px;");
                    else
                    dashboardLocalContainer.Attributes.Add("style", "position: absolute;right:0px;top:-13px;");
                    if (Sidebar)
                    {
                        dashboardLocalContainer.Visible = false;
                    }

                    List<ChartDimension> dimensionViews = null;
                    if (chartSetting != null && chartSetting.Dimensions != null)
                        dimensionViews = chartSetting.Dimensions.Where(x =>chartSetting.Dimensions.IndexOf(x) != 0 &&  x.ShowInDropDown).ToList();
                   

                    if (chartSetting != null && dimensionViews != null && dimensionViews.Count > 0)
                    {
                        DropDownList ddlDimensionFilter = new DropDownList();
                        ddlDimensionFilter.Items.Add(new ListItem(chartSetting.Dimensions[0].Title, "0"));
                        foreach (ChartDimension dims in dimensionViews)
                        {
                            ddlDimensionFilter.Items.Add(new ListItem(dims.Title, chartSetting.Dimensions.IndexOf(dims).ToString()));
                        }
                        ddlDimensionFilter.Attributes.Add("onchange", "pmenuDimensionItemClick(this)");
                        ddlDimensionFilter.CssClass = "dimensionmenu-super";
                        dashboardLocalContainer.Controls.Add(ddlDimensionFilter);
                        ddlDimensionFilter.SelectedIndex = ddlDimensionFilter.Items.IndexOf(ddlDimensionFilter.Items.FindByValue(DrillDownIndex.ToString()));
                        if (bottomType2.Visible)
                        {
                            ddlDimensionFilter.Style.Add("left", "-20px");
                        }
                        else
                        {
                            ddlDimensionFilter.Style.Add("left", "-10px");
                        }
                    }

                    if (chartSetting != null && !string.IsNullOrEmpty(chartSetting.BasicDateFitlerStartField) && !chartSetting.HideDateFilterDropdown)
                    {
                        DropDownList ddlLocalDateFilter = new DropDownList();
                        List<string> dateViews = DashboardCache.GetDateViewList();
                        foreach (string view in dateViews)
                        {
                            ddlLocalDateFilter.Items.Add(new ListItem(view, view));
                        }
                        ddlLocalDateFilter.Items.Insert(0, new ListItem("Select All", "Select All"));
                        ddlLocalDateFilter.Attributes.Add("onChange", string.Format("setLocalDateFilter(this,'{0}')", newDashboardID));
                        ddlLocalDateFilter.CssClass = "localdatefilter-super";
                        if (!string.IsNullOrEmpty(chartSetting.BasicDateFilterDefaultView))
                        {
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
                        }

                        ddlLocalDateFilter.Width = 110;
                        dashboardLocalContainer.Controls.Add(ddlLocalDateFilter);
                    }

                    if (chartSetting != null)
                    {

                        Panel dashboardTopAction = (Panel)e.Item.FindControl("dashboardTopAction");
                        PlaceHolder dashboardActionIcons = (PlaceHolder)e.Item.FindControl("dashboardActionIcons");
                        if (e.Item.ItemIndex != 0)
                        {
                            dashboardTopAction.CssClass = "cg-dashboardtopaction-type2";
                        }

                        if (!dashboard.panel.HideZoomView)
                        {
                            Image maximizeIcon = new Image();
                            maximizeIcon.ID = "maximizeicon";
                            maximizeIcon.ToolTip = "Zoom";
                            maximizeIcon.ImageUrl = "/content/images/maximize-icon12x12.png";
                            maximizeIcon.Attributes.Add("onClick", string.Format("maximizeDashboard(this, '{0}')", newDashboardID));
                            maximizeIcon.Attributes.Add("style", string.Format("right:{0}px", -20));
                            maximizeIcon.CssClass = "cg-dashboardaction-icon";
                            dashboardActionIcons.Controls.Add(maximizeIcon);
                        }

                        Label moreActions = new Label();
                        moreActions.Style.Add("float", "right");
                        moreActions.Style.Add(HtmlTextWriterStyle.Display, "none");
                        moreActions.CssClass = "dashboardacton-moreaction";
                        dashboardActionIcons.Controls.Add(moreActions);
                        if (!dashboard.panel.HideTableView)
                        {
                            Image tableIcon = new Image();
                            tableIcon.ID = "tableIcon";
                            tableIcon.ToolTip = "Show as Table";
                            tableIcon.ImageUrl = "/content/images/table-icon12x12.png";
                            tableIcon.Attributes.Add("onClick", string.Format("convertDashboardInTable(this, '{0}')", newDashboardID));
                            tableIcon.Attributes.Add("style", string.Format("right:{0}px", -19));
                            tableIcon.CssClass = "cg-dashboardaction-icon";
                            moreActions.Controls.Add(tableIcon);
                        }

                        if (!dashboard.panel.HidewDownloadView)
                        {
                            Image csvIcon = new Image();
                            csvIcon.ID = "csvIcon";
                            csvIcon.ToolTip ="Export to CSV";
                            csvIcon.ImageUrl = "/content/images/csv-icon12x12.png";
                            csvIcon.Attributes.Add("onClick", string.Format("getCSVOfDashboard(this, '{0}')", newDashboardID));
                            csvIcon.Attributes.Add("style", string.Format("right:{0}px", -19));
                            csvIcon.CssClass = "cg-dashboardaction-icon";
                            moreActions.Controls.Add(csvIcon);
                        }

                        if (this.Sidebar)
                        {
                            dashboardActionIcons.Visible = false;
                        }
                    }

                    Image cdpIcon = (Image)e.Item.FindControl("cdpIcon");
                    cdpIcon.Visible = false;
                    cdpIcon.Width = 32;
                    cdpIcon.Height = 32;
                    if (this.Sidebar)
                    {
                        dashboardDescription.Style.Add(HtmlTextWriterStyle.FontSize, "9px");
                        cdpIcon.Width = 20;
                        cdpIcon.Height = 20;
                    }

                    if (panelSetting != null)
                    {
                        XmlDocument paneldoc = new XmlDocument();

                        if (dashboard.Icon != null && Convert.ToString(dashboard.Icon.Trim()) != string.Empty)
                        {
                            cdpIcon.Visible = true;
                            cdpIcon.ImageUrl = UGITUtility.GetAbsoluteURL(dashboard.Icon);
                        }
                        List<DashboardPanelLink> Kpis = panelSetting.Expressions.Where(x => x.IsHide == false).ToList();

                        // For Panel URL
                        if (panelSetting.Expressions.Count > 0 && panelSetting.Expressions.Exists(x => x.UseAsPanel == true))
                        {
                            DashboardPanelLink panelLink = panelSetting.Expressions.FirstOrDefault(x => x.UseAsPanel == true);
                            //panelDashbaordPLink.Attributes.Add("title", panelLink.Title);
                            ExpressionCalc exp = new ExpressionCalc(panelLink.DashboardTable,HttpContext.Current.GetManagerContext());
                            string href = string.Empty;


                            if (panelLink != null && !panelLink.StopLinkDetail)
                            {
                                //panelDashbaordPLink.Attributes.Add("title", panelLink.Title);
                                string url = exp.GetKPIUrl(dashboard.ID, panelLink);

                             
                                if (url != string.Empty)
                                {
                                    string startQry = "?";
                                    if (panelLink.LinkUrl.Contains("?"))
                                    {
                                        startQry = "&";
                                    }
                                    if (panelLink.FormulaId > 0 || panelLink.ExpressionID > 0)
                                        url += string.Format("{0}fID={1}&eID={2}", startQry, panelLink.FormulaId, panelLink.ExpressionID);
                                    href = url;
                                }

                                //if (panelLink.ScreenView == 1)
                                //{
                                //    href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, panelLink.Title);
                                //}
                                Panel ctrContainer = (Panel)e.Item.FindControl("ctrContainer");
                                ctrContainer.Style.Add(HtmlTextWriterStyle.Cursor, "Pointer");
                                ctrContainer.Attributes.Add("onclick",string.Format("openPanelLink(this,'{0}','{1}','{2}')",href, panelLink.ScreenView,panelLink.Title));
                                //if (panelLink.ScreenView == 1)
                                //{
                                //    ctrContainer.Attributes.Add("onclick", href);
                                //}
                                //else if (panelLink.ScreenView == 2)
                                //{
                                //    ctrContainer.Attributes.Add("onclick", string.Format("window.open(\"{0}\", '_blank'", href));
                                //}
                                //else
                                //{
                                //    ctrContainer.Attributes.Add("onclick", string.Format("window.location.href=\"{0}\";", href));
                                //}
                            }

                        }

                        HtmlGenericControl pContainer = new HtmlGenericControl("Div");

                        HtmlGenericControl pTable = new HtmlGenericControl("Table");
                        pTable.Attributes.Add("cellspacing", "1");
                        pTable.Attributes.Add("cellpadding", "1");
                        pTable.Attributes.Add("width", "100%");
                        pContainer.Controls.Add(pTable);
                        foreach (DashboardPanelLink kpi in Kpis)
                        {
                            if (kpi.Title.Trim() != string.Empty)
                            {
                                ExpressionCalc exp = new ExpressionCalc(kpi.DashboardTable, HttpContext.Current.GetManagerContext());

                                string url = exp.GetKPIUrl(dashboard.ID, kpi);
                                string href = string.Empty;
                                if (!kpi.StopLinkDetail)
                                {
                                    if (kpi.ScreenView == 1)
                                    {
                                        href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, kpi.Title);
                                    }
                                    else if (kpi.ScreenView == 2)
                                    {
                                        if (Request["isdlg"] != null)
                                        {
                                            if (url.IndexOf("?") == -1)
                                                url += "?";
                                            url = string.Format("{0}&isdlg={1}", url, Request["isdlg"]);
                                        }
                                        href = string.Format("window.open(\"{0}\",'_blank')", url);
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
                                //string kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='event.cancelBubble = true;{1}'><td style='text-align:right;color:#2D2D2D;'>{3}</td><td class='dashboardkpi-td kpiresult' align='center'  style='color:{4}'><span class='dashboardkpi-txt'>{0}</span></td></tr>", "loading...", string.Format("openPanelLink(this,'{0}','{1}','{2}')", href, kpi.ScreenView, kpi.Title), kpi.LinkID, kpi.Title, kpi.FontColor);// kpi.ExpressionFormat
                                string kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='event.cancelBubble = true;{1}'><td style='text-align:right;color:#2D2D2D;'>{3}</td><td style='width:60%' class='dashboardkpi-td kpiresult' align='center'  style='color:{4}'><span class='dashboardkpi-txt'>{0}</span></td></tr>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor);// kpi.ExpressionFormat
                                if (kpi.HideTitle)
                                {
                                    kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='event.cancelBubble = true;{1}'><td colspan='2' class='dashboardkpi-td kpiresult' align='center' style='color:{4}'><span class='dashboardkpi-txt' >{0}</span></td></tr>", "loading...", string.Format("openPanelLink(this,'{0}','{1}','{2}')", href, kpi.ScreenView, ""), kpi.LinkID, kpi.Title, kpi.FontColor);
                                }

                                //kpiString = kpiString.Replace("$exp$", kpi.ExpressionFormat);
                                pTable.Controls.Add(new LiteralControl(kpiString));
                            }
                        }
                        panelDashboardContent.Controls.Add(pContainer);

                    }


                    //Script
                    Literal scriptCtr = (Literal)e.Item.FindControl("scriptCtr");
                    StringBuilder script = new StringBuilder();
                    script.Append("<script type='text/javascript'>");
                    script.Append("$(function() {");
                    script.AppendFormat("configurDashboardUrls(\"{0}\", \"{1}\", \"{2}\");", HttpContext.Current.Request.Url, filteredDataDetailPage, dasbhoardViewPage);
                    script.AppendFormat("ugitDashboardData(\"{10}\", {0}ViewID:\"{13}\", PanelID: \"{2}\", Width:\"{3}\", Height:\"{4}\", ViewType:'1', Type:\"{5}\", LocalFilter:\"{6}\", DimensionFilter :\"{7}\", ExpressionFilter:\"{8}\", GlobalFilter:\"{9}\", Sidebar:\"{11}\", Width:\"{12}\" {1});",
                        "{", "}", dashboard.ID, PanelWidth, pHeight.Value, dashboard.DashboardType.ToString().ToLower(), LocalFilter, DrillDownIndex, ExpressionFilter, GlobalFilter, newDashboardID, Sidebar, PanelWidth.Value, ViewID);

                    if (dashboard.DashboardType == DashboardType.Chart)
                    {
                        script.AppendFormat("updateCharts(\"{0}\");", newDashboardID);
                    }
                    else if (dashboard.DashboardType == DashboardType.Panel)
                    {
                       script.AppendFormat("updateKPIs(\"{0}\");", newDashboardID);
                    }
                    script.Append("});");
                    script.Append("</script>");
                    scriptCtr.Text = script.ToString();
                }
            }
        }
    }
}
