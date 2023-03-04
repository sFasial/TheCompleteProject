
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.Repository.DatabaseContext;
using TheCompleteProject.Repository.Infrastructure;

namespace TheCompleteProject.Repository.Repositories.Jwt
{
    public interface IJwtRefreshTokenRepository : IBaseRepository<UserRefreshTokens>
    {
    }

    public class JwtRefreshTokenRepository : BaseRepository<UserRefreshTokens>, IJwtRefreshTokenRepository
    {
        public JwtRefreshTokenRepository(ApplicationDbContext _context) : base(_context)
        {

        }
    }
}
