using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility.Entities.DB
{


    namespace uGovernIT.Utility
    {
        [Table(DatabaseObjects.Tables.Phrase)]
        public class Phrases : DBBaseEntity
        {
            [Key]
            public long PhraseId { get; set; }
            public string Phrase { get; set; }
            public int AgentType { get; set; }
            public string TicketType { get; set; }
            public long? RequestType { get; set; }
            public long? Services { get; set; }
            public string WikiLookUp { get; set; }
            public string HelpCardLookUp { get; set; }
            [NotMapped]
            public string RequestTypeName { get; set; }
            [NotMapped]
            public string ServiceName { get; set; }
        }
    }

}
