using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Manager.RMM.ViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class VMSaveAllocationAsTemplate
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string TicketID { get; set; }
        public string ModuleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PreconStartDate { get; set; }
        public DateTime PreconEndDate { get; set; }
        public DateTime ConstStartDate { get; set; }
        public DateTime ConstEndDate { get; set; }
        public DateTime CloseOutStartDate { get; set; }
        public DateTime CloseOutEndDate { get; set; }
        public int Duration { get; set; }
        public bool SaveOnExiting { get; set; }
        public string Allocations { get; set; }
    }
}
