using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:DockPanelManager runat=server></{0}:DockPanelManager>")]
    public class DockPanelManager : ASPxDockManager
    {
        public string Module { get; set; }
        protected override void OnInit(EventArgs e)
        {            
            base.OnInit(e);
        }
       
    }
}