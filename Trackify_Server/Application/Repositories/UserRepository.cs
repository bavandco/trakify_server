using Application.Interfaces.Contexts;
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

    }
}
