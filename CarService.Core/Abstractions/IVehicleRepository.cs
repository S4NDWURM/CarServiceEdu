using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IVehicleRepository
    {
        Task<Guid> Create(Vehicle model);
        Task<Guid> Delete(Guid id);
        Task<List<Vehicle>> Get();
        Task<Guid> Update(Guid id, string vIN, int year, Guid generationId);
        Task<Vehicle> GetById(Guid id);
        Task<List<Vehicle>> GetByVIN(string vin);
    }
}