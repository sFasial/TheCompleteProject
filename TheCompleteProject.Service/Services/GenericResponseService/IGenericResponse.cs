using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.Utility.Response;

namespace TheCompleteProject.Service.Services.GenericResponseService
{
    public interface IGenericResponse
    {
        Task<GenericResponse> Login(LoginRequestDto loginDto);
    }
}
