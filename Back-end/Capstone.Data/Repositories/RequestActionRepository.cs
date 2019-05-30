using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IRequestActionRepository : IRepository<RequestAction> { }
    public class RequestActionRepository : RepositoryBase<RequestAction>, IRequestActionRepository
    {
        public RequestActionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
