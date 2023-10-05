using Global5.Application.ViewModels.Requests.VehicleBrand;
using Global5.Application.ViewModels.Responses.VehicleBrand;
using Global5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.Interfaces
{
    public interface IVehicleBrandService
    {
        Task<VehicleBrandResponse> SelectVehicleBrandById(int vehicleBrandId, int userId);
        Task<VehicleBrandResponse> InsertVehicleBrand(VehicleBrandRequest request, int userId);
        Task<VehicleBrandResponse> UpdateVehicleBrand(VehicleBrandRequest request, int userId);
        Task<IEnumerable<VehicleBrandResponse>> SelectVehicleBrand(VehicleBrandPageRequest request, int userId);
        Task<IEnumerable<VehicleBrandResponse>> SelectVehicleBrandByName(string brandName);
        Task ToggleVehicleBrandActiveStatus(int brandId,int  userId);
    }
}
