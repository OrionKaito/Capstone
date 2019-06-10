using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowRepository : IRepository<WorkFlowTemplate>
    {
        WorkFlowTemplate GetByName(string name);
    }

    public class WorkFlowRepository : RepositoryBase<WorkFlowTemplate>, IWorkFlowRepository
    {
        public WorkFlowRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplate GetByName(string name)
        {
            return DbContext.WorkFlowTemplate.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }
    }
}
