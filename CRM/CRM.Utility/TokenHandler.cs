using System.Security.Claims;
using CRM.Utility.IUtility;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Utility;

public class TokenHandler(IOptions<TokenConfiguration>tokenConfig):ITokenHandler
{
    private readonly TokenConfiguration _tokenConfig = tokenConfig.Value;

    public string GenerateToken(List<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
        var tokenValidityPeriod = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_tokenConfig.TokenExpiry));

        var token = new JwtSecurityToken(
            issuer:_tokenConfig.Issuer,
            audience:_tokenConfig.Audience,
            claims: claims,
            expires:tokenValidityPeriod,
            signingCredentials:signingCredentials
        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public int GenerateRefreshTokenExpiryData()
    {
        throw new NotImplementedException();
    }

    public int GetMaxRefreshTokenAttempts()
    {
        throw new NotImplementedException();
    }
}