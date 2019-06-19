using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplateActionConnection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public Guid FromWorkFlowTemplateActionID { get; set; }
        [ForeignKey("FromWorkFlowTemplateActionID")]
        public virtual WorkFlowTemplateAction WorkFlowTemplateActionFrom { get; set; }

        public Guid ToWorkFlowTemplateActionID { get; set; }
        [ForeignKey("ToWorkFlowTemplateActionID")]
        public virtual WorkFlowTemplateAction WorkFlowTemplateActionTo { get; set; }

        public Guid ConnectionTypeID { get; set; }
        [ForeignKey("ConnectionTypeID")]
        public virtual ConnectionType ConnectionType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
