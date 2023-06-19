namespace XMessenger.Database.Db.Configurations
{
    public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
    {
        public void Configure(EntityTypeBuilder<Settlement> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OldNames).HasConversion(
                s => s.ToJson(),
                s => s.FromJson<List<string>>());
        }
    }
}