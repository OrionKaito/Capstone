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
        public virtual Notification Notification { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSend { get; set; }
    }
}
