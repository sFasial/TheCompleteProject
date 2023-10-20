using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Api.Infrastructure.Extensions;
using TheCompleteProject.Api.Infrastructure.Filter;
using TheCompleteProject.Api.Infrastructure.Middelware;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Repository.Repositories.User;
using TheCompleteProject.Service.MappingProfile;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility;
using WatchDog;
using WatchDog.src.Enums;

namespace TheCompleteProject.Api
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

            DotNetEnv.Env.Load();

            #region BINDING OF APP SETTINGS CLASS IN UTILITY WITH APPSETTING.JSON 

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);
            var appSettings = appSettingSection.Get<AppSettings>();


            #endregion

            #region ENVIORMENT VARIABLE IN FUTURE REFRENCE 

            services.AddDashServices(appSettings);

            //            using Microsoft.Extensions.DependencyInjection;
            //            using NPOI.SS.Formula.Functions;
            //            using System.Collections.Generic;
            //            using System.Reflection;
            //            using System;
            //            using TheCompleteProject.Utility;
            //            using System.Linq;

            //public static class ServiceCollectionExtensions
            //        {
            //            private static Dictionary<string, string> EnvironmentVariables = new Dictionary<string, string>();

            //            /// <summary>
            //            /// This method will read all the environment variables
            //            /// </summary>
            //            private static void ReadEnvironmentVariables()
            //            {
            //                #region ReadEnvironmentVariables

            //                foreach (FieldInfo info in typeof(ConstantsEnviormentTest).GetFields().Where(x => x.IsStatic))
            //                {
            //                    string key = info.GetRawConstantValue().ToString();
            //                    EnvironmentVariables.Add(key, Environment.GetEnvironmentVariable(key));
            //                }

            //                #endregion
            //            }

            //            public static IServiceCollection AddDashServices(this IServiceCollection services, AppSettings appsettings)
            //            {

            //                //SETTING UP THE PRIVATE DICTIONARY OBJECT 
            //                ReadEnvironmentVariables();


            //                //READING THE VALUE IF VALUE EXIST INITIALIZING IT IN THE APP SETTINGS 
            //                if (EnvironmentVariables[ConstantsEnviormentTest.MyMachineEnviorment] != "Development")
            //                    appsettings.ServerName = Environment.MachineName;

            //                string serverType = EnvironmentVariables[ConstantsEnviormentTest.MyMachineServer];
            //                if (string.IsNullOrWhiteSpace(serverType))
            //                    Log.Error("Kiwi Server Type is required in environment variables.");
            //                else
            //                    appsettings.ServerName = serverType;

            //                return services;
            //            }
            //        }

            #endregion


            #region FOR Self referencing loop 
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            #endregion

            services.AddControllers();
            //This Part is For Filters
            //services.AddControllers(options =>
            //{
            //    options.Filters.Add(typeof(ActionFilter));
            //}).AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());




            #region FOR MODEL STATE DEBUG
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region STEP 1 : DATABASE CONNECTION STRING
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));

            // EXTENSION METHOD
            //services.ConfigureDataBase(Configuration);
            #endregion

            #region STEP 3 :  AUTOMAPPER
            services.AddAutoMapper(typeof(AutoMapperProfile));
            #endregion

            #region STEP 2 : SWAGGER CONFIGURATION
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "THECOMPLETEDOTNETCOREGUIDE", Version = "v1" });
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Core.API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    //Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                    //  Enter 'Bearer' [space] and then your token in the text input below.
                    //  \r\n\r\nExample: 'Bearer 12345abcdef'",    
                    Description = "Description For The Api Goes Here",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
            });

            #endregion

            #region JWT

            //    var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };
            });

            #endregion

            #region SERVICE INITIALIZATION


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped(typeof(IUserRepository), typeof(UserRepository))
            //    .AddScoped(typeof(IUserService), typeof(UserService));

            services.RegisterServices();
            services.RegisterRepositories();

            #endregion

            #region WatchDog Initialization
            services.AddWatchDogServices();

            //services.AddWatchDogServices(opt =>
            //{
            //    opt.IsAutoClear = true;
            //    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
            //});

            //services.AddWatchDogServices(opt =>
            //{
            //    opt.IsAutoClear = false;
            //    opt.SetExternalDbConnString = "Server=localhost;Database=testDb;User Id=postgres;Password=root;";
            //    opt.DbDriverOption = WatchDogSqlDriverEnum.PostgreSql;
            //});

            #endregion

            services.AddTransient<ExceptionMiddleware>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomExceptionFilter v1"));
            }
            app.UseWatchDogExceptionLogger();

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWatchDog(opt =>
            {
                //opt.WatchPageUsername = "admin";
                opt.WatchPageUsername = Configuration["Watchdog:UserName"];
                opt.WatchPagePassword = Configuration["Watchdog:Password"];
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
