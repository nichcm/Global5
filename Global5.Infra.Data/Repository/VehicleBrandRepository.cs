using Dapper;
using Global5.Domain.Entities;
using Global5.Domain.Interfaces.Repository;
using MiniProfiler.Integrations;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    p_IsNational = model.IsNational,
                    p_Status = model.Status,
                    p_BrandName = model.BrandName,
                    p_CreatedBy = model.CreateBy
                };

                var result = await connection.QuerySingleAsync<int>("InsertVehicleBrand", parameters, commandType: CommandType.StoredProcedure);

                model.Id = result;

                return model;
            }
        }

        public async Task<IEnumerable<VehicleBrand>> SelectVehicleBrand(
            bool? national,
            string name,
            bool? active,
            int pageSize,
            int pageNumber)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    p_National = national,
                    p_Name = name,
                    p_Active = active,
                    p_pageSize = pageSize,
                    p_pageNumber = pageNumber
                };

                var brands = await connection.QueryAsync<VehicleBrand>("SelectVehicleBrand", parameters, commandType: CommandType.StoredProcedure);

                return brands;
            }
        }

        public async Task<VehicleBrand> SelectVehicleBrandById(int brandId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    p_scheduleFileId = brandId
                };

                var brand = await connection.QueryFirstOrDefaultAsync<VehicleBrand>("SelectVehicleBrandById", parameters, commandType: CommandType.StoredProcedure);

                return brand;
            }
        }

        public async Task<VehicleBrand> UpdateVehicleBrand(VehicleBrand model)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    p_Id = model.Id,
                    p_IsNational = model.IsNational,
                    p_Status = model.Status,
                    p_BrandName = model.BrandName,
                    p_UpdatedBy = model.UpdateBy
                };

                await connection.ExecuteAsync("UpdateVehicleBrand", parameters, commandType: CommandType.StoredProcedure);

                return model;
            }
        }

        public async Task<IEnumerable<VehicleBrand>> SelectVehicleBrandByName(string brandName)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parameters = new
                {
                    p_BrandName = brandName
                };

                var brands = await connection.QueryAsync<VehicleBrand>("SelectVehicleBrandByName", parameters, commandType: CommandType.StoredProcedure);

                return brands;
            }
        }

        public Task ToggleVehicleBrandActiveStatus(int brandId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new
                {
                    p_Id = brandId
                };

                connection.Execute("ToggleVehicleBrandActiveStatus", parameters, commandType: CommandType.StoredProcedure);
            }

            return Task.CompletedTask;
        }
    }
}
