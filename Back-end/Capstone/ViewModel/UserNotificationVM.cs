using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
    public class UserNotificationVM
    {
        public Guid ID { get; set; }
        public Guid NotificationID { get; set; }
        public string UserID { get; set; }
        public bool IsRead { get; set; }
    }

    public class UserNotificationCM
    {
        public Guid NotificationID { get; set; }
        public string UserID { get; set; }
    }

    public class UserNotificationUM
    {
        public Guid ID { get; set; }
        public Guid NotificationID { get; set; }
        public string UserID { get; set; }
    }
}
