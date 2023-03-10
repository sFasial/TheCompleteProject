using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Repositories.Jwt;
using TheCompleteProject.Repository.Repositories.Role;
using TheCompleteProject.Repository.Repositories.User;
using TheCompleteProject.Repository.Repositories.UserRoleMaping;

namespace TheCompleteProject.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _context;
        public IUserRepository UserRepository { get; }
        public IJwtRefreshTokenRepository JwtRefreshTokenRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserRoleMappingRepository UserRoleMappingRepository { get; }

        public UnitOfWork(
                           ApplicationDbContext context,
                           IUserRepository userRepository,
                           IJwtRefreshTokenRepository jwtRefreshTokenRepository,
                           IRoleRepository roleRepository,
                           IUserRoleMappingRepository userRoleMappingRepository
                         )
        {
            _context = context;
            UserRepository = userRepository;
            JwtRefreshTokenRepository = jwtRefreshTokenRepository;
            RoleRepository = roleRepository;
            UserRoleMappingRepository = userRoleMappingRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
