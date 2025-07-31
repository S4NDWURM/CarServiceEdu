using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface ICarModelRepository
    {
        Task<List<CarModel>> Get();
        Task<Guid> Create(CarModel item);
        Task<Guid> Update(Guid id, string name, Guid carBrandId);
        Task<Guid> Delete(Guid id);
        Task<CarModel> GetById(Guid id);
        Task<List<CarModel>> GetByCarBrandId(Guid carBrandId);
    }
}