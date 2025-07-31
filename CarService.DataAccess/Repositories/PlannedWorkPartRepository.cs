using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Repositories
{
    public class PlannedWorkPartRepository : IPlannedWorkPartRepository
    {
        private readonly CarServiceDbContext _db;
        public PlannedWorkPartRepository(CarServiceDbContext db) => _db = db;

        public async Task<List<PlannedWorkPart>> Get()
        {
            const string sql = @"SELECT ""PlannedWorkId"", ""PartId"", ""Quantity""
                                  FROM ""PlannedWorkParts""";
            var entities = await _db.PlannedWorkParts
                                    .FromSqlRaw(sql)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<PlannedWorkPart>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = PlannedWorkPart.Create(e.PlannedWorkId, e.PartId, e.Quantity);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }
            return result;
        }

        public async Task<List<PlannedWorkPart>> GetById(Guid plannedWorkId)
        {
            const string sql = @"
        SELECT ""PlannedWorkId"", ""PartId"", ""Quantity""
        FROM ""PlannedWorkParts""
        WHERE ""PlannedWorkId"" = {0}";

            var entities = await _db.PlannedWorkParts
                                    .FromSqlRaw(sql, plannedWorkId)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new List<PlannedWorkPart>(entities.Count);
            foreach (var e in entities)
            {
                var (model, error) = PlannedWorkPart.Create(e.PlannedWorkId, e.PartId, e.Quantity);
                if (!string.IsNullOrEmpty(error))
                    throw new InvalidOperationException(error);
                result.Add(model);
            }

            return result;
        }


        public async Task<List<PlannedWorkPartWithDetails>> GetByIdDetailed(Guid plannedWorkId)
        {
            const string sql = @"  
        SELECT p.""Id"" AS PartId, p.""Name"", p.""Article"", p.""Cost"", pw.""Quantity"", pb.""Name"" AS BrandName  
        FROM ""PlannedWorkParts"" pw  
        INNER JOIN ""Parts"" p ON p.""Id"" = pw.""PartId""  
        INNER JOIN ""PartBrands"" pb ON pb.""Id"" = p.""PartBrandId""  
        WHERE pw.""PlannedWorkId"" = @plannedWorkId";

            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("@plannedWorkId", plannedWorkId));
                await _db.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var result = new List<PlannedWorkPartWithDetails>();

                    while (await reader.ReadAsync())
                    {
                        var partId = reader.GetGuid(reader.GetOrdinal("PartId"));
                        var name = reader.GetString(reader.GetOrdinal("Name"));
                        var article = reader.GetString(reader.GetOrdinal("Article"));
                        var cost = reader.GetDecimal(reader.GetOrdinal("Cost"));
                        var quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                        var brandName = reader.GetString(reader.GetOrdinal("BrandName"));

                        var (model, error) = PlannedWorkPartWithDetails.Create(partId, name, article, cost, quantity, brandName);
                        if (!string.IsNullOrEmpty(error))
                            throw new InvalidOperationException(error);

                        result.Add(model);
                    }

                    return result;
                }
            }
        }






        public async Task Create(PlannedWorkPart model)
        {
            const string sql = @"
        INSERT INTO ""PlannedWorkParts"" (""PlannedWorkId"", ""PartId"", ""Quantity"")
        VALUES ({0}, {1}, {2})";

            await _db.Database.ExecuteSqlRawAsync(sql, model.PlannedWorkId, model.PartId, model.Quantity);
        }


        public async Task Delete(Guid plannedWorkId, Guid partId)
        {
            const string sql = @"
        DELETE FROM ""PlannedWorkParts""
        WHERE ""PlannedWorkId"" = {0}
        AND ""PartId"" = {1}";

            var rows = await _db.Database.ExecuteSqlRawAsync(sql, plannedWorkId, partId);

            if (rows == 0)
                throw new KeyNotFoundException(
                    $"PlannedWorkPart ({plannedWorkId}, {partId}) not found");
        }

    }
}
