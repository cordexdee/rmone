using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.EventCategories)]
    public class EventCategory: DBBaseEntity
    {
        public long ID { get; set; }
        public int ItemOrder { get; set; }
        public string ItemColor { get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
