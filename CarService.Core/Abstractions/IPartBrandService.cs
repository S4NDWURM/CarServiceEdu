using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IPartBrandService
    {
        Task<Guid> CreatePartBrand(PartBrand model);
        Task<Guid> DeletePartBrand(Guid id);
        Task<List<PartBrand>> GetAllPartBrands();
        Task<PartBrand> GetPartBrandById(Guid id);
        Task<Guid> UpdatePartBrand(Guid id, string name);
    }
}