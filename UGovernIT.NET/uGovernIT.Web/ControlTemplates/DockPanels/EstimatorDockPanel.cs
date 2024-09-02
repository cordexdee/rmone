using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Web.ControlTemplates.RMONE;

namespace uGovernIT.Web.ControlTemplates.DockPanels
{
    public class EstimatorDockPanel : DockPanel
    {
        public CRMEstimatorView control;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        public override Control LoadControl(Page page)
        {
            control = page.LoadControl("~/ControlTemplates/RMONE/CRMEstimatorView.ascx") as CRMEstimatorView;
            control.ID = "EstimatorDockPanel";
            return control;
        }
    }
}