using Microsoft.EntityFrameworkCore;
using XMessenger.Database.Context;
using XMessenger.Database.Dtos;
using XMessenger.Database.Services.Interfaces;
using XMessenger.Database.ViewModels;

namespace XMessenger.Database.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly DatabaseContext _db;
        public CountryService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<CountryViewModel>> CreateAsync(CountryDto country)
        {
            if (await _db.Countries.AnyAsync(s => s.Name == country.Name))
                return Result<CountryViewModel>.Error("Country already exist");

            var newCountry = new Country
            {
                Name = country.Name,
                Flag = country.Flag,
                Populations = country.Populations
            };

            await _db.Countries.AddAsync(newCountry);
            await _db.SaveChangesAsync();

            return Result<CountryViewModel>.SuccessWithData(new CountryViewModel { Id = newCountry.Id, Name = newCountry.Name, Flag = newCountry.Flag, Populations = newCountry.Populations });
        }

        public async Task<Result<CountryViewModel>> EditAsync(CountryDto country)
        {
            var countryToEdit = await _db.Countries.FindAsync(country.Id);

            if (countryToEdit == null)
                return Result<CountryViewModel>.NotFound();

            countryToEdit.Name = country.Name;
            countryToEdit.Flag = country.Flag;
            countryToEdit.Populations = country.Populations;

            await _db.SaveChangesAsync();
            return Result<CountryViewModel>.SuccessWithData(new CountryViewModel { Id = countryToEdit.Id, Name = countryToEdit.Name, Flag = countryToEdit.Flag, Populations = countryToEdit.Populations });
        }

        public async Task<Result<List<CountryViewModel>>> GetAllContriesAsync()
        {
            var contries = await _db.Countries.AsNoTracking().ToListAsync();

            return Result<List<CountryViewModel>>.CreatedList(contries.Select(x => new CountryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Flag = x.Flag,
                Populations = x.Populations,
            }).ToList(), Meta.FromMeta(contries.Count, contries.Count));
        }

        public async Task<Result<CountryViewModel>> RemoveAsync(int id)
        {
            var countryToRemove = await _db.Countries.FindAsync(id);
            if (countryToRemove == null)
                return Result<CountryViewModel>.NotFound();

            _db.Countries.Remove(countryToRemove);
            await _db.SaveChangesAsync();
            return Result<CountryViewModel>.SuccessWithData(new CountryViewModel
            {
                Id = countryToRemove.Id,
                Name = countryToRemove.Name,
                Flag = countryToRemove.Flag,
                Populations = countryToRemove.Populations
            });
        }
    }
}
