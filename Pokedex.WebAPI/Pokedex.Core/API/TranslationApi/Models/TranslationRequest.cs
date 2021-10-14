using System.Text.Json.Serialization;

namespace Pokedex.Core.API.TranslationApi.Models
{
    public class TranslationRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
