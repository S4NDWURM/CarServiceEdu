using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IStatusRepository
    {
        Task<Guid> Create(Status model);
        Task<Guid> Delete(Guid id);
        Task<List<Status>> Get();
        Task<Status> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}