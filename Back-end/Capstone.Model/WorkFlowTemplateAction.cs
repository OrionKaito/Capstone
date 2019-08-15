using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplateAction
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsApprovedByLineManager { get; set; }
        public string ToEmail { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }

        public Guid WorkFlowTemplateID { get; set; }
        [ForeignKey("WorkFlowTemplateID")]
        public virtual WorkFlowTemplate WorkFlowTemplate { get; set; }

        public Guid? ActionTypeID { get; set; }
        [ForeignKey("ActionTypeID")]
        public virtual ActionType ActionType { get; set; }

        public Guid? PermissionToUseID { get; set; }
        [ForeignKey("PermissionToUseID")]
        public virtual Permission PermissionToUse { get; set; }

        public bool IsDeleted { get; set; }
    }
}
