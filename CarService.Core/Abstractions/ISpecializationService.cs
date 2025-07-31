using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface ISpecializationService
    {
        Task<Guid> CreateSpecialization(Specialization model);
        Task<Guid> DeleteSpecialization(Guid id);
        Task<List<Specialization>> GetAllSpecializations();
        Task<Specialization> GetSpecializationById(Guid id);
        Task<Guid> UpdateSpecialization(Guid id, string name);
    }
}