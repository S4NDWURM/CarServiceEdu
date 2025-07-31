using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarService.Core.Models;
using CarService.DataAccess.Repositories;

namespace CarService.Application.Services
{
    public class DiagnosticsService : IDiagnosticsService
    {
        private readonly IDiagnosticsRepository _repo;
        public DiagnosticsService(IDiagnosticsRepository repo) => _repo = repo;

        public async Task<List<Diagnostics>> GetAllDiagnosticss() =>
            await _repo.Get();

        public async Task<Diagnostics> GetDiagnosticsById(Guid id) =>
            await _repo.GetById(id);

        public async Task<List<Diagnostics>> GetDiagnosticsByRequestId(Guid requestId) =>
            await _repo.GetByRequestId(requestId);


        public async Task<Guid> CreateDiagnostics(Diagnostics model) =>
            await _repo.Create(model);

        public async Task<Guid> UpdateDiagnostics(Guid id, DateTime diagnosticsDate, string resultDescription, Guid employeeId, Guid requestId) =>
            await _repo.Update(id, diagnosticsDate, resultDescription, employeeId, requestId);

        public async Task<Guid> DeleteDiagnostics(Guid id) =>
            await _repo.Delete(id);
    }
}
