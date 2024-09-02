using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DAL.Store
{
    public class HelpCardStore : StoreBase<HelpCard>, IHelpCardStore
    {
        public HelpCardStore(CustomDbContext context) : base(context)
        {

        }
    }

    public interface IHelpCardStore : IStore<HelpCard>
    {

    }
}
