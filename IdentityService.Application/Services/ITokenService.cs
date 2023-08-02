using System.Security.Claims;

namespace IdentityService.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims, string secretKey);
    }
}