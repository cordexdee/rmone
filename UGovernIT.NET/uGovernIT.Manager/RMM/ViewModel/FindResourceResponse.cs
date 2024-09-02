using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Manager.RMM.ViewModel;
using uGovernIT.Utility;

namespace uGovernIT.Manager.RMM
{
    public class FindResourceResponse
    {
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public double? TotalPctAllocation { get; set; }
        public double? PctAllocation { get; set; } //Hard allocation
        public double? SoftPctAllocation { get; set; }
        public string GroupID { get; set; }
        public string CellClass { get; set; }
        public int AllocationRange { get; set; }
        public int HighestComplexity { get; set; }

        public string TotalVolume { get; set; }
        public int TotalVolumeRange { get; set; }
        public int ProjectCount { get; set; }
        public int projectCountRange { get; set; }
        public string JobTitle { get; set; }
        public string RoleName { get; set; }
        public string UserImagePath { get; set; }
        public List<ResourceTag> ResourceTags { get; set; }
        public List<ResourceWeekWiseAvailabilityModel> WeekWiseAllocations { set; get; }
        //public FindResourceResponse()
        //{
        //    CellSettings = new CellSetting();
        //}
    }

    public class CellSetting
    {
        public string CellFontColor { get; set; }
        public string CellBgColor { get; set; }
        public string CellFontSize { get; set; }
    }
    public class ResourceTag
    {
        public string TagId { get; set; }
        public string TagCount { get; set; }
        public TagType Type { get; set; }
    }
    public class ResourceWeekWiseAvailabilityResponse
    {
        public string ID { get; set; }
        public string UserId { get; set; }
        public double AverageUtilization { get; set; }
        public double PostAverageUtilization { get; set; }
        public Availability AvailabilityType { get; set; }
        public List<ResourceWeekWiseAvailabilityModel> WeekWiseAllocations { set; get; }
    }
    public class ResourceWeekWiseAvailabilityModel
    {
        public DateTime WeekStartDate { get; set; }
        public double PctAllocation { get; set; }
        public double PostPctAllocation { get; set; }
        public double PctAvailability { get; set; }
        public double NoOfDays { get; set; }
        public bool IsAvailable { get; set; }
        public List<WeekdetailedSummary> WeekdetailedSummaries { set; get; }
    }

    public class WeekdetailedSummary
    {
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
    }
}
