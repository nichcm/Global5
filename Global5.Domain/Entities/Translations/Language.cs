
using System.Collections.Generic;

namespace Global5.Domain.Entities.Translations
{
    public class Language
    {
        public string LanguageCode { get; set; }
        public IEnumerable<DictionaryMessage> DictionaryMessages { get; set; }
    }
}