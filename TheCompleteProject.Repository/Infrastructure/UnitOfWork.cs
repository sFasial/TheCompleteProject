using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Repository.Repositories.User;

namespace TheCompleteProject.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context;
        public IUserRepository UserRepository { get; }
        public IJwtRefreshTokenRepository JwtRefreshTokenRepository { get; }

        public UnitOfWork(
                           ApplicationDbContext context,
                           IUserRepository userRepository,
                           IJwtRefreshTokenRepository jwtRefreshTokenRepository
                         )
        {
            _context = context;
            UserRepository = userRepository;
            JwtRefreshTokenRepository = jwtRefreshTokenRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
