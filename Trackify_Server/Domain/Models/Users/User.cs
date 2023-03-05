using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Models.Users
{
    public class User:IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string? GoogleAuthCode { get; set; }
        public UserGender Gender { get; set; }
        public DateTime BirthDate { get; set; }

    }

    public enum UserGender
    {
        MALE,
        FEMALE,
        OTHER
    }
}
