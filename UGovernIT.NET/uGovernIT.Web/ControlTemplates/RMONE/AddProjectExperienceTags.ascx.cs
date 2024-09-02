using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class AddProjectExperienceTags : System.Web.UI.UserControl
    {
        public string TicketId { get; set; }
        public string RequestFrom { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public bool HasAccessToAddTags
        {
            get
            {
                return uHelper.HasAccessToAddExpTags(context);
            }
        }
        public bool UseMultiSelect
        {
            get
            {
                return uHelper.HasExperienceTagAllowMultiselect(context);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}