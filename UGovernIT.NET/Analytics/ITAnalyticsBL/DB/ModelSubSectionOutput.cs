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
    [Table(DatabaseObjects.Tables.Analytic_ModelSubSectionOutputs)]
    public class ModelSubSectionOutput : DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ModelSubSectionOutputID { get; set; }

        //Foreign Key
        public virtual long ModelVersionID { get; set; }
        public virtual long ModelInputID { get; set; }
        public virtual long? ModelOutputID { get; set; }
        public virtual long? ModelSectionOutputID { get; set; }

        public virtual string SubSectionInternalID { get; set; }
        public virtual string SubSectionName { get; set; }
        public virtual double RowScore { get; set; }
        public virtual double WeightedScore { get; set; }
        public virtual double CummulativeScore { get; set; }
        public virtual double Weight { get; set; }
        public virtual double CummulativeWeight { get; set; }
       
        public virtual ModelVersion ModelVersion { get; set; }
        public virtual ModelInput ModelInput { get; set; }
        public virtual ModelOutput ModelOutput { get; set; }
        public virtual ModelSectionOutput ModelSectionOutput { get; set; }
        public virtual ICollection<ModelFeatureOutput> ModelFeatureOutputs { get; set; }
   
    }
}
