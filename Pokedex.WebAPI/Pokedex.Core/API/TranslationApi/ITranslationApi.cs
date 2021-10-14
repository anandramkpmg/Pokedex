using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pokedex.Core.API.TranslationApi.Models;
using Refit;

namespace Pokedex.Core.API.TranslationApi
{
    public interface ITranslationApi
    {
        [Post("/translate/shakespeare")]
        public Task<IApiResponse<TranslationResponse>> ToShakespeareAsync([Body] TranslationRequest translationRequest);

        [Post("/translate/yoda")]
        public Task<IApiResponse<TranslationResponse>> ToYodaAsync([Body] TranslationRequest translationRequest);
    }
}
