using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateActionConnectionRepository : IRepository<WorkFlowTemplateActionConnection>
    {

    }

    public class WorkFlowTemplateActionConnectionRepository : RepositoryBase<WorkFlowTemplateActionConnection>, IWorkFlowTemplateActionConnectionRepository
    {
        public WorkFlowTemplateActionConnectionRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
