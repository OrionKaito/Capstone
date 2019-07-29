using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Data.Repositories
{
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {

    }
    public class UserDeviceRepository : RepositoryBase<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
