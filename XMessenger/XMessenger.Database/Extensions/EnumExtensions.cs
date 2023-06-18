namespace XMessenger.Database.Extensions
{
    public static class EnumExtensions
    {
        public static string GetName(this Settlement value)
        {
            string name;

            if (value.Type == SettlementType.Village)
                name = $"с. {value.Name}";
            else if (value.Type == SettlementType.UrbanVillage)
                name = $"смт. {value.Name}";
            else if (value.Type == SettlementType.City)
                name = $"м. {value.Name}";
            else
                name = $"н/в {value.Name}";

            if (value.OldNames != null && value.OldNames.Count > 0)
                name += $" ({string.Join(", ", value.OldNames)})";

            return name;
        }
    }
}
