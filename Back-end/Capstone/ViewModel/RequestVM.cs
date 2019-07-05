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

        //Request Value
        public IEnumerable<ActionValueVM> ActionValues { get; set; }

        //Request File
        public IEnumerable<string> ImagePaths { get; set; }
    }

    public class RequestApproveCM
    {
        //Request Action
        public Guid RequestID { get; set; }
        public StatusEnum Status { get; set; }
        public Guid? NextStepID { get; set; }

        //Request Value
        public IEnumerable<ActionValueVM> ActionValues { get; set; }
    }

    public class RequestUM
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
    }

    public class ActionValueVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class ConnectionVM
    {
        public Guid NextStepID { get; set; }

        public Guid ConnectionID { get; set; }

        public string ConnectionTypeName { get; set; }
    }

    public class RequestFormVM
    {
        public IEnumerable<ConnectionVM> Connections { get; set; }

        public ActionTypeVM ActionType { get; set; }
    }

    public class HandleFormVM
    {
        public RequestVM Request { get; set; }

        public UserRequestActionVM UserRequestAction { get; set; }

        public IEnumerable<StaffRequestActionVM> StaffRequestActions { get; set; }

        public IEnumerable<ConnectionVM> Connections { get; set; }

        public ActionTypeVM ActionType { get; set; }
    }

    public class UserRequestActionVM
    {
        public IEnumerable<RequestFileVM> RequestFiles { get; set; }
        public IEnumerable<RequestValueVM> RequestValues { get; set; }
    }

    public class StaffRequestActionVM
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<RequestValueVM> RequestValues { get; set; }
    }
}
