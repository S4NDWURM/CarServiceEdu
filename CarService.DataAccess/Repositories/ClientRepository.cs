using CarService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly CarServiceDbContext _db;
        public ClientRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<Client>> Get()
        {
            const string sql = @"
                SELECT ""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""DateOfBirth"", ""RegistrationDate""
                  FROM ""Clients""";
            var entities = await _db.Clients.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var result = new List<Client>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Client.Create(
                    e.Id, e.LastName, e.FirstName, e.MiddleName, e.DateOfBirth, e.RegistrationDate);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<Client> GetById(Guid id)
        {
            const string sql = @"
        SELECT ""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""DateOfBirth"", ""RegistrationDate""
        FROM ""Clients""
        WHERE ""Id"" = {0}";

            var entity = await _db.Clients
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = Client.Create(
                entity.Id, entity.LastName, entity.FirstName, entity.MiddleName, entity.DateOfBirth, entity.RegistrationDate);

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<Guid> Create(Client model)
        {
            const string sql = @"
                INSERT INTO ""Clients""
                (""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""DateOfBirth"", ""RegistrationDate"")
                VALUES
                ({0}, {1}, {2}, {3}, {4}, {5})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.LastName, model.FirstName, model.MiddleName, model.DateOfBirth, model.RegistrationDate);
            return model.Id;
        }

        public async Task<Guid> Update(Guid id, string last, string first, string middle, DateTime dateOfBirth, DateTime registrationDate)
        {
            const string sql = @"
                UPDATE ""Clients""
                SET ""LastName"" = {1}, ""FirstName"" = {2}, ""MiddleName"" = {3}, 
                    ""DateOfBirth"" = {4}, ""RegistrationDate"" = {5}
                WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, last, first, middle, dateOfBirth, registrationDate);

            if (rows == 0)
                throw new KeyNotFoundException($"Client with id {id} not found");

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
                DELETE FROM ""Clients"" WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"Client with id {id} not found");

            return id;
        }
    }
}
