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
    //public List<ACRTypes> GetConfigAcrTypeViewData()
    //{
    //    StoreBase<ACRTypes> acrTypeData = new StoreBase<ACRTypes>(DatabaseObjects.Tables.ACRTypes);
    //    List<ACRTypes> acrTypeList = acrTypeData.Load();
    //    return acrTypeList;
    //}
    public class ACRTypeManager : ManagerBase<ACRType>, IACRTypeManager
    {
        public ACRTypeManager(ApplicationContext context) : base(context)
        {
            store = new ACRTypeStore(this.dbContext);
        }
    }
    public interface IACRTypeManager : IManagerBase<ACRType>
    {

    }
}
