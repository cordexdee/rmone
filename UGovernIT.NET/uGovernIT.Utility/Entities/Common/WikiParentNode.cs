using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
   public class WikiParentNode
    {
        public string text { get; set; }
        public List<WikiTree> items { get; set; }
        public bool expanded { get; set; }
        public WikiParentNode()
        {
            items = new List<WikiTree>();
        }
    }
}
