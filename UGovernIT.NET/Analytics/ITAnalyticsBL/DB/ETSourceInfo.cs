using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
   public class ETSourceInfo: DBBaseEntity
    {
        public virtual int ETSourceInfoID {get;set;}
        public virtual string Source {get;set;}
        public virtual string UserName {get;set;}
        public virtual string Password {get;set;}
        public virtual string DBName  {get;set;}
        public virtual int SourceType { get; set;}
       
    }
}
