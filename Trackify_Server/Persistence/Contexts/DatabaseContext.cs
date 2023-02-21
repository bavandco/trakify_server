using Application.Interfaces.Contexts;
using Domain.Models.Attributes;
using Domain.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DatabaseContext:IdentityDbContext<User>,IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*            builder.Entity<User>().Property<DateOnly?>("InsertTime");
                        builder.Entity<User>().Property<DateOnly?>("UpdateTime");
            */

            //base.OnModelCreating(builder);





            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    builder.Entity(entityType.Name).Property<DateTime>("InsertTime");
                    builder.Entity(entityType.Name).Property<DateTime>("UpdateTime");
                }
            }
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified || p.State == EntityState.Added);
            foreach (var entry in modifiedEntries)
            {
                var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());
                var inserted = entityType.FindProperty("InsertTime");
                var updated = entityType.FindProperty("UpdateTime");
                if (entry.State == EntityState.Added && inserted != null)
                {
                    entry.Property("InsertTime").CurrentValue = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified && inserted != null)
                {
                    entry.Property("UpdateTime").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
}
