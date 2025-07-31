using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> Get();
        Task<Guid> Create(Client item);
        Task<Guid> Update(Guid id, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate);
        Task<Guid> Delete(Guid id);
        Task<Client> GetById(Guid id);
    }
}