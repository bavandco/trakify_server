using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardServices _dashboardServices;
        private readonly IUserServices _userServices;
        public DashboardController(IDashboardServices _dashboardServices, IUserServices _userServices)
        {
            this._dashboardServices = _dashboardServices;
            this._userServices = _userServices;
        }


        [HttpGet]
        [Route("getthreeparameters")]
        public async Task<IActionResult> GetThreeParametersOverTime()
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var items = _dashboardServices.GetThreeParameterOverTime(userId);
            List<ThreeParameters> res = new List<ThreeParameters>();
            if (items != null)
            {
                foreach(var item in items)
                {
                    res.Add(new ThreeParameters()
                    {
                        dateTime = item.dateTime,
                        Happiness = item.Happiness,
                        Health = item.Health,
                        Satisfaction = item.Satisfaction,
                    });
                }
                return Ok(res);
            }
            return Ok("No Notes");

        }

        [HttpGet]
        [Route("getUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var profile = _userServices.GetUserProfile(userId);

            return Ok(profile);


        }

    }
}
