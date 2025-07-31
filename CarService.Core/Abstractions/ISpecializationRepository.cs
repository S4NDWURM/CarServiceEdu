using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface ISpecializationRepository
    {
        Task<Guid> Create(Specialization model);
        Task<Guid> Delete(Guid id);
        Task<List<Specialization>> Get();
        Task<Specialization> GetById(Guid id);
        Task<Guid> Update(Guid id, string name);
    }
}