using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace ITAnalyticsBL.DB
{
    public class DataIntegration
    {
        public virtual int DataIntegrationID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string ConnectionString { get; set; }
        public virtual string ListName { get; set; }
        public virtual string FieldName { get; set; }
        public virtual short SourceType { get; set; }
        public virtual string PublicKey { get; set; }
        public virtual bool Active { get; set; }
        public virtual ApplicationContext Context { get; set; }
    }
}
