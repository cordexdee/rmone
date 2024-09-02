using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;
using System.Data;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class ModuleColumnManager:ManagerBase<Utility.ModuleColumn>, IModuleColumnManager
    {
        public ModuleColumnManager(ApplicationContext context):base(context)
        {
            store = new DAL.Store.ModuleColumnsStore(this.dbContext);
        }

        public List<Utility.ModuleColumn> GetModuleColumns()
        {
            return Load();
        }


        public DataTable GetModuleColumnDataTable()
        {
           return GetDataTable();
        }

        public DataTable GetModuleColumnDataTable(string where)
        {
            return GetDataTable(where);
        }
    }
    public interface IModuleColumnManager : IManagerBase<Utility.ModuleColumn>
    {
        DataTable GetModuleColumnDataTable();

    }
}
