using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateActionRepository : IRepository<WorkFlowTemplateAction>
    {
        WorkFlowTemplateAction GetByName(string name);
        WorkFlowTemplateAction GetStartByWorFlowID(Guid ID);
    }

    public class WorkFlowTemplateActionRepository : RepositoryBase<WorkFlowTemplateAction>, IWorkFlowTemplateActionRepository
    {
        public WorkFlowTemplateActionRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplateAction GetByName(string name)
        {
            return DbContext.WorkFlowTemplateActions.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }

        public WorkFlowTemplateAction GetStartByWorFlowID(Guid ID)
        {
            return DbContext.WorkFlowTemplateActions.Where(w => w.WorkFlowTemplateID == ID && w.IsStart == true).FirstOrDefault();
        }
    }
}
