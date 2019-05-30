using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {

    }

    public class UserNotificationRepository : RepositoryBase<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
