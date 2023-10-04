using Global5.Application.ViewModels.Requests.Log;
using Global5.Application.ViewModels.Responses.Token;
using Schedule.Application.ViewModels.Responses.Token;
using System;
using System.Threading.Tasks;

namespace Global5.Application.Interfaces
{
    public interface ILogService : IDisposable
    {
        Task LogHistoryCreate(LogHistoryDataRequest request, TokenUserResponse contextUser);
        Task RegisterError(LogDataRequest request, TokenUserResponse contextUser);
        Task RegisterValidation(LogDataRequest request, TokenUserResponse contextUser);
        Task RegisterInformation(LogDataRequest request, TokenUserResponse contextUser);
    }
}