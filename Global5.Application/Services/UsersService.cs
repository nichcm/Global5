using AutoMapper;
using Global5.Application.Interfaces;
using Global5.Application.Services.Base;
using Global5.Application.ViewModels.Requests.Users;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Application.ViewModels.Responses.Token;
using Global5.Application.ViewModels.Responses.Users;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Repository;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Global5.Application.Services
{
    public class UsersService : BaseService, IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        public UsersService
       (
           IUsersRepository usersRepository,
           ITokenService tokenService,
           IMapper mapper,
           IConfiguration configuration,
           IFunctionalityRepository functionalityRepository,
           ITranslateService translateService) : base(translateService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _usersRepository = usersRepository;
            _tokenService = tokenService;
        }

        public async Task<TokenCreateResponse> AuthUserLogin(UserLoginRequest request)
        {
            TokenCreateResponse tokenResponse = new(null, null, Translate("auth-notfound").Message, false);

            var user = await _usersRepository.SelectUserByEmail(request.Email);

            if(user != null)
            {
                tokenResponse = _tokenService.CreateToken(request.Email, request.Password, user);
                if (tokenResponse != null)
                {
                    return tokenResponse;
                }
            }
            return tokenResponse;
        }

        
        public void Dispose()
        {
            _usersRepository?.Dispose();
        }
    }
}
