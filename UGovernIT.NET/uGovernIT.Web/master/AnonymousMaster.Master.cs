using System;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AnonymousMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            //DataTable dt = new DataTable("FieldConfiguration");
            //DataColumn dc = new DataColumn("Title", typeof(System.String));
            //DataRow dr = dt.dataro

            //uGovernIT.Manager.Controls.GetControls(dt,dc,ControlMode.Display);
            if (Page.User.Identity.IsAuthenticated && !Convert.ToString(Request.Url).Contains("DataMigration"))
            {
                Response.Redirect("~/Default.aspx");
            }

            ApplicationContext context = Context.GetManagerContext();

            ConfigurationVariable headerLogo = context.ConfigManager.Get(x => x.KeyName == ConfigConstants.HeaderLogo);
            if (headerLogo != null)
            {
                ImageLink.Src = headerLogo.KeyValue;
            }

            ConfigurationVariable FooterText = context.ConfigManager.Get(x => x.KeyName == Utility.Constants.FooterText);
            if (FooterText != null)
            {
                footerText.Visible = true;
                footerText.Text = FooterText.KeyValue;
            }
        }
    }
}