using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class ProjectInitiativeViewManager:ManagerBase<ProjectInitiative>
    {
        public ProjectInitiativeViewManager(ApplicationContext context):base(context)
        {

        }

    }
    public interface IProjectInitiativeViewManager : IManagerBase<ProjectInitiative>
    {

    }
}
