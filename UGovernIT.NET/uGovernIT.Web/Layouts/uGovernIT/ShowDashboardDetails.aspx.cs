using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Linq;
using System.Collections.Generic;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class ShowDashboardDetails : UPage
    {
        protected string frameId;
        protected bool printEnable;
        protected bool startDownload;
        DashboardManager objDashboardManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        DashboardPanelViewManager objDashboardPanelViewManager = new DashboardPanelViewManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            if (Request["enablePrint"] != null)
            {
                printEnable = true;
            }

            base.OnInit(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //Util.Log.ULog.WriteLog(HttpContext.Current.CurrentUser()?.Name + ": Visited: " + "Page ShowDashboardDetails.aspx");
            CreateDashboardViews();
        }


        public void CreateDashboardViews()
        {
            string dashboardViewInfo = Server.UrlDecode(Request["dashboards"]);
            if (dashboardViewInfo == null)
            {
              
                return;
            }

            DashboardFilter[] dashboardFilters = null;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            dashboardFilters = serializer.Deserialize<DashboardFilter[]>(dashboardViewInfo.Replace(":NaN",":0").ToString());
            //temp to check
            
            if (dashboardFilters == null || dashboardFilters.Length <= 0)
            {
                return;
            }

            bool noCache = UGITUtility.StringToBoolean(Request["noCache"]);
            List<Dashboard> uDashboards = objDashboardManager.LoadDashboardPanels(dashboardFilters.Select(x => (long)x.DashboardID).ToList());
            foreach (DashboardFilter dashboardFilter in dashboardFilters)
            {
                //remover after check
                //dashboardFilter.DashboardViewID = 1;
                DashboardPanelView view = objDashboardPanelViewManager.LoadByID(dashboardFilter.DashboardViewID);

                Dashboard dashboardInfo = uDashboards.FirstOrDefault(x => x.ID == dashboardFilter.DashboardID);
               
                //Check current user has permission to see dashboard or not
                if (dashboardInfo != null && (string.IsNullOrWhiteSpace(dashboardInfo.DashboardPermission) || objDashboardManager.HasPermission(HttpContext.Current.CurrentUser(),dashboardInfo)))
                {
                    ChartSetting chartSetting = (ChartSetting)dashboardInfo.panel;
                    chartSetting.IsCacheChart = !noCache;
                    DevxChartHelper chartH = new DevxChartHelper(chartSetting,HttpContext.Current.GetManagerContext());
                    chartH.UseAjax = true;
                    chartH.GlobalFilter = DevxChartHelper.GetGlobalFilter(HttpContext.Current.GetManagerContext(), dashboardFilter.GlobalFilter, view);
                    chartH.LocalFilter = dashboardFilter.LocalFilter;
                    int dimensionIndex = 0;
                    int.TryParse(dashboardFilter.DimensionFilter, out dimensionIndex);
                    chartH.DrillDownFilter = dimensionIndex;
                    chartH.DatapointFilter = dashboardFilter.ExpressionFilter;
                    chartH.ChartTitle = dashboardInfo.Title;

                    dashboardInfo.PanelWidth = dashboardFilter.ZoomWidth;
                    dashboardInfo.PanelHeight = dashboardFilter.ZoomHeight;

                    if (dashboardFilter.DownloadView == DownloadViewType.None)
                    {
                        if (dashboardFilter.View == DashboardView.Chart)
                        {
                            managementControls.Controls.Add(GetChartView(dashboardInfo, dashboardFilter));
                        }
                        else if (dashboardFilter.View == DashboardView.Table)
                        {
                            managementControls.Controls.Add(GetTableView(chartH, dashboardFilter));
                        }
                        else
                        {
                            managementControls.Controls.Add(GetChartView(dashboardInfo, dashboardFilter));
                            managementControls.Controls.Add(GetTableView(chartH, dashboardFilter));
                        }
                    }

                    if (dashboardFilter.DownloadView == DownloadViewType.CSV)
                    {
                        if (Request["startDownload"] != null)
                        {
                            DataTable table = chartH.GetTableView();
                            if (table.Columns.Contains("ExpressionIndex"))
                            {
                                table.Columns.Remove(table.Columns["ExpressionIndex"]);
                            }
                            string csvData = UGITUtility.ConvertTableToCSV(table);

                            string attachment = string.Format("attachment; filename={0}.csv", dashboardInfo.Title.Trim().Replace(" ", "_")); ;
                            Response.Clear();
                            Response.ClearHeaders();
                            Response.ClearContent();
                            Response.AddHeader("content-disposition", attachment);
                            Response.ContentType = "text/csv";
                            Response.Write(csvData.ToString());
                            Response.End();
                        }
                        else
                        {
                            startDownload = true;
                            LiteralControl literal = new LiteralControl();
                            literal.Text = "If your download does not start in 30 seconds, Click <a class='downloadlink' href='javascript:void(0)' onclick='startDownload()'>Start Download</a>";
                            managementControls.Controls.Add(literal);
                            managementControls.Controls.Add(GetTableView(chartH, dashboardFilter));
                        }
                    }
                }
            }
        }

        private Control GetTableView(DevxChartHelper chartH, DashboardFilter dfilter)
        {
            Table table = new Table();
            table.CssClass = "ro-table";
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Attributes.Add("rules", "all");
            table.Attributes.Add("frame", "box");

            DataTable datapointTable = chartH.GetTableView();
            TableHeaderRow header = new TableHeaderRow();
            header.CssClass = "ro-header";
            table.Rows.Add(header);
            if (datapointTable.Columns.Contains("ExpressionIndex"))
            {
                datapointTable.Columns.Remove(datapointTable.Columns["ExpressionIndex"]);
            }

            foreach (DataColumn column in datapointTable.Columns)
            {

                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = column.ColumnName;
				
				bool isCurrency = false;
                if (column.ExtendedProperties["expressionIndex"] != null && chartH.ChartSetting.Expressions != null && chartH.ChartSetting.Expressions.Count > UGITUtility.StringToInt(column.ExtendedProperties["expressionIndex"]))
                {
                    ChartExpression expression = chartH.ChartSetting.Expressions[UGITUtility.StringToInt(column.ExtendedProperties["expressionIndex"])];
                    isCurrency = expression.IsCurrency;
                }

                if (isCurrency)
                {
                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                }
                else
                    cell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                header.Cells.Add(cell);
            }

           

            foreach (DataRow row in datapointTable.Rows)
            {
                TableRow datapointRow = new TableRow();
                table.Rows.Add(datapointRow);
                if (datapointTable.Rows.IndexOf(row) % 2 == 0)
                {
                    datapointRow.CssClass = "ro-item";
                }
                else
                {
                    datapointRow.CssClass = "ro-alternateitem";
                }

                foreach (DataColumn column in datapointTable.Columns)
                {
                    TableCell cell = new TableCell();
                    cell.CssClass = "ro-padding";
                    cell.Text = Convert.ToString(row[column]);
                     bool isCurrency = false;
                    if (column.ExtendedProperties["expressionIndex"] != null && chartH.ChartSetting.Expressions != null && chartH.ChartSetting.Expressions.Count > UGITUtility.StringToInt(column.ExtendedProperties["expressionIndex"]))
                    {
                        ChartExpression expression  = chartH.ChartSetting.Expressions[UGITUtility.StringToInt(column.ExtendedProperties["expressionIndex"])];
                        isCurrency = expression.IsCurrency;
                    }

                    if (isCurrency)
                    {
                        cell.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                        cell.Text = string.Format("${0:n2}", row[column]); 
                    }
                    else if (column.DataType == typeof(double) || column.DataType == typeof(float) || column.DataType == typeof(decimal))
                    {
                        cell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                        cell.Text = string.Format("{0:#,0.##}", row[column]);
                    }
                    else
                        cell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    datapointRow.Cells.Add(cell);
                }
            }

            return table;
        }


        private Control GetChartView(Dashboard dashboard, DashboardFilter dfilter)
        {
            if (dfilter.ZoomWidth <= 10)
            {
                dfilter.ZoomWidth = 300;
            }

            if (dfilter.ZoomHeight <= 10)
            {
                dfilter.ZoomHeight = 300;
            }

            CustomDashboardPanel panel = (CustomDashboardPanel)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/CustomDashboardPanel.ascx");

            panel.Title = dfilter.Title;
            panel.dashboard = dashboard;
            dashboard.PanelWidth = dfilter.ZoomWidth;
            dashboard.PanelHeight = dfilter.ZoomHeight;
            panel.ViewID = dfilter.DashboardViewID;
            panel.BorderStyle = dfilter.BorderStyle;
            panel.GlobalFilter = dfilter.GlobalFilter;
            panel.LocalFilter = dfilter.LocalFilter;
            panel.ExpressionFilter = dfilter.ExpressionFilter;
            int dimensionIndex = 0;
            int.TryParse(dfilter.DimensionFilter, out dimensionIndex);
            panel.DrillDownIndex = dimensionIndex;
            panel.DrillDownBackArray = dfilter.DrillDownArray;
            if (dashboard.panel != null)
            {
                dashboard.panel.HideTableView = false;
                dashboard.panel.HidewDownloadView = false;
                dashboard.panel.HideZoomView = true;
            }
            panel.UseAjax = true;

            return panel;
        }


        private class DashboardFilter
        {
            public int DashboardViewID { get; set; }
            public int DashboardID { get; set; }
            public string GlobalFilter { get; set; }
            public string LocalFilter { get; set; }
            public string DimensionFilter { get; set; }
            public string ExpressionFilter { get; set; }
            public bool ZoomView { get; set; }
            public DashboardView View { get; set; }
            public DownloadViewType DownloadView { get; set; }
            public int ZoomWidth { get; set; }
            public int ZoomHeight { get; set; }
            public string BorderStyle { get; set; }
            public string Title { get; set; }
            public string DrillDownArray { get; set; }
            public DashboardFilter()
            {
                GlobalFilter = string.Empty;
                LocalFilter = string.Empty;
                DimensionFilter = string.Empty;
                ExpressionFilter = string.Empty;
                View = DashboardView.Chart;
                DownloadView = DownloadViewType.None;
                Title = string.Empty;
                DrillDownArray = string.Empty;
            }
        }

        private enum DownloadViewType
        {
            None=0,
            CSV=1,
            PDF=2
        }

        private enum DashboardView
        {
            Chart=0,
            Table = 1,
            Both =2
        }
    }

   
}
