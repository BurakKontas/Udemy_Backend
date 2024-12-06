using Microsoft.AspNetCore.Identity;
using Udemy.Auth.Domain.Interfaces;

namespace Udemy.Auth.Domain.Entities;

public class User : IdentityUser, IDeactivatable
{
    public string? Initials { get; set; }

    [ProtectedPersonalData]
    [PersonalData]
    public override required string? Email { get; set; }

    [ProtectedPersonalData]
    [PersonalData]
    public override required string? UserName { get; set; }

    public bool IsDeactivated { get; private set; } = false;

    public bool Deactivate => IsDeactivated = true!;
    public bool Activate => IsDeactivated = false!;
}