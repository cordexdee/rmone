using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    public class JobTitle : DBBaseEntity
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public long? DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string JobType { get; set; }
        public int LowProjectCapacity { get; set; }
        public int HighProjectCapacity { get; set; }
        public double LowRevenueCapacity { get; set; }
        public double HighRevenueCapacity { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        public double? BillingLaborRate { get; set; }
        public double? EmployeeCostRate { get; set; }
        public int? ResourceLevelTolerance { get; set; }
        [NotMapped]
        public string DepartmentDescription { get; set; }
    }
}
