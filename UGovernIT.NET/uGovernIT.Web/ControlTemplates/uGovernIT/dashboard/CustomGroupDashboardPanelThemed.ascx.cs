using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
namespace uGovernIT.Web
{
    public partial class CustomGroupDashboardPanelThemed : UserControl
    {
        public Dashboard DashboardObj { get; set; }
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
        public string BackgroundImage { get; set; }
        public string BackgroundIcon { get; set; }
        ApplicationContext _context = HttpContext.Current.GetManagerContext();
        ExpressionCalc objExpressionCalc;
        ConfigurationVariableManager objConfigurationVariableManager;
        protected void Page_Load(object sender, EventArgs e)
        {
            objConfigurationVariableManager = new ConfigurationVariableManager(_context);
            objExpressionCalc = new ExpressionCalc(_context);
            // uGITDashboardScript.Name = string.Format("{0}?rev={1}", uGITDashboardScript.Name, uGITCache.GetUGovernITDeploymentID());
            dasbhoardViewPage = UGITUtility.GetAbsoluteURL("/_layouts/15/ugovernit/ShowDashboardDetails.aspx");
            filteredDataDetailPage = UGITUtility.GetAbsoluteURL(objConfigurationVariableManager.GetValue("FilterTicketsPageUrl"));
          
           //rDashboardGroup.Style.Add("Background-image", BackgroundImage);

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Sidebar)
            {
                ctrContainer.CssClass = "ctrcontainer-sidebar";
            }

            string newDashboardID = Guid.NewGuid().ToString();

            if (ShowControl && ContentControl != null)
            {

                panelDashboardContent.Controls.Clear();
                panelDashboardContent.Controls.Add(ContentControl);
                if (!string.IsNullOrEmpty(BackgroundImage))
                {
                    panelDashboardContent.BackImageUrl = BackgroundImage;
                }
                dashboardTopAction.Visible = false;
                dashboardBottomAction.Visible = false;
                dashboardDescription.Visible = false;
                dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", PanelWidth.Value - 33));
                dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", PanelHeight.Value - 29));
            }
            else
            {
                Unit pHeight = new Unit();
                Dashboard dashboard = DashboardObj;
                dashboard.PanelWidth = (int)PanelWidth.Value;

                ChartSetting chartSetting = dashboard.panel as ChartSetting;
                PanelSetting panelSetting = dashboard.panel as PanelSetting;


                dashboardMainContainer.Attributes.Add("panelInstanceID", newDashboardID.ToString());
                divPanel.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", dashboard.PanelHeight));
                divPanel.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", dashboard.PanelWidth));
                if (!string.IsNullOrEmpty(BackgroundImage))
                {
                    divPanel.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("{0}", BackgroundImage));
                }
         
                //  panelDashboardContent.BackImageUrl = BackgroundImage;
                dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", PanelWidth.Value));
                if (!string.IsNullOrEmpty(BackgroundIcon))
                {
                    imgIcon.ImageUrl =UGITUtility.GetAbsoluteURL(BackgroundIcon);
                    imgIcon.Visible = true;
                }
                else
                {
                    imgIcon.Visible = false;
                }
               
                dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", dashboard.PanelHeight));
                //imgWhitePanel.Width = dashboard.Width;
                HtmlGenericControl container = new HtmlGenericControl("Div");
                container.Attributes.Add("panelID", dashboard.ID.ToString());
                container.Attributes.Add("uniqueID", dashboard.ID.ToString());
                panelDashboardContent.Controls.Clear();
                panelDashboardContent.Controls.Add(container);

                if (this.Sidebar)
                {
                    cdpTitleContainer.Style.Add(HtmlTextWriterStyle.FontSize, "9px");
                }

                if (panelSetting != null)
                {
                    XmlDocument paneldoc = new XmlDocument();
                    List<DashboardPanelLink> Kpis = panelSetting.Expressions.Where(x => x.IsHide == false).ToList();

                    //It impacts on performance. Don't need to load expression calc to load data. we are showing stats on ajax any way.
                    //List<ExpressionCalc> expressionsCal = new List<ExpressionCalc>();
                    //foreach (DashboardPanelLink kpi in Kpis)
                    //{
                    //    expressionsCal.Add(new ExpressionCalc(SPContext.Current.Web, kpi.DashboardTable));
                    //}
                    //ExpressionCalc.LoadFilterData(ref expressionsCal, filterTable, expressionTable);

                    // For Panel URL
                    if (panelSetting.Expressions.Count > 0 && panelSetting.Expressions.Exists(x => x.UseAsPanel == true))
                    {
                        DashboardPanelLink panelLink = panelSetting.Expressions.FirstOrDefault(x => x.UseAsPanel == true);
                        if ( panelLink != null && !panelLink.StopLinkDetail)
                        {
                            //panelDashbaordPLink.Attributes.Add("title", panelLink.Title);
                            
                            string url = objExpressionCalc.GetKPIUrl(dashboard.ID, panelLink);

                            string href = string.Empty;
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

                            if (panelLink.ScreenView == 1)
                            {
                                href = string.Format("window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, panelLink.Title);
                            }

                            dashboardContainerMain.Style.Add(HtmlTextWriterStyle.Cursor, "Pointer");

                            if (panelLink.ScreenView == 1)
                            {
                                dashboardContainerMain.Attributes.Add("onclick", href);
                            }
                            else if (panelLink.ScreenView == 2)
                            {
                                dashboardContainerMain.Attributes.Add("onclick", string.Format("javascript:window.open(\"{0}\",\"_blank\")", href));
                            }
                            else
                            {
                                dashboardContainerMain.Attributes.Add("onclick", string.Format("window.location.href=\"{0}\";", href));
                            }
                        }
                    }


                    HtmlGenericControl pContainer = new HtmlGenericControl("Div");
                    HtmlGenericControl pTable = new HtmlGenericControl("Table");
                    pTable.Attributes.Add("cellspacing", "0");
                    pTable.Style.Add("width", "100%");
                    pTable.Style.Add("text-align", "center");
                    pContainer.Controls.Add(pTable);
                    foreach (DashboardPanelLink kpi in Kpis)
                    {
                        if (kpi.Title.Trim() != string.Empty)
                        {
                         
                            string url = objExpressionCalc.GetKPIUrl(dashboard.ID, kpi);
                            string href = string.Empty;
                            if (!kpi.StopLinkDetail)
                            {
                                if (kpi.ScreenView == 1)
                                {
                                    href = string.Format("javascript:event.cancelBubble=true;window.parent.UgitOpenPopupDialog(\"{0}\",\"showalldetail=true&showFilterTabs=false\",\"{1}\",90,90,0)", url, kpi.Title);
                                }
                                else if (kpi.ScreenView == 2)
                                {
                                    if (Request["isdlg"] != null)
                                    {
                                        if (url.IndexOf("?") == -1)
                                            url += "?";
                                        url = string.Format("{0}&isdlg={1}", url, Request["isdlg"]);
                                    }
                                    href = string.Format("javascript:window.open(\"{0}\",\"_blank\")", url);
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
                            string kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='event.cancelBubble = true;{1}'><td style='text-align:right;color:#2D2D2D;'>{3}</td><td class='dashboardkpi-td kpiresult' align='center'  style='color:{4}'><strong class='dashboardkpi-txt'>{0}</strong></td></tr>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor);  //kpi.ExpressionFormat
                            if (kpi.HideTitle)
                            {
                                kpiString = string.Format("<tr style='cursor:pointer;' usepopup='1' kpilinkid='{2}'  title='{3}' class='kpitr' onclick='event.cancelBubble = true;{1}'><td colspan='2' class='dashboardkpi-td kpiresult' align='center' style='color:{4}'><strong class='dashboardkpi-txt' >{0}</strong></td></tr>", "loading...", href, kpi.LinkID, kpi.Title, kpi.FontColor);  //kpi.ExpressionFormat
                            }

                            //kpiString = kpiString.Replace("$exp$", kpi.ExpressionFormat);
                            pTable.Controls.Add(new LiteralControl(kpiString));
                        }
                    }
                    panelDashboardContent.Controls.Add(pContainer);

                }


                //Script
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
          
            base.OnPreRender(e);
        }

       
    }
}
