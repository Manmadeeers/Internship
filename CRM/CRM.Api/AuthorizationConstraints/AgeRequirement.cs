using Microsoft.AspNetCore.Authorization;

namespace CRM.Api.AuthorizationConstraints;

public class AgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    protected internal int MinimumAge { get; } = minimumAge;
}