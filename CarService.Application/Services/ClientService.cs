using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        public ClientService(IClientRepository repo) => _repo = repo;

        public async Task<List<Client>> GetAllClients() =>
            await _repo.Get();

        public async Task<Client> GetClientById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Guid> CreateClient(Client model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateClient(Guid id, string lastName, string firstName, string middleName,
            DateTime dateOfBirth, DateTime registrationDate) =>
            await _repo.Update(id, lastName, firstName, middleName, dateOfBirth, registrationDate);

        public async Task<Guid> DeleteClient(Guid id) =>
            await _repo.Delete(id);
    }
}
