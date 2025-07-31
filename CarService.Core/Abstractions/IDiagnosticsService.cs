using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IDiagnosticsService
    {
        Task<Guid> CreateDiagnostics(Diagnostics model);
        Task<Guid> DeleteDiagnostics(Guid id);
        Task<List<Diagnostics>> GetAllDiagnosticss();
        Task<Diagnostics> GetDiagnosticsById(Guid id);
        Task<List<Diagnostics>> GetDiagnosticsByRequestId(Guid requestId);
        Task<Guid> UpdateDiagnostics(Guid id, DateTime diagnosticsDate, string resultDescription, Guid employeeId, Guid requestId);
    }
}