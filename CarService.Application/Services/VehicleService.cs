using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repo;
        public VehicleService(IVehicleRepository repo) => _repo = repo;

        public async Task<List<Vehicle>> GetAllVehicles() =>
            await _repo.Get();

        public async Task<Vehicle> GetVehicleById(Guid id)
        {
            return await _repo.GetById(id);
        }
        public async Task<List<Vehicle>> GetVehiclesByVIN(string vin) =>
            await _repo.GetByVIN(vin);

        public async Task<Guid> CreateVehicle(Vehicle model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateVehicle(Guid id, string vin, int year, Guid generationId) =>
            await _repo.Update(id, vin, year, generationId);

        public async Task<Guid> DeleteVehicle(Guid id) =>
            await _repo.Delete(id);
    }
}