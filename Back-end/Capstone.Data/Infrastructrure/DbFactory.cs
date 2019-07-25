using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Capstone.Data.Infrastructrure
{
    public class DbFactory : Disposable, IDbFactory
    {
        CapstoneEntities dbContext;

        public IConfiguration _configuration { get; }

        public DbFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public CapstoneEntities Init()
        {
            if (dbContext == null)
            {
                var dbBuilder = new DbContextOptionsBuilder<CapstoneEntities>();
                dbBuilder.UseLazyLoadingProxies();
                dbBuilder.UseSqlServer(_configuration.GetConnectionString("CapstoneEntities"));

                dbContext = new CapstoneEntities(dbBuilder.Options);
            }

            return dbContext;
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
