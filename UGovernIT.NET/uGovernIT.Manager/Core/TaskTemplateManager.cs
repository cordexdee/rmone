using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using uGovernIT.Utility;
using uGovernIT.DAL;
using AutoMapper;
using System.Collections;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class TaskTemplateManager:ManagerBase<TaskTemplate>,ITaskTemplateManager
    {
        DataTable _taskTemplateList;
        DataTable _taskTemplateTable;
        ApplicationContext _context;
        UGITTaskManager taskManager;
        //Added 24 jan 2020
        public bool chkSaveTaskDate { get; set; }
        //
        public TaskTemplateManager(ApplicationContext context) : base(context)
        {
            _context = context;
            store = new TaskTemplateStore(this.dbContext);
            taskManager = new UGITTaskManager(this.dbContext);
           // StoreBase<TaskTemplate> storeTaskTemplate = new StoreBase<TaskTemplate>(DatabaseObjects.Tables.UGITTaskTemplates);
            List<TaskTemplate> taskTemplates = store.Load();
            _taskTemplateList = UGITUtility.ToDataTable<TaskTemplate>(taskTemplates);
           // SPListHelper.GetSPList(DatabaseObjects.Lists.UGITTaskTemplates, _spWeb);
           /// StoreBase<TaskTemplateItem> storeTaskTemplateItem = new StoreBase<TaskTemplateItem>(DatabaseObjects.Tables.TaskTemplateItems);
             //List<TaskTemplateItem> taskTemplateItems = store.Load();
             //_taskTemplateItemList = UGITUtility.ToDataTable<TaskTemplateItem>(taskTemplateItems); // SPListHelper.GetSPList(DatabaseObjects.Lists.TaskTemplateItems, _spWeb);

           // SPListItemCollection collection = SPListHelper.GetSPListItemCollection(_taskTemplateList.ID, query, _spWeb);
            _taskTemplateTable = _taskTemplateList;  // collection.GetDataTable();
            //_taskTemplates = taskTemplates.Cast<TaskTemplate>().ToList();
            //Added 24 jan 2020
            taskManager.chkSaveTaskDate = chkSaveTaskDate;
            //
        }

        public DataTable LoadTemplates()
        {
            return _taskTemplateTable;
        }

        public bool TemplateExist(string templateName)
        {
            return TemplateExist(templateName, string.Empty);
        }
        public bool TemplateExist(string templateName, string lifeCycleName)
        {
            bool exist = false;
            if (_taskTemplateTable == null)
                return exist;

            DataView view = _taskTemplateTable.AsDataView();
            string title = templateName.Replace("'", "''").Trim();
            if (string.IsNullOrEmpty(lifeCycleName))
                view.RowFilter = string.Format("{0}='{1}'", DatabaseObjects.Columns.Title, title);
            else
                view.RowFilter = string.Format("{0}='{1}' and {2}='{3}'", DatabaseObjects.Columns.Title, title, DatabaseObjects.Columns.ProjectLifeCycleLookup, lifeCycleName);
            if (view.ToTable().Rows.Count > 0)
            {
                exist = true;
            }

            return exist;
        }

        public void AddNewTaskTemplate(TaskTemplate template)
        {
            long result = store.Insert(template);
            long templateID = Convert.ToInt32(template.ID);

            List<UGITTask> tTasks = template.Tasks;
            foreach (UGITTask task in tTasks)
            {
                task.ProjectLookup = template.ID;
                task.ModuleNameLookup = "Template";
                task.ID = 0;
                task.TicketId = Convert.ToString(templateID);
            }

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap(typeof(UGITTask), typeof(TaskTemplateItem));
            //});
            //var mapper = config.CreateMapper();
            //List<TaskTemplateItem> tTemplateItems = mapper.Map<List<TaskTemplateItem>>(tTasks);  // mapper.Map<List<UGITTask>, List<TaskTemplateItem>>(tTasks);
            //foreach(UGITTask task in tTasks)
            //{
            //    taskManager.Insert(task);
            //}
            UGITTaskManager taskHelper = new UGITTaskManager(_context,DatabaseObjects.Tables.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
            tTasks = taskHelper.ImportTasks(tTasks);
        }

    

        /// <summary>
        /// Create tasks for each stage and make it as milestone.
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<UGITTask> GenerateDefaultTasks(LifeCycle lifeCycle, DateTime startDate)
        {
            List<UGITTask> defaultTasks = new List<UGITTask>();
            List<LifeCycleStage> stages = lifeCycle.Stages;
            DateTime tDate = startDate;

            foreach (LifeCycleStage stage in stages)
            {
                DateTime[] dates = uHelper.GetEndDateByWorkingDays(_context,tDate, 1);
                UGITTask task = new UGITTask();
                task.Title = stage.Name;
                task.StartDate = dates[0];
                task.DueDate = dates[0];
                tDate = dates[0].AddDays(1);
                task.IsMileStone = true;
                task.StageStep = stage.StageStep;
                task.ItemOrder = stages.IndexOf(stage) + 1;
                defaultTasks.Add(task);
            }

            //Reference Next task as predecessor of current task. but exclude as
            for (int i = defaultTasks.Count - 1; i > 0; i--)
            {
                defaultTasks[i].PredecessorTasks = new List<UGITTask>();
                defaultTasks[i].PredecessorTasks.Add(defaultTasks[i - 1]);
            }

            return defaultTasks;
        }

        /// <summary>
        /// it generates new task list with life cycle stage step if any task is matched.
        /// it makes task id =0; it also adjusts task start and due in accordance of new start date
        /// </summary>
        /// <param name="lifeCycle"></param>
        /// <param name="startDate"></param>
        /// <param name="taskTemplate"></param>
        /// <returns></returns>
        public List<UGITTask> GenerateTasksFromTaskTemplate(LifeCycle lifeCycle, DateTime startDate, List<UGITTask> taskTemplate, bool bCheckSaveDate = true)
        {
            List<UGITTask> newTaskList = new List<UGITTask>();
            if (taskTemplate == null || taskTemplate.Count == 0)
                return newTaskList;

            UGITTaskManager taskManager = new UGITTaskManager(_context, DatabaseObjects.Tables.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
            //Added 24 jan 2020
            taskManager.chkSaveTaskDate = bCheckSaveDate;

            taskManager.ReManageTasks(ref taskTemplate, false);
            DateTime templateStartDate = taskTemplate.Min(x => x.StartDate);

            // If template dates are greater than start date, move them back to one year before start date else it will throw off all the math
            int yearDelta = 0;
            if (templateStartDate > startDate && startDate!=DateTime.MinValue)
            {
                int origYear = templateStartDate.Year;
                int newYear = startDate.Year - 1;
                yearDelta = origYear - newYear;
                templateStartDate = new DateTime(newYear, templateStartDate.Month, templateStartDate.Day);
            }

            int deltaDayDiff = uHelper.GetTotalWorkingDaysBetween(_context, templateStartDate, startDate);
            foreach (UGITTask task in taskTemplate)
            {
                task.Status = "Not Started";
                task.PercentComplete = 0;

                //If step is not exist then remove milestone from task
                if (lifeCycle != null && lifeCycle.Stages.Exists(x => x.StageStep == task.StageStep))
                {
                    task.IsMileStone = true;
                }
                else
                {
                    task.IsMileStone = false;
                    task.StageStep = 0;
                }

                DateTime taskStart = yearDelta > 0 ? task.StartDate.AddYears(-yearDelta) : task.StartDate;
                DateTime taskDue = yearDelta > 0 ? task.DueDate.AddYears(-yearDelta) : task.DueDate;

                DateTime[] startdates = uHelper.GetEndDateByWorkingDays(_context, taskStart, deltaDayDiff);
                DateTime[] endDates = uHelper.GetEndDateByWorkingDays(_context, taskDue, deltaDayDiff);

                // Added 23 Jan 2020
                if (!bCheckSaveDate)
                {
                    // Second item contain new date with default difference
                    task.StartDate = startdates[1];
                    task.DueDate = endDates[1];
                }
                
                newTaskList.Add(task);
            }

            newTaskList = newTaskList.OrderBy(x => x.ItemOrder).ToList();

            return newTaskList;
        }

        public TaskTemplate LoadTemplateByID(long templateId, string projectId)
        {
            UGITTaskManager taskManager = new UGITTaskManager(_context, DatabaseObjects.Tables.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
            //added 24 jan 2020
            taskManager.chkSaveTaskDate = chkSaveTaskDate;
            //
            TaskTemplate template = new TaskTemplate();
            DataRow item = _taskTemplateList.AsEnumerable().FirstOrDefault(x => x.Field<string>(DatabaseObjects.Columns.ID) == Convert.ToString(templateId));
            if (item != null)
            {
                template = TaskTemplate.LoadItem(item);
                UGITTaskManager taskHelper = new UGITTaskManager(_context,DatabaseObjects.Tables.TaskTemplateItems, DatabaseObjects.Columns.TaskTemplateLookup);
                List<UGITTask> tasks = taskHelper.LoadByTemplateID(templateId);
                //added 24 jan 2020

                //
                taskManager.ReManageTasks(ref tasks, false);
                template.Tasks = tasks;
            }

            return template;
        }
    }
    public interface ITaskTemplateManager : IManagerBase<TaskTemplate>
    { }
}
