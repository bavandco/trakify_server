using Application.Repositories;
using Domain.Models.Users;
using MainEndpoint.Models;
using MainEndpoint.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration configuration;
        private readonly UserTokenRepository userTokenRep;
        private readonly CreateToken createToken;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, UserTokenRepository userTokenRep, CreateToken createToken)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
            this.userTokenRep = userTokenRep;
            this.createToken = createToken;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register( RegisterModel model)
        {

            if (model.Password == model.ConfirmPassword)
            {
                User newUser = new User()
                {
                    Email = model.Email,
                    BirthDate = model.BirthDate,
                    FristName = model.FristName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    UserName = model.Email,
                    GoogleAuthCode = model.GoogleAuthCode,
                };
                newUser.Id = Guid.NewGuid().ToString();
                var result = _userManager.CreateAsync(newUser, model.Password).Result;
                if (result.Succeeded)
                {
                    return Ok(createToken.CreateTokens(newUser));
                }
                return Unauthorized(result.Errors);
            }
            else
            {
                IdentityError err = new IdentityError();
                err.Code = "PasswordMisMatch";
                err.Description = "رمز عبور و تایید رمز عبور باید همسان باشند";
                return BadRequest(err);
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login( LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("لطفا برای نام کاربری از حروف انگلیسی و اعداد استفاده کنید");
            }

            var user = _userManager.FindByNameAsync(model.Email).Result;
            if (user == null)
            {
                return Unauthorized("نام کاربری یا کلمه عبور نادرست است");
            }
            var result = _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, true).Result;
            if (result.Succeeded == false)
            {
                return Unauthorized("نام کاربری یا کلمه عبور نادرست است");
            }

            return Ok(createToken.CreateTokens(user));

        }
        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(string RefreshToken)
        {
            var userToken = userTokenRep.FindRefreshToken(RefreshToken);
            if (userToken == null)
            {
                return Unauthorized();
            }
            if (userToken.RefreshTokenExp < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }
            var user = _userManager.FindByIdAsync(userToken.UserId).Result;
            var token = createToken.CreateTokens(user);
            userTokenRep.DeleteToken(RefreshToken);
            return Ok(token);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index");

        }
    }
}
