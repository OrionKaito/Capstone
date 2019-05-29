using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IActionTypeRepository : IRepository<ActionType>
    {
        ActionType GetByName(string Name);
    }

    public class ActionTypeRepository : RepositoryBase<ActionType>, IActionTypeRepository
    {
        public ActionTypeRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public ActionType GetByName(string Name)
        {
            return DbContext.ActionTypes.Where(a => a.Name.Equals(Name)).FirstOrDefault();
        }
    }
}
