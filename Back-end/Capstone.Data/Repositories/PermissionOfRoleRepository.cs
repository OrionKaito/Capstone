using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IPermissionOfRoleRepository : IRepository<PermissionOfRole>
    {
        PermissionOfRole CheckExist(Guid PermissionID, Guid RoleID);
    }

    public class PermissionOfRoleRepository : RepositoryBase<PermissionOfRole>, IPermissionOfRoleRepository
    {
        public PermissionOfRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public PermissionOfRole CheckExist(Guid PermissionID, Guid RoleID)
        {
            return DbContext.PermissionOfRoles.Where(p => p.PermissionID == PermissionID && p.RoleID == RoleID).FirstOrDefault();
        }
    }
}
