using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateActionConnectionRepository : IRepository<WorkFlowTemplateActionConnection>
    {
        IEnumerable<WorkFlowTemplateActionConnection> GetByFromWorkflowTemplateActionID(Guid ID);
        WorkFlowTemplateActionConnection GetByFromIDAndToID(Guid fromID, Guid toID);
    }

    public class WorkFlowTemplateActionConnectionRepository : RepositoryBase<WorkFlowTemplateActionConnection>, IWorkFlowTemplateActionConnectionRepository
    {
        public WorkFlowTemplateActionConnectionRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplateActionConnection GetByFromIDAndToID(Guid fromID, Guid toID)
        {
            return DbContext.WorkFlowTemplateActionConnections.Where(r => r.FromWorkFlowTemplateActionID == fromID && r.ToWorkFlowTemplateActionID == toID).FirstOrDefault();
        }

        public IEnumerable<WorkFlowTemplateActionConnection> GetByFromWorkflowTemplateActionID(Guid ID)
        {
            return DbContext.WorkFlowTemplateActionConnections.Where(w => w.FromWorkFlowTemplateActionID == ID).ToList();
        }
    }
}
