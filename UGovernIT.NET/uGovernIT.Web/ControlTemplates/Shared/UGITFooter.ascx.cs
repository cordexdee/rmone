
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Web.ControlTemplates.GlobalPage;

namespace uGovernIT.Web
{
    public partial class UGITFooter : UserControl
    {
        ConfigurationVariableManager objConfigurationVariableHelper = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            var pageConfig = this.Page.PageConfig();
            if (pageConfig != null && pageConfig.HideFooter)
                return;

            dvfooter.Visible = true;
            objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());

            string currentDevExTheme = DevExpress.Web.ASPxWebControl.GlobalTheme;
            if (currentDevExTheme.ToLower() != "ugitclassic" && currentDevExTheme.ToLower() != "ugitclassicdevex")
            {
                string logurlpage = objConfigurationVariableHelper.GetValue(ConfigConstants.FooterLogoLink);
                if (!string.IsNullOrEmpty(logurlpage))
                    imgfooterlogo.ClientSideEvents.Click = $"function(s, e) {{ window.open('{logurlpage}') }}";
                else
                    imgfooterlogo.Enabled = false;
                imgfooterlogo.ImageUrl = objConfigurationVariableHelper.GetValue(ConfigConstants.FooterLogo);
                dvfooter.CssClass += string.Format(" dxnbLite_{0}", currentDevExTheme);
               lbCopywrite.Text = "<span class='ugitfootercopyrightAnchor'>" + objConfigurationVariableHelper.GetValue(ConfigConstants.FooterText) + "</span>";// "uGovernIT.com 2013 | Version 2.0";
            }
            else
            {
                lbCopywrite.Text = objConfigurationVariableHelper.GetValue(ConfigConstants.FooterText);// "uGovernIT.com 2013 | Version 2.0";
            }

        }
    }
}
