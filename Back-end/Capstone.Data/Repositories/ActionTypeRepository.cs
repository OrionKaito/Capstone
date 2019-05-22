using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IActionTypeRepository : IRepository<ActionType>
    {

    }

    public class ActionTypeRepository : RepositoryBase<ActionType>, IActionTypeRepository
    {
        public ActionTypeRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
