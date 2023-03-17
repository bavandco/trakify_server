using Application.Repositories;
using Domain.Helpers;
using Domain.Models.Users;
using MainEndpoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminEndpoint.Token
{
    public class CreateToken
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserTokenRepository userTokenRep;
        private readonly IConfiguration configuration;
        public CreateToken(UserManager<User> _userManager, SignInManager<User> _signInManager, UserTokenRepository userTokenRep, IConfiguration configuration)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this.userTokenRep = userTokenRep;
            this.configuration = configuration;
        }
        public LoginResultDto CreateTokens(User Incominguser)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            var user = _userManager.FindByNameAsync(Incominguser.UserName).Result;
            string role = "";
            if (_userManager.IsInRoleAsync(user, "Administrator").Result)
            {
                role = "Administrator";
            }else
            {
                role = "User";
            }
            var claims = new List<Claim>
                {
                    new Claim ("UserId",user.Id.ToString()),
                    new Claim (ClaimTypes.Role,role),
                };
            string key = Environment.GetEnvironmentVariable("SECRET");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenExp = DateTime.Now.AddMinutes(int.Parse(configuration["JwtSettings:expires"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:validissuer"],
                audience: configuration["JwtSettings:validaudience"],
                expires: tokenExp,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = Guid.NewGuid();


            userTokenRep.SaveToken(new UserToken()
            {
                TokenExp = tokenExp,
                TokenHash = securityHelper.Getsha256Hash(jwtToken),
                UserId = user.Id,
                RefreshToken = securityHelper.Getsha256Hash(refreshToken.ToString()),
                RefreshTokenExp = DateTime.Now.AddDays(30)
            });
            return new LoginResultDto()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.ToString()
            };


        }
    }
}
