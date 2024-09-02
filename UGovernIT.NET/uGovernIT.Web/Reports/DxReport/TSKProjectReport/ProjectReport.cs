
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using uGovernIT.Manager.Entities;
using System.Linq;
using DevExpress.XtraCharts;
using System.Collections.Generic;
using uGovernIT.Utility;
using System.Text.RegularExpressions;
using uGovernIT.Manager;
using System.Web;
using System.Drawing.Printing;
using uGovernIT.Manager.Managers;

namespace uGovernIT.DxReport
{
    public partial class ProjectReport : XtraReport
    {
      
        TSKProjectReportEntity projectEntity;
        FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
        private TopMarginBand topMarginBand3;
        private DetailBand detailBand3;
        private BottomMarginBand bottomMarginBand3;
        FieldConfiguration field = null;
        public ProjectReport(TSKProjectReportEntity projectReport)
        {
            InitializeComponent();
            projectEntity = projectReport;
            DataTable ganttViewTable = null;

            this.ExportOptions.Xls.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;
            this.ExportOptions.Xlsx.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Text;

           
            xrTCPrecentComplete.DataBindings.Add("Text", null, "PctComplete", "{0}%");
            xrTCProjectPhase.DataBindings.Add("Text", null, "Status");
            xrTCProjectScore.DataBindings.Add("Text", null, "ProjectScore", "{0:#}");
            xrLiveDateTCTicketId.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDesiredCompletionDate, uGITFormatConstants.DateFormat);
            xrPHTCTicketId.DataBindings.Add("Text", null, "TicketId");
            xrPBProjectScore.Visible = false;
            xrPictureBoxState.Visible = false;
            xrLabelProjectHealthScore.Visible = false;
            xrLabelProjectHealthScoreState.Visible = false;
            if (projectEntity.ShowTrafficlight)
            {
                xrPictureBoxState.Visible = true;
                xrPBProjectScore.Visible = true;
                xrPBProjectScore.DataBindings.Add("ImageUrl", null, "OverallProjectScoreColor");
                xrPictureBoxState.DataBindings.Add("ImageUrl", null, "OverallProjectScoreColor");
                 
            }
            else
            {
                xrLabelProjectHealthScore.Visible = true;
                xrLabelProjectHealthScoreState.Visible = true;
                xrLabelProjectHealthScore.DataBindings.Add("Text", null, "OverallProjectScoreColor");
                xrLabelProjectHealthScoreState.DataBindings.Add("Text", null, "OverallProjectScoreColor");
                xrLabelProjectHealthScore.BeforePrint += XrLabelProjectHealthScore_BeforePrint;
                xrLabelProjectHealthScoreState.BeforePrint += XrLabelProjectHealthScore_BeforePrint;
            }

            xrlabelgrouptitle.DataBindings.Add("Bookmark", null, "Title");
            xrlabelgrouptitleTC.DataBindings.Add("Text", null, "TicketId");
            xrPHTicketId.BeforePrint += XrPHTicketId_BeforePrint;
            //xrpbPHCompanyLogo.DataBindings.Add("ImageUrl", null, "");
            xrlabelgrouptitle.BeforePrint += Xrlabelgrouptitle_BeforePrint;
            if (projectEntity.CompanyLogoBitmap != null)
                xrpbPHCompanyLogo.Image = projectEntity.CompanyLogoBitmap;
            else
                xrpbPHCompanyLogo.ImageUrl = projectEntity.CompanyLogo;

            xrpbPHCompanyLogo.SizeF = GetDynamicSize(projectEntity.LogoHeight, projectEntity.LogoWidth, 60);
            xrpbPHCompanyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            xrTableState.Visible = false;
            xrTProjectPhase.Visible = true;
            Detail.HeightF = Detail.HeightF - xrTableState.HeightF;
            if (projectEntity.Projects.Columns.Contains(DatabaseObjects.Columns.ProjectState) && !string.IsNullOrEmpty(Convert.ToString(projectEntity.Projects.Rows[0][DatabaseObjects.Columns.ProjectState])))
            {
                xrTProjectPhase.Visible = false;
                xrTableState.Visible = true;
                xrTCPrecentCompleteState.DataBindings.Add("Text", null, "PctComplete", "{0:0.00%}");
                xrTCProjectPhaseState.DataBindings.Add("Text", null, "Status");
                xrTableScoreState.DataBindings.Add("Text", null, "ProjectScore", "{0:#}");
                xrLiveDateTCTicketIdState.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketDesiredCompletionDate, uGITFormatConstants.DateFormat);
                xrTCProjectScoreState.DataBindings.Add("Text", null, "ProjectState");
            }

            // xrpbCompanyLogo.DataBindings.Add("ImageUrl", null, "");
            if (projectEntity.CompanyLogoBitmap != null)
                xrpbCompanyLogo.Image = projectEntity.CompanyLogoBitmap;
            else
                xrpbCompanyLogo.ImageUrl = projectEntity.CompanyLogo;

            xrpbCompanyLogo.SizeF = GetDynamicSize(projectEntity.LogoHeight, projectEntity.LogoWidth, 100);
            xrpbCompanyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;

            ///Weekly Summary Current Reporting Period
            if (projectEntity.ShowStatus)
                BindExecutiveSummary(projectEntity.Projects);
            else
                drExecutiveSummary.Visible = false;

            ///Project Roles
            if (projectEntity.ShowProjectRoles)
                BindProjectRoles(projectEntity.Projects);
            else
                drProjectRoles.Visible = false;

            xrTCProjManager.BeforePrint += xrTCProjManager_BeforePrint;
            ///Project Description
            if (projectEntity.ShowProjectDescription)
                BindDescription(projectEntity.Projects);
            else
                drDescription.Visible = false;

            this.DataSource = projectEntity.Projects;

            //Monitor State Option
            if (projectEntity.ShowMonitorState)
                BindMonitorStateOption(projectEntity.MonitorState);
            else
                drMonitorState.Visible = false;

            //Project Budget Summary
            if (projectEntity.ShowBudgetSummary)
                BindProjectBudgetSummary(projectEntity.PlannedvsActualsbyCategory);
            else
                drBudgetSummary.Visible = false;

            ///Accomplishment
            if (projectEntity.ShowAccomplishment)
                BindAcommplishments(projectEntity.Accomplishment);
            else
                drAccomplishment.Visible = false;

            ///Plan
            if (projectEntity.ShowPlan)
                BindImmediatePlans(projectEntity.ImmediatePlans);
            else
                drPlan.Visible = false;

            ///Risk
            if (projectEntity.ShowRisk)
                BindRisk(projectEntity.Risks);
            else
                drRisk.Visible = false;

            ///Issues
            if (projectEntity.ShowIssues)
                BindIssues(projectEntity.Issues);
            else
                drIssues.Visible = false;

            ///Key Receivable Tasks
            if (projectEntity.ShowReceivable)
                BuildReceivableTasks(projectEntity.Tasks);
            else
                drKeyReceivable.Visible = false;


            ///Key Deliverable Tasks
            if (projectEntity.ShowDeliverable)
                BuildDeliverableTasks(projectEntity.Tasks);
            else
                drKeyDeliverable.Visible = false;

            ///All Tasks
            if(projectEntity.ShowAllTask && projectEntity.ShowOpenTaskOnly)
                BindAllTasks(projectEntity.OpenTasks, projectEntity.ShowOpenTaskOnly);
            else if (projectEntity.ShowAllTask)
                BindAllTasks(projectEntity.Tasks, false);
            else
                drAllTasks.Visible = false;


            ///Task Summary 
            if (projectEntity.SummaryTasks != null)
                ganttViewTable = projectEntity.SummaryTasks.Copy();

            if (projectEntity.ShowMilestone)
                BindSummaryTasks(projectEntity.SummaryTasks);
            else
                drSummaryTasks.Visible = false;

            ///Resource Allocation Chart
            if (projectEntity.ShowResourceAllocation)
            {
                GenerateResourceChart(projectEntity.BudgetAllocation);
                ///Resource Allocation Grid
                BuildResourceAllocationSubReport(projectEntity.BudgetAllocation);
            }
            else
                drResourceAllocation.Visible = false;

            ///Project Gantt Chart
            if (projectEntity.ShowSummaryGanttChart)
                GenerateGanttChart(ganttViewTable);
            else
                dSummaryGanttView.Visible = false;

            ///Planned vs Actual by Budget Item
            if (projectEntity.ShowPlannedvsActualByBudgetItem)
                BindPlannedvsActualsbyBudgetItem(projectEntity.PlannedvsActualsbyCategory);
            else
                drPlannedvsActualsByBudgetItem.Visible = false;

            ///Planned vs Actual by Category
            if (projectEntity.ShowPlannedvsActualByCategory)
                BindPlannedvsActualsbyCategory(projectEntity.PlannedvsActualsbyCategory);
            else
                drPlannedvsActualsByCat.Visible = false;

            ///Planned vs Actual by Month
            if (projectEntity.ShowPlannedvsActualByMonth)
            {
                DataTable dtPlanned = new DataTable();
                DataTable dtActuals = new DataTable();
                if (projectEntity.PlannedMonthlyBudget != null && projectEntity.PlannedMonthlyBudget.Tables.Count > 0)
                {
                    dtPlanned = projectEntity.PlannedMonthlyBudget.Tables[0];
                }
                if (projectEntity.ActualMonthlyBudget != null && projectEntity.ActualMonthlyBudget.Tables.Count > 0)
                {
                    dtActuals = projectEntity.ActualMonthlyBudget.Tables[0];
                }
                BindPlannedvsActualbyMonthly(dtPlanned, dtActuals);
            }
            else
                drPlannedvsActualByMonth.Visible = false;

            //if (projectEntity.ShowDecisionLog)
            //    BindDecisionLog(projectEntity.DecisionLog);

        }

        private void xrTCProjManager_BeforePrint(object sender, PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            string title = Convert.ToString(this.GetCurrentColumnValue(DatabaseObjects.Columns.TicketProjectManager));// GetLookupValues(DatabaseObjects.Columns.TicketProjectManager, Convert.ToString(this.GetCurrentColumnValue(DatabaseObjects.Columns.TicketProjectManager)));
            label.Text = title;
        }

        private void XrLabelProjectHealthScore_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            label.Borders = DevExpress.XtraPrinting.BorderSide.None;
            label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;


            if (label.Text.EndsWith("LED_Green.png"))
            {
                label.Text = "Green";
            }
            else if (label.Text.EndsWith("LED_Yellow.png"))
            {
                label.Text = "Yellow";
            }
            else if (label.Text.EndsWith("LED_Red.png"))
            {
                label.Text = "Red";
            }
        }

        private void Xrlabelgrouptitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            string title = Convert.ToString(this.GetCurrentColumnValue(DatabaseObjects.Columns.Title));
            label.Text = string.Format("{0}", title);
        }

        private void XrPHTicketId_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            string title = Convert.ToString(this.GetCurrentColumnValue(DatabaseObjects.Columns.Title));
            label.Text = string.Format("{0}", title);
        }

        private void BindProjectRoles(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                drProjectRoles.Visible = false;
                return;
            }

            xrTCProjManager.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketProjectManager);
            xrTCProjSponcer.DataBindings.Add("Text", null, DatabaseObjects.Columns.TicketSponsors);
            xrTCProjLOB.DataBindings.Add("Text", null, "BeneficiariesLookup");

            drProjectRoles.DataSource = dt;
        }

        private string RemoveLookupIDs(string lookupData)
        {
            if (string.IsNullOrEmpty(lookupData))
                return string.Empty;

            string value = lookupData.Trim();
            string Separator = ";#";
            if (value.Contains(Separator))
            {
                // Get value and strip IDs if lookup field
                string[] components = value.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                if (components.Length > 1)
                {
                    value = string.Empty;
                    foreach (string component in components)
                    {
                        int id = 0;
                        if (!int.TryParse(component, out id))
                        {
                            if (value != string.Empty)
                                value += "; ";
                            value += component;
                        }
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// Project budget summary Details report
        /// </summary>
        /// <param name="dt">Datasource as datatable for details report.</param>
        private void BindProjectBudgetSummary(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                drBudgetSummary.Visible = false;
                return;
            }
            DataTable dataTable = dt.Copy();
            foreach (DataRow dr in dataTable.Rows)
            {
                dr["Planned"] = Convert.ToDouble(dr["Planned"]) / 1000;
                dr["BaseLine"] = Convert.ToDouble(dr["BaseLine"]) / 1000;
                dr["Actual"] = Convert.ToDouble(dr["Actual"]) / 1000;
            }
            xrTCRevisedBudget.Summary.Running = SummaryRunning.Report;
            xrTCRevisedBudget.Summary.Func = SummaryFunc.Sum;
            xrTCRevisedBudget.Summary.FormatString = "{0:$0.00k}";
            xrTCRevisedBudget.DataBindings.Add("Text", null, "Planned");

            xrTCOriginalBudget.Summary.Running = SummaryRunning.Report;
            xrTCOriginalBudget.Summary.Func = SummaryFunc.Sum;
            xrTCOriginalBudget.Summary.FormatString = "{0:$0.00k}";
            xrTCOriginalBudget.DataBindings.Add("Text", null, "BaseLine");

            xrTCSpendTillDate.Summary.Running = SummaryRunning.Report;
            xrTCSpendTillDate.Summary.Func = SummaryFunc.Sum;
            xrTCSpendTillDate.Summary.FormatString = "{0:$0.00k}";
            xrTCSpendTillDate.DataBindings.Add("Text", null, "Actual");

            xrTCExpectedCompletionSpend.BeforePrint += xrTCExpectedCompletionSpend_BeforePrint;
            xrTCProjectedVariance.BeforePrint += xrTCProjectedVariance_BeforePrint;

            ///Estimate Project spent Comment and Estimate Project spent Value fields get from PMMProjects List
            string budgetComments = string.Empty;
            var obj = (from p in projectEntity.Projects.AsEnumerable()
                       select new
                       {
                           Comment = p.Field<string>(DatabaseObjects.Columns.EstProjectSpendComment),
                           Spend = UGITUtility.StringToDouble(p.Field<string>(DatabaseObjects.Columns.EstProjectSpend))
                       })
                        .FirstOrDefault();
            budgetComments = obj != null ? obj.Comment : string.Empty;

            double? EstProjectspend = 0;
            EstProjectspend = obj != null ? (obj.Spend == 0 ? 0 : obj.Spend) : 0;

            xrTCExpectedCompletionSpend.Text = string.Format("{0:$0.00k}", EstProjectspend / 1000);
            xrTCComments.Text = budgetComments;

            drBudgetSummary.DataSource = dataTable;
        }

        void xrTCProjectedVariance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            double Expected = 0.0;
            if (projectEntity.CalculateExpected)
            {
                double actual = Convert.ToDouble(xrTCSpendTillDate.Summary.GetResult());
                double precentComplete = Convert.ToDouble(projectEntity.Projects.Rows[0]["PctComplete"]);
                if (precentComplete > 0 && actual > 0)
                {
                    Expected = (actual / precentComplete);
                }
                else
                {
                    double planned = Convert.ToDouble(xrTCRevisedBudget.Summary.GetResult());
                    Expected = planned;
                }
            }
            else
            {
                double planned = Convert.ToDouble(xrTCRevisedBudget.Summary.GetResult());
                Expected = planned;
            }

            if (Convert.ToDouble(Regex.Match(xrTCExpectedCompletionSpend.Text, @"\d+.\d+").Value) > 0)
            {
                Expected = Convert.ToDouble(Regex.Match(xrTCExpectedCompletionSpend.Text, @"\d+.\d+").Value);
            }

            double projectedVariance = Convert.ToDouble(xrTCRevisedBudget.Summary.GetResult()) - Expected;
            if (projectedVariance != 0)
            {
                xrTCProjectedVariance.Text = string.Format("{0:$0.00k}", projectedVariance);
                if (projectedVariance > 0)
                {
                    xrTCProjectedVariance.ForeColor = Color.Green;
                }
                else
                {
                    xrTCProjectedVariance.ForeColor = Color.Red;
                }
            }
            else
            {
                xrTCProjectedVariance.Text = "$0.00k";
            }
        }

        void xrTCExpectedCompletionSpend_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (projectEntity.CalculateExpected)
            {
                double totalActual = Convert.ToDouble(projectEntity.PlannedvsActualsbyCategory.Compute("Sum(Actual)", string.Empty)) / 1000;
                double precentComplete = Convert.ToDouble(projectEntity.Projects.Rows[0]["PctComplete"]);
                if (precentComplete > 0 && totalActual > 0)
                {
                    double Expected = (totalActual / precentComplete);
                    xrTCExpectedCompletionSpend.Text = string.Format("{0:$0.00k}", Expected);
                }
                else
                {
                    double totalPlanned = Convert.ToDouble(projectEntity.PlannedvsActualsbyCategory.Compute("Sum(Planned)", string.Empty)) / 1000;
                    xrTCExpectedCompletionSpend.Text = string.Format("{0:$0.00k}", totalPlanned);
                }
            }
            else
            {
                if (Convert.ToDouble(Regex.Match(xrTCExpectedCompletionSpend.Text, @"\d+.\d+").Value) <= 0)
                {
                    double totalPlanned = Convert.ToDouble(projectEntity.PlannedvsActualsbyCategory.Compute("Sum(Planned)", string.Empty)) / 1000;
                    xrTCExpectedCompletionSpend.Text = string.Format("{0:$0.00k}", totalPlanned);
                }
            }
        }

        private void BindDescription(DataTable dataTable)
        {
            xrLblDescription.DataBindings.Add("Text", null, "Description");
            drDescription.DataSource = dataTable;
        }

        private void BindExecutiveSummary(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                string status = Convert.ToString(dataTable.Rows[0]["ProjectSummaryNote"]);
                if (string.IsNullOrEmpty(status))
                {
                    drExecutiveSummary.Visible = false;
                    return;
                }
            }

            xrLblExecutiveSummary.BeforePrint += XrLblExecutiveSummary_BeforePrint;
            xrLblExecutiveSummary.DataBindings.Add("Text", null, "ProjectSummaryNote");
            drExecutiveSummary.DataSource = dataTable;
        }

        private void XrLblExecutiveSummary_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRRichText richtext = sender as XRRichText;
            richtext.Text = UGITUtility.StripHTML(richtext.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="precent">precent can be only in 10 multiples</param>
        /// <returns></returns>
        private System.Drawing.SizeF GetDynamicSize(float height, float width, float precent)
        {

            float resizeWidth = xrpbCompanyLogo.WidthF;
            float resizeHeight = xrpbCompanyLogo.HeightF;

            float aspect = width / height;

            if (resizeWidth < width)
            {
                width = resizeWidth;
                height = width / aspect;
            }
            if (resizeHeight < height)
            {
                aspect = width / height;
                height = resizeHeight;
                width = height * aspect;
            }
            if (precent > 0)
            {
                width = width * precent / 100;
                height = height * precent / 100;
            }

            System.Drawing.SizeF size = new SizeF(width, height);

            return size;
        }

        private void BuildDeliverableTasks(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                drKeyDeliverable.Visible = false;
                return;
            }

            DataTable dtdeliverable = new DataTable();
            DataRow[] dr = dataTable.Select("Behaviour='Deliverable'");
            if (dr.Length > 0)
            {
                dtdeliverable = dr.CopyToDataTable();

                KeyDeliverables keyDeliverable = new KeyDeliverables(dtdeliverable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = keyDeliverable;
                dKeyDeliverable.Controls.Add(subreport);
            }
            else
            {
                drKeyDeliverable.Visible = false;
            }
        }

        private void BuildReceivableTasks(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                drKeyReceivable.Visible = false;
                return;
            }
            DataTable dtreceivable = new DataTable();
            DataRow[] dr = dataTable.Select("Behaviour='Receivable'");
            if (dr != null && dr.Length > 0)
            {
                dtreceivable = dr.CopyToDataTable();
                KeyReceivablesReport keyReceivable = new KeyReceivablesReport(dtreceivable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = keyReceivable;
                dKeyReceivable.Controls.Add(subreport);
            }
            else
            {
                drKeyReceivable.Visible = false;
            }
        }

        private void BuildResourceAllocationSubReport(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                drResourceAllocation.Visible = false;
                return;
            }
            var query = dataTable.AsEnumerable()
                        .GroupBy(row => row.Field<String>("Category"))
                        .Select(g => new ResourceAllocation
                        {
                            Legend = g.Key,
                            Jan = g.Where(row => row.Field<DateTime>("Month").Month == 1).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Feb = g.Where(row => row.Field<DateTime>("Month").Month == 2).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Mar = g.Where(row => row.Field<DateTime>("Month").Month == 3).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Apr = g.Where(row => row.Field<DateTime>("Month").Month == 4).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            May = g.Where(row => row.Field<DateTime>("Month").Month == 5).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Jun = g.Where(row => row.Field<DateTime>("Month").Month == 6).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Jul = g.Where(row => row.Field<DateTime>("Month").Month == 7).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Aug = g.Where(row => row.Field<DateTime>("Month").Month == 8).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Sep = g.Where(row => row.Field<DateTime>("Month").Month == 9).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Oct = g.Where(row => row.Field<DateTime>("Month").Month == 10).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Nov = g.Where(row => row.Field<DateTime>("Month").Month == 11).Select(c => c.Field<double>("CategorySum")).FirstOrDefault(),
                            Dec = g.Where(row => row.Field<DateTime>("Month").Month == 12).Select(c => c.Field<double>("CategorySum")).FirstOrDefault()
                        }).ToList();

            //ResourceAllocSubReport subReport = new ResourceAllocSubReport(query);
            //xrSubReportResourceAlloc.ReportSource = subReport;
        }

        private void GenerateGanttChart(DataTable datatable)
        {
            if (datatable == null || datatable.Rows.Count == 0)
            {
                dSummaryGanttView.Visible = false;
                return;
            }
            DataRow[] dtRows = datatable.Select(string.Format("{0}=0", DatabaseObjects.Columns.UGITLevel));
            if (dtRows.Length == 0)
            {
                dSummaryGanttView.Visible = false;
                return;
            }

            DataTable dt = dtRows.CopyToDataTable();
            SumGanttChartSubReport summaryGanttChartReport = new SumGanttChartSubReport(dt);
            xrSubReportGantt.ReportSource = summaryGanttChartReport;
        }

        private void GenerateResourceChart(DataTable datatable)
        {
            if (datatable == null || datatable.Rows.Count == 0)
            {
                drResourceAllocation.Visible = false;
                return;
            }
            xrResourceChart.DataSource = datatable;
            xrResourceChart.SeriesDataMember = "Category";
            xrResourceChart.SeriesTemplate.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            xrResourceChart.SeriesTemplate.ArgumentScaleType = ScaleType.DateTime;
            xrResourceChart.SeriesTemplate.ValueScaleType = ScaleType.Numerical;
            xrResourceChart.SeriesTemplate.ValueDataMembers.AddRange("CategorySum");
            xrResourceChart.SeriesTemplate.ArgumentDataMember = "Month";
            xrResourceChart.PaletteName = "Office";
            xrResourceChart.Series["Series 1"].Visible = false;
        }

        private void BindSummaryTasks(DataTable datatable)
        {
            if (datatable == null || datatable.Rows.Count == 0)
            {
                drSummaryTasks.Visible = false;
                return;
            }
            datatable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
            KeyMileStone keymileStone = new KeyMileStone(datatable);
            XRSubreport subreport = new XRSubreport();
            subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
            subreport.ReportSource = keymileStone;
            dSummaryTasks.Controls.Add(subreport);

        }

        private void BindAllTasks(DataTable datatable, bool onlyOpenTasks)
        {
            if (datatable == null || datatable.Rows.Count == 0)
            {
                drAllTasks.Visible = false;
                return;
            }
            ProjectTasks projecttasks = new ProjectTasks(datatable, onlyOpenTasks);
            XRSubreport subreport = new XRSubreport();
            subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
            subreport.ReportSource = projecttasks;
            dTasks.Controls.Add(subreport);
        }

        private void BindIssues(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
                IssuesReport Issues = new IssuesReport(dataTable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = Issues;
                dIssues.Controls.Add(subreport);
            }
            else
            {
                drIssues.Visible = false;
            }
        }

        private void BindImmediatePlans(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
                PlanReport immediateplan = new PlanReport(dataTable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = immediateplan;
                dPlan.Controls.Add(subreport);
            }
            else
            {
                drPlan.Visible = false;
            }
        }

        private void UpdateNoteContent(DataRow row)
        {
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.ProjectNote))
                row[DatabaseObjects.Columns.ProjectNote] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.ProjectNote]));
            
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.MitigationPlan))
                row[DatabaseObjects.Columns.MitigationPlan] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.MitigationPlan]));
            
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.ContingencyPlan))
                row[DatabaseObjects.Columns.ContingencyPlan] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.ContingencyPlan]));
            
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.Body))
                row[DatabaseObjects.Columns.Body] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.Body]));
            
            if (row.Table.Columns.Contains(DatabaseObjects.Columns.UGITResolution))
                row[DatabaseObjects.Columns.UGITResolution] = UGITUtility.StripHTML(Convert.ToString(row[DatabaseObjects.Columns.UGITResolution]));
        }

        private void BindRisk(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));

                RiskReport risk = new RiskReport(dataTable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = risk;
                dRisk.Controls.Add(subreport);
            }
            else
            {
                drRisk.Visible = false;
            }
        }

        private void BindAcommplishments(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataTable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
                AccompolishmentReport accompolishment = new AccompolishmentReport(dataTable);
                XRSubreport subreport = new XRSubreport();
                subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
                subreport.ReportSource = accompolishment;
                dAccompolishment.Controls.Add(subreport);
            }
            else
            {
                drAccomplishment.Visible = false;
            }
        }

        private void BindPlannedvsActualbyMonthly(DataTable dtPlan, DataTable dtActual)
        {
            if (dtPlan == null || dtPlan.Rows.Count == 0)
            {
                drPlannedvsActualByMonth.Visible = false;
                return;
            }
            Dictionary<int, string> columnList = new Dictionary<int, string>();

            for (int i = 0; i < dtPlan.Columns.Count; i++)
            {
                if (i > 0 && i < dtPlan.Columns.Count - 1)
                {
                    columnList.Add(i, dtPlan.Columns[i].ColumnName);
                }
            }
            double numberofMonthReport = Math.Ceiling(columnList.Count / 12.0);
            for (int i = 0; i < numberofMonthReport; i++)
            {
                int startindex = i * 12;
                int endindex = startindex + 12;
                List<string> columns = new List<string>();
                columns.Add(dtPlan.Columns[0].ColumnName);
                columns.AddRange(columnList.Where(c => c.Key > startindex && c.Key <= endindex).Select(x => x.Value).ToArray());

                DataTable plan = dtPlan.DefaultView.ToTable(false, columns.ToArray());
                List<string> cols = new List<string>();
                cols.AddRange(columnList.Where(c => c.Key > startindex && c.Key <= endindex).Select(x => x.Value).ToArray());
                string express = "[" + string.Join("]+[", cols.ToArray()) + "]";

                plan.Columns.Add(new DataColumn("Total", typeof(double), express));

                DataTable actual = dtActual.DefaultView.ToTable(false, columns.ToArray());
                actual.Columns.Add(new DataColumn("Total", typeof(double), express));

                XRSubreport subreport = new XRSubreport();
              //  PlannedvsActualMonthReport monthReport = new PlannedvsActualMonthReport(plan, actual);
              //  subreport.ReportSource = monthReport;
                DetailReportBand drmonthlyBand = new DetailReportBand();
                DetailBand dmonthBand = new DetailBand();
                dmonthBand.KeepTogetherWithDetailReports = true;
                dmonthBand.HeightF = 50F;
                subreport.LocationF = new PointF(0F, 0F);
                subreport.HeightF = 50F;
                dmonthBand.Controls.Add(subreport);
                drmonthlyBand.Bands.Add(dmonthBand);

                drPlannedvsActualByMonth.Bands.Add(drmonthlyBand);
            }
        }

        private void BindPlannedvsActualsbyBudgetItem(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                drPlannedvsActualsByBudgetItem.Visible = false;
                return;
            }
          //  PlannedvsActualCategoryReport plannedvsactualcategory = new PlannedvsActualCategoryReport(dt.Copy(), true);
           // xrSubreportPlannedvsActualByBudgetItem.ReportSource = plannedvsactualcategory;
        }

        private void BindPlannedvsActualsbyCategory(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                drPlannedvsActualsByCat.Visible = false;
                return;
            }
           // PlannedvsActualCategoryReport plannedvsactualcategory = new PlannedvsActualCategoryReport(dt.Copy(), false);
           // xrSubreportPlannedvsActualByCat.ReportSource = plannedvsactualcategory;

        }

        private void BindMonitorStateOption(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                drMonitorState.Visible = false;
                return;
            }
           
            xrPBMonitorIcon.Visible = false;
            xrLabelMonitorState.Visible = false;
            if (projectEntity.ShowTrafficlight)
            {
                xrPBMonitorIcon.Visible = true;
                xrPBMonitorIcon.DataBindings.Add("ImageUrl", dt, "ModuleMonitorOptionLEDClassLookup");

            }
            else
            {
                xrLabelMonitorState.Visible = true;
                xrLabelMonitorState.DataBindings.Add("Text", null, "ModuleMonitorOptionLEDClassLookup");
                xrLabelMonitorState.BeforePrint += XrLabelMonitorState_BeforePrint;
                //xrTCMonitorIcon.BeforePrint += XrTCMonitorIcon_BeforePrint;
            }


            xrTCMonitorHealth.DataBindings.Add("Text", null, "ModuleMonitorName");
            xrTCMonitorStatus.DataBindings.Add("Text", null, "ModuleMonitorOptionName");
            xrTCMonitorNotes.DataBindings.Add("Text", null, "ProjectMonitorNotes");

            drMonitorState.DataSource = dt;
            
        }

        private void XrLabelMonitorState_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel label = sender as XRLabel;
            label.Borders = DevExpress.XtraPrinting.BorderSide.None;
            label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            label.SizeF = new SizeF(200, 20);
            if (label.Text.EndsWith("LED_Green.png"))
            {
                label.Text = "Green: ";
            }
            else if (label.Text.EndsWith("LED_Yellow.png"))
            {
                label.Text = "Yellow: ";
            }
            else if (label.Text.EndsWith("LED_Red.png"))
            {
                label.Text = "Red: ";
            }
            //throw new NotImplementedException();
        }

        //private string GetImage(string className)
        //{
        //    string fileName = string.Empty;
        //    switch (className)
        //    {
        //        case "GreenLED":
        //            fileName = @"/_layouts/15/images/uGovernIT/LED_Green.png";
        //            break;
        //        case "YellowLED":
        //            fileName = @"/_layouts/15/images/uGovernIT/LED_Yellow.png";
        //            break;
        //        case "RedLED":
        //            fileName = @"/_layouts/15/images/uGovernIT/LED_Red.png";
        //            break;
        //        default:
        //            break;
        //    }

        //    return fileName;
        //}

        private void xrLblTicketId_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrTCTasksTitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void BindDecisionLog(DataTable dataTable)
        {
            //if (dataTable!=null && dataTable.Rows.Count > 0)
            //{
            //    dataTable.AsEnumerable().ToList().ForEach(x => UpdateNoteContent(x));
            //    DecisionLogReport Issues = new DecisionLogReport(dataTable);
            //    XRSubreport subreport = new XRSubreport();
            //    subreport.WidthF = this.PageWidth - (this.Margins.Left + this.Margins.Right);
            //    subreport.ReportSource = Issues;
            //    dDecisionLog.Controls.Add(subreport);
            //}
            //else
            //{
            //    drDecisionLog.Visible = false;
            //}
        }

        private string GetLookupValues(string FieldName, string Ids)
        {
            string value = string.Empty;
            field = fieldManager.GetFieldByFieldName(FieldName, "");
            if (field != null)
            {
                if (field.Datatype == "Lookup")
                {
                    value = fieldManager.GetFieldConfigurationData(field.FieldName, Convert.ToString(Ids));
                }
                if (field.Datatype == "UserField")
                {
                    List<uGovernIT.Utility.Entities.UserProfile> userProfiles = HttpContext.Current.GetUserManager().GetUserInfosById(Convert.ToString(Ids));
                    if (userProfiles != null && userProfiles.Count > 0)
                    {
                        value = string.Join(Constants.Separator6, userProfiles.Select(x => x.Name));
                    }
                }
            }
            return value;
        }

        private void IInitializeComponent()
        {
            this.topMarginBand3 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand3 = new DevExpress.XtraReports.UI.DetailBand();
            this.bottomMarginBand3 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand3
            // 
            this.topMarginBand3.Name = "topMarginBand3";
            // 
            // detailBand3
            // 
            this.detailBand3.Name = "detailBand3";
            // 
            // bottomMarginBand3
            // 
            this.bottomMarginBand3.Name = "bottomMarginBand3";
            // 
            // ProjectReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand3,
            this.detailBand3,
            this.bottomMarginBand3});
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
