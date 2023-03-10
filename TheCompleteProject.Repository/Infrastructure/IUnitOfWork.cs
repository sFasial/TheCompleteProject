using System.Threading.Tasks;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Repository.Repositories.Role;
using TheCompleteProject.Repository.Repositories.User;
using TheCompleteProject.Repository.Repositories.UserRoleMaping;

namespace TheCompleteProject.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleMappingRepository UserRoleMappingRepository { get; }
        IJwtRefreshTokenRepository JwtRefreshTokenRepository { get; }
    }
}
