using FluentValidation.Results;
using Global5.Api.Controllers;
using Global5.Application.Interfaces;
using Global5.Application.ViewModels;
using Global5.Application.ViewModels.Requests.Users;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schedule.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ApiController<UsersController>
    {
        private readonly IUsersService _usersService;

        public UsersController
        (
            IHttpContextAccessor httpContextAccessor,
            ILogService logService,
            ITranslateService translateService,
            IUsersService usersService,
            ClaimsPrincipal userToken) : base(userToken, httpContextAccessor, logService, translateService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthUser([FromBody] UserLoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("user-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("user-name-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("user-password-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                var response = await _usersService.AuthUserLogin(request);

                return CustomResponse<ICreatedResponse>(response, "userLogin");
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(request));
            }
        }
    }
}
