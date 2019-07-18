using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class WorkFlowTemplateActionConnectionVM
    {
        public Guid ID { get; set; }
        public Guid FromWorkFlowTemplateActionID { get; set; }
        public Guid ToWorkFlowTemplateActionID { get; set; }
        public Guid ConnectionTypeID { get; set; }
    }

    public class WorkFlowTemplateActionConnectionCM
    {
        public Guid FromWorkFlowTemplateActionID { get; set; }
        public Guid ToWorkFlowTemplateActionID { get; set; }
        public Guid ConnectionTypeID { get; set; }
    }

    public class WorkFlowTemplateActionConnectionUM
    {
        public Guid ID { get; set; }
    }

    public class CheckConnectionVM
    {
        public IEnumerable<CheckConnection> Connections { get; set; }
        public IEnumerable<Node> Nodes { get; set; }
    }

    public class CheckConnection
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public Guid ConnectionTypeID { get; set; }
    }

    public class Node
    {
        public Guid NodeName { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
    }
}
