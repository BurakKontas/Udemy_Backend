using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Domain;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}