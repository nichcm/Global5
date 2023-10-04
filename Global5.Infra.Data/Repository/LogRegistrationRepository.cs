using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Queries;
using MiniProfiler.Integrations;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Infra.Data.Repository
{
    public class LogRegistrationRepository : ILogRegistrationRepository
    {
        private readonly CustomDbProfiler _customDbProfiler;
        private readonly string _connectionString;
        public LogRegistrationRepository(string connectionString)
        {
            _connectionString = connectionString;
            _customDbProfiler = new CustomDbProfiler();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task InsertLogRegistration(LogRegistration log)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    p_ChangeDate = log.ChangeDate,
                    p_IdUser = log.IdUser,
                    p_ModifiedField = log.ModifiedField,
                    p_OldValue = log.OldValue,
                    p_NewValue = log.NewValue
                };

                await connection.ExecuteAsync("InsertLogRegistration", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
