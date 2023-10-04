using Global5.Application.ViewModels.Responses.Token;
using Global5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.Interfaces
{
    public interface ITokenService
    {
        TokenCreateResponse CreateToken(string login, string pass, Users model);
    }
}
