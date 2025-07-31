using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class EmployeeSpecializationRepository : IEmployeeSpecializationRepository
    {
        private readonly CarServiceDbContext _db;
        public EmployeeSpecializationRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<EmployeeSpecialization>> Get()
        {
            const string sql = @"SELECT ""EmployeeId"", ""SpecializationId""
                                  FROM ""EmployeeSpecializations""";
            var entities = await _db.EmployeeSpecializations
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<EmployeeSpecialization>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = EmployeeSpecialization.Create(e.EmployeeId, e.SpecializationId);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<EmployeeSpecialization> GetById(Guid employeeId)
        {
            const string sql = @"
        SELECT ""EmployeeId"", ""SpecializationId""
        FROM ""EmployeeSpecializations""
        WHERE ""EmployeeId"" = {0}";

            var entity = await _db.EmployeeSpecializations
                                  .FromSqlRaw(sql, employeeId)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync();

            if (entity == null)
                throw new KeyNotFoundException($"EmployeeSpecialization with EmployeeId {employeeId} not found");

            var (model, error) = EmployeeSpecialization.Create(entity.EmployeeId, entity.SpecializationId);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<EmployeeSpecializationWithDetails>> GetByIdDetailed(Guid employeeId)
        {
            const string sql = @"
                SELECT es.""EmployeeId"", es.""SpecializationId"", s.""Name"", s.""Description""
                FROM ""EmployeeSpecializations"" es
                INNER JOIN ""Specializations"" s ON s.""Id"" = es.""SpecializationId""
                WHERE es.""EmployeeId"" = @employeeId";

            var connection = _db.Database.GetDbConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@employeeId", employeeId));

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var result = new List<EmployeeSpecializationWithDetails>();
                    while (await reader.ReadAsync())
                    {
                        var employeeIdValue = reader.GetGuid(reader.GetOrdinal("EmployeeId"));
                        var specializationId = reader.GetGuid(reader.GetOrdinal("SpecializationId"));
                        var name = reader.GetString(reader.GetOrdinal("Name"));
                        var description = reader.GetString(reader.GetOrdinal("Description"));

                        var (model, error) = EmployeeSpecializationWithDetails.Create(
                            employeeIdValue,
                            specializationId,
                            name,
                            description
                        );

                        if (!string.IsNullOrEmpty(error))
                            throw new InvalidOperationException(error);

                        result.Add(model);
                    }
                    return result;
                }
            }
        }


        public async Task Create(EmployeeSpecialization model)
        {
            const string sql = @"
        INSERT INTO ""EmployeeSpecializations"" (""EmployeeId"", ""SpecializationId"")
        VALUES ({0}, {1})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.EmployeeId, model.SpecializationId);
        }


        public async Task Delete(Guid employeeId, Guid specializationId)
        {
            const string sql = @"
        DELETE FROM ""EmployeeSpecializations""
        WHERE ""EmployeeId"" = {0} AND ""SpecializationId"" = {1}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, employeeId, specializationId);

            if (rows == 0)
                throw new KeyNotFoundException($"EmployeeSpecialization ({employeeId}, {specializationId}) not found");
        }

    }
}
