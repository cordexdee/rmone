using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    public class SurveyResult: DBBaseEntity
    {
        public virtual long SurveyResultID { get; set; }
        public virtual long SurveyID { get; set; }
        public virtual long RunID { get; set; }
        [NotMapped]
        public virtual string RunName { get; set; }
        public virtual long QuestionID { get; set; }       
        public virtual string AnswerType { get; set; }
        public virtual long AnswerRowID { get; set; }
        public virtual string Answer { get; set; }
        public virtual DateTime CreatedOn { get; set; }
    }
}
