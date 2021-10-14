using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Pokedex.Core;
using Pokedex.Core.API.PokemonApi;
using Pokedex.Core.API.TranslationApi;
using Pokedex.WebAPI.Mappers;
using Refit;


namespace Pokedex.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokedex.WebAPI", Version = "v1" });
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PokemonMapper());
            });

            services.AddSingleton(mapperConfig.CreateMapper());
            services.AddApplicationServices();

            services
                .AddRefitClient<IPokemonApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co/api/v2"));

            services
                .AddRefitClient<ITranslationApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.funtranslations.com"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
