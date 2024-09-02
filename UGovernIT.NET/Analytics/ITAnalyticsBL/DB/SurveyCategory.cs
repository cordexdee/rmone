using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class SurveyCategory: DBBaseEntity
    {
        public virtual int SurveyCategoryID { get; set; }
        public virtual string SurveyCategoryName { get; set; }
     
    }
}
