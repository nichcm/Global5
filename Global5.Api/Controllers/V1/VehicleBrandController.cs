using FluentValidation.Results;
using Global5.Api.Controllers;
using Global5.Application.Interfaces;
using Global5.Application.ViewModels;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Domain.Entities;
using Global5.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Schedule.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VehicleBrandController : ApiController<VehicleBrandController>
    {
        private readonly IVehicleBrandService _vehicleBrandService;

        public VehicleBrandController
        (
            IHttpContextAccessor httpContextAccessor,
            ILogService logService,
            ITranslateService translateService,
            IVehicleBrandService vehicleBrandService,
            ClaimsPrincipal userToken) : base(userToken, httpContextAccessor, logService, translateService)
        {
            _vehicleBrandService = vehicleBrandService;
        }

       
        [HttpGet]
        [Route("selectVehicleBrandById")]
        [AllowAnonymous]
        public async Task<IActionResult> SelectVehicleBrandById(int vehicleBrandId)
        {
            try
            {
                var vehicleBrand = await _vehicleBrandService.SelectVehicleBrandById(vehicleBrandId, GetContextUser().UserLoginId);
                CreateHistory(vehicleBrandId, vehicleBrand);
                return CustomResponse<IResponse>(vehicleBrand);
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(vehicleBrandId));
            }
        }

        [HttpPost]
        [Route("selectVehicleBrand")]
        [AllowAnonymous]
        public async Task<IActionResult> SelectVehicleBrand([FromBody] VehicleBrandPageRequest request)
        {
            try
            {
                var vehicleResponse = await _vehicleBrandService.SelectVehicleBrand(request, GetContextUser().UserLoginId);
                CreateHistory(request, vehicleResponse);
                return CustomResponse<ICreatedResponse>(vehicleResponse, "vehicleBrand");
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(request));
            }
        }


        [HttpPost]
        [Route("InsertVehicleBrand")]
        public async Task<IActionResult> InsertVehicleBrand([FromBody] VehicleBrandRequest request)
        {
            try
            {
                if (request == null)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                if (string.IsNullOrWhiteSpace(request.BrandName))
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-name-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                var response = await _vehicleBrandService.SelectVehicleBrandByName(request.BrandName);

                if (response.Any())
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-name-already-in-use").Message));
                    return CustomExceptionValidationResponse(validation);
                }
                var responseVehicle = await _vehicleBrandService.InsertVehicleBrand(request, GetContextUser().UserLoginId);
                CreateHistory(request, responseVehicle);
                return CustomResponse<ICreatedResponse>(responseVehicle, "scheduleFile");
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(request));
            }
        }

        [HttpPut]
        [Route("UpdateVehicleBrand")]
        public async Task<IActionResult> UpdateVehicleBrand([FromBody] VehicleBrandRequest request)
        {
            try
            {
                if (request == null)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                if (string.IsNullOrWhiteSpace(request.BrandName))
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-name-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                var response = await _vehicleBrandService.SelectVehicleBrandByName(request.BrandName);

                if (response.Count() > 1)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-name-already-in-use").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                var responseVehicle = await _vehicleBrandService.UpdateVehicleBrand(request, GetContextUser().UserLoginId);
                CreateHistory(request, responseVehicle);
                return CustomResponse<ICreatedResponse>(responseVehicle, "scheduleFile");
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(request));
            }
        }

        [HttpPut]
        [Route("ToggleActiveVehicleBrand")]
        public async Task<IActionResult> ToggleActiveVehicleBrand([FromBody] VehicleBrandToggleRequest request)
        {
            try
            {
                if (request == null)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }

                if (request.Id == 0)
                {
                    var validation = new ValidationResult();
                    validation.Errors.Add(new ValidationFailure("", Translate("vehiclebrand-id-required").Message));
                    return CustomExceptionValidationResponse(validation);
                }


                await _vehicleBrandService.ToggleVehicleBrandActiveStatus(request.Id, GetContextUser().UserLoginId);
                
                CreateHistory(request, null);

                return CustomResponse<ICreatedResponse>();
            }
            catch (Exception ex)
            {
                return CustomExceptionResponse(ex, JsonMapExtensions.Bind(request));
            }
        }
    }
}
