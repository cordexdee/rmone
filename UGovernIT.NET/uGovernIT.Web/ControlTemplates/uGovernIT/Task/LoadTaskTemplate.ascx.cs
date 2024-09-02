using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Core;
using System.Data;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
namespace uGovernIT.Web
{
    public partial class LoadTaskTemplate : UserControl
    {
        public string ProjectID { get; set; }
        public string ModuleName { get; set; }
        UGITTaskManager TaskManager;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            TaskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateColumns();
            GridDataBind();

            if (Request["moduleName"] != null)
                ModuleName = Request["moduleName"].Trim();

            if (Request["ProjectID"] != null)
                ProjectID = Request["ProjectID"].Trim();

            lbMessage.Text = string.Format("This action will delete all existing tasks of project \"{0}\"", ProjectID);
        }

        

        protected void btLoadFromTemplate_Click(object sender, EventArgs e)
        {
            List<UGITTask> projectTasks = TaskManager.LoadByProjectID(ModuleName, ProjectID);
            if (projectTasks != null && projectTasks.Count > 0)
            {
                TaskManager.DeleteTasks(ModuleName, projectTasks);
            }

            //Load project to get some datail like start and enddate
            DataRow project = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectID);
            DateTime projectStartDate = Convert.ToDateTime(project[DatabaseObjects.Columns.TicketActualStartDate]);
            if (projectStartDate == DateTime.MinValue)
                projectStartDate = DateTime.Now;

            int templateID = 0;
            List<object> selectedTemplate = grid.GetSelectedFieldValues("ID");
            if (selectedTemplate.Count > 0)
            {
                int.TryParse(Convert.ToString(selectedTemplate[0]), out templateID);
            }

            //fetch lifecycle of project
            //SPFieldLookupValue lifeCycleLookup = new SPFieldLookupValue(Convert.ToString(UGITUtility.GetSPItemValue(project, DatabaseObjects.Columns.ProjectLifeCycleLookup)));
            LifeCycleManager lifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
            LifeCycle lifeCycle = new LifeCycle();  // lifeCycleHelper.LoadLifeCycleByModule(ModuleName);
           
            //delete existing task and import task from selected template
            TaskTemplateManager taskTemplateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            List<UGITTask> templateTasks = new List<UGITTask>();

            TaskTemplate taskTemplate = taskTemplateHelper.LoadTemplateByID(templateID, ProjectID);
            templateTasks = taskTemplateHelper.GenerateTasksFromTaskTemplate(lifeCycle, projectStartDate, taskTemplate.Tasks, false);
            TaskManager.ReManageTasks(ref templateTasks, true);
            templateTasks = TaskManager.ImportTasks(ModuleName, templateTasks, false, ProjectID);

            //set fields like start and enddate and other schedule related field in project
            TaskManager.CalculateProjectStartEndDate(ModuleName, templateTasks, project);
           
            //Update field in project list
            //project.UpdateOverwriteVersion();

            //reload task cache
            //TaskCache.ReloadProjectTasks(ModuleName, ProjectID);

            //User Allocation in RMM in TaskImport...
            //ResourceAllocation.UpdateProjectPlannedAllocationByUser(SPContext.Current.Web, templateTasks, ModuleName, ProjectID,true);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void GridDataBind()
        {
            TaskTemplateManager templateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
            DataTable data = templateHelper.LoadTemplates();
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
                colId.Caption = DatabaseObjects.Columns.Title;
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
    }
}
