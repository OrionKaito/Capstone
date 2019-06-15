using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateActionVM
    {
        public Guid WorkFlowTemplateActionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid PermissionToUseID { get; set; }
    }

    public class WorkFlowTemplateActionCM
    {
        public Guid WorkFlowTemplateActionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid PermissionToUseID { get; set; }
    }

    public class WorkFlowTemplateActionUM
    {
        public Guid ID { get; set; }
        public Guid WorkFlowTemplateActionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ActionTypeID { get; set; }
        public Guid PermissionToUseID { get; set; }
    }
}
