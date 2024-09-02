using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AllocationTemplateResourceModel
    {
        public long TemplateID { get; set; }
        public List<AllocationTemplateModel> Allocations { get; set; }
    }
}
