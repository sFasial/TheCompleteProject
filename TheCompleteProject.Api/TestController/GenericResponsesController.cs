using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheCompleteProject.Api.Controllers;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.Service.Services.GenericResponseService;
using TheCompleteProject.Service.Services.Jwt;

namespace TheCompleteProject.Api.TestController
{
    public class GenericResponsesController : BaseController
    {
        private readonly IGenericResponse _genericResponseService;
        private readonly IJwtRefreshTokenService _jwtRefreshTokenService;
        private readonly IJWTManagerService _jwtManagerService;
        public GenericResponsesController
                            (
                             IGenericResponse genericResponseService
                            , IJwtRefreshTokenService jwtRefreshTokenService
                            , IJWTManagerService jwtManagerService
                            )
        {
            _genericResponseService = genericResponseService;
            _jwtRefreshTokenService = jwtRefreshTokenService;
            _jwtManagerService = jwtManagerService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticateWithGenericResponse")]
        public async Task<Dictionary<string, object>> Authenticate([FromBody] LoginRequestDto login)
        {
            var user = await _genericResponseService.Login(login);
            var userModel = (Users)user.Model;
            if (!user.Success)
            {
                //return BadRequest(user);
                return ApiResponse("EMP100", user);

            }

            //var userRoles = _userRoleMappingService.GetAllRolesByUserId(userModel.Id);

            var token = _jwtManagerService.GenerateToken(userModel.UserName);

            var tokenOne = _jwtManagerService.GenerateToken(userModel); //this is for testing

            if (token == null)
            {
                //return Unauthorized("Invalid Attempt!");
                return ApiResponse("EMP101", "Invalid Attempt");

            }

            // saving refresh token to the db
            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = token.Refresh_Token,
                UserName = userModel.UserName
            };

            await _jwtRefreshTokenService.AddUserRefreshTokens(obj);

            //return Ok(token);
            return ApiResponse("EMP100", token);
        }
    }
}
