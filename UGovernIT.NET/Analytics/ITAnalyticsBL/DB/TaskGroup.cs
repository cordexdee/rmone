using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class TaskGroup : DBBaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int TaskGroupID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
