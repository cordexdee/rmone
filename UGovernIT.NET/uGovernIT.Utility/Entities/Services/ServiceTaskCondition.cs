using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class ServiceTaskCondition
    {

        public Guid ID { get; set; }

        public string Title { get; set; }

        public string Condition { get; set; }

        public List<WhereExpression> Conditions { get; set; }

        public List<long> SkipTasks { get; set; }

        public ServiceTaskCondition()
        {
            ID = Guid.NewGuid();
            Title = string.Empty;
            Condition = string.Empty;
            SkipTasks = new List<long>();
            Conditions = new List<WhereExpression>();
        }
    }
}
