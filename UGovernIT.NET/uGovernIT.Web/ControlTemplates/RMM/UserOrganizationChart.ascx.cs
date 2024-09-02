using System;
using System.Web.UI;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class UserOrganizationChart : UserControl
    {
        public string jsonString { get; set; }
        
        protected override void OnInit(EventArgs e)
        {
            hdnfield.Set("UserInfo",UGITUtility.GetAbsoluteURL(string.Format("/ControlTemplates/RMM/userinfo.aspx?")));
            hdnfield.Set("RequestUrl", Request.Url.AbsolutePath);

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}
