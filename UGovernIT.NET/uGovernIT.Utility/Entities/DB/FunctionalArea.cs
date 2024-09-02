using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.FunctionalAreas)]
    [ProtoContract(ImplicitFields =ImplicitFields.AllFields)]
    public class FunctionalArea:DBBaseEntity
    {
       
        public long ID { get; set; }
        public long? DepartmentLookup { get; set; }
        public string FunctionalAreaDescription { get; set; }
        //public bool IsDeleted { get; set; }
        [Column(DatabaseObjects.Columns.Owner)]
        public string Owner { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public List<string> Managers { get; set; }
    }
}
