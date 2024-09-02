using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleColumnsStore : StoreBase<Utility.ModuleColumn>, IModuleColumnsStore
    {
        public ModuleColumnsStore(CustomDbContext context) : base(context)
        {

        }
        public List<Utility.ModuleColumn> LoadByModule(string moduleName)
        {
            List<Utility.ModuleColumn> objModuleColumns = new List<Utility.ModuleColumn>();
            objModuleColumns = this.Load(string.Format("Where {0} = '{1}'", DatabaseObjects.Columns.CategoryName, moduleName));// moduleColumns.GetData().ToList();
            return objModuleColumns;
        }
    }
    public interface IModuleColumnsStore : IStore<Utility.ModuleColumn>
    {
        List<Utility.ModuleColumn> LoadByModule(string moduleName);
    }
}
