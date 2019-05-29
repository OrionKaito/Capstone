using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IPermissionOfRoleRepository : IRepository<PermissionOfRole>
    {

    }

    public class PermissionOfRoleRepository : RepositoryBase<PermissionOfRole>, IPermissionOfRoleRepository
    {
        public PermissionOfRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
