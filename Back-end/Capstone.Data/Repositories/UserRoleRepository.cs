using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {

    }

    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
