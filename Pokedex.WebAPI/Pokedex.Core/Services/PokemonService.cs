using System;
using System.Linq;
using System.Threading.Tasks;
using Pokedex.Core.API.PokemonApi;
using Pokedex.Core.API.TranslationApi;
using Pokedex.Core.API.TranslationApi.Models;
using Pokedex.Core.Entities;
using static System.String;

namespace Pokedex.Core.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApi _pokemonApi;
        private readonly ITranslationApi _translationApi;
        private const string CaveHabitat = "cave";
        public PokemonService(IPokemonApi pokemonApi, ITranslationApi translationApi)
        {
            _pokemonApi = pokemonApi;
            _translationApi = translationApi;
        }

        public async Task<PokemonEntity> GetPokemon(string name)
        {
            if (IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var response = await _pokemonApi.GetSpecies(name.ToLower());

            if (response == null || !response.IsSuccessStatusCode || response.Content == null)
            {
                return new PokemonEntity();
            }

            return new PokemonEntity
            {
                Name = response.Content.Name,
                Description = response.Content.Descriptions?.Where(d => d.Language.Name == "en").FirstOrDefault()?.Value?.Replace("\n", " ") ?? Empty,
                Habitat = response.Content.Habitat.Name,
                IsLegendary = response.Content.IsLegendary
            };
        }

        public async Task<PokemonEntity> GetTranslatedPokemon(string name)
        {
            var pokemon = await GetPokemon(name.ToLowerInvariant());

            if (pokemon == null || IsNullOrEmpty(pokemon.Description)) return pokemon;

            var request = new TranslationRequest
            {
                Text = pokemon.Description
            };

            var response = pokemon.Habitat.ToLowerInvariant() == CaveHabitat || pokemon.IsLegendary ?
                await _translationApi.ToYoda(request) : await _translationApi.ToShakespeare(request);

            if (response is {IsSuccessStatusCode: true})
            {
                pokemon.Description = response.Content is {Content: { }} ? response.Content.Content.TranslatedText : Empty;
            }

            return pokemon;
        }
    }
}
