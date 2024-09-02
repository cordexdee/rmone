using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_Interpretations)]
    public class Interpretation: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long InterpretationId { get; set; }
        public virtual long ModelVersionID { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Please enter Title")]
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string InterpretationText { get; set; }
        public virtual string Expression { get; set; }
        public virtual int Activated { get; set; }
        public virtual string Scope { get; set; }
        public virtual string ScopeId { get; set; }
        public virtual string ExpressionXml { get; set; }

    }
}
