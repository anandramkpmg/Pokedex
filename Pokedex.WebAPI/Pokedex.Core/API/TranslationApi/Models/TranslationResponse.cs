using System.Text.Json.Serialization;

namespace Pokedex.Core.API.TranslationApi.Models
{
    public class TranslationResponse
    {
        [JsonPropertyName("contents")]
        public TranslatedContent Content { get; set; }
    }
}
