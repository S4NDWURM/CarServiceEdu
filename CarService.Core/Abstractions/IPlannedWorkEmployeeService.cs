using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IPlannedWorkEmployeeService
    {
        Task CreatePlannedWorkEmployee(PlannedWorkEmployee model);
        Task DeletePlannedWorkEmployee(Guid plannedWorkId, Guid employeeId);
        Task<List<PlannedWorkEmployee>> GetAllPlannedWorkEmployees();
        Task<List<PlannedWorkEmployee>> GetPlannedWorkEmployeesById(Guid plannedWorkId);
        Task<List<PlannedWorkEmployeeWithDetails>> GetPlannedWorkEmployeesWithDetailsById(Guid plannedWorkId);
    }
}