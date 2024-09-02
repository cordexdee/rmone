using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class OuterPanel : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            outerPanel.Style.Add("height", Height.ToString());
            outerPanel.Style.Add("width", Width.ToString());
        }
    }
}