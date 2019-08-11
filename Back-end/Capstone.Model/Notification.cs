using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Model
{
    public class Notification
    {
        [Key]
        public Guid ID { get; set; }
        public Guid EventID { get; set; }
        public DateTime? CreateDate { get; set; }
        public NotificationEnum NotificationType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
