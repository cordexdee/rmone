using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class UserHistory
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string key { get; set; }

        public string Resume { get; set; }
        public string UserDatailsControl { get; set; }
        public string Picture { get; set; }
        public string UserGlobalRole { get; set; }
        public string UserProjectCount { get; set; } = "0";
        public string UserFilteredProjectCount { get; set; } = "0";
        public string UserHardAllocPer { get; set; } = "0";
        public string UserSoftAllocPer { get; set; } = "0";
        public string Roles { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }


        public List<string> Certificates { get; set; }
        public List<UserHistoryDetail> items = new List<UserHistoryDetail>();
        
    }

    public class UserHistoryDetail
    {
        public string TicketId { get; set; }
        public string ProjectTitle { get; set; }
        public string ContractValue { get; set; }        
        public string CompanyName { get; set; }
        public string DisplayCompanyName { get; set; }
        public string ProjectType { get; set; }       
        public string Description { get; set; }
        public string PreconStartDate { get; set; }
        public string PreconEndDate { get; set; }
        public string Url { get; set; }
        public bool IsEmpty { get; set; }
        public string RoleName { get; set; }
        public int Duration { get; set; }
        public string ProjectComplexityChoice { get; set; }
    }


    public class UserHistoryResponse
    {
        //public string Key { get; set; }
        //public List<> ProjectTitle { get; set; }
        //public string ContractValue { get; set; }
        //public string CompanyName { get; set; }
        //public string ProjectType { get; set; }
        //public string Description { get; set; }
        //public string PreconStartDate { get; set; }
        //public string PreconEndDate { get; set; }
    }
}
