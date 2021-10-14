using System;
using System.Linq;
using System.Threading.Tasks;
using Pokedex.Core.API.PokemonApi;
using Pokedex.Core.API.TranslationApi;
using Pokedex.Core.API.TranslationApi.Models;
using Pokedex.Core.Entities;

namespace Pokedex.Core.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApi _pokemonApi;
        private readonly ITranslationApi _translationApi;

        public PokemonService(IPokemonApi pokemonApi, ITranslationApi translationApi)
        {
            _pokemonApi = pokemonApi;
            _translationApi = translationApi;
        }

        public async Task<PokemonEntity> GetPokemonAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var response = await _pokemonApi.GetSpeciesAsync(name.ToLower());

            if (response == null || !response.IsSuccessStatusCode)
            {
                return new PokemonEntity();
            }

            return new PokemonEntity
            {
                Name = response.Content.Name,
                Description = response.Content.Descriptions?.Where(d => d.Language.Name == "en").FirstOrDefault()?.Value?.Replace("\n", " ") ?? string.Empty,
                Habitat = response.Content.Habitat.Name,
                IsLegendary = response.Content.IsLegendary
            };
        }

        public async Task<PokemonEntity> GetTranslatedPokemonAsync(string name)
        {
            var pokemon = await GetPokemonAsync(name.ToLowerInvariant());

            if (pokemon != null && !string.IsNullOrEmpty(pokemon.Description))
            {
                var request = new TranslationRequest()
                {
                    Text = pokemon.Description
                };

                var response = pokemon.Habitat.ToLowerInvariant() == "cave" || pokemon.IsLegendary ?
                    await _translationApi.ToYodaAsync(request) : await _translationApi.ToShakespeareAsync(request);

                if (response != null && response.IsSuccessStatusCode)
                {
                    pokemon.Description = response.Content.Content.TranslatedText;
                }
            }

            return pokemon;
        }
    }
}
