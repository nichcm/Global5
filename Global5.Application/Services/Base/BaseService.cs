using Global5.Application.Interfaces;
using Global5.Application.ViewModels.Responses;

namespace Global5.Application.Services.Base
{
    public class BaseService
    {
        private readonly ITranslateService _translateService;
        public BaseService(
             ITranslateService translateService
        )
        {
            _translateService = translateService;
        }
        protected MessageResponse Translate(string key = "", string language = "")
        {
            return _translateService.Translate(key, language);
        }
    }
}