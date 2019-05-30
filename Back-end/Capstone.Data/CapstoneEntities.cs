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
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleOfGroup> RoleOfGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionOfRole> PermissionOfRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestAction> RequestActions { get; set; }
        public DbSet<RequestValue> RequestValues { get; set; }
        public DbSet<RequestFile> RequestFiles { get; set; }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
