using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DB;

namespace uGovernIT.Utility.Entities.Common
{
    public  class WikiDetailsResponse
    {
        public string TicketID { get; set; }
        public List<WikiLinks> WikiLinks { get; set; }
        public List<WikiDiscussion> WikiDiscussions { get; set; }
        public List<WikiReviews> WikiReviews { get; set;}
        public WikiContents wikiContents { get; set;}
        public WikiArticles WikiArticles { get; set; }
        public string WikiContentsModified { get; set; }
        public string WikiDiscussionCreated { get; set; }
        public bool IsAuthorizedToView { get; set; }
        public WikiDetailsResponse()
        {
            WikiLinks = new List<WikiLinks>();
            WikiDiscussions = new List<WikiDiscussion>();
            wikiContents = new WikiContents();
            WikiReviews = new List<WikiReviews>();
            WikiArticles = new WikiArticles();
        }
    }
}
