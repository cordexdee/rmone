using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class CommonTicketResponse 
    {
        public bool Status { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Data { get; set; }
    }

    
    
    public class NewTicketFieldViewModel
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
        public bool IsExcluded { get; set; }
    }

    public class UpdateTicketViewModel
    {
        public string RecordId { get; set; }
        public bool UpdateAllocations { get; set; }
        public bool UpdatePastAllocations { get; set; }
        public List<NewTicketFieldViewModel> Fields { get; set; }
    }

    public class ExternalTeamViewModel
    {
        public string CRMCompanyLookup { get; set; }
        public List<string> ContactLookup { get; set; }
        public string RelationshipTypeLookup { get; set; }
    }
}