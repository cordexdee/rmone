using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class ModuleConstraintsListDx : System.Web.UI.UserControl
    {
        public string ConstraintTaskUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=modulestagetask");
        public string TicketId { get; set; }
        public bool IsRequestFromSummary { get; set; }
        //public bool IsRequestFromSummaryOrTask { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["fromCommentTask"] != null)
            {
                UGITUtility.CreateCookie(Response, "fromCommentTask", Session["fromCommentTask"].ToString());
                Session["fromCommentTask"] = null;
            }
            else
            {
                UGITUtility.DeleteCookie(Request, Response, "fromCommentTask");
            }
        }
    }
}