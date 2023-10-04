using Global5.Application.Interfaces;
using Global5.Application.ViewModels;
using Global5.Application.ViewModels.Requests.Log;
using Global5.Application.ViewModels.Responses;
using Global5.Application.ViewModels.Responses.Body;
using Global5.Application.ViewModels.Responses.Token;
using Global5.Domain.Extensions;
using Azure.Core;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Schedule.Application.ViewModels.Responses.Token;

namespace Global5.Api.Controllers
{
    [Authorize]
    public abstract class ApiController<TController> : ControllerBase
    {
        private readonly ICollection<string> _errors = new List<string>();

        private readonly ClaimsPrincipal _userToken;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogService _logService;
        protected readonly ITranslateService _translateService;
        protected readonly string _requestId;

        public ApiController
        (
            ClaimsPrincipal userToken,
            IHttpContextAccessor httpContextAccessor,
            ILogService logService,
            ITranslateService translateService
        )
        {
            _userToken = userToken;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
            _translateService = translateService;
            _requestId = Guid.NewGuid().ToString();
        }
        protected TokenUserResponse GetContextUser()
        {
            TokenUserResponse tokenUserResponse = new();

            if (_userToken != null && _userToken.Claims.Any())
            {
                if (_userToken.FindFirst("UserId") != null)
                {
                    if (_httpContextAccessor != null)
                    {
                        tokenUserResponse = new TokenUserResponse
                        {
                             UserLoginId = Convert.ToInt32(_userToken.FindFirst("UserLoginId")?.Value)
                            ,Name = CryptoExtensions.Decrypt(_userToken.FindFirst("Name")?.Value)
                            ,Email = CryptoExtensions.Decrypt(_userToken.FindFirst("Email")?.Value)
                            ,AccessToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "").Replace("bearer ", "")
                        };
                    }
                }
            }
            return tokenUserResponse;
        }
        private void CreateHistory(object request, object oldValue)
        {
            if (true)
            {
                var method = this.ControllerContext.HttpContext.Request.Method;

                if ((method == "POST") || (method == "PUT"))
                {
                    var controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    var actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    var requestValue = JsonMapExtensions.Bind(request);
                    var requestOldValue = JsonMapExtensions.Bind(oldValue);

                    if (requestOldValue == "null")
                    {
                        requestOldValue = string.Empty;
                    }

                    LogHistoryDataRequest logHistoryDataRequest = new()
                    {
                        ActionName = controllerName + "/" + actionName,
                        NewValue = requestValue,
                        OldValue = requestOldValue,
                        FunctionalityCode = actionName,
                        Id = string.Empty
                    };
                    _logService.LogHistoryCreate(logHistoryDataRequest, GetContextUser());
                }
            }
        }
        private void CreateLogError(Exception ex, object request)
        {
            if (true)
            {
                var controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                var requestValue = JsonMapExtensions.Bind(request);

                if (requestValue == "null")
                {
                    requestValue = string.Empty;
                }

                LogDataRequest logDataRequest = new()
                {
                    Module = string.Empty,
                    ActionName = controllerName + "/" + actionName,
                    Request = requestValue,
                    Message = ex.Message,
                    Id = string.Empty
                };
                _logService.RegisterError(logDataRequest, GetContextUser());
            }
        }
        private void CreateLogValidation(string message, object request)
        {
            if (true)
            {
                var controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                var requestValue = JsonMapExtensions.Bind(request);

                if (requestValue == "null")
                {
                    requestValue = string.Empty;
                }

                LogDataRequest logDataRequest = new()
                {
                    Module = string.Empty,
                    ActionName = controllerName + "/" + actionName,
                    Request = requestValue,
                    Message = message,
                    Id = string.Empty
                };
                _logService.RegisterValidation(logDataRequest, GetContextUser());
            }
        }
        private void CreateLogInformation(string message)
        {
            if (true)
            {
                var controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = this.ControllerContext.RouteData.Values["action"].ToString();

                LogDataRequest logDataRequest = new()
                {
                    Module = string.Empty,
                    ActionName = controllerName + "/" + actionName,
                    Request = string.Empty,
                    Message = message,
                    Id = string.Empty
                };
                _logService.RegisterInformation(logDataRequest, GetContextUser());
            }
        }
        protected ActionResult CustomNoticeResponse(string codRespuesta, string descRespuesta)
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            return Ok(new ResponseBodyNotice(codRespuesta, descRespuesta));
        }
        protected ActionResult CustomResponse<T>(object result = null, string createdUri = null)
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            if (result == null)
                return NoContent();

            if (typeof(T) == typeof(ICreateResponseToken))
            {
                var response = (ICreateResponseToken)result;

                var nameOfProperty = "IsValid";
                var propertyInfo = response.GetType().GetProperty(nameOfProperty);
                var value = propertyInfo.GetValue(response, null).ToString();

                if (value == "False")
                {
                    return Unauthorized(new ResponseBody(HttpStatusCode.Unauthorized, true, result, null));
                }
                else
                {
                    return Created(createdUri, new ResponseBody(HttpStatusCode.Created, true, result, null));
                }
            }
            
            if (typeof(T) == typeof(ICreateResponseValidation))
            {
                var response = (ICreateResponseValidation)result;

                var nameOfProperty = "IsValid";
                var propertyInfo = response.GetType().GetProperty(nameOfProperty);
                var value = propertyInfo.GetValue(response, null).ToString();

                if (value == "False")
                {
                    return Created(createdUri, new ResponseBody(HttpStatusCode.Forbidden, true, result, null));
                }
                else
                {
                    return Created(createdUri, new ResponseBody(HttpStatusCode.Created, true, result, null));
                }
            }

            if (typeof(T) == typeof(IEnumerable<IResponse>))
            {
                if (((IEnumerable<IResponse>)result).Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(new ResponseBody(HttpStatusCode.OK, true, result, null));
                }
            }

            if (typeof(T) == typeof(IResponse))
            {
                return Ok(new ResponseBody(HttpStatusCode.OK, true, result, null));
            }

            if (typeof(T) == typeof(ICreatedResponse))
            {
                return Created(createdUri, new ResponseBody(HttpStatusCode.Created, true, result, null));
            }

            if (typeof(T) == typeof(Guid))
            {
                return Ok(new ResponseBody(HttpStatusCode.OK, true, result, null));
            }
            return NoContent();
        }
        protected ActionResult CustomExceptionResponse(Exception exception = null, string eventName = "", string request = "", string id = "0")
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            CreateLogError(exception, request);

            AddError(exception.Message);

            return BadRequestResult();
        }
        protected MessageResponse Translate(string key = "", string language = "")
        {
            return _translateService.Translate(key, language);
        }

        protected ActionResult CustomMessageResponse(string param, string message)
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            if (!string.IsNullOrWhiteSpace(param))
            {
                AddError(param + " : " + message);
            }
            else
            {
                AddError(message);
            }

            return BadRequestResult();
        }
        protected ActionResult CustomExceptionValidateResponse(string eventName = "", string message = "", string request = "", string id = "")
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            AddError(message);

            return BadRequestResult();
        }
        protected ActionResult CustomExceptionValidationResponse(ValidationResult validation = null, string eventName = "", string request = "", string id = "")
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            CreateLogValidation(validation.Errors.First().ErrorMessage, request);

            foreach (var error in validation.Errors)
            {
                AddError(error.ErrorMessage);
            }

            return UnprocessableEntityResult();
        }
        protected ActionResult CustomExceptionAccessDeniedResponse(ValidationResult validation)
        {
            if (!Response.Headers.ContainsKey("X-Request-Id"))
                Response.Headers.Add("X-Request-Id", _requestId);

            CreateLogInformation(validation.Errors.First().ErrorMessage);

            foreach (var error in validation.Errors)
            {
                AddError(error.ErrorMessage);
            }
            return AccessDeniedEntityResult();
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AddError(error.ErrorMessage);
            }

            return UnprocessableEntityResult();
        }

        protected void AddError(string erro)
        {
            _errors.Add(erro);
        }

        protected ActionResult BadRequestResult()
        {
            return BadRequest(new ResponseWithErrors(HttpStatusCode.BadRequest, false, _errors.ToArray()));
        }

        protected ActionResult UnprocessableEntityResult()
        {
            if (_errors.Count == 1)
            {
                var error = _errors.ToArray();
                return UnprocessableEntity(new ResponseWithError(HttpStatusCode.UnprocessableEntity, false, error[0]));
            }
            else
            {
                return UnprocessableEntity(new ResponseWithErrors(HttpStatusCode.UnprocessableEntity, false, _errors.ToArray()));
            }
        }
        protected ActionResult AccessDeniedEntityResult()
        {
            if (_errors.Count == 1)
            {
                var error = _errors.ToArray();
                return UnprocessableEntity(new ResponseWithError(HttpStatusCode.Forbidden, false, error[0]));
            }
            else
            {
                return UnprocessableEntity(new ResponseWithErrors(HttpStatusCode.Forbidden, false, _errors.ToArray()));
            }
        }
    }
}