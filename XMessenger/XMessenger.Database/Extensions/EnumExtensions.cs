namespace XMessenger.Database.Extensions
{
    public static class EnumExtensions
    {
        public static string GetName(this Settlement value)
        {
            if (value.Type == SettlementType.Village)
                return $"с. {value.Name}";
            if (value.Type == SettlementType.UrbanVillage)
                return $"смт. {value.Name}";
            if (value.Type == SettlementType.City)
                return $"м. {value.Name}";
            else
                return $"н/в {value.Name}";
        }
    }
}
