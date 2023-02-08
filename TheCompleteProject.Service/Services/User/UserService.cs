using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Repository.Infrastructure;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;


        public UserService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;

        }

        public async Task<Users> GetUserByForLogin(string email, string password)
        {
            var user = await _unitOfWork.UserRepository.GetDefaultAsync(x => x.Email == email && x.Password == password);
            if (user == null)
            {
                throw new Exception("No Users Find");
            }
            return user;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAsync();
            return users;
        }

        public async Task<Users> AddUserAsync(Users user)
        {
            try
            {
                var userResponse = await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return userResponse;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
