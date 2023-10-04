using Global5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Domain.Interfaces.Repository
{
    public interface IVehicleBrandRepository : IDisposable
    {
        Task<VehicleBrand> SelectVehicleBrandById(int brandId);
        Task<VehicleBrand> InsertVehicleBrand(VehicleBrand model);
        Task<IEnumerable<VehicleBrand>> SelectVehicleBrand(
                    bool? national,
                    string name,
                    bool? active,
                    int pageSize,
                    int pageNumber);
        Task<VehicleBrand> UpdateVehicleBrand(VehicleBrand model);
        Task<IEnumerable<VehicleBrand>> SelectVehicleBrandByName(string brandName);
        Task ToggleVehicleBrandActiveStatus(int brandId);
    }
}
