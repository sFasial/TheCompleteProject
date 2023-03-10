using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings.Dto;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.UserRoleMappings
{
    public interface IUserRoleMappingService
    {
        List<UserRoleMappingDto> GetAllRolesByUserId(int userId);
        IQueryable<object> GetUserRoleMapping();
        GenericResponse AddUserRoleMapping(AddUserRoleMapping model);
        Task<GenericResponse> UpdateUserRoleMappingAsync(UserRoleMappingDto model, int id);
        Task<GenericResponse> DeactiveUserRoleMapping(int id);
    }
}
