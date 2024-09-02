using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class FinancialDetails : System.Web.UI.UserControl
    {
        public string Filter { get; set; }
        public string DataType { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DataType))
                DataType = DataType.ToLower();
        }
    }
}