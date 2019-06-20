using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class Request
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Description { get; set; }

        public string InitiatorID { get; set; }
        [ForeignKey("InitiatorID")]
        public User User { get; set; }

        public Guid WorkFlowTemplateID { get; set; }
        [ForeignKey("WorkFlowTemplateID")]
        public WorkFlowTemplate WorkFlowTemplate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
