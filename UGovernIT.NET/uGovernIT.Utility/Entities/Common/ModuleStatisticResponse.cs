using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace uGovernIT.Utility
{
    public class ModuleStatisticResponse
    {
        public Dictionary<string, int> TabCounts { get; set; }
        public string CurrentTab { get; set; }
        public int CurrentTabCount { get; set; }
        public DataTable ResultedData { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }

        public ModuleStatisticResponse()
        {
            TabCounts = new Dictionary<string, int>();
            ModuleName = string.Empty;
        }
    }
}
