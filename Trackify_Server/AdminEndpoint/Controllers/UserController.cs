using AdminEndpoint.Models;
using Application.Repositories;
using Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserRepository repo;
        private readonly UserManager<User> _userManager;
        public UserController(UserRepository repo, UserManager<User> _userManager)
        {
            this.repo = repo;
            this._userManager = _userManager;
        }

        [HttpGet]
        [Route("AdminGetAllUsers")]
        public async Task<IActionResult> GetAllUsers(UsersGetDto dto)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null || !_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId).Result, "Administrator").Result)
            {
                return Unauthorized();
            }
            return Ok(repo.GetAllUsers(dto.pageNumber, dto.pageSize));
        }
    }
}
