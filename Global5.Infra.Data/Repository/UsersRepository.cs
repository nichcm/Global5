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
    public class UsersRepository : IUsersRepository
    {
        private readonly CustomDbProfiler _customDbProfiler;
        private readonly string _connectionString;
        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
            _customDbProfiler = new CustomDbProfiler();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<Users> SelectUserByEmail(string email)
        {
            Users model = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@p_email", email);

                model = connection.QueryFirstOrDefault<Users>(UsersQuery.SelectUserByEmail, parameters, commandType: CommandType.StoredProcedure);
            }
            return model;
        }
    }
}
