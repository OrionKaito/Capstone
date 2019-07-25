using Capstone.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Capstone.Data
{
    public class CapstoneEntities : IdentityDbContext<User>
    {
        public CapstoneEntities(DbContextOptions<CapstoneEntities> options) : base(options)
        {
        }

        //WorkFlows
        public DbSet<WorkFlowTemplate> WorkFlowTemplates { get; set; }
        public DbSet<WorkFlowTemplateAction> WorkFlowTemplateActions { get; set; }
        public DbSet<WorkFlowTemplateActionConnection> WorkFlowTemplateActionConnections { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<ConnectionType> ConnectionTypes { get; set; }

        //Authorizations
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionOfGroup> PermissionOfGroups { get; set; }
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
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

            modelBuilder.Entity<User>().Property(u => u.CreateDate)
                                            .HasDefaultValueSql("CONVERT(date, GETDATE())");
            modelBuilder.Entity<Request>().Property(r => r.CreateDate)
                                            .HasDefaultValueSql("CONVERT(date, GETDATE())");
            modelBuilder.Entity<RequestAction>().Property(r => r.CreateDate)
                                            .HasDefaultValueSql("CONVERT(date, GETDATE())");
            modelBuilder.Entity<Notification>().Property(n => n.CreateDate)
                                            .HasDefaultValueSql("CONVERT(date, GETDATE())");

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var foreignkey in cascadeFKs)
            {
                foreignkey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //Seed
            var PasswordHash = new PasswordHasher<string>();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                DateOfBirth = DateTime.Now,
                Email = "orionkaito@gmail.com",
                NormalizedEmail = "orionkaito@gmail.com".ToUpper(),
                EmailConfirmCode = "999999",
                FullName = "orionkaito",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = PasswordHash.HashPassword("orionkaito", "123456"),
                UserName = "orionkaito@gmail.com",
                NormalizedUserName = "orionkaito".ToUpper(),
                EmailConfirmed = true,
                IsDeleted = false,
            });

            modelBuilder.Entity<Role>().HasData(new Role
            {
                ID = Guid.NewGuid(),
                Name = "user",
                IsDeleted = false,
            },
            new Role
            {
                ID = Guid.NewGuid(),
                Name = "admin",
                IsDeleted = false,
            },
            new Role
            {
                ID = Guid.NewGuid(),
                Name = "staff",
                IsDeleted = false,
            }
            );

            modelBuilder.Entity<Permission>().HasData(new Permission
            {
                ID = Guid.NewGuid(),
                Name = "send request",
                IsDeleted = false,
            });

            modelBuilder.Entity<ActionType>().HasData(new ActionType
            {
                ID = Guid.NewGuid(),
                Data = "data",
                Name = "name",
                IsDeleted = false,
            });
        }
    }
}
