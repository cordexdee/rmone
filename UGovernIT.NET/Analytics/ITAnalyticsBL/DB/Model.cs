using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_Models)]
    public class Model: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ModelID { get; set; }
        public virtual string Description { get; set; }
        public virtual long ModelCategoryID { get; set; }

        [Required(AllowEmptyStrings=true, ErrorMessage="Please enter Title")]
        public virtual string Title { get; set; }
        public virtual long CurrentActiveVersionID { get; set; }

        public virtual ICollection<ModelVersion> ModelVersions { get; set; }
        public virtual ModelCategory ModelCategory { get; set; }
    }
}
