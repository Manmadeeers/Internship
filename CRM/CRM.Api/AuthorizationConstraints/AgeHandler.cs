using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CRM.Api.AuthorizationConstraints;

public class AgeHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        var yearClaim = context.User.FindFirst(c=>c.Type==ClaimTypes.DateOfBirth);
        if (yearClaim is not null)
        {
            if (int.TryParse(yearClaim.Value, out var age))
            {
                if ((DateTime.Now.Year - age) >= requirement.Age)
                {
                    context.Succeed(requirement);
                }
            }
        }
        
        return Task.CompletedTask;
    }    
}
