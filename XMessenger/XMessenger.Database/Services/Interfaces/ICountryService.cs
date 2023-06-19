namespace XMessenger.Database.Services.Interfaces
{
    public interface ICountryService
    {
        Task<Result<List<CountryViewModel>>> GetAllContriesAsync();
        Task<Result<CountryViewModel>> CreateAsync(CountryDto country);
        Task<Result<CountryViewModel>> EditAsync(CountryDto country);
        Task<Result<CountryViewModel>> RemoveAsync(int id);
    }
}