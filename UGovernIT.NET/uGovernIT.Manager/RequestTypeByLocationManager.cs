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
    public class RequestTypeByLocationManager: ManagerBase<ModuleRequestTypeLocation>, IModuleRequestTypeLocation
    {
        public RequestTypeByLocationManager(ApplicationContext context):base(context)
        {
            store = new ModuleRequestTypeLocationStore(this.dbContext);
        }
        public List<ModuleRequestTypeLocation> GetRequestTypeLocationData(string moduelName)
        {
            List<ModuleRequestTypeLocation> moduleRequestTypeLocationList = store.Load(string.Format("where {0}='{1}'",DatabaseObjects.Columns.ModuleNameLookup,moduelName));
            foreach (ModuleRequestTypeLocation m in moduleRequestTypeLocationList)
            {
                FieldLookupValue loc = new FieldLookupValue(Convert.ToInt32(m.LocationLookup), DatabaseObjects.Columns.Title, DatabaseObjects.Tables.Location);
                FieldLookupValue mRequestType = new FieldLookupValue(Convert.ToInt32(m.RequestTypeLookup), DatabaseObjects.Columns.Title, DatabaseObjects.Tables.RequestType);
                if (loc != null)
                    m.Location = new LookupValue(loc.ID, loc.Value);

                if (mRequestType != null)
                    m.RequestType = new LookupValue(mRequestType.ID, mRequestType.Value);
            }
            return moduleRequestTypeLocationList;
        }
        
    }
    public interface IModuleRequestTypeLocation : IManagerBase<ModuleRequestTypeLocation>
    {
        List<ModuleRequestTypeLocation> GetRequestTypeLocationData(string moduelName);
    }
}
