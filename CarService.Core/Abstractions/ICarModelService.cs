using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface ICarModelService
    {
        Task<Guid> CreateCarModel(CarModel model);
        Task<Guid> DeleteCarModel(Guid id);
        Task<List<CarModel>> GetAllCarModels();
        Task<Guid> UpdateCarModel(Guid id, string name, Guid carBrandId);
        Task<CarModel> GetCarModelById(Guid id);
        Task<List<CarModel>> GetCarModelsByCarBrandId(Guid carBrandId);
    }
}