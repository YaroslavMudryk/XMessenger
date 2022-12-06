using System.Text.Json;
using System.Text.Json.Serialization;
namespace XMessenger.Helpers.Extensions
{
    public static class JsonExtension
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data, _options);
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}