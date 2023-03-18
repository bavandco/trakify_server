using Application.Interfaces.Contexts;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class UserRepository
    {
        private readonly IDatabaseContext context;
        public UserRepository(IDatabaseContext context)
        {
            this.context = context;
        }

        public IQueryable<User> GetAllUsers(int pageNumber,int pageSize)
        {
            return context.Users.Skip(pageNumber - 1).Take(pageSize);
        }

        public void CreateUserProfilePicture(string userId,string src)
        {
            UserProfilePicture userProfilePicture = new UserProfilePicture();
            userProfilePicture.UserId = userId;
            userProfilePicture.Src = src;
            context.UserProfilePictures.Add(userProfilePicture);
            context.SaveChanges();
        }

        public UserProfilePicture GetUserProfilePicture(string userId)
        {
            return context.UserProfilePictures.SingleOrDefault(p => p.UserId == userId);
        }

        public void RemoveUserProfilePicture(string userId)
        {
            var userpp = context.UserProfilePictures.SingleOrDefault(m => m.UserId == userId);
            context.UserProfilePictures.Remove(userpp);
            context.SaveChanges();
        }

        public User GetUserProfile(string userId)
        {
            return context.Users.SingleOrDefault(p=>p.Id == userId);
            
        }
    }
}
