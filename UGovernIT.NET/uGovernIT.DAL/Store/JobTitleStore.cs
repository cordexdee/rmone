using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL
{
    public class JobTitleStore : StoreBase<JobTitle>, IJobTitleStore
    {
        public JobTitleStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IJobTitleStore : IStore<JobTitle>
    {

    }
}
