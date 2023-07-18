using Application.Repositories;
using Domain.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserServices
    {
        public string CreateUserProfilePicture(string UserId, IFormFile file);
        public FileStream GetUserProfilePicture(string userId);
        public UserDto GetUserProfile(string userId);
    }
    public class UserServices : IUserServices
    {
        public static IWebHostEnvironment _env;
        private readonly UserRepository repo;
        private readonly INoteServices noteServices;
        public UserServices(UserRepository repo, IWebHostEnvironment env, INoteServices noteServices)
        {
            this.repo = repo;
            _env = env;
            this.noteServices = noteServices;
        }

        public string CreateUserProfilePicture(string UserId, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    if (!Directory.Exists(_env.WebRootPath + "\\UserProfileImages\\"))
                    {

                        Directory.CreateDirectory(_env.WebRootPath + "\\UserProfilePictures\\");
                    }
                    

                    var currentImage = repo.GetUserProfilePicture(UserId);
                    if (currentImage != null)
                    {
                        if (File.Exists(_env.WebRootPath + currentImage.Src))
                        {
                            File.Delete(_env.WebRootPath + currentImage.Src);
                            repo.RemoveUserProfilePicture(UserId);
                        }
                    }
                    using (FileStream fileStream = System.IO.File.Create(_env.WebRootPath + "\\UserProfilePictures\\" + UserId + file.FileName))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Flush();
                        repo.CreateUserProfilePicture(UserId, "\\UserProfilePictures\\" + UserId + file.FileName);
                        return "\\UserProfilePictures\\" + UserId + file.FileName;
                    }   
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public FileStream GetUserProfilePicture(string userId)
        {
            var image = repo.GetUserProfilePicture(userId);
            var imageFile = System.IO.File.OpenRead(_env.WebRootPath + image.Src);
            return imageFile;
        }
        private int CalculateJournalingStreak(string userId)
        {
            var user = repo.GetUserProfile(userId);
            var lastNoteDatePlus2Days = noteServices.GetLastUserNoteDatePlus2Days(userId);
            if(lastNoteDatePlus2Days != DateTime.MinValue)
            {
                if (lastNoteDatePlus2Days.AddDays(-2) < DateTime.Now.AddDays(-1))
                {
                    repo.ZeroOutUserJournalingStreak(userId);
                    return 0;
                }
                return user.JournalingStreak;
            }
            else { return 0; }

        }


        public UserDto GetUserProfile(string userId)
        {
            var user = repo.GetUserProfile(userId);
            return new UserDto()
            {
                Id = user.Id,
                FristName = user.FristName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Gender = user.Gender,
                GoogleAuthCode = user.GoogleAuthCode,
                JournalingStreak =CalculateJournalingStreak(userId)
            };
        }

    }

    public class UserDto
    {
        public string Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? GoogleAuthCode { get; set; }
        public UserGender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int JournalingStreak { get; set; }

    }
}
