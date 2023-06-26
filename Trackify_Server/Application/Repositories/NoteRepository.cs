using Application.Interfaces.Contexts;
using Domain.Models.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class NoteRepository
    {
        private readonly IDatabaseContext context;

        public NoteRepository(IDatabaseContext Context)
        {
            context = Context;
        }

        public void CreateNote(string title,string text,string userId,int happiness,int satisfaction,int health)
        {
          
            context.Notes.Add(new Note() {
                Title=title,Text = text, CreatedAt = DateTime.Now, UserId = userId,
                Happiness = happiness, Health = health, Satisfaction = satisfaction, UpdatedAt = DateTime.Now
            });
            context.SaveChanges();

        }

        public void UpdateNote(Guid id,string title,string text, int happiness, int satisfaction, int health)
        {
            Note note = context.Notes.SingleOrDefault(p => p.Id == id);
            note.Text = text;
            note.Title = title;
            note.Happiness = happiness;
            note.Satisfaction = satisfaction;
            note.Health = health;
            note.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public void RemoveNote(Guid id)
        {
            context.Notes.Remove(GetNote(id));
            context.SaveChanges();
        }

        public Note GetNote(Guid id)
        {
            return context.Notes.SingleOrDefault(p=>p.Id == id);
        }

        public List<Note> GetAllUserNotes(string userId,int pageNumber,int pageSize)
        {
            return context.Notes.Where(p => p.UserId == userId).Skip(pageNumber-1).Take(pageSize).ToList();
        }


        public List<Note> GetAllNotes( int pageNumber, int pageSize)
        {
            return context.Notes.Skip(pageNumber - 1).Take(pageSize).ToList();
        }


        public List<Note> GetNotesBasedOnDateRange(string targetUserId,
            DateTime startingDate, DateTime endingDate, int pageNumber, int pageSize)
        {
            return context.Notes.Where(
                p => p.UserId == targetUserId 
                && p.CreatedAt>startingDate 
                && p.CreatedAt<endingDate)
                .Skip(pageNumber - 1)
                .Take(pageSize)
                .ToList();
        }

    }
}
