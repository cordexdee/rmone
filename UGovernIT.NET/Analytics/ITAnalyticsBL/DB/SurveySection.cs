using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using uGovernIT.Utility;
using System.ComponentModel.DataAnnotations;

namespace ITAnalyticsBL.DB
{
    public class SurveySection: DBBaseEntity
    {
        [Key]
        public virtual long SurveySectionID { get; set; }
        public virtual string SurveySectionName { get; set; }
    }
}
