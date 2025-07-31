using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class GenerationRepository : IGenerationRepository
    {
        private readonly CarServiceDbContext _ctx;
        public GenerationRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<Generation>> Get()
        {
            const string sql = @"SELECT ""Id"", ""CarModelId"", ""Name"", ""StartYear"", ""EndYear""
                         FROM ""Generations""";

            var entities = await _ctx.Generations
                                      .FromSqlRaw(sql)
                                      .AsNoTracking()
                                      .ToListAsync();

            var result = new List<Generation>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Generation.Create(e.Id, e.CarModelId, e.Name, e.StartYear, e.EndYear);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<Generation> GetById(Guid id)
        {
            const string sql = @"SELECT ""Id"", ""CarModelId"", ""Name"", ""StartYear"", ""EndYear""
                         FROM ""Generations""
                         WHERE ""Id"" = {0}";

            var entity = await _ctx.Generations
                                   .FromSqlRaw(sql, id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = Generation.Create(entity.Id, entity.CarModelId, entity.Name, entity.StartYear, entity.EndYear);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<Generation>> GetByCarModelId(Guid carModelId)
        {
            const string sql = @"SELECT ""Id"", ""CarModelId"", ""Name"", ""StartYear"", ""EndYear""
                         FROM ""Generations""
                         WHERE ""CarModelId"" = {0}";

            var entities = await _ctx.Generations
                                     .FromSqlRaw(sql, carModelId)
                                     .AsNoTracking()
                                     .ToListAsync();

            var result = new List<Generation>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Generation.Create(e.Id, e.CarModelId, e.Name, e.StartYear, e.EndYear);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }



        public async Task<Guid> Create(Generation model)
        {
            const string sql = @"
                INSERT INTO ""Generations"" (""Id"", ""CarModelId"", ""Name"", ""StartYear"", ""EndYear"")
                VALUES ({0}, {1}, {2}, {3}, {4})";

            await _ctx.Database.ExecuteSqlRawAsync(sql, model.Id, model.CarModelId, model.Name, model.StartYear, model.EndYear);
            return model.Id;
        }


        public async Task<Guid> Update(Guid id, Guid carModelId, string name, int startYear, int endYear)
        {
            const string sql = @"
                UPDATE ""Generations""
                SET ""CarModelId"" = {1}, ""Name"" = {2}, ""StartYear"" = {3}, ""EndYear"" = {4}
                WHERE ""Id"" = {0}";

            var rows = await _ctx.Database.ExecuteSqlRawAsync(sql, id, carModelId, name, startYear, endYear);

            if (rows == 0)
                throw new KeyNotFoundException($"Generation with id {id} not found");

            return id;
        }


        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"DELETE FROM ""Generations"" WHERE ""Id"" = {0}";

            var rows = await _ctx.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"Generation with id {id} not found");

            return id;
        }

    }
}
