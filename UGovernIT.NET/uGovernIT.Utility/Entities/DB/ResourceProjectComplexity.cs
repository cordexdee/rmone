using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Serializable]
    [Table(DatabaseObjects.Tables.ResourceProjectComplexity)]
    public class SummaryResourceProjectComplexity : DBBaseEntity
    {
        public long ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long DepartmentID { get; set; }
        public long? FunctionalAreaID { get; set; }
        [Column(DatabaseObjects.Columns.Manager)]
        public string Manager { get; set; }
        public int Complexity { get; set; }
        public int Count { get; set; }
        public double HighProjectCapacity { get; set; }
        public string RequestTypes { get; set; }
        public string ModuleNameLookup { get; set; }
        [NotMapped]
        public string ComplexityText { get; set; } = "Complexity Level";
        //Change by Hareram
        public string GroupID { get; set; }

        // Added to maintain both Open and Closed Projects data
        public int AllCount { get; set; }
        public double AllHighProjectCapacity { get; set; }
    }
}
