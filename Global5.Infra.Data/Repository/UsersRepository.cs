using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using MiniProfiler.Integrations;
using System;
using System.Data;
using System.Data.SqlClient;
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

            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_email", email);

                model = await connection.QueryFirstOrDefaultAsync<Users>(
                    "SelectUserByEmail", // Replace with the actual stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            return model;
        }


    }
}
