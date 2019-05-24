using Capstone.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Data
{
    public class CapstoneEntities : IdentityDbContext<User>
    {
        public CapstoneEntities()
            : base(new DbContextOptionsBuilder()
                  .UseSqlServer(@"Server=.;Database=Capstone;user id=sa;password=123456789;Trusted_Connection=True;")
                  .Options)
        {
        }

        public DbSet<WorkFlow> Workflows { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
