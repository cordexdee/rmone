using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class HelpCardAdd : System.Web.UI.UserControl
    {
        public string strAction { get; set; }
        public string TicketId { get; set; }
        public long HelpCardId { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}