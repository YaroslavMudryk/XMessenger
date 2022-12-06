﻿using XMessenger.Application.Dtos;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers;
using XMessenger.Helpers.Identity;
using XMessenger.Infrastructure.Data.EntityFramework.Context;

namespace XMessenger.Application.Seeder
{
    public interface ISeederService
    {
        Task<Result<bool>> SeedSystemAsync(); 
    }

    public class BaseSeederService : ISeederService
    {
        private readonly IdentityContext _db;
        public BaseSeederService(IdentityContext db)
        {
            _db = db;
        }

        public async Task<Result<bool>> SeedSystemAsync()
        {
            int counter = 0;
            if (!_db.Roles.Any())
            {
                await _db.Roles.AddRangeAsync(GetDefaultsRoles());
                counter++;
            }
            if (!_db.Apps.Any())
            {
                await _db.Apps.AddRangeAsync(GetDefaultsApp());
                counter++;
            }

            if (counter > 0)
                await _db.SaveChangesAsync();

            return Result<bool>.Success();
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
                Name = "Тестовий заостосунок",
                Description = "Тестовий застосунок для процесу розробки",
                IsActive = true,
                ShortName = "Тест. зас.",
                ActiveFrom = now,
                ActiveTo = now.AddYears(5),
                ClientId = "F94A4E87C1FD23C102800D",
                ClientSecret = "b2e459a6c58a472da53e47a46d6c5ad4c1ecda073aa4402b9724ac55cd65dcc4",
            };
        }
    }
}