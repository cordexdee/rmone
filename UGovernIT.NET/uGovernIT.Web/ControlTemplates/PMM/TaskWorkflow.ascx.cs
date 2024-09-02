using System;

namespace uGovernIT.Web.ControlTemplates.PMM
{
    public partial class TaskWorkflow : System.Web.UI.UserControl
    {
        public string TicketID { get; set; }
        public string ModuleName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}