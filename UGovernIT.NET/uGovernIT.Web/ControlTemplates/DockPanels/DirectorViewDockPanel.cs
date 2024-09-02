using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.RMONE;
using uGovernIT.Web.ControlTemplates.uGovernIT;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class DirectorViewDockPanel : DockPanel
    {
        public ProjectGantt control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/RMONE/ProjectGantt.ascx") as ProjectGantt;
            control.ID = "DirectorViewDockPanel";
            return control;
        }
    }
}