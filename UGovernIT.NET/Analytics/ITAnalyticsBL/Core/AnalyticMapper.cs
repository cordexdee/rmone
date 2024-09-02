using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Core
{
  public  class AnalyticMapper
    {
      public string Title { get; set; }
      public bool ShowLabel { get; set; }
      public long ModelVersionId { get; set; }
      public string MapperType { get; set; }
      public long OutputMapperId { get; set; }
      public string Shape { get; set; }
      public AnalyticMapper()
      {
          Title = String.Empty;
          MapperType = string.Empty;
          Shape = String.Empty;
      }
    }

  
}
