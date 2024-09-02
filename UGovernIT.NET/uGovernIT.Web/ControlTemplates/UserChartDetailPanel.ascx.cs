using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class UserChartDetailPanel : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string ViewID { get; set; }
        public string panelId { get; set; }
        public string ShowDetails { get; set; }
        public string KPIId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            divChartDetailPanel.Style.Add("height", Height.ToString());
            divChartDetailPanel.Style.Add("width", Width.ToString());
        }
    }
}