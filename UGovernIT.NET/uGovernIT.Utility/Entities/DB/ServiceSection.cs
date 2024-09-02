using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ServiceSections)]
    public class ServiceSection:DBBaseEntity
    {

        public long ID { get;  set; }

        public long ServiceID { get; set; }

        public string SectionName { get; set; }

        public string Title { get; set; }

        public int ItemOrder { get; set; }

        public string Description { get; set; }

        public int SectionSequence { get; set; }
        public string IconUrl { get; set; }

        [NotMapped]
        public string ServiceName { get; set; }
    }
}
