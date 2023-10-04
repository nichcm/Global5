using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.ViewModels.Requests.VehicleBrand
{
    public class VehicleBrandPageRequest : ICreatedResponse
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Name { get; set; }
        public bool? IsNational { get; set; }
        public bool? Active { get; set; }
    }
}
