
using AutoMapper;
using Global5.Application.ViewModels.Requests.Users;
using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Domain.Entities;

namespace Global5.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<UsersRequest, Users>();
            CreateMap<VehicleBrandRequest, VehicleBrand>();
        }
    }
}