using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
     public  class StatusSummary
     {
        public bool IsUserTicketCom = false;
        public long userTicket { get; set; }

        public bool IsticketCom = false;
        public long ticketCount { get; set; }

        public bool IsUserCom = false;
        public long Usercount { get; set; }

        public bool IsRegistrationCom { get; set; }
        

        public bool IsSvcTicketCom = false;
        public long svcTicketcount { get; set; }

        public bool IsDepartmentCom = false;
        public long departmentCount { get; set; }

        public bool IsTitleCom = false;
        public long titleCount { get; set; }

        public bool IsRoleCom = false;
        public long roleCount { get; set; }

        public bool IsProjectCom = false;
        public long projectCount { get; set; }
        public bool IsAllocationCom = false;
        public long allocationCount { get; set; } 


    }
}
