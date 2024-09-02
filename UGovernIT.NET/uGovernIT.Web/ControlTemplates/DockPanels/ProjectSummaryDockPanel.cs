using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.RMONE;
using uGovernIT.Web.ControlTemplates.uGovernIT;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class ProjectSummaryDockPanel : DockPanel
    {
        public ProjectSummary control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/RMONE/ProjectSummary.ascx") as ProjectSummary;
            control.ID = "ProjectSummaryPageDock";
            return control;
        }
    }
}