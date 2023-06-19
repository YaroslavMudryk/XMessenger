namespace XMessenger.Identity.Seeder
{
    public class IdentitySeederService : ISeederService
    {
        private readonly IdentityContext _identityDb;
        public IdentitySeederService(IdentityContext identityDb)
        {
            _identityDb = identityDb;
        }

        public async Task<Result<int>> SeedSystemAsync()
        {
            int counter = 0;
            if (!await _identityDb.Roles.AnyAsync())
            {
                await _identityDb.Roles.AddRangeAsync(GetDefaultsRoles());
                counter++;
            }
            if (!await _identityDb.Apps.AnyAsync())
            {
                await _identityDb.Apps.AddRangeAsync(GetDefaultsApp());
                counter++;
            }

            if (counter > 0)
                await _identityDb.SaveChangesAsync();

            return Result<int>.SuccessWithData(counter);
        }

        private IEnumerable<Role> GetDefaultsRoles()
        {
            yield return new Role
            {
                Name = DefaultsRoles.Administrator,
                NameNormalized = DefaultsRoles.Administrator.ToUpper(),
            };
            yield return new Role
            {
                Name = DefaultsRoles.Moderator,
                NameNormalized = DefaultsRoles.Moderator.ToUpper(),
            };
            yield return new Role
            {
                Name = DefaultsRoles.User,
                NameNormalized = DefaultsRoles.User.ToUpper(),
            };
        }

        private IEnumerable<App> GetDefaultsApp()
        {
            var now = DateTime.Now;

            yield return new App
            {
                Name = "Тестовий застосунок",
                Description = "Тестовий застосунок для процесу розробки",
                IsActive = true,
                ShortName = "Тест. зас.",
                ActiveFrom = now,
                ActiveTo = now.AddYears(5),
                Image = "/files/Other.png",
                ClientId = "F94A4E87C1FD23C102800D",
                ClientSecret = "b2e459a6c58a472da53e47a46d6c5ad4c1ecda073aa4402b9724ac55cd65dcc4",
            };
        }
    }
}
