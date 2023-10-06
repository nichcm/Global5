using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using MiniProfiler.Integrations;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Infra.Data.Repository
{
    public class VehicleBrandRepository : IVehicleBrandRepository
    {

        private readonly CustomDbProfiler _customDbProfiler;
        private readonly string _connectionString;
        public VehicleBrandRepository(string connectionString)
        {
            _connectionString = connectionString;
            _customDbProfiler = new CustomDbProfiler();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<VehicleBrand> InsertVehicleBrand(VehicleBrand model)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_IsNational = model.IsNational,
                    p_Status = model.Status,
                    p_BrandName = model.BrandName,
                    p_CreatedBy = model.CreateBy
                };

                var result = await connection.QuerySingleAsync<int>(
                    "InsertVehicleBrand", 
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                model.Id = result;

                return model;
            }
        }


        public async Task<IEnumerable<VehicleBrand>> SelectVehicleBrand(int pageSize, int pageNumber)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_pageSize = pageSize,
                    p_pageNumber = pageNumber
                };

                var brands = await connection.QueryAsync<VehicleBrand>(
                    "SelectVehicleBrand",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return brands;
            }
        }


        public async Task<VehicleBrand> SelectVehicleBrandById(int brandId)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_scheduleFileId = brandId
                };

                var brand = await connection.QueryFirstOrDefaultAsync<VehicleBrand>(
                    "SelectVehicleBrandById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return brand;
            }
        }


        public async Task<IEnumerable<VehicleBrand>> SelectVehicleBrandByName(string brandName)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_BrandName = brandName
                };

                var brands = await connection.QueryAsync<VehicleBrand>(
                    "SelectVehicleBrandByName", // Replace with the actual stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return brands;
            }
        }

        public async Task ToggleVehicleBrandActiveStatus(int brandId)
        {
            var factory = new SqlServerDbConnectionFactory(_connectionString);
            using (var connection = ProfiledDbConnectionFactory.New(factory, _customDbProfiler))
            {
                var parameters = new
                {
                    p_Id = brandId
                };

                await connection.ExecuteAsync(
                    "ToggleVehicleBrandActiveStatus", // Replace with the actual stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }


    }
}
