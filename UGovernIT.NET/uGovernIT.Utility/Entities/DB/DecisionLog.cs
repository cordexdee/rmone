using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.DecisionLog)]
    public class DecisionLog : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public Nullable <DateTime>  ReleaseDate { get; set; }
        public int ItemOrder { get; set; }
        public string ReleaseID { get; set; }
        public string Description { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }
        public string AdditionalComments { get; set; }
        [Column("ModuleNameLookup")]
        public string ModuleName { get; set; }
        public string TicketId { get; set; }
        public Nullable<DateTime> DateIdentified { get; set; }
        public string DecisionStatus { get; set; }
        [Column(DatabaseObjects.Columns.DecisionMaker)]
        public string DecisionMaker { get; set; }
        public Nullable<DateTime> DateAssigned { get; set; }
        public string Decision { get; set; }
        public Nullable<DateTime> DecisionDate { get; set; }
        public string DecisionSource { get; set; }
    }
}
