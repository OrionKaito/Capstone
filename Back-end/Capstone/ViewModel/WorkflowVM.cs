using System;

namespace Capstone.ViewModel
{
    public class WorkflowVM
    {
        public Guid WorkFlowID { get; set; }
        public Guid OwnerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class WorkflowCM
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class WorkflowUM
    {
        public Guid WorkFlowID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
