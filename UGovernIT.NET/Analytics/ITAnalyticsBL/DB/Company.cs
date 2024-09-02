using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAnalyticsBL.DB
{
    public class Company
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [MaxLength(10)]
        public string Zip { get; set; }
        [NotMapped]
        public string ExternalConString { get; set; }
        [NotMapped]
        public string ConString { get; set; }

        [NotMapped]
        [Required]
        [StringLength(20, MinimumLength =6, ErrorMessage ="Minimum length 6 and maximum length : 20")]
        public string Password { get; set; }

        [NotMapped]
        public bool Disabled { get; set; }
    }
    public class EditCompany
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [MaxLength(10)]
        public string Zip { get; set; }
        [NotMapped]
        public string ExternalConString { get; set; }
        [NotMapped]
        public string ConString { get; set; }

       
        public bool Disabled { get; set; }
    }
}
