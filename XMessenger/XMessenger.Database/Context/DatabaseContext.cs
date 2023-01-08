using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models.Database;
using XMessenger.Helpers.Db.Extensions;
using XMessenger.Helpers.Services;

namespace XMessenger.Database.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly IIdentityService _identityService;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IIdentityService identityService) : base(options)
        {
            Database.EnsureCreated();
            _identityService = identityService;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Metro> Metro { get; set; }
        public DbSet<MetroLine> MetroLines { get; set; }
        public DbSet<MetroStation> MetroStations { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfo(_identityService);

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfo(_identityService);

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
