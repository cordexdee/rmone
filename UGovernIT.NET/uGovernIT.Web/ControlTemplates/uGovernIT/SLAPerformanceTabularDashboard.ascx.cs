using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web
{
    public partial class SLAPerformanceTabularDashboard : UserControl
    {
        public string Width { get; set; }
        public string Height { get; set; }
        public string Title { get; set; }
        public string Module { get; set; }
        public DataTable slaTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (slaTable != null)
            {
                rptSLAParent.DataSource = slaTable;
                rptSLAParent.DataBind();
            }
        }
    }
}