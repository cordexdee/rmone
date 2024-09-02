using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
   public class FormLayoutStore:StoreBase<ModuleFormTab>, IFormLayoutStore
    {
        public FormLayoutStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IFormLayoutStore : IStore<ModuleFormTab>
    { }
}
