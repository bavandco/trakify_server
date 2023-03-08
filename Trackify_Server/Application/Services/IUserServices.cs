using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserServices
    {
    }
    public class UserServices : IUserServices
    {
        private readonly UserRepository repo;
        public UserServices(UserRepository repo)
        {
            this.repo = repo;
        }
    }
}
