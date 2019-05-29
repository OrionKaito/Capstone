using Capstone.Data.Infrastructrure;
using Capstone.Model;

namespace Capstone.Data.Repositories
{
    public interface IRequestFileRepository : IRepository<RequestFile> { }
    public class RequestFileRepository : RepositoryBase<RequestFile>, IRequestFileRepository
    {
        public RequestFileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
