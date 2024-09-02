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
    [Table(DatabaseObjects.Tables.Analytic_ETSchemaInfoes)]
    public class ETSchemaInfo : DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ETSchemaInfoID { get; set; }
        public virtual string AliasName { get; set; }
        public virtual long ETTableID { get; set; }
        public virtual string FieldName { get; set; }
        public virtual string SourceTable { get; set; }
        public virtual string ForeignKey { get; set; }
        public virtual long DataIntegrationID { get; set; }
        public virtual Boolean PrimaryKey { get; set; }
        public virtual string FieldConstraint { get; set; }
        public virtual string DataType { get; set; }
        public virtual string AggregateFunction { get; set; }
        public virtual ETTable ETTable { get; set; }
    }
}
