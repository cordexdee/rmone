using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Utility;

namespace uGovernIT.Manager.RMM
{
    public class FindResourceRequest
    {
        public int ID { get; set; }
        public string ModuleName { get; set; }
        public string ProjectID { get; set; }
        public string GroupID { get; set; }
        public DateTime AllocationStartDate { get; set; }
        public DateTime AllocationEndDate { get; set; }
        public ResourceAvailabilityType ResourceAvailability { get; set; }
        public bool Complexity { get; set; }
        public bool ProjectVolume { get; set; }
        public bool ProjectCount { get; set; }
        public string Type { get; set; }
        public double PctAllocation { get; set; }
        public bool RequestTypes { get; set; }
        public bool ModuleIncludes { get; set; }    
        public string JobTitles { get; set; }
        public long departments { get; set; }
        public string SelectedUserID { get; set; }
        public bool isAllocationView { get; set; }
        public bool Customer { get; set; }
        public string CompanyLookup { get; set; }
        public bool Sector { get; set; }
        public bool IsRequestFromSummaryView { get; set; }
        public string SectorName { get; set; }
        public List<ProjectTag> SelectedTags { get; set; }
        public double PctAllocationCloseOut { get;set; }
        public double PctAllocationConst { get; set; }
        public string DivisionId { get; set; }
        public string DepartmentId { get; set; }
        public string FunctionId { get; set; }
        public string ResourceManager { get; set; }
        public string SelectedCertifications { get; set; }
        public List<AllocationData> Allocations { get; set; } 
    }

    public class AllocationData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double RequiredPctAllocation { get; set; }
    }
}
