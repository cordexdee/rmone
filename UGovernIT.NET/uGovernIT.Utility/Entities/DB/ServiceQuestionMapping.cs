using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ServiceTicketDefaultValues)]
    public class ServiceQuestionMapping:DBBaseEntity
    {

        public long ID { get;  set; }

        public string ColumnName { get; set; }

        public string ColumnValue { get; set; }

        public string PickValueFrom { get; set; }

        public long ServiceID { get; set; }
        [NotMapped]
        public string ServiceName { get; set; }

        public long? ServiceQuestionID { get; set; }
        [NotMapped]
        public string ServiceQuestionName { get; set; }
      
        public long? ServiceTaskID { get; set; }
     
        public string Title { get; set; }

   
    }
}
