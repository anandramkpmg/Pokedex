using System.Text.Json.Serialization;

namespace Pokedex.Core.API.TranslationApi.Models
{
    public class TranslatedContent
    {
        [JsonPropertyName("text")]
        public string OriginalText { get; set; }

        [JsonPropertyName("translated")]
        public string TranslatedText { get; set; }
    }
}
