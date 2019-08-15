using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class UserNotificationPaginVM
    {
        public int TotalRecord { get; set; }
        public IEnumerable<UserNotificationVM> UserNotifications { get; set; }
    }

    public class UserNotificationVM
    {
        public string WorkflowName { get; set; }
        public Guid UserNotificationID { get; set; }
        public Guid EventID { get; set; }
        public string Message { get; set; }
        public string ActorName { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public string NotificationTypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsHandled { get; set; }
    }
}
