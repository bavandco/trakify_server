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
        public string F_Name { get; set; }
    }
}
