using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IRequestService
    {
        Task<Guid> CreateRequest(UserRequest model);
        Task<Guid> DeleteRequest(Guid id);
        Task<List<UserRequest>> GetAllRequests();
        Task<UserRequest> GetRequestById(Guid id);
        Task<UserRequestWithDetailsModel> GetRequestByIdDetailed(Guid id);
        Task<List<UserRequest>> GetRequestsByClientId(Guid clientId);
        Task<Guid> UpdateRequest(Guid id, string reason, DateTime openDate, DateTime? closeDate, Guid clientId, Guid vehicleId, Guid statusId);
        Task<Guid> UpdateRequestStatus(Guid id, Guid statusId);
    }
}