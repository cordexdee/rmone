using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ProjectAllocationTemplates)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ProjectAllocationTemplate : DBBaseEntity
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string TicketID { get; set; }
        public DateTime? TicketStartDate { get; set; }
        public DateTime? TicketEndDate { get; set; }
        [Column(DatabaseObjects.Columns.ModuleNameLookup)]
        public string ModuleName { get; set; }
        public int Duration { get; set; }
        public DateTime? PreconStartDate { get; set; }
        public DateTime? PreconEndDate { get; set; }
        public DateTime? ConstStartDate { get; set; }
        public DateTime? ConstEndDate { get; set; }
        public DateTime? CloseOutStartDate { get; set; }
        public DateTime? CloseOutEndDate { get; set; }
        public string Template { get; set; }
    }
}
