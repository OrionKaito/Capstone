using Capstone.Model;
using System;

namespace Capstone.ViewModel
{
    public class NotificationVM
    {
        public Guid ID { get; set; }

        public Guid EventID { get; set; }

        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class NotificationCM
    {
        public Guid EventID { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class NotificationUM
    {
        public Guid ID { get; set; }

        public Guid EventID { get; set; }

        public DateTime DateTime { get; set; }
    }

    public class NotificationViewModel
    {
        public Guid EventID { get; set; }
        public string Message { get; set; }
        public string Fullname { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public string ApproverName { get; set; }
    }
}
