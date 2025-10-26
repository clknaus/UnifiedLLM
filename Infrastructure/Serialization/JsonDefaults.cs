using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Serialization
{
    public static class JsonDefaults
    {
        public static readonly JsonSerializerOptions CachedJsonOptions_PropertyNameCaseInsensitive = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static readonly JsonSerializerOptions CachedJsonOptions_PropertyNamingPolicyCamelCase_DefaultIgnoreConditionWhenWritingNull = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static readonly JsonSerializerOptions CachedJsonOptions_PropertyNamingPolicyCamelCase = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
