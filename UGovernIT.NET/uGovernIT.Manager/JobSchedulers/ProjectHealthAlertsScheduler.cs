using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;
using uGovernIT.Utility;

namespace uGovernIT.Manager.JobSchedulers
{
    public class ProjectHealthAlertsScheduler : IJobScheduler
    {
        public string Duration { get; set; }

        public async Task Execute(string TenantID)
        {
            await Task.FromResult(0);
            ApplicationContext context = ApplicationContext.CreateContext(TenantID);
            ExecuteTask(context);
        }
        public void ExecuteTask(ApplicationContext context)
        {
            UGITTaskManager taskmgr = new UGITTaskManager(context);
            DataRow[] pmmProjectColl = GetTableDataManager.GetTableData(DatabaseObjects.Tables.PMMProjects, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}' AND {DatabaseObjects.Columns.TicketClosed}<>1").Select();
            if (pmmProjectColl != null && pmmProjectColl.Count() > 0)
            {
                ModuleMonitorManager mmm = new ModuleMonitorManager(context);
                List<ModuleMonitor> lstmonitor = mmm.Load(x => x.MonitorName == "On Time");
                if (lstmonitor == null && lstmonitor.Count== 0)
                    return;
                string prms = string.Format("{0} ='{1}' and {2} =1 and {3}={4}", DatabaseObjects.Columns.TenantID, context.TenantID, DatabaseObjects.Columns.AutoCalculate, DatabaseObjects.Columns.ModuleMonitorNameLookup, lstmonitor[0].ID);
                DataTable projectMonitorState = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ProjectMonitorState, prms);
                bool ProjectHealthAlertsEnable = context.ConfigManager.GetValueAsBool(ConfigConstants.ProjectHealthAlertsEnable);
                DataRow[] pmmTaskColl = null;
                if (ProjectHealthAlertsEnable)
                {
                    string prmsh = string.Format("{0} <>'Completed' and {2} =1 and {3}= " + DateTime.Now.ToString() + "", DatabaseObjects.Columns.Status, context.TenantID, DatabaseObjects.Columns.DueDate, DatabaseObjects.Columns.ModuleMonitorNameLookup);
                    pmmTaskColl = taskmgr.GetDataTable(string.Format("{0}= '{1}'", DatabaseObjects.Columns.ModuleNameLookup, ModuleNames.PMM)).Select();

                }
                foreach (DataRow project in pmmProjectColl)
                {
                    taskmgr.AutoCalculateProjectMonitorStateOnTime(project, context);

                    if (!ProjectHealthAlertsEnable)
                        continue;

                    string pmmTicketId = Convert.ToString(project[DatabaseObjects.Columns.TicketId]);
                    List<TaskProperties> lstSetValues = new List<TaskProperties>();
                    StringBuilder strHtml = null;

                    decimal actualCost = Convert.ToDecimal(Convert.IsDBNull(project[DatabaseObjects.Columns.ProjectCost]));
                    decimal budgetCost = Convert.ToDecimal(Convert.IsDBNull(project[DatabaseObjects.Columns.TicketTotalCost]));

                    if (actualCost > budgetCost)
                    {
                        if (strHtml == null)
                        {
                            strHtml = new StringBuilder();
                            strHtml.Append("This project has the following project health alerts:<br><br>");
                            strHtml.Append("<table style='border:1px solid;border-collapse: collapse;'>");
                        }

                        decimal tenPercent = budgetCost * 10 / 100;
                        decimal twentyPercent = budgetCost * 20 / 100;
                        decimal differenceBudgetHealth = actualCost - budgetCost;

                        if (differenceBudgetHealth > tenPercent && differenceBudgetHealth <= twentyPercent)
                        {
                            strHtml.AppendFormat("<tr><td style='background: none repeat scroll 0 0 #E8F5F8;width:140px; text-align: right;border:1px solid;padding:4px;font-weight: bold;'>Budget Variance</td><td style='border:1px solid;background: rgb(251, 251, 251) none repeat scroll 0px 0px;text-align: center;'><table style='width:20px;height:20px;padding:14px;margin:8px;background-color:yellow;'><tr><td></td></tr></table></td><td style='border:1px solid;padding:4px;background: rgb(251, 251, 251) none repeat scroll 0px 0px;'>Between 10-20%</td></tr>");
                        }
                        else if (differenceBudgetHealth > twentyPercent)
                        {
                            strHtml.AppendFormat("<tr><td style='background: none repeat scroll 0 0 #E8F5F8;width:140px;text-align: right;border:1px solid;padding:4px;font-weight: bold;'>Budget Variance</td><td style='border:1px solid;background: rgb(251, 251, 251) none repeat scroll 0px 0px;text-align: center;'><table style='width:20px;height:20px;padding:14px;margin:8px;background-color:red;'><tr><td></td></tr></table></td><td style='border:1px solid;padding:4px;background: rgb(251, 251, 251) none repeat scroll 0px 0px;'>Over 20%</td></tr>");
                        }
                    }

                    if (pmmTaskColl != null)
                    {
                        var dueDateOverTask = pmmTaskColl.Cast<DataRow>().Where(i => Convert.ToString(i[DatabaseObjects.Columns.TicketId]) == pmmTicketId && Convert.ToDateTime(i[DatabaseObjects.Columns.DueDate]).Date < DateTime.Now.Date);
                        var onDueDateTask = pmmTaskColl.Cast<DataRow>().Where(i => Convert.ToString(i[DatabaseObjects.Columns.TicketId]) == pmmTicketId && Convert.ToDateTime(i[DatabaseObjects.Columns.DueDate]).Date == DateTime.Now.Date);

                        if (dueDateOverTask.Count() > 0)
                        {
                            if (strHtml == null)
                            {
                                strHtml = new StringBuilder();
                                strHtml.Append("This project has the following project health alerts:<br><br>");
                                strHtml.Append("<table style='border:1px solid;border-collapse: collapse;'>");
                            }

                            lstSetValues = (from t in dueDateOverTask.AsEnumerable()
                                            select new TaskProperties() { Title = Convert.ToString(t["Title"]), Id = Convert.ToString(t["ID"]) }).ToList();

                            strHtml.AppendFormat("<tr><td style='background: none repeat scroll 0 0 #E8F5F8;width:140px;text-align: right;border:1px solid;padding:4px;font-weight: bold;'>Over Due Task(s)</td><td style='border:1px solid;background: rgb(251, 251, 251) none repeat scroll 0px 0px;text-align: center;'><table style='width:20px;height:20px;padding:14px;margin:8px;background-color:red;'><tr><td></td></tr></table></td><td style='border:1px solid;padding:4px;background: rgb(251, 251, 251) none repeat scroll 0px 0px;'>{0}</td></tr>", lstSetValues.Count > 5 ? string.Format("{0} Over-Due Task(s) including: {1}, ..", lstSetValues.Count, string.Join(", ", lstSetValues.Select(x => x.Title).Take(5))) : string.Format("{0} Over-Due Task(s): {1}", lstSetValues.Count, string.Join(", ", lstSetValues.Select(x => x.Title))));
                        }
                        if (onDueDateTask.Count() > 0)
                        {
                            if (strHtml == null)
                            {
                                strHtml = new StringBuilder();
                                strHtml.Append("This project has the following project health alerts:<br><br>");
                                strHtml.Append("<table style='border:1px solid;border-collapse: collapse;'>");
                            }

                            lstSetValues.Clear();
                            lstSetValues = (from t in onDueDateTask.AsEnumerable()
                                            select new TaskProperties() { Title = Convert.ToString(t["Title"]), Id = Convert.ToString(t["ID"]) }).ToList();

                            strHtml.AppendFormat("<tr><td style='background: none repeat scroll 0 0 #E8F5F8;width:140px;text-align: right;border:1px solid;padding:4px;font-weight: bold;'>Today Due Task(s)</td><td style='border:1px solid;background: rgb(251, 251, 251) none repeat scroll 0px 0px;text-align: center;'><table style='width:20px;height:20px;padding:14px;margin:8px;background-color:yellow;'><tr><td></td></tr></table></td><td style='border:1px solid;padding:4px;background: rgb(251, 251, 251) none repeat scroll 0px 0px;'>{0}</td></tr>", lstSetValues.Count > 5 ? string.Format("{0} Task(s) Due Today including: {1}, ..", lstSetValues.Count, string.Join(", ", lstSetValues.Select(x => x.Title).Take(5))) : string.Format("{0} Task(s) Due Today: {1}", lstSetValues.Count, string.Join(", ", lstSetValues.Select(x => x.Title))));
                        }
                    }

                    if (strHtml != null && strHtml.Length > 0)
                    {
                        strHtml.AppendFormat("</table>");
                        string subject = string.Format("Project Health Alert for {0}: {1}", pmmTicketId, Convert.ToString(project[DatabaseObjects.Columns.Title]));
                        string mailBody = strHtml.ToString();
                        Ticket ticket = new Ticket(context, ModuleNames.PMM);
                        ticket.SendEmailToActionUsers(project, subject, mailBody);
                    }
                }
            }

            ULog.WriteLog("job done!");
        }

        private class TaskProperties
        {
            public string Title { get; set; }
            public string Id { get; set; }
        }
    }

}
