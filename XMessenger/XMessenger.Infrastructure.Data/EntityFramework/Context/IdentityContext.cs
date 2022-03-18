using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models.Identity;
using XMessenger.Infrastructure.Data.EntityFramework.Configurations;

namespace XMessenger.Infrastructure.Data.EntityFramework.Context
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}