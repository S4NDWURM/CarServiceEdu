using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IEmployeeSpecializationService
    {
        Task CreateEmployeeSpecialization(EmployeeSpecialization model);
        Task DeleteEmployeeSpecialization(Guid employeeId, Guid specializationId);
        Task<List<EmployeeSpecialization>> GetAllEmployeeSpecializations();
        Task<EmployeeSpecialization> GetEmployeeSpecializationById(Guid employeeId);
        Task<List<EmployeeSpecializationWithDetails>> GetEmployeeSpecializationByIdDetailed(Guid employeeId);
    }
}