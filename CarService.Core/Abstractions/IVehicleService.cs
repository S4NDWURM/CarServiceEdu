using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IVehicleService
    {
        Task<Guid> CreateVehicle(Vehicle model);
        Task<Guid> DeleteVehicle(Guid id);
        Task<List<Vehicle>> GetAllVehicles();
        Task<Guid> UpdateVehicle(Guid id, string vin, int year, Guid generationId);
        Task<Vehicle> GetVehicleById(Guid id);
        Task<List<Vehicle>> GetVehiclesByVIN(string vin);
    }
}