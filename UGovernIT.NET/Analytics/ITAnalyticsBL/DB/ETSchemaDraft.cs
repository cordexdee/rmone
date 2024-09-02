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
    [Table(DatabaseObjects.Tables.Analytic_ETSchemaDrafts)]
    public class ETSchemaDraft : DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ETSchemaDraftID { get; set; }
        public virtual string TableName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string SelectedTables { get; set; }
        public virtual string SelectedSchema { get; set; }

        //public virtual User User { get; set; }
    }
}
