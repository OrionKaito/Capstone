using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplateAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid ActionTypeID { get; set; }
        [ForeignKey("ActionTypeID")]
        public virtual ActionType ActionType { get; set; }

        public Guid PermissionToUse { get; set; }
        [ForeignKey("PermissionToUse")]
        public virtual Permission Permission { get; set; }

        public bool IsDeleted { get; set; }
    }
}
