using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IRoleService
    {
        Task<IReadOnlyCollection<Role>> GetAll();
        Task<Role> GetByName(string name);
    }
}