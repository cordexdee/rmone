using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class TicketRelationManager: ManagerBase<TicketRelation>,ITicketRelationManager
    {
        public TicketRelationManager(ApplicationContext context):base(context)
        {
            store = new TicketRelationStore(this.dbContext);
        }
        public  TicketRelation GetTicketRelationShip(string childticketid,string parentticketid)
        {
            TicketRelation moduleImpactList;
            string moduleName = uHelper.getModuleNameByTicketId(parentticketid);
            StoreBase<TicketRelation> moduleImpact = new StoreBase<TicketRelation>(this.dbContext);
            if (moduleName == ModuleNames.CMDB)
                moduleImpactList = moduleImpact.Load().Where(x => (x.ChildTicketID == childticketid && x.ParentTicketID == parentticketid) || (x.ChildTicketID == parentticketid && x.ParentTicketID == childticketid)).FirstOrDefault();
            else
                moduleImpactList = moduleImpact.Load().Where(x => x.ChildTicketID == childticketid && x.ParentTicketID == parentticketid).FirstOrDefault();
            return moduleImpactList;
        }
    }
    public interface ITicketRelationManager:IManagerBase<TicketRelation>
    {
        TicketRelation GetTicketRelationShip(string childticketid, string parentticketid);
    }
}
