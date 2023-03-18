using Domain.Models.Users;
using MainEndpoint.Models;
using AdminEndpoint.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly CreateToken createToken;
        public AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager,CreateToken createToken)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this.createToken = createToken;
        }

        [HttpPost]
        [Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("لطفا برای نام کاربری از حروف انگلیسی و اعداد استفاده کنید");
            }

            var user = _userManager.FindByNameAsync(model.Email).Result;
            if (user == null || !_userManager.IsInRoleAsync(user, "Administrator").Result)
            {
                return Unauthorized();
            }
            var result = _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, true).Result;
            if (result.Succeeded == false)
            {
                return Unauthorized("نام کاربری یا کلمه عبور نادرست است");
            }

            return Ok(createToken.CreateTokens(user));

        }
    }
}
