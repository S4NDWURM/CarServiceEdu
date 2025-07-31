using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class PlannedWorkEmployeeRepository : IPlannedWorkEmployeeRepository
    {
        private readonly CarServiceDbContext _db;
        public PlannedWorkEmployeeRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<PlannedWorkEmployee>> Get()
        {
            const string sql = @"SELECT ""PlannedWorkId"", ""EmployeeId""
                                  FROM ""PlannedWorkEmployees""";
            var entities = await _db.PlannedWorkEmployees
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<PlannedWorkEmployee>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = PlannedWorkEmployee.Create(e.PlannedWorkId, e.EmployeeId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<List<PlannedWorkEmployee>> GetById(Guid plannedWorkId)
        {
            const string sql = @"
        SELECT ""PlannedWorkId"", ""EmployeeId""
        FROM ""PlannedWorkEmployees""
        WHERE ""PlannedWorkId"" = {0}";

            var entities = await _db.PlannedWorkEmployees
                                    .FromSqlRaw(sql, plannedWorkId)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<PlannedWorkEmployee>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = PlannedWorkEmployee.Create(e.PlannedWorkId, e.EmployeeId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }

            return result;
        }

        public async Task<List<PlannedWorkEmployeeWithDetails>> GetByIdDetailed(Guid plannedWorkId)
        {
            const string sql = @"
        SELECT pw.""PlannedWorkId"", pw.""EmployeeId"", e.""LastName"", e.""FirstName"", e.""MiddleName""
        FROM ""PlannedWorkEmployees"" pw
        INNER JOIN ""Employees"" e ON e.""Id"" = pw.""EmployeeId""
        WHERE pw.""PlannedWorkId"" = {0}";

            var entities = await _db.Set<PlannedWorkEmployeeWithDetails>()
                                     .FromSqlRaw(sql, plannedWorkId)
                                     .AsNoTracking()
                                     .Select(e => new
                                     {
                                         e.PlannedWorkId,
                                         e.EmployeeId,
                                         e.LastName,
                                         e.FirstName,
                                         e.MiddleName
                                     })
                                     .ToListAsync();

            var result = new List<PlannedWorkEmployeeWithDetails>(entities.Count);


            foreach (var e in entities)
            {
                var (model, error) = PlannedWorkEmployeeWithDetails.Create(
                    e.PlannedWorkId, e.EmployeeId, e.LastName, e.FirstName, e.MiddleName);

                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);

                result.Add(model);
            }

            return result;
        }




        public async Task Create(PlannedWorkEmployee model)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO ""PlannedWorkEmployees"" (""PlannedWorkId"", ""EmployeeId"")
                VALUES ({model.PlannedWorkId}, {model.EmployeeId})");
        }

        public async Task Delete(Guid plannedWorkId, Guid employeeId)
        {
            var rows = await _db.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM ""PlannedWorkEmployees""
                 WHERE ""PlannedWorkId"" = {plannedWorkId}
                   AND ""EmployeeId""    = {employeeId}");
            if (rows == 0)
                throw new KeyNotFoundException(
                    $"PlannedWorkEmployee ({plannedWorkId}, {employeeId}) not found");
        }
    }
}
