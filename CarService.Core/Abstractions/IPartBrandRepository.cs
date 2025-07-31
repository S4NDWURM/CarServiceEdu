using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IPartBrandRepository
    {
        Task<Guid> Create(PartBrand model);
        Task<Guid> Delete(Guid id);
        Task<List<PartBrand>> Get();
        Task<PartBrand> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}