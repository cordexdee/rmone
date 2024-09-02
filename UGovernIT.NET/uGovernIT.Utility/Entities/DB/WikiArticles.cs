using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DB
{
    [Table(DatabaseObjects.Tables.WikiArticles)]
    public class WikiArticles : DBBaseEntity
    {
        public long ID { get; set; }
        public string AuthorizedToView { get; set; }
        //public bool IsDeleted { get; set; }

        public string ModuleNameLookup { get; set; }
        public long? RequestTypeLookup { get; set; }

        public string Title { get; set; }
        public long? WikiContentID { get; set; }
        public string TicketId { get; set; }
        public string WikiSnapshot { get; set; }

        public double?  WikiAverageScore { get; set; }
        public long?  WikiDiscussionCount { get; set; }
        public long?  WikiDislikesCount { get; set; }
        public bool?  WikiFavorites { get; set; }
        public long?  WikiLikesCount { get; set; }
        public long?  WikiLinksCount { get; set; }
        public long?  WikiServiceRequestCount { get; set; }
        public long?  WikiViews { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string WikiDescription { get; set; }
        public string WikiHistory { get; set; }

        
        // extra added 

    }
}
