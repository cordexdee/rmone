using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.DAL.Store;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;

namespace uGovernIT.Manager
{
    public class PhraseManager : ManagerBase<Phrases>
    {
        public PhraseManager(ApplicationContext context) : base(context)
        {
            store = new PhraseStore(this.dbContext);
        }
    }
}
