using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XMessenger.Domain.Models.Identity;
namespace XMessenger.Infrastructure.Data.EntityFramework.Configurations
{
    public class MFAConfiguration : IEntityTypeConfiguration<MFA>
    {
        public void Configure(EntityTypeBuilder<MFA> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(field => field.RestoreCodes, builder =>
            {
                builder.ToJson();
            });
        }
    }
}