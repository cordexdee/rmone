using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace uGovernIT.DxReport
{ 
    public partial class SumGanttChartSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        public SumGanttChartSubReport(DataTable datatable)
        {
            InitializeComponent();

            
            Series series1 = new Series("Planned", ViewType.Gantt);
            series1.ValueScaleType = ScaleType.DateTime;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            ((OverlappedGanttSeriesView)series1.View).LinkOptions.Thickness = 3;
            ((OverlappedGanttSeriesView)series1.View).LinkOptions.ArrowHeight = 7;
            ((OverlappedGanttSeriesView)series1.View).LinkOptions.ArrowWidth = 11;
            //((OverlappedGanttSeriesView)series1.View).FillStyle.FillMode = FillMode.Solid;
            //((OverlappedGanttSeriesView)series1.View).Color = System.Drawing.Color.FromArgb(10, 149, 233);


            List<SeriesPoint> pointLists1 = new List<SeriesPoint>();
            List<SeriesPoint> pointLists2 = new List<SeriesPoint>();

            Series series2 = new Series("Completed", ViewType.Gantt);
            series2.ValueScaleType = ScaleType.DateTime;
            series2.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            //((OverlappedGanttSeriesView)series2.View).Border.Thickness = 1;
            //((OverlappedGanttSeriesView)series2.View).Border.Color = Color.White;
            //((OverlappedGanttSeriesView)series2.View).FillStyle.FillMode = FillMode.Solid;
            //((OverlappedGanttSeriesView)series2.View).Color = System.Drawing.Color.FromArgb(0, 108, 0);

            DataView dv = datatable.DefaultView;
            dv.Sort = "ItemOrder ASC";
            
            foreach (DataRow dr in dv.ToTable().Rows)
            {
                DateTime duedatetime = Convert.ToDateTime(dr["DueDate"]);
                DateTime startdatetime = Convert.ToDateTime(dr["StartDate"]);
                string title = Convert.ToString(dr["Title"]);
                if (string.IsNullOrEmpty(title))
                    title = "No Title";
                int itemOrder  = Convert.ToInt32(dr["ItemOrder"]);
                //string predecessors = Convert.ToString(dr["PredecessorsByorder"]);
                double precentcomplete = uGovernIT.Utility.UGITUtility.StringToDouble(dr["PercentComplete"]);
                if (startdatetime == duedatetime)
                {
                    duedatetime = startdatetime.AddHours(8);
                }

                SeriesPoint point1 = new SeriesPoint(title, new DateTime[]{
                   startdatetime, duedatetime});
               
                point1.Tag = itemOrder;

                series1.Points.Add(point1);

                pointLists1.Add(point1);

                if (precentcomplete > 0)
                {
                    TimeSpan ts = (duedatetime - startdatetime);
                    DateTime duedate = startdatetime.AddHours((ts.TotalHours * precentcomplete) / 100);
                    SeriesPoint point2 = new SeriesPoint(title, new object[]{
                    (object)startdatetime, (object)duedate}, itemOrder);

                    pointLists2.Add(point2);
                    series2.Points.Add(point2);
                }

            }

            foreach (DataRow dr in datatable.Rows)
            {
                string predecessors = Convert.ToString(dr["Predecessors"]);
                int itemOrder = Convert.ToInt32(dr["ItemOrder"]);
               
                if (!string.IsNullOrEmpty(predecessors))
                {
                    string[] values = predecessors.Split(',');
                    foreach (string val in values)
                    {
                        int predecessor = Convert.ToInt32(val);
                        SeriesPoint cpoint = pointLists1.Find(p => (int)p.Tag == predecessor);
                        if (cpoint != null)
                        {
                            TaskLink tl = new TaskLink(cpoint, TaskLinkType.FinishToStart);
                            SeriesPoint ppoint = pointLists1.Find(p => (int)p.Tag == itemOrder);
                            ppoint.Relations.Add(tl);
                        }
                    }
                }
            }
            
            xrSummaryGanttChart.Series.AddRange(new Series[] { series1, series2 });

            // Access the view-type-specific options of the series.
            OverlappedGanttSeriesView myView1 = (OverlappedGanttSeriesView)series1.View;
            myView1.BarWidth = 0.6;

            OverlappedGanttSeriesView myView2 = (OverlappedGanttSeriesView)series2.View;
            myView2.BarWidth = 0.2;

            // Customize the chart (if necessary).
            GanttDiagram myDiagram = (GanttDiagram)xrSummaryGanttChart.Diagram;

            
            myDiagram.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.True;
            myDiagram.AxisX.Title.Text = "Tasks";
            myDiagram.AxisY.Interlaced = true;
           // myDiagram.AxisY.GridSpacing = 0.5;
            myDiagram.AxisY.Label.Angle = -30;
            myDiagram.AxisY.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            myDiagram.AxisY.Interlaced = true;
            //myDiagram.AxisY.DateTimeGridAlignment = DateTimeMeasurementUnit.Week;
            //myDiagram.AxisY.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
            //myDiagram.AxisY.DateTimeOptions.Format = DateTimeFormat.Custom;
            //myDiagram.AxisY.DateTimeOptions.FormatString = "MMM-dd";
            //myDiagram.AxisY.WorkdaysOnly = true;
            //myDiagram.AxisY.WorkdaysOptions.Workdays = Weekday.Monday | Weekday.Tuesday | Weekday.Wednesday | Weekday.Thursday | Weekday.Friday;


            myDiagram.AxisY.DateTimeScaleOptions.GridSpacing = 0.5;
            myDiagram.AxisY.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Week;
            // comentted by deepak due to error..
          //  myDiagram.AxisY.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
            myDiagram.AxisY.Label.TextPattern = "{V:m}";
            myDiagram.AxisY.DateTimeScaleOptions.WorkdaysOnly = true;
            myDiagram.AxisY.DateTimeScaleOptions.WorkdaysOptions.Workdays = Weekday.Monday | Weekday.Tuesday | Weekday.Wednesday | Weekday.Thursday | Weekday.Friday;

            xrSummaryGanttChart.HeightF = (float)80 + ((float)30 * datatable.Rows.Count);

           
            Palette palette = xrSummaryGanttChart.PaletteRepository["Pastel Kit"];
            Palette palette1 = xrSummaryGanttChart.PaletteRepository["Nature Colors"];
            List<PaletteEntry> paletteEntries = new List<PaletteEntry>();
            paletteEntries.Add(new PaletteEntry( palette[4].Color, palette[4].Color2));
            paletteEntries.Add(new PaletteEntry(palette1[6].Color, palette1[6].Color2));

            xrSummaryGanttChart.PaletteRepository.Add("CustomePalette", new Palette("CustomePalette", paletteEntries.ToArray()));
            xrSummaryGanttChart.PaletteName = "CustomePalette";
        }
    }
}
