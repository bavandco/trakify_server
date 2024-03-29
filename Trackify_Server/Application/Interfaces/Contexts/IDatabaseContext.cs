﻿using Domain.Models.Notes;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IDatabaseContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfilePicture> UserProfilePictures { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
