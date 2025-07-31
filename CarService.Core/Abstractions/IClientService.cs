using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IClientService
    {
        Task<Guid> CreateClient(Client model);
        Task<Guid> DeleteClient(Guid id);
        Task<List<Client>> GetAllClients();
        Task<Client> GetClientById(Guid id);
        Task<Guid> UpdateClient(Guid id, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate);
    }
}