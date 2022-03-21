using Extensions.DeviceDetector.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XMessenger.Domain.Models.Identity;
using XMessenger.Infrastructure.Data.EntityFramework.Extensions;
namespace XMessenger.Infrastructure.Data.EntityFramework.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.App).HasConversion(
                v => v.ToJson(),
                v => v.FromJson<App>());

            builder.Property(v => v.Location).HasConversion(
                v => v.ToJson(),
                v => v.FromJson<Location>());

            builder.Property(v => v.Client).HasConversion(
                v => v.ToJson(),
                v => v.FromJson<ClientInfo>());
        }
    }
}