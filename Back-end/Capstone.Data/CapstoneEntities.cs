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
        public DbSet<WorkFlowTemplateActionConnection> WorkFlowTemplateActionConnections { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<ConnectionType> ConnectionTypes { get; set; }

        //Authorizations
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<RoleOfGroup> RoleOfGroups { get; set; }
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

        //Trả lỗi db
        //public override int SaveChanges()
        //{
        //    var errorMessage = "";
        //    var entities = (from entry in ChangeTracker.Entries()
        //                    where entry.State == EntityState.Modified || entry.State == EntityState.Added
        //                    select entry.Entity);

        //    var validationResults = new List<ValidationResult>();
        //    foreach (var entity in entities)
        //    {
        //        if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
        //        {
        //            errorMessage += string.Format(validationResults.ToString()) + Environment.NewLine;
        //            throw new Exception(errorMessage);

        //        }
        //    }
        //    return base.SaveChanges();
        //}

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
            //var PasswordHash = new PasswordHasher<string>();

            //modelBuilder.Entity<User>().HasData(new User
            //{
            //    Id = "8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0",
            //    CreateDate = DateTime.Now,
            //    DateOfBirth = DateTime.Now,
            //    Email = "orionkaito@gmail.com",
            //    NormalizedEmail = "orionkaito@gmail.com".ToUpper(),
            //    EmailConfirmCode = "999999",
            //    FullName = "orionkaito",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    ManagerID = "f05c1885-cadc-4bbc-bf89-20d39b1e02ae",
            //    PasswordHash = PasswordHash.HashPassword("orionkaito", "123456"),
            //    UserName = "orionkaito",
            //    NormalizedUserName = "orionkaito".ToUpper(),
            //    EmailConfirmed = true,
            //    IsDeleted = false,
            //},

            //new User
            //{
            //    Id = "f05c1885-cadc-4bbc-bf89-20d39b1e02ae",
            //    CreateDate = DateTime.Now,
            //    DateOfBirth = DateTime.Now,
            //    Email = "fontend@gmail.com",
            //    NormalizedEmail = "fontend@gmail.com".ToUpper(),
            //    EmailConfirmCode = "999999",
            //    FullName = "manager",
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    ManagerID = "",
            //    PasswordHash = PasswordHash.HashPassword("manager", "123456"),
            //    UserName = "manager",
            //    NormalizedUserName = "manager".ToUpper(),
            //    EmailConfirmed = true,
            //    IsDeleted = false,
            //}
            //);

            //modelBuilder.Entity<Role>().HasData(new Role
            //{
            //    ID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    Name = "user",
            //    IsDeleted = false,
            //},
            //new Role
            //{
            //    ID = new Guid("9e0a671f-854e-4e55-a61c-fa41880ac860"),
            //    Name = "admin",
            //    IsDeleted = false,
            //},
            //new Role
            //{
            //    ID = new Guid("102ed99e-b97d-404f-975c-a58fcb9c3c2b"),
            //    Name = "staff",
            //    IsDeleted = false,
            //}
            //);

            //modelBuilder.Entity<UserRole>().HasData(new UserRole
            //{
            //    ID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    RoleID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    UserID = "8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0",
            //    IsDeleted = false,
            //});

            //modelBuilder.Entity<Permission>().HasData(new Permission
            //{
            //    ID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    Name = "send request",
            //    IsDeleted = false,
            //});

            //modelBuilder.Entity<PermissionOfRole>().HasData(new PermissionOfRole
            //{
            //    ID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    PermissionID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    RoleID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    IsDeleted = false,
            //});

            //modelBuilder.Entity<ActionType>().HasData(new ActionType
            //{
            //    ID = new Guid("8f571e0e-b1b6-4cd8-a7d9-0fc45b0fd8f0"),
            //    Data = "data",
            //    Name = "name",
            //    IsDeleted = false,
            //});
        }
    }
}
