﻿using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface INoteServices
    {
        public void CreateNote(string title, string text, string userId, int happiness, int satisfaction, int health);
        public int DeleteNote(string signedInUserId, Guid noteId);
        public int UpdateNote(string userId, Guid id, string title, string text, int happiness, int satisfaction, int health);
        public GetNoteDto GetNote(string signedInUserId,Guid noteId);
        public GetNoteDto GetLastUserNote(string signedInUserId);
        public List<GetNoteDto> GetAllUserNotes(string signedInUserId,int pageNumber,int pageSize);
        public List<GetNoteDto> GetNotesBasedOnDateRange(string signedInUserId ,DateTime startingDate,DateTime endingDate, int pageNumber, int pageSize);
    }
    public class NoteServices : INoteServices
    {
        private readonly NoteRepository _noteRepo;
        private readonly UserRepository _userRepo;

        public NoteServices(NoteRepository _noteRepo, UserRepository _userRepo)
        {
            this._noteRepo = _noteRepo;
            this._userRepo = _userRepo;
        }
        public void CreateNote(string title, string text, string userId, int happiness, int satisfaction, int health)
        {
            if(GetLastUserNote(userId) != null)
            {
                if (DateTime.Now > GetLastUserNote(userId).CreatedDate.AddDays(2))
                {
                    _userRepo.ZeroOutUserJournalingStreak(userId);
                    _userRepo.IncrementUserJournalingStreak(userId);
                }
                else if (DateTime.Now < GetLastUserNote(userId).CreatedDate.AddDays(1))
                {

                }
                else if (DateTime.Now < GetLastUserNote(userId).CreatedDate.AddDays(2))
                {
                    _userRepo.IncrementUserJournalingStreak(userId);
                }
            }
            else
            {
                _userRepo.IncrementUserJournalingStreak(userId);
            }

            _noteRepo.CreateNote(title,text, userId, happiness, satisfaction, health);
        }

        public int DeleteNote(string signedInUserId, Guid noteId)
        {
            var note = _noteRepo.GetNote(noteId);
            if (note.UserId == signedInUserId)
            {
                _noteRepo.RemoveNote(noteId);
                return 0;
            }
            return 1;
            
        }

        public List<GetNoteDto> GetAllUserNotes(string signedInUserId, int pageNumber, int pageSize)
        {
            List<GetNoteDto> result = new List<GetNoteDto>();
            if(pageNumber >= 1)
            {
                var notes = _noteRepo.GetAllUserNotes(signedInUserId, pageNumber, pageSize);
                foreach (var note in notes)
                {
                    result.Add(new GetNoteDto()
                    {
                        NoteId = note.Id,
                        UserId = note.UserId,
                        CreatedDate = note.CreatedAt,
                        Happiness = note.Happiness,
                        Health = note.Health,
                        Satisfaction = note.Satisfaction,
                        Text = note.Text,
                        Title = note.Title,
                    });
                }
                return result;
            }else
            {
                return null;
            }
            
        }

        public GetNoteDto GetNote(string signedInUserId, Guid noteId)
        {
            var note = _noteRepo.GetNote(noteId);
            if (note.UserId == signedInUserId)
            {
                return new GetNoteDto()
                {
                    UserId = note.UserId,
                    Text = note.Text,
                    Title = note.Title,
                    Satisfaction = note.Satisfaction,
                    CreatedDate = note.CreatedAt,
                    Happiness= note.Happiness,
                    Health= note.Health,
                    NoteId = note.Id,
                    UpdatedDate = note.UpdatedAt

                };
            }
            return null;
        }

        public GetNoteDto GetLastUserNote(string signedInUserId)
        {
            var notes = _noteRepo.GetAllUserNotes(signedInUserId,1,1).SingleOrDefault();
            if (notes!=null&&notes.UserId == signedInUserId)
            {
                return new GetNoteDto()
                {
                    UserId = notes.UserId,
                    Text = notes.Text,
                    Title = notes.Title,
                    Satisfaction = notes.Satisfaction,
                    CreatedDate = notes.CreatedAt,
                    Happiness = notes.Happiness,
                    Health = notes.Health,
                    NoteId = notes.Id,
                    UpdatedDate = notes.UpdatedAt
                };
            }
            return null;
        }

        public List<GetNoteDto> GetNotesBasedOnDateRange(string signedInUserId, DateTime startingDate, DateTime endingDate, int pageNumber, int pageSize)
        {
            List<GetNoteDto> result = new List<GetNoteDto>();
            if (pageNumber >= 1)
            {
                var notes =_noteRepo.GetNotesBasedOnDateRange(signedInUserId, startingDate, endingDate, pageNumber, pageSize);
                foreach (var note in notes)
                {
                    result.Add(new GetNoteDto()
                    {
                        NoteId = note.Id,
                        UserId = note.UserId,
                        CreatedDate = note.CreatedAt,
                        Happiness = note.Happiness,
                        Health = note.Health,
                        Satisfaction = note.Satisfaction,
                        Text = note.Text,
                        Title = note.Title,
                    });
                }
                return result;
            }
            return null;
            
        }

        public int UpdateNote(string userId, Guid id,string title, string text, int happiness, int satisfaction, int health)
        {
            var note = _noteRepo.GetNote(id);
            if(note.UserId == userId)
            {
                _noteRepo.UpdateNote(id,title, text, happiness, satisfaction, health);
                return 0;
            }
            return 1;
            
        }
    }

    public class GetNoteDto
    {
        public Guid NoteId;
        public string Text;
        public string Title;
        public string UserId;
        public int? Happiness;
        public int? Satisfaction;
        public int? Health;
        public DateTime CreatedDate;
        public DateTime UpdatedDate;

    }
}
