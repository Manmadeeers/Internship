using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CRM.Api.AuthorizationConstraints;

public class AgeHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        var yearClaim = context.User.FindFirst(c=>c.Type==ClaimTypes.DateOfBirth);
        if (yearClaim is null)
        {
            return Task.CompletedTask;
        }

        if (!DateTime.TryParseExact(yearClaim.Value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dateOfBitrh))
        {
            return Task.CompletedTask;
        }

        var oldestAllowedBirthDate = DateTime.UtcNow.Date.AddYears(-requirement.MinimumAge);
        if (dateOfBitrh.Date <= oldestAllowedBirthDate)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }    
}
