using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class MessageBoardStore:StoreBase<MessageBoard>, IMessageBoardStore
    {
        public MessageBoardStore(CustomDbContext context):base(context)
        {

        }

    }
    public interface IMessageBoardStore : IStore<MessageBoard>
    {

    }
}
