using Capstone.Model;
using System;

namespace Capstone.ViewModel
{
    public class NotificationVM
    {
        public Guid ID { get; set; }

        public Guid EventID { get; set; }

        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class NotificationCM
    {
        public Guid EventID { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class NotificationUM
    {
        public Guid ID { get; set; }

        public Guid EventID { get; set; }

        public DateTime CreateDate { get; set; }
    }

    public class NotificationViewModel
    {
        public string WorkflowName { get; set; }
        public Guid EventID { get; set; }
        public string Message { get; set; }
        public string ActorName { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public string NotificationTypeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsHandled { get; set; }
    }
}
