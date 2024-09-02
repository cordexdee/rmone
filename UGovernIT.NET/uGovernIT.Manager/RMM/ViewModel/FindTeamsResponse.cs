using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Utility;

namespace uGovernIT.Manager.RMM
{
    public class FindTeamsResponse
    {
        public string ID { get; set; }
        public int Index { get; set; }
        public string TypeName { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public List<FindResourceResponse> Resources { get; set; }
    }
}
