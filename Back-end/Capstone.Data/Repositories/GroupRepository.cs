using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Group GetByName(string Name);
    }

    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public Group GetByName(string Name)
        {
            return DbContext.Groups.Where(g => g.Name.Equals(Name)).FirstOrDefault();
        }
    }
}
