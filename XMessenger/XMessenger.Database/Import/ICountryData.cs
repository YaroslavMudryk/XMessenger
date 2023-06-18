namespace XMessenger.Database.Import
{
    public interface ICountryData
    {
        Task<Country> ImportCountryDataAsync();
        Task<Country> ExportCountryDataAsync(string name);
    }
}