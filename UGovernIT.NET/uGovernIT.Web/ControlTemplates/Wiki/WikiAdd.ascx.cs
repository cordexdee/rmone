using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;

namespace uGovernIT.Web.ControlTemplates.Wiki
{
    public partial class WikiAdd : System.Web.UI.UserControl
    {
        public string strAction { get; set; }
        public string TicketId { get; set; }
        public string RelatedTicketId { get; set; }
        public string ModuleId { get; set; }

        WikiContentsManager wikiContentsManager = new WikiContentsManager(HttpContext.Current.GetManagerContext());
        public string AssemblyVersion = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //ASPxHtmlEditor.Html =


            if (strAction == "edit")
            {
                //TicketId = "WIKI-19-000001";

                //string query = "where TicketID ='" + TicketId + "' ";
                string query = $"ticketid ='{TicketId.Trim()}'";
                //WikiArticles wikiArticle = wikiArticlesManager.Get(query);

                //var wikicontent = wikiContentsManager.Get(query);
                var wikicontent = wikiContentsManager.Load(query).FirstOrDefault();

                // wikiArticle.Content = content.Content; //html body;
                if (wikicontent != null)

                ASPxHtmlEditor.Html = Server.HtmlDecode(wikicontent.Content);

            }

        }
    }
}