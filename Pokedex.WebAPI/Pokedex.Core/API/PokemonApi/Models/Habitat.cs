using System.Text.Json.Serialization;

namespace Pokedex.Core.API.PokemonApi.Models
{
    public class Habitat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
