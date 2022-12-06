using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers.Extensions;
using XMessenger.Helpers.Services;
using XMessenger.Infrastructure.Data.EntityFramework.Configurations;
namespace XMessenger.Infrastructure.Data.EntityFramework.Context
{
    public class IdentityContext : DbContext
    {
        private readonly IIdentityService _identityService;

        public IdentityContext(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Qr> Qrs { get; set; }
        public DbSet<Confirm> Confirms { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<MFA> MFAs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new MFAConfiguration());
        }

        public override int SaveChanges()
        {
            var dateNow = DateTime.Now;

            ApplyUpsertInfo(dateNow);
            ApplyDeleteInfo(dateNow);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var dateNow = DateTime.Now;

            ApplyUpsertInfo(dateNow);
            ApplyDeleteInfo(dateNow);

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyUpsertInfo(DateTime dateTimeNow)
        {
            this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ForEach(entry =>
                {
                    var entity = (BaseModel)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = dateTimeNow;
                        entity.CreatedBy = _identityService.GetUserId();
                        entity.CreatedFromIP = _identityService.GetIP();
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entity.LastUpdatedAt = dateTimeNow;
                        entity.LastUpdatedBy = _identityService.GetUserId();
                        entity.LastUpdatedFromIP = _identityService.GetIP();
                    }

                    entity.Version++;
                });
        }

        private void ApplyDeleteInfo(DateTime dateTimeNow)
        {
            this.ChangeTracker
                .Entries()
                .Where(s => s.Entity is BaseModel && s.State == EntityState.Deleted)
                .ForEach(entry =>
                {
                    var entity = (BaseModel)entry.Entity;
                    if (entity.IsSoftDelete)
                    {
                        entity.IsDeleted = true;
                        entity.DeletedAt = dateTimeNow;
                        entity.DeletedBy = _identityService.GetUserId();
                        entry.State = EntityState.Modified;
                    }
                });
        }
    }
}