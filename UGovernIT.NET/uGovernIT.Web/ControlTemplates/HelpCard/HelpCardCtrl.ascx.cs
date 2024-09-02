using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Utility;
using System.Web.UI.WebControls;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class HelpCardCtrl : System.Web.UI.UserControl
    {
        public Unit Height { get; set; }
        public Unit Width { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Constants.PageTitle.HLP;
        }
    }
}