using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig
{
    public interface IPMMModule : IModule
    {
        List<ProjectClass> GetProjectClass();
        List<ProjectInitiative> GetProjectInitiative();
        List<ModuleLifeCycle> GetProjectLifeCycles();
        List<ProjectLifecycleStages> GetProjectLifecycleStages();
        List<EventCategory> GetEventCategories();
    }
}
