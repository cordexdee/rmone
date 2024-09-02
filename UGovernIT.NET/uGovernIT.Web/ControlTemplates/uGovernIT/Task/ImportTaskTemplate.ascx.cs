using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using System.Threading;
using System.Web.UI.HtmlControls;

namespace uGovernIT.Web
{
    public partial class ImportTaskTemplate : UserControl
    {
        public string ProjectID { get; set; }
        public string ModuleName { get; set; }
        UGITTaskManager TaskManager;
        ApplicationContext AppContext = null;
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        private string formTitle = "Task Template";
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            AppContext = HttpContext.Current.GetManagerContext();
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            objConfigurationVariableHelper = new ConfigurationVariableManager(AppContext);
            if (Request["moduleName"] != null)
                ModuleName = Request["moduleName"].Trim();

            if (Request["ProjectID"] != null)
                ProjectID = Request["ProjectID"].Trim();

            BindTask();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateColumns();
            GridDataBind();

            //lbMessage.Text = string.Format("This action will import task as a sub-task for the selected task for the project \"{0}\"", ProjectID);
        }

        private void BindTask()
        {
            //DataTable dt = TaskCache.GetAllTasksByProjectID(ModuleName, ProjectID);
            var tasks = TaskManager.LoadByProjectID(ModuleName, ProjectID);
            tasks = UGITTaskManager.MapRelationalObjects(tasks);
            var ptask = tasks.FindAll(x => x.ParentTaskID == 0);
            if (ptask != null && ptask.Count > 0)
            {
                TreeViewNode node = new TreeViewNode { Text = ProjectID };
                //node.Enabled = false;
                AddNodesInTreeView(ptask, ref node);
                tvTasks.Nodes.Add(node);
            }
            tvTasks.ExpandAll();
        }

        void AddNodesInTreeView(List<UGITTask> tasks, ref TreeViewNode node)
        {
            //= new TreeViewNode();
            foreach (var task in tasks)
            {
                TreeViewNode cnode = new TreeViewNode
                {
                    Text = task.Title,
                    Name = task.ID.ToString()
                };
                node.Nodes.Add(cnode);
                if (task.ChildCount > 0)
                {
                    cnode.TextStyle.Font.Bold = true;
                    AddNodesInTreeView(task.ChildTasks, ref cnode);
                }
            }
            //return node;
        }

        protected void btLoadFromTemplate_Click(object sender, EventArgs e)
        {
            DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectID);

            TaskManager.chkSaveTaskDate = chkboxSaveTaskDate.Checked;

            List<UGITTask> projectTasks = null;
            if (importtask.Checked)
            {
                projectTasks = ImportTaskUnderExistingTask();
            }
            else if (overrideTask.Checked)
            {
                projectTasks = OverrideAllTasks();
            }
            //set fields like start and enddate and other schedule related field in project
            TaskManager.CalculateProjectStartEndDate(ModuleName, projectTasks, project);

            bool autoCreateRMMProjectAllocation = objConfigurationVariableHelper.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);
            if (autoCreateRMMProjectAllocation && projectTasks != null)
            {
                ResourceAllocationManager allocationManager = new ResourceAllocationManager(AppContext);
                ThreadStart threadStartMethodUpdateProjectPlannedAllocation = delegate () { allocationManager.UpdateProjectPlannedAllocationByUser(projectTasks, "NPR", Convert.ToString(project[DatabaseObjects.Columns.TicketId]), false); };
                Thread sThreadUpdateProjectPlannedAllocation = new Thread(threadStartMethodUpdateProjectPlannedAllocation);
                sThreadUpdateProjectPlannedAllocation.IsBackground = true;
                sThreadUpdateProjectPlannedAllocation.Start();

            }

            //User Allocation in RMM in TaskImport...
            //ResourceAllocation.UpdateProjectPlannedAllocationByUser(SPContext.Current.Web, projectTasks, ModuleName, ProjectID,true);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void GridDataBind()
        {
            TaskTemplateManager templateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            LifeCycleManager lifeCycleManager = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            //Added 24 jan 2020
            templateHelper.chkSaveTaskDate = chkboxSaveTaskDate.Checked;
            //
            DataTable data = templateHelper.LoadTemplates();
            int cnt = 0;
            foreach (DataRow dr in data.Rows)
            {
                if (!string.IsNullOrEmpty(UGITUtility.ObjectToString(data.Rows[cnt][DatabaseObjects.Columns.ProjectLifeCycleLookup])))
                {
                    var lifeCycleName = lifeCycleManager.Get($"Where ID={data.Rows[cnt][DatabaseObjects.Columns.ProjectLifeCycleLookup]}");
                    dr[DatabaseObjects.Columns.ProjectLifeCycleLookup] = Convert.ToString(lifeCycleName.Name);
                }
                cnt++;
            }
            grid.DataSource = data;
            grid.DataBind();
        }

        private void GenerateColumns()
        {
            if (grid.Columns.Count <= 0)
            {
                GridViewDataTextColumn colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.ProjectLifeCycleLookup;
                colId.Caption = "Lifecycle";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.GroupIndex = 0;
                grid.Columns.Add(colId);

                colId = new GridViewDataTextColumn();
                colId.FieldName = DatabaseObjects.Columns.Title;
                colId.Caption = "Pick a template";
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.Settings.ShowFilterRowMenu = DevExpress.Utils.DefaultBoolean.True;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                colId.EditCellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                grid.Columns.Add(colId);
            }
        }

        protected void SaveTaskDate_CheckedChanged(object sender, EventArgs e)
        {
            TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            if (chkboxSaveTaskDate.Checked)
            {

                taskTemplateHelper.chkSaveTaskDate = true;
            }
            else
            {
                taskTemplateHelper.chkSaveTaskDate = false;
            }

        }

        protected void overrideTask_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbImporttask = sender as RadioButton;
            if (rbImporttask.ID == overrideTask.ID)
            {
                splitterImporttask.GetPaneByName("Tasks").Collapsed = true;
            }
            else
            {
                splitterImporttask.GetPaneByName("Tasks").Collapsed = false;
            }
        }

        protected List<UGITTask> OverrideAllTasks()
        {
            List<UGITTask> projectTasks = TaskManager.LoadByProjectID(ModuleName, ProjectID);
            if (projectTasks != null && projectTasks.Count > 0)
            {
                TaskManager.DeleteTasks(ModuleName, projectTasks);
            }

            //Load project to get some datail like start and enddate
            DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectID);
            DateTime projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate] == DBNull.Value ? new DateTime(1800, 1, 1) : project[DatabaseObjects.Columns.TicketActualStartDate]);
            if (projectStartDate == DateTime.MinValue)
                projectStartDate = DateTime.Now;

            int templateID = 0;
            List<object> selectedTemplate = grid.GetSelectedFieldValues("ID");
            if (selectedTemplate.Count > 0)
            {
                int.TryParse(Convert.ToString(selectedTemplate[0]), out templateID);
            }

            //fetch lifecycle of project
            //SPFieldLookupValue lifeCycleLookup = new SPFieldLookupValue(Convert.ToString(uHelper.GetSPItemValue(project, DatabaseObjects.Columns.ProjectLifeCycleLookup)));
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            LifeCycle lifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleName)[0];

            //delete existing task and import task from selected template
            TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            List<UGITTask> templateTasks = new List<UGITTask>();
            //Added 24 jan 2020
            taskTemplateHelper.chkSaveTaskDate = chkboxSaveTaskDate.Checked;
            //
            TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(templateID, ProjectID);
            templateTasks = taskTemplateHelper.GenerateTasksFromTaskTemplate(lifeCycle, projectStartDate, taskTemplate.Tasks, chkboxSaveTaskDate.Checked);//Added chksavedate

            TaskManager.ReManageTasks(ref templateTasks, true);

            foreach (UGITTask tk in templateTasks)
            {
                tk.ModuleNameLookup = ModuleName;
                //tk.ProjectLookup = Convert.ToInt32(project[DatabaseObjects.Columns.TicketId]); // new SPFieldLookupValue(project.ID, Convert.ToString(project[DatabaseObjects.Columns.TicketId]));
                tk.TicketId = Convert.ToString(project[DatabaseObjects.Columns.TicketId]);
            }

            templateTasks = TaskManager.ImportTasks(ModuleName, templateTasks, false, ProjectID);

            return templateTasks;
        }

        protected List<UGITTask> ImportTaskUnderExistingTask()
        {
            int selectedTaskText = 0;
            List<UGITTask> projectTasks = TaskManager.LoadByProjectID(ModuleName, ProjectID);

            if (tvTasks.SelectedNode != null && tvTasks.SelectedNode.Name != "")
                selectedTaskText = Convert.ToInt32(tvTasks.SelectedNode.Name);
            UGITTask selectedtask = null;
            if (projectTasks.Exists(x => x.ID == selectedTaskText))
            {
                selectedtask = projectTasks.Find(x => x.ID == selectedTaskText);
            }

            //Load project to get some datail like start and enddate
            DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectID);
            DateTime projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate] == DBNull.Value ? DateTime.MinValue : project[DatabaseObjects.Columns.TicketActualStartDate]);
            if (projectStartDate == DateTime.MinValue && !(project[DatabaseObjects.Columns.TicketTargetStartDate] is DBNull))
                projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketTargetStartDate]); //DateTime.Now;

            int templateID = 0;
            List<object> selectedTemplate = grid.GetSelectedFieldValues("ID");
            if (selectedTemplate.Count > 0)
            {
                int.TryParse(Convert.ToString(selectedTemplate[0]), out templateID);
            }

            //fetch lifecycle of project
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            LifeCycle lifeCycle = lifeCycleHelper.LoadLifeCycleByModule(ModuleName)[0]; // lifeCycleHelper.LoadProjectLifeCycleByName(lifeCycleLookup.LookupValue);

            //delete existing task and import task from selected template
            TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            List<UGITTask> templateTasks = new List<UGITTask>();
            taskTemplateHelper.chkSaveTaskDate = chkboxSaveTaskDate.Checked;
            TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(templateID, ProjectID);
            templateTasks = taskTemplateHelper.GenerateTasksFromTaskTemplate(lifeCycle, projectStartDate, taskTemplate.Tasks, chkboxSaveTaskDate.Checked);

            TaskManager.ReManageTasks(ref templateTasks, true);

            List<UGITTask> rootChildTask = new List<UGITTask>();
            foreach (UGITTask tk in templateTasks)
            {
                tk.ModuleNameLookup = ModuleName;
                tk.TicketId = Convert.ToString(project[DatabaseObjects.Columns.TicketId]);
                if (tk.ParentTaskID == 0 && selectedtask != null)
                {
                    tk.ParentTask = selectedtask;
                    rootChildTask.Add(tk);
                }
                tk.Changes = true;
                tk.ID = 0;
                tk.Predecessors = null;
                tk.ParentTaskID = 0;
            }
            //code to import all tasks under selected task
            if (selectedtask != null)
            {
                if (rootChildTask.Count > 0)
                    selectedtask.ChildTasks = rootChildTask;
            }
            projectTasks.AddRange(templateTasks);


            TaskManager.SaveTasks(ref projectTasks, ModuleName, ProjectID);

            ////remanage parentid and predecessors
            foreach (UGITTask task in projectTasks)
            {

                if (task.ChildTasks != null)
                {
                    foreach (UGITTask childtask in task.ChildTasks)
                    {
                        childtask.ParentTaskID = task.ID;
                        task.Changes = true;
                    }
                }
            }

            TaskManager.ReManageTasks(ref projectTasks, false);

            //Saves parentid and predecessor relation
            TaskManager.SaveTasks(ref projectTasks, ModuleName, ProjectID);

            return projectTasks;
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit")
            {
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}";
                int Index = e.VisibleIndex;
                string editParam = "tasktemplateview";
                string datakeyvalue = Convert.ToString(e.KeyValue);
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, datakeyvalue));
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','250',0,'{2}','true')", editItem, title, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor ahtml = (HtmlAnchor)grid.FindRowCellTemplateControl(Index, e.DataColumn, "editlink");
                ahtml.Attributes.Add("href", url);
                if (e.DataColumn.FieldName == "Title")
                {
                    ahtml.InnerText = e.CellValue.ToString();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }
    }
}
