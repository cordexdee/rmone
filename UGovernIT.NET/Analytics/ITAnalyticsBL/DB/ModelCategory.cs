using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;
using System.ComponentModel.DataAnnotations;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_ModelCategories)]
    public class ModelCategory: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ModelCategoryID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<Model> Models { get; set; }
       
    }
}
