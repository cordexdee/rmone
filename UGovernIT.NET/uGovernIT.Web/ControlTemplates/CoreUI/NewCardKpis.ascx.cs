using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.CoreUI
{
    public partial class NewCardKpis : System.Web.UI.UserControl
    {
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string _headtype;
        public String HeadType
        {
            get { return _headtype; }
            set { _headtype = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //_headtype = UGITUtility.ObjectToString((this.Parent as dynamic).HeadType);
        }
    }
}