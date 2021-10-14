using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Core.Services;

namespace Pokedex.Core
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();
        }
    }
}
