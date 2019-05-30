using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Permission GetByName(string Name);
    }

    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Permission GetByName(string Name)
        {
            return DbContext.Permissions.Where(p => p.Name.Equals(Name)).FirstOrDefault();
        }
    }
}
