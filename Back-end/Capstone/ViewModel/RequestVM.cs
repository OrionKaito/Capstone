using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class RequestPaginVM
    {
        public int TotalRecord { get; set; }
        public IEnumerable<RequestVM> Requests { get; set; }
    }

    public class RequestVM
    {
        public Guid ID { get; set; }

        public string Description { get; set; }

        public string InitiatorID { get; set; }

        public string InitiatorName { get; set; }

        public Guid WorkFlowTemplateID { get; set; }

        public string WorkFlowTemplateName { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid RequestActionID { get; set; }
    }

    public class RequestCM
    {
        //Request
        public string Description { get; set; }

        public Guid WorkFlowTemplateID { get; set; }

        //Request Action
        public Guid NextStepID { get; set; }

        public Guid WorkFlowTemplateActionID { get; set; }

        //Request Value
        public IEnumerable<ActionValueVM> ActionValues { get; set; }

        //Request File
        public IEnumerable<string> ImagePaths { get; set; }
    }

    public class RequestApproveCM
    {
        //Request Action
        public Guid RequestID { get; set; }

        public Guid RequestActionID { get; set; }

        //public StatusEnum Status { get; set; }

        public Guid NextStepID { get; set; }

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
        public string WorkFlowName { get; set; }

        public string WorkFlowTemplateActionName { get; set; }

        public Guid WorkFlowTemplateActionID { get; set; }

        public IEnumerable<ConnectionVM> Connections { get; set; }

        public ActionTypeVM ActionType { get; set; }
    }

    public class HandleFormVM
    {
        public string InitiatorName { get; set; }

        public string WorkFlowTemplateName { get; set; }

        public string WorkFlowTemplateActionName { get; set; }

        public Guid WorkFlowTemplateActionID { get; set; }

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
        public string FullName { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public IEnumerable<RequestValueVM> RequestValues { get; set; }

        public string WorkFlowActionName { get; set; }
    }

    public class RequestResultVM
    {
        public string WorkFlowTemplateName { get; set; }

        public string Status { get; set; }

        public IEnumerable<StaffRequestActionVM> StaffResult { get; set; }
    }

    public class MyRequestPaginVM
    {
        public int TotalRecord { get; set; }
        public IEnumerable<MyRequestVM> MyRequests { get; set; }
    }

    public class MyRequestVM
    {
        public Guid ID { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Description { get; set; }

        public Guid WorkFlowTemplateID { get; set; }

        public string WorkFlowTemplateName { get; set; }

        public Guid CurrentRequestActionID { get; set; }

        public string CurrentRequestActionName { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }
    }
}
