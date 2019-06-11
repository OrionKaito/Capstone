using System;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateVM
    {
        public Guid WorkFlowTemplateID { get; set; }
        public Guid OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class WorkFlowTemplateCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class WorkFlowTemplateUM
    {
        public Guid WorkFlowTemplateID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
