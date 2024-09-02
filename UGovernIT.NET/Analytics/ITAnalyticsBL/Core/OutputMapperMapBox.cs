using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
   public class OutputMapperMapBox
    {
       public Guid ID { get; set; }
       public Guid XAxisLabelID { get; set; }
       public Guid YAxisLabelID { get; set; }
       public string BgColor { get; set; }
       public string LabelText { get; set; }
       public string LabelColor { get; set; }
       public string LabelPosition { get; set; }
    }
}
