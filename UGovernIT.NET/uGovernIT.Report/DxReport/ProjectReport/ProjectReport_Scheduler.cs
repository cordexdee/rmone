using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
namespace uGovernIT.Report.DxReport
{
    public class ProjectReport_Scheduler : IReportScheduler
    {
        ApplicationContext context;
        public ProjectReport_Scheduler()
        {

        }
        public ProjectReport_Scheduler(ApplicationContext applicationContext)
        {
            context = applicationContext;
        }
        public Dictionary<string, object> GetDefaultData()
        {
            Dictionary<string, object> ConfigProjectDefaults, ProjectDefaultProperties = null;
            List<string> lstOfMappingControl = new List<string>() { "chkStatus:ProjectStatus", "chkTrafficLight:TrafficLights", "chkProjectRoles:ProjectRoles", "chkProjectDescription:Description", "chkBudgetSummary:BugetSummary", "chkCalculate:ExpectedSpend",
                                                                              "chkPlannedvsActualByCategory:PlannedVsActualCategory","chkAccomplishments:Accomplishments","chkPlan:PlannedItems","chkRisks:Risks","chkIssues:Issues","chkDecisionLog:DecisionLog","chkKeyDeliverables:Deliverables",
                                                                            "chkKeyReceivables:Receivables","chkPlannedvsActualByBudgetItem:PlannedVsActualBudgetItem","chkPlannedvsActualByMonth:PlannedVsActualMonth","chkShowMilestone:keyTasks","chkAllMilestone:AllMileStones","chkShowAllTasks:AllTasks",
                                                                            "chkResourceAllocation:ResourceAllocation","chkMilestone:GanttChart"};
            string configValue = context.ConfigManager.GetValue(ConfigConstants.ProjectReportDefaults);
            if (!string.IsNullOrEmpty(configValue))
            {
                ConfigProjectDefaults = new Dictionary<string, object>();
                ProjectDefaultProperties = new Dictionary<string, object>();
                List<string> lstConfigValue = configValue.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                lstConfigValue.ForEach(x =>
                        {
                            string[] arr = x.Split(new string[] { Constants.Separator7 }, StringSplitOptions.RemoveEmptyEntries);
                            if (!ConfigProjectDefaults.ContainsKey(Convert.ToString(arr[0])) && arr.Length == 2) { ConfigProjectDefaults.Add(Convert.ToString(arr[0]), UGITUtility.StringToBoolean(arr[1]) == true ? true : false); }
                        });

                lstOfMappingControl.ForEach(x =>
                {
                    string[] arr = x.Split(new string[] { Constants.Separator7 }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 2)
                    {
                        string key = Convert.ToString(arr[0]);
                        string value = Convert.ToString(arr[1]);
                        if (!ProjectDefaultProperties.ContainsKey(key) && ConfigProjectDefaults.ContainsKey(value))
                            ProjectDefaultProperties.Add(key, ConfigProjectDefaults[value]);
                        else
                            ProjectDefaultProperties.Add(key, false);
                    }

                });
            }

            return ProjectDefaultProperties;
        }
        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
            string filePath  = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid(); //Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
            string fileName = string.Empty;
            TSKProjectReportEntity proEntity = new TSKProjectReportEntity();

            proEntity.ShowAccomplishment = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAccomplishment]);
            proEntity.ShowPlan = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlan]);
            proEntity.ShowIssues = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowIssues]);
            proEntity.ShowStatus = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowStatus]);
            proEntity.ShowSummaryGanttChart = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowSummaryGanttChart]);
            proEntity.ShowAllTask = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowAllTask]);
            proEntity.ShowMilestone = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMilestone]);
            proEntity.ShowDeliverable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowDeliverable]);
            proEntity.ShowReceivable = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowReceivable]);
            proEntity.CalculateExpected = Convert.ToBoolean(formobj[ReportScheduleConstant.CalculateExpected]);
            proEntity.ShowProjectDescription = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectDescription]);
            proEntity.ShowBudgetSummary = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowBudgetSummary]);
            proEntity.ShowPlannedvsActualByCategory = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByCategory]);
            proEntity.ShowPlannedvsActualByBudgetItem = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByBudgetItem]);
            proEntity.ShowPlannedvsActualByMonth = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowPlannedvsActualByMonth]);
            proEntity.ShowProjectRoles = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowProjectRoles]);
            proEntity.ShowResourceAllocation = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowResourceAllocation]);
            proEntity.ShowMonitorState = Convert.ToBoolean(formobj[ReportScheduleConstant.ShowMonitorState]);

            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);

            int[] PMMids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.ids]).Split(','), int.Parse);
            PMMProjectReportHelper prHelper = new PMMProjectReportHelper();
            proEntity = prHelper.GetProjectReportEntity(applicationContext,proEntity, PMMids);
            MultiProReport multiproject = new MultiProReport(proEntity);

            return ReportHelper.ExportFiles(multiproject, attachFormat, filePath, title);
        }
    }
}