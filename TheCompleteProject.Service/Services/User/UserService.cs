using System.Collections.Generic;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.Repository.Infrastructure;

namespace TheCompleteProject.Service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
