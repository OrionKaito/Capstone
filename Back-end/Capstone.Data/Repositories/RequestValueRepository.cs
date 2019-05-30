using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IRequestValueRepository : IRepository<RequestValue> { }
    public class RequestValueRepository : RepositoryBase<RequestValue>, IRequestValueRepository
    {
        public RequestValueRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
