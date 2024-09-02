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
    public partial class AddUserExperienceTags : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public bool HasAccessToAddTags
        {
            get {
                return uHelper.HasAccessToAddExpTags(context);
            }
        }

        public bool UseMultiSelect
        {
            get {
                return uHelper.HasExperienceTagAllowMultiselect(context);
            }
        }
        public string UserId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}