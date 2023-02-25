using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INoteServices
    {
        public void CreateNote(string text, string userId, int happiness, int satisfaction, int health);
        public void DeleteNote(string signedInUserId, Guid noteId);
        public void UpdateNote(string userId,Guid id, string text, int happiness, int satisfaction, int health);
        public GetNoteDto GetNote(string signedInUserId,Guid noteId);
        public List<GetNoteDto> GetAllUserNotes(string signedInUserId,string targetUserId,int pageNumber,int pageSize);
        public List<GetNoteDto> GetNotesBasedOnDateRange(string signedInUserId,string targetUserId,DateTime startingDate,DateTime endingDate, int pageNumber, int pageSize);
    }
    public class NoteServices : INoteServices
    {

    }

    public class GetNoteDto
    {
        public Guid NoteId;
        public string Text;
        public string UserId;
        public int Happiness;
        public int Satisfaction;
        public int Health;
        public DateTime CreatedDate;
    }
}
