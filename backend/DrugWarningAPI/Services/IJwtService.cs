using MedicineWarningAPI.Models;

namespace MedicineWarningAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(Admin admin, out DateTime expiresAt);
    }
}
