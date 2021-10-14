using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pokedex.Core.Entities;

namespace Pokedex.Core.Services
{
    public interface IPokemonService
    {
        public Task<PokemonEntity> GetPokemonAsync(string name);
        public Task<PokemonEntity> GetTranslatedPokemonAsync(string name);
    }
}
