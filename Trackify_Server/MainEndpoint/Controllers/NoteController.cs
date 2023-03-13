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
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                _noteServices.CreateNote(note.Title,note.Text, userId, note.Happiness, note.Satisfaction, note.Health);
                return StatusCode(200);
            }
            else
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }
        }

        [HttpPost]
        [Route("deletenote")]
        public async Task<IActionResult> DeleteNote(NoteIdDto note)
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
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

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            int res = _noteServices.UpdateNote(userId,note.Id,note.Title,note.Text,note.Happiness,note.Satisfaction,note.Health);
            if (res == 0)
            {
                return StatusCode(200);
            }
            return BadRequest("Not Your Note");

        }

        [HttpGet]
        [Route("getnote")]
        public async Task<IActionResult> GetNote(NoteIdDto note)
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var res =  _noteServices.GetNote(userId, note.Id);
            if (res !=null)
            {
                NoteGetDto noteResult = new NoteGetDto();
                noteResult.Id = res.NoteId;
                noteResult.Text = res.Text;
                noteResult.Title = res.Title;
                noteResult.UpdatedAt = res.UpdatedDate;
                noteResult.CreatedAt = res.CreatedDate;
                noteResult.Health = res.Health;
                noteResult.Happiness = res.Happiness;
                noteResult.Satisfaction = res.Satisfaction;
                noteResult.UserId = userId;
                return Ok(noteResult);
            }
            return BadRequest("Bad Id");

        }

        [HttpGet]
        [Route("getallnotes")]
        public async Task<IActionResult> GetAllNotes(NotesGetDto model)
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var res = _noteServices.GetAllUserNotes(userId, model.PageNumber,model.PageSize);
            var result = new List<NoteGetDto>();
            if (res != null)
            {
                foreach(var note in res)
                {
                    result.Add(
                    new NoteGetDto()
                    {
                    Id = note.NoteId,
                    Text = note.Text,
                    Title = note.Title,
                    UpdatedAt = note.UpdatedDate,
                    CreatedAt = note.CreatedDate,
                    Health = note.Health,
                    Happiness = note.Happiness,
                    Satisfaction = note.Satisfaction,
                    UserId = userId
                    }
                    );
                }

                return Ok(result);
            }
            return BadRequest("Bad Page Size Or Number");

        }

        [HttpGet]
        [Route("getallnotesbetweendates")]
        public async Task<IActionResult> GetAllNotesBetweenDates(NotesGetBetweenDatesDto model)
        {

            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var res = _noteServices.GetNotesBasedOnDateRange(userId, model.StartingDate, model.EndingDate,model.PageNumber,model.PageSize);
            var result = new List<NoteGetDto>();
            if (res != null)
            {
                foreach (var note in res)
                {
                    result.Add(
                    new NoteGetDto()
                    {
                        Id = note.NoteId,
                        Text = note.Text,
                        Title = note.Title,
                        UpdatedAt = note.UpdatedDate,
                        CreatedAt = note.CreatedDate,
                        Health = note.Health,
                        Happiness = note.Happiness,
                        Satisfaction = note.Satisfaction,
                        UserId = userId
                    }
                    );
                }

                return Ok(result);
            }
            return BadRequest("Bad Page Size , Number Or Date Range");

        }
    }
}
