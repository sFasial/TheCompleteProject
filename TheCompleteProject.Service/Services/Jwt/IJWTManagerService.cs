using System.Security.Claims;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;

namespace TheCompleteProject.Service.Services.Jwt
{
    public interface IJWTManagerService
    {
        Tokens GenerateToken(string userName);
        Tokens GenerateToken(Users userModel);
        Tokens GenerateRefreshToken(string userName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
