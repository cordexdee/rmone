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
    public class SlaRulesManager: ManagerBase<ModuleSLARule>, ISlaRulesManager
    {
        public SlaRulesManager(ApplicationContext context):base(context)
        {
            store = new SlaRuleStore(this.dbContext);
        }
        public  ModuleSLARule AddUpdateSLARule(ModuleSLARule relation)
        {
            if (relation != null)
            {
                if (relation.ID > 0)
                {
                    store.Update(relation);
                }
                else
                {
                    store.Insert(relation);
                }
            }
            else
            {
                relation = null;
            }
            return relation;
        }
    }
    public interface ISlaRulesManager : IManagerBase<ModuleSLARule>
    {

    }
}
