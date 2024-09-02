using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ServiceUpdates_Master)]
    public class ServiceUpdates_Master: DBBaseEntity
    {
        public long ID { get; set; }

        public string  Version { get; set; }

        public long ServiceId { get; set; }

        public string ServiceType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string  ServiceInfo { get; set; }

        public bool AvailableForUpdate { get; set; }

        

    }
}
