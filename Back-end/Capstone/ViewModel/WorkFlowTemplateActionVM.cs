using System;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateActionVM
    {
        public Guid WorkFlowTemplateID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid? PermissionToUseID { get; set; }
        public bool IsApprovedByLineManager { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }

    public class WorkFlowTemplateActionCM
    {
        public Guid WorkFlowTemplateID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid? PermissionToUseID { get; set; }
        public bool IsApprovedByLineManager { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }

    public class WorkFlowTemplateActionUM
    {
        public Guid ID { get; set; }
        public Guid WorkFlowTemplateID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid? PermissionToUseID { get; set; }
        public bool IsApprovedByLineManager { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }
}
