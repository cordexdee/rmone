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
    [Table(DatabaseObjects.Tables.Analytic_ModelOutputMappers)]
    public class ModelOutputMapper : DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ID { get; set; }
        public virtual long ModelVersionID { get; set; }
        public virtual string Yaxis { get; set; }
        public virtual string Xaxis { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Please enter Title")]
        public virtual string Title { get; set; }
        public virtual string ConfigString { get; set; }
        public virtual int Rows { get; set; }
        public virtual int Columns { get; set; }
        public virtual ModelVersion ModelVersion { get; set; }
        public virtual int Activated { get; set; }
        public virtual string MapperType { get; set; }
      
    }
}
