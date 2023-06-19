namespace XMessenger.Identity.Db.Configurations
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