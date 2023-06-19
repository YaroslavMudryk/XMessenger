namespace XMessenger.Identity.Db.Configurations
{
    public class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
    {
        public void Configure(EntityTypeBuilder<LoginAttempt> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(field => field.Location, builder =>
            {
                builder.ToJson();
            });

            builder.OwnsOne(field => field.Device, builder =>
            {
                builder.ToJson();

                builder.OwnsOne(obj => obj.Device);
                builder.OwnsOne(obj => obj.Browser);
                builder.OwnsOne(obj => obj.OS);
            });
        }
    }
}