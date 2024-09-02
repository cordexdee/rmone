using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace ITAnalyticsBL.Core
{
   public  class OutputMapper :AnalyticMapper
    {      
       public OutputMapperAxis XAxis { get; set; }
       public OutputMapperAxis YAxis { get; set; }      
       public int Rows { get; set; }
       public int Columns { get; set; }
       public List<OutputMapperTile> TileList { get; set; }
    
      
       public OutputMapper()
       {
           XAxis = new OutputMapperAxis("xaxis");
           YAxis = new OutputMapperAxis("yaxis");
           Title = string.Empty;
           ShowLabel = true;
           TileList = new List<OutputMapperTile>();
       }
       
    }
}
