
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Global5.Application.Interfaces;
using Global5.Application.Services.Base;
using Global5.Domain.Interfaces.Repository;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Application.ViewModels.Responses.VehicleBrand;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Global5.Domain.Entities;
using Global5.Infra.Data.Repository;

namespace Global5.Application.Services
{
    public class VehicleBrandService : BaseService, IVehicleBrandService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IVehicleBrandRepository _vehicleBrandRepository;
        private readonly IFunctionalityRepository _functionalityRepository;


        public VehicleBrandService
        (
            IVehicleBrandRepository vehicleBrandRepository,
            IMapper mapper,
            IConfiguration configuration,
            IFunctionalityRepository functionalityRepository,
            ITranslateService translateService) : base(translateService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _vehicleBrandRepository = vehicleBrandRepository;
            _functionalityRepository = functionalityRepository;

        }
        public async Task<VehicleBrandResponse> SelectVehicleBrandById(int vehicleBrandId)
        {
            return _mapper.Map<VehicleBrandResponse>(await _vehicleBrandRepository.SelectVehicleBrandById(vehicleBrandId));
        }

        public async Task<VehicleBrandResponse> InsertVehicleBrand(VehicleBrandRequest request, int userId)
        {
            var canEdit = await _functionalityRepository.CheckFunctionalityExists(userId, "InsertVehicleBrand");
            if (!canEdit) return null;
            var model = _mapper.Map<VehicleBrand>(request);
            return _mapper.Map<VehicleBrandResponse>(await _vehicleBrandRepository.InsertVehicleBrand(model));
        }
        public async Task<VehicleBrandResponse> UpdateVehicleBrand(VehicleBrandRequest request, int userId)
        {
            var canEdit = await _functionalityRepository.CheckFunctionalityExists(userId, "UpdateVehicleBrand");

            if (!canEdit) return null;
            var model = _mapper.Map<VehicleBrand>(request);
            return _mapper.Map<VehicleBrandResponse>(await _vehicleBrandRepository.UpdateVehicleBrand(model));
        }
        public async Task<IEnumerable<VehicleBrandResponse>> SelectVehicleBrand(VehicleBrandPageRequest request)
        {
            var brands = await _vehicleBrandRepository.SelectVehicleBrand(
                request.IsNational,
                request.Name,
                request.Active,
                request.PageSize,
                request.PageNumber
            );

            return _mapper.Map<IEnumerable<VehicleBrandResponse>>(brands);
        }

        public async Task<IEnumerable<VehicleBrandResponse>> SelectVehicleBrandByName(string brandName)
        {
            var brands = await _vehicleBrandRepository.SelectVehicleBrandByName(brandName);
            return _mapper.Map<IEnumerable<VehicleBrandResponse>>(brands);
        }

        public async Task ToggleVehicleBrandActiveStatus(int brandId, int userId)
        {
            var canEdit = await _functionalityRepository.CheckFunctionalityExists(userId, "UpdateVehicleBrand");
            if (!canEdit) return;
            await _vehicleBrandRepository.ToggleVehicleBrandActiveStatus(brandId);
        }

        public void Dispose()
        {
            _vehicleBrandRepository?.Dispose();
            _functionalityRepository?.Dispose();

        }
    }
}
