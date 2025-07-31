using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class PartBrandRepository : IPartBrandRepository
    {
        private readonly CarServiceDbContext _db;
        public PartBrandRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<PartBrand>> Get()
        {
            const string sql = @"SELECT ""Id"", ""Name"" FROM ""PartBrands""";
            var entities = await _db.PartBrands.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var result = new List<PartBrand>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = PartBrand.Create(e.Id, e.Name);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<PartBrand> GetById(Guid id)
        {
            const string sql = @"SELECT ""Id"", ""Name"" FROM ""PartBrands"" WHERE ""Id"" = {0}";

            var entity = await _db.PartBrands
                                   .FromSqlRaw(sql, id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = PartBrand.Create(entity.Id, entity.Name);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }



        public async Task<Guid> Create(PartBrand model)
        {
            const string sql = @"
        INSERT INTO ""PartBrands"" (""Id"", ""Name"")
        VALUES ({0}, {1})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.Name);
            return model.Id;
        }


        public async Task<Guid> Update(Guid id, string name)
        {
            const string sql = @"
        UPDATE ""PartBrands""
        SET ""Name"" = {1}
        WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, name);

            if (rows == 0)
                throw new KeyNotFoundException($"PartBrand with id {id} not found");

            return id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
        DELETE FROM ""PartBrands""
        WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"PartBrand with id {id} not found");

            return id;
        }

    }
}
