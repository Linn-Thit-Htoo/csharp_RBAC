using csharp_RBAC.Models;

namespace csharp_RBAC.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(JwtRequestModel requestModel);
        string GenerateRefreshToken(JwtRequestModel requestModel);
    }
}
