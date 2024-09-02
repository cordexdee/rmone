using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    
    public class TaskManagerDockPanel : DockPanel
    {
        public CustomProjectTasks control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/PMM/CustomProjectTasks.ascx") as CustomProjectTasks;
            control.ID = "customProjectTasks";

            return control;
        }
    }
}