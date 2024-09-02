using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Store
{
    public class ExperiencedTagStore:StoreBase<ExperiencedTag>, IExperiencedTagStore
    {
        public ExperiencedTagStore(CustomDbContext context):base(context)
        {

        }
    }
    public interface IExperiencedTagStore : IStore<ExperiencedTag>
    {
    }
}
