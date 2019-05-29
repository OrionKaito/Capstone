using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetByName(string Name);
    }

    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }

        public Role GetByName(string Name)
        {
            return DbContext.Roles.Where(r => r.Name.Equals(Name)).FirstOrDefault();
        }
    }
}
