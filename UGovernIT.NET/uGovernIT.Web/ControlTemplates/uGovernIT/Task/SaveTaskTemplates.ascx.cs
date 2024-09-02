using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Collections.Generic;
using uGovernIT.Core;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;

namespace uGovernIT.Web
{
    public partial class SaveTaskTemplates : UserControl
    {
        public string ProjectPublicID{get;set;}
        public string ModuleName { get; set; }
        int lifeCycle;
        TaskTemplateManager taskTemplateHelper;
        protected override void OnInit(EventArgs e)
        {
            if (Request["moduleName"] != null)
                ModuleName = Request["moduleName"].Trim();

            if (Request["ProjectID"] != null)
                ProjectPublicID = Request["ProjectID"].Trim();

            DataRow item = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), ModuleName, ProjectPublicID);
            if (item != null && UGITUtility.IsSPItemExist(item, DatabaseObjects.Columns.ProjectLifeCycleLookup))
            {
                lifeCycle = Convert.ToInt32(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.ProjectLifeCycleLookup));
            }

            taskTemplateHelper = new TaskTemplateManager(HttpContext.Current.GetManagerContext());
        
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSaveAsTemplate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
            List<UGITTask> tasks = taskManager.LoadByProjectID(ModuleName, ProjectPublicID);
            if (tasks.Count > 0)
            {
                TaskTemplate template = new TaskTemplate();
                template.Title = txtTitle.Text.Trim();
                template.Description = txtDescription.Text.Trim();
                if (lifeCycle > 0)
                    template.ProjectLifeCycleLookup = lifeCycle;
                else
                    template.ProjectLifeCycleLookup = null;

                tasks = UGITTaskManager.MapRelationalObjects(tasks);
                template.Tasks = tasks;
                taskTemplateHelper.AddNewTaskTemplate(template);
                uHelper.ClosePopUpAndEndResponse(Context, false);
            }
            else
            {
                lbMessage.Text = "You need to create task(s) before saving it as a template.";
            }
        }

        protected void cvTxtTitle_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (taskTemplateHelper.TemplateExist(txtTitle.Text))
            {
                args.IsValid = false;
            }
        }
    }
}
