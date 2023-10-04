using Global5.Application.Interfaces;
using Global5.Application.ViewModels.Responses;
using Global5.Domain.Entities.Translations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Global5.Application.Services
{
    public class TranslateService : ITranslateService
    {
        private readonly IEnumerable<Language> _languages;

        public TranslateService
        (
            IEnumerable<Language> languages
        )
        {
            _languages = languages;
        }
        public MessageResponse Translate(string key, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = "pt-br";
            }
            var validateMessageResponse = new MessageResponse();

            var model = _languages.Where(x => x.LanguageCode == language).FirstOrDefault();

            if (model != null)
            {
                var response = model.DictionaryMessages.Where(x => x.Key == key).FirstOrDefault();

                if (response != null)
                {
                    validateMessageResponse.Message = response.Value;
                }
            }
            return validateMessageResponse;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}