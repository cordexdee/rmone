using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities.DMSDB;

namespace uGovernIT.DAL.Store
{

    public class DocRepositoryStore : DocRepositoryBase, IDocRepositoryStore
    {
        public DocRepositoryStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IDocRepositoryStore : IDocRepository
    {

    }

}