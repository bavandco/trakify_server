using Application.Services;
using MainEndpoint.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserProfileController(IUserServices _userServices)
        {
            this._userServices = _userServices;

        }
        [HttpPost]
        [Route("UploadUserProfilePicture")]
        public async Task<IActionResult> UploadUserProfilePicture([FromForm] IFormFile file)
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(_userServices.CreateUserProfilePicture(userId, file));
        }

        [HttpGet]
        [Route("GetUserProfilePicture")]
        public async Task<IActionResult> GetUserProfilePicture()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var image = _userServices.GetUserProfilePicture(userId);
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            return Ok("No Image");
        }
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if(userId == null)
            {
                return Unauthorized();
            }
            var user = _userServices.GetUserProfile(userId);
            return Ok(user);

        }

    }
}
