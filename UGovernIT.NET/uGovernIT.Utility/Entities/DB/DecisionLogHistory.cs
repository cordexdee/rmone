using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.DecisionLogHistory)]
    public class DecisionLogHistory:DBBaseEntity
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int ItemOrder { get; set; }

        public string ReleaseID { get; set; }

        public string Description { get; set; }
        [Column(DatabaseObjects.Columns.AssignedTo)]
        public string AssignedTo { get; set; }

        public string AdditionalComments { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }

        public string TicketId { get; set; }

        public DateTime? DateIdentified { get; set; }

        public string DecisionStatus { get; set; }
        [Column(DatabaseObjects.Columns.DecisionMaker)]
        public string DecisionMaker { get; set; }

        public DateTime? DateAssigned { get; set; }

        public string Decision { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string DecisionSource { get; set; }

        public DateTime  BaselineDate { get; set; }

        public int BaselineId { get; set; }


    }
}
