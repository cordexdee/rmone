using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class LifeCycleManager:ManagerBase<LifeCycle>, ILifeCycleHelper
    {
       
        public LifeCycleManager(ApplicationContext context):base(context)
        {
            store = new LifeCycleStore(this.dbContext);
        }

        public List<LifeCycle> LoadLifeCycleByModule(string moduleName)
        {
            List<LifeCycle> lcs = new List<LifeCycle>();
            LifeCycle lc = new LifeCycle();
            lcs = (store as LifeCycleStore).LoadByModule(moduleName);

            return lcs;
        }        

        /// <summary>
        /// Fetch all lifecycles (archive and active both)
        /// </summary>
        /// <returns></returns>
        public List<LifeCycle> LoadProjectLifeCycles()
        {
            List<LifeCycle> lcs = new List<LifeCycle>();
            LifeCycle lc = new LifeCycle();
            lcs = (store as LifeCycleStore).LoadLifeCyleForPMM();
            return lcs;
        }

        /// <summary>
        /// Fetch archive or unarchived lifecycles
        /// </summary>
        /// <returns></returns>
        public List<LifeCycle> LoadProjectLifeCycles(bool archived)
        {
            List<LifeCycle> lcs = new List<LifeCycle>();
            
            lcs = (store as LifeCycleStore).LoadLifeCyleForPMM();
            lcs = lcs.Where(x => x.Deleted == archived).ToList();
            return lcs;
        }

        private List<LifeCycleStage> LoadStage(List<LifeCycleStage> items)
        {
            foreach (LifeCycleStage item in items)
            {
                item.ApprovedStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageApprovedStatus) == x.StageStep);
                item.ReturnStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageReturnStatus) == x.StageStep);
                item.RejectStage = items.FirstOrDefault(x => UGITUtility.StringToInt(item.StageRejectedStatus) == x.StageStep);
                //LoadStageItem(item);
            }
            return items;
        }

        public LifeCycle LoadProjectLifeCycleByName(string lifeCycle)
        {
            LifeCycle lifeCycleCtr = new LifeCycle();
            List<LifeCycle> lcs = new List<LifeCycle>();
            lcs = (store as LifeCycleStore).LoadByModule(ModuleNames.PMM);
            lifeCycleCtr = lcs.FirstOrDefault(x => x.Name == lifeCycle);
            return lifeCycleCtr;
        }
    }
    public interface ILifeCycleHelper : IManagerBase<LifeCycle>
    {
        List<LifeCycle> LoadLifeCycleByModule(string moduleName);
    }
}
