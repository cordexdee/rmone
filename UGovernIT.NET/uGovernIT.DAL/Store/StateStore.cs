using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class StateStore : StoreBase<State>, IStateStore
    {
        public StateStore(CustomDbContext context) : base(context)
        {

        }

    }
    public interface IStateStore : IStore<State>
    {

    }
}