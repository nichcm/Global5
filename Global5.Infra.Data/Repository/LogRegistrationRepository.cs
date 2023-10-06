using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Queries;
using MiniProfiler.Integrations;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_ChangeDate = log.ChangeDate,
                    p_IdUser = log.IdUser,
                    p_ModifiedField = log.ModifiedField,
                    p_OldValue = log.OldValue,
                    p_NewValue = log.NewValue
                };

                await connection.ExecuteAsync(
                    "InsertLogRegistration", // Replace with the actual stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }


    }
}
