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
        public int TotalVertice { get; set; }
        public int Root { get; set; }
        public IEnumerable<CheckConnection> Connections { get; set; }
    }

    public class CheckConnection
    {
        public int From { get; set; }
        public int To { get; set; }
        public int ConnectionTypeID { get; set; }
    }
}
