using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetByNotificationType(NotificationEnum notificationType);
        IEnumerable<Notification> GetByNotificationTypeAndIsHandled(NotificationEnum notificationType);
    }

    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Notification> GetByNotificationType(NotificationEnum notificationType)
        {
            return DbContext.Notifications.Where(n => n.NotificationType == notificationType).ToList();
        }

        public IEnumerable<Notification> GetByNotificationTypeAndIsHandled(NotificationEnum notificationType)
        {
            return DbContext.Notifications.Where(n => n.NotificationType == notificationType).ToList();
        }
    }
}
