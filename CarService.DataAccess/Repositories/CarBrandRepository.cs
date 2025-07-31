using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class CarBrandRepository : ICarBrandRepository
    {
        private readonly CarServiceDbContext _db;

        public CarBrandRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<CarBrand>> Get()
        {
            const string sql = @"SELECT ""Id"", ""Name"" FROM ""CarBrands""";
            var entities = await _db.CarBrands
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<CarBrand>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = CarBrand.Create(e.Id, e.Name);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<CarBrand> GetById(Guid id)
        {
            const string sql = @"SELECT ""Id"", ""Name"" FROM ""CarBrands"" WHERE ""Id"" = {0}";

            var entity = await _db.CarBrands
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = CarBrand.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(CarBrand model)
        {
            const string sql = @"INSERT INTO ""CarBrands"" (""Id"", ""Name"")
                         VALUES ({0}, {1})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.Name);
            return model.Id;
        }


        public async Task<Guid> Update(Guid id, string name)
        {
            const string sql = @"UPDATE ""CarBrands""
                         SET ""Name"" = {1}
                         WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, name);

            if (rows == 0)
                throw new KeyNotFoundException($"CarBrand with id {id} not found");

            return id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"DELETE FROM ""CarBrands"" WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"CarBrand with id {id} not found");

            return id;
        }

    }
}
