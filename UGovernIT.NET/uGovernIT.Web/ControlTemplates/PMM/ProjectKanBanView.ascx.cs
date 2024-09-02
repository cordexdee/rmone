using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web.ControlTemplates.PMM
{
    public partial class ProjectKanBanView : System.Web.UI.UserControl
    {
        public string TicketID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            TicketID = "PMM-19-000012";
        }
    }
}