using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace uGovernIT.Utility
{
    public class ApplicationRegistrationRequest
    {
        [EmailAddress(ErrorMessage = "Enter valid Email ID")]
        [Required(ErrorMessage = "Email ID is required")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Ticket ID is required")]
        public string TicketID { get; set; }
    }
}
