using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.uGovernIT.Budget;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class UserEntryDockPanel : DockPanel
    {
        public UserEntryControl control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/UserEntryControl.ascx") as UserEntryControl;
            control.ID = "UserEntryControl";
            return control;
        }
    }
}