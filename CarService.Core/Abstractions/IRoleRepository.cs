using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> Create(Guid id, string name);
        Task<IReadOnlyCollection<Role>> GetAll();
        Task<Role> GetById(Guid id);
        Task<Role> GetByName(string name);
    }
}