using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;

namespace uGovernIT.Manager
{
    public class FunctionalAreasManager:ManagerBase<FunctionalArea>,IFunctionalAreasManager
    {
        public FunctionalAreasManager(ApplicationContext context) : base(context)
        {
            store = new FunctionAreasStore(this.dbContext);
        }

        public List<FunctionalArea> LoadFunctionalAreas()
        {
            return Load();
        }

        public FunctionalArea LoadFunctionalArea(FunctionalArea item)
        {
            FunctionalArea functionalArea = new FunctionalArea();
            //functionalArea.ID = item.ID;
            //functionalArea.Name = Convert.ToString(item[DatabaseObjects.Columns.Title]);
            //functionalArea.IsDeleted = UGITUtility.StringToBoolean(item[DatabaseObjects.Columns.IsDeleted]);
            return functionalArea;
        }

    }
    public interface IFunctionalAreasManager : IManagerBase<FunctionalArea>
    {

    }
}
