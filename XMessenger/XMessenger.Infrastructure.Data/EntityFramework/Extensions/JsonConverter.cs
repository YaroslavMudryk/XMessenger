using System.Text.Json;
namespace XMessenger.Infrastructure.Data.EntityFramework.Extensions
{
    public static class JsonConverter
    {
        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}