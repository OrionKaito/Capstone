using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Data.Repositories
{
  
    public interface IRequestRepository : IRepository<Request> {
        int CountMyRequest(string ID);
    }
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public int CountMyRequest(string ID)
        {
            return DbContext.Requests.Where(r => r.InitiatorID.Equals(ID)).Count();
        }
    }
}
