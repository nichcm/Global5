using ApiConnector;
using Global5.Application.Interfaces;
using Global5.Application.ViewModels.Requests.Log;
using Global5.Application.ViewModels.Responses.Token;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Schedule.Application.ViewModels.Responses.Token;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Global5.Application.Services
{
    public class LogService : ILogService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRegistrationRepository _logRegistrationRepository;
        public LogService
        (
            IConfiguration configuration,
            ILogRegistrationRepository logRegistrationRepository
        )
        {
            _configuration = configuration;
            _logRegistrationRepository = logRegistrationRepository;
        }
        public async Task LogHistoryCreate(LogHistoryDataRequest request, TokenUserResponse contextUser)
        {
            var logRegistration = new LogRegistration(DateTime.Now, contextUser.UserLoginId, request.ActionName, request.OldValue, request.NewValue );
            await _logRegistrationRepository.InsertLogRegistration(logRegistration);

        }
        public async Task RegisterError(LogDataRequest request, TokenUserResponse contextUser)
        {
            var logRegistration = new LogRegistration(DateTime.Now, contextUser.UserLoginId, request.ActionName,null, request.Request);
            await _logRegistrationRepository.InsertLogRegistration(logRegistration);
        }
        public async Task RegisterValidation(LogDataRequest request, TokenUserResponse contextUser)
        {
            var logRegistration = new LogRegistration(DateTime.Now, contextUser.UserLoginId, request.ActionName,null, request.Request);
            await _logRegistrationRepository.InsertLogRegistration(logRegistration);
        }

        public async Task RegisterInformation(LogDataRequest request, TokenUserResponse contextUser)
        {
            var logRegistration = new LogRegistration(DateTime.Now, contextUser.UserLoginId, request.ActionName, null, request.Request);
            await _logRegistrationRepository.InsertLogRegistration(logRegistration);
        }
        public void Dispose()
        {
            _logRegistrationRepository?.Dispose();
        }
    }
}