using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateRepository : IRepository<WorkFlowTemplate>
    {
        WorkFlowTemplate GetByName(string name);
    }

    public class WorkFlowTemplateRepository : RepositoryBase<WorkFlowTemplate>, IWorkFlowTemplateRepository
    {
        public WorkFlowTemplateRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplate GetByName(string name)
        {
            return DbContext.WorkFlowTemplate.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }
    }
}
