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
    [Table(DatabaseObjects.Tables.Analytic_ModelVersions)]
    public class ModelVersion: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long ModelVersionID { get; set; }
        public virtual string ModelXml { get; set; }
        public virtual string HelpXml { get; set; }
        public virtual string ScoreSegmentXml { get; set; }
        public virtual string Comment { get; set; }
        public virtual byte Status { get; set; }
        public virtual long ModelID { get; set; }
        public virtual long ParentID { get; set; }
        public virtual Guid RevisionID { get; set; }
        public virtual int VersionNumber { get; set; }
        public virtual byte ScoreType { get; set; }

        public virtual Model Model { get; set; }
        public virtual ICollection<ModelInput> ModelInputs { get; set; }
        public virtual ICollection<ModelOutputMapper> ModelOutputMappers { get; set; }
        public virtual ICollection<Interpretation> ModelInterpretations { get; set; }
       
    }
}
