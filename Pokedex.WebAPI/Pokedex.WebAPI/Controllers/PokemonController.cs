using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Core.Services;
using Pokedex.WebAPI.Models;

namespace Pokedex.WebAPI.Controllers
{
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonService pokemonService, IMapper mapper)
        {
            _pokemonService = pokemonService;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string name)
        {
            var pokemon = await _pokemonService.GetPokemon(name);

            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PokemonModel>(pokemon));
        }

        [HttpGet("translated/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTranslatedPokemon(string name)
        {
            var entity = await _pokemonService.GetTranslatedPokemon(name);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PokemonModel>(entity));
        }
    }
}
