using System.Text.Json.Serialization;

namespace Pokedex.Core.API.PokemonApi.Models
{
    public class Language
    {
        [JsonPropertyName("iso3166")]
        public string Code { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
