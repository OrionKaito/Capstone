using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class UserDevice
    {
        [Key]
        public Guid ID { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public string DeviceToken { get; set; }
    }
}
