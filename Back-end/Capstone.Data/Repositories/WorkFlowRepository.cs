using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowRepository : IRepository<WorkFlow>
    {
        WorkFlow GetByName(string name);
    }

    public class WorkFlowRepository : RepositoryBase<WorkFlow>, IWorkFlowRepository
    {
        public WorkFlowRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlow GetByName(string name)
        {
            return DbContext.WorkFlows.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }
    }
}
