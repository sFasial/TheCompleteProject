using System;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.Repository.Infrastructure;

namespace TheCompleteProject.Service.Services.Jwt
{
    public class JwtRefreshTokenService : IJwtRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        public JwtRefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRefreshTokens> AddUserRefreshTokens(UserRefreshTokens user)
        {
            var userRefreshToken = (UserRefreshTokens)await _unitOfWork.JwtRefreshTokenRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return userRefreshToken;
        }

        public async Task<Users> IsValidUserAsync(Users users)
        {
            var user = await _unitOfWork.UserRepository.GetDefaultAsync(o => o.UserName == users.UserName && o.Password == users.Password);
            if (user == null)
            {
                throw new Exception("BadRequest");
            }
            return user;
        }

        public async Task DeleteUserRefreshTokensAsync(string username, string refreshToken)
        {
            var item = await _unitOfWork.JwtRefreshTokenRepository.GetDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _unitOfWork.JwtRefreshTokenRepository.Delete(item);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<UserRefreshTokens> GetSavedRefreshTokensAsync(string username, string refreshToken)
        {
            return await _unitOfWork.JwtRefreshTokenRepository.GetDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true);
        }
    }
}
