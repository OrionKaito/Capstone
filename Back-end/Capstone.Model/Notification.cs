using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public Guid EventID { get; set; }

        public DateTime DateTime { get; set; }
        public NotificationEnum NotificationType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
