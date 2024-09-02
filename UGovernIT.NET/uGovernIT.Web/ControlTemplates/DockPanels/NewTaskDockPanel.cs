using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility.DockPanels;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class NewTaskDockPanel : DockPanel
    {
        Control control;
        public override Control LoadControl(Page page)
        {
            control = (TaskUserControlNew)page.LoadControl("~/ControlTemplates/RMONE/TaskUserControlNew.ascx");
            control.ID = "dashboardNewSLADockPanel";
            return control;
        }
    }
}