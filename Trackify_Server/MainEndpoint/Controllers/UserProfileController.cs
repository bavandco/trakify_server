using Application.Services;
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
        public async Task<string> UploadUserProfilePicture([FromForm] IFormFile file)
        {
            string userId = User.Claims.First(x => x.Type == "UserId").Value;
            return _userServices.CreateUserProfilePicture(userId, file);

        }
    }
}
