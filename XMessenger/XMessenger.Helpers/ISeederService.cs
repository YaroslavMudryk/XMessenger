namespace XMessenger.Helpers
{
    public interface ISeederService
    {
        Task<Result<int>> SeedSystemAsync();
    }
}