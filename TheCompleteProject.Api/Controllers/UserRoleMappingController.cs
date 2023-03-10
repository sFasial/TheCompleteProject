using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.UserRoleMappings.Dto;
using TheCompleteProject.Service.Services.UserRoleMappings;

namespace TheCompleteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleMappingController : ControllerBase
    {
        private readonly IUserRoleMappingService _userRoleMappingService;

        public UserRoleMappingController(IUserRoleMappingService userRoleMappingService)
        {
            _userRoleMappingService = userRoleMappingService;
        }

        [Route("GetAllRolesByUserId/{id}")]
        [HttpGet]
        public IActionResult GetAllRolesByUserId(int id)
        {
            var response = _userRoleMappingService.GetAllRolesByUserId(id);
            return Ok(response);
        }

        [Route("UserRoleMapping")]
        [HttpGet]
        public IActionResult GetUserRoleMapping()
        {
            var response = _userRoleMappingService.GetUserRoleMapping();
            return Ok(response);
        }

        [Route("UserRoleMapping")]
        [HttpPost]
        public IActionResult AddUserRoleMapping(AddUserRoleMapping model)
        {
            var response = _userRoleMappingService.AddUserRoleMapping(model);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Route("UserRoleMapping/{Id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserRoleMappingAsync(UserRoleMappingDto model, int Id)
        {
            var response = await _userRoleMappingService.UpdateUserRoleMappingAsync(model, Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Route("DeactiveUserRoleMapping/{Id}")]
        [HttpPost]
        public async Task<IActionResult> DeactiveUserRoleMapping(int Id)
        {
            var response = await _userRoleMappingService.DeactiveUserRoleMapping(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
