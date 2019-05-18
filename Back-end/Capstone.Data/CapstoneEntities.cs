using Capstone.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Data
{
    public class CapstoneEntities : IdentityDbContext<User>
    {
        public CapstoneEntities()
            : base(new DbContextOptionsBuilder()
                  .UseSqlServer(@"Server=.;Database=Capstone;user id=sa;password=920713823597;Trusted_Connection=True;")
                  .Options)
        {
        }

        public DbSet<WorkFlow> Workflows { get; set; }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
