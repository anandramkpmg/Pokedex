using System.Threading.Tasks;
using Pokedex.Core.API.TranslationApi.Models;
using Refit;

namespace Pokedex.Core.API.TranslationApi
{
    public interface ITranslationApi
    {
        [Post("/translate/shakespeare")]
        public Task<IApiResponse<TranslationResponse>> ToShakespeare([Body] TranslationRequest translationRequest);

        [Post("/translate/yoda")]
        public Task<IApiResponse<TranslationResponse>> ToYoda([Body] TranslationRequest translationRequest);
    }
}
