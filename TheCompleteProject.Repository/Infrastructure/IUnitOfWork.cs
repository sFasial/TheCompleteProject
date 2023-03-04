using System;
using System.Threading.Tasks;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Repository.Repositories.User;

namespace TheCompleteProject.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IUserRepository UserRepository { get; }
        IJwtRefreshTokenRepository JwtRefreshTokenRepository { get; }
    }
}
