using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class HelpCardContentStore : StoreBase<HelpCardContent>, IHelpCardContentStore
    {
        public HelpCardContentStore(CustomDbContext context) : base(context)
        {

        }

    }

    public interface IHelpCardContentStore : IStore<HelpCardContent>
    {

    }
}
