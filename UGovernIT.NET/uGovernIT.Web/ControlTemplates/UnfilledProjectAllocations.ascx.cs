using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web
{
    public partial class UnfilledProjectAllocations : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string ViewID { get; set; }
        public string Caption { get; set; }
        //public string UnfilledAllocationType { get; set; }
        public bool ShowByUsersDivision { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}