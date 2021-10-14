using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.Core.API.PokemonApi.Models
{
    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorText> Descriptions { get; set; }

        [JsonPropertyName("habitat")]
        public Habitat Habitat { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
    }
}
