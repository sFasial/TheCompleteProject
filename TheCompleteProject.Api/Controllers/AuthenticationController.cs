using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.Service.Services.User;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IUserService userService, IOptions<AppSettings> appSettings, IConfiguration Configuration)
        {
            _userService = userService;
            _configuration = Configuration;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<Dictionary<string, object>> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userService.GetUserByForLogin(loginRequest.email, loginRequest.password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email , user.Email),
                    new Claim(ClaimTypes.Name , user.UserName),
                    new Claim("UserId" , Convert.ToString(user.Id)),
                    new Claim("RoleId" , Convert.ToString(user.RoleId)),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                  //signingCredentials: signingCredential
                );

                //return ApiResponse("100", new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken(token),
                //    expiration = token.ValidTo
                //});

                var _token = new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user

                };
                return ApiResponse("EMP100", _token);
            }
            throw new Exception("No Users Found");
        }
    }
}
