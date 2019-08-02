using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Data.Repositories
{
    public interface IRequestActionRepository : IRepository<RequestAction>
    {
        RequestAction GetStartAction(Guid startActionTemplateID, Guid requestID);
        IEnumerable<RequestAction> GetExceptStartAction(Guid startActionTemplateID, Guid requestID);
    }
    public class RequestActionRepository : RepositoryBase<RequestAction>, IRequestActionRepository
    {
        public RequestActionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<RequestAction> GetExceptStartAction(Guid startActionTemplateID, Guid requestID)
        {
            return DbContext.RequestActions.Where(r => r.WorkFlowTemplateActionID != startActionTemplateID && r.RequestID == requestID).ToList();
        }

        public RequestAction GetStartAction(Guid startActionTemplateID, Guid requestID)
        {
            return DbContext.RequestActions.Where(r => r.WorkFlowTemplateActionID == startActionTemplateID && r.RequestID == requestID).FirstOrDefault();
        }
    }
}
