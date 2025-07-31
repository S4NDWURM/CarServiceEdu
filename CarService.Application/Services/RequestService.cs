using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;


namespace CarService.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _repo;
        public RequestService(IRequestRepository repo) => _repo = repo;

        public async Task<List<UserRequest>> GetAllRequests() =>
            await _repo.Get();
        public async Task<UserRequest> GetRequestById(Guid id)
        {
            return await _repo.GetById(id);
        }

        public async Task<UserRequestWithDetailsModel> GetRequestByIdDetailed(Guid id)
        {
            return await _repo.GetByIdDetailed(id);
        }

        public async Task<List<UserRequest>> GetRequestsByClientId(Guid clientId) =>
            await _repo.GetRequestsByClientId(clientId);

        public async Task<Guid> CreateRequest(UserRequest model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateRequestStatus(Guid id, Guid statusId)
        {
            var updated = await _repo.UpdateStatus(id, statusId);
            return updated;
        }


        public async Task<Guid> UpdateRequest(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId) =>
            await _repo.Update(id, reason, openDate, closeDate, clientId, vehicleId, statusId);

        public async Task<Guid> DeleteRequest(Guid id) =>
            await _repo.Delete(id);
    }
}