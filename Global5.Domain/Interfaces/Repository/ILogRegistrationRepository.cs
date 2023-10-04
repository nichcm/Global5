using Global5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Domain.Interfaces.Repository
{
    public interface ILogRegistrationRepository : IDisposable
    {
        Task InsertLogRegistration(LogRegistration log);
    }
}
