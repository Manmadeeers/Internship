using System.Security.Claims;

namespace CRM.Utility.IUtility;

public interface ITokenHandler
{
    string GenerateToken(List<Claim> claims);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    string GenerateRefreshToken();
    int GenerateRefreshTokenExpiryDays();
    int GetMaxRefreshTokenAttempts();
}