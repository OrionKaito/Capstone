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

        public Guid WorkFlowID { get; set; }
        [ForeignKey("WorkFlowID")]
        public WorkFlow WorkFlow { get; set; }

        public string Data { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
