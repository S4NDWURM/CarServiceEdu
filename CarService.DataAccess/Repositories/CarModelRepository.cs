using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly CarServiceDbContext _db;
        public CarModelRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<CarModel>> Get()
        {
            const string sql = @"SELECT ""Id"", ""Name"", ""CarBrandId"" FROM ""CarModels""";
            var entities = await _db.CarModels
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<CarModel>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = CarModel.Create(e.Id, e.Name, e.CarBrandId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<CarModel> GetById(Guid id)
        {
            const string sql = @"SELECT ""Id"", ""Name"", ""CarBrandId"" FROM ""CarModels"" WHERE ""Id"" = {0}";

            var entity = await _db.CarModels
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = CarModel.Create(entity.Id, entity.Name, entity.CarBrandId);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<CarModel>> GetByCarBrandId(Guid carBrandId)
        {
            const string sql = @"SELECT ""Id"", ""Name"", ""CarBrandId"" FROM ""CarModels"" WHERE ""CarBrandId"" = {0}";
            var entities = await _db.CarModels
                                    .FromSqlRaw(sql, carBrandId)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<CarModel>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = CarModel.Create(e.Id, e.Name, e.CarBrandId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }



        public async Task<Guid> Create(CarModel model)
        {
            const string sql = @"
            INSERT INTO ""CarModels"" (""Id"", ""Name"", ""CarBrandId"")
            VALUES ({0}, {1}, {2})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.Name, model.CarBrandId);
            return model.Id;
        }


        public async Task<Guid> Update(Guid id, string name, Guid carBrandId)
        {
            const string sql = @"
            UPDATE ""CarModels""
            SET ""Name"" = {1}, ""CarBrandId"" = {2}
            WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, name, carBrandId);

            if (rows == 0)
                throw new KeyNotFoundException($"CarModel with id {id} not found");

            return id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
            DELETE FROM ""CarModels""
            WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"CarModel with id {id} not found");

            return id;
        }

    }
}
