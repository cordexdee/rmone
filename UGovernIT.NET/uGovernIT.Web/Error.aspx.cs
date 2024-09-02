using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ErrorMsgPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid guid= Guid.NewGuid();
            lblTimeStamp.Text = DateTime.Now.ToString(@"MMM-dd-yyyy hh:mm:ss tt");
            lblerrorcode.Text = guid.ToString();
            ULog.WriteException($"Error code: {guid}, Time: {lblTimeStamp.Text}");
        }
    }
}