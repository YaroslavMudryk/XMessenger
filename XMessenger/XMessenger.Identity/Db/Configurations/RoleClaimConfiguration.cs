namespace XMessenger.Identity.Db.Configurations
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