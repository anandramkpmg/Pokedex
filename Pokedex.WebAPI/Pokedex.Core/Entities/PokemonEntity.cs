using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Core.Entities
{
    public class PokemonEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
        //public bool Exists { get; set; }
    }
}
