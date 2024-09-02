using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Integration
{
    public class ListDetail
    {
        public string ListName{get;set;}
        public List<FieldDetail> Fields { get; set; }
        public string ListId { get; set; }
    }
}
