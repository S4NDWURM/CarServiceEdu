using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IRequestRepository
    {
        Task<Guid> Create(UserRequest model);
        Task<Guid> Delete(Guid id);
        Task<List<UserRequest>> Get();
        Task<UserRequest> GetById(Guid id);
        Task<UserRequestWithDetailsModel> GetByIdDetailed(Guid id);
        Task<List<UserRequest>> GetRequestsByClientId(Guid clientId);
        Task<Guid> Update(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId);
        Task<Guid> UpdateStatus(Guid id, Guid statusId);
    }
}