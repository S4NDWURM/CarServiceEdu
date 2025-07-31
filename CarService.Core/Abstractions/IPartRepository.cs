using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IPartRepository
    {
        Task<Guid> Create(Part model);
        Task<Guid> Delete(Guid id);
        Task<List<Part>> Get();
        Task<Part> GetById(Guid id);
        Task<List<Part>> SearchByName(string name);
        Task<Guid> Update(Guid id, string name, string article, decimal cost, Guid partBrandId);
    }
}