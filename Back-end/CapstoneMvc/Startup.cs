using Capstone.Data;
using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using Capstone.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

namespace CapstoneMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                })
                .SetApplicationName("Capstone");


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2); services.AddDbContext<CapstoneEntities>(
                         options => options.UseLazyLoadingProxies()
                         .UseSqlServer(Configuration.GetConnectionString("CapstoneEntities")));
            #region DI

            services.AddScoped<IDbFactory, DbFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //WorkFlow
            services.AddTransient<IWorkFlowTemplateRepository, WorkFlowTemplateRepository>();
            services.AddTransient<IWorkFlowTemplateService, WorkFlowTemplateService>();

            //WorkFlowAction
            services.AddTransient<IWorkFlowTemplateActionRepository, WorkFlowTemplateActionRepository>();
            services.AddTransient<IWorkFlowTemplateActionService, WorkFlowTemplateActionService>();

            //WorkFlowTemplateActionConnection
            services.AddTransient<IWorkFlowTemplateActionConnectionRepository, WorkFlowTemplateActionConnectionRepository>();
            services.AddTransient<IWorkFlowTemplateActionConnectionService, WorkFlowTemplateActionConnectionService>();

            //ActionType
            services.AddTransient<IActionTypeRepository, ActionTypeRepository>();
            services.AddTransient<IActionTypeService, ActionTypeService>();

            //Group
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IGroupService, GroupService>();

            //UserGroup
            services.AddTransient<IUserGroupRepository, UserGroupRepository>();
            services.AddTransient<IUserGroupService, UserGroupService>();

            //Role
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleService, RoleService>();

            //Permission
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IPermissionService, PermissionService>();

            //PermissionOfRole
            services.AddTransient<IPermissionOfGroupRepository, PermissionOfGroupRepository>();
            services.AddTransient<IPermissionOfGroupService, PermissionOfGroupService>();

            //UserRole
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IUserRoleService, UserRoleService>();

            //Notification
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<INotificationService, NotificationService>();

            //UserNotification
            services.AddTransient<IUserNotificationRepository, UserNotificationRepository>();
            services.AddTransient<IUserNotificationService, UserNotificationService>();

            //User
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            //Request
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<IRequestService, RequestService>();

            //RequestAction
            services.AddTransient<IRequestActionRepository, RequestActionRepository>();
            services.AddTransient<IRequestActionService, RequestActionService>();

            //Hangfire
            services.AddTransient<IHangfireService, HangfireService>();

            //RequestValue
            services.AddTransient<IRequestValueRepository, RequestValueRepository>();
            services.AddTransient<IRequestValueService, RequestValueService>();

            //RequestFile
            services.AddTransient<IRequestFileRepository, RequestFileRepository>();
            services.AddTransient<IRequestFileService, RequestFileService>();

            //Email
            services.AddTransient<IEmailService, EmailServicce>();

            //ConnectionType
            services.AddTransient<IConnectionTypeRepository, ConnectionTypeRepository>();
            services.AddTransient<IConnectionTypeService, ConnectionTypeService>();

            //UserDevice
            services.AddTransient<IUserDeviceRepository, UserDeviceRepository>();
            services.AddTransient<IUserDeviceService, UserDeviceService>();

            //WebHook
            services.AddTransient<IWebHookService, WebHookService>();

            //Encode
            services.AddTransient<IEncodeService, EncodeService>();
            #endregion

            // Auto Mapper Configurations
            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            // Add identity
            var authBuilder = services.AddIdentityCore<User>(o =>
            {
                // Configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            authBuilder = new IdentityBuilder(authBuilder.UserType, typeof(IdentityRole), authBuilder.Services);
            authBuilder.AddEntityFrameworkStores<CapstoneEntities>().AddDefaultTokenProviders();

            //services.AddDbContext<CapstoneEntities>(opt => opt.UseSqlServer("CapstoneEntities"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "Capstone API", Version = "v1" });
            //    c.AddSecurityDefinition("Bearer",
            //    new ApiKeyScheme
            //    {
            //        In = "header",
            //        Description = "Please enter into field the word 'Bearer' following by space and JWT",
            //        Name = "Authorization",
            //        Type = "apiKey"
            //    });
            //    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
            //    { "Bearer", Enumerable.Empty<string>() },
            //});
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
