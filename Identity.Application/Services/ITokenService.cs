using System.Security.Claims;

namespace Identity.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims, string secretKey);
    }
}