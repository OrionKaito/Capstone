using Capstone.Model;
using System;
using System.Collections.Generic;

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
        //Request
        public string Description { get; set; }
        public Guid WorkFlowTemplateID { get; set; }

        //Request Action
        public Guid NextStepID { get; set; }
        public StatusEnum Status { get; set; }

        //Request Value
        public IEnumerable<ActionValue> ActionValues { get; set; }

        //Request File
        public IEnumerable<string> ImagePaths { get; set; }
    }

    public class RequestUM
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
    }

    public class ActionValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
