using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Helpers
{
    [DataContract]
    public class ServiceTask
    {
        [DataMember]
        public string QuestionID { get; set; }

        [DataMember]
        public string QuestionProperties { get; set; }

        [DataMember]
        public string UGITSubTaskType { get; set; }
        [DataMember]
        public bool AutoCreateUser { get; set; }

        [DataMember]
        public bool AutoFillRequestor { get; set; }

        [DataMember]
        public string PickUserFrom { get; set; }

        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public LookupValue Predecessors { get; set; }
        [DataMember]
        public LookupValue RequestCategory { get; set; }
        [DataMember]
        public LookupValue Service { get; set; }

        [DataMember]
        public LookupValue Module { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public int ItemOrder { get; set; }

        [DataMember]
        public List<LookupValue> AssignedTo { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public double EstimatedHours { get; set; }

        [DataMember]
        public int ParentTask { get; set; }

        [DataMember]
        public int TaskLevel { get; set; }
        public string Author { get; set; }
        public string ModifiedBy { get; set; }

        [DataMember]
        public bool EnableApproval { get; set; }
        [DataMember]

        public List<LookupValue> Approver { get; set; }

        [DataMember]
        public string Approvalstatus { get; set; }

        [DataMember]
        public string ApprovalType { get; set; }

        [DataMember]
        public string TaskActionUser { get; set; }
        [DataMember]
        public bool UseADAuthentication { get; set; }
        [DataMember]
        public bool SLADisabled { get; set; }
        [DataMember]
        public bool NotificationDisabled { get; set; }
    }
}
