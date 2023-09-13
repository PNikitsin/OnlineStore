using System.Security.Claims;

namespace Identity.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims, string secretKey);
    }
}