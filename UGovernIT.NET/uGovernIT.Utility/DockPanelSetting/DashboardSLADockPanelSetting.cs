using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.DockPanels
{
    public class DashboardSLADockPanelSetting : DockPanelSetting
    {
        public string ContentTitle { get; set; }
        public string Module { get; set; }
        public string FilterView { get; set; }
        public bool IncludeOpen { get; set; }
        public bool ShowSLAName { get; set; }
        public string SLAEnableModules { get; set; }
        public string StringOfSelectedModule { get; set; }
        public string LegendSetting { get; set; }        
    }
}
