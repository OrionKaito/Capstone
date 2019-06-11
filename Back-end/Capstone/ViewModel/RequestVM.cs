using System;

namespace Capstone.ViewModel
{

    public class RequestVM
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
        public string InitiatorID { get; set; }
        public Guid WorkFlowTemplateID { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class RequestCM
    {
        public string Description { get; set; }
        public Guid WorkFlowTemplateID { get; set; }
    }

    public class RequestUM
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
    }
}
