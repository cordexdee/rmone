using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraCharts;
using System.Web.UI.HtmlControls;
using DevExpress.XtraCharts.UI;
using DevExpress.XtraCharts.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class GanttReport : System.Web.UI.UserControl
    {
        public string TicketID;
        public string Module;
        public string GanttType;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Create a chart.
            WebChartControl chart = new WebChartControl();

            // Dock the chart into its parent and add it to the current form.
            projectPlanContainer.Controls.Add(chart);

            // Specify the BoundDataChanged event handler.
            chart.BoundDataChanged +=
                new BoundDataChangedEventHandler(chart_BoundDataChanged);

            // Create an empty Bar series and add it to the chart.
            Series seriesPlanned = new Series("Planned", ViewType.Gantt);
            Series seriesCompleted = new Series("Completed", ViewType.Gantt);
            chart.Series.Add(seriesPlanned);
            chart.Series.Add(seriesCompleted);

            // Generate a data table and bind the series to it.
            seriesPlanned.DataSource = CreateChartData();

            // Specify data members to bind the series.
            seriesPlanned.ArgumentScaleType = ScaleType.Auto;
            seriesPlanned.ArgumentDataMember = "Title";
            seriesPlanned.ValueScaleType = ScaleType.DateTime;
            seriesPlanned.ValueDataMembers.AddRange(new string[] { "StartDate", "DueDate" });

            seriesCompleted.DataSource = CreateChartData();
            seriesCompleted.ArgumentScaleType = ScaleType.Auto;
            seriesCompleted.ArgumentDataMember = DatabaseObjects.Columns.Title;
            seriesCompleted.ValueScaleType = ScaleType.DateTime;
            seriesCompleted.ValueDataMembers.AddRange(new string[] { DatabaseObjects.Columns.StartDate, DatabaseObjects.Columns.CompletionDate });

            chart.Series.AddRange(new Series[] { seriesPlanned, seriesCompleted });

            ((GanttSeriesView)seriesPlanned.View).BarWidth = 0.3;
            ((GanttSeriesView)seriesCompleted.View).BarWidth = 0.15;
            //((GanttSeriesView)seriesPlanned.View).Color = System.Drawing.Color.Red;
            //((GanttSeriesView)seriesCompleted.View).Color = System.Drawing.Color.Blue;

            chart.Width = new Unit(600, UnitType.Pixel);

            // Access the type-specific options of the diagram.
            GanttDiagram myDiagram = (GanttDiagram)chart.Diagram;
            myDiagram.AxisY.Interlaced = true;
            myDiagram.AxisY.DateTimeScaleOptions.GridSpacing = 2;
            myDiagram.AxisY.Label.TextPattern = "{V: MMMM dd}";
            // Add task links for the first Gantt series.
            ((GanttSeriesView)seriesPlanned.View).LinkOptions.ArrowHeight = 7;
            ((GanttSeriesView)seriesPlanned.View).LinkOptions.ArrowWidth = 11;
            for (int i = 1; i < seriesPlanned.Points.Count; i++)
            {
                seriesPlanned.Points[i].Relations.Add(seriesPlanned.Points[i - 1]);
            }

            //// Add a progress line.
            //ConstantLine progress =
            //new ConstantLine("Current progress", new DateTime(2006, 9, 10));
            //progress.ShowInLegend = false;
            //progress.Title.Alignment = ConstantLineTitleAlignment.Far;
            //myDiagram.AxisY.ConstantLines.Add(progress);

            // Adjust the legend.
            chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            //BarSeriesView view2 = ((BarSeriesView)chart.Series[1].View);
            //view2.BarWidth = 0.3;
            //view2.Color = System.Drawing.Color.Blue;
            //seriesCompleted.DataSource = CreateChartData();

            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        }

        private DataTable CreateChartData()
        {
            UGITTaskManager objUGITTaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            // Create an empty table.
            DataTable table = new DataTable("Table1");
            table = objUGITTaskManager.LoadTasksTable(Module,false,TicketID);

            return table;
        }

        private void chart_BoundDataChanged(object sender, EventArgs e)
        {
            WebChartControl chart = (WebChartControl)sender;
            for (int i = 1; i < chart.Series["Planned"].Points.Count; i++)
            {
                chart.Series["Planned"].Points[i].Relations.Add(chart.Series["Planned"].Points[i - 1]);
            }
        }
    }
}