﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class AllOpenOPM : System.Web.UI.UserControl
    {
        public string viewTicketsPath; 
        protected void Page_Load(object sender, EventArgs e)
        {
           viewTicketsPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=homedashboardtickets&Module=CPR");
        }
    }
}