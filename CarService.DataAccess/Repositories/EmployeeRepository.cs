using CarService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CarServiceDbContext _db;
        public EmployeeRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<Employee>> Get()
        {
            const string sql = @"
                SELECT ""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""WorkExperience"", ""HireDate"", ""EmployeeStatusId""
                  FROM ""Employees""";
            var entities = await _db.Employees.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var result = new List<Employee>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Employee.Create(
                    e.Id, e.LastName, e.FirstName, e.MiddleName, e.WorkExperience, e.HireDate, e.EmployeeStatusId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<Employee> GetById(Guid id)
        {
            const string sql = @"
        SELECT ""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""WorkExperience"", ""HireDate"", ""EmployeeStatusId""
        FROM ""Employees""
        WHERE ""Id"" = {0}";

            var entity = await _db.Employees
                                  .FromSqlRaw(sql, id)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                return null;

            var (model, error) = Employee.Create(
                entity.Id, entity.LastName, entity.FirstName, entity.MiddleName, entity.WorkExperience, entity.HireDate, entity.EmployeeStatusId);

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<Guid> Create(Employee model)
        {
            const string sql = @"
                INSERT INTO ""Employees""
                (""Id"", ""LastName"", ""FirstName"", ""MiddleName"", ""WorkExperience"", ""HireDate"", ""EmployeeStatusId"")
                VALUES
                ({0}, {1}, {2}, {3}, {4}, {5}, {6})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.Id, model.LastName, model.FirstName, model.MiddleName, model.WorkExperience, model.HireDate, model.EmployeeStatus);
            return model.Id;
        }

        public async Task<Guid> Update(Guid id, string last, string first, string middle, int workExperience, DateTime hireDate, Guid employeeStatus)
        {
            const string sql = @"
                UPDATE ""Employees""
                SET ""LastName"" = {1}, ""FirstName"" = {2}, ""MiddleName"" = {3}, 
                    ""WorkExperience"" = {4}, ""HireDate"" = {5}, ""EmployeeStatusId"" = {6}
                WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id, last, first, middle, workExperience, hireDate, employeeStatus);

            if (rows == 0)
                throw new KeyNotFoundException($"Employee with id {id} not found");

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
                DELETE FROM ""Employees"" WHERE ""Id"" = {0}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rows == 0)
                throw new KeyNotFoundException($"Employee with id {id} not found");

            return id;
        }
    }
}
