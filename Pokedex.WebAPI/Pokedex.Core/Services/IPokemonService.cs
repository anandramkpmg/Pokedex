using System.Threading.Tasks;
using Pokedex.Core.Entities;

namespace Pokedex.Core.Services
{
    public interface IPokemonService
    {
        public Task<PokemonEntity> GetPokemon(string name);
        public Task<PokemonEntity> GetTranslatedPokemon(string name);
    }
}
