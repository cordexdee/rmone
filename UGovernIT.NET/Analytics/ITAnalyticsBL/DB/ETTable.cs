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
    [Table(DatabaseObjects.Tables.Analytic_ETTables)]
    public class ETTable: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ETTableID {get;set;}
        public virtual string TableName { get; set; }
        public virtual string Description { get; set; }
      
        public virtual byte Status { get; set; }
        public virtual DateTime? LastUpdated { get; set; }
        public virtual List<ETSchemaInfo> ETSchemaInfoes { get; set; }
    
    }
}
