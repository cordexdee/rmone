using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_ModelInputs)]
    public class ModelInput: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ModelInputID { get; set; }
        public virtual string InputXml { get; set; }
        public virtual long ModelVersionID { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

        public virtual ModelVersion ModelVersion { get; set; }
        public virtual ICollection<DashboardModelInput> DashboardModelInputs { get; set; }
    }
}
