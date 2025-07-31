using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IPartService
    {
        Task<Guid> CreatePart(Part model);
        Task<Guid> DeletePart(Guid id);
        Task<List<Part>> GetAllParts();
        Task<Part> GetPartById(Guid id);
        Task<List<Part>> SearchPartsByName(string name);
        Task<Guid> UpdatePart(Guid id, string name, string article, decimal cost, Guid partBrandId);
    }
}