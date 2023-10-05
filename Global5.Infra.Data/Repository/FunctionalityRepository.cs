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
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmd = new MySqlCommand("CheckFunctionalityByUserIdAndCode", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("p_IdUser", userId);
                cmd.Parameters.AddWithValue("p_FunctionalityCode", functionalityCode);

                var existsParam = new MySqlParameter("p_Exists", MySqlDbType.Bit);
                existsParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(existsParam);

                await cmd.ExecuteNonQueryAsync();
                UInt64 byteValue = (UInt64)existsParam.Value;

                return byteValue != 0;
            }
        }
    }
}
