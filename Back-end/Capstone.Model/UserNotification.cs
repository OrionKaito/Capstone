using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class UserNotification
    {
        [Key]
        public Guid ID { get; set; }

        public Guid NotificationID { get; set; }
        [ForeignKey("NotificationID")]
        public Notification Notification { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public bool IsRead { get; set; }
        public bool IsHandled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
