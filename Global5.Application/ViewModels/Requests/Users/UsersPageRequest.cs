using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.ViewModels.Requests.VehicleBrand
{
    public class UsersPageRequest : ICreatedResponse
    {
        public DateTimeOffset DateScheduleStart { get; set; }
        public DateTimeOffset DateScheduleEnd { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
