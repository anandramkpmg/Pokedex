using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Pokedex.Core.API.PokemonApi;
using Pokedex.Core.API.PokemonApi.Models;
using Pokedex.Core.API.TranslationApi;
using Pokedex.Core.API.TranslationApi.Models;
using Pokedex.Core.Services;
using Refit;
using Xunit;

namespace Pokedex.Core.Tests
{
    public class PokemonServiceTests
    {
        private const string PokemonName = "mewtwo";
        private const string InvalidPokemonName = "test";

        private const string CaveHabitat = "cave";
        private const string OriginalDescription = "This is a\ndescription";
        private const string ExpectedDescription = "This is a description";
        
        private const string TranslatedDescirption = "Translated description, this is.";
        private const string UrbanHabitat = "urban";
        
        private const string English = "en";
        private const string French = "fr";

        private readonly Mock<ITranslationApi> _translationMock;
        private readonly Mock<IPokemonApi> _pokemonMock;
        private readonly IPokemonService _pokemonService;

        public PokemonServiceTests()
        {
            _pokemonMock = new Mock<IPokemonApi>();
            _translationMock = new Mock<ITranslationApi>();
            _pokemonService = new PokemonService(_pokemonMock.Object, _translationMock.Object);

        }
      
        [Fact]
        public async Task GetPokemon_PokemonNotFound_ReturnEmptyEntity()
        {
            SetUpPokeApiMock(null, HttpStatusCode.NotFound);

            var entity = await _pokemonService.GetPokemon(InvalidPokemonName);

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task GetTranslatedPokemon_NotFound_ReturnsNull()
        {
            SetUpPokeApiMock(null, HttpStatusCode.NotFound);

            var entity = await _pokemonService.GetTranslatedPokemon(InvalidPokemonName);

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task GetPokemon_NonEnglishDescription_ReturnsEmptyString()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, CaveHabitat, OriginalDescription, French), HttpStatusCode.OK);

            var pokemon = await _pokemonService.GetPokemon(PokemonName);

            Assert.Equal(string.Empty, pokemon.Description);
        }

        [Fact]
        public async Task GetPokemon_ValidName_ReturnsPokemon()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, CaveHabitat, OriginalDescription, English, true), HttpStatusCode.OK);

            var pokemon = await _pokemonService.GetPokemon(PokemonName);

            Assert.NotNull(pokemon);
            Assert.Equal(PokemonName, pokemon.Name);
            Assert.Equal(ExpectedDescription, pokemon.Description);
            Assert.True(pokemon.IsLegendary);
            Assert.Equal(CaveHabitat, pokemon.Habitat);
        }

        [Fact]
        public async Task GetTranslatedPokemon_CaveHabitat_ApplyYodaTranslation()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, CaveHabitat, OriginalDescription, English), HttpStatusCode.OK);
            SetUpTranslationsApiMock(TranslatedDescirption);

            var entity = await _pokemonService.GetTranslatedPokemon(PokemonName);

            Assert.Equal(TranslatedDescirption, entity.Description);

            _translationMock.Verify(m => m.ToYoda(It.IsAny<TranslationRequest>()), Times.Once);
            _translationMock.Verify(m => m.ToShakespeare(It.IsAny<TranslationRequest>()), Times.Never);
        }

        [Fact]
        public async Task GetTranslatedPokemon_IsLegendary_ApplyYodaTranslation()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, UrbanHabitat, OriginalDescription, English, true), HttpStatusCode.OK);
            SetUpTranslationsApiMock(TranslatedDescirption);

            var entity = await _pokemonService.GetTranslatedPokemon(PokemonName);

            Assert.Equal(TranslatedDescirption, entity.Description);

            _translationMock.Verify(m => m.ToShakespeare(It.IsAny<TranslationRequest>()), Times.Never);
            _translationMock.Verify(m => m.ToYoda(It.IsAny<TranslationRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetTranslatedPokemon_NeitherCaveNorLegendary_ApplyShakespeareTranslation()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, UrbanHabitat, OriginalDescription, English), HttpStatusCode.OK);
            SetUpTranslationsApiMock(TranslatedDescirption);

            var entity = await _pokemonService.GetTranslatedPokemon(PokemonName);

            Assert.Equal(TranslatedDescirption, entity.Description);

            _translationMock.Verify(m => m.ToShakespeare(It.IsAny<TranslationRequest>()), Times.Once);
            _translationMock.Verify(m => m.ToYoda(It.IsAny<TranslationRequest>()), Times.Never);
        }

        [Fact]
        public async Task GetTranslatedPokemon_EmptyDescription_ShouldNotCallTheTranslationService()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, CaveHabitat, string.Empty, French), HttpStatusCode.OK);
            SetUpTranslationsApiMock(TranslatedDescirption);

            var entity = await _pokemonService.GetTranslatedPokemon(PokemonName);

            Assert.NotNull(entity);
            Assert.Equal(PokemonName, entity.Name);

            _translationMock.Verify(m => m.ToShakespeare(It.IsAny<TranslationRequest>()), Times.Never);
            _translationMock.Verify(m => m.ToYoda(It.IsAny<TranslationRequest>()), Times.Never);
        }

        [Fact]
        public async Task GetTranslatedPokemon_EmptyName_ThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _pokemonService.GetTranslatedPokemon(string.Empty));
        }

        [Fact]
        public async Task GetTranslatedPokemon_NotSuccessStatus_ShouldApplyYodaTranslation()
        {
            SetUpPokeApiMock(GetNewPokemon(PokemonName, UrbanHabitat, OriginalDescription, English, true), HttpStatusCode.OK);
            SetUpTranslationsApiMock(null);

            var entity = await _pokemonService.GetTranslatedPokemon(PokemonName);

            Assert.Equal(ExpectedDescription, entity.Description);

            _translationMock.Verify(m => m.ToYoda(It.IsAny<TranslationRequest>()), Times.Once);
        }

        private void SetUpPokeApiMock(Pokemon pokemon, HttpStatusCode statusCode)
        {
            _pokemonMock.Setup(mock => mock.GetSpecies(It.IsAny<string>())).ReturnsAsync(() =>
            {
                var response = new Mock<IApiResponse<Pokemon>>();

                response.SetupGet(res => res.StatusCode).Returns(statusCode);
                response.SetupGet(res => res.Content).Returns(pokemon);
                response.SetupGet(res => res.IsSuccessStatusCode).Returns(statusCode == HttpStatusCode.OK);

                return response.Object;
            });
        }

        private static Pokemon GetNewPokemon(string name, string habitat, string description, string language, bool isLegendary = false) => new Pokemon
        {
            Name = name,
            Habitat = new Habitat
            {
                Name = habitat
            },
            Descriptions = description == null ? null : new List<FlavorText>
            {
                new FlavorText
                {
                    Language = new Language
                    {
                        Name = language
                    },
                    Value = description
                }
            },
            IsLegendary = isLegendary
        };

        private void SetUpTranslationsApiMock(string translation)
        {
            _translationMock.Setup(client => client.ToShakespeare(It.IsAny<TranslationRequest>())).ReturnsAsync(GetMockedTranslationResponse(translation));
            _translationMock.Setup(client => client.ToYoda(It.IsAny<TranslationRequest>())).ReturnsAsync(GetMockedTranslationResponse(translation));
        }

        private static IApiResponse<TranslationResponse> GetMockedTranslationResponse(string translation)
        {
            var response = new Mock<IApiResponse<TranslationResponse>>();

            response.SetupGet(r => r.IsSuccessStatusCode).Returns(translation != null);
            response.SetupGet(r => r.Content).Returns(new TranslationResponse
            {
                Content = new TranslatedContent
                {
                    TranslatedText = translation
                }
            });

            return response.Object;
        }
    }
}
