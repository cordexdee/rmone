using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.StatisticsConfiguration)]
    public class StatisticsConfiguration
    {
        public long ID { get; set; }
        public string ModuleName { get; set; }
        public string FieldName { get; set; }
    }
}
