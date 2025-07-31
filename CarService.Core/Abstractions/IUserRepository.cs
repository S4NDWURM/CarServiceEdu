using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User> GetByEmail(string email);
        Task<User> GetByUserId(Guid userId);
    }
}