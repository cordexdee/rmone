using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL;

namespace uGovernIT.Manager
{
    public class StudioManager : ManagerBase<Studio>, IStudioManager
    {
        public StudioManager(ApplicationContext context) : base(context)
        {

        }
        public long GetDivisionIdForStudio(long StudioID)
        {
            Studio ticketStudio = this.LoadByID(UGITUtility.StringToLong(StudioID));
            if (ticketStudio != null)
                return ticketStudio.DivisionLookup;
            else
                return 0;
        }

    }
    public interface IStudioManager : IManagerBase<Studio>
    {

    }
}
