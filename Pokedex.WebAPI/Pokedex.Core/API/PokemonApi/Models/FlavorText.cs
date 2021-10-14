using System.Text.Json.Serialization;

namespace Pokedex.Core.API.PokemonApi.Models
{
    public class FlavorText
    {
        [JsonPropertyName("language")]
        public Language Language { get; set; }

        [JsonPropertyName("flavor_text")]
        public string Value { get; set; }
    }
}
