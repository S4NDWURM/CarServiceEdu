using CarService.Core.Models;

namespace CarService.Application.Services
{
    public interface IExcelGenerationService
    {
        Task<MemoryStream> GenerateOrderRequestExcel(Guid requestId, List<Part> parts, List<Work> works, List<int> partQuantities);
    }
}