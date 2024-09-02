using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace uGovernIT.Web.Models
{
    public class LoginModel
    {
        public string AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}