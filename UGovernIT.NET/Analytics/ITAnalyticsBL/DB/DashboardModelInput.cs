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
    [Table(DatabaseObjects.Tables.Analytic_DashboardModelInputs)]
    public class DashboardModelInput : DBBaseEntity
    {
        [ForeignKey(nameof(ModelInput))]
        public virtual long ModelInputID { get; set; }
        [ForeignKey(nameof(Dashboard))]
        public virtual long DashboardID { get; set; }
        
    }
}
