using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class RoleTask: DBBaseEntity
    {
        [Key]
        [Column(Order = 0)]
        public virtual long RoleID { get; set; }
        [Key]
        [Column(Order = 1)]
        public virtual int TaskID { get; set; }
        public virtual string Role_ID { get; set; }
        //[NotMapped]
        //public virtual Role Role { get; set; }
        
        public virtual Task Task { get; set; }
        [Column(Order = 2)]
        public long CompanyID { get; set; }
    }
    
}
