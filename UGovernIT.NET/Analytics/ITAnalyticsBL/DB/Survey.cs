using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using uGovernIT.Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAnalyticsBL.DB
{
    public class Survey: DBBaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual long SurveyID { get; set; }
        public virtual string Description { get; set; }


        [Required(AllowEmptyStrings = true, ErrorMessage = "Please Enter Name")]
        public virtual string Name { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual string Category { get; set; }
        public virtual string Owner { get; set; }
        public virtual string AuthorizedToRun { get; set; }
        public virtual bool? RequireOwnerApproval { get; set; }
        public virtual int? OrderNo { get; set; }
        public virtual string AttachmentRequired { get; set; }
        public virtual string WikiHelp { get; set; }
     
    }
}
