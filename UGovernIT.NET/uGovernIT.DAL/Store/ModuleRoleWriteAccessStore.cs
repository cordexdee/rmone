using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ModuleRoleWriteAccessStore : StoreBase<ModuleRoleWriteAccess>, IModuleRoleWriteAccessStore
    {
        public ModuleRoleWriteAccessStore(CustomDbContext context)
            : base(context)
        {
        }

        public List<ModuleRoleWriteAccess> LoadByModule(string moduleName)
        {
            List<ModuleRoleWriteAccess> objRoleWriteAccess = new List<ModuleRoleWriteAccess>();
                objRoleWriteAccess = this.Load(string.Format("Where ModuleNameLookup='{0}'", moduleName)); // moduleRoleWriteAccess.GetData().ToList();

            return objRoleWriteAccess;
        }

        //public List<ModuleRoleWriteAccess> Load()
        //{
        //    StoreBase<ModuleRoleWriteAccess> store = new DAL.StoreBase<ModuleRoleWriteAccess>(DatabaseObjects.Tables.RequestRoleWriteAccess);
        //    List<ModuleRoleWriteAccess> objRoleWriteAccess = new List<ModuleRoleWriteAccess>();
        //    objRoleWriteAccess = store.Load();
        //    return objRoleWriteAccess;
        //}
    }
    public interface IModuleRoleWriteAccessStore : IStore<ModuleRoleWriteAccess>
    {
        List<ModuleRoleWriteAccess> LoadByModule(string moduleName);
    }
}
