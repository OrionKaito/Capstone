using Capstone.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Capstone.Data
{
    public class CapstoneEntities : IdentityDbContext<User>
    {
        public CapstoneEntities()
            : base(new DbContextOptionsBuilder()
                  .UseSqlServer(@"Server=.;Database=Capstone;user id=sa;password=920713823597;Trusted_Connection=false;")
                  .Options)
        {
        }

        //WorkFlows
        public DbSet<WorkFlowTemplate> WorkFlowTemplates { get; set; }
        public DbSet<WorkFlowTemplateAction> WorkFlowTemplateActions { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<ConnectionType> ConnectionTypes { get; set; }

        //Authorizations
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleOfGroup> RoleOfGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionOfRole> PermissionOfRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        //Notifications
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        //Requests
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestAction> RequestActions { get; set; }
        public DbSet<RequestValue> RequestValues { get; set; }
        public DbSet<RequestFile> RequestFiles { get; set; }

        public void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users", "dbo");
            modelBuilder.Entity<User>().Ignore(c => c.AccessFailedCount)
                                           .Ignore(c => c.LockoutEnabled)
                                           .Ignore(c => c.LockoutEnd)
                                           .Ignore(c => c.ConcurrencyStamp)
                                           .Ignore(c => c.TwoFactorEnabled)
                                           .Ignore(c => c.PhoneNumber)
                                           .Ignore(c => c.PhoneNumberConfirmed);

            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityRole>();

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var foreignkey in cascadeFKs)
            {
                foreignkey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}
