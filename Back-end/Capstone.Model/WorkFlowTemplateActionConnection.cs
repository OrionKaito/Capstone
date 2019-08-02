using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplateActionConnection
    {
        [Key]
        public Guid ID { get; set; }

        public Guid FromWorkFlowTemplateActionID { get; set; }
        [ForeignKey("FromWorkFlowTemplateActionID")]
        public virtual WorkFlowTemplateAction FromWorkFlowTemplateAction { get; set; }

        public Guid ToWorkFlowTemplateActionID { get; set; }
        [ForeignKey("ToWorkFlowTemplateActionID")]
        public virtual WorkFlowTemplateAction ToWorkFlowTemplateAction { get; set; }

        public Guid ConnectionTypeID { get; set; }
        [ForeignKey("ConnectionTypeID")]
        public virtual ConnectionType ConnectionType { get; set; }

        public bool IsDeleted { get; set; }
    }
}
