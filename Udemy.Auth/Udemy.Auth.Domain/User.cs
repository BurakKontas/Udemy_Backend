using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Domain;

public class User : IdentityUser
{
    public string? Initials { get; set; }

    [ProtectedPersonalData]
    [PersonalData]
    public override string? Email { get; set; }
}