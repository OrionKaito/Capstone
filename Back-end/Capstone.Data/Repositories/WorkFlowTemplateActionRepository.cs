using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateActionRepository : IRepository<WorkFlowTemplateAction>
    {
        WorkFlowTemplateAction GetByName(string name);
    }

    public class WorkFlowTemplateActionRepository : RepositoryBase<WorkFlowTemplateAction>, IWorkFlowTemplateActionRepository
    {
        public WorkFlowTemplateActionRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplateAction GetByName(string name)
        {
            return DbContext.WorkFlowTemplateActions.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }
    }
}
