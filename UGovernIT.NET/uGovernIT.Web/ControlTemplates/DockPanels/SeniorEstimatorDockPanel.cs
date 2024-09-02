using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class SeniorEstimatorDockPanel : DockPanel
    {
        public CRMLeadEstimatorView control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/RMONE/CRMLeadEstimatorView.ascx") as CRMLeadEstimatorView;
            control.ID = "SeniorEstimatorDockPanel";
            return control;
        }
    }
}