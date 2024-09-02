using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernIT.Utility
{
    [JsonObject(MemberSerialization.OptOut)]
    public class MonitorState
    {
        public long Id { get; set; }
        public string title { get; set; }
        public string LEDClass { get; set; }
        public string SelectedOption { get; set; }
        public long SelectedOptionId { get; set; }
        public float MonitorStateScore { get; set; }
        public float Overallscore { get; set; }
        public int Weight { get; set; }
        public string ModuleMonitorName { get; set; }
    }
}
