using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_Questions)]
    public class Question: DBBaseEntity
    {
        [Key]
        public virtual long QuestionId { get; set; }
        public virtual long SurveyId { get; set; }
        public virtual string QuestionType { get; set; }
        public virtual string QuestionDesc { get; set; }
        public virtual string Section { get; set; }
        public virtual int ItemOrder { get; set; }
        public virtual string Token { get; set; }
        public virtual string QuestionTypeProperties { get; set; }
        public virtual bool Mandatory { get; set; }
        public virtual string HelpText { get; set; }
        [NotMapped]
        public string SectionName { get; set; }
       

    }
}
