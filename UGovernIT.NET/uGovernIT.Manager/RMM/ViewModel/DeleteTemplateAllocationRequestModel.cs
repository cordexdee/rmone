using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeleteTemplateAllocationRequestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TicketID { get; set; }
        public int AllocationID { get; set; }
        public bool DeleteTemplate { get; set; }
    }
}
