using CarService.Core.Models;

namespace CarService.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}