using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowRepository : IRepository<WorkFlow>
    {
        
    }

    public class WorkFlowRepository : RepositoryBase<WorkFlow>, IWorkFlowRepository
    {
        public WorkFlowRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
