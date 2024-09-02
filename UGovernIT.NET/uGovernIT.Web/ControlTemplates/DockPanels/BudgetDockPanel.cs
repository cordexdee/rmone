﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.uGovernIT.Budget;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class BudgetDockPanel : DockPanel
    {
        public uGovernITBudgetUserControl control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/uGovernIT/Budget/uGovernITBudgetUserControl.ascx") as uGovernITBudgetUserControl;
            control.ID = "BudgetUserControl";
            return control;
        }
    }
}