using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.Service.Services.Jwt;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationWithRefreshTokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;
        private readonly IJwtRefreshTokenService _jwtRefreshTokenService;
        private readonly IJWTManagerService _jwtManagerService;
        private readonly IMapper _mapper;

        public AuthenticationWithRefreshTokenController
               (
                 IUserService userService
               , IOptions<AppSettings> appSettings
               , IConfiguration Configuration
               , IJwtRefreshTokenService jwtRefreshTokenService
               , IJWTManagerService jwtManagerService
               , IMapper mapper
               )
        {
            _userService = userService;
            _configuration = Configuration;
            _jwtRefreshTokenService = jwtRefreshTokenService;
            _jwtManagerService = jwtManagerService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto loginDto)
        {
            var User = new Users();
            _mapper.Map(loginDto, User);

            var validUser = await _jwtRefreshTokenService.IsValidUserAsync(User);
            if (validUser != null)
            {
                //var userRoles = _userRoleMappingService.GetAllRolesByUserId(validUser.Id);

                var token = _jwtManagerService.GenerateToken(validUser.UserName);

                var tokenOne = _jwtManagerService.GenerateToken(User); //this is for testing

                if (token == null)
                {
                    return Unauthorized("Invalid Attempt!");
                }

                // saving refresh token to the db
                UserRefreshTokens obj = new UserRefreshTokens
                {
                    RefreshToken = token.Refresh_Token,
                    UserName = validUser.UserName
                };

                var refreshToken = _jwtRefreshTokenService.AddUserRefreshTokens(obj);

                return Ok(token);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> RefreshAsync(Tokens token)
        {
            var principal = _jwtManagerService.GetPrincipalFromExpiredToken(token.Access_Token);
            var username = principal.Identity?.Name;

            //retrieve the saved refresh token from database
            var savedRefreshToken = await _jwtRefreshTokenService.GetSavedRefreshTokensAsync(username, token.Refresh_Token);

            if (savedRefreshToken.RefreshToken != token.Refresh_Token)
            {
                return Unauthorized("Invalid attempt!");
            }

            var newJwtToken = _jwtManagerService.GenerateRefreshToken(username);

            if (newJwtToken == null)
            {
                return Unauthorized("Invalid attempt!");
            }

            // saving refresh token to the db
            UserRefreshTokens obj = new UserRefreshTokens
            {
                RefreshToken = newJwtToken.Refresh_Token,
                UserName = username
            };

            await _jwtRefreshTokenService.DeleteUserRefreshTokensAsync(username, token.Refresh_Token);
            await _jwtRefreshTokenService.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }
    }
}
