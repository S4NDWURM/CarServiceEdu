using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IDiagnosticsRepository
    {
        Task<Guid> Create(Diagnostics model);
        Task<Guid> Delete(Guid id);
        Task<List<Diagnostics>> Get();
        Task<Diagnostics> GetById(Guid id);
        Task<List<Diagnostics>> GetByRequestId(Guid requestId);
        Task<Guid> Update(Guid id, DateTime diagnosticsDate, string resultDescription, Guid employeeId, Guid requestId);
    }
}