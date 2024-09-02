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
    public class ModuleUserTypeManager : ManagerBase<ModuleUserType>, IModuleUserTypeManager
    {
        public ModuleUserTypeManager(ApplicationContext context):base(context)
        {
            store = new ModuleUserTypeStore(this.dbContext);
        }
        public DataTable LoadModuleUserTypeDt()
        {
            DataTable dtResult = new DataTable();
           
            List<ModuleUserType> moduleusertypelist = store.Load();
            dtResult = UGITUtility.ToDataTable<ModuleUserType>(moduleusertypelist);
            return dtResult;
        }
       
    }
    public interface IModuleUserTypeManager : IManagerBase<ModuleUserType>
    {
        DataTable LoadModuleUserTypeDt();
    }
}
