using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IWorkRepository
    {
        Task<Guid> Create(Work model);
        Task<Guid> Delete(Guid id);
        Task<List<Work>> Get();
        Task<Work> GetById(Guid id);
        Task<Guid> Update(Guid id, string name, string description, decimal cost);
    }
}