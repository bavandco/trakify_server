﻿using Application.Interfaces.Contexts;
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
        IDatabaseContext context;
        public NoteRepository(IDatabaseContext Context)
        {
            context = Context;
        }

        public void CreateNote(string text,string userId,int happiness,int satisfaction,int health)
        {
            context.Notes.Add(new Note() {
                Text = text, CreatedAt = DateTime.Now, UserId = userId,
                Happiness = happiness, Health = health, Satisfaction = satisfaction, UpdatedAt = DateTime.Now
            });
            context.SaveChanges();

        }

        public void UpdateNote(Guid id,string text, int happiness, int satisfaction, int health)
        {
            Note note = context.Notes.SingleOrDefault(p => p.Id == id);
            note.Text = text;
            note.Happiness = happiness;
            note.Satisfaction = satisfaction;
            note.Health = health;
            note.UpdatedAt = DateTime.Now;
            context.SaveChanges();
        }

        public void RemoveNote(Guid id)
        {
            context.Notes.Remove(new Note() { Id = id });
            context.SaveChanges();
        }

        public Note GetNote(Guid id)
        {
            return context.Notes.SingleOrDefault(p=>p.Id == id);
        }

        public List<Note> GetUserNotes(string userId,int pageNumber,int pageSize)
        {
            return context.Notes.Where(p => p.UserId == userId).Skip(pageNumber-1).Take(pageSize).ToList();
        }

    }
}
