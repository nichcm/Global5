using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using Global5.Infra.Data.Queries;
using MiniProfiler.Integrations;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Infra.Data.Repository
{
    public class FunctionalityRepository : IFunctionalityRepository
    {
        private readonly CustomDbProfiler _customDbProfiler;
        private readonly string _connectionString;
        public FunctionalityRepository(string connectionString)
        {
            _connectionString = connectionString;
            _customDbProfiler = new CustomDbProfiler();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<bool> CheckFunctionalityExists(int userId, string functionalityCode)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_IdUser", userId);
                parameters.Add("@p_FunctionalityCode", functionalityCode);
                parameters.Add("@p_Exists", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "CheckFunctionalityByUserIdAndCode", // Replace with the actual stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                bool exists = parameters.Get<bool>("@p_Exists");
                return exists;
            }
        }

    }
}
