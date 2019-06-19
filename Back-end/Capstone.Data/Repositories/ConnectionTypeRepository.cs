using Capstone.Data.Infrastructrure;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Data.Repositories
{
    public interface IConnectionTypeRepository : IRepository<ConnectionType>
    {
        ConnectionType GetByName(string name);
    }

    public class ConnectionTypeRepository : RepositoryBase<ConnectionType>, IConnectionTypeRepository
    {
        public ConnectionTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public ConnectionType GetByName(string name)
        {
            return DbContext.ConnectionTypes.Where(c => c.Name.Equals(name)).FirstOrDefault();
        }
    }
}
