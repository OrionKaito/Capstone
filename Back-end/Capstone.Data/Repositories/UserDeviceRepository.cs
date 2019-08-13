using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Data.Repositories
{
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {
        IEnumerable<UserDevice> GetDeviceTokenByUserID(string userID);
    }
    public class UserDeviceRepository : RepositoryBase<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<UserDevice> GetDeviceTokenByUserID(string userID)
        {
            return DbContext.UserDevices.Where(u => u.UserID.Equals(userID)).ToList();
        }
    }
}
