using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IWorkFlowTemplateRepository : IRepository<WorkFlowTemplate>
    {
        WorkFlowTemplate GetByName(string name);
        IEnumerable<WorkFlowTemplate> GetByPermissionToUse(Guid permissionID);
        IEnumerable<WorkFlowTemplate> SearchByNameOnPermissionToUse(Guid permissionID, string search);
    }

    public class WorkFlowTemplateRepository : RepositoryBase<WorkFlowTemplate>, IWorkFlowTemplateRepository
    {
        public WorkFlowTemplateRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public WorkFlowTemplate GetByName(string name)
        {
            return DbContext.WorkFlowTemplates.Where(w => w.Name.Equals(name)).FirstOrDefault();
        }

        public IEnumerable<WorkFlowTemplate> GetByPermissionToUse(Guid permissionID)
        {
            return DbContext.WorkFlowTemplates.Where(w => w.PermissionToUseID == permissionID && w.IsDeleted == false && w.IsEnabled == true);
        }

        public IEnumerable<WorkFlowTemplate> SearchByNameOnPermissionToUse(Guid permissionID, string search)
        {
            return DbContext.WorkFlowTemplates.Where(w => w.PermissionToUseID == permissionID && w.IsDeleted == false && w.IsEnabled == true && w.Name.Contains(search));
        }
    }
}
