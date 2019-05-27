using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IRoleOfGroupRepository : IRepository<RoleOfGroup>
    {

    }

    public class RoleOfGroupRepository : RepositoryBase<RoleOfGroup>, IRoleOfGroupRepository
    {
        public RoleOfGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
