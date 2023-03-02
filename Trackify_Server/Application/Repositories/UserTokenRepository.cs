using Application.Interfaces.Contexts;
using Domain.Helpers;
using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class UserTokenRepository
    {
        private readonly IDatabaseContext _context;
        public UserTokenRepository(IDatabaseContext _context)
        {
            this._context = _context;
        }

        public void SaveToken(UserToken userToken)
        {
            _context.UserTokens.Add(userToken);
            _context.SaveChanges();
        }

        public UserToken FindRefreshToken(string RefreshToken)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            string RefreshTokenHash = securityHelper.Getsha256Hash(RefreshToken);
            var userToken = _context.UserTokens.SingleOrDefault(p => p.RefreshToken == RefreshTokenHash);
            return userToken;
        }

        public void DeleteToken(string RefreshToken)
        {
            var token = FindRefreshToken(RefreshToken);
            if (token != null)
            {
                _context.UserTokens.Remove(token);
                _context.SaveChanges();
            }
        }
    }
}
