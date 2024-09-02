using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web.ControlTemplates
{
    public partial class HelpCardDisplay : System.Web.UI.UserControl
    {
        public string TicketId;
        public ApplicationContext applicationContext = null;
        public HelpCardContentManager helpCardContentManager = null;
        HelpCardContent helpCardContent = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            applicationContext = HttpContext.Current.GetManagerContext();
            helpCardContentManager = new HelpCardContentManager(applicationContext);
            helpCardContent = new HelpCardContent();
            if (string.IsNullOrEmpty(TicketId))
                return;

            helpCardContent = helpCardContentManager.Load(x => x.TicketId == TicketId).FirstOrDefault();
            helpCardPreview.InnerHtml = helpCardContent.Content;
        }
    }
}