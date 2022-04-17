using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XMessenger.Domain.Models.Identity;
namespace XMessenger.Infrastructure.Data.EntityFramework.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(s => new
            {
                s.ClaimId,
                s.RoleId
            });
        }
    }
}