using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.uGovernIT;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class BusinessInitiativeDocPanel : DockPanel
    {
        public BusinessInitiative control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/uGovernIT/BusinessInitiative.ascx") as BusinessInitiative;
            control.ID = "BusinessInitiativeDock";
            return control;
        }
    }
}