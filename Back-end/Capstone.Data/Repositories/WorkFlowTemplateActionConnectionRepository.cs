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
    }

    public class WorkFlowTemplateActionConnectionRepository : RepositoryBase<WorkFlowTemplateActionConnection>, IWorkFlowTemplateActionConnectionRepository
    {
        public WorkFlowTemplateActionConnectionRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public IEnumerable<WorkFlowTemplateActionConnection> GetByFromWorkflowTemplateActionID(Guid ID)
        {
            return DbContext.WorkFlowTemplateActionConnections.Where(w => w.FromWorkFlowTemplateActionID == ID).ToList();
        }
    }
}
