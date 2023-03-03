using Application.Services;
using MainEndpoint.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteServices _noteServices;
        public NoteController(INoteServices _noteServices)
        {
            this._noteServices = _noteServices;
        }
        [HttpPost]
        [Route("createnote")]
        public async Task<IActionResult> CreateNote(NoteDto note)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Claims.First(x => x.Type == "UserId").Value;
                _noteServices.CreateNote(note.Text, userId, note.Happiness, note.Satisfaction, note.Health);
                return StatusCode(200);
            }
            else
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        [HttpPost]
        [Route("deletenote")]
        public async Task<IActionResult> DeleteNote(NoteDeleteDto note)
        {

            string userId = User.Claims.First(x => x.Type == "UserId").Value;
            int res = _noteServices.DeleteNote(userId,note.Id);
            if(res == 0)
            {
                return StatusCode(200);
            }
            return BadRequest("Not Your Note");

        }

        [HttpPost]
        [Route("updatenote")]
        public async Task<IActionResult> UpdateNote(NoteUpdateDto note)
        {

            string userId = User.Claims.First(x => x.Type == "UserId").Value;
            int res = _noteServices.UpdateNote(userId,note.Id,note.Text,note.Happiness,note.Satisfaction,note.Health);
            if (res == 0)
            {
                return StatusCode(200);
            }
            return BadRequest("Not Your Note");

        }
    }
}
