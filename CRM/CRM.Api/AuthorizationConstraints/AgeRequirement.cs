using Microsoft.AspNetCore.Authorization;

namespace CRM.Api.AuthorizationConstraints;

public class AgeRequirement(int age) : IAuthorizationRequirement
{
    protected internal int Age { get; set; } = age;
}