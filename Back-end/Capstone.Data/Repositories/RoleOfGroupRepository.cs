using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IRoleOfGroupRepository : IRepository<RoleOfGroup>
    {
        //RoleOfGroup CheckExist(Guid RoleID, Guid GroupID);
    }

    public class RoleOfGroupRepository : RepositoryBase<RoleOfGroup>, IRoleOfGroupRepository
    {
        public RoleOfGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        //public RoleOfGroup CheckExist(Guid RoleID, Guid GroupID)
        //{
        //    return DbContext.RoleOfGroups.Where(r => r.RoleID == RoleID && r.GroupID == GroupID).FirstOrDefault();
        //}
    }
}
