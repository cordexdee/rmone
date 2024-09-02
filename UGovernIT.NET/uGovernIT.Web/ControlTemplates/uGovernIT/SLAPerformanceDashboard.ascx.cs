using DevExpress.XtraCharts;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;
using Mode = uGovernIT.Utility.ShowMode;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class SLAPerformanceDashboard : UserControl
    {
        public string Width { get; set; }
        public string Height { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
        public string ShowMode { get; set; }
        public string DisplayUnit { get; set; }
        public string HeaderText { get; set; }
        public bool ShowTotal { get; set; }
        public string CreatedOn { get; set; }
        public string CompletedOn { get; set; }
        DataTable legendTable;
        protected string drilDownData = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=sladrildowndata");
        public string filterExp;
        public List<Tuple<string, DateTime, DateTime>> lstFilter = new List<Tuple<string, DateTime, DateTime>>();
        public bool IncludeOpenTickets { get; set; }

        protected override void OnInit(EventArgs e)
        {
            HeaderText = "Days";
            drilDownData = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=sladrildowndata&dUnit=d");
            if (DisplayUnit.Equals("h"))
            {
                HeaderText = "Hrs";
                drilDownData = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=sladrildowndata&dUnit=h");
            }


            BindPeriodFilter(ddlCreatedOn);
            BindPeriodFilter(ddlCompletedOn);
            if (!string.IsNullOrEmpty(CreatedOn))
                ddlCreatedOn.SelectedIndex = ddlCreatedOn.Items.IndexOf(ddlCreatedOn.Items.FindByValue(CreatedOn));
            if (!string.IsNullOrEmpty(CompletedOn))
                ddlCompletedOn.SelectedIndex = ddlCompletedOn.Items.IndexOf(ddlCompletedOn.Items.FindByValue(CompletedOn));
            chkslaperformanceOpen.Checked = IncludeOpenTickets;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Width))
                //Width = "700px";
            if (string.IsNullOrWhiteSpace(Height))
                //Height = "300px";

            if (!IsPostBack)
            {
                SetFilter();
                BindChartData();
                //divchartcontainer.Attributes.Add("style", string.Format("height:{0}px", ganttChart.Height.Value + 65));
            }

            filterExp = string.Format("{0}~#{1}", ddlCreatedOn.Value, ddlCompletedOn.Value);
        }

        private void SetBasicPropertyOfChart()
        {
            ganttChart.Width = new Unit(Width);
            ganttChart.Height = new Unit(Height);
            if (ganttChart.Width.Value < 200)
            {
                ganttChart.Width = new Unit("200px");
            }
            else
            {
                if (ShowMode == Mode.ChartOnly.ToString())//if Show Chart Only
                    ganttChart.Width = new Unit(ganttChart.Width.Value - 200, UnitType.Pixel);
            }

            if (ganttChart.Height.Value - 250 < 300)
                ganttChart.Height = new Unit("300px");
            else
                ganttChart.Height = new Unit(ganttChart.Height.Value, UnitType.Pixel);
            //ganttChart.Height = new Unit(ganttChart.Height.Value - 250, UnitType.Pixel);



            // Create two Gantt series.
            Series series1 = new Series("Target", ViewType.SideBySideGantt);
            Series series2 = new Series("Actual", ViewType.SideBySideGantt);

            series1.View.Color = ColorTranslator.FromHtml("#39cb71");
            series2.View.Color = ColorTranslator.FromHtml("#43e9e9");


            // Specify the date-time value scale type,
            // because it is qualitative by default.
            series1.ValueScaleType = ScaleType.Numerical;
            series2.ValueScaleType = ScaleType.Numerical;

            //Show crosshair axis lines and crosshair axis labels to see format values of crosshair labels 
            ganttChart.CrosshairOptions.ShowArgumentLabels = true;
            ganttChart.CrosshairOptions.ShowValueLabels = true;
            ganttChart.CrosshairOptions.ShowValueLine = true;
            ganttChart.CrosshairOptions.ShowArgumentLine = true;
            //series2.View.Color = Color.Yellow;
            ganttChart.Series.AddRange(new Series[] { series1, series2 });
            if (DisplayUnit.Equals("h"))
            {
                series1.CrosshairLabelPattern = string.Format("{0}: {1}", "{S}", "{VD:#0.##}h");
                series2.CrosshairLabelPattern = string.Format("{0}: {1}", "{S}", "{VD:#0.##}h");
            }
            else
            {
                series1.CrosshairLabelPattern = string.Format("{0}: {1}", "{S}", "{VD:#0.##}d");
                series2.CrosshairLabelPattern = string.Format("{0}: {1}", "{S}", "{VD:#0.##}d");
            }
            // Access the view-type-specific options of the second series.
            SideBySideGanttSeriesView myView2 = (SideBySideGanttSeriesView)series2.View;

            //myView2.MaxValueMarker.Visible = true;                                                       

            myView2.MaxValueMarker.Kind = MarkerKind.Star;
            myView2.MaxValueMarker.StarPointCount = 5;
            myView2.MaxValueMarker.Size = 10;

            //myView2.MinValueMarker.Visible = true;
            myView2.MinValueMarker.Kind = MarkerKind.Circle;
            myView2.MinValueMarker.Size = 10;

            myView2.BarWidth = 0.5;

            // Customize the chart (if necessary).
            if (ganttChart.Diagram == null)
            {
                ganttChart.Diagram = new GanttDiagram();
            }
            GanttDiagram myDiagram = (GanttDiagram)ganttChart.Diagram;

            myDiagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            myDiagram.AxisX.Title.Text = "Tasks";
            myDiagram.AxisY.Interlaced = true;
            myDiagram.AxisY.NumericScaleOptions.GridSpacing = 10;
            myDiagram.AxisY.Label.Angle = -30;
            myDiagram.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Customize the legend (if necessary).
            ganttChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            ganttChart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            ganttChart.Legend.Direction = LegendDirection.LeftToRight;

            ganttChart.Legend.FillStyle.FillMode = FillMode.Solid;
        }

        private void GetChartData(string moduleName, List<Tuple<string, DateTime, DateTime>> lstFilter, bool includeOpen = false)
        {
            try
            {
                ganttChart.Series.Clear();
                SetBasicPropertyOfChart();
                DataTable slaTable = uHelper.GetSLAAndTabularDashboardData(HttpContext.Current.GetManagerContext(), moduleName, lstFilter, DisplayUnit, includeOpen: includeOpen);                
                if (slaTable != null && slaTable.Rows.Count > 0)
                {
                    DataRow[] dtRow = slaTable.AsEnumerable().GroupBy(r => r.Field<string>(DatabaseObjects.Columns.Title)).Select(g => g.FirstOrDefault()).ToArray();
                    if (dtRow != null && dtRow.Length > 0)
                    {
                        slaTable = dtRow.CopyToDataTable();
                        DataView view = slaTable.DefaultView;
                        view.Sort = string.Format("{0},{1} asc", DatabaseObjects.Columns.StartStageStep, DatabaseObjects.Columns.Title);
                        slaTable = view.ToTable();
                    }
                }

                if (slaTable != null)
                    legendTable = slaTable.Copy();

                double totalTargetDays = 0.0;
                double totalActualDays = 0.0;

                var maxStepNo = Convert.ToInt32(slaTable.AsEnumerable().Max(x => x[DatabaseObjects.Columns.StartStageStep]));
                if (slaTable != null && slaTable.Rows.Count > 0)
                {
                    totalTargetDays = (double)slaTable.Compute("sum(SLATargetX2)", string.Empty);
                    totalActualDays = (double)slaTable.Compute("sum(SLAActualX2)", string.Empty);
                }

                double realDays = 0.0;
                if (totalTargetDays == totalActualDays)
                    realDays = totalTargetDays;
                else if (totalTargetDays > totalActualDays)
                    realDays = totalTargetDays;
                else if (totalTargetDays < totalActualDays)
                    realDays = totalActualDays;

                int constantLineDifference = 0;
                if (maxStepNo != 0)
                    constantLineDifference = (int)realDays / maxStepNo;

                int currentDiff = 0;
                for (int i = 0; i < maxStepNo; i++)
                {
                    currentDiff = i * constantLineDifference;
                    DataRow[] drColl = slaTable.Select(string.Format("{0}={1}", DatabaseObjects.Columns.StartStageStep, i + 1));
                    foreach (DataRow drupdateX1 in drColl)
                    {
                        drupdateX1["SLATargetX1"] = currentDiff;
                        drupdateX1["SLATargetX2"] = currentDiff + Math.Round(Convert.ToDouble(drupdateX1["SLATargetX2"]), 2);
                        drupdateX1["SLAActualX1"] = currentDiff;
                        drupdateX1["SLAActualX2"] = currentDiff + Math.Round(Convert.ToDouble(drupdateX1["SLAActualX2"]), 2);

                    }
                }

                Series targetS = ganttChart.Series["Target"];
                Series actualS = ganttChart.Series["Actual"];

                foreach (DataRow row in slaTable.Rows)
                {
                    if (Convert.ToString(row[DatabaseObjects.Columns.Title]) == string.Empty)
                        continue;

                    double target1 = UGITUtility.StringToDouble(row["SLATargetX1"]);
                    double target2 = UGITUtility.StringToDouble(row["SLATargetX2"]);
                    double actual1 = UGITUtility.StringToDouble(row["SLAActualX1"]);
                    double actual2 = UGITUtility.StringToDouble(row["SLAActualX2"]);
                    int pointIndex = targetS.Points.Add(new SeriesPoint(Convert.ToString(row[DatabaseObjects.Columns.Title]), new double[] { target1, target2 }));

                    TextAnnotation annotation = targetS.Points[pointIndex].Annotations.AddTextAnnotation();
                    if (DisplayUnit.Equals("h"))//"d" & "h" stand for Days & Hours
                        annotation.Text = string.Format("{0}h", Math.Round(target2 - target1, 2));
                    else
                        annotation.Text = string.Format("{0}d", Math.Round(target2 - target1, 2));

                    annotation.ShapePosition = new RelativePosition(30, 25); // 360 will put in line with bar, and 0 = no connector!
                    annotation.ShapeKind = ShapeKind.Rectangle;

                    pointIndex = actualS.Points.Add(new SeriesPoint(Convert.ToString(row[DatabaseObjects.Columns.Title]), new double[] { actual1, actual2 }));
                    annotation = actualS.Points[pointIndex].Annotations.AddTextAnnotation();
                    annotation.ShapePosition = new RelativePosition(360, 50);
                    if (DisplayUnit.Equals("h"))//"d" & "h" stand for Days & Hours
                        annotation.Text = string.Format("{0}h", Math.Round(actual2 - actual1, 2));
                    else
                        annotation.Text = string.Format("{0}d", Math.Round(actual2 - actual1, 2));

                    annotation.ShapeKind = ShapeKind.Rectangle;
                }
            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
        }

        protected void ganttChart_CustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
        {
            GanttDrawOptions gdOption = e.SeriesDrawOptions as GanttDrawOptions;
            if (gdOption != null)
                gdOption.FillStyle.FillMode = FillMode.Solid;
        }

        protected void rptSLAParent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                e.Item.Visible = ShowTotal;
                Label lbTotalTarget = e.Item.FindControl("lblSLATargetX2Total") as Label;
                Label lbTotalActual = e.Item.FindControl("lblSLAActualX2Total") as Label;
                if (legendTable != null && legendTable.Rows.Count > 0)
                {
                    lbTotalTarget.Text = Math.Round(Convert.ToDouble(legendTable.Compute("sum(SLATargetX2)", "")), 2).ToString();
                    lbTotalActual.Text = Math.Round(Convert.ToDouble(legendTable.Compute("sum(SLAActualX2)", "")), 2).ToString("n2");
                }
            }
        }

        private void BindPeriodFilter(ASPxComboBox ddl)
        {
            ddl.Items.Clear();
            if (ddl.Items.Count <= 0)
            {
                List<string> dateViews = DashboardCache.GetDateViewList();
                foreach (string item in dateViews)
                {
                    ListEditItem itm = new ListEditItem(item, item);
                    ddl.Items.Add(new ListEditItem(item, item));
                }

                ddl.Items.Insert(0, new ListEditItem("--ALL", string.Empty));
                if (ddl.Items.Count > 0)
                {
                    string itemText = "Custom";
                    ddl.Items.Insert(ddl.Items.Count, new ListEditItem(itemText, itemText));
                }
            }
        }

        protected void ddlCreatedOn_ValueChanged(object sender, EventArgs e)
        {
            ASPxComboBox cmbddl = sender as ASPxComboBox;
            if (cmbddl.UniqueID.EndsWith("ddlCreatedOn") && "Custom" == Convert.ToString(cmbddl.Value))
            {
                customfilterPopup.ShowOnPageLoad = true;
            }
            else if (cmbddl.UniqueID.EndsWith("ddlCompletedOn") && "Custom" == Convert.ToString(cmbddl.Value))
            {
                CompletedcustomfilterPopup.ShowOnPageLoad = true;
            }
            //
            SetFilter();
            BindChartData();
        }

        private void BindChartData()
        {
            GetChartData(Module, lstFilter, chkslaperformanceOpen.Checked);
            tdSLAParent.Visible = false;
            tdchart.Visible = false;
            if (ShowMode == Mode.TableOnly.ToString())//If show Table Only
                tdSLAParent.Visible = true;
            else if (ShowMode == Mode.ChartOnly.ToString())//if Show Chart Only
                tdchart.Visible = true;
            else if (string.IsNullOrEmpty(ShowMode) || ShowMode == Mode.Both.ToString())//if Show Both
            {
                tdSLAParent.Visible = true;
                tdchart.Visible = true;
            }

            if (tdSLAParent.Visible)
                rptSLAParent.DataBind();

            if (!IncludeOpenTickets || !chkslaperformanceOpen.Checked)
                divchartcontainer.Attributes.Add("style", string.Format("height:{0}px", ganttChart.Height.Value + 83));
            else
                divchartcontainer.Attributes.Add("style", string.Format("height:{0}px", ganttChart.Height.Value + 45));
        }

        protected void rptSLAParent_DataBinding(object sender, EventArgs e)
        {
            if (legendTable == null)
                rptSLAParent.DataSource = DefaultSchema();
            else
            {
                rptSLAParent.DataSource = legendTable;
            }
        }

        private DataTable DefaultSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(DatabaseObjects.Columns.Title);
            dt.Columns.Add("SLATargetX2");
            dt.Columns.Add("SLAActualX2");
            return dt;
        }

        protected void chkslaperformanceOpen_ValueChanged(object sender, EventArgs e)
        {
            if (lstFilter != null && lstFilter.Count > 0)
                lstFilter.Clear();
            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;
            string stringText = string.Empty;
            if (ddlCreatedOn.SelectedIndex != -1 && Convert.ToString(ddlCreatedOn.Value) == "Custom")
            {
                if (!string.IsNullOrEmpty(lbldatecStartDate.Text))
                    startDate = UGITUtility.StringToDateTime(lbldatecStartDate.Text);
                if (!string.IsNullOrEmpty(lbldatecEndDate.Text))
                    endDate = UGITUtility.StringToDateTime(lbldatecEndDate.Text);
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", startDate, endDate));
            }
            else if (ddlCreatedOn.SelectedIndex != -1)
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(ddlCreatedOn.Value), ref startDate, ref endDate, ref stringText);
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", startDate, endDate));
            }

            if (ddlCompletedOn.SelectedIndex != -1 && Convert.ToString(ddlCompletedOn.Value) == "Custom")
            {
                if (!string.IsNullOrEmpty(lbldatecomStartDate.Text))
                    startDate = UGITUtility.StringToDateTime(lbldatecomStartDate.Text);
                if (!string.IsNullOrEmpty(lbldatecomEndDate.Text))
                    endDate = UGITUtility.StringToDateTime(lbldatecomEndDate.Text);
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", startDate, endDate));
            }
            else if (ddlCompletedOn.SelectedIndex != -1)
            {
                uHelper.GetStartEndDateFromDateView(Convert.ToString(ddlCompletedOn.Value), ref startDate, ref endDate, ref stringText);
                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", startDate, endDate));
            }
            BindChartData();
        }

        void SetFilter()
        {
            string stringText = string.Empty;
            if (lstFilter != null && lstFilter.Count > 0)
                lstFilter.Clear();

            DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;

            if (ddlCreatedOn.SelectedIndex > 0) // 0 == ALL, so no filtering needed
            {
                if (ddlCreatedOn.SelectedIndex != -1 && !Convert.ToString(ddlCreatedOn.Value).Equals("Custom"))
                    uHelper.GetStartEndDateFromDateView(Convert.ToString(ddlCreatedOn.Value), ref startDate, ref endDate, ref stringText);
                else if (ddlCreatedOn.SelectedIndex != -1) // Custom
                {
                    startDate = UGITUtility.StringToDateTime(lbldatecStartDate.Text);
                    endDate = UGITUtility.StringToDateTime(lbldatecEndDate.Text);
                }

                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CreatedOn", startDate, endDate));
            }

            if (ddlCompletedOn.SelectedIndex > 0) // 0 == ALL, so no filtering needed
            {
                if (ddlCompletedOn.SelectedIndex != -1 && !Convert.ToString(ddlCompletedOn.Value).Equals("Custom"))
                    uHelper.GetStartEndDateFromDateView(Convert.ToString(ddlCompletedOn.Value), ref startDate, ref endDate, ref stringText);
                else if (ddlCompletedOn.SelectedIndex != -1) // Custom
                {
                    startDate = UGITUtility.StringToDateTime(lbldatecomStartDate.Text);
                    endDate = UGITUtility.StringToDateTime(lbldatecomEndDate.Text);
                }

                lstFilter.Add(new Tuple<string, DateTime, DateTime>("CompletedOn", startDate, endDate));
            }
        }

        protected void slaperfomancecallbackpanel_Callback(object sender, CallbackEventArgsBase e)
        {
            customfilterPopup.ShowOnPageLoad = false;
            CompletedcustomfilterPopup.ShowOnPageLoad = false;
            DateTime cSStartDate = DateTime.MinValue, cSEndDate = DateTime.MaxValue;
            DateTime comStartDate = DateTime.MinValue, comEndDate = DateTime.MaxValue;
            string stringText = string.Empty;
            if (lstFilter == null)
                lstFilter = new List<Tuple<string, DateTime, DateTime>>();
            if (lstFilter.Count > 0)
                lstFilter.Clear();

            if (e.Parameter == "slaperformanceCreatedOne" || Convert.ToString(ddlCreatedOn.Value).Equals("Custom"))
            {
                cSStartDate = dtStartdate.Date == DateTime.MinValue ? DateTime.Today : dtStartdate.Date;
                cSEndDate = dtEndDate.Date == DateTime.MinValue ? DateTime.Today : dtEndDate.Date;
                if (ddlCreatedOn.SelectedIndex != -1 && Convert.ToString(ddlCreatedOn.Value).Equals("Custom"))
                {
                    lbldatecStartDate.Text = cSStartDate.Date.ToString("yyyy-MM-dd");
                    lbldatecEndDate.Text = cSEndDate.Date.ToString("yyyy-MM-dd");
                    if (hdnCustomFilterValues.Contains("CreatedOnDate"))
                    {
                        string datefilter = Convert.ToString(hdnCustomFilterValues.Get("CreatedOnDate"));
                        string[] dateArr = UGITUtility.SplitString(datefilter, Constants.Separator4);
                        if (dateArr.Length == 2 && (UGITUtility.StringToDateTime(dateArr[0]) != UGITUtility.StringToDateTime(lbldatecStartDate.Text) || UGITUtility.StringToDateTime(dateArr[1]) != UGITUtility.StringToDateTime(lbldatecEndDate.Text)))
                        {
                            lbldatecStartDate.Text = cSStartDate.Date.ToString("yyyy-MM-dd");
                            lbldatecEndDate.Text = cSEndDate.Date.ToString("yyyy-MM-dd");
                        }
                    }
                    else
                        hdnCustomFilterValues.Set("CreatedOnDate", string.Format("{0}~#{1}", lbldatecStartDate.Text, lbldatecEndDate.Text));
                }
            }

            if (e.Parameter == "slaperformanceCompletedOne" || Convert.ToString(ddlCompletedOn.Value).Equals("Custom"))
            {
                comStartDate = cStartDate.Date == DateTime.MinValue ? DateTime.Today : cStartDate.Date;
                comEndDate = cEndDate.Date == DateTime.MinValue ? DateTime.Today : cEndDate.Date;

                if (ddlCompletedOn.SelectedIndex != -1 && Convert.ToString(ddlCompletedOn.Value).Equals("Custom"))
                {
                    lbldatecomStartDate.Text = comStartDate.Date.ToString("yyyy-MM-dd");
                    lbldatecomEndDate.Text = comEndDate.Date.ToString("yyyy-MM-dd");
                    if (hdnCustomFilterValues.Contains("CompletedOnDate"))
                    {
                        string datefilter = Convert.ToString(hdnCustomFilterValues.Get("CompletedOnDate"));
                        string[] dateArr = UGITUtility.SplitString(datefilter, Constants.Separator4);
                        if (dateArr.Length == 2 && (UGITUtility.StringToDateTime(dateArr[0]) != UGITUtility.StringToDateTime(lbldatecomStartDate.Text) || UGITUtility.StringToDateTime(dateArr[1]) != UGITUtility.StringToDateTime(lbldatecomEndDate.Text)))
                        {
                            lbldatecomStartDate.Text = comStartDate.Date.ToString("yyyy-MM-dd"); ;
                            lbldatecomEndDate.Text = comEndDate.Date.ToString("yyyy-MM-dd");
                        }
                    }
                    else
                        hdnCustomFilterValues.Set("CompletedOnDate", string.Format("{0}~#{1}", lbldatecomStartDate.Text, lbldatecomEndDate.Text));
                }
            }

            SetFilter();
            BindChartData();

        }
    }
}