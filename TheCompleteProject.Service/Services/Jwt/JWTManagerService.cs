using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.DbModels.Jwt;
using TheCompleteProject.Repository.Repositories.Jwt;

namespace TheCompleteProject.Service.Services.Jwt
{
    public class JWTManagerService : IJWTManagerService
    {
        private readonly IConfiguration iconfiguration;
        private readonly IJwtRefreshTokenRepository _jwtTokenRepository;

        public JWTManagerService(IConfiguration iconfiguration, IJwtRefreshTokenRepository jwtTokenRepository)
        {
            this.iconfiguration = iconfiguration;
            _jwtTokenRepository = jwtTokenRepository;
        }
        public Tokens GenerateToken(string userName)
        {
            return GenerateJWTTokens(userName);
        }

        /// <summary>
        /// YOU CAN DO IT IN BOTH OF THE WAY EITHER BY PASSING USERNAME OR THE USER MODEL THE ABOVE METHOD TAKES USERNAME WHERE AS THIS ONE TAKES USERMODEL
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public Tokens GenerateToken(Users userModel)  
        {
            return GenerateJWTTokens(userModel);
        }

        public Tokens GenerateRefreshToken(string username)
        {
            return GenerateJWTTokens(username);
        }

        public Tokens GenerateJWTTokens(string userName)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                  {
                    new Claim(ClaimTypes.Name, userName)
                  }),
                    Expires = DateTime.Now.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var refreshToken = GenerateRefreshToken();
                return new Tokens
                {
                    Access_Token = tokenHandler.WriteToken(token),
                    Refresh_Token = refreshToken
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Tokens GenerateJWTTokens(Users userModel)
        {
            try
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, userModel.UserName),
                new Claim(ClaimTypes.Email, userModel.Email),
                new Claim("UserId", Convert.ToString(userModel.Id), ClaimValueTypes.Integer),
                new Claim("Email", userModel.Email, ClaimValueTypes.String),
                new Claim("RoleId", Convert.ToString(userModel.RoleId), ClaimValueTypes.Integer),
                //new Claim(ClaimTypes.Role, userRoles.RoleName , ClaimValueTypes.String )
                };

                //foreach (var role in userRoles)
                //{
                //    //claims.Add(new Claim{ClaimTypes.Role, Convert.ToString(userModel.RoleId), ClaimValueTypes.Integer});
                //}

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration["JWT:Secret"]));

                //TOKEN DESCRIPTOR DEFINES HOW THE TOKEN WILL LOOK

                var token = new JwtSecurityToken(
                    issuer: iconfiguration["JWT : ValidIssuer"],
                    audience: iconfiguration["JWT : ValidAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                var refreshToken = GenerateRefreshToken();
                return new Tokens
                {
                    Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Refresh_Token = refreshToken
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(iconfiguration["JWT:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }

    }
}
