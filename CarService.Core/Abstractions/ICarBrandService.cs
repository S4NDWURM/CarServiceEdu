using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface ICarBrandService
    {
        Task<Guid> CreateCarBrand(CarBrand model);
        Task<Guid> DeleteCarBrand(Guid id);
        Task<List<CarBrand>> GetAllCarBrands();
        Task<Guid> UpdateCarBrand(Guid id, string name);
        Task<CarBrand> GetCarBrandById(Guid id);
    }
}