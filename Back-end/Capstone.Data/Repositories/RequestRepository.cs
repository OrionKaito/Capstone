using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Data.Repositories
{
  
    public interface IRequestRepository : IRepository<Request> { }
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
