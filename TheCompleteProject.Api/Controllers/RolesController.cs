using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DbModels.Role.Dto;
using TheCompleteProject.Service.Services.Role;

namespace TheCompleteProject.Api.Controllers
{
    [Authorize(Roles ="Admin,User")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> Roles()
        {
            var role = await _roleService.GetAllRoles();
            if (!role.Success)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }

        [HttpPost]
        [Route("Roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoles(AddRoleDto roleDto)
        {
            var role = await _roleService.AddRoles(roleDto);
            if (!role.Success)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }

        [HttpPut]
        [Route("Roles")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateRoles(UpdateRoleDto roleDto)
        {
            var role = await _roleService.UpdateRole(roleDto);
            if (!role.Success)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }

        [HttpPost]
        [Route("DeactiveRole/{roleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactiveRole(int roleId)
        {
            var role = await _roleService.DeactiveRole(roleId);
            if (!role.Success)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
    }
}
