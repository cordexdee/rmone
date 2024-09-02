using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public partial class SimilarProjects : System.Web.UI.UserControl
    {
        public string ProjectID { get; set; }
        //public string Sector { get; set; }
        //public string ProjectComplexity { get; set; }
        public string SearchData { get; set; }

        public string Module;
        protected void Page_Load(object sender, EventArgs e)
        {
            Module = uHelper.getModuleNameByTicketId(ProjectID);
        }
    }
}