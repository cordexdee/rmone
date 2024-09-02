using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using System.Data;

namespace uGovernIT.Manager
{
    public class ModuleStageConstraintsManager:ManagerBase<ModuleStageConstraints>, IModuleStageConstraintsManager
    {
        public ModuleStageConstraintsManager(ApplicationContext context):base(context)
        {
            store = new ModuleStageConstraintsStore(this.dbContext);
        }

        public DataTable GetAllStageConstraints()
        {
            DataTable dtAllStageConstraints = null;

            List<ModuleStageConstraints> stageConstraints = new List<ModuleStageConstraints>();
            stageConstraints = Load(x => !x.Deleted && !string.IsNullOrEmpty(x.TicketId));

            dtAllStageConstraints = UGITUtility.ToDataTable<ModuleStageConstraints>(stageConstraints);
            return dtAllStageConstraints;
        }
        public List<ModuleStageConstraints> GetAllStageConstraintsList()
        {
            List<ModuleStageConstraints> stageConstraints = new List<ModuleStageConstraints>();
            stageConstraints = Load(x => !x.Deleted && !string.IsNullOrEmpty(x.TicketId));
            return stageConstraints;
        }

        public DataTable GetOpenStageConstraints()
        {
            DataTable dtStageConstraints = null;

            List<ModuleStageConstraints> stageConstraints = new List<ModuleStageConstraints>();
            stageConstraints = Load(x => !x.Deleted && !string.IsNullOrEmpty(x.TicketId) && x.TaskStatus != "Completed");

            dtStageConstraints = UGITUtility.ToDataTable<ModuleStageConstraints>(stageConstraints);
            return dtStageConstraints;
        }

        public DataTable GetCompletedStageConstraints()
        {
            DataTable dtStageConstraints = null;

            List<ModuleStageConstraints> stageConstraints = new List<ModuleStageConstraints>();
            stageConstraints = Load(x => !x.Deleted && !string.IsNullOrEmpty(x.TicketId) && x.TaskStatus == "Completed");

            dtStageConstraints = UGITUtility.ToDataTable<ModuleStageConstraints>(stageConstraints);
            return dtStageConstraints;
        }

    }
    public interface IModuleStageConstraintsManager : IManagerBase<ModuleStageConstraints>
    {

    }
}
