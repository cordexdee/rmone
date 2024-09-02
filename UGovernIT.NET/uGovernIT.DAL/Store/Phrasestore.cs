using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class PhraseStore : StoreBase<Phrases>
    {
        public PhraseStore(CustomDbContext context) : base(context)
        {

        }
    }
}
