using Domain.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITokenValidator
    {
        Task Execute(TokenValidatedContext context);
    }

    public class TokenValidate : ITokenValidator
    {
        private readonly UserManager<User> _userManager;
        public TokenValidate(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task Execute(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if(claimsIdentity == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("claims not found");
                return;
            }
            var userId = claimsIdentity.FindFirst("UserId").Value;
            if(!Guid.TryParse(userId,out Guid userGuid))
            {
                context.Fail("claims not found");
                return;
            }

        }
    }
}
