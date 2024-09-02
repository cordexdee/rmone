using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.DRQSystemAreas)]
    public class DRQSystemArea: DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
