using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class SurveyRun: DBBaseEntity
    {
        [Key]
        public virtual long SurveyRunId { get; set; }
        public virtual long SurveyID { get; set; } 
        public virtual string SurveyRunName { get; set; }
        public virtual DateTime CreatedOn { get; set; }
    }
}
