using AutoMapper;
using Global5.Application.ViewModels.Responses.Users;
using Global5.Application.ViewModels.Responses.VehicleBrand;
using Global5.Domain.Entities;

namespace Global5.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMap<Users, UsersResponse>();
            CreateMap<VehicleBrand, VehicleBrandResponse>();

        }
    }
}