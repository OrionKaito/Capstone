using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IGroupRepository : IRepository<Group> { }
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
