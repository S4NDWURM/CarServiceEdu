using CarService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class EmployeeStatusRepository : IEmployeeStatusRepository
    {
        private readonly CarServiceDbContext _db;
        public EmployeeStatusRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<EmployeeStatus>> Get()
        {
            const string sql = @"
                SELECT ""Id"", ""Name""
                  FROM ""EmployeeStatuses""";
            var entities = await _db.EmployeeStatuses.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var result = new List<EmployeeStatus>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = EmployeeStatus.Create(e.Id, e.Name);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<EmployeeStatus> GetById(Guid id)
        {
            const string sql = @"
        SELECT ""Id"", ""Name""
        FROM ""EmployeeStatuses""
        WHERE ""Id"" = {0}";

            var entity = await _db.EmployeeStatuses
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = EmployeeStatus.Create(entity.Id, entity.Name);

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<Guid> Create(EmployeeStatus model)
        {
            const string sql = @"
                INSERT INTO ""EmployeeStatuses""
                (""Id"", ""Name"")
                VALUES
                ({0}, {1})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.Name);
            return model.Id;
        }

        public async Task<Guid> Update(Guid id, string name)
        {
            const string sql = @"
                UPDATE ""EmployeeStatuses""
                SET ""Name"" = {1}
                WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, name);

            if (rows == 0)
                throw new KeyNotFoundException($"EmployeeStatus with id {id} not found");

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
                DELETE FROM ""EmployeeStatuses"" WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"EmployeeStatus with id {id} not found");

            return id;
        }
    }
}
