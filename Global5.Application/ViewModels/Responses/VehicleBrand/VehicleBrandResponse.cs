using Global5.Application.ViewModels.Responses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.ViewModels.Responses.VehicleBrand
{
    public class VehicleBrandResponse : BaseResponseAudit, IResponse
    {
        public int Id { get; set; }
        public bool IsNational { get; set; }
        public bool Status { get; set; }
        public string BrandName { get; set; }
    }
}
