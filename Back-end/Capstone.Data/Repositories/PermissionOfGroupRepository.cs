using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IPermissionOfGroupRepository : IRepository<PermissionOfGroup>
    {
        PermissionOfGroup CheckExist(Guid PermissionID, Guid GroupID);
        PermissionOfGroup GetByPermission(Guid PermissionID);
    }

    public class PermissionOfGroupRepository : RepositoryBase<PermissionOfGroup>, IPermissionOfGroupRepository
    {
        public PermissionOfGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public PermissionOfGroup CheckExist(Guid PermissionID, Guid GroupID)
        {
            return DbContext.PermissionOfGroups.Where(p => p.PermissionID == PermissionID && p.GroupID == GroupID).FirstOrDefault();
        }

        public PermissionOfGroup GetByPermission(Guid PermissionID)
        {
            return DbContext.PermissionOfGroups.Where(p => p.PermissionID == PermissionID).FirstOrDefault();
        }
    }
}
