using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.AssetIncidentRelations)]
    public class AssetIncidentRelations:DBBaseEntity
    {
        public long ID { get; set; }
        public string AssetTagNumLookup { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string ParentTicketId { get; set; }
    }
}
