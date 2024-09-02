using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.Tenant)]
    public class Tenant
    {
        public Guid TenantID { get; set; }
        [Required(ErrorMessage = "Tenant Name is required")]

        public string TenantName { get; set; }

        [Required(ErrorMessage = "Account Id is required")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Account Id can have alphabets, numbers and underscore only")]
        public string AccountID { get; set; }

        public string Country { get; set; }

        public string TenantUrl { get; set; }

        public string DBName { get; set; }

        public string DBServer { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string CreatedByUser { get; set; }

        public string ModifiedByUser { get; set; }

        public bool Deleted { get; set; }

        public bool IsOffice365Subscription { get; set; }

        public bool? SelfRegisteredTenant {get; set;}

        public string Name { get; set; }

        public string Title { get; set; }

        public string Contact { get; set; }

        public int? Subscription { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}

