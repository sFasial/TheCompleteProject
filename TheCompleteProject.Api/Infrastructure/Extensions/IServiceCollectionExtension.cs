using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System.Reflection;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Repository.Repositories.User;
using TheCompleteProject.Service;

namespace TheCompleteProject.Api.Infrastructure.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]
            {
             Assembly.GetExecutingAssembly(),
             Assembly.GetAssembly (typeof (IBaseService))
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .AsPublicImplementedInterfaces();
        }

        public static void ConfigureDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Connection")));      
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IJwtRefreshTokenRepository), typeof(JwtRefreshTokenRepository));
        }
    }
}