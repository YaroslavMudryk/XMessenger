using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers.Extensions;

namespace XMessenger.Infrastructure.Data.EntityFramework.Configurations
{
    public class MFAConfiguration : IEntityTypeConfiguration<MFA>
    {
        public void Configure(EntityTypeBuilder<MFA> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RestoreCodes).HasConversion(
                s => s.ToJson(),
                s => s.FromJson<string[]>());
        }
    }
}