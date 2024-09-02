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
    [Table(DatabaseObjects.Tables.Analytic_AnalysisDashboards)]
    public class Dashboard: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long DashboardID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter name")]
        public virtual string Name { get; set; }
        
        public virtual string Description { get; set; }
        public virtual long? DashboardGroupID { get; set; }
        public virtual long? ModelVersionID { get; set; }
        public virtual long RelativeInputID { get; set; }
        public virtual bool PromptForRelativeID { get; set; }

        public virtual DashboardGroup DashboardGroup { get; set; }
        public virtual ModelVersion ModelVersion { get; set; }
        public virtual ICollection<DashboardModelInput> DashboardModelInputs { get; set; }

    }
}
