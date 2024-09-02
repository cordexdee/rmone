using System;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.State)]
    public class State : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
