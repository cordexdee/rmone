using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;using System.ComponentModel.DataAnnotations.Schema;

namespace uGovernIT.Utility
{
    [Table(DatabaseObjects.Tables.ServiceQuestions)]
    public class ServiceQuestion:DBBaseEntity
    {

        public long ID { get; set; }

        public long? ServiceSectionID { get; set; }
        [NotMapped]
        public string ServiceSectionName { get; set; }

        public long ServiceID { get; set; }
        [NotMapped]
        public string ServiceName { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionType { get; set; }

        [NotMapped]
        public Dictionary<string, string> QuestionTypePropertiesDicObj { get; set; }
        public string QuestionTypeProperties { get; set; }

        public string Helptext { get; set; }
        public string NavigationUrl { get; set; }
        public string NavigationType { get; set; }

        public int ItemOrder { get; set; }

        public string TokenName { get; set; }
        public bool FieldMandatory { get; set; }
        [NotMapped]
        public bool EnableZoomIn { get; set; }
        public bool? ContinueSameLine { get; set; } = false;
        public ServiceQuestion()
        {
            QuestionTitle = string.Empty;
            ServiceSectionName = string.Empty;
            Helptext = string.Empty;
            TokenName = string.Empty;
            QuestionType = string.Empty;

            QuestionTypePropertiesDicObj = new Dictionary<string, string>();

        }

        public ServiceQuestion(int serviceID)
            : base()
        {
            ServiceID = serviceID;

        }
    }
}
                              
