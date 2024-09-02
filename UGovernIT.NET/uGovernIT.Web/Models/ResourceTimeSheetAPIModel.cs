using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class ResourceTimeSheetAPIModel
    {
       public string timeSheetData { get; set; }
        public string startDate { get; set; }
        public string userID { get; set; }
        public string currentUserID { get; set; }
    }
}