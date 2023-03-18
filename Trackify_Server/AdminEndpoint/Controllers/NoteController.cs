using AdminEndpoint.Models;
using Application.Repositories;
using Domain.Models.Users;
using MainEndpoint.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteRepository repo;
        private readonly UserManager<User> _userManager;
        public NoteController(NoteRepository repo, UserManager<User> _userManager)
        {
            this.repo = repo;
            this._userManager = _userManager;
        }

        [HttpGet]
        [Route("AdminGetAllNotes")]
        public async Task<IActionResult> GetAllNotes(NotesGetDto dto)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null || !_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId).Result,"Administrator").Result)
            {
                return Unauthorized();
            }
            return Ok(repo.GetAllNotes(dto.PageNumber,dto.PageSize));
        }



        [HttpGet]
        [Route("AdminGetUserNotes")]
        public async Task<IActionResult> GetUserNotes(AdminGetUserNotesDto dto)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null || !_userManager.IsInRoleAsync(_userManager.FindByIdAsync(userId).Result, "Administrator").Result)
            {
                return Unauthorized();
            }
            return Ok(repo.GetAllUserNotes(dto.UserId,dto.PageNumber, dto.PageSize));
        }
    }
}
