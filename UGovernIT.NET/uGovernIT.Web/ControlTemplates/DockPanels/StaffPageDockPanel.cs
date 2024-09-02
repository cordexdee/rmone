using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.RMONE;
using uGovernIT.Web.ControlTemplates.uGovernIT;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class StaffPageDockPanel : DockPanel
    {
        public EstimatedBillingHoursAnalytic control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/RMONE/EstimatedBillingHoursAnalytic.ascx") as EstimatedBillingHoursAnalytic;
            control.ID = "StaffPageDock";
            return control;
        }
    }
}