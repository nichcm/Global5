using Global5.Application.ViewModels.Requests.Users;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Application.ViewModels.Responses.Token;
using Global5.Application.ViewModels.Responses.Users;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.Interfaces
{
    public interface IUsersService : IDisposable
    {
        Task<TokenCreateResponse> AuthUserLogin(UserLoginRequest request);

    }
}
