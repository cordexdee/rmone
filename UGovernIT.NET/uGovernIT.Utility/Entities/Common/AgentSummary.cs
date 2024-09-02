using System;
using System.Collections.Generic;


namespace uGovernIT.Utility.Entities.Common
{
        public class AgentSummary
    {
        public string AgentName { get; set; }
        public string AgentType { get; set; }
        public string ModuleName { get; set; }
        public String StageSteps { get; set; }
        public bool IsAgentActivated { get; set; } = false;
        public List<LifeCycleStage> Stages { get; set; }

        public AgentSummary()
        {
            Stages = new List<LifeCycleStage>();
        }


    }
}

