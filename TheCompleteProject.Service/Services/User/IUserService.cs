using System.Collections.Generic;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels;

namespace TheCompleteProject.Service.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetUsersAsync();
        Task<Users> AddUserAsync(Users user);

        Task<Users> GetUserByForLogin(string email, string password);
    }
}
