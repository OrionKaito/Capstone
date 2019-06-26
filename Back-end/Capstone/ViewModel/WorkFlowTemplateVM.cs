using System;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateVM
    {
        public Guid ID { get; set; }
        public Guid OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PermissionToEditID { get; set; }
        public Guid PermissionToUseID { get; set; }
    }

    public class WorkFlowTemplateCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid PermissionToEditID { get; set; }
        public Guid PermissionToUseID { get; set; }
    }

    public class WorkFlowTemplateUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
