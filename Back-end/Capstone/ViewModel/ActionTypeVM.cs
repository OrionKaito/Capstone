using System;

namespace Capstone.ViewModel
{
    public class FullWorkFlowTem
    {
        public Guid WorkFlowID { get; set; }
        public ActionItem[] action { get; set; }
        public ArrowItem[] arrow { get; set; }

    }

    public class ArrowItem
    {
        public string[] idDiv { get; set; }
        public string idArrow { get; set; }
        public string name { get; set; }
    }
    public class ActionItem
    {
        public string id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public Guid permissionToUseID { get; set; }
        public bool isApprovedByLineManager { get; set; }
        public bool start { get; set; }
        public bool end { get; set; }

    }
    public class ActionTypeVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class ActionTypeCM
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class ActionTypeUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
