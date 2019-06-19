using System;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateActionConnectionVM
    {
        public Guid ID { get; set; }
        public Guid FromWorkFlowTemplateActionID { get; set; }
        public Guid ToWorkFlowTemplateActionID { get; set; }
        public Guid ConnectionTypeID { get; set; }
    }

    public class WorkFlowTemplateActionConnectionCM
    {
        public Guid FromWorkFlowTemplateActionID { get; set; }
        public Guid ToWorkFlowTemplateActionID { get; set; }
        public Guid ConnectionTypeID { get; set; }
    }

    public class WorkFlowTemplateActionConnectionUM
    {
        public Guid ID { get; set; }
    }
}
