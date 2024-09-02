using System;
using System.Configuration;

namespace uGovernIT.Web
{
    public partial class DocumentControlView : System.Web.UI.UserControl
    {
        public string TicketId { get; set; }
        public string ModuleName { get; set; }
        public string DocumentManagementUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //DocumentManagementUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}/documentmanagement/repository?ticketid={TicketId}";
            DocumentManagementUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}/documentmanagement/Index?ticketid={TicketId}";
        }
    }
}