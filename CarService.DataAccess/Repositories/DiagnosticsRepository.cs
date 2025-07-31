using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class DiagnosticsRepository : IDiagnosticsRepository
    {
        private readonly CarServiceDbContext _db;
        public DiagnosticsRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<Diagnostics>> Get()
        {
            const string sql = @"
                SELECT ""Id"", ""DiagnosticsDate"", ""ResultDescription"", ""EmployeeId"", ""RequestId""
                  FROM ""Diagnostics""";
            var entities = await _db.Diagnostics.FromSqlRaw(sql).AsNoTracking().ToListAsync();

            var result = new List<Diagnostics>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Diagnostics.Create(
                    e.Id, e.DiagnosticsDate, e.ResultDescription, e.EmployeeId, e.RequestId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<List<Diagnostics>> GetByRequestId(Guid requestId)
        {
            const string sql = @"
        SELECT ""Id"", ""DiagnosticsDate"", ""ResultDescription"", ""EmployeeId"", ""RequestId""
          FROM ""Diagnostics""
         WHERE ""RequestId"" = {0}";  

            var entities = await _db.Diagnostics.FromSqlRaw(sql, requestId).AsNoTracking().ToListAsync();

            var result = new List<Diagnostics>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = Diagnostics.Create(
                    e.Id, e.DiagnosticsDate, e.ResultDescription, e.EmployeeId, e.RequestId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }


        public async Task<Diagnostics> GetById(Guid id)
        {
            const string sql = @"
        SELECT ""Id"", ""DiagnosticsDate"", ""ResultDescription"", ""EmployeeId"", ""RequestId""
          FROM ""Diagnostics""
         WHERE ""Id"" = {0}"; 

            var entity = await _db.Diagnostics.FromSqlRaw(sql, id).AsNoTracking().FirstOrDefaultAsync();

            if (entity == null)
                throw new KeyNotFoundException($"Diagnostics with id {id} not found");

            var (model, error) = Diagnostics.Create(entity.Id, entity.DiagnosticsDate, entity.ResultDescription, entity.EmployeeId, entity.RequestId);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }


        public async Task<Guid> Create(Diagnostics model)
        {
            const string sql = @"
        INSERT INTO ""Diagnostics"" (""Id"", ""DiagnosticsDate"", ""ResultDescription"", ""EmployeeId"", ""RequestId"")
        VALUES ({0}, {1}, {2}, {3}, {4})";

            var rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql,
                model.Id, model.DiagnosticsDate, model.ResultDescription, model.EmployeeId, model.RequestId);

            if (rowsAffected == 0)
                throw new InvalidOperationException("Ошибка при создании записи");

            return model.Id;
        }


        public async Task<Guid> Update(Guid id, DateTime date, string desc, Guid empId, Guid reqId)
        {
            const string sql = @"
        UPDATE ""Diagnostics""
           SET ""DiagnosticsDate"" = {1}, 
               ""ResultDescription"" = {2},
               ""EmployeeId"" = {3},
               ""RequestId"" = {4}
         WHERE ""Id"" = {0}";

            var rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, id, date, desc, empId, reqId);

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Diagnostics with id {id} not found");

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            const string sql = @"
        DELETE FROM ""Diagnostics"" WHERE ""Id"" = {0}";

            var rowsAffected = await _db.Database.ExecuteSqlRawAsync(sql, id);

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Diagnostics with id {id} not found");

            return id;
        }

    }
}
