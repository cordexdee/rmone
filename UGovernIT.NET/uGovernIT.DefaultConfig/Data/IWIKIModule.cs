using System.Collections.Generic;
using uGovernIT.Utility;

namespace uGovernIT.DefaultConfig
{
    public interface IWIKIModule : IModule
    {
        List<WikiLeftNavigation> GetWikiLeftNavigation();
    }
}
