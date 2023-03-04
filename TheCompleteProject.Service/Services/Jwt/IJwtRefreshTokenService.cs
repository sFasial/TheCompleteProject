using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;

namespace TheCompleteProject.Service.Services.Jwt
{
    public interface IJwtRefreshTokenService
    {
        Task<Users> IsValidUserAsync(Users users);

        Task<UserRefreshTokens> AddUserRefreshTokens(UserRefreshTokens user);

        Task<UserRefreshTokens> GetSavedRefreshTokensAsync(string username, string refreshToken);

        Task DeleteUserRefreshTokensAsync(string username, string refreshToken);
    }
}
