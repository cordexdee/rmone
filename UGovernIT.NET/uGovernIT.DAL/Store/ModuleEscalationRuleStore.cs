using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleEscalationRuleStore : StoreBase<ModuleEscalationRule>
    {
        public ModuleEscalationRuleStore(CustomDbContext context) : base(context)
        {

        }
        public ModuleEscalationRule AddUpdateSLARule(ModuleEscalationRule relation)
        {
            if (relation != null)
            {
                if (relation.ID > 0)
                    this.Update(relation);
                else
                    this.Insert(relation);
            }
            else
            {
                relation = null;
            }
            return relation;
        }

    }
    public interface IModuleEscalationRuleStore : IStore<ModuleEscalationRule>
    {
        ModuleEscalationRule AddUpdateSLARule(ModuleEscalationRule relation);
    }
}
