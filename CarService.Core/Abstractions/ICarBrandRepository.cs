using CarService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarService.DataAccess.Repositories
{
    public interface ICarBrandRepository
    {
        Task<List<CarBrand>> Get();
        Task<Guid> Create(CarBrand item);
        Task<Guid> Update(Guid id, string name);
        Task<Guid> Delete(Guid id);
        Task<CarBrand> GetById(Guid id);
    }
}