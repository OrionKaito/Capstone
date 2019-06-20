using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        UserGroup CheckExist(string UserID, Guid GroupID);
    }

    public class UserGroupRepository : RepositoryBase<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public UserGroup CheckExist(string UserID, Guid GroupID)
        {
            return DbContext.UserGroups.Where(u => u.UserID.Equals(UserID) && u.GroupID == GroupID).FirstOrDefault();
        }
    }
}
