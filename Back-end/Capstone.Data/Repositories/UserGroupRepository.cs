using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IUserGroupRepository : IRepository<UserGroup> { }

    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
