using System.Threading.Tasks;
using Pokedex.Core.API.PokemonApi.Models;
using Refit;

namespace Pokedex.Core.API.PokemonApi
{
    public interface IPokemonApi
    {
        [Get("/pokemon-species/{name}")]
        public Task<IApiResponse<Pokemon>> GetSpeciesAsync(string name);
    }
}
