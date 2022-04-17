using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models.Identity;
using XMessenger.Infrastructure.Data.EntityFramework.Configurations;
namespace XMessenger.Infrastructure.Data.EntityFramework.Context
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
        }
    }
}