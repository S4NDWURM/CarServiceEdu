using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly CarServiceDbContext _ctx;
        public VehicleRepository(CarServiceDbContext ctx) => _ctx = ctx;

        public async Task<List<Vehicle>> Get() =>
            (await _ctx.Vehicles.AsNoTracking().ToListAsync())
             .Select(e =>
             {
                 var (model, error) = Vehicle.Create(e.Id, e.VIN, e.Year, e.GenerationId);
                 if (model == null)
                 {
                     throw new InvalidOperationException(error);
                 }
                 if (!string.IsNullOrEmpty(error))
                     throw new InvalidOperationException(error);
                 return model;
             })
             .ToList();

        public async Task<Vehicle> GetById(Guid id)
        {
            var entity = await _ctx.Vehicles
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(v => v.Id == id);

            if (entity == null)
                return null;

            var (model, error) = Vehicle.Create(entity.Id, entity.VIN, entity.Year, entity.GenerationId);
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);

            return model;
        }

        public async Task<List<Vehicle>> GetByVIN(string vin) =>
        (await _ctx.Vehicles
            .AsNoTracking()
            .Where(v => v.VIN.Contains(vin))
            .ToListAsync())
        .Select(e =>
        {
            var (model, error) = Vehicle.Create(e.Id, e.VIN, e.Year, e.GenerationId);
            if (model == null)
            {
                throw new InvalidOperationException(error);
            }
            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException(error);
            return model;
        })
        .ToList();

        public async Task<Guid> Create(Vehicle model)
        {
            var e = new VehicleEntity
            {
                Id = model.Id,
                VIN = model.VIN,
                Year = model.Year,
                GenerationId = model.GenerationId
            };
            await _ctx.Vehicles.AddAsync(e);
            await _ctx.SaveChangesAsync();
            return e.Id;
        }

        public async Task<Guid> Update(Guid id, string vIN, int year, Guid generationId)
        {
            var e = await _ctx.Vehicles.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Vehicle with id {id} not found");
            e.VIN = vIN;
            e.Year = year;
            e.GenerationId = generationId;
            await _ctx.SaveChangesAsync();
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var e = await _ctx.Vehicles.FindAsync(id);
            if (e == null)
                throw new KeyNotFoundException($"Vehicle with id {id} not found");
            _ctx.Vehicles.Remove(e);
            await _ctx.SaveChangesAsync();
            return id;
        }
    }
}