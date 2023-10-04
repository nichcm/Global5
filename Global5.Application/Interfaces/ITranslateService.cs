using Global5.Application.ViewModels.Responses;
using System;

namespace Global5.Application.Interfaces
{
    public interface ITranslateService : IDisposable
    {
        MessageResponse Translate(string key = "", string language = "pt-br");
    }
}