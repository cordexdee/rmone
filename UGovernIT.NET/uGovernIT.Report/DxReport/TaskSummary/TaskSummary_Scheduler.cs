using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Manager.Report.Entities;
using uGovernIT.Manager.Reports;
using uGovernIT.Report.Helpers;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Linq;

namespace uGovernIT.Report.DxReport
{
    public class TaskSummary_Scheduler : IReportScheduler
    {
        TicketManager ticketManager;
        UGITTaskManager uGITTaskManager;
        ModuleViewManager moduleViewManager;
        public TaskSummary_Scheduler()
        {

        }
        /// <summary>
        /// Gets the TSK summary report.
        /// </summary>
        /// <param name="formobj">The formobj.</param>
        /// <returns>System.String.</returns>
        public Dictionary<string, object> GetDefaultData()
        {
            throw new NotImplementedException();
            
        }

        public string GetReport(ApplicationContext applicationContext, Dictionary<string, object> formobj, string attachFormat)
        {
            string filePath = uHelper.GetTempFolderPathNew() + "/" + Guid.NewGuid();//Microsoft.SharePoint.Utilities.SPUtility.GetVersionedGenericSetupPath(string.Format(@"TEMPLATE\IMAGES\UGOVERNITTEMP\{0}\", Guid.NewGuid()), 15);
            string fileName = string.Empty;

            int[] Ids = Array.ConvertAll<string, int>(Convert.ToString(formobj[ReportScheduleConstant.Projects]).Split(','), int.Parse);
            TicketStatus tstatus = (TicketStatus)Enum.Parse(typeof(TicketStatus), Convert.ToString(formobj[ReportScheduleConstant.TicketStatus]));
            string moduleName = Convert.ToString(formobj[ReportScheduleConstant.Module]);

            if (string.IsNullOrEmpty(attachFormat))
                attachFormat = Convert.ToString(formobj[ReportScheduleConstant.AttachmentFormat]);

            string title = Convert.ToString(formobj[DatabaseObjects.Columns.Title]);
            DataTable dataSource = GetProjectsTasksTable(applicationContext, Ids, tstatus, moduleName);

            TaskSummary_Report report = new TaskSummary_Report(dataSource);

            return ReportHelper.ExportFiles(report, attachFormat, filePath, title); 
        }
        public DataTable GetProjectsTasksTable(ApplicationContext applicationContext, int[] Ids, TicketStatus tstatus, string moduleName)
        {
            UserProfileManager userManager = new UserProfileManager(applicationContext);
            ticketManager = new TicketManager(applicationContext);
            uGITTaskManager = new UGITTaskManager(applicationContext);
            moduleViewManager = new ModuleViewManager(applicationContext);
            DataTable openProjects = new DataTable();
            //int moduleId = getModuleIdByModuleName(moduleName);
            UGITModule uGITModule = moduleViewManager.LoadByName(moduleName,true);

            DataTable allTasks = null;
            if (tstatus == TicketStatus.Open)
                allTasks = uGITTaskManager.GetAllTasks(moduleName);
            else
                allTasks = uGITTaskManager.LoadTasksTable(moduleName);

            switch (tstatus)
            {
                case TicketStatus.Open:
                    openProjects = ticketManager.GetOpenTickets(uGITModule); //uGITCache.ModuleDataCache.GetOpenTickets(moduleId, spWeb);
                    //allTasks =   TaskCache.GetAllTasks(moduleName,spWeb); // Returns all tasks of OPEN projects only
                    break;
                case TicketStatus.Closed:
                    openProjects = ticketManager.GetClosedTickets(uGITModule); //uGITCache.ModuleDataCache.GetClosedTickets(moduleId, spWeb);
                    //allTasks = UGITTaskHelper.LoadTasksTable(spWeb, moduleName, false, null);
                    break;
                default: // All projects
                    openProjects = ticketManager.GetAllTickets(uGITModule); //uGITCache.ModuleDataCache.GetAllTickets(moduleId, spWeb);
                    //allTasks = UGITTaskHelper.LoadTasksTable(spWeb, moduleName);
                    break;
            }



            if (allTasks == null || allTasks.Rows.Count <= 0 || openProjects == null || openProjects.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                var dr = (from x in openProjects.AsEnumerable()
                                where (Ids.Contains(Convert.ToInt32(x.Field<long>(DatabaseObjects.Columns.ID))))
                                select x);
                if (dr.Count<DataRow>() > 0)
                    openProjects = dr.CopyToDataTable();
                else
                    openProjects = openProjects.Clone();
                //openProjects = (from x in openProjects.AsEnumerable()
                //                where (Ids.Contains(Convert.ToInt32(x.Field<long>(DatabaseObjects.Columns.ID))))
                //                select x).CopyToDataTable();
            }

            var openTaskRows = (from t in allTasks.AsEnumerable()
                                join p in openProjects.AsEnumerable() on
                                    t.Field<string>(DatabaseObjects.Columns.TicketId) equals
                                    p.Field<string>(DatabaseObjects.Columns.TicketId)
                                select t).ToArray();

            DataTable openProjectTasks = null;

            if (openTaskRows.Length > 0)
            {
                openProjectTasks = openTaskRows.CopyToDataTable();

                //Update Predecessor by order field
                //  uGITTaskManager.UpdatePredecessorsByOrder(openProjectTasks);

                openProjectTasks.Columns.Add("TitleWithPctComplete");
                openProjectTasks.Columns.Add(DatabaseObjects.Columns.BehaviourIcon);
                int openProjectWithTasks = 0;

                foreach (DataRow row in openProjects.Rows)
                {
                    DataRow[] projectTasks = openProjectTasks.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketId, row[DatabaseObjects.Columns.TicketId]));
                    if (projectTasks.Length > 0)
                    {
                        openProjectWithTasks += 1;
                        foreach (DataRow pTask in projectTasks)
                        {
                            int pctComplete = 0;
                            int.TryParse(Convert.ToString(UGITUtility.GetSPItemValue(row, DatabaseObjects.Columns.TicketPctComplete)), out pctComplete);
                            pTask["TitleWithPctComplete"] = string.Format("{0} ({1}% complete)", row[DatabaseObjects.Columns.Title], pctComplete);
                            pTask[DatabaseObjects.Columns.BehaviourIcon] =uHelper.GetBehaviourIcon(Convert.ToString(pTask[DatabaseObjects.Columns.TaskBehaviour]));
                            pTask[DatabaseObjects.Columns.AssignedTo] = userManager.GetUserNameById(pTask[DatabaseObjects.Columns.UGITAssignedTo].ToString());
                        }
                    }
                }
            }

            if (openProjectTasks == null || openProjectTasks.Rows.Count <= 0)
            {
                return null;
            }

            DataView view = openProjectTasks.DefaultView;
            view.Sort = string.Format("{0} asc, {1} asc", DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.ItemOrder);

            return openProjectTasks.DefaultView.ToTable();
        }
        //internal static DataTable GetProjectsTasksTable(int[] Ids, TicketStatus tstatus, string moduleName)
        //{
        //    return GetProjectsTasksTable(Ids, tstatus, moduleName, SPContext.Current.Web);
        //}

    }
}