using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public  class InterpretationView
    {
        public  string Title { get; set; }
        public  string Description { get; set; }
        public  string InterpretationText { get; set; }
        public List<string> RunNameList { get; set; }

       public InterpretationView()
       {
           this.Title = String.Empty;
           this.Description = String.Empty;
           this.InterpretationText = string.Empty;
           this.RunNameList = new List<string>();
       }
    }
   
}
