using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Domain;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public string? Initials { get; set; }
}