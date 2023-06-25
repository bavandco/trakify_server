using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IDashboardServices
    {
    }

    public class DashboardServices : IDashboardServices
    {
        private readonly NoteRepository _noteRepo;

        public DashboardServices(NoteRepository _noteRepo)
        {
            this._noteRepo = _noteRepo;
        }

        public List<ThreeParameters> GetThreeParameterOverTime(string signedInUserId)
        {
            List<ThreeParameters> result = new List<ThreeParameters>();
            var notes = _noteRepo.GetAllUserNotes(signedInUserId, 1, 36500);
            foreach (var note in notes)
            {
                result.Add(new ThreeParameters()
                {
                    dateTime = note.CreatedAt,
                    Happiness = note.Happiness,
                    Satisfaction = note.Satisfaction,
                    Health = note.Health
                });
            }
            return result;
            

        }
    }
    public class ThreeParameters
    {
        public DateTime dateTime { get; set; }
        public int? Happiness;
        public int? Satisfaction;
        public int? Health;
    }
}
