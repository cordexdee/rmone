using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    public class PotentialAllocationsRequest
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IncludeSoftAllocation { get; set; }
        public List<ProjectAllocationModel> Allocations { get; set; }
    }
    public class PotentialAllocationsResponse
    {
        public DataTable dtUnfilledAllocations { get; set; }
        public DataTable dtDefaultDivisionRole { get; set; }
        public List<ResourceWeekWiseAvailabilityResponse> lstResourceWeekWiseAvailability { get; set; }
    }

}
