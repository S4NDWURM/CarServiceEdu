using CarService.Core.Models;

namespace CarService.DataAccess.Repositories
{
    public interface IEmployeeSpecializationRepository
    {
        Task Create(EmployeeSpecialization model);
        Task Delete(Guid employeeId, Guid specializationId);
        Task<List<EmployeeSpecialization>> Get();
        Task<EmployeeSpecialization> GetById(Guid employeeId);
        Task<List<EmployeeSpecializationWithDetails>> GetByIdDetailed(Guid employeeId);
    }
}