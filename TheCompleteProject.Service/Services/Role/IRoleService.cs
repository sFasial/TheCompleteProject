using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role.Dto;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.Role
{
    public interface IRoleService
    {
        Task<GenericResponse> GetRoleById(int roleId);
        Task<GenericResponse> GetAllRoles();
        Task<GenericResponse> AddRoles(AddRoleDto roleDto);
        Task<GenericResponse> UpdateRole(UpdateRoleDto roleDto);
        Task<GenericResponse> DeactiveRole(int roleId);
    }
}
