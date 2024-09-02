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
    [Table(DatabaseObjects.Tables.Analytic_DashboardGroups)]
    public class DashboardGroup : DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long DashboardGroupID { get; set; }
        [Required(AllowEmptyStrings=false, ErrorMessage="Please specify Name")]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
       
        public virtual ICollection<Dashboard> Dashboards { get; set; }
    }
}
