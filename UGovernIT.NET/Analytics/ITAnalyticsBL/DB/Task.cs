using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class Task : DBBaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int TaskID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int TaskGroupID { get; set; }
        [NotMapped]
        public virtual bool IsAssigned { get; set; }
        public virtual TaskGroup TaskGroup { get; set; }
        public virtual ICollection<RoleTask> RoleTasks { get; set; }
    }
}
