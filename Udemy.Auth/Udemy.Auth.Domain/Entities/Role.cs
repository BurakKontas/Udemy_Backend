using Microsoft.AspNetCore.Identity;

namespace Udemy.Auth.Domain.Entities;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}