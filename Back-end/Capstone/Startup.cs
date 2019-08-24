using AutoMapper;
using Capstone.Data;
using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Mappings;
using Capstone.Model;
using Capstone.Service;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
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
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Capstone
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

            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                })
                .SetApplicationName("Capstone");

            //fix bug lỗi không chạy dc do "has been blocked by CORS policy: 
            //Response to preflight request doesn't pass access control check: 
            //No 'Access-Control-Allow-Origin' header is present on the requested resource."
            //lý do lỗi: 2 miền khác nhau, chrome tự bảo mật nên k cho vào
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("CapstoneEntities"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddDbContext<CapstoneEntities>(
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

            #endregion

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Capstone API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() },
            });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IHostingEnvironment env)
        {
            //fix bug lỗi không chạy dc do "has been blocked by CORS policy: 
            //Response to preflight request doesn't pass access control check: 
            //No 'Access-Control-Allow-Origin' header is present on the requested resource." 
            //lý do lỗi: 2 miền khác nhau, chrome tự bảo mật nên k cho vào 
            app.UseCors("CorsPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable jwt
            app.UseAuthentication();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // Specifying the swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Capstone API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // the default hsts value is 30 days. you may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard();

            //khi chạy server
            //xóa hết id cũ
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }

            //backgroundJobs.Schedule<IHangfireService>(u => u.checkAndChange(), TimeSpan.FromMinutes(1));
            RecurringJob.AddOrUpdate<IHangfireService>(h => h.HandleByHangfire(), Configuration["CronExpression"]);

            //List<RecurringJobDto> list;
            //using (var connection = JobStorage.Current.GetConnection())
            //{
            //    list = connection.GetRecurringJobs();
            //}

            //var job = list?.FirstOrDefault(j => j.Id == "test");  // jobId is the recurring job ID, whatever that is
            //if (job != null && !string.IsNullOrEmpty(job.LastJobId))
            //{
            //    BackgroundJob.Delete(job.LastJobId);
            //}

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
