using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
   public class ChartEntity
    {
        public string ChartUrl { get; set; }

        public string Title { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public List<int> Row { get; set; }
        public int Order { get; set; }
        public bool IsNewLine { get; set; }
        public int ViewId { get; set; }
    }
}
