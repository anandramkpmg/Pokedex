using AutoMapper;
using Pokedex.Core.Entities;
using Pokedex.WebAPI.Models;

namespace Pokedex.WebAPI.Mappers
{
    public class PokemonMapper : Profile
    {
        public PokemonMapper()
        {
            CreateMap<PokemonEntity, PokemonModel>();
        }
    }
}
