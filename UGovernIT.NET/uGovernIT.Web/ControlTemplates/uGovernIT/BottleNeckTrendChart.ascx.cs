using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Utility;
using DevExpress.XtraCharts;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class BottleNeckTrendChart : UserControl
    {
        public string ModuleName { get; set; }
        public int StageStep { get; set; }
        public string StageTitle { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtcStartDate.Date = DateTime.Now.AddDays(-7);
                dtcEndDate.Date = DateTime.Now;
            }

            if (dtcStartDate.Date == null)
            {
                dtcStartDate.Date = DateTime.Now.AddDays(-7);
            }
            if (dtcEndDate.Date == null)
            {
                dtcEndDate.Date = DateTime.Now;
            }

            WebChartControl1.Series.Clear();

            Series spline = new Series("Spline", ViewType.Line);
            spline.ValueDataMembers.AddRange("StageCount");
            spline.ArgumentDataMember = "StageEndDate";
            WebChartControl1.Series.Add(spline);


            WebChartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            spline.ArgumentScaleType = ScaleType.DateTime;
            spline.ValueScaleType = ScaleType.Numerical;

            // Cast the chart's diagram to the XYDiagram type, to access its axes.
            XYDiagram diagram = WebChartControl1.Diagram as XYDiagram;

            if (!IsPostBack)
            {
                // Define the date-time measurement unit, to which the beginning of 
                // a diagram's gridlines and labels should be aligned. 
                diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
            }
            if (ddlDateType.SelectedItem.Text == "Week")
            {
                // diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Week;
            }
            else if (ddlDateType.SelectedItem.Text == "Month")
            {
                // diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Month;
            }
            else
            {
                // Define the detail level for date-time values.
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
            }

            // Enable the diagram's scrolling.
            diagram.EnableAxisXScrolling = true;
            // diagram.EnableAxisYScrolling = true;

            // Define the whole range for the X-axis. 
            diagram.AxisX.WholeRange.Auto = false;
            diagram.AxisX.WholeRange.SetMinMaxValues(dtcStartDate.Date, dtcEndDate.Date);

            diagram.AxisX.Label.TextPattern = "{V:MMM-dd-yyyy}";
            //diagram.AxisX.Label.Angle = 270;


            WebChartControl1.Series["Spline"].DataSource = CreateLineData();
            spline.CrosshairLabelPattern = "{A:MMM-dd-yyyy}: {V}";
        }

        DataTable CreateLineData()
        {
            DataTable dtStageTicketData = GetTicketData();
            string filterExpression = $"{DatabaseObjects.Columns.StageEndDate} is not null and {DatabaseObjects.Columns.StageStep} is not null and {DatabaseObjects.Columns.ModuleNameLookup} = '{ModuleName}'";
            DataTable dtStage = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleWorkflowHistory, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {filterExpression}");

            DataTable tempModuleWorkFlowHistorydata = new DataTable();

            if (dtStage != null && dtStage.Rows.Count > 0)
            {
                DataTable newdt = new DataTable();
                newdt = dtStage;
                newdt.Columns.Add("StartDate", typeof(DateTime));

                DataTable temptable = new DataTable();
                temptable = dtStage.Clone();

                DataRow temprowdt = null;
                foreach (DataRow row in newdt.Rows)
                {
                    if (Convert.ToInt32(row[DatabaseObjects.Columns.StageStep]) == 1)
                    {
                        if (dtStageTicketData != null && dtStageTicketData.Rows.Count > 0)
                        {
                            DataRow[] drTicketData = dtStageTicketData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, Convert.ToString(row[DatabaseObjects.Columns.TicketId])));
                            if (drTicketData != null && drTicketData.Length > 0)
                            {
                                row["StartDate"] = Convert.ToDateTime(drTicketData[0][DatabaseObjects.Columns.Created]);
                                temprowdt = row;
                            }
                            else
                                temprowdt = null;
                        }
                    }
                    else
                    {
                        if (temprowdt == null)
                        {
                            row["StartDate"] = row[DatabaseObjects.Columns.StageEndDate];
                            temprowdt = row;
                        }
                        else
                        {
                            if (Convert.ToInt32(row[DatabaseObjects.Columns.StageStep]) > Convert.ToInt32(temprowdt[DatabaseObjects.Columns.StageStep]) && Convert.ToString(row[DatabaseObjects.Columns.TicketId]) == Convert.ToString(temprowdt[DatabaseObjects.Columns.TicketId]))
                            {
                                row["StartDate"] = temprowdt[DatabaseObjects.Columns.StageEndDate];
                                temprowdt = row;
                            }
                            else
                            {
                                row["StartDate"] = row[DatabaseObjects.Columns.StageEndDate];
                                temprowdt = row;
                            }
                        }
                    }
                }

                if (temptable != null && temptable.Rows.Count > 0)
                {
                    temptable.Merge(newdt);
                }
                else
                {
                    temptable = newdt;
                }

                if (temptable != null && temptable.Rows.Count > 0)
                {
                    DataRow[] result = temptable.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.StageStep, StageStep));
                    if (result != null && result.Length > 0)
                        tempModuleWorkFlowHistorydata = result.CopyToDataTable();
                }
            }

            //final result scehma.
            DataTable tempnewdatatable = new DataTable();
            tempnewdatatable.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            tempnewdatatable.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(int));
            tempnewdatatable.Columns.Add(DatabaseObjects.Columns.StageEndDate, typeof(DateTime));
            tempnewdatatable.Columns.Add(DatabaseObjects.Columns.ModuleNameLookup, typeof(string));
            tempnewdatatable.Columns.Add("StartDate", typeof(DateTime));

            //new block..
            if (tempModuleWorkFlowHistorydata != null)
            {
                foreach (DataRow item in tempModuleWorkFlowHistorydata.Rows)
                {
                    DataRow drnew = tempnewdatatable.NewRow();
                    drnew[DatabaseObjects.Columns.TicketId] = item[DatabaseObjects.Columns.TicketId];
                    drnew[DatabaseObjects.Columns.StageEndDate] = item[DatabaseObjects.Columns.StageEndDate];
                    drnew[DatabaseObjects.Columns.StageStep] = item[DatabaseObjects.Columns.StageStep];
                    //drnew[DatabaseObjects.Columns.ModuleNameLookup] = item[DatabaseObjects.Columns.ModuleNameLookup];
                    drnew["StartDate"] = item["StartDate"];
                    tempnewdatatable.Rows.Add(drnew);
                }
            }

            DataTable tempticketdata = new DataTable();
            if (dtStageTicketData != null && dtStageTicketData.Rows.Count > 0)
            {
                DataRow[] drTicketData = dtStageTicketData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.StageStep, StageStep));
                if (drTicketData != null && drTicketData.Length > 0)
                    tempticketdata = drTicketData.CopyToDataTable();
            }

            if (tempticketdata != null && tempticketdata.Rows.Count > 0)
            {
                foreach (DataRow item in tempticketdata.Rows)
                {
                    DataRow drnew = tempnewdatatable.NewRow();
                    drnew[DatabaseObjects.Columns.TicketId] = item[DatabaseObjects.Columns.TicketId];
                    drnew[DatabaseObjects.Columns.StageEndDate] = DateTime.Now;
                    drnew[DatabaseObjects.Columns.StageStep] = item[DatabaseObjects.Columns.StageStep];
                    //drnew[DatabaseObjects.Columns.ModuleNameLookup] = item[DatabaseObjects.Columns.ModuleNameLookup];
                    drnew["StartDate"] = item[DatabaseObjects.Columns.CurrentStageStartDate];
                    tempnewdatatable.Rows.Add(drnew);
                }
            }


            if (tempnewdatatable != null && tempnewdatatable.Rows.Count > 0)
            {
                foreach (DataRow item in tempnewdatatable.Rows)
                {
                    item[DatabaseObjects.Columns.StageEndDate] = Convert.ToDateTime(item[DatabaseObjects.Columns.StageEndDate]).ToShortDateString();
                    item["StartDate"] = item["StartDate"] is DBNull ? DateTime.MinValue.ToShortDateString() : Convert.ToDateTime(item["StartDate"]).ToShortDateString();
                }
            }

            DataTable distinctTable = tempnewdatatable.DefaultView.ToTable(true);

            DataTable finalTable = new DataTable();
            finalTable.Columns.Add("StageEndDate", typeof(DateTime));
            finalTable.Columns.Add("StageCount", typeof(int));

            if (distinctTable != null && distinctTable.Rows.Count > 0)
            {
                for (DateTime date = dtcStartDate.Date; date <= dtcEndDate.Date; date = date.AddDays(1))
                {
                    DataRow[] drow = distinctTable.Select(string.Format("({2} <= '{0}' AND {1} >= '{0}')", Convert.ToDateTime(date.ToShortDateString()), DatabaseObjects.Columns.StageEndDate, "StartDate"));
                    if (drow != null && drow.Length > 0)
                    {
                        DataRow newfinalrow = finalTable.NewRow();
                        newfinalrow["StageEndDate"] = date;
                        newfinalrow["StageCount"] = drow.Length;

                        finalTable.Rows.Add(newfinalrow);
                    }
                }
            }
            return finalTable;
        }

        private DataTable GetTicketData()
        {
            DataRow moduleRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Modules, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleName} = '{this.ModuleName}'").Rows[0];  //.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ModuleName) == this.ModuleName);
            DataTable dtResult = new DataTable();
            string filterExpression = $"{DatabaseObjects.Columns.StageStep} is not null";
            dtResult = GetTableDataManager.GetTableData(Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {filterExpression}");

            return dtResult;
        }

        protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
 