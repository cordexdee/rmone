using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.Common
{
    public class LoadOptions
    {
        public int skip { get; set; }
        public string sort { get; set; }
        public int take { get; set; }
        public string searchExpr { get; set; }
        public string searchOperation { get; set; }
        public string searchValue { get; set; }
    }
}
