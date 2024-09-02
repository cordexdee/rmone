using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.DAL.Store;
using uGovernIT.Utility;
namespace uGovernIT.Manager
{
    public class MessageBoardManager:ManagerBase<MessageBoard>, IMessageBoardManager
    {
        public MessageBoardManager(ApplicationContext context):base(context)
        {
            store = new MessageBoardStore(this.dbContext);
        }
    }
    public interface IMessageBoardManager : IManagerBase<MessageBoard>
    {

    }
}
