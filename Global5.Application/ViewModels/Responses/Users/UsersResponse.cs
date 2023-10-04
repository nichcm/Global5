using Global5.Application.ViewModels.Responses.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.ViewModels.Responses.Users
{
    public class UsersResponse : BaseResponseAudit, IResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
