using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility
{
    public class EditUrlEntity
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Mapped passed parameter to actual column
        /// </summary>
        public Dictionary<string, string> ParamMapping { get; set; }
    }
}
