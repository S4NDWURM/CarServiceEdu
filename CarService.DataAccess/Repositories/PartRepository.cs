using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class PartRepository : IPartRepository
    {
        private readonly CarServiceDbContext _db;
        public PartRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<Part>> Get()
        {
            const string sql = @"
                SELECT ""Id"", ""Name"", ""Article"", ""Cost"", ""PartBrandId""
                  FROM ""Parts""";

            var entities = await _db.Parts
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<Part>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Part.Create(
                    e.Id, e.Name, e.Article, e.Cost, e.PartBrandId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);

                result.Add(model);
            }
            return result;
        }

        public async Task<Part> GetById(Guid id)
        {
            const string sql = @"
        SELECT ""Id"", ""Name"", ""Article"", ""Cost"", ""PartBrandId""
        FROM ""Parts""
        WHERE ""Id"" = {0}";

            var entity = await _db.Parts
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = Part.Create(entity.Id, entity.Name, entity.Article, entity.Cost, entity.PartBrandId);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<Part>> SearchByName(string name)
        {
            const string sql = @"
        SELECT ""Id"", ""Name"", ""Article"", ""Cost"", ""PartBrandId""
        FROM ""Parts""
        WHERE ""Name"" ILIKE {0}"; 

            var entities = await _db.Parts
                                    .FromSqlRaw(sql, $"%{name}%")
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<Part>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Part.Create(e.Id, e.Name, e.Article, e.Cost, e.PartBrandId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);

                result.Add(model);
            }
            return result;
        }


        public async Task<Guid> Create(Part model)
        {
            const string sql = @"
        INSERT INTO ""Parts"" (""Id"", ""Name"", ""Article"", ""Cost"", ""PartBrandId"")
        VALUES ({0}, {1}, {2}, {3}, {4})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.Name, model.Article, model.Cost, model.PartBrandId);
            return model.Id;
        }


        public async Task<Guid> Update(Guid id, string name, string article, decimal cost, Guid partBrandId)
        {
            const string sql = @"
        UPDATE ""Parts""
        SET ""Name"" = {1}, ""Article"" = {2}, ""Cost"" = {3}, ""PartBrandId"" = {4}
        WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, name, article, cost, partBrandId);

            if (rows == 0)
                throw new KeyNotFoundException($"Part with id {id} not found");

            return id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
        DELETE FROM ""Parts""
        WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"Part with id {id} not found");

            return id;
        }

    }
}
