﻿using XMessenger.Helpers;

namespace XMessenger.Identity.Seeder
{
    public interface ISeederService
    {
        Task<Result<int>> SeedSystemAsync();
    }
}