using Capstone.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Data
{
    public class CapstoneEntities : IdentityDbContext<User>
    {
        public CapstoneEntities(DbContextOptions<CapstoneEntities> options)
            : base(options)
        {
        }

        public DbSet<Workflow> Workflows { get; set; }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
