using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uGovernIT.Web.Models
{
    public class RequesetTypeDependentModel
    {
        public long ID { get; set; }
        public string RequestType { get; set; }
       
        public string OwnerID { get; set; }
        public string Owner { get; set; }
        public string Category { get; set; }
        public string Workflowtype { get; set; }
        public string PRPGroup { get; set; }
        public string PRPGroupID { get; set; }
        public string ORP { get; set; }
        public string ORPID { get; set; }
        public string FunctionalArea { get; set; }
        public long? FunctionalAreaLookup { get; set; }

        public double EstimatedHours { get; set; }
        public string SubCategory { get; set; }
        public long LocationID { get; set; }
    }
}