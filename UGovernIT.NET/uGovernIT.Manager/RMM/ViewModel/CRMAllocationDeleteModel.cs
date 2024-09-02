using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AllocationDeleteModel
    {
        public long ID { get; set; }
        public string TicketID { get; set; }
        public string UserID { get; set; }
        public string Tags { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
