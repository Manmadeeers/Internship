using System.Security.Claims;

namespace CRM.Utility.IUtility;

public interface ITokenHandler
{
    string GenerateToken(List<Claim> claims);
}